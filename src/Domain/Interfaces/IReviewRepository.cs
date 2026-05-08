using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetByContractIdAsync(Guid contractId, CancellationToken ct = default);
        Task<IEnumerable<Review>> GetByReviewedUserIdAsync(Guid userId, CancellationToken ct = default);
    }
}
