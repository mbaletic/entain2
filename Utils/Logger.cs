using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entain2.Utils
{
    public class Logger
    {
        private readonly string logFilePath;
        private readonly TestContext? testContext;
        private readonly object fileLock = new();
        public Logger(string logFilePath, TestContext? testContext)
        {
            this.logFilePath = logFilePath;
            this.testContext = testContext;
        }
        public void Log(string message, bool logToTxtFileOnly = false)
        {
            string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            if (testContext != null)
            {
                testContext.WriteLine(entry);
            }
            if (!string.IsNullOrEmpty(logFilePath))
            {
                lock (fileLock)
                {
                    File.AppendAllText(logFilePath, entry + Environment.NewLine);
                }
            }
        }
    }
}
