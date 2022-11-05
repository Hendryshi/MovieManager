using Dapper.Contrib.Extensions;
using MovieManager.Core.Enumerations;
using System;

namespace MovieManager.Core.Entities
{
	[Table("J_ScrapeReport")]
	public class ScrapeReport : BaseEntity
	{
		[Key]
		public int IdReport { get; set; }
		public DateTime DtReport { get; set; } = DateTime.Today;
		public int NbReleased { get; set; }
		public int NbInterest { get; set; }
		public int NbDownload { get; set; }
		public bool IsSent { get; set; }
	}
}
