namespace System.Data
{
  /// <summary>
  /// Specifies that the property is (or is part of) the Primary Key
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
  public sealed class FactoryAttribute : Attribute
  {
    /// <summary>
    /// Default Constructor (applies to classes or methods).
    /// </summary>
    public FactoryAttribute() { }

    /// <summary>
    /// Constructor (applies to classes only, definition is ignored for methods)
    /// </summary>
    /// <param name="objectId">The Container ObjectId used to retrieve the Factory from the IOC Container.</param>
    public FactoryAttribute(string objectId) : this(null, objectId) { }

    /// <summary>
    /// Constructor (applies to classes only, definition is ignored for methods)
    /// </summary>
    /// <param name="contextId">The Container ContextId used to retrieve the Factory from the IOC Container.</param>
    /// <param name="objectId">The Container ObjectId used to retrieve the Factory from the IOC Container.</param>
    public FactoryAttribute(string contextId, string objectId)
    {
      ContextId = contextId;
      ObjectId = objectId;
    }

    /// <summary>
    /// Gets the Container ContextId used to retrieve the Factory from the IOC Container.
    /// </summary>
    public string ContextId { get; private set; }

    /// <summary>
    /// Gets the Container ObjectId used to retrieve the Factory from the IOC Container.
    /// </summary>
    public string ObjectId { get; private set; }

  }
}
