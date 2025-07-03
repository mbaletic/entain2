using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace entain2
{
    public class Base
    {
        public HttpClient httpClient;
        public Client client;

        public Base()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client = new Client(httpClient);
        }

        [AssemblyCleanup]
        public void TearDown()
        {

            httpClient.Dispose();

        }


    }
}