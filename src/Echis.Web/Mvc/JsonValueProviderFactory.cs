using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web.Script.Serialization;

namespace System.Web.Mvc
{
	/// <summary>
	/// Base class for retrieving Values from a Json serialized object(s) found in a Name Value Collection within the Request Context.
	/// </summary>
	public abstract class JsonValueProviderFactoryBase : ValueProviderFactory
	{
		/// <summary>
		/// Gets the values, if found, from the Json Serialized object(s) found within the Request Context
		/// </summary>
		/// <param name="controllerContext">The controller context containing the Request Context.</param>
		public override IValueProvider GetValueProvider(ControllerContext controllerContext)
		{
			IDictionary<string, object> dictionary = GetDictionary(controllerContext);

			return (dictionary == null) ? null : new DictionaryValueProvider<object>(dictionary, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Gets the Name Value Collection from the Request Context in which a Json Serialized object(s) exists
		/// </summary>
		/// <param name="request">The Request Context</param>
		protected abstract NameValueCollection GetNameValueCollection(HttpRequestBase request);

		/// <summary>
		/// Gets the values, if found, from the Json Serialized object(s) found within the Request Context
		/// </summary>
		/// <param name="controllerContext">The controller context containing the Request Context.</param>
		protected virtual IDictionary<string, object> GetDictionary(ControllerContext controllerContext)
		{
			if (controllerContext == null) throw new ArgumentNullException("controllerContext");

			Dictionary<string, IDictionary<string, object>> dictionaries = GetDictionaries(controllerContext);

			if (dictionaries.Count == 0)
			{
				return null;
			}
			else if (dictionaries.Count == 1)
			{
				return dictionaries.First().Value;
			}
			else
			{
				Dictionary<string, object> retVal = new Dictionary<string, object>();

				dictionaries.ForEach(item => AddToDictionary(retVal, item));

				return retVal;
			}
		}

		/// <summary>
		/// Gets Dictionaries from all Json Serialized objects within the Request Context.
		/// </summary>
		/// <param name="controllerContext">The controller context containing the Request Context.</param>
		protected virtual Dictionary<string, IDictionary<string, object>> GetDictionaries(ControllerContext controllerContext)
		{
			Dictionary<string, IDictionary<string, object>> dictionaries = new Dictionary<string, IDictionary<string, object>>();

			if (controllerContext != null)
			{
				NameValueCollection data = GetNameValueCollection(controllerContext.HttpContext.Request);
				JavaScriptSerializer serializer = new JavaScriptSerializer();

				for (int idx = 0; idx < data.Count; idx++)
				{
					string value = HttpUtility.UrlDecode(data[idx]);
					if (!string.IsNullOrWhiteSpace(value) && (value.StartsWith("{", StringComparison.OrdinalIgnoreCase)))
					{
						dictionaries.Add(data.Keys[idx], serializer.DeserializeObject(value) as IDictionary<string, object>);
					}
				}
			}

			return dictionaries;
		}

		/// <summary>
		/// Adds an item to the master dictionary
		/// </summary>
		/// <param name="dictionary">The master dictionary to which values will be added.</param>
		/// <param name="keyValuePair">The item to be added to the master dictionary</param>
		private static void AddToDictionary(Dictionary<string, object> dictionary, KeyValuePair<string, IDictionary<string, object>> keyValuePair)
		{
			keyValuePair.Value.ForEach(item => AddToDictionary(dictionary, keyValuePair.Key, item.Key, item.Value));
		}

		/// <summary>
		/// Adds an item to the master dictionary
		/// </summary>
		/// <param name="dictionary">The master dictionary to which values will be added.</param>
		/// <param name="prefix">The prefix to be appended to the key.</param>
		/// <param name="key">The key of the item to be added.</param>
		/// <param name="value">The value of the item to be added.</param>
		private static void AddToDictionary(Dictionary<string, object> dictionary, string prefix, string key, object value)
		{
			dictionary.Add(string.Format(CultureInfo.InvariantCulture, "{0}.{1}", prefix, key), value);
		}
	}

	/// <summary>
	/// Provides Values from a Json serialized object(s) found in within the Request Context Form data.
	/// </summary>
	public class JsonFormValueProviderFactory : JsonValueProviderFactoryBase
	{
		/// <summary>
		/// Gets the Form Name Value Collection from the Request Context
		/// </summary>
		/// <param name="request">The Request Context</param>
		protected override NameValueCollection GetNameValueCollection(HttpRequestBase request)
		{
			return (request == null) ? new NameValueCollection() : request.Form;
		}
	}

	/// <summary>
	/// Provides Values from a Json serialized object(s) found in within the Request Context Query String data.
	/// </summary>
	public class JsonQueryStringValueProviderFactory : JsonValueProviderFactoryBase
	{
		/// <summary>
		/// Gets the Query String Name Value Collection from the Request Context
		/// </summary>
		/// <param name="request">The Request Context</param>
		protected override NameValueCollection GetNameValueCollection(HttpRequestBase request)
		{
			return (request == null) ? new NameValueCollection() : request.QueryString;
		}
	}
}