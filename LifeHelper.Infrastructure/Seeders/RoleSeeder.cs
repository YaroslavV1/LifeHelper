using LifeHelper.Infrastructure.Entities;
using LifeHelper.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;

namespace LifeHelper.Infrastructure.Seeders;

public class RoleSeeder : IRoleSeeder
{
    public void Seed(ModelBuilder builder)
    {
        var roles = ((RoleName[])Enum.GetValues(typeof(RoleName)))
            .Select(role => new Role
            {
                Id = (int)role,
                RoleName = role,
                NormalName = role.ToString().Normalize()
            });
        builder.Entity<Role>().HasData(roles);
    }
}