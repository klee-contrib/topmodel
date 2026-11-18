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
        public string GetMessage(params object[] args)
        {
            var format = ResourceManagerCache<T>.Instance.GetString(Enum.GetName(messageType)!)
                ?? Enum.GetName(messageType)!;
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
