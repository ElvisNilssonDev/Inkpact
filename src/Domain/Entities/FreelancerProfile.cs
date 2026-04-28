using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class FreelancerProfile : BaseEntity
    {
        public string Bio { get; set; } = string.Empty;
        public string Skills { get; set; } = string.Empty;

        public decimal HourlyRate { get; set; }
        public string? PortfolioUrl { get; set; }

        public Guid UserId { get; set; }
        public required User User { get; set; }
    }
}
