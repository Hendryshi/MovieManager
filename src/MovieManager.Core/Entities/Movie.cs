using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

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
		public string Star { get; set; }
		public DateTime? DtRelease { get; set; }
		public int? Duration { get; set; }
		public DateTime DtUpdate { get; set; }
		public string PictureUrl { get; set; }
		public string Url { get; set; }
		public int IdStatus { get; set; }

		[Computed]
		public List<MovieRelation> MovieRelations { get; set; }
	}
}
