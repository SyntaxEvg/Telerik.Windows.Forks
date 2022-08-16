using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Telerik.Windows.Documents.Core.Imaging.Jpeg;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;

namespace Telerik.Windows.Documents.Core.Imaging
{
	class JpegImageSizeDecoder : ImageSizeDecoderBase
	{
		protected override IEnumerable<string> SupportedExtensions
		{
			get
			{
				return JpegImageSizeDecoder.supportedExtensions;
			}
		}

		protected override Size DecodeSizeInternal(Stream stream)
		{
			JpegHeadersDecoder decoder = new JpegHeadersDecoder(stream);
			return JpegImageSizeDecoder.DecodeSizeCore(decoder);
		}

		protected override Size DecodeSizeInternal(byte[] bytes)
		{
			JpegHeadersDecoder decoder = new JpegHeadersDecoder(bytes);
			return JpegImageSizeDecoder.DecodeSizeCore(decoder);
		}

		static Size DecodeSizeCore(JpegHeadersDecoder decoder)
		{
			JpegImageInfo jpegImageInfo;
			if (decoder.TryDecodeJpegImage(out jpegImageInfo))
			{
				return new Size((double)jpegImageInfo.Width, (double)jpegImageInfo.Height);
			}
			return ImageSizeDecoderBase.DefaultNotDecodedImageSize;
		}

		static readonly string[] supportedExtensions = new string[] { "jpg", "jpeg" };
	}
}
