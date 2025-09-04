using PFM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PFM.Application.Dto
{
    public class CreateAccountDto
    {
        [JsonPropertyName("account-number")]
        public long AccountNumber { get; set; }

        [JsonPropertyName("available-amount")]
        public double AvailableAmount { get; set; }

        [JsonPropertyName("reserved-amount")]
        public double ReservedAmount { get; set; }

        [JsonPropertyName("currency")]
        public required string Currency { get; set; }

        [JsonPropertyName("user-jmbg")]
        public required string UserJmbg { get; set; }

        [JsonPropertyName("account-type")]
        public string AccountType { get; set; } = AccountTypeEnum.checking.ToString();
    }
}
