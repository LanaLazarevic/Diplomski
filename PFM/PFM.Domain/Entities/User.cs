using PFM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public DateOnly Birthday { get; set; }

        public RoleEnum Role { get; set; }

        public List<Card>? Cards { get; set; } = [];
    }
}
