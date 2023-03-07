using LifeHelper.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeHelper.Infrastructure.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasOne(t => t.User)
            .WithMany(u => u.Notes)
            .HasForeignKey(t => t.UserId);
    }
}