using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace System.Drawing
{

	/// <summary>
	/// This class is a modified Bar-Code reader taken from http://www.codeproject.com/script/Articles/ViewDownloads.aspx?aid=10734
	/// 
	/// Modifications:
	///  - Return List&lt;string&gt; instead of taking an ArrayList as a ref parameter.
	///  - Use BarCodeMap rather than a method with a large switch statement.
	///  - Determine wide/thin bar based on average bar width rather than arbitrarily using the first narrow bar's width.
	///  - Use StringBuilder rather than string concats.
	/// 
	/// Original code authored by "qlipoth" (http://www.codeproject.com/script/Membership/View.aspx?mid=1770204)
	/// </summary>
	/// <remarks>This class is used only internally to find bar codes for the BarCodeSplitResolver class</remarks>
	internal static class BarcodeImaging
	{
		/// <summary>
		/// Structure used to return the processed data from an image's histogram
		/// </summary>
		private struct HistogramResult
		{
			public float[] histogram;
			public float min;
			public float max;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bmp"></param>
		/// <param name="numscans"></param>
		/// <returns></returns>
		public static List<string> VScanPageCode39(Bitmap bmp, int numscans)
		{
			List<string> retVal = new List<string>();

			for (int i = 0; i < numscans; i++)
			{
				retVal.AddIf(ReadCode39(bmp, i * (bmp.Height / numscans), (i * (bmp.Height / numscans)) + (bmp.Height / numscans)),
					read => (!string.IsNullOrEmpty(read) && !retVal.Contains(read)));
			}

			return retVal;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bmp"></param>
		/// <param name="startheight"></param>
		/// <param name="endheight"></param>
		/// <returns></returns>
		public static string ReadCode39(Image bmp, int startheight, int endheight)
		{
			// To find a horizontal barcode, find the vertical histogram to find individual barcodes, 
			// then get the vertical histogram to decode each
			HistogramResult vertHist = GetVerticalHistogram(bmp as Bitmap, startheight, endheight);

			// Set the threshold for determining dark/light bars to half way between the histograms min/max
			float threshold = vertHist.min + ((vertHist.max - vertHist.min) / 2);

			List<int> bars = new List<int>();
			int barStart = -1;
			bool isDarkBar = false;

			for (int idx = 0; idx < vertHist.histogram.Length; idx++)
			{
				if (vertHist.histogram[idx] <= threshold)
				{
					// Dark
					if (barStart < 0)
					{
						// First dark stripe.
						barStart = idx;
						isDarkBar = true;
					}
					else if (!isDarkBar)
					{
						// Light bar has ended, record width.
						bars.Add(idx - barStart);
						barStart = idx;
						isDarkBar = true;
					}
				}
				else
				{
					// Light 
					if (isDarkBar)
					{
						// Dark bar has ended, record width.
						bars.Add(idx - barStart);
						barStart = idx;
						isDarkBar = false;
					}
				}
			}

			int minWidth = int.MaxValue;
			int maxWidth = 0;

			bars.ForEach(bar =>
			{
				if (bar < minWidth) minWidth = bar;
				if (bar > maxWidth) maxWidth = bar;
			});

			int barThreshold = minWidth + ((maxWidth - minWidth) / 2);

			int mod = bars.Count % 10;
			if (mod > 0)
			{
				mod = 10 - mod;
				for (int idx = 0; idx < mod; idx++)
				{
					bars.Add(0);
				}
			}

			StringBuilder retVal = new StringBuilder();

			for (int idx = 0; idx < bars.Count; idx += 10)
			{
				int key = 0;
				for (int pos = 0; pos < 9; pos++)
				{
					if (bars[idx + pos] > barThreshold) key += (int)Math.Pow(2, pos);
				}

				retVal.Append(BarcodeMap.Code39Map.GetValue(key));
			}

			return retVal.ToString();
		}

		/// <summary>
		/// Vertical histogram of an image
		/// </summary>
		/// <param name="bmp">the bitmap to be processed</param>
		/// <param name="startheight"></param>
		/// <param name="endheight"></param>
		/// <returns>a histogramResult representing the vertical histogram</returns>
		//[SuppressMessage("Microsoft.Security", "CA2122")]
		private static HistogramResult GetVerticalHistogram(Bitmap bmp, int startheight, int endheight)
		{
			if (endheight <= 0) endheight = 1;

			// Create the return value
			float[] histResult = new float[bmp.Width];

			float[] vertSum = new float[bmp.Width];
			// Start the max value at zero
			float maxValue = 0;
			// Start the min value at the absolute maximum
			float minValue = 255;

			// GDI+ still lies to us - the return format is BGR, NOT RGB.
			BitmapData bmData = bmp.LockBits(new Rectangle(0, startheight, bmp.Width, endheight - startheight), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

			int stride = bmData.Stride;
			System.IntPtr Scan0 = bmData.Scan0;

			unsafe
			{

				byte* p = (byte*)(void*)Scan0;

				int nOffset = stride - bmp.Width * 3;
				//int nWidth = bmp.Width * 3;

				for (int y = startheight; y < endheight; ++y)
				{
					for (int x = 0; x < bmp.Width; ++x)
					{
						// Add up all the pixel values vertically (average the R,G,B channels)
						vertSum[x] += ((p[0] + (p + 1)[0] + (p + 2)[0]) / 3);

						p += 3;
					}
					p += nOffset;
				}
			}

			bmp.UnlockBits(bmData);

			// Now get the average of the row by dividing the pixel by num pixels
			for (int i = 0; i < bmp.Width; i++)
			{
				histResult[i] = (vertSum[i] / (endheight - startheight));
				//Save the max value for later
				if (histResult[i] > maxValue) maxValue = histResult[i];
				// Save the min value for later
				if (histResult[i] < minValue) minValue = histResult[i];
			}

			HistogramResult retVal = new HistogramResult();
			retVal.histogram = histResult;
			retVal.max = maxValue;
			retVal.min = minValue;
			return retVal;
		}
	}

	/// <summary>
	/// Represents a map of Bar-Code values to String values.
	/// </summary>
	[Serializable]
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
		Justification = "Map is the correct suffix.")]
	public class BarcodeMap : Dictionary<int, string>
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public BarcodeMap() { }
		/// <summary>
		/// Serialization Constructor.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected BarcodeMap(SerializationInfo info, StreamingContext context) : base(info, context) { }

		private static BarcodeMap _code39Map;
		/// <summary>
		/// Gets the Bar-Code Map for Code 39 Bar-Codes.
		/// </summary>
		public static BarcodeMap Code39Map
		{
			get
			{
				if (_code39Map == null)
				{
					_code39Map = XmlSerializer<BarcodeMap>.DeserializeFromResource(Assembly.GetExecutingAssembly(), "System.Drawing.CodeMaps.Code39Map.xml");
				}
				return _code39Map;
			}
		}

		/// <summary>
		/// Gets the value for the specified key.  If the key does not exists, returns an empty string.
		/// </summary>
		/// <param name="key">The bar-code key.</param>
		public string GetValue(int key)
		{
			string retVal = string.Empty;
			if (ContainsKey(key)) retVal = this[key];
			return retVal;
		}
	}
}