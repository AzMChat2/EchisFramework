using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace System
{
	/// <summary>
	/// The Reflection Helper class provides functions which use Reflection to create objects.
	/// </summary>
	public static class ReflectionExtensions
	{
		/// <summary>
		/// Clones an object using XmlSerialization.
		/// </summary>
		/// <typeparam name="T">The Type of object to be cloned.</typeparam>
		/// <param name="source">The source object to be cloned.</param>
		/// <returns>Return a deep copy of the source object</returns>
		/// <exception cref="System.NotSupportedException">Thrown when the object does not support XmlSerialization.</exception>
		public static T Clone<T>(this T source)
		{
			T retVal = default(T);

			using (MemoryStream stream = new MemoryStream())
			{
				XmlSerializer<T>.Serialize(stream, source);

				// Set position back to the beginning of stream
				stream.Position = 0;

				// Deserialize into a new object
				retVal = XmlSerializer<T>.Deserialize(stream);
			}

			return retVal;
		}

		/// <summary>
		/// Clones a derived type into a base type using XmlSerialization.
		/// </summary>
		/// <typeparam name="TInput">The Type of object to be cloned (must be derived from TResult).</typeparam>
		/// <typeparam name="TResult">The Type of object to be returned.</typeparam>
		/// <param name="source">The source object to be cloned.</param>
		/// <returns>Return a deep copy of the source object</returns>
		/// <exception cref="System.NotSupportedException">Thrown when the object does not support XmlSerialization.</exception>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public static TResult CloneAs<TInput, TResult>(this TInput source) where TInput : TResult
		{
			// Add Derived type to XmlSerializer for TResult
			XmlSerializer<TResult>.SetSerializerAttributes(new Type[] { typeof(TInput) });

			string xml = XmlSerializer<TResult>.SerializeToXml(source, CultureInfo.CurrentCulture);

			// Get rid of the reference to the TInput Type
			xml = xml.Replace(string.Format(CultureInfo.InvariantCulture, "xsi:type=\"{0}\"", typeof(TInput).Name), string.Empty);

			// Reset XmlSerializer for TResult
			XmlSerializer<TResult>.SetSerializerAttributes();

			// Deserialize into a new TResult
			return XmlSerializer<TResult>.DeserializeFromXml(xml);
		}

		/// <summary>
		/// Gets the fully qualified name of the member.
		/// </summary>
		/// <param name="memberInfo">The member for which the fully qualified name is desired.</param>
		public static string GetMemberFullName(this MemberInfo memberInfo)
		{
			if (memberInfo == null) throw new ArgumentNullException("memberInfo");

			if (memberInfo.DeclaringType == null)
			{
				Type type = memberInfo as Type;
				return (type == null) ? memberInfo.Name : string.Format(CultureInfo.InvariantCulture, "{0},{1}", type.FullName, type.Assembly.GetName().Name);
			}
			else
			{
				return string.Format(CultureInfo.InvariantCulture, "{0}.{1},{2}",
					memberInfo.DeclaringType.FullName, memberInfo.Name, memberInfo.DeclaringType.Assembly.GetName().Name);
			}
		}

		/// <summary>
		/// Stores cached attributes for Members Info.
		/// </summary>
		private static Dictionary<MemberInfo, object> _attributeCache = new Dictionary<MemberInfo, object>();

		/// <summary>
		/// Gets attribute matching the specified attribute type for a given member.
		/// </summary>
		/// <param name="memberInfo">The member to be inspected for the specified attribute</param>
		public static TAttribute FindAttribute<TAttribute>(this MemberInfo memberInfo)
			where TAttribute : Attribute
		{
			if (memberInfo == null) throw new ArgumentNullException("memberInfo");

			if (!_attributeCache.ContainsKey(memberInfo))
			{
				lock (_attributeCache)
				{
					if (!_attributeCache.ContainsKey(memberInfo)) _attributeCache.Add(memberInfo, FindAttribute(memberInfo, typeof(TAttribute)));
				}
			}

			return _attributeCache[memberInfo] as TAttribute;
		}

		/// <summary>
		/// Gets attribute matching the specified attribute type for a given member.
		/// </summary>
		private static object FindAttribute(MemberInfo memberInfo, Type attributeType)
		{
			object[] attributes = memberInfo.GetCustomAttributes(attributeType, true);

			if (attributes.IsNullOrEmpty())
			{
				if (memberInfo.DeclaringType == null)
				{
					Type type = memberInfo as Type;
					if ((type != null) && type.IsClass)
					{
						Type[] interfaceTypes = type.GetInterfaces();
						foreach (Type interfaceType in interfaceTypes)
						{
							attributes = interfaceType.GetCustomAttributes(attributeType, true);
							if (!attributes.IsNullOrEmpty())
							{
								return attributes[0];
							}
						}
					}
				}
				else if (memberInfo.DeclaringType.IsClass)
				{
					// Member does not contain the attribute, check interfaces for member and attribute.
					Type[] interfaceTypes = memberInfo.DeclaringType.GetInterfaces();

					foreach (Type interfaceType in interfaceTypes)
					{
						MemberInfo interfaceMember = FindMember(interfaceType, memberInfo);

						if (interfaceMember != null)
						{
							// Member found in interface, check for Attribute
							object interfaceAttribute = FindAttribute(interfaceMember, attributeType);
							if (interfaceAttribute != null) return interfaceAttribute;
						}
					}
				}
			}
			else
			{
				return attributes[0];
			}

			return null;
		}

		/// <summary>
		/// Finds a matching Member for a given Type
		/// </summary>
		/// <param name="type">The type to be searched</param>
		/// <param name="memberInfo">The member to be matched.</param>
		public static MemberInfo FindMember(this Type type, MemberInfo memberInfo)
		{
			if (memberInfo == null) throw new ArgumentNullException("memberInfo");
			if (type == null) throw new ArgumentNullException("type");
			if (memberInfo.DeclaringType == type) return memberInfo;

			switch (memberInfo.MemberType)
			{
				case MemberTypes.Method:
					return FindMethod(type, memberInfo as MethodInfo);
				case MemberTypes.Property:
					return FindProperty(type, memberInfo as PropertyInfo);
				case MemberTypes.Field:
					return FindField(type, memberInfo as FieldInfo);
				case MemberTypes.Constructor:
					return FindConstructor(type, memberInfo as ConstructorInfo);
				case MemberTypes.Event:
					return FindEvent(type, memberInfo as EventInfo);
				default:
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The member type specified ({0}) is not supported", memberInfo.MemberType));
			}
		}

		/// <summary>
		/// Finds a matching Method for a given Type
		/// </summary>
		/// <param name="type">The type to be searched</param>
		/// <param name="methodInfo">The method to be matched.</param>
		public static MethodInfo FindMethod(this Type type, MethodInfo methodInfo)
		{
			if (methodInfo == null) throw new ArgumentNullException("methodInfo");
			if (type == null) throw new ArgumentNullException("type");
			if (methodInfo.DeclaringType == type) return methodInfo;

			MethodInfo retVal = null;

			MethodInfo[] candidates = type.GetMethods();
			foreach (MethodInfo candidate in candidates)
			{
				if ((candidate.Name == methodInfo.Name) &&
					(candidate.ReturnType == methodInfo.ReturnType) &&
					(candidate.IsAssembly == methodInfo.IsAssembly) &&
					(candidate.IsFamily == methodInfo.IsFamily) &&
					(candidate.IsPrivate == methodInfo.IsPrivate) &&
					(candidate.IsPublic == methodInfo.IsPublic) &&
					(candidate.IsGenericMethod == methodInfo.IsGenericMethod) &&
					IsMatch(candidate.GetParameters(), methodInfo.GetParameters()))
				{
					retVal = candidate;
					break;
				}
			}

			return retVal;
		}

		/// <summary>
		/// Finds a matching Constructor for a given Type
		/// </summary>
		/// <param name="type">The type to be searched</param>
		/// <param name="constructorInfo">The Constructor to be matched</param>
		public static ConstructorInfo FindConstructor(this Type type, ConstructorInfo constructorInfo)
		{
			if (constructorInfo == null) throw new ArgumentNullException("constructorInfo");
			if (type == null) throw new ArgumentNullException("type");
			if (constructorInfo.DeclaringType == type) return constructorInfo;

			ConstructorInfo retVal = null;

			ConstructorInfo[] candidates = type.GetConstructors();
			foreach (ConstructorInfo candidate in candidates)
			{
				if ((candidate.IsAssembly == constructorInfo.IsAssembly) &&
					(candidate.IsFamily == constructorInfo.IsFamily) &&
					(candidate.IsPrivate == constructorInfo.IsPrivate) &&
					(candidate.IsPublic == constructorInfo.IsPublic) &&
					IsMatch(candidate.GetParameters(), constructorInfo.GetParameters()))
				{
					retVal = candidate;
					break;
				}
			}

			return retVal;
		}

		/// <summary>
		/// Finds a matching Property for a given Type
		/// </summary>
		/// <param name="type">The type to be searched</param>
		/// <param name="propertyInfo">The property to be matched</param>
		public static PropertyInfo FindProperty(this Type type, PropertyInfo propertyInfo)
		{
			if (propertyInfo == null) throw new ArgumentNullException("propertyInfo");
			if (type == null) throw new ArgumentNullException("type");
			if (propertyInfo.DeclaringType == type) return propertyInfo;

			return type.GetProperty(propertyInfo.Name, propertyInfo.PropertyType);
		}

		/// <summary>
		/// Finds a matching Field for a given Type
		/// </summary>
		/// <param name="type">The type to be searched</param>
		/// <param name="fieldInfo">The field to be matched</param>
		public static FieldInfo FindField(this Type type, FieldInfo fieldInfo)
		{
			if (fieldInfo == null) throw new ArgumentNullException("fieldInfo");
			if (type == null) throw new ArgumentNullException("type");
			if (fieldInfo.DeclaringType == type) return fieldInfo;

			FieldInfo candidate = type.GetField(fieldInfo.Name);
			return ((candidate != null) && (candidate.FieldType == fieldInfo.FieldType)) ? candidate : null;
		}

		/// <summary>
		/// Finds a matching Event for a given Type
		/// </summary>
		/// <param name="type">The type to be searched</param>
		/// <param name="eventInfo">The event to be matched</param>
		public static EventInfo FindEvent(this Type type, EventInfo eventInfo)
		{
			if (eventInfo == null) throw new ArgumentNullException("eventInfo");
			if (type == null) throw new ArgumentNullException("type");
			if (eventInfo.DeclaringType == type) return eventInfo;

			EventInfo candidate = type.GetEvent(eventInfo.Name);
			return ((candidate != null) && (candidate.EventHandlerType == eventInfo.EventHandlerType)) ? candidate : null;
		}

		/// <summary>
		/// Determines if two ParameterInfo arrays are a match.
		/// </summary>
		private static bool IsMatch(ParameterInfo[] source, ParameterInfo[] target)
		{
			bool retVal = (source.Length == target.Length);

			if (retVal)
			{
				for (int idx = 0; idx < source.Length; idx++)
				{
					if (source[idx].ParameterType != target[idx].ParameterType)
					{
						retVal = false;
						break;
					}
				}
			}

			return retVal;
		}

		/// <summary>
		/// Create an object using the Assembly specified by name and Class specified by name.
		/// </summary>
		/// <param name="assemblyName">The name of the assembly from which the object will be created.</param>
		/// <param name="className">The class name of the object to be created.</param>
		/// <returns>Returns an instance of the object specified.</returns>
		public static object CreateObject(string assemblyName, string className)
		{
			Assembly assembly = Assembly.Load(assemblyName);
			return assembly.CreateInstance(className);
		}

		/// <summary>
		/// Create an object using the Assembly specified by name and Class specified by name.
		/// </summary>
		/// <param name="assemblyName">The name of the assembly from which the object will be created.</param>
		/// <param name="className">The class name of the object to be created.</param>
		/// <returns>Returns an instance of the object specified.  If an exception is encountered during the creation, returns null.</returns>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Various exceptions are possible, handling is the same in each case: return null.")]
		[DebuggerStepThrough]
		public static object CreateObjectUnsafe(string assemblyName, string className)
		{
			object retVal = null;

			try
			{
				retVal = CreateObject(assemblyName, className);
			}
			catch (Exception ex)
			{
				TS.Logger.WriteLineIf(TS.EC.TraceError, TS.Categories.Error, "Unable to create type '{0}'.\r\n{1}", className, ex);
			}

			return retVal;
		}

		/// <summary>
		/// Create an object of the specified type.
		/// </summary>
		/// <param name="typeName">The name of the type to be created.</param>
		/// <returns>Returns an instance of the object specified.</returns>
		public static object CreateObject(string typeName)
		{
			Type type = Type.GetType(typeName);
			return type.Assembly.CreateInstance(type.FullName);
		}

		/// <summary>
		/// Create an object of the specified type.
		/// </summary>
		/// <param name="typeName">The name of the type to be created.</param>
		/// <returns>Returns an instance of the object specified.  If an exception is encountered during the creation, returns null.</returns>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Various exceptions are possible, handling is the same in each case: return null.")]
		[DebuggerStepThrough]
		public static object CreateObjectUnsafe(string typeName)
		{
			object retVal = null;

			try
			{
				retVal = CreateObject(typeName);
			}
			catch (Exception ex)
			{
				TS.Logger.WriteLineIf(TS.EC.TraceError, TS.Categories.Error, "Unable to create type '{0}'.\r\n{1}", typeName, ex);
			}

			return retVal;
		}

		/// <summary>
		/// Create an object of the specified type.
		/// </summary>
		/// <param name="objectType">The object type to be created.</param>
		/// <returns>Returns an instance of the object specified.</returns>
		public static object CreateObject(Type objectType)
		{
			if (objectType == null) throw new ArgumentNullException("objectType");
			return objectType.Assembly.CreateInstance(objectType.FullName);
		}

		/// <summary>
		/// Create an object of the specified type.
		/// </summary>
		/// <param name="objectType">The object type to be created.</param>
		/// <returns>Returns an instance of the object specified.  If an exception is encountered during the creation, returns null.</returns>
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Various exceptions are possible, handling is the same in each case: return null.")]
		[DebuggerStepThrough]
		public static object CreateObjectUnsafe(Type objectType)
		{
			if (objectType == null) throw new ArgumentNullException("objectType");

			object retVal = null;

			try
			{
				retVal = CreateObject(objectType);
			}
			catch (Exception ex)
			{
				TS.Logger.WriteLineIf(TS.EC.TraceError, TS.Categories.Error, "Unable to create type '{0}'.\r\n{1}", objectType.FullName, ex);
			}

			return retVal;
		}

		/// <summary>
		/// Create an object using the Assembly specified by name and Class specified by name.
		/// </summary>
		/// <typeparam name="T">The object type which will be created.</typeparam>
		/// <param name="assemblyName">The name of the assembly from which the object will be created.</param>
		/// <param name="className">The class name of the object to be created.</param>
		/// <returns>Returns an instance of the object specified.</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public static T CreateObject<T>(string assemblyName, string className)
		{
			return (T)CreateObject(assemblyName, className);
		}

		/// <summary>
		/// Create an object using the Assembly specified by name and Class specified by name.
		/// </summary>
		/// <typeparam name="T">The object type which will be created.</typeparam>
		/// <param name="assemblyName">The name of the assembly from which the object will be created.</param>
		/// <param name="className">The class name of the object to be created.</param>
		/// <returns>Returns an instance of the object specified.  If an exception is encountered during the creation, returns null.</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public static T CreateObjectUnsafe<T>(string assemblyName, string className)
		{
			return (T)CreateObjectUnsafe(assemblyName, className);
		}

		/// <summary>
		/// Create an object using the Assembly specified by name and Class specified by name.
		/// </summary>
		/// <typeparam name="T">The object type which will be created.</typeparam>
		/// <param name="typeName">The name of the type to be created.</param>
		/// <returns>Returns an instance of the object specified.</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public static T CreateObject<T>(string typeName)
		{
			return (T)CreateObject(typeName);
		}

		/// <summary>
		/// Create an object using the Assembly specified by name and Class specified by name.
		/// </summary>
		/// <typeparam name="T">The object type which will be created.</typeparam>
		/// <param name="typeName">The name of the type to be created.</param>
		/// <returns>Returns an instance of the object specified.  If an exception is encountered during the creation, returns null.</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public static T CreateObjectUnsafe<T>(string typeName)
		{
			return (T)CreateObjectUnsafe(typeName);
		}

		/// <summary>
		/// Create an object of the specified type.
		/// </summary>
		/// <typeparam name="T">The object type which will be created.</typeparam>
		/// <param name="objectType">The object type to be created.</param>
		/// <returns>Returns an instance of the object specified.</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public static T CreateObject<T>(Type objectType)
		{
			return (T)CreateObject(objectType);
		}


		/// <summary>
		/// Create an object of the specified type.
		/// </summary>
		/// <typeparam name="T">The object type which will be created.</typeparam>
		/// <param name="objectType">The object type to be created.</param>
		/// <returns>Returns an instance of the object specified.  If an exception is encountered during the creation, returns null.</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public static T CreateObjectUnsafe<T>(Type objectType)
		{
			return (T)CreateObjectUnsafe(objectType);
		}

		/// <summary>
		/// Create an object of the specified type.
		/// </summary>
		/// <typeparam name="T">The object type which will be created.</typeparam>
		/// <returns>Returns an instance of the object specified.</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public static T CreateObject<T>()
		{
			return (T)CreateObject(typeof(T));
		}

		/// <summary>
		/// Create an object of the specified type.
		/// </summary>
		/// <typeparam name="T">The object type which will be created.</typeparam>
		/// <returns>Returns an instance of the object specified.  If an exception is encountered during the creation, returns null.</returns>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "Not possible with this method.")]
		public static T CreateObjectUnsafe<T>()
		{
			return (T)CreateObjectUnsafe(typeof(T));
		}
	}
}
