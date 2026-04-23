using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Milestone : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public MilestoneStatus Status { get; set; } = MilestoneStatus.Pending;

        public Guid ContractId { get; set; }
        public Contract Contract { get; set; } = null!;
    }
}
