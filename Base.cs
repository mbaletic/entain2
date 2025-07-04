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

        [TestCleanup]
        public async Task TestTeardown()
        {
            foreach (var petId in PetGenerator.generatedPetIds)
            {
                try
                {
                    await client.DeletePetAsync("", petId);
                    Logger.Log($"Deleted pet {petId}.");
                }
                catch (ApiException e)
                {
                    Logger.Log($"Could not delete pet {petId}: {e.Message}");
                }
            }
            PetGenerator.generatedPetIds.Clear();
        }

        [AssemblyCleanup]
        public static async Task SuiteTeardown()
        {
            var response = await client.FindPetsByStatusAsync(new List<PetStatus> { PetStatus.Available, PetStatus.Sold, PetStatus.Pending });
            Logger.Log($"End of the test:\n{JsonConvert.SerializeObject(response, Formatting.Indented)}");
            httpClient.Dispose();
        }
    }
}