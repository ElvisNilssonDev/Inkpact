using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Pagination
{
    public record PagedResult<T>(
        IEnumerable<T> Items,
        int Page,
        int PageSize,
        int TotalCount
    )
    {
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNext => Page < TotalPages;
        public bool HasPrevious => Page > 1;
    }
}
