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
			services.AddTransient<IMovieService, Services.MovieService>();
			services.AddTransient<IActorService, Services.ActorService>();
			services.AddTransient<ICategoryService, Services.CategoryService>();
			services.AddTransient<ICompanyService, Services.CompanyService>();
			services.AddTransient<IDirectorService, Services.DirectorService>();
			services.AddTransient<IMovieMagnetService, Services.MovieMagnetService>();
			services.AddTransient<IJavScrapeService, Services.JavScrapeService>();
			services.AddTransient<IMagnetScrapeService, Services.MagnetScrapeService>();
			services.AddTransient<IJavScrapeDailyJob, Jobs.JavScrapeDailyJob>();
			return services;
		}
	}
}
