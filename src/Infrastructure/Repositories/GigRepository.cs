using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GigRepository : GenericRepository<Gig>, IGigRepository
{
    public GigRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Gig>> SearchAsync(
        string? tag,
        decimal? maxBudget,
        CancellationToken ct = default)
    {
        IQueryable<Gig> query = _dbSet;

        if (!string.IsNullOrEmpty(tag))
            query = query.Where(g => g.Tags.Contains(tag));

        if (maxBudget.HasValue)
            query = query.Where(g => g.Budget <= maxBudget.Value);

        return await EntityFrameworkQueryableExtensions.ToListAsync(query, ct);
    }

    public async Task<IEnumerable<Gig>> GetByClientIdAsync(Guid clientId, CancellationToken ct = default)
    {
        IQueryable<Gig> query = _dbSet.Where(g => g.ClientId == clientId);
        return await EntityFrameworkQueryableExtensions.ToListAsync(query, ct);
    }
}