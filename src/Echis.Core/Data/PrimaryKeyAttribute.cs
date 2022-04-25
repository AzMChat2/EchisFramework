namespace System.Data
{
	/// <summary>
	/// Specifies that the property is (or is part of) the Primary Key
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public sealed class PrimaryKeyAttribute : Attribute
	{
	}
}
