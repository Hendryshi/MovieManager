using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using MovieManager.Core.Enumerations;

namespace MovieManager.Core.Entities
{
	[Table("J_Movie")]
	public class Movie : BaseEntity
	{
		[Key]
		public int IdMovie { get; set; }
		public string Number { get; set; }
		public string Title { get; set; }
		public string Company { get; set; }
		public string Director { get; set; }
		public string Publisher { get; set; }
		public string Category { get; set; }
		public string Actor { get; set; }
		public DateTime? DtRelease { get; set; }
		public int? NbWant { get; set; }
		public int? NbWatched { get; set; }
		public int? NbOwned { get; set; }
		public int? Duration { get; set; }
		public string ThumbnailUrl { get; set; }
		public string CoverUrl { get; set; }
		public JavlibFavLevel FavLevel { get; set; }
		public MovieStatus IdStatus { get; set; }
		public string Url { get; set; }
		public DateTime DtUpdate { get; set; }

		[Computed]
		public List<MovieRelation> MovieRelations { get; set; } = new List<MovieRelation>();
	}
}
