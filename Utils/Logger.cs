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
        private static string logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static string logFilePath;

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
        public static void Log(string message)
        {
            string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            File.AppendAllText(logFilePath, entry + Environment.NewLine);
        }
    }
}
