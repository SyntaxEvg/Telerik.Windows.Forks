using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Windows.Documents.Core.Imaging.Jpeg;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.ColorSpaces;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder;
using Telerik.Windows.Documents.Fixed.Model.Extensions;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters
{
	public class DctDecode : IPdfFilter
	{
		public string Name
		{
			get
			{
				return "DCTDecode";
			}
		}

		public byte[] Encode(PdfObject encodedObject, byte[] inputData)
		{
			Guard.ThrowExceptionIfNull<PdfObject>(encodedObject, "encodedObject");
			Guard.ThrowExceptionIfNull<byte[]>(inputData, "inputData");
			JpegColorSpace jpegColorSpace = DctDecode.GetJpegColorSpace(encodedObject.ColorSpace);
			return DctDecode.Encode(encodedObject.Width, encodedObject.Height, (float)encodedObject.ExportContext.Settings.ImageQuality, jpegColorSpace, inputData);
		}

		internal static byte[] Encode(int width, int height, float imageQuality, JpegColorSpace colorSpace, byte[] inputData)
		{
			Guard.ThrowExceptionIfNull<byte[]>(inputData, "inputData");
			JpegImage jpegImage = new JpegImage(width, height, colorSpace, inputData);
			JpegEncoder jpegEncoder = new JpegEncoder(jpegImage, new JpegEncoderParameters
			{
				QuantizingQuality = imageQuality
			});
			return jpegEncoder.Encode();
		}

		public virtual byte[] Decode(PdfObject decodedObject, byte[] inputData, DecodeParameters parms)
		{
			Guard.ThrowExceptionIfNull<PdfObject>(decodedObject, "decodedObject");
			Guard.ThrowExceptionIfNull<byte[]>(inputData, "inputData");
			return DctDecode.DecodeWithBitmapImage(decodedObject, inputData);
		}

		protected static byte[] DecodeWithJpegDecoder(byte[] inputData)
		{
			JpegDecoder jpegDecoder = new JpegDecoder(inputData);
			JpegImage jpegImage = jpegDecoder.Decode();
			return jpegImage.GetData();
		}

		static JpegColorSpace GetJpegColorSpace(ColorSpace colorSpace)
		{
			switch (colorSpace)
			{
			case ColorSpace.Gray:
				return JpegColorSpace.Grayscale;
			case ColorSpace.RGB:
				return JpegColorSpace.Rgb;
			default:
				throw new NotSupportedException(string.Format("Not supported colorspace: {0}", colorSpace));
			}
		}

		static byte[] DecodeWithBitmapImage(PdfObject decodedObject, byte[] inputData)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(inputData))
			{
				BitmapSource bitmap = DctDecode.GetBitmap(memoryStream, decodedObject);
				ColorSpace colorSpace = decodedObject.ColorSpace;
				byte[] array;
				if (colorSpace == ColorSpace.Gray && DctDecode.TryGetGray8DataFromImage(bitmap, out array))
				{
					result = array;
				}
				else
				{
					IEnumerable<int> enumerable = ((colorSpace == ColorSpace.CMYK) ? ImageSourceExtensions.GetCmykPixels(bitmap) : ImageSourceExtensions.GetPixels(bitmap));
					int numberOfComponents = DctDecode.GetNumberOfComponents(colorSpace);
					bool ifCmykColorsShouldBeInverted = DctDecode.GetIfCmykColorsShouldBeInverted(colorSpace, inputData);
					Func<int, IEnumerable<byte>> pixelToDataConverter = DctDecode.GetPixelToDataConverter(colorSpace, ifCmykColorsShouldBeInverted);
					int num = bitmap.PixelWidth * bitmap.PixelHeight;
					array = new byte[num * numberOfComponents];
					int num2 = 0;
					foreach (int arg in enumerable)
					{
						foreach (byte b in pixelToDataConverter(arg))
						{
							array[num2] = b;
							num2++;
						}
					}
					result = array;
				}
			}
			return result;
		}

		static bool GetIfCmykColorsShouldBeInverted(ColorSpace colorSpace, byte[] inputData)
		{
			bool result = false;
			if (colorSpace == ColorSpace.CMYK)
			{
				JpegHeadersDecoder jpegHeadersDecoder = new JpegHeadersDecoder(inputData);
				JpegImageInfo jpegImageInfo;
				result = jpegHeadersDecoder.TryDecodeJpegImage(out jpegImageInfo) && jpegImageInfo.HasAdobeInvertedColors;
			}
			return result;
		}

		static BitmapSource GetBitmap(Stream imageStream, PdfObject decodedObject)
		{
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.BeginInit();
			bitmapImage.StreamSource = imageStream;
			bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
			bitmapImage.DecodePixelWidth = decodedObject.Width;
			bitmapImage.EndInit();
			return bitmapImage;
		}

		static bool TryGetGray8DataFromImage(BitmapSource source, out byte[] resultData)
		{
			if (source.Format == PixelFormats.Gray8)
			{
				resultData = new byte[source.PixelWidth * source.PixelHeight];
				source.CopyPixels(resultData, source.PixelWidth, 0);
				return true;
			}
			resultData = null;
			return false;
		}

		static int GetNumberOfComponents(ColorSpace colorSpace)
		{
			switch (colorSpace)
			{
			case ColorSpace.Gray:
				return 1;
			case ColorSpace.RGB:
				return 3;
			case ColorSpace.CMYK:
				return 4;
			default:
				throw new NotSupportedException(string.Format("Not supported colorspace: {0}", colorSpace));
			}
		}

		static Func<int, IEnumerable<byte>> GetPixelToDataConverter(ColorSpace colorSpace, bool inverseCmykData)
		{
			switch (colorSpace)
			{
			case ColorSpace.Gray:
				return new Func<int, IEnumerable<byte>>(DctDecode.GetGrayDataFromPixel);
			case ColorSpace.RGB:
				return new Func<int, IEnumerable<byte>>(DctDecode.GetRgbDataFromPixel);
			case ColorSpace.CMYK:
				if (inverseCmykData)
				{
					return new Func<int, IEnumerable<byte>>(DctDecode.GetInversedCmykDataFromPixel);
				}
				return new Func<int, IEnumerable<byte>>(DctDecode.GetCmykDataFromPixel);
			default:
				throw new NotSupportedException(string.Format("Not supported colorspace: {0}", colorSpace));
			}
		}

		static IEnumerable<byte> GetCmykDataFromPixel(int pixel)
		{
			yield return (byte)(pixel & 255);
			yield return (byte)((pixel >> 8) & 255);
			yield return (byte)((pixel >> 16) & 255);
			yield return (byte)((pixel >> 24) & 255);
			yield break;
		}

		static IEnumerable<byte> GetInversedCmykDataFromPixel(int pixel)
		{
			foreach (byte component in DctDecode.GetCmykDataFromPixel(pixel))
			{
				yield return byte.MaxValue - component;
			}
			yield break;
		}

		static IEnumerable<byte> GetGrayDataFromPixel(int pixel)
		{
			yield return (byte)(pixel & 255);
			yield break;
		}

		static IEnumerable<byte> GetRgbDataFromPixel(int pixel)
		{
			yield return (byte)((pixel >> 16) & 255);
			yield return (byte)((pixel >> 8) & 255);
			yield return (byte)(pixel & 255);
			yield break;
		}
	}
}
