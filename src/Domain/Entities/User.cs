using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class User :  BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }

        public FreelancerProfile? FreelancerProfile { get; set; }
        public ICollection<Gig> PostedGigs { get; set; } = new List<Gig>();
        public ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
