using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MovieManager.Core
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddCoreInjection(this IServiceCollection services)
		{
			services.AddSingleton<Interfaces.IJavScrapeDailyJob, Jobs.JavScrapeDailyJob>();
			return services;
		}
	}
}
