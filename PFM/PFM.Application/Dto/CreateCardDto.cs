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

        [JsonPropertyName("available-amount")]
        public double AvailableAmount { get; set; }

        [JsonPropertyName("reserved-amount")]
        public double ReservedAmount { get; set; }

        [JsonPropertyName("user-jmbg")]
        public string UserJmbg { get; set; }

        [JsonPropertyName("card-type")]
        public string CardType { get; set; } = CardTypeEnum.debit.ToString();
    }
}
