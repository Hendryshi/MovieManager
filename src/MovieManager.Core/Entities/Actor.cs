using Dapper.Contrib.Extensions;
using MovieManager.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Entities
{
	[Table("J_Actor")]
	public class Actor : BaseEntity
	{
		[Key]
		public int IdActor { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
		public string Description { get; set; }
		public JavlibFavLevel FavLevel { get; set; }
		public DateTime DtUpdate { get; set; }
	}
}
