using System.IO;

namespace System.Drawing
{
	/// <summary>
	/// A split resolver used by the TiffImageSplitter to determine if a page is a splitter page.
	/// </summary>
	public interface ISplitResolver
	{
		/// <summary>
		/// Called by the TiffImageSplitter to determine if a page is a splitter page.
		/// </summary>
		/// <param name="imageStream">The System.IO.Stream containing the image page to be tested.</param>
		/// <returns>Returns true if the image page a splitter page.
		/// Returns false if the image page is not a splitter page.</returns>
		bool IsSplitterPage(Stream imageStream);
	}
}
