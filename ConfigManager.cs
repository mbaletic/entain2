using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entain2
{

    public static class ConfigManager
    {
        static readonly Config _config = null!;

        static ConfigManager()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            var json = File.ReadAllText(path);
            _config = JsonConvert.DeserializeObject<Config>(json) ?? throw new Exception("Failed to deserialize config.");
        }

        public static Config Settings => _config;
    }

}
