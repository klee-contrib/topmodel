using Microsoft.Extensions.Logging;
using TopModel.Core;

namespace TopModel.Generator;

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
                Console.ForegroundColor = _generatorName != null ? ConsoleColor.Magenta : ConsoleColor.DarkGray;
                Console.Write(split[0]);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($" {split[1]} ");
                Console.ForegroundColor = logLevel switch
                {
                    LogLevel.Error => ConsoleColor.Red,
                    LogLevel.Warning => ConsoleColor.Yellow,
                    _ => ConsoleColor.Gray
                };

                message = WriteAction(message, "Supprimé", ConsoleColor.DarkRed);
                message = WriteAction(message, "Créé", ConsoleColor.DarkGreen);
                message = WriteAction(message, "Modifié", ConsoleColor.DarkCyan);

                var split2 = message.Split(Path.DirectorySeparatorChar);
                if (split2.Length > 1)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{string.Join(Path.DirectorySeparatorChar, split2[0..^1])}{Path.DirectorySeparatorChar}");
                    Console.ForegroundColor = ConsoleColor.Blue;
                }

                Console.WriteLine(split2[^1]);

                if (exception is not null and not ModelException)
                {
                    Console.WriteLine(exception.StackTrace);
                }

                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public string WriteAction(string message, string action, ConsoleColor color)
        {
            if (message.LastIndexOf(action) >= 0)
            {
                Console.ForegroundColor = color;
                message = message.Split(action)[1];
                Console.Write(action);
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }

            return message;
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