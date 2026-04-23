using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Proposal : BaseEntity
    {
        public string CoverLetter { get; set; } = string.Empty;
        public decimal ProposedRate { get; set; }
        public int EstimatedDays { get; set; }
        public ProposalStatus status { get; set; } = ProposalStatus.Pending;

        public Guid GigId { get; set; }
        public Gig Gig { get; set; } = null!;

        public Guid FreelancerId { get; set; }
        public User Freelancer { get; set; } = null!;
    }
}
