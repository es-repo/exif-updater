using System;
using System.Linq;

namespace ExifUpdater
{
	public static class KeywordsUtils
	{
		public static int CountJoined(string keywordsJoined)
		{
			if (keywordsJoined == null)
				return 0;

			string[] kewords = Split(keywordsJoined);
			return kewords.Length;
		}

		public static string[] Split(string keywordsJoined)
		{
			return keywordsJoined.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(k => k.Trim())
				.Where(k => k != "")
				.ToArray();
		}

		public static string Join(string[] keywords)
		{
			return string.Join(", ", keywords);
		}

		public static string[] SortAndRemoveDuplicates(string[] keywords)
		{
			return keywords.OrderBy(o => o).Distinct().ToArray();
		}

		public static string SortAndRemoveDuplicatesJoined(string keywordsJoined)
		{
			string[] kewords = Split(keywordsJoined);
			string[] kewordsSorted = SortAndRemoveDuplicates(kewords);
			return Join(kewordsSorted);
		}

		public static string ConcatJoined(string sourceJoined, string addJoined)
		{
			string[] source = Split(sourceJoined);
			string[] add = Split(addJoined);
			var concated = source.Concat(add).ToArray();
			return Join(concated);
		}

		public static string NormilizeJoined(string keywordsJoined)
		{
			string[] kewords = Split(keywordsJoined);
			return Join(kewords);
		}
	}
}
