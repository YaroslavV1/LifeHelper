using AutoMapper;
using AutoMapper.QueryableExtensions;
using LifeHelper.Infrastructure;
using LifeHelper.Infrastructure.Entities;
using LifeHelper.Infrastructure.Exceptions;
using LifeHelper.Services.Areas.Categories;
using LifeHelper.Services.Areas.Helpers.Jwt;
using LifeHelper.Services.Areas.Helpers.Jwt.DTOs;
using LifeHelper.Services.Areas.Users.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BadRequestException = LifeHelper.Infrastructure.Exceptions.BadRequestException;

namespace LifeHelper.Services.Areas.Users;

public class UserService : IUserService
{
    private readonly LifeHelperDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ICategoryService _categoryService;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly TokenInfoDto _currentUserInfo;

    public UserService(
        LifeHelperDbContext dbContext,
        IMapper mapper,
        IHttpContextAccessor contextAccessor,
        IClaimParserService claimParserService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _passwordHasher = new PasswordHasher<User>();
        _currentUserInfo = claimParserService.ParseInfoFromClaims(contextAccessor.HttpContext);
    }
    
    public async Task<IList<UserDto>> GetListAsync()
    {
        return await _dbContext.Users.ProjectTo<UserDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await _dbContext.Users
                       .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                       .FirstOrDefaultAsync(user => user.Id == id)
                   ?? throw new NotFoundException($"User with Id: {id} not found");
        
        CheckPermissionAccess(user.Id);

        return user;
    }
    
    public async Task<UserDto> GetByLoginAsync(UserLoginDto loginDto)
    {
        var loggedInUser = await _dbContext.Users
                   .Include(user => user.Roles)
                   .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                   .FirstOrDefaultAsync(user => 
                       user.Nickname.ToLower() == loginDto.Login.ToLower() ||
                       user.Email.ToLower() == loginDto.Login.ToLower()) 
               ?? throw new NotFoundException($"User with Login: {loginDto.Login} not found");
        
        if (!await VerifyHashedPasswordAsync(loggedInUser.Id, loginDto.Password))
        {
            throw new BadRequestException("Passwords don't match");
        }

        return loggedInUser;
    }

    public async Task<UserDto> CreateAsync(UserInputDto userInputDto)
    {
        var user = _mapper.Map<User>(userInputDto);

        await ThrowIfEmailIsNotAvailableAsync(userInputDto.Email, user.Id);
            
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

        CheckPermissionAccess(user.Id);
        
        await ThrowIfEmailIsNotAvailableAsync(userInputDto.Email, user.Id);

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
        
        CheckPermissionAccess(user.Id);
        
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    private void CheckPermissionAccess(int userId)
    {
        if (!(_currentUserInfo.Role == "Admin" || userId == _currentUserInfo.Id))
        {
            throw new BadRequestException("You do not have permission to access this user");
        }
    }
    
    private async Task ThrowIfEmailIsNotAvailableAsync(string email, int id)
    {
        var isEmailExists = await _dbContext.Users
            .AnyAsync(user => user.Email.ToLower() == email.ToLower() && user.Id != id);

        if (isEmailExists)
        {
            throw new BadRequestException("The user with this mail is already exists");
        }
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