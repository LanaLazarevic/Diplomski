using PFM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; set; }

        public long AccountNumber { get; set; }

        public double AvailableAmount { get; set; }

        public double ReservedAmount { get; set; }

        public string Currency { get; set; }

        public AccountTypeEnum AccountType { get; set; }

        public bool IsActive { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public List<Card> Cards { get; set; } = [];
    }
}
