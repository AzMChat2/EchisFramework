using System;
using System.Xml;

namespace System.Scheduler
{
	/// <summary>
	/// Interface which all Processors must implement.
	/// </summary>
	public interface IProcessor
	{
    /// <summary>
    /// Fired when the Processor is executed.
    /// </summary>
    event EventHandler Executed;

    /// <summary>
		/// Gets the Processor Info on which this processor runs.
		/// </summary>
		ProcessorInfo Info { get; set; }

		/// <summary>
		/// Gets a flag which indicates if the process thread is currently running (process may be running or idle).
		/// </summary>
		bool IsRunning { get; }

		/// <summary>
		/// Starts the schedule for the Processor.
		/// </summary>
		void StartProcess();

		/// <summary>
		/// Stops the schedule for the Processor (will wait for processor to complete current execution if necessary).
		/// </summary>
    void StopProcess();

    /// <summary>
    /// Aborts processing.
    /// </summary>
    void AbortProcess();

    /// <summary>
    /// Writes the Processor run status to an XmlWriter.
    /// </summary>
    /// <param name="writer"></param>
    void WriteStatus(XmlWriter writer);
	}
}
