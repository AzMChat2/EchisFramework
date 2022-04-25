using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;
using TraceLevels = System.Diagnostics.TraceLevel;

namespace System.Diagnostics
{
	/// <summary>
	/// Represents System Configuration Values stored in the App or Web Config file.
	/// </summary>
	[Serializable]
	public sealed class Settings : SettingsBase<Settings>
	{
		#region Constants
		/// <summary>
		/// The Defaults class contains all of the Settings default values.
		/// </summary>
		private static class Defaults
		{
			/// <summary>
			/// Default value for the DefaultTraceLevel property.
			/// </summary>
			public const TraceLevels TraceLevel = TraceLevels.Off;
			/// <summary>
			/// Default value for the UseEventLog property.
			/// </summary>
			public const bool UseEventLog = true;
			/// <summary>
			/// Default value for the SystemFrameworkContext property.
			/// </summary>
			public const string ContextSystem = "System";
			/// <summary>
			/// Default value for the DefaultContext property.
			/// </summary>
			public const string ContextDefault = "Default";
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public Settings()
		{
			DefaultLevel = Defaults.TraceLevel;
		}
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the Diagnostics Context used for System Framework Components.
		/// </summary>
		[XmlAttribute]
		public string SystemFrameworkContext { get; set; }

		/// <summary>
		/// Gets or sets the Default Diagnostics Context.
		/// </summary>
		[XmlAttribute]
		public string DefaultContext { get; set; }

		/// <summary>
		/// Gets the default trace level.
		/// </summary>
		[XmlAttribute]
		public TraceLevels DefaultLevel { get; set; }

		/// <summary>
		/// Gets the Full Type Name of the Logger device to use.
		/// </summary>
		[XmlAttribute]
		public string Logger { get; set; }

		/// <summary>
		/// Gets the Full Type Name of the Logger Registry to use.
		/// </summary>
		[XmlAttribute]
		public string LoggerRegistry { get; set; }

		//NOTE: The XmlSerializer chokes if we use { get; private set;} on List properties,
		//      but works fine if we use an underlying variable (e.g, _threadTracelevels, _contextLevels).
		/// <summary>
		/// Stores the list of Thread Trace Levels.
		/// </summary>
		private List<ThreadTraceLevel> _threadTraceLevels = new List<ThreadTraceLevel>();
		/// <summary>
		/// Gets collection containing Trace Levels for threads by thread name.
		/// </summary>
		[XmlElement("Thread")]
		public List<ThreadTraceLevel> ThreadTraceLevels { get { return _threadTraceLevels; } }

		/// <summary>
		/// Stores the list of Context Trace Levels
		/// </summary>
		private List<ContextTraceLevel> _contextLevels = new List<ContextTraceLevel>();
		/// <summary>
		/// Gets the list of Context Trace Levels
		/// </summary>
		[XmlElement("ContextLevel")]
		public List<ContextTraceLevel> ContextLevels { get { return _contextLevels; } }

		#endregion

		#region Methods
		/// <summary>
		/// Validates configuration and sets defaults for missing settings.
		/// </summary>
		public override void Validate()
		{
			if (SystemFrameworkContext == null) SystemFrameworkContext = Defaults.ContextSystem;
			if (DefaultContext == null) DefaultContext = Defaults.ContextDefault;
		}

		/// <summary>
		/// Ignore any Exceptions.
		/// </summary>
		/// <param name="exception">The exception thrown while attempting to read the settings from the configuration file.</param>
		protected override void HandleException(Exception exception)
		{
			// Ignore errors
		}
		#endregion
	}
}
