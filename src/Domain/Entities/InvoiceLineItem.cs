using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class InvoiceLineItem : BaseEntity
    {
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }

        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;

        public Guid? MilestoneId { get; set; }
        public Milestone? Milestone { get; set; }
    }
}
