using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IGigRepository : IRepository<Gig>
    {
        Task<IEnumerable<Gig>> SearchAsync(string? tag, decimal? maxBudget, CancellationToken ct = default);
        Task<IEnumerable<Gig>> GetByClientIdAsync(Guid cliendId, CancellationToken ct = default);
    }
}
