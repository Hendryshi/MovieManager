using Dapper.Contrib.Extensions;
using MovieManager.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Entities
{
	[Table("J_MovieHistory")]
	public class MovieHistory : BaseEntity
	{
		[Key]
		public int IdMovieHistory { get; set; }
		public int IdMovie { get; set; }
		public string DescHistory { get; set; }
		public DateTime DtCreation { get; set; }
		public bool IsActive { get; set; }
	}
}
