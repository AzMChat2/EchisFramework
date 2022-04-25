using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Serialization;

namespace System.Scheduler.Schedules
{

	/// <summary>
	/// Represents a Schedule which runs at a set time each day.
	/// </summary>
	public class DailySchedule : DailySchedule<object> { }

	/// <summary>
	/// Represents a Schedule which runs at a set time each day.
	/// </summary>
	public class DailySchedule<T> : Schedule<T>
	{
		/// <summary>
		/// Gets or sets the time of day to run.
		/// </summary>
		[XmlAttribute]
		[SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "ToRun")]
		public DateTime TimeToRun { get; set; }

		/// <summary>
		/// Determines the next scheduled run time.
		/// </summary>
		/// <returns></returns>
		protected override DateTime CalculateNextRun()
		{
			if ((LastRun.Date < DateTime.Today) ||
				((LastRun.Date == DateTime.Today) && (DateTime.Now.TimeOfDay < TimeToRun.TimeOfDay)))
			{
				return DateTime.Today.Add(TimeToRun.TimeOfDay);
			}
			else
			{
				return LastRun.Date.AddDays(1).Add(TimeToRun.TimeOfDay);
			}
		}
	}
}
