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
using MovieManager.Infrastructure;
using Serilog;


namespace MovieManagerWeb
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			services.AddHangfire(config =>
				config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
				.UseSimpleAssemblyNameTypeSerializer()
				.UseDefaultTypeSerializer()
				.UseMemoryStorage());

			services.AddHangfireServer();

			services.AddCoreInjection();
			services.AddInfrastructureInjection();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
		{
			if(env.IsDevelopment())
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

			app.UseHangfireDashboard();
			ConfigureHangfireJob(recurringJobManager, serviceProvider);
		}

		public void ConfigureHangfireJob(IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider)
		{
			//BackgroundJob.Enqueue(() => Console.WriteLine("Hello Hangfire job !"));
			//recurringJobManager.AddOrUpdate("Run every minute", () => serviceProvider.GetService<MovieManager.Core.Job.ITestJob>().Run(), "* * * * * ");
			BackgroundJob.Enqueue(() => serviceProvider.GetService<MovieManager.Core.Interfaces.IJavScrapeDailyJob>().Run());
		}
	}
}
