using PFM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PFM.Application.Dto
{
    public class CreateCardDto
    {
        [JsonPropertyName("owner-name")]
        public required string OwnerName { get; set; }

        [JsonPropertyName("card-number")]
        public required string CardNumber { get; set; }

        [JsonPropertyName("expiration-date")]
        public DateOnly ExpirationDate { get; set; }

        [JsonPropertyName("user-jmbg")]
        public string UserJmbg { get; set; }

        [JsonPropertyName("account-number")]
        public long AccountNumber { get; set; }

        [JsonPropertyName("card-type")]
        public string CardType { get; set; } = CardTypeEnum.debit.ToString();
    }
}
