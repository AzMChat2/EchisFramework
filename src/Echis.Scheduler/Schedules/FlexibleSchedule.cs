using System;
using System.Xml;
using System.Xml.Serialization;

namespace System.Scheduler.Schedules
{
	/// <summary>
	/// Represents a flexible schedule which can run for select days of the week, on an interval, and/or within a window of operation.
	/// </summary>
	public class FlexibleSchedule : FlexibleSchedule<object> { }

	/// <summary>
	/// Represents a flexible schedule which can run for select days of the week, on an interval, and/or within a window of operation.
	/// </summary>
	public class FlexibleSchedule<T> : Schedule<T>
	{
		/// <summary>
		/// Gets or sets the day(s) of the week for this schedule.
		/// </summary>
    [XmlAttribute]
		public DaysOfTheWeek Day { get; set; }

		/// <summary>
		/// Gets or sets the start of the operational window for this schedule.
		/// </summary>
    [XmlAttribute]
    public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets the end of the operation window for this schedule.
		/// </summary>
    [XmlAttribute]
    public DateTime EndTime { get; set; }

		/// <summary>
		/// Gets or sets the interval upon which this schedule runs.
		/// </summary>
    [XmlAttribute]
    public DateTime Interval { get; set; }

		/// <summary>
		/// Calculates the next run time for this schedule.
		/// </summary>
		/// <returns></returns>
    protected override DateTime CalculateNextRun()
		{
			DateTime retVal = GetNextDate(LastRun, Day);
			if (retVal == LastRun.Date)
			{
				if (LastRun < LastRun.Date.Add(StartTime.TimeOfDay))
				{
          retVal = LastRun.Date.Add(StartTime.TimeOfDay);
					if (retVal < DateTime.Now)
					{
            retVal = GetNextTime(retVal, Interval.TimeOfDay);
					}
				}
				else
				{
          retVal = GetNextTime(LastRun, Interval.TimeOfDay);
          if ((retVal.Date > LastRun.Date) || (retVal.TimeOfDay > EndTime.TimeOfDay))
					{
						retVal = GetNextDate(LastRun.Date.AddDays(1), Day);
            retVal = retVal.Add(StartTime.TimeOfDay);
					}
				}
			}
			else
			{
        retVal = retVal.Add(StartTime.TimeOfDay);
			}
			return retVal;
		}

		/// <summary>
		/// Determines the time of day for the next run.
		/// </summary>
		/// <param name="lastRun">Time of the last run.</param>
		/// <param name="interval">The schedule interval.</param>
		/// <returns></returns>
		protected virtual DateTime GetNextTime(DateTime lastRun, TimeSpan interval)
		{
			DateTime retVal = lastRun.Add(interval);

			while (retVal < DateTime.Now)
			{
				retVal = retVal.Add(interval);
				if ((retVal.Date > lastRun.Date)) break;
			}

			return retVal;
		}

		/// <summary>
		/// Determines the next date which the schedule will run.
		/// </summary>
		/// <param name="lastRunDate">The date of the last run.</param>
		/// <param name="day">The day(s) of the week for running.</param>
		/// <returns></returns>
		protected virtual DateTime GetNextDate(DateTime lastRunDate, DaysOfTheWeek day)
		{
			DateTime retVal = lastRunDate.Date;
      while (!retVal.IsDayOfTheWeek(day))
			{
				retVal = retVal.AddDays(1);
			}

			return retVal;
		}
	}
}
