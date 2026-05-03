using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations
{
    public class ProposalConfiguration : IEntityTypeConfiguration<Proposal>
    {
        public void Configure(EntityTypeBuilder<Proposal> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.CoverLetter)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(p => p.ProposedRate)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Status)
                .IsRequired()
                .HasConversion<string>();

            // 1-to-many: User (Freelancer) → Proposals which is pretty neat in my opinion :D
            builder.HasOne(p => p.Freelancer)
                .WithMany(u => u.Proposals)
                .HasForeignKey(p => p.FreelancerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.GigId);
            builder.HasIndex(p => p.FreelancerId);
            builder.HasIndex(p => p.Status);
        }
    }
}
