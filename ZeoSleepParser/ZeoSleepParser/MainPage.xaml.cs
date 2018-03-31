/* ----------------------------------------
Filename:		MainPage.xaml.cs
Create Date:	03/30/2018
Author:			Ryan Michaels
Update Date:	03/30/2018
---------------------------------------- */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ZeoSleepParser
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{

		private Windows.Storage.StorageFile fileToParse;

		public MainPage()
		{
			this.InitializeComponent();

			// Start things up!
			StartMainApp();
		}

		public void StartMainApp()
		{
			textError.Text = "This app has started.";
		}

		/// <summary>
		/// Open a file browser and select a file to parse.
		/// </summary>
		/// <param name="sender">Sending object.</param>
		/// <param name="e">Event arguments.</param>
		private async void buttonBrowse_Click(object sender, RoutedEventArgs e)
		{
			var picker = new Windows.Storage.Pickers.FileOpenPicker();
			picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
			picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
			picker.FileTypeFilter.Add(".csv");

			Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
			if (file != null)
			{
				textError.Text = "Selected file: " + file.Name;
				fileToParse = file;
				textParse.Text = string.Empty;
			}
			else
			{
				textError.Text = "Operation cancelled.";
				fileToParse = null;
				textParse.Text = string.Empty;
			}
		}

		/// <summary>
		/// Parse selected file.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void ButtonParse_Click(object sender, RoutedEventArgs e)
		{
			if (fileToParse == null )
			{
				textError.Text = "No file selected.";
				textParse.Text = string.Empty;
				return;
			}

			string fileText = await Windows.Storage.FileIO.ReadTextAsync(fileToParse);
			textParse.Text = fileText;
		}
	}
}
