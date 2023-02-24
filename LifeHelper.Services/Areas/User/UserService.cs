using AutoMapper;
using AutoMapper.QueryableExtensions;
using LifeHelper.Infrastructure;
using LifeHelper.Infrastructure.Exceptions;
using LifeHelper.Services.Areas.User.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LifeHelper.Services.Areas.User;

using Infrastructure.Entities;

public class UserService : IUserService
{
    private readonly LifeHelperDbContext _dbContext;
    private readonly IMapper _mapper;

    public UserService(LifeHelperDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<ICollection<UserDto>> GetAllUsersAsync()
    {
        return await _dbContext.Users.ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await _dbContext.Users.ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(u => u.Id == id);
        
        if (user is null)
        {
            throw new NotFoundException($"User with id - {id} not found");
        }

        return user;
    }

    public async Task<UserDto> GetByNicknameAsync(string nickname)
    {
        var user = await _dbContext.Users.ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(u => u.Nickname.ToLower() == nickname.ToLower());
        
        if (user is null)
        {
            throw new NotFoundException($"User with Nickname - {nickname} not found");
        }

        return user;
    }

    public async Task<int> CreateAsync(UserInputDto userInputDto)
    {
        await IsEmailAvailableAsync(userInputDto.Email);
        
        userInputDto = PasswordHasher(userInputDto);

        var user = _mapper.Map<User>(userInputDto);

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return user.Id;
    }

    public async Task<int> UpdateAsync(int id, UserInputDto userInputDto)
    {
        var user = await _dbContext.Users
            .Include(u => u.TaskItems)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
        {
            throw new NotFoundException($"User with id - {id} not found");
        }

        await IsEmailAvailableAsync(userInputDto.Email);

        
        if (VerifyHashedPassword(user, userInputDto.Password))
        {
            throw new BadRequestException($"The password should not be the same");
        }
        else
        {
            userInputDto = PasswordHasher(userInputDto);
        }
        
        _mapper.Map(userInputDto, user);

        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();

        return user.Id;

    }

    public async Task DeleteAsync(int id)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
        {
            throw new NotFoundException($"User with id - {id} not found");
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    private async Task IsEmailAvailableAsync(string email)
    {
        var user = await FindByEmailAsync(email);
        
        if (user is not null)
        {
            throw new BadRequestException("The entered mail is already being used");
        }
        
    }

    private async Task<UserDto?> FindByEmailAsync(string email)
    {
        return await _dbContext.Users.ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    private UserInputDto PasswordHasher(UserInputDto userInputDto)
    {
        var passwordHasher = new PasswordHasher<UserInputDto>();

        userInputDto.Password = passwordHasher.HashPassword(userInputDto, userInputDto.Password);
        
        return userInputDto;
    }

    public bool VerifyHashedPassword(User user, string password)
    {
        var passwordHasher = new PasswordHasher<User>();
        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

        if (result == PasswordVerificationResult.Success)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}