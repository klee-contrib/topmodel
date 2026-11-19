using System.Globalization;
using System.Resources;
using Microsoft.Extensions.Logging;

namespace TopModel.Utils;

public static class LogUtils
{
    extension(ILogger logger)
    {
        public void LogError<T>(T key, params object[] args)
            where T : struct, Enum
        {
            logger.LogError(GetLogMessage(key, args));
        }

        public void LogInformation<T>(T key, params object[] args)
            where T : struct, Enum
        {
            logger.LogInformation(GetLogMessage(key, args));
        }

        public void LogWarning<T>(T key, params object[] args)
            where T : struct, Enum
        {
            logger.LogWarning(GetLogMessage(key, args));
        }
    }

    extension<T>(T messageType)
        where T : struct, Enum
    {
        public string GetLocalizedMessage(string? language, params object[] args)
        {
            try
            {
                if (!string.IsNullOrEmpty(language))
                {
                    var culture = CultureInfo.GetCultureInfo(language);
                    return messageType.GetLocalizedMessage(culture, args);
                }
            }
            catch (CultureNotFoundException)
            {
                // Si la culture est invalide, on ignore et on utilise la culture par défaut.
            }

            return messageType.GetLocalizedMessage(CultureInfo.InvariantCulture, args);
        }

        public string GetMessage(params object[] args)
        {
            return messageType.GetLocalizedMessage(CultureInfo.CurrentUICulture, args);
        }

        private string GetLocalizedMessage(CultureInfo? culture, params object[] args)
        {
            var name = Enum.GetName(messageType)!;
            var format = ResourceManagerCache<T>.Instance.GetString(name, culture)
                ?? ResourceManagerCache<T>.Instance.GetString(name, CultureInfo.InvariantCulture)
                ?? name;
            return args.Length > 0 ? string.Format(format, args) : format;
        }
    }

    extension(ErrorType errorType)
    {
        public string FormatDiagnosticMessage(string message)
        {
            return $"{{{errorType}}} - {message}";
        }
    }

    private static string GetLogMessage<T>(T key, params object[] args)
        where T : struct, Enum
    {
        return key is ErrorType errorType
            ? errorType.FormatDiagnosticMessage(errorType.GetMessage(args))
            : key.GetMessage(args);
    }

    private static class ResourceManagerCache<T>
        where T : struct, Enum
    {
        public static readonly ResourceManager Instance = new(typeof(T));
    }
}
