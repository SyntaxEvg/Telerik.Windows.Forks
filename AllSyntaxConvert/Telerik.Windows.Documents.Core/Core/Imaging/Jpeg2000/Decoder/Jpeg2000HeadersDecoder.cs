using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Primitives;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg2000.Decoder
{
	class Jpeg2000HeadersDecoder
	{
		public Jpeg2000HeadersDecoder(byte[] data)
		{
			this.data = data;
		}

		bool HasReadAllHeaders
		{
			get
			{
				return this.Size != null && this.ColorSpace != null && this.NumberOfComponents != null;
			}
		}

		bool HasReadAnyHeader
		{
			get
			{
				return this.Size != null || this.ColorSpace != null || this.NumberOfComponents != null;
			}
		}

		SizeI? Size { get; set; }

		Jpeg2000ColorSpace? ColorSpace { get; set; }

		int? NumberOfComponents { get; set; }

		public bool TryDecodeJpeg2000Image(out Jpeg2000ImageInfo imageInfo)
		{
			Jpeg2000Reader reader = new Jpeg2000Reader(this.data);
			if (!Jpeg2000HeadersDecoder.TryReadJpeg2000StartOfImageMarker(reader))
			{
				imageInfo = null;
				return false;
			}
			foreach (Box box in this.ReadBoxes(reader))
			{
				if (box.Type.Equals("jp2h"))
				{
					this.ParseJpeg2000HeaderBox(box);
				}
				else if (box.Type.Equals("jp2c"))
				{
					this.ParseJpeg2000ContentBox(box);
				}
			}
			return this.TryCreateImageInfo(out imageInfo);
		}

		IEnumerable<Box> ReadBoxes(Jpeg2000Reader reader)
		{
			while (!reader.EndOfFile && !this.HasReadAllHeaders)
			{
				Box box = reader.ReadBox();
				yield return box;
			}
			yield break;
		}

		void ParseJpeg2000ContentBox(Box contentBox)
		{
			Jpeg2000Reader jpeg2000Reader = new Jpeg2000Reader(contentBox.Content);
			short num = jpeg2000Reader.ReadBigEndianShort();
			short num2 = jpeg2000Reader.ReadBigEndianShort();
			bool flag = num == -177 && num2 == -175;
			if (flag)
			{
				this.ReadSizMarkerHeaderBytes(jpeg2000Reader);
			}
		}

		void ReadSizMarkerHeaderBytes(Jpeg2000Reader reader)
		{
			ushort num = reader.ReadBigEndianUShort();
			ushort num2 = reader.ReadBigEndianUShort();
			int num3 = reader.ReadBigEndianInt();
			int num4 = reader.ReadBigEndianInt();
			int num5 = reader.ReadBigEndianInt();
			int num6 = reader.ReadBigEndianInt();
			int num7 = reader.ReadBigEndianInt();
			int num8 = reader.ReadBigEndianInt();
			int num9 = reader.ReadBigEndianInt();
			int num10 = reader.ReadBigEndianInt();
			ushort num11 = reader.ReadBigEndianUShort();
			bool flag = num > 0 && num2 <= 2 && num3 >= 0 && num4 >= 0 && num5 >= 0 && num6 >= 0 && num7 >= 0 && num8 >= 0 && num9 >= 0 && num10 >= 0 && num11 > 0;
			if (flag)
			{
				this.Size = new SizeI?(this.Size ?? new SizeI(num3, num4));
				this.NumberOfComponents = new int?((int)num11);
			}
		}

		void ParseJpeg2000HeaderBox(Box headerBox)
		{
			Jpeg2000Reader reader = new Jpeg2000Reader(headerBox.Content);
			foreach (Box box in this.ReadBoxes(reader))
			{
				if (box.Type.Equals("ihdr"))
				{
					this.ParseSizeBox(box);
				}
				else if (box.Type.Equals("colr"))
				{
					this.ParseColorSpaceBox(box);
				}
			}
		}

		void ParseSizeBox(Box box)
		{
			Jpeg2000Reader jpeg2000Reader = new Jpeg2000Reader(box.Content);
			uint height = jpeg2000Reader.ReadBigEndianUInt();
			uint width = jpeg2000Reader.ReadBigEndianUInt();
			this.Size = new SizeI?(new SizeI((int)width, (int)height));
		}

		void ParseColorSpaceBox(Box box)
		{
			if (this.ColorSpace != null)
			{
				return;
			}
			this.ColorSpace = new Jpeg2000ColorSpace?(Jpeg2000ColorSpace.Unknown);
			Jpeg2000Reader jpeg2000Reader = new Jpeg2000Reader(box.Content);
			byte b = jpeg2000Reader.Read();
			bool flag = b == 1;
			bool flag2 = b == 2;
			if (flag)
			{
				jpeg2000Reader.Read();
				jpeg2000Reader.Read();
				switch (jpeg2000Reader.ReadBigEndianUInt())
				{
				case 3U:
				case 4U:
				case 9U:
				case 11U:
				case 13U:
				case 18U:
				case 22U:
				case 23U:
				case 24U:
					this.ColorSpace = new Jpeg2000ColorSpace?(Jpeg2000ColorSpace.Unknown);
					return;
				case 12U:
					this.ColorSpace = new Jpeg2000ColorSpace?(Jpeg2000ColorSpace.Cmyk);
					return;
				case 16U:
				case 20U:
				case 21U:
					this.ColorSpace = new Jpeg2000ColorSpace?(Jpeg2000ColorSpace.Rgb);
					return;
				case 17U:
					this.ColorSpace = new Jpeg2000ColorSpace?(Jpeg2000ColorSpace.Grayscale);
					return;
				}
				this.ColorSpace = new Jpeg2000ColorSpace?(Jpeg2000ColorSpace.Unknown);
				return;
			}
			if (flag2)
			{
				this.ColorSpace = new Jpeg2000ColorSpace?(Jpeg2000ColorSpace.Unknown);
			}
		}

		bool TryCreateImageInfo(out Jpeg2000ImageInfo imageInfo)
		{
			bool hasReadAnyHeader = this.HasReadAnyHeader;
			if (hasReadAnyHeader)
			{
				imageInfo = new Jpeg2000ImageInfo();
				if (this.Size != null)
				{
					imageInfo.Width = this.Size.Value.Width;
					imageInfo.Height = this.Size.Value.Height;
				}
				if (this.ColorSpace != null)
				{
					imageInfo.ColorSpace = this.ColorSpace.Value;
				}
				if (this.NumberOfComponents != null)
				{
					imageInfo.NumberOfComponents = this.NumberOfComponents.Value;
				}
			}
			else
			{
				imageInfo = null;
			}
			return hasReadAnyHeader;
		}

		static bool TryReadJpeg2000StartOfImageMarker(Jpeg2000Reader reader)
		{
			bool flag = reader.Length >= (long)Jpeg2000HeadersDecoder.jpeg2000HeaderBytesStart.Length;
			if (flag)
			{
				foreach (byte b in Jpeg2000HeadersDecoder.jpeg2000HeaderBytesStart)
				{
					byte b2 = reader.Read();
					if (b != b2)
					{
						flag = false;
						break;
					}
				}
			}
			return flag;
		}

		const string SizeBoxType = "ihdr";

		const string ColorSpaceBoxType = "colr";

		const string Jpeg2000HeaderBoxType = "jp2h";

		const string Jpeg2000ContentBoxType = "jp2c";

		const short StartOfContentMarker = -177;

		const short SizMarker = -175;

		readonly byte[] data;

		static readonly byte[] jpeg2000HeaderBytesStart = new byte[]
		{
			0, 0, 0, 12, 106, 80, 32, 32, 13, 10,
			135, 10
		};
	}
}
