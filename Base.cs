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
        public TestContext TestContext { get; set; }

        [AssemblyInitialize]
        public static async Task Setup(TestContext context)
        {
            var loggingHandler = new LoggingHandler(new HttpClientHandler());
            httpClient = new HttpClient(loggingHandler);
            client = new Client(httpClient);
            client.BaseUrl = ConfigManager.Settings.BaseUrl;
            var response = await client.FindPetsByStatusAsync(new List<PetStatus> { PetStatus.Available, PetStatus.Sold, PetStatus.Pending });
            Logger.Log($"Start of the test:\n{JsonConvert.SerializeObject(response, Formatting.Indented)}", true);
        }

        [TestInitialize]
        public void TestSetup()
        {
            Logger.TestContext = TestContext;
            Logger.Log($"Starting test: {TestContext.TestName}");
        }

        [TestCleanup]
        public async Task TestTeardown()
        {
            foreach (var petId in PetGenerator.generatedPetIds)
            {
                try
                {
                    await client.DeletePetAsync(ConfigManager.Settings.ApiKey, petId);
                    Logger.Log($"Deleted pet {petId}.");
                }
                catch (ApiException e)
                {
                    Logger.Log($"Could not delete pet {petId}: {e.Message}");
                }
            }
            PetGenerator.generatedPetIds.Clear();
            Logger.Log($"Ending test: {TestContext.TestName}");
        }

        [AssemblyCleanup]
        public static async Task SuiteTeardown()
        {
            var response = await client.FindPetsByStatusAsync(new List<PetStatus> { PetStatus.Available, PetStatus.Sold, PetStatus.Pending });
            Logger.Log($"End of the test:\n{JsonConvert.SerializeObject(response, Formatting.Indented)}", true);
            httpClient.Dispose();
        }
    }
}