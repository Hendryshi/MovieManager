using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Extensions
{
	public static class BooleanExtension
	{
		public static string ToYesNoString(this bool value)
		{
			return value ? "Yes" : "No";
		}
	}
}
