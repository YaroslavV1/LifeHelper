using LifeHelper.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeHelper.Infrastructure.Configurations;

public class SubtaskItemConfiguration : IEntityTypeConfiguration<SubtaskItem>
{
    public void Configure(EntityTypeBuilder<SubtaskItem> builder)
    {
        builder.HasOne(sb => sb.TaskItem)
            .WithMany(t => t.SubtaskItems)
            .HasForeignKey(sb => sb.TaskItemId);
    }
}