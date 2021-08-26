using Microsoft.Extensions.Configuration;
using Dapper;
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

		#region async
		protected async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null)
		{
			using(var conn = NewConnection())
			{
				return await conn.QuerySingleOrDefaultAsync<T>(sql, param);
			}
		}

		protected async Task<T> QuerySingleAsync<T>(string sql, object param = null)
		{
			using(var conn = NewConnection())
			{
				return await conn.QuerySingleAsync<T>(sql, param);
			}
		}

		protected async Task<T> QueryFirstAsync<T>(string sql, object param = null)
		{
			using(var conn = NewConnection())
			{
				return await conn.QueryFirstAsync<T>(sql, param);
			}
		}

		protected async Task<List<T>> QueryAsync<T>(string sql, object param = null)
		{
			using(var conn = NewConnection())
			{
				var result = await conn.QueryAsync<T>(sql, param);

				return result.ToList();
			}
		}

		protected async Task<int> ExecuteAsync(string sql, object param = null)
		{
			using(var conn = NewConnection())
			{
				return await conn.ExecuteAsync(sql, param);
			}
		}

		protected async Task QueryMultipleAsync(string sql, Action<SqlMapper.GridReader> map, object param = null)
		{
			using(var conn = NewConnection())
			{
				var result = await conn.QueryMultipleAsync(sql, param);

				map(result);
			}
		}
		#endregion

		#region sync
		protected T QuerySingleOrDefault<T>(string sql, object param = null)
		{
			using(var conn = NewConnection())
			{
				return conn.QuerySingleOrDefault<T>(sql, param);
			}
		}

		protected T QuerySingle<T>(string sql, object param = null)
		{
			using(var conn = NewConnection())
			{
				return conn.QuerySingle<T>(sql, param);
			}
		}

		protected List<T> Query<T>(string sql, object param = null)
		{
			using(var conn = NewConnection())
			{
				var result = conn.Query<T>(sql, param);

				return result.ToList();
			}
		}

		protected int Execute(string sql, object param = null)
		{
			using(var conn = NewConnection())
			{
				return conn.Execute(sql, param);
			}
		}

		protected void QueryMultiple(string sql, Action<SqlMapper.GridReader> map, object param = null)
		{
			using(var conn = NewConnection())
			{
				var reader = conn.QueryMultiple(sql, param);
				map(reader);
			}
		}

		protected void BatchInsert(DataTable dt)
		{
			using(SqlConnection conn = (SqlConnection)NewConnection())
			{
				SqlBulkCopy bulkCopy = new(conn);
				bulkCopy.DestinationTableName = "ReportItem";
				bulkCopy.BatchSize = dt.Rows.Count;
				conn.Open();
				if(dt != null && dt.Rows.Count != 0)
				{
					bulkCopy.WriteToServer(dt);
				}
			}
		}
		#endregion
	}
}