using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ExifUpdater
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const string KeywordsDefaultFileName = "keywords.txt";

		private TextBoxLogger _logger;

		public string KeywordsFile { get; set; }
		
		public string[] FilesToProcess { get; private set; }

		public MainWindow()
		{
			InitializeComponent();
			_logger = new TextBoxLogger(LogTextBox);

			ContentRendered += (sender, args) =>
			{
				if (sender is Window win)
				{
					win.SizeToContent = SizeToContent.Manual;
				}
			};
		}

		private void UpdateInputUIState()
		{
			KeywordsFileInput.FilePath = KeywordsFile ?? "";
			ImagesFoundLabel.Content = $"Найдено изображений: {FilesToProcess.Length}";
			MetadataUpdateButton.IsEnabled = !string.IsNullOrWhiteSpace(KeywordsFile) && FilesToProcess.Length > 0;
			ProgressBar.Value = 0;
			FileProcessedLabel.Content = "";
		}

		private void OnFolderSelected(object sender, EventArgs e)
		{
			string folderPath = ((FolderInput)sender).FolderPath;
			FilesToProcess = FindFilesToProcess(folderPath).ToArray();
			KeywordsFile = FindKeywordsFile(folderPath);

			UpdateInputUIState();
		}

		private void OnKeywordsFileSelected(object sender, EventArgs e)
		{
			KeywordsFile = ((FileInput)sender).FilePath;
			
			UpdateInputUIState();
		}

		private async void OnMetadataUpdateClick(object sender, RoutedEventArgs e)
		{
			if (FilesToProcess == null || FilesToProcess.Length == 0)
				return;

			KeywordsFileInput.IsEnabled = false;
			FilesToProcessFolderInput.IsEnabled = false;
			ProgressBar.Maximum = FilesToProcess.Length;
			ProgressBar.Value = 0;
			MetadataUpdateButton.IsEnabled = false;
			FileProcessedLabel.Content = "";
			LogTextBox.Text = "";
			HasErrorsLabel.Visibility = Visibility.Hidden;

			var progressReport = new Progress<UpdateMetadataProgressEntry>(
				entry =>
				{
					int progress = entry.FileIndex + 1;
					ProgressBar.Value = progress;
					FileProcessedLabel.Content = $"готово {progress} из {FilesToProcess.Length} файлов";
					if (!entry.UpdateResult.Success)
						HasErrorsLabel.Visibility = Visibility.Visible;
					_logger.LogUpdateMetadataProgressEntry(entry);
				});

			await Task.Run(() => UpdateMetada(FilesToProcess, progressReport));

			KeywordsFileInput.IsEnabled = true;
			FilesToProcessFolderInput.IsEnabled = true;
		}

		private static IEnumerable<string> FindFilesToProcess(string folderPath)
		{
			return Directory.GetFiles(folderPath, "*.jpg");
		}

		private static string FindKeywordsFile(string folderPath)
		{
			string keywordsFile = Path.Combine(folderPath, KeywordsDefaultFileName);
			return File.Exists(keywordsFile) ? keywordsFile : null;
		}

		private static void UpdateMetada(string[] files, IProgress<UpdateMetadataProgressEntry> progressReport)
		{
			if (files == null)
				return;

			for(int i = 0; i < files.Length; i++)
			{
				string f = files[i];
				var result = ImageMetadataUpdater.Update(f, "My Title V2", new string[] { "My keyword 1 V2", "My keyword 2 V2" });
				UpdateMetadataProgressEntry progressEntry = new UpdateMetadataProgressEntry
				{
					FileIndex = i,
					File = f,
					UpdateResult = result
				};
				progressReport.Report(progressEntry);
			}
		}
	}
}
