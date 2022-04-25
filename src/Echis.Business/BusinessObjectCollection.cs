using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using System.Business;
using System.Business.Rules;
using BusinessSettings = System.Business.Settings;
using System.Collections;

namespace System.Data.Objects
{
	/// <summary>
	/// Represents a list of Business Objects
	/// </summary>
	/// <typeparam name="TCollection">The interface type of the Business Object Collection.</typeparam>
	/// <typeparam name="TClass">The type of the elements stored in the List.</typeparam>
	/// <typeparam name="TInterface">The interface type of the elements stored in the List.</typeparam>
	[SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes",
		Justification = "All type parameters are necessary")]
	public abstract class BusinessObjectCollection<TCollection, TClass, TInterface> : List<TClass, TInterface>, IBusinessObjectCollection<TInterface>
		where TCollection : class, IBusinessObjectCollection<TInterface>
		where TClass : BusinessObject<TInterface>, TInterface, new()
		where TInterface : class, IBusinessObject
	{
		#region Events
		/// <summary>
		/// Occurs when the List has been invalidated by one or more rules.
		/// </summary>
		public event EventHandler<InvalidatedEventArgs> Invalidated;
		/// <summary>
		/// Occurs when the list has been validated by all rules.
		/// </summary>
		public event EventHandler<ValidatedEventArgs> Validated;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the BusinessObjectCollection class that is empty and has the default initial capacity.
		/// </summary>
		protected BusinessObjectCollection() { }
		/// <summary>
		/// Initializes a new instance of the BusinessObjectCollection class that is empty and has the specified initial capacity.
		/// </summary>
		/// <param name="capacity">The number of elements that the new list can initially store.</param>
		protected BusinessObjectCollection(int capacity) : base(capacity) { }
		/// <summary>
		/// Initializes a new instance of the BusinessObjectCollection class that contains elements copied from the specified collection
		/// and has sufficient capacity to accomodate the number of elements copied.
		/// </summary>
		/// <param name="collection">The collection whose elements are copied to the new list.</param>
		protected BusinessObjectCollection(IEnumerable<TInterface> collection) : base(collection) { }
		/// <summary>
		/// Initializes a new instance of the BusinessObjectCollection class that contains elements copied from the specified collection
		/// and has sufficient capacity to accomodate the number of elements copied.
		/// </summary>
		/// <param name="collection">The collection whose elements are copied to the new list.</param>
		protected BusinessObjectCollection(IEnumerable<TClass> collection) : base(collection) { }
		#endregion

		#region Event Methods
		/// <summary>
		/// Raises the Invalided event.
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

		#region Abstract Members
		/// <summary>
		/// Gets the domain name used to load business rules.
		/// </summary>
		protected abstract string DomainId { get; }
		#endregion

		#region Properties
		/// <summary>
		/// Gets a flag indicating if the object is new.
		/// </summary>
		[SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Property is not applicable to the Collection")]
		bool IUpdatable.IsNew { get { return false; } set { } }
		
		/// <summary>
		/// Gets a flag indicating if any object in the list is dirty.
		/// </summary>
		[XmlIgnore]
		[SoapIgnore]
		[ScriptIgnore]
		public bool IsDirty
		{
			get { return Exists(item => item.IsDirty); }
		}

		/// <summary>
		/// Gets the validation status of the object.
		/// </summary>
		/// <remarks>If the object is valid, this will be an empty string.</remarks>
		[XmlAttribute]
		[SoapIgnore]
		[ScriptIgnore]
		public string RuleMessages { get; private set; }

		#endregion

		#region Methods
		/// <summary>
		/// Determines if the object is valid for the default context.
		/// </summary>
		public bool IsValid()
		{
			return IsValid(BusinessSettings.Values.DefaultContextId);
		}

		/// <summary>
		/// Determines if the object is valid for the specified context.
		/// </summary>
		/// <param name="contextId">The validation context to validate the object against.</param>
		public bool IsValid(string contextId)
		{
			bool retVal = false;

			StringBuilder messages = new StringBuilder();
			if (Services.RuleManager.ValidateCollection(this as TCollection, contextId, DomainId, messages) & ValidateCollectionElements(contextId))
			{
				retVal = true;
				RuleMessages = string.Empty;
				OnValidated();
			}
			else
			{
				RuleMessages = string.Format(CultureInfo.InvariantCulture, messages.ToString().Trim(), DomainId);
				OnInvalidated();
			}

			return retVal;
		}

		/// <summary>
		/// Validates the elements contained within the Business Object Collection.
		/// </summary>
		/// <param name="contextId">The validation context to validate the object against.</param>
		private bool ValidateCollectionElements(string contextId)
		{
			bool retVal = true;
			ForEach(item => retVal = retVal & item.IsValid(contextId));
			return retVal;
		}

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
			ForEach(businessObject => msgList.AddRange(businessObject.GetAllRuleMessages()));
		}

		/// <summary>
		/// Resets all elements in the collection.
		/// </summary>
		public void Reset()
		{
			ForEach(item => item.Reset());
		}

		/// <summary>
		/// Updates all elements in the collection.
		/// </summary>
		public void Update()
		{
			ForEach(item => item.Update());
		}

		/// <summary>
		/// Performs the specified action on each element of the list.
		/// </summary>
		/// <param name="action">The System.Action&lt;T&gt; delegate to perform on each element of the list.</param>
		/// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
		void IBusinessObjectCollection.ForEach(Action<IBusinessObject> action)
		{
			ForEach(action);
		}

		#endregion

		#region ReadData
		/// <summary>
		/// Loads the List using a DataReader.
		/// </summary>
		/// <param name="reader">The reader which contains the resultset stream from which to generate objects.</param>
		public virtual void ReadData(IDataReader reader)
		{
			if (reader == null) throw new ArgumentNullException("reader");

			while (reader.Read())
			{
				TClass item = new TClass();
				Services.PropertyMapper.Process(item.Properties, reader);
				item.IsNew = false;
				Add(item);
			}
		}
		#endregion

	}
}
