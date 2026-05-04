using System;
using Domain.Interfaces;
using Infrastructure.Persistence;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IUserRepository Users { get; }
        public IGigRepository Gigs { get; }
        public IProposalRepository Proposals { get; }
        public IContractRepository Contracts { get; }
        public IMilestoneRepository Milestones { get; }
        public IInvoiceRepository Invoices { get; }
        public IReviewRepository Reviews { get; }

        public UnitOfWork(
            AppDbContext context,
            IUserRepository users,
            IGigRepository gigs,
            IProposalRepository proposals,
            IContractRepository contracts,
            IMilestoneRepository milestones,
            IInvoiceRepository invoices,
            IReviewRepository reviews)
        {
            _context = context;
            Users = users;
            Gigs = gigs;
            Proposals = proposals;
            Contracts = contracts;
            Milestones = milestones;
            Invoices = invoices;
            Reviews = reviews;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await _context.SaveChangesAsync(ct);

        public void Dispose() => _context.Dispose();
    }
}
