using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entain2.Utils
{
    public static class RawJsonClient
    {

        public static async Task<HttpResponseMessage> PostPetAsync(string jsonPayload)
        {
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            return await Base.httpClient.PostAsync($"{Base.client.BaseUrl}pet", content);
        }
    }
}
