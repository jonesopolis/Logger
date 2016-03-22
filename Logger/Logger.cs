using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Jones.Logger
{
    public abstract class Logger
    {
        LogLevel LogLevel { get; set; }

        protected Logger(LogLevel logLevel)
        {
            LogLevel = logLevel;
        }

        public void Log(LogLevel logLevel, string message, [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (CanLog(logLevel))
            {
                LogHeader(logLevel);
                Log(message, memberName, lineNumber);
            }
        }

        public void Log(LogLevel logLevel, Exception ex, [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (CanLog(logLevel))
            {
                LogHeader(logLevel);
                Log(ex, memberName, lineNumber);
            }
        }

        public void Log(LogLevel logLevel, string message, Exception ex, [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (CanLog(logLevel))
            {
                LogHeader(logLevel);
                Log(message, ex, memberName, lineNumber);
            }
        }

        protected abstract void LogHeader(LogLevel logLevel);
        protected abstract void Log(string message, string memberName, int lineNumber);
        protected abstract void Log(Exception ex, string memberName, int lineNumber);
        protected abstract void Log(string message, Exception ex, string memberName, int lineNumber);

        protected string GetMessageString(string message)
        {
            return $"Message: {message}{Environment.NewLine}";
        }
        protected string GetExceptionString(Exception ex, int level = 0)
        {
            StackTrace trace = new StackTrace(ex, false);
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{GetTabs(level)}Exception: {ex.GetType().Name}");
            sb.AppendLine($"{GetTabs(level)}Exception Message: {ex.Message}");
            if (trace.FrameCount > 0)
            {
                sb.AppendLine($"{GetTabs(level)}In Class: {trace.GetFrame(0).GetMethod().DeclaringType}");
                sb.AppendLine($"{GetTabs(level)}In Method: {trace.GetFrame(0).GetMethod()}");
                sb.AppendLine($"{GetTabs(level)}At Line: {trace.GetFrame(0).GetFileLineNumber()}");
            }

            if (ex.InnerException != null)
                sb.Append(GetExceptionString(ex.InnerException, ++level));

            return sb.ToString();
        }

        protected string GetCallerInfoString(string memberName, int lineNumber)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Logged By: {memberName}");
            sb.AppendLine($"Logged At Line: {lineNumber}");

            return sb.ToString();
        }

        protected string GetHeaderString(LogLevel logLevel)
        {
            return $"{logLevel.ToString().ToUpper()} ----------------- {DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss")}";
        }

        private string GetTabs(int count)
        {
            var tabs = "";
            for (var i = 0; i < count; i++)
            {
                tabs += "\t";
            }

            return tabs;
        }

        private bool CanLog(LogLevel logLevel)
        {
            return logLevel >= LogLevel;
        }
    }
}