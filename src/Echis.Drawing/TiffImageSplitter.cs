using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	/// <summary>
	/// The TiffImageSplitter class provides the ability to split a multi-page Tiff file into multiple Tiff files using a Split Resolver
	/// </summary>
	public static class TiffImageSplitter
	{
		/// <summary>
		/// Mime type name for Tiff Images.
		/// </summary>
		private const string _tiffMimeType = "image/tiff";

		/// <summary>
		/// Codec Encoder for Tiff Images
		/// </summary>
		private static readonly ImageCodecInfo _tiffEncoder = GetTiffEncoder();

		/// <summary>
		/// Gets the TIFF Image Encoder.
		/// </summary>
		private static ImageCodecInfo GetTiffEncoder()
		{
			ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();

			foreach (ImageCodecInfo encoder in encoders)
			{
				if (encoder.MimeType.Equals(_tiffMimeType, StringComparison.OrdinalIgnoreCase))
				{
					return encoder;
				}
			}

			return null;
		}


		/// <summary>
		/// Separates each page of the image into a separate image file.
		/// </summary>
		/// <param name="fileName">The filename of the original image.</param>
		/// <param name="compressionOption">The compression option used to compress the resulting image files.</param>
		/// <returns>Returns an array of strings containing the filenames of the resulting image files.</returns>
		public static string[] SeparateImages(string fileName, CompressionOptions compressionOption)
		{
			return SplitImage(fileName, new AlwaysSplitResolver(), ImageSplitOptions.IncludeSplitterPageBefore, compressionOption);
		}

		/// <summary>
		/// Splits a Multipage Tiff image into multiple files.
		/// </summary>
		/// <param name="fileName">The filename of the Tiff Image to split</param>
		/// <param name="resolver">The Split Resolver used to determine where to split the image.</param>
		/// <param name="imageSplitOption">The split option used to determine if and where the slitter page will be included in the resulting image files.</param>
		/// <param name="compressionOption">The compression option used to compress the resulting image files.</param>
		/// <returns>Returns an array of strings containing the filenames of the resulting image files.</returns>
		public static string[] SplitImage(string fileName, ISplitResolver resolver, ImageSplitOptions imageSplitOption, CompressionOptions compressionOption)
		{
			if (resolver == null) throw new ArgumentNullException("resolver");

			using (Bitmap image = Image.FromFile(fileName) as Bitmap)
			{
				List<string> retVal = new List<string>();

				int pageCount = image.GetFrameCount(FrameDimension.Page);

				if (pageCount == 1)
				{
					// If there is only one page, there is nothing to split.
					retVal.Add(fileName);
				}
				else
				{
					Image currentImage = null;
					using (EncoderParameters encInit = new EncoderParameters(2), encAdd = new EncoderParameters(2), encFinal = new EncoderParameters(2))
					{
						EncoderParameter comp = GetCompresionParameter(compressionOption);

						encInit.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
						encAdd.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);
						encFinal.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.Flush);

						encInit.Param[1] = comp;
						encAdd.Param[1] = comp;
						encFinal.Param[1] = comp;

						for (int idx = 0; idx < pageCount; idx++)
						{
							image.SelectActiveFrame(FrameDimension.Page, idx);

							using (MemoryStream stream = GetPageStream(image))
							{
								bool isSplitterPage = resolver.IsSplitterPage(stream);

								bool addPage = (!isSplitterPage || (imageSplitOption != ImageSplitOptions.ExcludeSplitterPage));
								bool splitBefore = (isSplitterPage && (imageSplitOption != ImageSplitOptions.IncludeSplitterPageAfter));
								bool splitAfter = (isSplitterPage && (imageSplitOption == ImageSplitOptions.IncludeSplitterPageAfter));

								if (splitBefore && (currentImage != null))
								{
									currentImage.SaveAdd(encFinal);
									currentImage = null;
								}

								if (addPage)
								{
									if (currentImage == null)
									{
										string workingExt = string.Format(CultureInfo.InvariantCulture, "{0}{1}", retVal.Count, Path.GetExtension(fileName));
										string workingFile = Path.ChangeExtension(fileName, workingExt);

										retVal.Add(workingFile);
										currentImage = Image.FromStream(stream);
										currentImage.Save(workingFile, _tiffEncoder, encInit);
									}
									else
									{
										currentImage.SaveAdd(Image.FromStream(stream), encAdd);
									}
								}

								if (splitAfter && (currentImage != null))
								{
									currentImage.SaveAdd(encFinal);
									currentImage = null;
								}
							}
						}

						if (currentImage != null)
						{
							currentImage.SaveAdd(encFinal);
							currentImage = null;
						}
					}
				}
				return retVal.ToArray();
			}
		}

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "Method is a factory method which creates and returns an IDisposable object, consuming code is responsible for disposing.")]
		private static MemoryStream GetPageStream(Bitmap image)
		{
			MemoryStream retVal = new MemoryStream();

			try
			{
				image.Save(retVal, ImageFormat.Tiff);
			}
			catch (ExternalException)
			{
				// "A generic error occurred in GDI+"
				// This happens occasionally with certain TIFF files.  
				// The fix is to create a new Bitmap from the original.

				// Create a rectangle than encompasses the entire image.
				Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

				// Create a new image of the same size with the correct format.
				using (Bitmap fix = new Bitmap(image.Width, image.Height, image.PixelFormat))
				{
					BitmapData writer = fix.LockBits(rect, ImageLockMode.WriteOnly, image.PixelFormat);
					BitmapData reader = image.LockBits(rect, ImageLockMode.ReadOnly, image.PixelFormat);

					// Create a buffer that will hold the entire image's data
					int length = Math.Abs(reader.Stride) * image.Height;
					byte[] data = new byte[length];

					// Copy from the original image into the buffer
					Marshal.Copy(reader.Scan0, data, 0, length);
					// Copy from the buffer into the new image
					Marshal.Copy(data, 0, writer.Scan0, length);

					// Release the image bits
					image.UnlockBits(reader);
					fix.UnlockBits(writer);

					// Finally save the new image to our file-stream.
					fix.Save(retVal, _tiffEncoder, null);
				}
			}

			retVal.Position = 0;
			return retVal;
		}

		/// <summary>
		/// Gets the Compression Encoder Parameter for the specified Compression Option.
		/// </summary>
		/// <param name="compressionOption">The compression option which specifies which Compression Encoder to use.</param>
		/// <returns>Return the Compression Encoder Parameter for the specified Compression Option.</returns>
		private static EncoderParameter GetCompresionParameter(CompressionOptions compressionOption)
		{
			EncoderParameter retVal;

			switch (compressionOption)
			{
				case CompressionOptions.RLE:
					retVal = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionRle);
					break;
				case CompressionOptions.LZW:
					retVal = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionLZW);
					break;
				case CompressionOptions.CCITT3:
					retVal = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionCCITT3);
					break;
				case CompressionOptions.CCITT4:
					retVal = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionCCITT4);
					break;
				default:
					retVal = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionNone);
					break;
			}

			return retVal;
		}

		#region overloads

		/// <summary>
		/// Separates each page of the image into a separate image file.
		/// </summary>
		/// <param name="fileName">The filename of the original image.</param>
		/// <returns>Returns an array of strings containing the filenames of the resulting image files.</returns>
		public static string[] SeparateImages(string fileName)
		{
			return SeparateImages(fileName, CompressionOptions.None);
		}

		/// <summary>
		/// Splits a Multipage Tiff image into multiple files.
		/// </summary>
		/// <param name="fileName">The filename of the Tiff Image to split</param>
		/// <returns>Returns an array of strings containing the filenames of the resulting image files.</returns>
		public static string[] SplitImage(string fileName)
		{
			return SplitImage(fileName, new DefaultSplitResolver(), ImageSplitOptions.ExcludeSplitterPage, CompressionOptions.None);
		}

		/// <summary>
		/// Splits a Multipage Tiff image into multiple files.
		/// </summary>
		/// <param name="fileName">The filename of the Tiff Image to split</param>
		/// <param name="resolver">The Split Resolver used to determine where to split the image.</param>
		/// <returns>Returns an array of strings containing the filenames of the resulting image files.</returns>
		public static string[] SplitImage(string fileName, ISplitResolver resolver)
		{
			return SplitImage(fileName, resolver, ImageSplitOptions.ExcludeSplitterPage, CompressionOptions.None);
		}

		/// <summary>
		/// Splits a Multipage Tiff image into multiple files.
		/// </summary>
		/// <param name="fileName">The filename of the Tiff Image to split</param>
		/// <param name="imageSplitOption">The split option used to determine if and where the slitter page will be included in the resulting image files.</param>
		/// <returns>Returns an array of strings containing the filenames of the resulting image files.</returns>
		public static string[] SplitImage(string fileName, ImageSplitOptions imageSplitOption)
		{
			return SplitImage(fileName, new DefaultSplitResolver(), imageSplitOption, CompressionOptions.None);
		}

		/// <summary>
		/// Splits a Multipage Tiff image into multiple files.
		/// </summary>
		/// <param name="fileName">The filename of the Tiff Image to split</param>
		/// <param name="compressionOption">The compression option used to compress the resulting image files.</param>
		/// <returns>Returns an array of strings containing the filenames of the resulting image files.</returns>
		public static string[] SplitImage(string fileName, CompressionOptions compressionOption)
		{
			return SplitImage(fileName, new DefaultSplitResolver(), ImageSplitOptions.ExcludeSplitterPage, compressionOption);
		}

		/// <summary>
		/// Splits a Multipage Tiff image into multiple files.
		/// </summary>
		/// <param name="fileName">The filename of the Tiff Image to split</param>
		/// <param name="resolver">The Split Resolver used to determine where to split the image.</param>
		/// <param name="compressionOption">The compression option used to compress the resulting image files.</param>
		/// <returns>Returns an array of strings containing the filenames of the resulting image files.</returns>
		public static string[] SplitImage(string fileName, ISplitResolver resolver, CompressionOptions compressionOption)
		{
			return SplitImage(fileName, resolver, ImageSplitOptions.ExcludeSplitterPage, compressionOption);
		}

		/// <summary>
		/// Splits a Multipage Tiff image into multiple files.
		/// </summary>
		/// <param name="fileName">The filename of the Tiff Image to split</param>
		/// <param name="imageSplitOption">The split option used to determine if and where the slitter page will be included in the resulting image files.</param>
		/// <param name="compressionOption">The compression option used to compress the resulting image files.</param>
		/// <returns>Returns an array of strings containing the filenames of the resulting image files.</returns>
		public static string[] SplitImage(string fileName, ImageSplitOptions imageSplitOption, CompressionOptions compressionOption)
		{
			return SplitImage(fileName, new DefaultSplitResolver(), imageSplitOption, compressionOption);
		}

		#endregion

	}

	/// <summary>
	/// Image Split Options
	/// </summary>
	public enum ImageSplitOptions
	{
		/// <summary>
		/// Excludes the splitter page from the resulting images.
		/// </summary>
		ExcludeSplitterPage,
		/// <summary>
		/// Includes the splitter page, when found, as the first page in the resulting images.
		/// </summary>
		IncludeSplitterPageBefore,
		/// <summary>
		/// Includes the splitter page, when found, as the last page in the resulting images.
		/// </summary>
		IncludeSplitterPageAfter
	}

	/// <summary>
	/// Image Compression Options
	/// </summary>
	public enum CompressionOptions
	{
		/// <summary>
		/// No Compression
		/// </summary>
		None,
		/// <summary>
		/// Rle Compression
		/// </summary>
		RLE,
		/// <summary>
		/// LZW Compression
		/// </summary>
		LZW,
		/// <summary>
		/// CCITT3 Compression
		/// </summary>
		CCITT3,
		/// <summary>
		/// CCITT4 Compression
		/// </summary>
		CCITT4
	}
}
