using System;
using System.Diagnostics;
using System.Text;

namespace WebApi
{
    public class LogHelper
    {
        private static LisaEventLog lisaEventLog = new LisaEventLog();

        public static void CreateLog(Exception ex)
        {
            StringBuilder stringBuilder = new StringBuilder();
            while (ex != null)
            {
                stringBuilder.AppendLine(ex.ToString());
                ex = ex.InnerException;
            }
            CreateLog(EventLogEntryType.Error, stringBuilder.ToString());
        }

        public static void CreateLog(EventLogEntryType type, string logInfo)
        {
            lisaEventLog.WriteEntry(logInfo, type);
        }

        public static void CreateLog(string logInfo)
        {
            lisaEventLog.WriteEntry(logInfo, EventLogEntryType.Information);
        }
    }
}