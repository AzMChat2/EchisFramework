
namespace System.Scheduler.Service
{
	/// <summary>
	/// The Service manager starts and stops the Scheduling Service.
	/// </summary>
	public static class ServiceManager
	{
		/// <summary>
		/// Starts the Scheduling Service
		/// </summary>
		public static void Start()
		{
			ProcessorCollection.AddRange(Settings.Values.Processors);
			ProcessorCollection.StartAll();
		}

		/// <summary>
		/// Stops the Scheduling Service
		/// </summary>
		public static void Stop()
		{
			ProcessorCollection.StopAll();
			ProcessorCollection.SaveStatusAll();
			ProcessorCollection.ClearProcessors();
		}
	}
}
