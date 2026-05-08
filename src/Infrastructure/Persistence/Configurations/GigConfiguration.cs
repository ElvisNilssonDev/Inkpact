using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations
{
    public class GigConfiguration : IEntityTypeConfiguration<Gig>
    {
        public void Configure(EntityTypeBuilder<Gig> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(g => g.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(g => g.Budget)
                .HasColumnType("decimal(18,2)");

            builder.Property(g => g.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(g => g.Tags)
                .HasMaxLength(200);

            // 1-to-many: User (Client) → Gigs
            builder.HasOne(g => g.Client)
                .WithMany(u => u.PostedGigs)
                .HasForeignKey(g => g.ClientId)
                .OnDelete(DeleteBehavior.Restrict);   // can't delete a user with active gigs

            // 1-to-many: Gig → Proposals
            builder.HasMany(g => g.Proposals)
                .WithOne(p => p.Gig)
                .HasForeignKey(p => p.GigId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(g => g.Status);
            builder.HasIndex(g => g.ClientId);
        }
    }
}
