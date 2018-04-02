/* ----------------------------------------
Filename:		MainPage.xaml.cs
Create Date:	03/30/2018
Author:			Ryan Michaels
Update Date:	03/30/2018
---------------------------------------- */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

		private List<string> header;
		private List<string[]> data;

		public MainPage()
		{
			this.InitializeComponent();

			header = new List<string>();
			data = new List<string[]>();

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
			const char DELIMETER = ',';
			const char NEWLINE_DELIMETER = '\n';
			const char CARRIAGE_RETURN_DELIMETER = '\r';

			textError.Text = "Loading file.";
			string fileText = await ReadFileAsync();
			textError.Text = "File loaded.";

			if (string.IsNullOrWhiteSpace(fileText))
			{
				textError.Text = "Invalid file.";
				return;
			}

			bool isHeaderFound = false;

			int fileLength = fileText.Length;

			int lastPosition = 0;
			int position = 0;

			int dataRow = 0;
			int wordCount = 0;


			textError.Text = "Starting to parse file.";
			while ( position < fileLength)
			{
				// Find a new line
				if (fileText[position] == CARRIAGE_RETURN_DELIMETER)
				{
					// Check next character
					if (fileText[position + 1] == NEWLINE_DELIMETER)
					{
						textError.Text = "Newline found";
						isHeaderFound = true;

						// Move the data row to the next
						dataRow++;
						position++;

						continue;
					}
				}

				if (fileText[position] == DELIMETER)
				{
					textError.Text = "Found delimeter!";
					if ( !isHeaderFound )
					{
						// Keep adding to header record if we haven't found the end yet
						header.Add(fileText.Substring(lastPosition, position - lastPosition));
					}
					else
					{
						//data[dataRow].Add(fileText.Substring(lastPosition, position - lastPosition));
					}

					// Swap positions
					lastPosition = position + 1;
					wordCount++;
				}

				position++;
			}

			textError.Text = "Parsing done!";
		}
		
		/// <summary>
		/// Asynchronous method to read in file
		/// </summary>
		private async Task<string> ReadFileAsync()
		{
			if (fileToParse == null)
			{
				textError.Text = "No file selected.";
				textParse.Text = string.Empty;
				return string.Empty;
			}

			return await Windows.Storage.FileIO.ReadTextAsync(fileToParse);
		}

		private void UpdateParseText()
		{
			int parseTextPosition = 0;

			switch (parseTextPosition)
			{
				case 0:
					textError.Text = "Parsing";
					break;
				case 1:
					textError.Text = "Parsing.";
					break;
				case 2:
					textError.Text = "Parsing..";
					break;
				case 3:
				default:
					textError.Text = "Parsing...";
					break;
			}

			parseTextPosition = parseTextPosition == 3 ? 0 : parseTextPosition++;
		}
	}
}
