using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Review : BaseEntity
    {
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;

        public Guid ReviewerId { get; set; }
        public User Reviewer { get; set; } = null!;

        public Guid ContractId { get; set; }
        public Contract Contract { get; set; } = null!;
    }
}
