using System;
using System.IO;
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
            return new ConsoleLogger(categoryName.Split(".").Last());
        }

        public class ConsoleLogger : ILogger
        {
            private static readonly object _lock = new();

            private readonly string _categoryName;
            private string? _generatorName;

            public ConsoleLogger(string categoryName)
            {
                _categoryName = categoryName;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                if (!IsEnabled(logLevel))
                {
                    return;
                }

                lock (_lock)
                {
                    var message = formatter(state, exception);

                    if (message == string.Empty)
                    {
                        Console.WriteLine();
                        return;
                    }

                    var name = ((_generatorName ?? _categoryName) + " ").PadRight(19, '-');
                    var split = name.Split(" ");
                    Console.ForegroundColor = _generatorName != null ? ConsoleColor.DarkGreen : ConsoleColor.DarkGray;
                    Console.Write(split[0]);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($" {split[1]} ");
                    Console.ForegroundColor = logLevel switch
                    {
                        LogLevel.Error => ConsoleColor.Red,
                        LogLevel.Warning => ConsoleColor.Yellow,
                        _ => ConsoleColor.Gray
                    };

                    var split2 = message.Split(Path.DirectorySeparatorChar);
                    if (split2.Length > 1)
                    {
                        Console.Write($"{string.Join(Path.DirectorySeparatorChar, split2[0..^1])}{Path.DirectorySeparatorChar}");
                        if (Console.ForegroundColor == ConsoleColor.Gray)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                    }

                    Console.WriteLine(split2[^1]);

                    if (exception is not null and not ModelException)
                    {
                        Console.WriteLine(exception.StackTrace);
                    }

                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                _generatorName = state as string;
                return null!;
            }
        }
    }
}
