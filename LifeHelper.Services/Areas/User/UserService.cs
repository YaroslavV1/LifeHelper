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

    public async Task<UserDto> GetByNicknameAsync(string nickname)
    {
        return await _dbContext.Users
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(user => user.Nickname.ToLower() == nickname.ToLower()) 
               ?? throw new NotFoundException($"User with Nickname: {nickname} not found");
    }

    public async Task<int> CreateAsync(UserInputDto userInputDto)
    {
        if (!CheckIfEmailIsAvailableAsync(userInputDto.Email).Result)
        {
            throw new BadRequestException("A user with this mail already exists");
        }
        
        var user = _mapper.Map<User>(userInputDto);
        
        user.PasswordHash = await HashPasswordAsync(user, userInputDto.Password);

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return user.Id;
    }

    public async Task<int> UpdateByIdAsync(int id, UserInputDto userInputDto)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id)
                ?? throw new NotFoundException($"User with Id: {id} not found");

        if (!CheckIfEmailIsAvailableAsync(userInputDto.Email).Result)
        {
            throw new BadRequestException("The user with this mail already exists");
        }
        
        userInputDto.Password = await HashPasswordAsync(user, userInputDto.Password);
        
        _mapper.Map(userInputDto, user);

        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();

        return user.Id;
    }

    public async Task DeleteByIdAsync(int id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id)
                ?? throw new NotFoundException($"User with Id: {id} not found");
        
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> CheckIfEmailIsAvailableAsync(string email)
    {
        return await _dbContext.Users
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(user => user.Email == email) is null;
    }
    
    public async Task<bool> VerifyHashedPasswordAsync(User user, string password)
    {
        var result = 
            await Task.Run(() => _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password));
        
        return result == PasswordVerificationResult.Success;
    }

    private async Task<string> HashPasswordAsync(User user, string password)
    {
        return await Task.Run(() => _passwordHasher.HashPassword(user, password));
    }

    
}