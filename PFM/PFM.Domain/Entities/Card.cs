using PFM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Domain.Entities
{
    public class Card
    {
        public Guid Id { get; set; }

        public string OwnerName { get; set; }

        public string CardNumber { get; set; }

        public DateOnly ExpirationDate { get; set; }

        public CardTypeEnum CardType { get; set; }

        public bool IsActive { get; set; }

        public Guid UserId { get; set; }
        public Guid AccountId { get; set; }

        public User User { get; set; }

        public Account Account { get; set; }

        public List<Transaction>? Transactions { get; set; } = [];
    }
}
