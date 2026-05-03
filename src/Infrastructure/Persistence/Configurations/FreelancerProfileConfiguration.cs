using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations
{
    public class FreelancerProfileConfiguration : IEntityTypeConfiguration<FreelancerProfile>
    {
        public void Configure(EntityTypeBuilder<FreelancerProfile> builder)
        {
            builder.HasKey(fp => fp.Id);

            builder.Property(fp => fp.Bio)
                .HasMaxLength(2000);

            builder.Property(fp => fp.Skills)
                .HasMaxLength(500);

            builder.Property(fp => fp.HourlyRate)
                .HasColumnType("decimal(18,2)");

            builder.Property(fp => fp.PortfolioUrl)
                .HasMaxLength(500);
        }
    }
}
