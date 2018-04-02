/* ----------------------------------------
Filename:		ZeoNight.cs
Create Date:	04/02/2018
Author:			Ryan Michaels
Update Date:	04/02/2018
---------------------------------------- */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// A class to hold specific data for Zeo nights.
/// </summary>
namespace ZeoSleepParser
{
	class ZeoNight
	{
		private DateTime _sleepDate;
		private TimeSpan _startTime;
		private string _rawReadings;

		/// <summary>
		/// Class constructor.
		/// </summary>
		public ZeoNight()
		{
			_sleepDate = DateTime.Now;
			_startTime = TimeSpan.Zero;
			_rawReadings = string.Empty;
		}

		/// <summary>
		/// Class constructor.
		/// </summary>
		/// <param name="sleepDate">Date for this night of sleep.</param>
		/// <param name="startTime">Start time of the sleep.</param>
		/// <param name="rawReadings">Raw readings for the night of sleep.</param>
		public ZeoNight(DateTime sleepDate, TimeSpan startTime, string rawReadings)
		{
			_sleepDate = sleepDate;
			_startTime = startTime;
			_rawReadings = rawReadings;
		}

		/// <summary>
		/// Gets or sets the sleep date property.
		/// </summary>
		public DateTime SleepDate
		{
			get
			{
				return _sleepDate;
			}
			set
			{
				_sleepDate = value;
			}
		}

		/// <summary>
		/// Gets or sets the start time property.
		/// </summary>
		public TimeSpan StartTime
		{
			get
			{
				return _startTime;
			}
			set
			{
				_startTime = value;
			}
		}

		/// <summary>
		/// Gets or sets the start time property.
		/// </summary>
		public string RawReadings
		{
			get
			{
				return _rawReadings;
			}
			set
			{
				_rawReadings = value;
			}
		}
	}
}
