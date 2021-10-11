using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Enumerations
{
	//TODO: Add constraint&Def to every table in DB
	public enum MovieStatus : short
	{
		NotScanned = 0,
		Scanned,
		HasTorrent,
		Downloading,
		Downloaded,
		Finished,
		InError
	}

	//TODO: Add constraint&Def to every table in DB
	public enum MagnetStatus : short
	{
		Dead = 0,
		IsReady,
		Downloading,
		Seeding,
		Finished
	}

	public enum MagnetSource : short
	{
		Javbus = 1,
		Sukebei
	}
}
