using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IProposalRepository : IRepository<Proposal>
    {
        Task<IEnumerable<Proposal>> GetByGigIdAsync(Guid gigId, CancellationToken ct = default);
        Task<IEnumerable<Proposal>> GetByFreelancerIdAsync(Guid freelancerId, CancellationToken ct = default);
        Task RejectAllExceptAsync(Guid gigId, Guid acceptedProposalId, CancellationToken ct = default);
    }
}
