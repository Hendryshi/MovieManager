using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Enumerations
{
	public enum JavlibEntryType : short
	{
		NewRelease = 1,
		Category,
		Actress,
		Director,
		Company,
		Publisher,
		BestRate,
		MostWanted,
		Update,
		Rank,
		Other,
		Search,
		Movie,
		Scan
	}

	//TODO: Add constraint to every table in DB
	public enum JavlibFavLevel : short
	{
		NotInterest = 0,
		DlTorrent,
		DlMovie,
		DlChineseSub
	}

	public enum JavlibRoleType : short
	{
		Category = 1,
		Company,
		Director,
		Publisher,
		Actor
	}
}
