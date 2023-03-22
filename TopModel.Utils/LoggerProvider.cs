using Microsoft.Extensions.Logging;

namespace TopModel.Utils;

public class LoggerProvider : ILoggerProvider
{
    public int Changes { get; private set; }

    public void Dispose()
    {
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new ConsoleLogger(categoryName.Split(".").Last(), () => Changes++);
    }

    public class ConsoleLogger : ILogger
    {
        private static readonly object _lock = new();

        private readonly string _categoryName;
        private readonly Action _registerChange;
        private string? _generatorName;
        private int? _storeNumber;
        private ConsoleColor? _storeColor;

        public ConsoleLogger(string categoryName, Action registerChange)
        {
            _categoryName = categoryName;
            _registerChange = registerChange;
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

                if (_storeNumber != null && _storeColor != null)
                {
                    Console.ForegroundColor = _storeColor.Value;
                    Console.Write($"#{_storeNumber.Value} ");
                }

                var name = ((_generatorName ?? _categoryName) + " ").PadRight(22, '-');
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

                if (logLevel != LogLevel.Error && logLevel != LogLevel.Warning)
                {
                    var split2 = message.Split('/');
                    if (split2.Length > 1)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"{string.Join('/', split2[0..^1])}/");
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }

                    Console.WriteLine(split2[^1]);
                }
                else
                {
                    Console.WriteLine(message);
                }

                if (exception is not null and not LegitException)
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
                _registerChange();
            }

            return message;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            if (state is LoggingScope scope)
            {
                _storeNumber = scope.Number;
                _storeColor = scope.Color;
            }
            else if (state is string generatorName)
            {
                _generatorName = generatorName;
            }

            return null!;
        }
    }
}