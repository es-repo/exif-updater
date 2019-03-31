using System.IO;
using System.Windows.Controls;

namespace ExifUpdater
{
	public class TextBoxLogger
	{
		private readonly TextBox _textBox;

		public TextBoxLogger(TextBox textBox)
		{
			_textBox = textBox;
		}

		public void LogInfo(string v)
		{
			_textBox.Text += v + "\n";
		}

		public void LogError(string v)
		{
			_textBox.Text += $"\n ОШИБКА:\n{v}\n";
		}

		public void LogUpdateMetadataProgressEntry(UpdateMetadataProgressEntry v)
		{
			string fileName = Path.GetFileName(v.File);

			if (v.UpdateResult.Success)
				LogInfo($"{fileName}: OK");
			else
				LogError($"{fileName}: {v.UpdateResult.Error}");
		}
	}
}
