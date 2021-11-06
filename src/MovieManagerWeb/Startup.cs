using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieManager.Core;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Enumerations;
using MovieManager.Infrastructure;
using Serilog;
using Hangfire.SqlServer;
using Hangfire.Storage;

namespace MovieManagerWeb
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment environment)
		{
			_env = environment;

			// use the default config and add config from appsettings.COMPUTERNAME.json (if it exists)
			var builder = new ConfigurationBuilder()
				.SetBasePath(environment.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange:true)
				.AddEnvironmentVariables();

			_config = builder.Build();
		}

		public IConfiguration _config { get; }
		public IWebHostEnvironment _env { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			services.AddHangfire(config =>
				config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
				.UseSimpleAssemblyNameTypeSerializer()
				.UseDefaultTypeSerializer()
				.UseSqlServerStorage(_config.GetConnectionString("MovieManagerDB"), new SqlServerStorageOptions
				{
					CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
					SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
					QueuePollInterval = TimeSpan.Zero,
					UseRecommendedIsolationLevel = true,
					DisableGlobalLocks = true
				}));

			services.AddHangfireServer();
			services.AddInfrastructureInjection(_config);
			services.AddCoreInjection();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
		{
			if(_env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStaticFiles();

			app.UseSerilogRequestLogging();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});

			app.UseHangfireDashboard("/jobs", new DashboardOptions
			{
				Authorization = new[]
				{
					new HangfireAuthorizationFilter("admin")
				}
			});
			ConfigureHangfireJob(recurringJobManager, serviceProvider);
		}

		public void ConfigureHangfireJob(IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
		{
			using(var connection = JobStorage.Current.GetConnection())
			{
				foreach(var recurringJob in StorageConnectionExtensions.GetRecurringJobs(connection))
				{
					RecurringJob.RemoveIfExists(recurringJob.Id);
				}
			}

			string recurringJobs = _config.GetSection("HangfireJob").GetValue<string>("RecurringJobs");
			foreach(string recurringJob in recurringJobs.Split(';'))
			{
				HangfireJob hangfireJob;
				string jobName = recurringJob.Split(':')[0].Trim();
				string jobCron = recurringJob.Split(':')[1].Trim();

				if(Enum.TryParse(jobName, true, out hangfireJob))
				{
					switch(hangfireJob)
					{
						case HangfireJob.ScrapeNewReleasedMovie:
							recurringJobManager.AddOrUpdate(jobName, () => serviceProvider.GetService<IJavScrapeService>().ScrapeNewReleasedMovie(), jobCron);
							break;
						case HangfireJob.ScrapeMovieMagnet:
							recurringJobManager.AddOrUpdate(jobName, () => serviceProvider.GetService<IMagnetScrapeService>().DailyDownloadMovieMagnet(), jobCron);
							break;
						case HangfireJob.MonitorMovieDownload:
							recurringJobManager.AddOrUpdate(jobName, () => serviceProvider.GetService<IDownloadService>().MonitorMovieDownload(), jobCron);
							break;
					}
				}
			}
		}
	}
}
