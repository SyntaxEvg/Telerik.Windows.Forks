using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;

namespace Telerik.Windows.Documents.Core.Imaging
{
	class MetafileImageSizeDecoder : ImageSizeDecoderBase
	{
		protected override IEnumerable<string> SupportedExtensions
		{
			get
			{
				return MetafileImageSizeDecoder.supportedExtensions;
			}
		}

		protected override System.Windows.Size DecodeSizeInternal(Stream stream)
		{
			return MetafileImageSizeDecoder.Decode(stream);
		}

		protected override System.Windows.Size DecodeSizeInternal(byte[] bytes)
		{
			System.Windows.Size result;
			using (Stream stream = new MemoryStream(bytes))
			{
				result = MetafileImageSizeDecoder.Decode(stream);
			}
			return result;
		}

		static System.Windows.Size Decode(Stream stream)
		{
			System.Windows.Size result;
			try
			{
				using (Image image = Image.FromStream(stream))
				{
					result = new System.Windows.Size((double)image.Width, (double)image.Height);
				}
			}
			catch (ArgumentException)
			{
				result = ImageSizeDecoderBase.DefaultNotDecodedImageSize;
			}
			return result;
		}

		static readonly IEnumerable<string> supportedExtensions = new string[] { "emf", "wmf", "exif", "ico" };
	}
}
