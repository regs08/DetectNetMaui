using System;
using System.Collections.Generic;

namespace DetectApp
{
    public class LogParser
    {
        private List<LogEntry> _logs;

        public LogParser(List<LogEntry> logs)
        {
            _logs = logs;
        }

        public List<string> ParseLogs(List<string> filterLabels = null, double confidenceThreshold = 0.75)
        {
            filterLabels ??= new List<string> { "person" };
            List<string> parsedLogs = new List<string>();

            foreach (LogEntry logEntry in _logs)
            {
                if (logEntry != null && logEntry.Event == "detection" &&
            filterLabels.Contains(logEntry.Label) && logEntry.Confidence >= confidenceThreshold)
                {
                    // Build a formatted log message
                    string logMessage = FormatLogMessage(logEntry);

                    // Add the formatted log to the list
                    parsedLogs.Add(logMessage);
                }
            }

            return parsedLogs;
        }

        private string FormatLogMessage(LogEntry logEntry)
        {
            // Build a formatted log message based on the log entry properties
            return $"Event: {logEntry.Event}, Label: {logEntry.Label}, Confidence: {logEntry.Confidence}";
        }
    }
}
