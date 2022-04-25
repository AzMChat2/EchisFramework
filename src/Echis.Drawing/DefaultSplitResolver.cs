using System.IO;

namespace System.Drawing
{
	/// <summary>
	/// The default split resolver used by the TiffImageSplitter to determine if a page is a splitter page.
	/// </summary>
	public class DefaultSplitResolver : ISplitResolver
	{
		/// <summary>
		/// The default threshold value, which is 20 kilobytes.
		/// </summary>
		protected const int DefaultThreshold = IOExtensions.BufferSize.Kilobytes.Sixteen + IOExtensions.BufferSize.Kilobytes.Four;

		/// <summary>
		/// Creates an instance of the DefaultSplitResolver using the default Threshold value.
		/// </summary>
		public DefaultSplitResolver() : this(DefaultThreshold) { }
		/// <summary>
		/// Creates an instance of the DefaultSplitResolver using the specified Threshold value.
		/// </summary>
		/// <param name="threshold">The maximum size of the splitter image in bytes.</param>
		public DefaultSplitResolver(int threshold)
		{
			Threshold = threshold;
		}

		/// <summary>
		/// Gets or sets the maximum size, in bytes, of the splitter image.
		/// Image pages below this Threshold are considered "splitter pages" and will cause the TiffImageSplitter to split the multipage Tiff image.
		/// </summary>
		public int Threshold { get; set; }

		/// <summary>
		/// Called by the TiffImageSplitter to determine if a page is a splitter page.
		/// </summary>
		/// <param name="imageStream">The System.IO.Stream containing the image page to be tested.</param>
		/// <returns>Returns true if the image page is smaller than the Threshold.
		/// Returns false if the image page is larger than the Threshold.</returns>
		public virtual bool IsSplitterPage(Stream imageStream)
		{
			if (imageStream == null) throw new ArgumentNullException("imageStream");

			return (imageStream.Length <= Threshold);
		}
	}
}
