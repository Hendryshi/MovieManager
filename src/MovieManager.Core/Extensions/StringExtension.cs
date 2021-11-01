using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Extensions
{
	public static class StringExtension
	{
		public static string ReplaceInvalidChar(this string str)
		{
			return str.Replace("'", "").Replace("?", "").Replace(":", "").Replace("*", "").Replace("|", "").Replace("\\", "").Replace("/", "").Replace("<", "").Replace(">", "").Replace(" （ブルーレイディスク）", "").Replace("（ブルーレイディスク）", "").Replace("・", "").Replace("♪", "").Replace("´", "").Replace("′", "").Replace("｀", "").Replace("◯", "").Replace("?", "").Replace("≪", "").Replace("≫", "").Replace("｢", "").Replace("｣", "").Replace("〜", "").Replace("･", "").Replace("∀", "").Replace("○", "").Replace("～", "").Replace("♯", "").Replace("､", "").Replace("━", "").Replace("ﾟ", "").Replace("｡", "").Replace("⇒", "").Replace("⇔", "").Replace("ｷ", "").Replace("ﾀ", "");
		}

		public static string Truncate(this string str, int length)
		{
			if(str.Length > length)
				str = str.Substring(0, length) + "...";

			return str;
		}

		public static decimal GetByteSize(this string str)
		{
			decimal ret = 0;
			str = str.ToLower();

			if(str.EndsWith("mb"))
				decimal.TryParse(str.Replace("mb", ""), out ret);
			else if(str.EndsWith("gb") || str.EndsWith("gib"))
			{
				decimal.TryParse(str.Replace("gb", "").Replace("gib", ""), out ret);
				ret *= 1024;
			}
			else if(str.EndsWith("tb"))
			{
				decimal.TryParse(str.Replace("tb", ""), out ret);
				ret *= 1024 * 1024;
			}
			return ret;
		}
	}
}
