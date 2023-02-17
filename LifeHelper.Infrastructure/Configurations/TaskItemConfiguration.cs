using LifeHelper.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeHelper.Infrastructure.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.HasOne(t => t.User)
            .WithMany(u => u.TaskItems)
            .HasForeignKey(t => t.UserId);
    }
}