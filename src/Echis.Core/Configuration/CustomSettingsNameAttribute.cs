using System;

namespace System.Configuration
{
	/// <summary>
	/// Defines a custom configuration section name for a Settings class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class CustomSettingsNameAttribute : Attribute
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		/// <param name="sectionName">The name of the configuration section for the Settings class.</param>
		public CustomSettingsNameAttribute(string sectionName)
		{
			SectionName = sectionName;
		}

		/// <summary>
		/// Gets or sets the name of the configuration section for the Settings class.
		/// </summary>
		public string SectionName { get; private set; }

	}
}
