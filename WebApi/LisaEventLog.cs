using System;
using System.Diagnostics;

namespace WebApi
{
    public class LisaEventLog
    {
        private readonly string _logName = @"Chuck";

        public string LogName => _logName;

        public LisaEventLog()
        {
        }

        public LisaEventLog(string logName)
        {
            _logName = logName;
        }

        public void WriteEntry(string error, EventLogEntryType type)
        {
            var sourceName = AppDomain.CurrentDomain.FriendlyName;
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, _logName);
            }
            using (EventLog eventLog = new EventLog(_logName))
            {
                eventLog.Source = sourceName;
                var message = $"{AppDomain.CurrentDomain.BaseDirectory}{Environment.NewLine}{error}";
                eventLog.WriteEntry(message, type);
            }
        }
    }
}