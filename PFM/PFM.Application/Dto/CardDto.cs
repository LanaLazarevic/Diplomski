using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PFM.Application.Dto
{
    public class CardDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

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

        [JsonPropertyName("card-type")]
        public required string CardType { get; set; }

        [JsonPropertyName("user-id")]
        public Guid UserId { get; set; }

        [JsonPropertyName("is-active")]
        public bool IsActive { get; set; }
    }
}
