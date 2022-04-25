using System.Collections.Generic;
using System.IO;
using System.Drawing.Drawing2D;

namespace System.Drawing
{
	/// <summary>
	/// The BarCode Split Resolver class searches for a Bar-Code within the specified image.
	/// Derived classes will analyze the bar-code results to determine if the image page is a splitter page.
	/// </summary>
	/// <remarks>This class works best when the SearchRect property is defined covering the area where the bar-code is expected.</remarks>
	public abstract class BarcodeSplitResolver : DefaultSplitResolver
	{
		/// <summary>
		/// Creates a BarCode Split Resolver which will search the entire image if it is smaller than the default threshold.
		/// </summary>
		protected BarcodeSplitResolver() : this(DefaultThreshold, Rectangle.Empty) { }
		/// <summary>
		/// Creates a BarCode Split Resolver which will search specified area of the image if the image is smaller than the specified threshold.
		/// </summary>
		/// <param name="threshold">The maximum size of the splitter image in bytes.</param>
		/// <param name="searchRect">The area of the image to search for a bar-code.</param>
		protected BarcodeSplitResolver(int threshold, Rectangle searchRect)
			: base(threshold)
		{
			SearchRect = searchRect;
		}

		/// <summary>
		/// Gets or sets the area of the image which will be searched for a bar-code.
		/// </summary>
		public Rectangle SearchRect { get; set; }

		/// <summary>
		/// Called by the TiffImageSplitter to determine if a page is a splitter page.
		/// </summary>
		/// <param name="imageStream">The System.IO.Stream containing the image page to be tested.</param>
		/// <returns>Returns true if the image page is smaller than the Threshold and a valid bar-code has been found.
		/// Returns false if the image page is larger than the Threshold or if no valid bar-code has been found.</returns>
		public override bool IsSplitterPage(Stream imageStream)
		{
			bool retVal = false;

			if (base.IsSplitterPage(imageStream)) // if length of imageStream is under the Threshold.
			{
				Bitmap image = Image.FromStream(imageStream) as Bitmap;
				retVal = IsSplitterPage(image);

				if (retVal == false)
				{
					// Didn't find the bar-code, check if image is up-side-down.
					image.RotateFlip(RotateFlipType.Rotate180FlipNone);
					retVal = IsSplitterPage(image);

					if (retVal == false)
					{
						// Try rotating the image 90 degrees.
						image.RotateFlip(RotateFlipType.Rotate90FlipNone);
						retVal = IsSplitterPage(image);

						if (retVal == false)
						{
							// Last try - rotate the image 180 degrees.
							image.RotateFlip(RotateFlipType.Rotate180FlipNone);
							retVal = IsSplitterPage(image);
						}
					}
				}
			}

			return retVal;
		}

		/// <summary>
		/// Searches the specified image for a bar-code.
		/// </summary>
		/// <param name="image">The image to be searched for a bar-code.</param>
		/// <returns>Returns true if the image contains a valid bar-code, returns false if no valid bar-code has been found.</returns>
		protected virtual bool IsSplitterPage(Bitmap image)
		{
			if (image == null) throw new ArgumentNullException("image");

			List<string> codesRead;
			try
			{
				Bitmap barcodePart = (SearchRect == Rectangle.Empty) ? image : image.Clone(SearchRect, image.PixelFormat);
				codesRead = GetBarcodes(barcodePart);
			}
			catch (OutOfMemoryException)
			{
				// Search Rectangle is outside of the bounds of the image. Try just the original image.
				codesRead = GetBarcodes(image);
			}

			return ContainsSplitBarcode(codesRead);
		}

		/// <summary>
		/// Gets the barcodes found in the specified image.
		/// </summary>
		/// <param name="image">The image to search for bar-codes</param>
		protected virtual List<string> GetBarcodes(Bitmap image)
		{
			return BarcodeImaging.VScanPageCode39(image, 5);
		}

		/// <summary>
		/// Implemented by derived classes to determine if any valid bar-codes have been found.
		/// </summary>
		/// <param name="codesRead">The list of potential bar-codes found within the image.</param>
		/// <returns>Returns true if a valid bar-code has been found, returns false if no valid bar-code has been found.</returns>
		protected abstract bool ContainsSplitBarcode(List<string> codesRead);
	}
}
