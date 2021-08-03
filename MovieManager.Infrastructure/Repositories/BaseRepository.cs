using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MovieManager.Infrastructure.Repositories
{
	public class BaseRepository
	{
		private readonly IConfiguration _configuration;
		private string connectionString { get; set; }
		private IDbConnection NewConnection() => new SqlConnection(connectionString);

		public BaseRepository(IConfiguration configuration)
		{
			_configuration = configuration;
			connectionString = configuration.GetConnectionString("MovieManagerDB");
		}


	}
}
