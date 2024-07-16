using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ADS.readXml;

namespace ADS
{
    public class LogHelper
    {
        public void LogMessage(string message, LogLevel level)
        {
            string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "application.log");
            string logMessage = $"{DateTime.Now} [{level}] : {message}";
            const int maxFileSizeInBytes = 5 * 1024 * 1024;
            if (File.Exists(logFilePath) && new FileInfo(logFilePath).Length > maxFileSizeInBytes)
            {
                string arquivFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"application_{DateTime.Now:yyyyMMddHHmmss}.log");
                File.Move(logFilePath, arquivFilePath);
            }
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }
        public enum LogLevel
        {
            INFO,
            WARN,
            ERROR
        }
    }
}
