using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ContractRepository : GenericRepository<Contract>, IContractRepository
{
    public ContractRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Contract>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await _dbSet
            .Where(c => c.ClientId == userId || c.FreelancerId == userId)
            .ToListAsync(ct);
}
