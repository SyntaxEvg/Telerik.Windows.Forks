using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Telerik.Windows.Documents.Core.Imaging
{
	class BitmapImageSizeDecoder : ImageSizeDecoderBase
	{
		protected override IEnumerable<string> SupportedExtensions
		{
			get
			{
				return BitmapImageSizeDecoder.supportedExtensions;
			}
		}

		protected override int SignatureLength
		{
			get
			{
				return BitmapImageSizeDecoder.signature.Length;
			}
		}

		protected override int BytesToSkipAfterSignature
		{
			get
			{
				return 16;
			}
		}

		protected override bool IsValidImage(Stream stream)
		{
			return stream.Length > 26L && ImageSizeDecoderBase.IsSignatureValid(stream, BitmapImageSizeDecoder.signature);
		}

		protected override bool IsValidImage(byte[] bytes)
		{
			return bytes.Length > 26 && ImageSizeDecoderBase.IsSignatureValid(bytes, BitmapImageSizeDecoder.signature);
		}

		protected override Size DecodeSizeInternal(byte[] bytes)
		{
			int num = ImageSizeDecoderBase.ReadInt32(bytes, base.GetSizeStartIndexInBytes());
			int num2 = ImageSizeDecoderBase.ReadInt32(bytes, base.GetSizeStartIndexInBytes() + 4);
			return new Size((double)num, (double)num2);
		}

		protected override Size DecodeSizeInternal(Stream stream)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			stream.Seek(0L, SeekOrigin.Begin);
			binaryReader.ReadBytes(base.GetSizeStartIndexInBytes());
			int num = binaryReader.ReadInt32();
			int num2 = binaryReader.ReadInt32();
			return new Size((double)num, (double)num2);
		}

		const int BitmapImageSizeMinimumBytesCount = 26;

		static readonly string[] supportedExtensions = new string[] { "bmp" };

		static readonly byte[] signature = new byte[] { 66, 77 };
	}
}
