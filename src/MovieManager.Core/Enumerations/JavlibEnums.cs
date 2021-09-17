using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Enumerations
{
	public class JavlibEnums
	{
		public enum EntryPoint : short
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
            Scan
        }
	}
}
