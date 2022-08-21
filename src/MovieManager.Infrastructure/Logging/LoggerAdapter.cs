using MovieManager.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;

namespace MovieManager.Infrastructure.Logging
{
    public class LoggerAdapter<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;
        public LoggerAdapter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<T>();
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        public void LogWarning(Exception exception, string message, params object[] args)
        {
            _logger.LogWarning(exception, message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogInformation(Exception exception, string message, params object[] args)
        {
            _logger.LogInformation(exception, message, args);
        }

        //TODO: Add if logEnable
        public void LogError(string message, params object[] args)
        {
            _logger.LogError(message, args);
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            _logger.LogError(exception, message, args);
        }

        public void LogJob( string message, params object[] args)
		{
            _logger.LogInformation("*****************************************************************************************************");
            _logger.LogInformation("************** {message} - {Date} **************", message, DateTime.Now.ToString("u", DateTimeFormatInfo.InvariantInfo));
            _logger.LogInformation("*****************************************************************************************************");
        }
    }
}
