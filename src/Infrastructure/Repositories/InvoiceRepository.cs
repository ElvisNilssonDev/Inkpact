using System;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Invoice>> GetByContractIdAsync(Guid contractId, CancellationToken ct = default)
            => await _dbSet
            .Include(i => i.LineItems)
            .Where(i => i.ContractId == contractId)
            .ToListAsync(ct);

        public async Task<IEnumerable<Invoice>> GetOverdueAsync(CancellationToken ct = default)
            => await _dbSet
            .Where(i => i.Status == InvoiceStatus.Sent && i.DueDate < DateTime.UtcNow)
            .ToListAsync(ct);
    }
}
