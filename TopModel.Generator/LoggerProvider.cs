using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator
{
    public class LoggerProvider : ILoggerProvider
    {
        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new ConsoleLogger(categoryName);
        }

        public class ConsoleLogger : ILogger
        {
            private readonly string _categoryName;

            public ConsoleLogger(string categoryName)
            {
                _categoryName = categoryName;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (!IsEnabled(logLevel))
                {
                    return;
                }

                Console.ForegroundColor = logLevel switch
                {
                    LogLevel.Error => ConsoleColor.Red,
                    LogLevel.Warning => ConsoleColor.Yellow,
                    _ => ConsoleColor.Gray
                };

                var message = formatter(state, exception);

                var category = message == string.Empty ? "I" : _categoryName.Split(".").Last().Replace("Generator", string.Empty);
                category = category == "I" ? string.Empty : $"{category}: ";

                Console.WriteLine($"{category}{message}");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null!;
            }
        }
    }
}
