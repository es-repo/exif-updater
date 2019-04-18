using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Diagnostics;
using System.IO;

namespace ExifUpdater
{
	public static class ImageMetadataUpdater
	{
		private static string ExifToolPath = Path.Combine(Directory.GetCurrentDirectory(), "exiftool.exe");

		public static ImageMetadataUpdateResult Update(string filePath, string title, string[] keywords)
		{
			string keywordsJoined = string.Join(", ", keywords);

			try
			{
				UpdateIptc(filePath, title, keywordsJoined);
			}
			catch (Exception ex)
			{
				return new ImageMetadataUpdateResult
				{
					Success = false,
					Error = $"Can't update IPTC metadata. Error: {ex.Message}"
				};
			}

			try
			{
				UpdateExif(filePath, title, keywordsJoined);
			}
			catch (Exception ex)
			{
				return new ImageMetadataUpdateResult
				{
					Success = false,
					Error = $"Can't update EXIF metadata: {ex.Message}"
				};
			}

			return new ImageMetadataUpdateResult { Success = true };
		}

		private static void UpdateExif(string filePath, string title, string keywords)
		{
			var file = ShellFile.FromFilePath(filePath);

			using (ShellPropertyWriter propertyWriter = file.Properties.GetPropertyWriter())
			{
				propertyWriter.WriteProperty(SystemProperties.System.Title, title);
				propertyWriter.WriteProperty(SystemProperties.System.Subject, title);
				propertyWriter.WriteProperty(SystemProperties.System.Keywords, keywords);
				propertyWriter.WriteProperty(SystemProperties.System.Comment, keywords);
				
				propertyWriter.Close();
			}
		}

		private static void UpdateIptc(string filePath, string title, string keywords)
		{
			Process p = new Process();
			p.StartInfo = new ProcessStartInfo(ExifToolPath);
			p.StartInfo.CreateNoWindow = true;
			p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			p.StartInfo.Arguments = $"-q -overwrite_original -iptc:caption-abstract=\"{title}\" -iptc:keywords=\"{keywords}\" \"{filePath}\"";
			p.Start();
			p.WaitForExit(30000);
		}
	}
}
