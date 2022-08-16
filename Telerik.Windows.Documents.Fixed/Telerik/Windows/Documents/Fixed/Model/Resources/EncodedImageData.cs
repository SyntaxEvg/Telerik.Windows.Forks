using System;
using System.Runtime.CompilerServices;
using Telerik.Windows.Documents.Core.Imaging.Jpeg;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.ColorSpaces;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;
using Telerik.Windows.Documents.Core.Imaging.Jpeg2000;
using Telerik.Windows.Documents.Core.Imaging.Jpeg2000.Decoder;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Resources
{
	public class EncodedImageData
	{
		public byte[] Data { get; set; }

		public byte[] AlphaChannel { get; set; }

		public int BitsPerComponent { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public string ColorSpace { get; set; }

		public string[] Filters { get; set; }

		internal double[] DecodeArray { get; set; }

		public EncodedImageData(byte[] data, int bitsPerComponent, int width, int height, string colorSpace, string[] filters)
			: this(data, null, bitsPerComponent, width, height, colorSpace, filters)
		{
		}

		public EncodedImageData(byte[] data, byte[] alphaChannel, int bitsPerComponent, int width, int height, string colorSpace, string[] filters)
		{
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			this.ColorSpace = colorSpace;
			this.Filters = filters;
			this.Data = data;
			this.AlphaChannel = alphaChannel;
			this.BitsPerComponent = bitsPerComponent;
			this.Width = width;
			this.Height = height;
		}

		internal static bool TryCreateFromUnknownImageData(byte[] unknownImageData, out EncodedImageData encodedImageData)
		{
			encodedImageData = null;
			return EncodedImageData.TryCreateJpegEncodedImagaData(unknownImageData, out encodedImageData) || EncodedImageData.TryCreateJpeg2000EncodedImagaData(unknownImageData, out encodedImageData);
		}

		static bool TryCreateJpegEncodedImagaData(byte[] data, out EncodedImageData encodedImageData)
		{
			JpegHeadersDecoder jpegHeadersDecoder = new JpegHeadersDecoder(data);
			JpegImageInfo jpegImageInfo;
			string colorSpace;
			if (jpegHeadersDecoder.TryDecodeJpegImage(out jpegImageInfo) && EncodedImageData.TryGetJpegColorspace(jpegImageInfo.ColorSpace, out colorSpace))
			{
				encodedImageData = new EncodedImageData(data, 8, jpegImageInfo.Width, jpegImageInfo.Height, colorSpace, new string[] { "DCTDecode" });
				EncodedImageData encodedImageData2 = encodedImageData;
				object decodeArray;
				if (!jpegImageInfo.HasAdobeInvertedColors)
				{
					decodeArray = null;
				}
				else
				{
					RuntimeHelpers.InitializeArray(decodeArray = new double[8], fieldof(PImplD_6454440FC0214D9599F03BFB2485AF25.method0x6001c1c1).FieldHandle);
				}
				encodedImageData2.DecodeArray = decodeArray;
				return true;
			}
			encodedImageData = null;
			return false;
		}

		static bool TryCreateJpeg2000EncodedImagaData(byte[] data, out EncodedImageData encodedImageData)
		{
			Jpeg2000HeadersDecoder jpeg2000HeadersDecoder = new Jpeg2000HeadersDecoder(data);
			Jpeg2000ImageInfo jpeg2000ImageInfo;
			if (jpeg2000HeadersDecoder.TryDecodeJpeg2000Image(out jpeg2000ImageInfo))
			{
				encodedImageData = new EncodedImageData(data, 0, jpeg2000ImageInfo.Width, jpeg2000ImageInfo.Height, null, new string[] { "JPXDecode" });
				return true;
			}
			encodedImageData = null;
			return false;
		}

		static bool TryGetJpegColorspace(JpegColorSpace jpegColorspace, out string colorspace)
		{
			colorspace = null;
			if (jpegColorspace != JpegColorSpace.Grayscale)
			{
				if (jpegColorspace != JpegColorSpace.Rgb)
				{
					if (jpegColorspace == JpegColorSpace.Cmyk)
					{
						colorspace = "DeviceCMYK";
					}
				}
				else
				{
					colorspace = "DeviceRGB";
				}
			}
			else
			{
				colorspace = "DeviceGray";
			}
			return colorspace != null;
		}
	}
}
