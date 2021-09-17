using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MovieManager.Core.Interfaces;

namespace MovieManager.Core
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddCoreInjection(this IServiceCollection services)
		{
			services.AddTransient<IJavScrapeDailyJob, Jobs.JavScrapeDailyJob>();
			services.AddTransient<IMovieService, Services.MovieService>();
			services.AddTransient<IJavScrapeService, Services.JavScrapeService>();
			return services;
		}
	}
}
