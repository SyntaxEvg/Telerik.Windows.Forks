using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Telerik.Windows.Documents.Core.Imaging
{
	class PngImageSizeDecoder : ImageSizeDecoderBase
	{
		protected override IEnumerable<string> SupportedExtensions
		{
			get
			{
				return PngImageSizeDecoder.supportedExtensions;
			}
		}

		protected override int SignatureLength
		{
			get
			{
				return PngImageSizeDecoder.signature.Length;
			}
		}

		protected override int BytesToSkipAfterSignature
		{
			get
			{
				return 8;
			}
		}

		protected override bool IsValidImage(Stream stream)
		{
			return stream.Length > 24L && ImageSizeDecoderBase.IsSignatureValid(stream, PngImageSizeDecoder.signature);
		}

		protected override bool IsValidImage(byte[] bytes)
		{
			return bytes.Length > 24 && ImageSizeDecoderBase.IsSignatureValid(bytes, PngImageSizeDecoder.signature);
		}

		protected override Size DecodeSizeInternal(byte[] bytes)
		{
			int num = ImageSizeDecoderBase.ReadLittleEndianInt32(bytes, base.GetSizeStartIndexInBytes());
			int num2 = ImageSizeDecoderBase.ReadLittleEndianInt32(bytes, base.GetSizeStartIndexInBytes() + 4);
			return new Size((double)num, (double)num2);
		}

		protected override Size DecodeSizeInternal(Stream stream)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			stream.Seek(0L, SeekOrigin.Begin);
			binaryReader.ReadBytes(base.GetSizeStartIndexInBytes());
			int num = ImageSizeDecoderBase.ReadLittleEndianInt32(binaryReader);
			int num2 = ImageSizeDecoderBase.ReadLittleEndianInt32(binaryReader);
			return new Size((double)num, (double)num2);
		}

		const int PngImageSizeMinimumBytesCount = 24;

		static readonly string[] supportedExtensions = new string[] { "png" };

		static readonly byte[] signature = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };
	}
}
