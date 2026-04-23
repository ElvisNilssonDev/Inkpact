using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Gig : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Budget { get; set; }
        public DateTime Deadline { get; set; }
        public GigStatus Status { get; set; } = GigStatus.Open;
        public string Tags { get; set; } = string.Empty;

        public Guid ClientId { get; set; }
        public User Client { get; set; } = null!;
        public ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();
    }
}
