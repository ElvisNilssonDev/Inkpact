using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class MilestoneRepository : GenericRepository<Milestone>, IMilestoneRepository
    {
        public MilestoneRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Milestone>> GetByContractIdAsync(Guid contractId, CancellationToken ct = default)
            => await _dbSet.Where(m => m.ContractId == contractId).ToListAsync (ct);
    }
}
