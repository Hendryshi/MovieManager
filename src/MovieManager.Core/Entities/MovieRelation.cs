using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace MovieManager.Core.Entities
{
	[Table("J_MovieRelation")]
	public class MovieRelation : BaseEntity
	{
		public int IdMovie { get; set; }
		public short IdTyRole { get; set; }
		public int IdRelation { get; set; }

	}
}
