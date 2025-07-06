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
        public static HttpClient httpClient = null!;
        public static Client client = null!;
        public TestContext TestContext { get; set; }

        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            var loggingHandler = new LoggingHandler(new HttpClientHandler());
            httpClient = new HttpClient(loggingHandler);
            client = new Client(httpClient)
            {
                BaseUrl = ConfigManager.Settings.BaseUrl
            };
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
            foreach (var petId in PetHelper.generatedPetIds)
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
            PetHelper.generatedPetIds.Clear();
            Logger.Log($"Ending test: {TestContext.TestName}");
        }

        [AssemblyCleanup]
        public static void SuiteTeardown()
        {
            httpClient.Dispose();
        }
    }
}