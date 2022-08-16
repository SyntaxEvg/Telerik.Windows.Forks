using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Telerik.Windows.Documents.Core.Imaging
{
	static class ImageSizeDecodersManager
	{
		static ImageSizeDecodersManager()
		{
			ImageSizeDecodersManager.InitializeDefaultDecoders();
		}

		public static Size GetSize(Stream stream, string imageTypeExtension)
		{
			IImageSizeDecoder imageSizeDecoder = (from dec in ImageSizeDecodersManager.registeredDecoders
				where dec.CanDecode(imageTypeExtension)
				select dec).FirstOrDefault<IImageSizeDecoder>();
			if (imageSizeDecoder != null)
			{
				return imageSizeDecoder.DecodeSize(stream);
			}
			return ImageSizeDecoderBase.DefaultNotDecodedImageSize;
		}

		public static Size GetSize(byte[] imageBytes, string imageTypeExtension)
		{
			IImageSizeDecoder imageSizeDecoder = (from dec in ImageSizeDecodersManager.registeredDecoders
				where dec.CanDecode(imageTypeExtension)
				select dec).FirstOrDefault<IImageSizeDecoder>();
			if (imageSizeDecoder != null)
			{
				return imageSizeDecoder.DecodeSize(imageBytes);
			}
			return ImageSizeDecoderBase.DefaultNotDecodedImageSize;
		}

		static void InitializeDefaultDecoders()
		{
			ImageSizeDecodersManager.registeredDecoders.Add(new PngImageSizeDecoder());
			ImageSizeDecodersManager.registeredDecoders.Add(new JpegImageSizeDecoder());
			ImageSizeDecodersManager.registeredDecoders.Add(new GifImageSizeDecoder());
			ImageSizeDecodersManager.registeredDecoders.Add(new BitmapImageSizeDecoder());
			ImageSizeDecodersManager.registeredDecoders.Add(new TifImageSizeDecoder());
			ImageSizeDecodersManager.registeredDecoders.Add(new MetafileImageSizeDecoder());
		}

		static readonly HashSet<IImageSizeDecoder> registeredDecoders = new HashSet<IImageSizeDecoder>();
	}
}
