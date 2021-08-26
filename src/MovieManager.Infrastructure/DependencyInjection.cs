using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MovieManager.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructureInjection(this IServiceCollection services)
		{
			services.AddTransient<Repositories.BaseRepository>();
			return services;
		}
	}
}
