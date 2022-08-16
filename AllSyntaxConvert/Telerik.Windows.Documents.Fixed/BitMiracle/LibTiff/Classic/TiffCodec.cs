using System;

namespace BitMiracle.LibTiff.Classic
{
	class TiffCodec
	{
		public TiffCodec(Tiff tif, Compression scheme, string name)
		{
			this.m_scheme = scheme;
			this.m_tif = tif;
			this.m_name = name;
		}

		public virtual bool CanEncode
		{
			get
			{
				return false;
			}
		}

		public virtual bool CanDecode
		{
			get
			{
				return false;
			}
		}

		public virtual bool Init()
		{
			return true;
		}

		public virtual bool SetupDecode()
		{
			return true;
		}

		public virtual bool PreDecode(short plane)
		{
			return true;
		}

		public virtual bool DecodeRow(byte[] buffer, int offset, int count, short plane)
		{
			return this.noDecode("scanline");
		}

		public virtual bool DecodeStrip(byte[] buffer, int offset, int count, short plane)
		{
			return this.noDecode("strip");
		}

		public virtual bool DecodeTile(byte[] buffer, int offset, int count, short plane)
		{
			return this.noDecode("tile");
		}

		public virtual bool SetupEncode()
		{
			return true;
		}

		public virtual bool PreEncode(short plane)
		{
			return true;
		}

		public virtual bool PostEncode()
		{
			return true;
		}

		public virtual bool EncodeRow(byte[] buffer, int offset, int count, short plane)
		{
			return this.noEncode("scanline");
		}

		public virtual bool EncodeStrip(byte[] buffer, int offset, int count, short plane)
		{
			return this.noEncode("strip");
		}

		public virtual bool EncodeTile(byte[] buffer, int offset, int count, short plane)
		{
			return this.noEncode("tile");
		}

		public virtual void Close()
		{
		}

		public virtual bool Seek(int row)
		{
			Tiff.ErrorExt(this.m_tif, this.m_tif.m_clientdata, this.m_tif.m_name, "Compression algorithm does not support random access", new object[0]);
			return false;
		}

		public virtual void Cleanup()
		{
		}

		public virtual int DefStripSize(int size)
		{
			if (size < 1)
			{
				int num = this.m_tif.ScanlineSize();
				size = 8192 / ((num == 0) ? 1 : num);
				if (size == 0)
				{
					size = 1;
				}
			}
			return size;
		}

		public virtual void DefTileSize(ref int width, ref int height)
		{
			if (width < 1)
			{
				width = 256;
			}
			if (height < 1)
			{
				height = 256;
			}
			if ((width & 15) != 0)
			{
				width = Tiff.roundUp(width, 16);
			}
			if ((height & 15) != 0)
			{
				height = Tiff.roundUp(height, 16);
			}
		}

		bool noEncode(string method)
		{
			TiffCodec tiffCodec = this.m_tif.FindCodec(this.m_tif.m_dir.td_compression);
			if (tiffCodec != null)
			{
				Tiff.ErrorExt(this.m_tif, this.m_tif.m_clientdata, this.m_tif.m_name, "{0} {1} encoding is not implemented", new object[] { tiffCodec.m_name, method });
			}
			else
			{
				Tiff.ErrorExt(this.m_tif, this.m_tif.m_clientdata, this.m_tif.m_name, "Compression scheme {0} {1} encoding is not implemented", new object[]
				{
					this.m_tif.m_dir.td_compression,
					method
				});
			}
			return false;
		}

		bool noDecode(string method)
		{
			TiffCodec tiffCodec = this.m_tif.FindCodec(this.m_tif.m_dir.td_compression);
			if (tiffCodec != null)
			{
				Tiff.ErrorExt(this.m_tif, this.m_tif.m_clientdata, this.m_tif.m_name, "{0} {1} decoding is not implemented", new object[] { tiffCodec.m_name, method });
			}
			else
			{
				Tiff.ErrorExt(this.m_tif, this.m_tif.m_clientdata, this.m_tif.m_name, "Compression scheme {0} {1} decoding is not implemented", new object[]
				{
					this.m_tif.m_dir.td_compression,
					method
				});
			}
			return false;
		}

		protected Tiff m_tif;

		protected internal Compression m_scheme;

		protected internal string m_name;
	}
}
