using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Serilog;
using System;

namespace MovieManagerWeb
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT ");

			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional:false, reloadOnChange:true)
				.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
				.Build();

			Serilog.Debugging.SelfLog.Enable(Console.Out);

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.CreateLogger();

			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSerilog()
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
