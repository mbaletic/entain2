using entain2.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
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
        public static async Task Setup(TestContext context)
        {
            httpClient = new HttpClient();
            client = new Client(httpClient);
            var response = await client.FindPetsByStatusAsync(new List<PetStatus> { PetStatus.Available, PetStatus.Sold, PetStatus.Pending });
            Logger.Log($"Start of the test:\n{JsonConvert.SerializeObject(response, Formatting.Indented)}");
        }


        [AssemblyCleanup]
        public static async Task Teardown()
        {

            foreach (var testPetId in PetGenerator.generatedPetIds)
            {
                try
                {
                    await client.DeletePetAsync("", testPetId);
                    Logger.Log($"Deleted {testPetId}.");
                }
                catch (ApiException e)
                {
                    // Because in one method we already deleted the pet!
                    Logger.Log($"Failed to delete pet {testPetId} - {e.Message}");
                }
            }
            var response = await client.FindPetsByStatusAsync(new List<PetStatus> { PetStatus.Available, PetStatus.Sold, PetStatus.Pending });
            Logger.Log($"End of the test:\n{JsonConvert.SerializeObject(response, Formatting.Indented)}");
            httpClient.Dispose();
        }
    }
}