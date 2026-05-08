using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.TotalAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.HasMany(i => i.LineItems)
                .WithOne(li => li.Invoice)
                .HasForeignKey(li => li.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(i => i.ContractId);
            builder.HasIndex(i => i.Status);
        }
    }
}
