using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;

namespace ExifUpdater
{
	/// <summary>
	/// Interaction logic for FolderEntry.xaml
	/// </summary>
	public partial class FolderInput : System.Windows.Controls.UserControl
	{
		public static DependencyProperty TextProperty = DependencyProperty.Register("FolderPath", typeof(string), typeof(FolderInput), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public string FolderPath { get { return GetValue(TextProperty) as string; } set { SetValue(TextProperty, value); } }

		public event EventHandler FolderSelected;

		public FolderInput() { InitializeComponent(); }

		private void OnTextChanged(object sender, EventArgs e)
		{
			var textBox = (System.Windows.Controls.TextBox)sender;
			FolderPath = textBox.Text;
			FolderSelected?.Invoke(this, EventArgs.Empty);
		}

		private void Browse(object sender, RoutedEventArgs e)
		{
			using (FolderBrowserDialog dlg = new FolderBrowserDialog())
			{
				dlg.SelectedPath = FolderPath;
				dlg.ShowNewFolderButton = false;
				DialogResult result = dlg.ShowDialog();
				if (result == DialogResult.OK)
				{
					FolderPath = dlg.SelectedPath;
					BindingExpression be = GetBindingExpression(TextProperty);
					if (be != null)
						be.UpdateSource();
				}
			}
		}
	}
}
