using System.Xml;

namespace System.Data
{
	/// <summary>
	/// Interface used by ExecuteDataXml method.
	/// </summary>
	public interface IXmlLoader
	{
		/// <summary>
		/// Called by the DataAccess object after execution with an open XmlReader object.
		/// </summary>
		/// <param name="reader">The XmlReader object containing the requested data.</param>
		void ReadXml(XmlReader reader);
	}
}
