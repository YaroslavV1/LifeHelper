using LifeHelper.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeHelper.Infrastructure.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(money => money.MoneyLimit)
            .HasColumnType("decimal(12, 2)") 
            .HasPrecision(12, 2);
    }
}