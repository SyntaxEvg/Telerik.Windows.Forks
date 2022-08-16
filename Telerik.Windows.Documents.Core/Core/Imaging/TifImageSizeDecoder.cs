using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Telerik.Windows.Documents.Core.Imaging
{
	class TifImageSizeDecoder : ImageSizeDecoderBase
	{
		protected override IEnumerable<string> SupportedExtensions
		{
			get
			{
				return TifImageSizeDecoder.supportedExtensions;
			}
		}

		protected override Size DecodeSizeInternal(Stream stream)
		{
			return TifImageSizeDecoder.Decode(stream);
		}

		protected override Size DecodeSizeInternal(byte[] bytes)
		{
			Size result;
			using (Stream stream = new MemoryStream(bytes))
			{
				result = TifImageSizeDecoder.Decode(stream);
			}
			return result;
		}

		static Size Decode(Stream stream)
		{
			Size result;
			try
			{
				BitmapDecoder bitmapDecoder = BitmapDecoder.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
				BitmapFrame bitmapFrame = bitmapDecoder.Frames[0];
				result = new Size((double)bitmapFrame.PixelWidth, (double)bitmapFrame.PixelHeight);
			}
			catch (NotSupportedException)
			{
				result = ImageSizeDecoderBase.DefaultNotDecodedImageSize;
			}
			return result;
		}

		static readonly IEnumerable<string> supportedExtensions = new string[] { "tif", "tiff" };
	}
}
