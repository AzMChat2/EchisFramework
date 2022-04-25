using System;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace System.Scheduler.Schedules
{
	/// <summary>
	/// The Schedule List represents a list of schedules for a given processor.
	/// </summary>
	public class ScheduleCollection : XmlWrapperList<Schedule>
	{
		/// <summary>
		/// Gets the last run for the Processor.
		/// </summary>
    [XmlIgnore]
		public DateTime LastRun { get; private set; }

    /// <summary>
    /// Gets the number of enabled schedules in the collection.
    /// </summary>
    [XmlIgnore]
    public int EnabledCount
    {
      get { return (from s in this where s.Value.Enabled select s).Count(); }
    }

		/// <summary>
		/// Gets the next scheduled run for the processor.
		/// </summary>
    [XmlIgnore]
    public Schedule NextSchedule
		{
      get
      {
        Sort(CompareSchedules);
        return Count == 0 ? null : this[0].Value;
      }
		}

		/// <summary>
		/// Sorts schedules by the next run date.
		/// </summary>
		private int CompareSchedules(XmlWrapper<Schedule> source, XmlWrapper<Schedule> target)
		{
			return source.Value.NextRun.CompareTo(target.Value.NextRun);
		}

		/// <summary>
		/// Sets the last run time for all schedules in the list.
		/// </summary>
		/// <param name="lastRun">The date and time of the last run for the process.</param>
		public void SetLastRun(DateTime lastRun)
		{
			LastRun = lastRun;
			ForEach(item => item.Value.LastRun = lastRun);
		}
	}
}
