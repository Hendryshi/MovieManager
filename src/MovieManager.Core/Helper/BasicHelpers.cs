using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Helper
{
	public class BasicHelpers
	{
        public static int GetInt(string instring)
        {
            return int.TryParse(instring, out var result) ? result : 0;
        }

        public static bool GetBoolean(string instring)
        {
            return bool.TryParse(instring, out var result) && result;
        }
    }
}
