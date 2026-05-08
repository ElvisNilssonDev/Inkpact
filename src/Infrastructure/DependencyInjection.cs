using Domain.Interfaces;

using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(  //The AddInfrastructure extension method bundles ALL Infrastructure registrations into a single call.
            this IServiceCollection services,                 //And one line replaces 12+ registrations. Much cleaner. and Neater.
            IConfiguration config)
        {
            // Database
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGigRepository, GigRepository>();
            services.AddScoped<IProposalRepository, ProposalRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IMilestoneRepository, MilestoneRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            // Services
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
