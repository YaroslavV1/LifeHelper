using LifeHelper.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace LifeHelper.Infrastructure;

public class LifeHelperDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<TaskItem> TaskItems { get; set; } = null!;
    public DbSet<SubtaskItem> SubtaskItems { get; set; } = null!;

    public LifeHelperDbContext(DbContextOptions<LifeHelperDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LifeHelperDbContext).Assembly);
    }
}