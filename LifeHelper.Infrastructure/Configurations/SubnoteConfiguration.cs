﻿using LifeHelper.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeHelper.Infrastructure.Configurations;

public class SubnoteConfiguration : IEntityTypeConfiguration<SubNote>
{
    public void Configure(EntityTypeBuilder<SubNote> builder)
    {
        builder.HasOne(sb => sb.Note)
            .WithMany(t => t.SubNotes)
            .HasForeignKey(sb => sb.NoteId);
    }
}