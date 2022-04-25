using System;

namespace System.Scheduler.Schedules
{
	/// <summary>
	/// Represents a schedule which will run continuously for select days of the week, and/or within a window of operation.
	/// </summary>
	public class ContinualExecutionSchedule : ContinualExecutionSchedule<object>
	{
	}

	/// <summary>
	/// Represents a schedule which will run continuously for select days of the week, and/or within a window of operation.
	/// </summary>
	/// 
	public class ContinualExecutionSchedule<TSettings> : FlexibleSchedule<TSettings>
	{
		/// <summary>
		/// Determines the time of day for the next run.
		/// </summary>
		/// <param name="lastRun">Not used.</param>
		/// <param name="interval">Not used.</param>
		/// <returns>Returns DateTime.Now for continuous execution.</returns>
		protected override DateTime GetNextTime(DateTime lastRun, TimeSpan interval)
		{
			return DateTime.Now;
		}
	}
}
