using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using System.Reflection;
using Dapper.Contrib.Extensions;

namespace MovieManager.Infrastructure.DbContext
{
	public class DapperContext
	{
		private readonly IConfiguration _configuration;
		private readonly string connectionString;
		private IDbConnection GetConnection() => new SqlConnection(connectionString);

		public DapperContext(IConfiguration configuration)
		{
			_configuration = configuration;
			connectionString = configuration.GetConnectionString("MovieManagerDB");
		}

		protected static string GetDescriptionFromAttribute(MemberInfo member)
		{
			if(member == null) return null;

			var attrib = (DescriptionAttribute)Attribute.GetCustomAttribute(member, typeof(DescriptionAttribute), false);
			return (attrib?.Description ?? member.Name).ToLower();
		}

		protected void DoCustomMap<T>()
		{
			var map = new CustomPropertyTypeMap(typeof(T), (type, columnName)
					=> type.GetProperties().FirstOrDefault(prop => GetDescriptionFromAttribute(prop) == columnName.ToLower()));
			Dapper.SqlMapper.SetTypeMap(typeof(T), map);
		}

		#region async
		protected async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null)
		{
			using(var conn = GetConnection())
			{
				return await conn.QuerySingleOrDefaultAsync<T>(sql, param);
			}
		}

		protected async Task<T> QuerySingleAsync<T>(string sql, object param = null)
		{
			using(var conn = GetConnection())
			{
				return await conn.QuerySingleAsync<T>(sql, param);
			}
		}

		protected async Task<T> QueryFirstAsync<T>(string sql, object param = null)
		{
			using(var conn = GetConnection())
			{
				return await conn.QueryFirstAsync<T>(sql, param);
			}
		}

		protected async Task<List<T>> QueryAsync<T>(string sql, object param = null)
		{
			using(var conn = GetConnection())
			{
				var result = await conn.QueryAsync<T>(sql, param);

				return result.ToList();
			}
		}

		protected async Task<int> ExecuteAsync(string sql, object param = null)
		{
			using(var conn = GetConnection())
			{
				return await conn.ExecuteAsync(sql, param);
			}
		}

		protected async Task QueryMultipleAsync(string sql, Action<SqlMapper.GridReader> map, object param = null)
		{
			using(var conn = GetConnection())
			{
				var result = await conn.QueryMultipleAsync(sql, param);

				map(result);
			}
		}
		#endregion

		#region sync
		protected T QuerySingleOrDefault<T>(string sql, object param = null)
		{
			using(var conn = GetConnection())
			{
				DoCustomMap<T>();
				return conn.QuerySingleOrDefault<T>(sql, param);
			}
		}

		protected T QuerySingle<T>(string sql, object param = null)
		{
			using(var conn = GetConnection())
			{
				return conn.QuerySingle<T>(sql, param);
			}
		}

		protected List<T> Query<T>(string sql, object param = null)
		{
			using(var conn = GetConnection())
			{
				DoCustomMap<T>();
				var result = conn.Query<T>(sql, param);
				return result.ToList();
			}
		}

		protected int Execute(string sql, object param = null)
		{
			using(var conn = GetConnection())
			{
				return conn.Execute(sql, param);
			}
		}

		protected void QueryMultiple(string sql, Action<SqlMapper.GridReader> map, object param = null)
		{
			using(var conn = GetConnection())
			{
				var reader = conn.QueryMultiple(sql, param);
				map(reader);
			}
		}

		protected void BatchInsert(DataTable dt)
		{
			using(SqlConnection conn = (SqlConnection)GetConnection())
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

		public long InsertEntity(object entity)
		{
			using(var conn = GetConnection())
			{
				return conn.Insert(entity);
			}
		}

		public bool UpdateEntity(object entity)
		{
			using(var conn = GetConnection())
			{
				return conn.Update(entity);
			}
		}
		#endregion
	}
}