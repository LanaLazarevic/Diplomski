using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Infrastructure.Settings
{
    public class EmailSettings
    {
        public string Host { get; set; } = string.Empty;

        public int Port { get; set; } = 25;

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? From { get; set; }
    }
}
