using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations
{
    public class MilestoneConfiguration : IEntityTypeConfiguration<Milestone>
    {
        public void Configure(EntityTypeBuilder<Milestone> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(m => m.Amount)
                .HasColumnType("decimal(18,2)");

            builder.Property(m => m.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.HasIndex(m => m.ContractId);
            builder.HasIndex(m => m.Status);
        }
    }
}
