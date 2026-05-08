using System;
using Domain.Entities;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        Task<IEnumerable<Invoice>> GetByContractIdAsync(Guid contractId, CancellationToken ct = default);
        Task<IEnumerable<Invoice>> GetOverdueAsync(CancellationToken ct = default);
    }
}
