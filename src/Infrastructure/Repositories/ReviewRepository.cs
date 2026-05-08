using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Review>> GetByContractIdAsync(Guid contractId, CancellationToken ct = default)
            => await _dbSet.Where(r => r.ContractId == contractId).ToListAsync(ct);

        public async Task<IEnumerable<Review>> GetByReviewedUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            // A review is "for" the OTHER party in the contract
            // So we need to join with contracts and find reviews where the contract's other party = userId. and this is Important knowledge so ill keep this here!
            return await _dbSet
                .Where(r => _context.Contracts.Any(c =>
                    c.Id == r.ContractId &&
                    ((c.ClientId == userId && r.ReviewerId == c.FreelancerId) ||
                     (c.FreelancerId == userId && r.ReviewerId == c.ClientId))))
                .ToListAsync(ct);
        }
    }
}
