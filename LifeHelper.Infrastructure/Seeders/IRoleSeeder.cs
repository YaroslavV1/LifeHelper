using Microsoft.EntityFrameworkCore;

namespace LifeHelper.Infrastructure.Seeders;

public interface IRoleSeeder
{
    public void Seed(ModelBuilder builder);
}