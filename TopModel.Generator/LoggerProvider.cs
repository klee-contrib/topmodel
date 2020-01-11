using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace TopModel.Generator
{
    public class LoggerProvider : ILoggerProvider
    {
        public void Dispose() { }

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

                var category = _categoryName.Split(".").Last().Replace("Generator", string.Empty);
                category = category == "I" ? string.Empty : $"{category}: ";
                Console.WriteLine($"{category}{formatter(state, exception)}");
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
