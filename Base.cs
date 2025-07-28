using entain2.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace entain2
{
    [TestClass]
    public class Base
    {
        public static HttpClient httpClient = null!;
        public static Client client = null!;
        public TestContext TestContext { get; set; }
        protected Logger logger;
        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            httpClient = new HttpClient(new HttpClientHandler());
            client = new Client(httpClient)
            {
                BaseUrl = ConfigManager.Settings.BaseUrl
            };
        }
        [TestInitialize]
        public void TestSetup()
        {
            var logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }
            string fileName = $"{TestContext.TestName}-{Guid.NewGuid()}.txt";
            string logFilePath = Path.Combine(logsDirectory, fileName);
            logger = new Logger(logFilePath, TestContext);
            var loggingHandler = new LoggingHandler(new HttpClientHandler(), logger);
            httpClient = new HttpClient(loggingHandler);
            client = new Client(httpClient)
            {
                BaseUrl = ConfigManager.Settings.BaseUrl
            };
            logger.Log($"Starting test: {TestContext.TestName}");
        }
        [TestCleanup]
        public async Task TestTeardown()
        {
            foreach (var petId in PetHelper.generatedPetIds)
            {
                try
                {
                    await client.DeletePetAsync(ConfigManager.Settings.ApiKey, petId);
                    logger.Log($"Deleted pet {petId}.");
                }
                catch (ApiException e)
                {
                    logger.Log($"Could not delete pet {petId}: {e.Message}");
                }
            }
            PetHelper.generatedPetIds.Clear();
            logger.Log($"Ending test: {TestContext.TestName}");
        }
        [AssemblyCleanup]
        public static void SuiteTeardown()
        {
            httpClient.Dispose();
        }
    }
}