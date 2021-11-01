using MovieManager.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.Core.Helper
{
	public class HistoryDiffHelpers
	{
		public static bool GetDifferencesFieldString(string fieldName, string field1, string field2, ref string result)
		{
			bool hasChanged = false;
			result = string.Empty;
			if(!(string.IsNullOrEmpty(field1) && string.IsNullOrEmpty(field2)))
			{
				if(!string.Equals(field1, field2))
				{
					string changedFrom = string.IsNullOrEmpty(field1) ? string.Empty : field1.Truncate(200);
					string changedTo = string.IsNullOrEmpty(field2) ? string.Empty : field2.Truncate(200);

					result = string.Format("{0} has been changed from \"{1}\" to \"{2}\"", fieldName, changedFrom, changedTo);
					hasChanged = true;
				}
			}
			return hasChanged;
		}

		public static bool GetDifferencesFieldDate(string fieldName, DateTime? field1, DateTime? field2, ref string result)
		{
			bool hasChanged = false;
			result = string.Empty;
			if(field1.HasValue || field2.HasValue)
			{
				string changedFrom = field1.HasValue ? field1.Value.ToString("u", DateTimeFormatInfo.InvariantInfo) : string.Empty;
				string changedTo = field2.HasValue ? field2.Value.ToString("u", DateTimeFormatInfo.InvariantInfo) : string.Empty;

				if(!string.Equals(changedFrom, changedTo))
				{
					result = string.Format("{0} has been changed from \"{1}\" to \"{2}\"", fieldName, changedFrom, changedTo);
					hasChanged = true;
				}
			}
			return hasChanged;
		}

		public static bool GetDifferencesFieldDemical(string fieldName, decimal? field1, decimal? field2, ref string result)
		{
			bool hasChanged = false;
			result = string.Empty;
			if(field1.HasValue || field2.HasValue)
			{
				decimal changedFrom = field1.HasValue ? field1.Value : 0;
				decimal changedTo = field2.HasValue ? field2.Value : 0;

				if(!string.Equals(changedFrom, changedTo))
				{
					result = string.Format("{0} has been changed from \"{1}\" to \"{2}\"", fieldName, changedFrom, changedTo);
					hasChanged = true;
				}
			}
			return hasChanged;
		}

		public static bool GetDifferencesFieldInteger(string fieldName, int? field1, int? field2, ref string result)
		{
			bool hasChanged = false;
			result = string.Empty;
			if(field1.HasValue || field2.HasValue)
			{
				int changedFrom = field1.HasValue ? field1.Value : 0;
				int changedTo = field2.HasValue ? field2.Value : 0;

				if(!string.Equals(changedFrom, changedTo))
				{
					result = string.Format("{0} has been changed from \"{1}\" to \"{2}\"", fieldName, changedFrom, changedTo);
					hasChanged = true;
				}
			}
			return hasChanged;
		}

		public static bool GetDifferencesFieldBoolean(string fieldName, bool? field1, bool? field2, ref string result)
		{
			bool hasChanged = false;
			result = string.Empty;
			if(field1.HasValue || field2.HasValue)
			{
				string changedFrom = field1.HasValue ? field1.Value.ToYesNoString() : string.Empty;
				string changedTo = field2.HasValue ? field2.Value.ToYesNoString() : string.Empty;

				if(!string.Equals(changedFrom, changedTo))
				{
					result = string.Format("{0} has been changed from \"{1}\" to \"{2}\"", fieldName, changedFrom, changedTo);
					hasChanged = true;
				}
			}
			return hasChanged;
		}
	}
}
