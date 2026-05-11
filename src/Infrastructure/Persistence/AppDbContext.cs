using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options) : base(options) { }

        public DbSet<User> Users => Set <User>();
        public DbSet<FreelancerProfile> FreelancerProfiles => Set<FreelancerProfile>();
        public DbSet<Gig> Gigs => Set<Gig>();
        public DbSet<Proposal> Proposals => Set<Proposal>();
        public DbSet<Contract> Contracts => Set<Contract>();
        public DbSet<Milestone> Milestones => Set<Milestone>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<InvoiceLineItem> InvoiceLineItems => Set<InvoiceLineItem>();
        public DbSet<Review> Reviews => Set<Review>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }


    }
}
