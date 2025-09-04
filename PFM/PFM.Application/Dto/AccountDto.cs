using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PFM.Application.Dto
{
    public class AccountDto
    {

        [JsonPropertyName("account-number")]
        public long AccountNumber { get; set; }

        [JsonPropertyName("available-amount")]
        public double AvailableAmount { get; set; }

        [JsonPropertyName("reserved-amount")]
        public double ReservedAmount { get; set; }

        [JsonPropertyName("currency")]
        public required string Currency { get; set; }

        [JsonPropertyName("account-type")]
        public required string AccountType { get; set; }

        [JsonPropertyName("is-active")]
        public bool IsActive { get; set; }

        [JsonPropertyName("user-full-name")]
        public required string UserFullName { get; set; }
    }
}
