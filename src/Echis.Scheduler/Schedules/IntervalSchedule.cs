using System;
using System.Xml;
using System.Xml.Serialization;

namespace System.Scheduler.Schedules
{

	/// <summary>
	/// Represents a Schedule based on an Interval (i.e. Every 5 minutes)
	/// </summary>
	public class IntervalSchedule : IntervalSchedule<object> { }

	/// <summary>
	/// Represents a Schedule based on an Interval (i.e. Every 5 minutes)
	/// </summary>
	public class IntervalSchedule<T> : Schedule<T>
	{
		/// <summary>
		/// Contains defaults used by the IntervalSchedule class.
		/// </summary>
		private static class Defaults
		{
			/// <summary>
			/// The default interval.
			/// </summary>
      public readonly static DateTime Interval = new DateTime(1, 1, 1, 0, 5, 0); // 5 Minutes
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public IntervalSchedule()
		{
			Interval = Defaults.Interval;
		}

		/// <summary>
		/// Gets or sets the interval at which the schedule runs.
		/// </summary>
    [XmlAttribute]
		public DateTime Interval { get; set; }

		/// <summary>
		/// Determines the next run time.
		/// </summary>
		/// <returns></returns>
		protected override DateTime CalculateNextRun()
		{
			DateTime retVal = LastRun.Add(Interval.TimeOfDay);

			while (retVal < DateTime.Now)
			{
				retVal = retVal.Add(Interval.TimeOfDay);
			}

			return retVal;
		}
	}
}
