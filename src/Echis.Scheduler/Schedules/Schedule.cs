using System;
using System.Xml;
using System.Xml.Serialization;

namespace System.Scheduler.Schedules
{
	/// <summary>
	/// Base class from which all Schedule definitions are derived.
	/// </summary>
	public abstract class Schedule 
	{
		/// <summary>
		/// Stores the Date and Time the Schedule was last run.
		/// </summary>
		private DateTime _lastRun;
		/// <summary>
		/// Gets or sets the Date and Time the schedule was last run.
		/// </summary>
    [XmlAttribute]
		public DateTime LastRun
		{
			get { return _lastRun; }
			set
			{
				_lastRun = value;
				_nextRun = CalculateNextRun();
			}
		}

		/// <summary>
		/// Gets or sets a flag indicating if the schedule is enabled. (Default is true).
		/// </summary>
    [XmlAttribute]
    public bool Enabled { get; set; }

		/// <summary>
		/// Stores the next run scheduled.
		/// </summary>
		private DateTime _nextRun;
		/// <summary>
		/// Gets the next run scheduled.
		/// </summary>
    [XmlIgnore]
    public DateTime NextRun
		{
			get { return Enabled ? _nextRun : DateTime.MaxValue; }
		}

		/// <summary>
		/// Derived classes will override to indicate the next run time based on the last run.
		/// </summary>
		/// <returns></returns>
		protected abstract DateTime CalculateNextRun();
	}

	/// <summary>
	/// Base class from which all Schedule definitions are derived.
	/// </summary>
	public abstract class Schedule<T> : Schedule
	{
		/// <summary>
		/// A custom settings object with values relative to this schedule.
		/// </summary>
    [XmlElement]
		public T Settings { get; set; }
	}
}
