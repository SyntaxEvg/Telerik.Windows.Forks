using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging
{
	abstract class ImageSizeDecoderBase : IImageSizeDecoder
	{
		protected abstract IEnumerable<string> SupportedExtensions { get; }

		protected virtual int SignatureLength
		{
			get
			{
				return 0;
			}
		}

		protected virtual int BytesToSkipAfterSignature
		{
			get
			{
				return 0;
			}
		}

		public bool CanDecode(string imageTypeExtension)
		{
			Guard.ThrowExceptionIfNull<string>(imageTypeExtension, "imageTypeExtension");
			return this.SupportedExtensions.Any((string extension) => string.Equals(extension, imageTypeExtension.Trim().Trim(new char[] { '.' }), StringComparison.OrdinalIgnoreCase));
		}

		public Size DecodeSize(byte[] bytes)
		{
			if (this.IsValidImage(bytes))
			{
				return this.DecodeSizeInternal(bytes);
			}
			return ImageSizeDecoderBase.DefaultNotDecodedImageSize;
		}

		public Size DecodeSize(Stream stream)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			Guard.ThrowExceptionIfFalse(stream.CanSeek, "Cannot decode non-seekable stream.");
			Size result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				stream.Seek(0L, SeekOrigin.Begin);
				stream.CopyTo(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				if (this.IsValidImage(memoryStream))
				{
					result = this.DecodeSizeInternal(memoryStream);
				}
				else
				{
					result = ImageSizeDecoderBase.DefaultNotDecodedImageSize;
				}
			}
			return result;
		}

		protected abstract Size DecodeSizeInternal(Stream stream);

		protected abstract Size DecodeSizeInternal(byte[] bytes);

		protected virtual bool IsValidImage(Stream stream)
		{
			return true;
		}

		protected virtual bool IsValidImage(byte[] bytes)
		{
			return true;
		}

		protected int GetSizeStartIndexInBytes()
		{
			return this.SignatureLength + this.BytesToSkipAfterSignature;
		}

		protected static bool IsSignatureValid(byte[] bytes, byte[] signature)
		{
			if (bytes.Length >= signature.Length)
			{
				for (int i = 0; i < signature.Length; i++)
				{
					if (bytes[i] != signature[i])
					{
						return false;
					}
				}
			}
			return true;
		}

		protected static bool IsSignatureValid(Stream stream, byte[] signature)
		{
			stream.Seek(0L, SeekOrigin.Begin);
			BinaryReader binaryReader = new BinaryReader(stream);
			byte[] bytes = binaryReader.ReadBytes(signature.Length);
			return ImageSizeDecoderBase.IsSignatureValid(bytes, signature);
		}

		protected static int ReadLittleEndianInt32(BinaryReader binaryReader)
		{
			byte[] array = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				array[3 - i] = binaryReader.ReadByte();
			}
			return BitConverter.ToInt32(array, 0);
		}

		protected static int ReadLittleEndianInt32(byte[] imageBytes, int startIndex)
		{
			byte[] array = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				array[3 - i] = imageBytes[i + startIndex];
			}
			return BitConverter.ToInt32(array, 0);
		}

		protected static int ReadInt16(byte[] imageBytes, int startIndex)
		{
			byte[] array = new byte[2];
			for (int i = 0; i < 2; i++)
			{
				array[i] = imageBytes[i + startIndex];
			}
			return (int)BitConverter.ToInt16(array, 0);
		}

		protected static int ReadInt32(byte[] imageBytes, int startIndex)
		{
			byte[] array = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = imageBytes[i + startIndex];
			}
			return BitConverter.ToInt32(array, 0);
		}

		public static readonly Size DefaultNotDecodedImageSize = Size.Empty;
	}
}
