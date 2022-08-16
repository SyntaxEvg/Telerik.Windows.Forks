using System;
using System.IO;
using System.Text;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers
{
	class JfifMarker : JpegMarker
	{
		public byte DensityUnits
		{
			get
			{
				return this.densityUnits;
			}
			set
			{
				this.densityUnits = value;
			}
		}

		public ushort DensityX
		{
			get
			{
				return this.xdensity;
			}
			set
			{
				this.xdensity = value;
			}
		}

		public ushort DensityY
		{
			get
			{
				return this.ydensity;
			}
			set
			{
				this.ydensity = value;
			}
		}

		public override ushort Length
		{
			get
			{
				return this.length;
			}
		}

		public string Identifier
		{
			get
			{
				return this.identifier;
			}
			set
			{
				if (value != "JFIF" && value != "JFXX")
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.identifier = value;
			}
		}

		public override JpegMarkerType MarkerType
		{
			get
			{
				return JpegMarkerType.APP0;
			}
		}

		public byte[] ThumbnailData
		{
			get
			{
				return this.thumbnailData;
			}
			set
			{
				this.thumbnailData = value;
			}
		}

		public byte ThumbnailHeight
		{
			get
			{
				return this.thumbnHeight;
			}
			set
			{
				this.thumbnHeight = value;
				this.UpdateMarkerLength();
			}
		}

		public byte ThumbnailWidth
		{
			get
			{
				return this.thumbnWidth;
			}
			set
			{
				this.thumbnWidth = value;
				this.UpdateMarkerLength();
			}
		}

		public override void InterpretMarker(JpegDecoderBase decoder)
		{
			this.length = decoder.Reader.Read16();
			byte[] array = new byte[5];
			decoder.Reader.Read(array, 5);
			this.identifier = Encoding.ASCII.GetString(array, 0, 4);
			if (this.identifier == "JFIF")
			{
				this.versionHi = decoder.Reader.Read8();
				this.versionLo = decoder.Reader.Read8();
				this.densityUnits = decoder.Reader.Read8();
				this.xdensity = decoder.Reader.Read16();
				this.ydensity = decoder.Reader.Read16();
				this.thumbnWidth = decoder.Reader.Read8();
				this.thumbnHeight = decoder.Reader.Read8();
				if (this.thumbnWidth != 0 && this.thumbnHeight != 0)
				{
					this.thumbnailData = new byte[(int)(3 * this.thumbnWidth * this.thumbnHeight)];
					decoder.Reader.Read(this.thumbnailData, this.thumbnailData.Length);
					return;
				}
			}
			else
			{
				decoder.Reader.Seek((long)(this.length - 7), SeekOrigin.Current);
			}
		}

		public override void WriteMarker(JpegEncoder encoder)
		{
			base.WriteMarker(encoder);
			encoder.Writer.Write16(16);
			string s = "JFIF";
			byte[] bytes = Encoding.ASCII.GetBytes(s);
			encoder.Writer.Write(bytes, bytes.Length);
			encoder.Writer.Write8(0);
			encoder.Writer.Write8(1);
			encoder.Writer.Write8(1);
			encoder.Writer.Write8(0);
			encoder.Writer.Write16(1);
			encoder.Writer.Write16(1);
			encoder.Writer.Write8(0);
			encoder.Writer.Write8(0);
		}

		void UpdateMarkerLength()
		{
			if (this.identifier == "JFIF")
			{
				this.length = (ushort)(16 + 3 * this.thumbnWidth * this.thumbnHeight);
			}
		}

		internal const string JFIF_Identifier = "JFIF";

		internal const string JFXX_Identifier = "JFXX";

		internal const int DEFAULT_LENGTH = 16;

		const byte DefaultVersionHi = 1;

		const byte DefaultVersionLo = 1;

		const byte DefaultDensityUnits = 0;

		const ushort DefaultXDensity = 1;

		const ushort DefaultYDensity = 1;

		const byte DefaultThumbnailWidth = 0;

		const byte DefaultThumbnailHeight = 0;

		string identifier = "JFIF";

		ushort length = 16;

		byte versionHi = 1;

		byte versionLo = 1;

		byte densityUnits;

		ushort xdensity = 1;

		ushort ydensity = 1;

		byte thumbnWidth;

		byte thumbnHeight;

		byte[] thumbnailData;
	}
}
