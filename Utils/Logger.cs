using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entain2.Utils
{
    /// <summary>
    /// Serves as a additional logger to the test project /  failsafe  - to log state of the petstore before, during and after the run
    /// </summary>
    public static class Logger
    {
        private readonly static string logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private readonly static string logFilePath;
        public static TestContext? TestContext { get; set; }

        static Logger()
        {
            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
            }

            string fileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt";
            logFilePath = Path.Combine(logsDirectory, fileName);
            Log("Log file created");
        }
        public static void Log(string message, bool logToTxtFileOnly = false)
        {
            string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            if (TestContext != null)
            {
                TestContext.WriteLine(entry);
            }
            File.AppendAllText(logFilePath, entry + Environment.NewLine);
        }
    }
}
