﻿using LifeHelper.Infrastructure.Entities;
using LifeHelper.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;

namespace LifeHelper.Infrastructure;

public class LifeHelperDbContext : DbContext
{
    private readonly IRoleSeeder _roleSeeder;
    
    public DbSet<User> Users { get; set; }
    public DbSet<UserMoney> UserMonies { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<SubNote> SubNotes { get; set; }
    public DbSet<ArchiveNote> ArchiveNotes { get; set; }
    public DbSet<ArchiveSubNote> ArchiveSubNotes { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Expense> Expenses { get; set; }

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