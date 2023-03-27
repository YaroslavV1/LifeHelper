using System.Reflection;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using LifeHelper.Api.Middlewares;
using LifeHelper.Infrastructure;
using LifeHelper.Infrastructure.Seeders;
using LifeHelper.Services.Areas.Archive;
using LifeHelper.Services.Areas.Authentication;
using LifeHelper.Services.Areas.Categories;
using LifeHelper.Services.Areas.Helpers.Jwt;
using LifeHelper.Services.Areas.Notes;
using LifeHelper.Services.Areas.SubNotes;
using LifeHelper.Services.Areas.UserMonies;
using LifeHelper.Services.Areas.Users;
using LifeHelper.Services.Areas.Users.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "LifeHelper API",
        Description = "An ASP.NET Core Web Api for my LifeHelper application"
    });
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        In = ParameterLocation.Header,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT"
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[] {}
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("JwtSecurity:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("JwtSecurity:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSecurity:SecurityKey").Value!))
    };
});

builder.Services.AddDbContext<LifeHelperDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LifeHelperDatabase")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UserInputValidator>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRoleSeeder, RoleSeeder>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IClaimParserService, ClaimParserService>();
builder.Services.AddTransient<INoteService, NoteService>();
builder.Services.AddTransient<ISubNoteService, SubNoteService>();
builder.Services.AddTransient<IArchiveService, ArchiveService>();
builder.Services.AddTransient<IUserMoneyService, UserMoneyService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
}

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();