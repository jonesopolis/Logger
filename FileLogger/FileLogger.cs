using System;
using System.IO;
using System.Security.Cryptography;
using Jones.Logger;

namespace Jones.FileLogger
{
    public sealed class FileLogger : Logger.Logger
    {
        private readonly string _filePath;

        public FileLogger(LogLevel logLevel) : base(logLevel)
        {
            _filePath = Path.Combine(Environment.CurrentDirectory, "log.txt");

            if (!File.Exists(_filePath))
            {
                using (var fs = File.Create(_filePath)) { }
            }
        }

        public FileLogger(string filePath, LogLevel logLevel) : base(logLevel)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!Path.IsPathRooted(filePath))
            {
                filePath = Path.Combine(Environment.CurrentDirectory, filePath);
            }

            _filePath = filePath;

            if (!Directory.Exists(Path.GetDirectoryName(_filePath)))
            {
                throw new DirectoryNotFoundException(_filePath);
            }

            if (!File.Exists(_filePath))
            {
                using (var fs = File.Create(_filePath)) { }
            }
        }

        protected override void LogHeader(LogLevel logLevel)
        {
            using (var fs = File.Open(_filePath, FileMode.Append))
            {
                using (var writer = new StreamWriter(fs))
                {
                    writer.WriteLine(GetHeaderString(logLevel));
                }
            }
        }

        protected override void Log(string message, string memberName, int lineNumber)
        {
            using (var fs = File.Open(_filePath, FileMode.Append))
            {
                using (var writer = new StreamWriter(fs))
                {
                    writer.WriteLine(GetMessageString(message));
                    writer.WriteLine(GetCallerInfoString(memberName, lineNumber));
                }
            }
        }

        protected override void Log(Exception ex, string memberName, int lineNumber)
        {
            using (var fs = File.Open(_filePath, FileMode.Append))
            {
                using (var writer = new StreamWriter(fs))
                {
                    writer.WriteLine(GetExceptionString(ex));
                    writer.WriteLine(GetCallerInfoString(memberName, lineNumber));
                }
            }
        }

        protected override void Log(string message, Exception ex, string memberName, int lineNumber)
        {
            using (var fs = File.Open(_filePath, FileMode.Append))
            {
                using (var writer = new StreamWriter(fs))
                {
                    writer.WriteLine(GetMessageString(message));
                    writer.WriteLine(GetExceptionString(ex));
                    writer.WriteLine(GetCallerInfoString(memberName, lineNumber));
                }
            }
        }
    }
}