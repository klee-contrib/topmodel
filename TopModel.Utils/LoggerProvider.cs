using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace TopModel.Utils;

public class LoggerProvider : ILoggerProvider
{
    static LoggerProvider()
    {
        AnsiConsole.Profile.Width = int.MaxValue;
    }

    public int Changes { get; private set; }

    /// <inheritdoc cref="ILoggerProvider.CreateLogger" />
    public ILogger CreateLogger(string categoryName)
    {
        return new ConsoleLogger(categoryName.Split(".")[^1], () => Changes++);
    }

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose() { }

    public class ConsoleLogger(string categoryName, Action registerChange) : ILogger
    {
        private static readonly object _lock = new();
        private string? _generatorName;
        private string? _storeColor;
        private int? _storeNumber;

        /// <inheritdoc cref="ILogger.BeginScope{TState}" />
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
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

            return null;
        }

        /// <inheritdoc cref="ILogger.IsEnabled" />
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.Debug;
        }

        /// <inheritdoc cref="ILogger.Log{TState}" />
        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter
        )
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
                    AnsiConsole.WriteLine();
                    return;
                }

                var prefix = string.Empty;
                if (_storeNumber != null && _storeColor != null)
                {
                    prefix = $"[{_storeColor}]#{_storeNumber.Value} [/]";
                    AnsiConsole.Markup(prefix);
                }
                var name = ((_generatorName ?? categoryName) + " ").PadRight(
                    25 - (prefix.Length > 0 ? (prefix.Length - 3 - prefix.IndexOf('#')) : 0),
                    '-'
                );
                var split = name.Split(" ");
                var fColor = _generatorName != null ? "fuchsia" : "grey";
                AnsiConsole.Markup($"[{fColor}]{split[0].EscapeMarkup()}[/]");
                AnsiConsole.Markup($" {split[1]} ");

                message = message.EscapeMarkup();
                message = WriteAction(message, "Supprimé", "maroon");
                message = WriteAction(message, "Créé", "green");
                message = WriteAction(message, "Modifié", "teal");
                message = WriteAction(message, "Renommé", "yellow");
                if (logLevel != LogLevel.Error && logLevel != LogLevel.Warning)
                {
                    var split2 = message.Split('/');
                    var color = "silver";
                    if (split2.Length > 1)
                    {
                        AnsiConsole.Markup($"{string.Join('/', split2[0..^1])}/");
                        color = "blue";
                    }

                    var split3 = split2[^1].Split('\'');
                    if (split3.Length == 2)
                    {
                        AnsiConsole.Markup($"{split3[0]}");
                        AnsiConsole.MarkupLine($"'{split3[1]}");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[{color}]{split2[^1]}[/]");
                    }
                }
                else if (logLevel == LogLevel.Warning)
                {
                    AnsiConsole.MarkupLine($"[yellow]{message}[/]");
                }
                else if (logLevel == LogLevel.Error)
                {
                    AnsiConsole.MarkupLine($"[red]{message}[/]");
                }

                if (exception is not null and not LegitException)
                {
                    AnsiConsole.MarkupLine($"[red]{exception.StackTrace.EscapeMarkup()}[/]");
                }
            }
        }

        public string WriteAction(string message, string action, string color)
        {
            if (message.LastIndexOf(action) >= 0)
            {
                registerChange();
                AnsiConsole.Markup($"[{color}]{action}[/]");
                return message.Split(action)[1];
            }

            return message;
        }
    }
}
