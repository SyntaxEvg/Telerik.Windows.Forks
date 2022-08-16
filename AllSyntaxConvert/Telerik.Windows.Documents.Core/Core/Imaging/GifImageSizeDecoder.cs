using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Telerik.Windows.Documents.Core.Imaging
{
	class GifImageSizeDecoder : ImageSizeDecoderBase
	{
		protected override IEnumerable<string> SupportedExtensions
		{
			get
			{
				return GifImageSizeDecoder.supportedExtensions;
			}
		}

		protected override int SignatureLength
		{
			get
			{
				return GifImageSizeDecoder.signature1.Length;
			}
		}

		protected override bool IsValidImage(Stream stream)
		{
			return stream.Length > 10L && (ImageSizeDecoderBase.IsSignatureValid(stream, GifImageSizeDecoder.signature1) || ImageSizeDecoderBase.IsSignatureValid(stream, GifImageSizeDecoder.signature2));
		}

		protected override bool IsValidImage(byte[] bytes)
		{
			return bytes.Length > 10 && (ImageSizeDecoderBase.IsSignatureValid(bytes, GifImageSizeDecoder.signature1) || ImageSizeDecoderBase.IsSignatureValid(bytes, GifImageSizeDecoder.signature2));
		}

		protected override Size DecodeSizeInternal(byte[] bytes)
		{
			int num = ImageSizeDecoderBase.ReadInt16(bytes, base.GetSizeStartIndexInBytes());
			int num2 = ImageSizeDecoderBase.ReadInt16(bytes, base.GetSizeStartIndexInBytes() + 2);
			return new Size((double)num, (double)num2);
		}

		protected override Size DecodeSizeInternal(Stream stream)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			int num = (int)binaryReader.ReadInt16();
			int num2 = (int)binaryReader.ReadInt16();
			return new Size((double)num, (double)num2);
		}

		const int GifImageSizeMinimumBytesCount = 10;

		static readonly string[] supportedExtensions = new string[] { "gif" };

		static readonly byte[] signature1 = new byte[] { 71, 73, 70, 56, 55, 97 };

		static readonly byte[] signature2 = new byte[] { 71, 73, 70, 56, 57, 97 };
	}
}
