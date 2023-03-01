using LifeHelper.Infrastructure.Entities;
using LifeHelper.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;

namespace LifeHelper.Infrastructure;

public class LifeHelperDbContext : DbContext
{
    private readonly IRoleSeeder _roleSeeder;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<TaskItem> TaskItems { get; set; } = null!;
    public DbSet<SubtaskItem> SubtaskItems { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;

    public LifeHelperDbContext(DbContextOptions<LifeHelperDbContext> options, IRoleSeeder roleSeeder) : base(options)
    {
        _roleSeeder = roleSeeder;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LifeHelperDbContext).Assembly);
        _roleSeeder.Seed(modelBuilder);
    }
    
}