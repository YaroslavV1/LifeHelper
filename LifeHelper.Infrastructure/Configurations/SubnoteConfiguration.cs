using LifeHelper.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeHelper.Infrastructure.Configurations;

public class SubnoteConfiguration : IEntityTypeConfiguration<Subnote>
{
    public void Configure(EntityTypeBuilder<Subnote> builder)
    {
        builder.HasOne(sb => sb.Note)
            .WithMany(t => t.Subnotes)
            .HasForeignKey(sb => sb.NoteId);
    }
}