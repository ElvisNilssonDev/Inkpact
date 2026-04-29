using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IMilestoneRepository : IRepository<Milestone>
    {
        Task<IEnumerable<Milestone>> GetByContractIdAsync(Guid contractId, CancellationToken ct = default);
    }
}
