using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Contract : BaseEntity
    {
        public decimal AgreedRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ContractStatus Status { get; set; } = ContractStatus.Active;

        public Guid GigId { get; set; }
        public Gig Gig { get; set; } = null!;

        public Guid ClientId { get; set; }
        public User Client { get; set; } = null!;

        public Guid FreelancerId { get; set; }
        public User Freelancer { get; set; } = null!;

        public ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
