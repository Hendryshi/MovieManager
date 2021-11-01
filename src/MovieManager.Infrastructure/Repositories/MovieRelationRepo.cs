using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieManager.Core.Interfaces;
using MovieManager.Core.Entities;
using MovieManager.Infrastructure.DbContext;

namespace MovieManager.Infrastructure.Repositories
{
	public class MovieRelationRepo : IMovieRelationRepo
	{
		private readonly DapperContext db;
		private readonly string Table = "J_MovieRelation";

		public MovieRelationRepo(DapperContext dbContext)
		{
			db = dbContext;
		}

		public List<MovieRelation> LoadAllRelations(int idMovie)
		{
			var sql = $"SELECT * FROM {Table} WHERE idMovie = @idMovie";
			return db.Query<MovieRelation>(sql, new { idMovie });
		}

		public void SaveAllRelations(int idMovie, List<MovieRelation> movieRelations)
		{
			var sql = new StringBuilder();
			sql.AppendLine($"MERGE INTO {Table} AS Target");
			sql.AppendLine("USING(");

			if(movieRelations.Count > 0)
			{
				sql.AppendLine("	VALUES");
				sql.AppendLine(string.Join(",", movieRelations.Select(mr => $"({idMovie}, {(short)mr.IdTyRole}, {mr.IdRelation})")));
			}
			else
				sql.AppendLine("	SELECT 0, 0, 0 WHERE 1=0");

			sql.AppendLine(") AS Source(idMovie, idTyRole, idRelation)");
			sql.AppendLine("ON Source.idMovie = Target.idMovie AND Source.idTyRole = Target.idTyRole AND Source.idRelation = Target.idRelation");
			sql.AppendLine("WHEN NOT MATCHED");
			sql.AppendLine("THEN INSERT (idMovie, idTyRole, idRelation)");
			sql.AppendLine("VALUES (Source.idMovie, Source.idTyRole, Source.idRelation)");
			sql.AppendLine($"WHEN NOT MATCHED BY SOURCE AND Target.idMovie = {idMovie}");
			sql.AppendLine("THEN DELETE;");

			db.Execute(sql.ToString());
		}
	}
}
