using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.Business;
using BusinessSettings = System.Business.Settings;

namespace System.Data.Objects
{

	#region PropertyBase
	/// <summary>
	/// Represents the basic Business Object Property.
	/// </summary>
	public abstract class PropertyBase : IValidatable, IUpdatable, INotifyPropertyChanged, INotifyPropertyChanging
	{
		#region static and constant members
		/// <summary>
		/// The name of the Value Property.
		/// </summary>
		private const string ValueProperty = "Value";
		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		protected PropertyBase() { IsXmlSerializable = true; }

		#region Events
		/// <summary>
		/// Occurs when the value of the property has been invalidated by one or more rules.
		/// </summary>
		public event EventHandler<InvalidatedEventArgs> Invalidated;
		/// <summary>
		/// Occurs when the value of the property has been validated by all applied rules.
		/// </summary>
		public event EventHandler<ValidatedEventArgs> Validated;
		/// <summary>
		/// Occurs after the value of a property has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		/// <summary>
		/// Occurs when the value of a property is changing.
		/// </summary>
		public event PropertyChangingEventHandler PropertyChanging;
		#endregion

		#region Event Methods
		/// <summary>
		/// Raises the PropertyChanged event.
		/// </summary>
		protected void OnPropertyChanged()
		{
			Valid = null;
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(ValueProperty));
			}
		}

		/// <summary>
		/// Raises the PropertyChanging Event.
		/// </summary>
		protected void OnPropertyChanging()
		{
			if (PropertyChanging != null)
			{
				PropertyChanging(this, new PropertyChangingEventArgs(ValueProperty));
			}
		}

		/// <summary>
		/// Raises the Invalidated Event.
		/// </summary>
		protected void OnInvalidated()
		{
			if (Invalidated != null) Invalidated(this, new InvalidatedEventArgs(RuleMessages));
		}

		/// <summary>
		/// Raises the Validated Event.
		/// </summary>
		protected void OnValidated()
		{
			if (Validated != null) Validated(this, new ValidatedEventArgs());
		}
		#endregion

		#region Properties
		/// <summary>
		/// Not Used.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes",
			Justification = "The IsNew property is not applicable to Property Objects")]
		bool IUpdatable.IsNew { get { return false; } set { } }

		/// <summary>
		/// Gets or sets the value of the Property
		/// </summary>
		[XmlIgnore]
		public abstract object PropertyValue { get; set; }

		/// <summary>
		/// Gets the DomainId of the object to which this property belongs
		/// </summary>
		[XmlAttribute]
		public string DomainId { get; set; }
		/// <summary>
		/// Gets the name of the Property.
		/// </summary>
		[XmlAttribute]
		public string Name { get; set; }
		/// <summary>
		/// Gets the validation status of the property.
		/// </summary>
		/// <remarks>If the property is valid, this will be an empty string.</remarks>
		[XmlAttribute]
		[ScriptIgnore]
		public string RuleMessages { get; set; }
		/// <summary>
		/// Caches the last validation result.
		/// </summary>
		[XmlAttribute]
		protected bool? Valid { get; set; }

		/// <summary>
		/// Gets or sets a flag which determines if the Property is Xml Serializable
		/// </summary>
		/// <remarks>Default value is true.</remarks>
		[XmlIgnore]
		public bool IsXmlSerializable { get; set; }

		/// <summary>
		/// Gets a flag indicating if the Property Value has changed.
		/// </summary>
		public abstract bool IsDirty { get; }

		#endregion

		#region Methods
		/// <summary>
		/// Updates the old value with the new.  This is called after the object has been persisted.
		/// </summary>
		public abstract void Update();
		/// <summary>
		/// Resets the value of the property to the original value.
		/// </summary>
		public abstract void Reset();

		/// <summary>
		/// Determines if the property is valid for the specified context.
		/// </summary>
		/// <param name="contextId">The validation context to validate the object against.</param>
		public abstract bool IsValid(string contextId);

		/// <summary>
		/// Determines if the property is valid for the specified context.
		/// </summary>
		public bool IsValid()
		{
			return IsValid(BusinessSettings.Values.DefaultContextId);
		}

		/// <summary>
		/// Gets the rule messages for rules that currently do not pass validation for the current object and any child objects within the current object.
		/// </summary>
		public IList<string> GetAllRuleMessages()
		{
			IList<string> retVal = new List<string>();
			GetRuleMessages(retVal);
			return retVal;
		}

		/// <summary>
		/// Adds the current object's Rule Messages to the Message List.
		/// </summary>
		/// <param name="msgList">A list containing Rule Messages.</param>
		internal void GetRuleMessages(IList<string> msgList)
		{
			if (!string.IsNullOrWhiteSpace(RuleMessages)) msgList.Add(RuleMessages);
		}
		#endregion

	}
	#endregion

	#region Property<T>
	/// <summary>
	/// Represents the basic Business Object Property.
	/// </summary>
	/// <typeparam name="T">The property type.</typeparam>
	[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Property")]
	public class Property<T> : PropertyBase
	{

		#region Properties
		/// <summary>
		/// Gets the Type of the property
		/// </summary>
		[XmlIgnore]
		internal protected Type PropertyType { get { return typeof(T); } }

		/// <summary>
		/// Stores the value of the Value property.
		/// </summary>
		private T _value;
		/// <summary>
		/// Gets or sets the value of the Property.
		/// </summary>
		[XmlElement]
		public T Value
		{
			get { return _value; }
			set { SetValue(value); }
		}

		/// <summary>
		/// Gets the original value of the Property.
		/// </summary>
		[XmlElement]
		public T OldValue { get; set; }

		/// <summary>
		/// Gets or sets the value of the Property
		/// </summary>
		[XmlIgnore]
		public override object PropertyValue
		{
			get { return _value; }
			set { SetValue((T)value); }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Sets the value of the Property
		/// </summary>
		/// <param name="value">The new value of the property.</param>
		/// <remarks>Causes the PropertyChanging and PropertyChanged events to fire if the new value is different than the original value.
		/// Also causes validation to occur which will raise the Invalidated or Validated event.</remarks>
		protected virtual void SetValue(T value)
		{
			if (!AreEqual(value, _value))
			{
				OnPropertyChanging();
				_value = value;
				OnPropertyChanged();
			}
		}
	
		/// <summary>
		/// Determines if two objects are equal.
		/// </summary>
		/// <param name="obj1"></param>
		/// <param name="obj2"></param>
		/// <returns></returns>
		protected static bool AreEqual(T obj1, T obj2)
		{
			return (obj1 == null) ? (obj2 == null) : obj1.Equals(obj2);
		}

		/// <summary>
		/// Updates the old value with the new.  This is called after the object has been persisted.
		/// </summary>
		public override void Update()
		{
			OldValue = Value;
		}

		/// <summary>
		/// Resets the value of the property to the original value.
		/// </summary>
		public override void Reset()
		{
			Value = OldValue;
		}

		/// <summary>
		/// Gets a flag indicating if the Property Value has changed.
		/// </summary>
		[XmlIgnore]
		public override bool IsDirty
		{
			get
			{
				if (OldValue == null)
				{
					return (Value != null);
				}
				else
				{
					return !OldValue.Equals(Value);
				}
			}
		}

		/// <summary>
		/// Object used to ensure thread safety.
		/// </summary>
		private object validationLock = new object();
		/// <summary>
		/// Determines if the property is valid for the specified context.
		/// </summary>
		/// <param name="contextId">The validation context to validate the object against.</param>
		public override bool IsValid(string contextId)
		{
			lock (validationLock)
			{
				if (!Valid.HasValue)
				{
					Valid = false;
					StringBuilder messages = new StringBuilder();
					if (Services.RuleManager.ValidateProperty(this, contextId, DomainId, messages))
					{
						Valid = true;
						RuleMessages = string.Empty;
						OnValidated();
					}
					else
					{
						RuleMessages = string.Format(CultureInfo.InvariantCulture, messages.ToString().Trim(), Name);
						OnInvalidated();
					}
				}
			}

			return Valid.Value;
		}
		#endregion

	}
	#endregion

}
