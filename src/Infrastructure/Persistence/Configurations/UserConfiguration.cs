using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(u => u.Email)
                .IsUnique();   // no duplicate emails

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.Role)
                .IsRequired()
                .HasConversion<string>();   // store enum as string in DB (more readable than 0/1/2)

            // 1-to-1 relationship: User → FreelancerProfile (optional)
            builder.HasOne(u => u.FreelancerProfile)
                .WithOne(fp => fp.User)
                .HasForeignKey<FreelancerProfile>(fp => fp.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
