using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProposalRepository : GenericRepository<Proposal>, IProposalRepository
{
    public ProposalRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Proposal>> GetByGigIdAsync(Guid gigId, CancellationToken ct = default)
    {
        IQueryable<Proposal> query = _dbSet.Where(p => p.GigId == gigId);
        return await EntityFrameworkQueryableExtensions.ToListAsync(query, ct);
    }

    public async Task<IEnumerable<Proposal>> GetByFreelancerIdAsync(Guid freelancerId, CancellationToken ct = default)
    {
        IQueryable<Proposal> query = _dbSet.Where(p => p.FreelancerId == freelancerId);
        return await EntityFrameworkQueryableExtensions.ToListAsync(query, ct);
    }

    public async Task RejectAllExceptAsync(Guid gigId, Guid acceptedProposalId, CancellationToken ct = default)
    {
        IQueryable<Proposal> query = _dbSet.Where(p =>
            p.GigId == gigId
            && p.Id != acceptedProposalId
            && p.Status == ProposalStatus.Pending);

        var others = await EntityFrameworkQueryableExtensions.ToListAsync(query, ct);

        foreach (var p in others)
        {
            p.Status = ProposalStatus.Rejected;
            p.UpdatedAt = DateTime.UtcNow;
        }
    }
}
