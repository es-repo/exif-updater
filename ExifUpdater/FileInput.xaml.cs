using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;

namespace ExifUpdater
{
	/// <summary>
	/// Interaction logic for FileInput.xaml
	/// </summary>
	public partial class FileInput : System.Windows.Controls.UserControl
	{
		public static DependencyProperty FilePathProperty = DependencyProperty.Register("FilePath", typeof(string), typeof(FileInput), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public string FilePath { get { return GetValue(FilePathProperty) as string; } set { SetValue(FilePathProperty, value); } }

		//public over string IsEnabled { get { return GetValue(IsEnabledProperty) as string; } set { SetValue(IsEnabledProperty, value); } }

		public event EventHandler FileSelected;


		public FileInput()
		{
			InitializeComponent();
		}

		private void Browse(object sender, RoutedEventArgs e)
		{
			using (OpenFileDialog dlg = new OpenFileDialog())
			{
				dlg.DefaultExt = "txt";
				dlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
				dlg.Multiselect = false;
				DialogResult result = dlg.ShowDialog();
				if (result == DialogResult.OK)
				{
					FilePath = dlg.FileName;
					BindingExpression be = GetBindingExpression(FilePathProperty);
					if (be != null)
						be.UpdateSource();

					FileSelected?.Invoke(this, EventArgs.Empty);
				}
			}
		}
	}
}
