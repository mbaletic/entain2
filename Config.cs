using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entain2
{
    public class Config
    {
        public required string BaseUrl { get; set; }
        public int TimeoutSeconds { get; set; }
        public string? Environment { get; set; }
        public string? ApiKey { get; set; }

    }
}
