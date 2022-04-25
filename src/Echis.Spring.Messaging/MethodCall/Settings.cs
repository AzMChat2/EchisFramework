using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using System.Configuration;

namespace System.Spring.Messaging.MethodCall
{
	/// <summary>
	/// Provides information the Method Info Provider uses to determine Method Info for a given Method Call.
	/// </summary>
	public class Settings : SettingsBase<Settings>
	{
		/// <summary>
		/// Gets or sets the list of Services which contain methods whose calls are being intercepted for Messaging purposes.
		/// </summary>
		[XmlElement("Service")]
		[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
			Justification = "The property setter is required by the XmlSerializer.")]
		public ServiceCollection Services { get; set; }

		/// <summary>
		/// Finds a service using the Service's type name.
		/// </summary>
		/// <param name="typeName">The type name of the service.</param>
		public Service FindService(string typeName)
		{
			return Services.Find(item => item.TypeName.Equals(typeName, StringComparison.OrdinalIgnoreCase));
		}

		/// <summary>
		/// Stores information about a Service which contains methods whose calls are being intercepted for Messaging purposes.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible",
			Justification = "This class is only used for settings and is public because it is deserialized using the XmlSerializer.")]
		public class Service
		{
			/// <summary>
			/// Gets or sets the type name of the service.
			/// </summary>
			[XmlAttribute]
			public string TypeName { get; set; }

			/// <summary>
			/// Gets or sets the list of methods whose calls are being intercepted for Messaging purposes.
			/// </summary>
			[XmlElement("Method")]
			[SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly",
				Justification = "The property setter is required by the XmlSerializer.")]
			public MethodCollection Methods { get; set; }

			/// <summary>
			/// Finds a method by name within a service.
			/// </summary>
			/// <param name="name">The name of the method.</param>
			public MethodCallInfo FindMethod(string name)
			{
				MethodCallInfo retVal = Methods.Find(item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

				if (retVal == null)
				{
					retVal = new MethodCallInfo() { Name = name };
					Methods.Add(retVal);
				}

				return retVal;
			}
		}

		/// <summary>
		/// Stores information about Services which contains methods whose calls are being intercepted for Messaging purposes.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible",
			Justification = "This class is only used for settings and is public because it is deserialized using the XmlSerializer.")]
		public class ServiceCollection : List<Service> { }

		/// <summary>
		/// Stores information about Methods whose calls are being intercepted for Messaging purposes.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible",
			Justification = "This class is only used for settings and is public because it is deserialized using the XmlSerializer.")]
		public class MethodCollection : List<MethodCallInfo> { }
	}
}