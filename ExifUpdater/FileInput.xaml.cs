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

		public event EventHandler FileSelected;

		public FileInput()
		{
			InitializeComponent();
		}

		private void OnTextChanged(object sender, EventArgs e)
		{
			var textBox = (System.Windows.Controls.TextBox)sender;
			FilePath = textBox.Text;
			FileSelected?.Invoke(this, EventArgs.Empty);
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
