using Dapper.Contrib.Extensions;
using MovieManager.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Entities
{
	[Table("J_MovieMagnet")]
	public class MovieMagnet : BaseEntity
	{
		[Key]
		public int IdMovieMag { get; set; }
		public int IdMovie { get; set; }
		public string MovieNumber { get; set; }
		public string MagName { get; set; }
		public string MagnetUrl { get; set; }
		public string Hash { get; set; }
		public decimal Size { get; set; }
		public DateTime? DtMagnet { get; set; }
		public bool IsHD { get; set; }
		public bool HasSub { get; set; }
		public MagnetSource IdMagSource { get; set; } = MagnetSource.Javbus;
		public DateTime? DtStart { get; set; }
		public DateTime? DtFinish { get; set; }
		public string SavePath { get; set; }
		public MagnetStatus IdStatus { get; set; } = MagnetStatus.IsReady;
		public DateTime DtUpdate { get; set; }

		public string GenerateHash()
		{
			if(MagnetUrl != null)
				foreach(var item in MagnetUrl?.Split("&"))
				{
					if(item.Contains("btih:"))
						Hash = item.Substring(item.IndexOf("btih:") + 5);
				}

			return Hash;
		}

		
	}
}
