using PFM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PFM.Application.Dto
{
    public class CreateUserDto
    {
        [JsonPropertyName("first-name")]
        public required string FirstName { get; set; }

        [JsonPropertyName("last-name")]
        public required string LastName { get; set; }

        [JsonPropertyName("email")]
        public required string Email { get; set; }

        [JsonPropertyName("password")]
        public required string Password { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("phone-number")]
        public required string PhoneNumber { get; set; }

        [JsonPropertyName("birthday")]
        public DateOnly Birthday { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; } = RoleEnum.user.ToString();

        [JsonPropertyName("jmbg")]
        public required string Jmbg { get; set; }
    }
}
