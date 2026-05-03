using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations
{
    public class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.AgreedRate)
                .HasColumnType("decimal(18,2)");

            builder.Property(c => c.TerminationReason)
            .HasMaxLength(500);

            builder.HasOne(c => c.Gig)
            .WithMany()
            .HasForeignKey(c => c.GigId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.HasOne(c => c.Client)
                .WithMany()
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Freelancer)
                .WithMany()
                .HasForeignKey(c => c.FreelancerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Milestones)
                .WithOne(m => m.Contract)
                .HasForeignKey(m => m.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Invoices)
                .WithOne(i => i.Contract)
                .HasForeignKey(i => i.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Reviews)
                .WithOne()
                .HasForeignKey(r => r.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(c => c.ClientId);
            builder.HasIndex(c => c.FreelancerId);
            builder.HasIndex(c => c.Status);
        }
    }
}
