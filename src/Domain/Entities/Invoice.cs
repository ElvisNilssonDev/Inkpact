using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

        public Guid ContractId { get; set; }
        public Contract Contract { get; set; } = null!;

        public ICollection<InvoiceLineItem> LineItems { get; set; } = new List<InvoiceLineItem>();
    }
}
