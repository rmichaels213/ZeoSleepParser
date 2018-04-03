/* ----------------------------------------
Filename:		MainPage.xaml.cs
Create Date:	03/30/2018
Author:			Ryan Michaels
Update Date:	04/02/2018
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
		private List<ZeoNight> data;

		public MainPage()
		{
			this.InitializeComponent();

			header = new List<string>();
			data = new List<ZeoNight>();

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
			bool isNewLineFound = false;

			int fileLength = fileText.Length;

			int lastPosition = 0;
			int position = 0;
			int columnNumber = 0;

			int dataRow = 0;
			int wordCount = 0;

			ZeoNight localZeoNight = new ZeoNight();

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
						isNewLineFound = true;
					}
				}

				if (fileText[position] == DELIMETER || isNewLineFound )
				{
					textError.Text = "Found delimeter!";

					if ( !isHeaderFound )
					{
						// Keep adding to header record if we haven't found the end yet
						header.Add(fileText.Substring(lastPosition, position - lastPosition));
					}
					else
					{
						if ( columnNumber >= header.Count )
						{
							// This is bad
							continue;
						}

						string currentColumn = header[columnNumber];
						string currentValue = fileText.Substring(lastPosition, position - lastPosition);

						// Here is a data record, save it if we need it.
						if (header[columnNumber] == "Sleep Date")
						{
							localZeoNight.SleepDate = Convert.ToDateTime(fileText.Substring(lastPosition, position - lastPosition));
						}
						if (header[columnNumber] == "Start of Night")
						{
							DateTime localDateTime = Convert.ToDateTime(fileText.Substring(lastPosition, position - lastPosition));
							localZeoNight.StartTime = localDateTime.TimeOfDay;
						}
						if (header[columnNumber] == "Detailed Sleep Graph")
						{
							localZeoNight.RawReadings = fileText.Substring(lastPosition, position - lastPosition);
						}

						//data[dataRow].Add(fileText.Substring(lastPosition, position - lastPosition));
					}

					// Swap positions
					position++;
					lastPosition = position;
					
					columnNumber++;

					if ( isNewLineFound )
					{
						if (isHeaderFound)
						{
							data.Add(localZeoNight);
							localZeoNight = new ZeoNight();

							dataRow++;
						}

						// Account for newline/return characters
						position += 3;
						lastPosition = position - 1;

						isHeaderFound = true;
						columnNumber = 0;
						isNewLineFound = false;
					}

					continue;
				}

				position++;
			}

			textError.Text = "Parsing done!";

			// Read out a summary
			foreach ( ZeoNight night in data )
			{
				night.ToString();
			}
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
	}
}
