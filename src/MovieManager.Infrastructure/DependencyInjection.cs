using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Settings;

namespace MovieManager.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructureInjection(this IServiceCollection services, IConfiguration _config)
		{
			services.Configure<JavlibSettings>(_config.GetSection("JavlibSettings"));
			services.Configure<CommonSettings>(_config.GetSection("CommonSettings"));
			services.Configure<MagnetSettings>(_config.GetSection("MagnetSettings"));
			services.Configure<QbittorrentSettings>(_config.GetSection("QbittorrentSettings"));
			services.Configure<LocalFileSettings>(_config.GetSection("LocalFileSettings"));

			services.AddScoped<DbContext.DapperContext>();
			services.AddScoped(typeof(IAppLogger<>), typeof(Logging.LoggerAdapter<>));
			services.AddTransient<IHtmlService, Services.HtmlService>();
			services.AddTransient<IQbittorrentService, Services.QbittorrentService>();
			services.AddTransient<ILocalFileService, Services.LocalFileService>();
			services.AddTransient<IEmailSender, Services.EmailSender>();
			services.AddTransient<IMovieRelationRepo, Repositories.MovieRelationRepo>();
			services.AddTransient<IMovieRepo, Repositories.MovieRepo>();
			services.AddTransient<IActorRepo, Repositories.ActorRepo>();
			services.AddTransient<ICategoryRepo, Repositories.CategoryRepo>();
			services.AddTransient<ICompanyRepo, Repositories.CompanyRepo>();
			services.AddTransient<IDirectorRepo, Repositories.DirectorRepo>();
			services.AddTransient<IPublisherRepo, Repositories.PublisherRepo>();
			services.AddTransient<IMovieMagnetRepo, Repositories.MovieMagnetRepo>();
			services.AddTransient<IMovieHistoryRepo, Repositories.MovieHistoryRepo>();
			services.AddTransient<IScrapeReportRepo, Repositories.ScrapeReportRepo>();
			return services;
		}
	}
}
