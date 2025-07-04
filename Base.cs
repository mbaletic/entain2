using entain2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace entain2
{
    [TestClass]
    public class Base
    {
        public static HttpClient httpClient;
        public static Client client;

        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            httpClient = new HttpClient();
            client = new Client(httpClient);
        }


        [AssemblyCleanup]
        public static async Task Teardown()
        {

            foreach (var testPetId in PetGenerator.generatedPetIds)
            {
                try
                {
                    await client.DeletePetAsync("", testPetId);
                    Console.WriteLine($"Deleted {testPetId}.");
                }
                catch (ApiException ex)
                {
                    // Because in one method we already deleted the pet!
                    Console.WriteLine($"Failed to delete pet {testPetId} - {ex.Message}");
                }
            }
            httpClient.Dispose();
        }
    }
}