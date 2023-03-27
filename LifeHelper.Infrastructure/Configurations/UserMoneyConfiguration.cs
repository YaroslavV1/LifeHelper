using LifeHelper.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeHelper.Infrastructure.Configurations;

public class UserMoneyConfiguration : IEntityTypeConfiguration<UserMoney>
{
    public void Configure(EntityTypeBuilder<UserMoney> builder)
    {
        builder.Property(money => money.Money)
            .HasColumnType("decimal(12, 2)") 
            .HasPrecision(12, 2);
    }
}