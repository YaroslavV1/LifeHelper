using AutoMapper;
using AutoMapper.QueryableExtensions;
using LifeHelper.Infrastructure;
using LifeHelper.Infrastructure.Exceptions;
using LifeHelper.Services.Areas.User.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BadRequestException = LifeHelper.Infrastructure.Exceptions.BadRequestException;

namespace LifeHelper.Services.Areas.User;

using Infrastructure.Entities;

public class UserService : IUserService
{
    private readonly LifeHelperDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserService(LifeHelperDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _passwordHasher = new PasswordHasher<User>();
    }
    
    public async Task<IList<UserDto>> GetListAsync()
    {
        return await _dbContext.Users.ProjectTo<UserDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        return await _dbContext.Users
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(user => user.Id == id) 
               ?? throw new NotFoundException($"User with Id: {id} not found");
    }
    
    public async Task<UserDto> GetByLoginAsync(UserLoginDto loginDto)
    {
        var loggedInUser = await _dbContext.Users
                   .Include(user => user.Roles)
                   .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync(user => 
                       user.Nickname.ToLower() == loginDto.Login.ToLower() || user.Email == loginDto.Login) 
               ?? throw new NotFoundException($"User with Login: {loginDto.Login} not found");
        
        if (!VerifyHashedPasswordAsync(loggedInUser.Id, loginDto.Password).Result)
        {
            throw new BadRequestException("Passwords don't match");
        }

        return loggedInUser;
    }

    public async Task<UserDto> CreateAsync(UserInputDto userInputDto)
    {
        if (!CheckIfEmailIsAvailableAsync(userInputDto.Email).Result)
        {
            throw new BadRequestException("A user with this mail already exists");
        }
        
        var user = _mapper.Map<User>(userInputDto);
        var userDto = _mapper.Map<UserDto>(user);
        
        user.PasswordHash = await HashPasswordAsync(user, userInputDto.Password);
        
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        await AddRoleToUser(user.Id, "User");

        return userDto;
    }

    public async Task<UserDto> UpdateByIdAsync(int id, UserInputDto userInputDto)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id)
                ?? throw new NotFoundException($"User with Id: {id} not found");

        if (!CheckIfEmailIsAvailableAsync(userInputDto.Email).Result)
        {
            throw new BadRequestException("The user with this mail is already exists");
        }
        
        userInputDto.Password = await HashPasswordAsync(user, userInputDto.Password);
        
        _mapper.Map(userInputDto, user);
        var userDto = _mapper.Map<UserDto>(user);
        
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();

        return userDto;
    }

    public async Task DeleteByIdAsync(int id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id)
                ?? throw new NotFoundException($"User with Id: {id} not found");
        
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<bool> CheckIfEmailIsAvailableAsync(string email)
    {
        return await _dbContext.Users
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(user => user.Email == email) is null;
    }
    
    public async Task<bool> VerifyHashedPasswordAsync(int userId, string password)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId)
            ?? throw new NotFoundException($"User with Id: {userId} not found");
        
        var result = 
            await Task.Run(() => _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password));
        
        return result == PasswordVerificationResult.Success;
    }

    private async Task AddRoleToUser(int userId, string roleName)
    {
        var user = await _dbContext.Users.Include(user => user.Roles).FirstOrDefaultAsync(user => user.Id == userId);
        var role = await _dbContext.Roles.FirstOrDefaultAsync(role => role.NormalName.ToLower() == roleName.ToLower());

        if (role != null && user != null)
        {
            user.Roles.Add(role);
            await _dbContext.SaveChangesAsync();
        }
    }
    private async Task<string> HashPasswordAsync(User user, string password)
    {
        return await Task.Run(() => _passwordHasher.HashPassword(user, password));
    }

    
}