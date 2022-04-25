using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Globalization;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using System.Business;
using BusinessSettings = System.Business.Settings;

namespace System.Data.Objects
{
	/// <summary>
	/// Represents the base class from which Business Objects will be derived.
	/// </summary>
	/// <typeparam name="T">The final Business Object interface type.</typeparam>
	public abstract class BusinessObject<T> : EntityObject, IBusinessObject
		where T : class, IBusinessObject
	{
		#region Events
		/// <summary>
		/// Occurs when the object has been invalidated by one or more rules.
		/// </summary>
		public event EventHandler<InvalidatedEventArgs> Invalidated;
		/// <summary>
		/// Occurs when the object has been validated by all rules.
		/// </summary>
		public event EventHandler<ValidatedEventArgs> Validated;
		#endregion

		#region Constructors
		/// <summary>
		/// Default Constructor.
		/// </summary>
		protected BusinessObject()
		{
			Properties = new PropertyCollection();
			IsNew = true;
		}

		#endregion

		#region Abstract Members
		/// <summary>
		/// Gets the domain name used to load business rules.
		/// </summary>
		protected abstract string DomainId { get; }
		#endregion

		#region Event Methods
		/// <summary>
		/// Handles the PropertyChanged event for all properties.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void property_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			_isValid = null;
			PropertyBase property = sender as PropertyBase;
			ReportPropertyChanged(property.Name);
			OnPropertyChanged(property.Name);
		}

		/// <summary>
		/// Handles the PropertyChanging event for all properties.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void property_PropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			PropertyBase property = sender as PropertyBase;
			OnPropertyChanging(property.Name);
			ReportPropertyChanging(property.Name);
		}

		/// <summary>
		/// Raises the Invalidated event.
		/// </summary>
		protected virtual void OnInvalidated()
		{
			if (Invalidated != null) Invalidated(this, new InvalidatedEventArgs(RuleMessages));
		}

		/// <summary>
		/// Raises the Validated event.
		/// </summary>
		protected virtual void OnValidated()
		{
			if (Validated != null) Validated(this, new ValidatedEventArgs());
		}

		#endregion

		#region Property List
		/// <summary>
		/// Gets the collection of properties.
		/// </summary>
		[ScriptIgnore]
		[XmlElement]
		public PropertyCollection Properties { get; set; }

		/// <summary>
		/// Adds a Property to the Property List
		/// </summary>
		/// <param name="properties">The properties to be added to the Property List.</param>
		protected void AddProperties(params PropertyBase[] properties)
		{
			if (properties == null) throw new ArgumentNullException("properties");

			Properties.InternalList.AddRange(properties);

			foreach (PropertyBase property in properties)
			{
				property.PropertyChanged += new PropertyChangedEventHandler(property_PropertyChanged);
				property.PropertyChanging += new PropertyChangingEventHandler(property_PropertyChanging);
			}
		}

		#endregion

		#region Properties
		/// <summary>
		/// Gets a flag indicating if the object is new.
		/// </summary>
		[XmlAttribute]
		public bool IsNew { get; set; }

		/// <summary>
		/// Gets a flag indicating if any of the object's properties have changed.
		/// </summary>
		[XmlIgnore]
		[SoapIgnore]
		[ScriptIgnore]
		public bool IsDirty
		{
			get { return Properties.IsDirty; }
		}

		/// <summary>
		/// Gets the validation status of the object.
		/// </summary>
		/// <remarks>If the object is valid, this will be an empty string.</remarks>
		[XmlAttribute]
		[ScriptIgnore]
		public string RuleMessages { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Gets the rule messages for rules that currently do not pass validation for the current object and any child objects within the current object.
		/// </summary>
		public IList<string> GetAllRuleMessages()
		{
			List<string> retVal = new List<string>();
			GetRuleMessages(retVal);
			return retVal;
		}

		/// <summary>
		/// Adds the current object's Rule Messages to the Message List.
		/// </summary>
		/// <param name="msgList">A list containing Rule Messages.</param>
		internal void GetRuleMessages(List<string> msgList)
		{
			if (!string.IsNullOrWhiteSpace(RuleMessages)) msgList.Add(RuleMessages);
			Properties.ForEach(property => msgList.AddRange(property.GetAllRuleMessages()));
		}

		/// <summary>
		/// Determines if the object is valid for the default context.
		/// </summary>
		public bool IsValid()
		{
			return IsValid(BusinessSettings.Values.DefaultContextId);
		}

		/// <summary>
		/// Caches the last validation result.
		/// </summary>
		private bool? _isValid;
		/// <summary>
		/// Object used to ensure thread safety.
		/// </summary>
		private object validationLock = new object();
		/// <summary>
		/// Determines if the object is valid for the specified context.
		/// </summary>
		/// <param name="contextId">The validation context to validate the object against.</param>
		public bool IsValid(string contextId)
		{
			lock (validationLock)
			{
				if (!_isValid.HasValue)
				{
					_isValid = false;

					StringBuilder messages = new StringBuilder();
					if (Services.RuleManager.ValidateObject(this as T, contextId, DomainId, messages) & Properties.ValidateCollectionElements(contextId))
					{
						_isValid = true;
						RuleMessages = string.Empty;
						OnValidated();
					}
					else
					{
						RuleMessages = string.Format(CultureInfo.InvariantCulture, messages.ToString().Trim(), DomainId);
						OnInvalidated();
					}
				}
			}

			return _isValid.Value;
		}

		/// <summary>
		/// Updates the object. This is called after the object has been persisted.
		/// </summary>
		public virtual void Update()
		{
			Properties.UpdateAll();
			IsNew = false;
		}

		/// <summary>
		/// Resets the object to it's original state.
		/// </summary>
		public virtual void Reset()
		{
			Properties.ResetAll();
		}
		#endregion

		#region ReadData

		/// <summary>
		/// Loads the object's properties using a Data Reader
		/// </summary>
		/// <param name="reader">The reader which contains the resultset stream from which to populate the properties.</param>
		/// <remarks>
		/// This method is called by the data access layer as part of the IDataLoader interface.  The reader here will not be
		/// positioned on a valid record, so the first thing is to move the reader to the first record, then load the data by
		/// calling the private method.
		/// </remarks>
		public virtual void ReadData(IDataReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			if (reader.Read())
			{
				Services.PropertyMapper.Process(Properties, reader);
				IsNew = false;
			}
		}
		#endregion
	}
}
