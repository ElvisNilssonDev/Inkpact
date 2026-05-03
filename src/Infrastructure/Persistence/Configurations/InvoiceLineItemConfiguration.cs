using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations
{
    public class InvoiceLineItemConfiguration : IEntityTypeConfiguration<InvoiceLineItem>
    {
        public void Configure(EntityTypeBuilder<InvoiceLineItem> builder)
        {
            builder.HasKey(li => li.Id);

            builder.Property(li => li.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(li => li.Amount)
                .HasColumnType("decimal(18,2)");

            // Optional FK to milestone (line item might not always be tied to a milestone)
            builder.HasOne(li => li.Milestone)
                .WithMany()
                .HasForeignKey(li => li.MilestoneId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
