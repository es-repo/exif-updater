using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExifUpdater
{
	public static class InputMetataDataReader
	{
		public static IEnumerable<InputMetataDataRecord> Read(string filePath)
		{
			if (!File.Exists(filePath))
				yield break;

			string[] lines = File.ReadAllLines(filePath);
			int index = 0;
			while (index < lines.Length)
			{
				string fileNamesRange;
				string title;
				string keywordsJoined;

				GetNextRawRecord(lines, ref index, out fileNamesRange, out title, out keywordsJoined);

				if (fileNamesRange != null)
				{
					string[] fileNames = GetFileNames(fileNamesRange).ToArray();
					foreach(string f in fileNames)
					{
						string[] keywords = string.IsNullOrWhiteSpace(keywordsJoined) 
							? new string[] { } 
							: keywordsJoined.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
								.Select(s => s.Trim())
								.Where(s => s != "")
								.Distinct()
								.ToArray();

						yield return new InputMetataDataRecord
						{
							FileName = f,
							Title = title?.Trim() ?? "",
							Keywords = keywords
						};
					}
				}
			}
		}

		private static IEnumerable<string> GetFileNames(string fileNamesRange)
		{
			string[] parts = fileNamesRange.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
			parts = parts.Select(f => f.Trim()).Where(f => !string.IsNullOrEmpty(f)).ToArray();

			if (parts.Length == 0)
				yield break;

			if (parts.Length == 1)
			{
				yield return parts[0];
				yield break;
			}

			int rangeStart = int.Parse(parts[0]);
			int rangeEnd = int.Parse(parts[1]);

			int len = parts[0].Length;

			for(int i = rangeStart; i <= rangeEnd; i++)
			{
				yield return i.ToString().PadLeft(len, '0');
			}
		}

		private static void GetNextRawRecord(
			string[] lines, ref int index,
			out string fileNameRange,
			out string title,
			out string keywords)  
		{
			fileNameRange = null;
			title = null;
			keywords = null;

			while(index < lines.Length)
			{
				string l = lines[index].Trim();
				if (!string.IsNullOrEmpty(l))
				{
					if (fileNameRange == null)
					{
						fileNameRange = l;
					}
					else if (title == null)
					{
						title = l;
					}
					else
					{
						keywords = l;
						index++;
						return;
					}
				}

				index++;
			}
		}
	}
}
