using System;

namespace MovieManager.Core.Interfaces
{
    /// <summary>
    /// This type eliminates the need to depend directly on the ASP.NET Core logging types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAppLogger<T>
    {
		void LogError(string message, params object[] args);
		void LogError(Exception exception, string message, params object[] args);
		void LogInformation(string message, params object[] args);
		void LogInformation(Exception exception, string message, params object[] args);
		void LogJob(string message, params object[] args);
		void LogWarning(string message, params object[] args);
		void LogWarning(Exception exception, string message, params object[] args);
	}
}
