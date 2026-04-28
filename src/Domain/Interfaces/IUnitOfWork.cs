using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IGigRepository Gigs { get; }
        IProposalRepository Proposals { get; }
        IContractRepository Contracts { get; }
        IMilestoneRepository Milestones { get; }
        IInvoiceRepository Invoice { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
