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

        public DateTime ExpirationDate { get; set; }

        public double AvailableAmount { get; set; }

        public double ReservedAmount { get; set; }

        public CardTypeEnum CardType { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public List<Transaction>? Transactions { get; set; } = [];
    }
}
