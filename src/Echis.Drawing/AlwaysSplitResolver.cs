using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace System.Drawing
{
	/// <summary>
	/// Represents a Split Resolver which always returns true.  This is used by the TiffImageSplitter in the SeparateImages method.
	/// </summary>
	internal class AlwaysSplitResolver : ISplitResolver
	{
		/// <summary>
		/// Always returns true.  This will cause the TiffImageSplitter to split on every page.
		/// </summary>
		public bool IsSplitterPage(Stream imageStream)
		{
			return true;
		}
	}
}
