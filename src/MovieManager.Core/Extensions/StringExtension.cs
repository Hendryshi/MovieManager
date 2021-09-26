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
	}
}
