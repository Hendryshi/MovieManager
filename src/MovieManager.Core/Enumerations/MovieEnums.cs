using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Enumerations
{
	public enum MovieStatus : short
	{
		NotScanned = 0,
		Scanned,
		Downloading,
		Downloaded,
		InError
	}
}
