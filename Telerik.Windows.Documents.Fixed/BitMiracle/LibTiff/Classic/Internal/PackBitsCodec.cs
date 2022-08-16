using System;

namespace BitMiracle.LibTiff.Classic.Internal
{
	class PackBitsCodec : TiffCodec
	{
		public PackBitsCodec(Tiff tif, Compression scheme, string name)
			: base(tif, scheme, name)
		{
		}

		public override bool Init()
		{
			return true;
		}

		public override bool CanEncode
		{
			get
			{
				return true;
			}
		}

		public override bool CanDecode
		{
			get
			{
				return true;
			}
		}

		public override bool DecodeRow(byte[] buffer, int offset, int count, short plane)
		{
			return this.PackBitsDecode(buffer, offset, count, plane);
		}

		public override bool DecodeStrip(byte[] buffer, int offset, int count, short plane)
		{
			return this.PackBitsDecode(buffer, offset, count, plane);
		}

		public override bool DecodeTile(byte[] buffer, int offset, int count, short plane)
		{
			return this.PackBitsDecode(buffer, offset, count, plane);
		}

		public override bool PreEncode(short plane)
		{
			return this.PackBitsPreEncode(plane);
		}

		public override bool EncodeRow(byte[] buffer, int offset, int count, short plane)
		{
			return this.PackBitsEncode(buffer, offset, count, plane);
		}

		public override bool EncodeStrip(byte[] buffer, int offset, int count, short plane)
		{
			return this.PackBitsEncodeChunk(buffer, offset, count, plane);
		}

		public override bool EncodeTile(byte[] buffer, int offset, int count, short plane)
		{
			return this.PackBitsEncodeChunk(buffer, offset, count, plane);
		}

		bool PackBitsPreEncode(short s)
		{
			if (this.m_tif.IsTiled())
			{
				this.m_rowsize = this.m_tif.TileRowSize();
			}
			else
			{
				this.m_rowsize = this.m_tif.ScanlineSize();
			}
			return true;
		}

		bool PackBitsEncode(byte[] buf, int offset, int cc, short s)
		{
			int num = this.m_tif.m_rawcp;
			PackBitsCodec.EncodingState encodingState = PackBitsCodec.EncodingState.BASE;
			int num2 = 0;
			int num3 = offset;
			while (cc > 0)
			{
				int num4 = (int)buf[num3];
				num3++;
				cc--;
				int num5 = 1;
				while (cc > 0 && num4 == (int)buf[num3])
				{
					num5++;
					cc--;
					num3++;
				}
				bool flag = false;
				while (!flag)
				{
					if (num + 2 >= this.m_tif.m_rawdatasize)
					{
						if (encodingState == PackBitsCodec.EncodingState.LITERAL || encodingState == PackBitsCodec.EncodingState.LITERAL_RUN)
						{
							int num6 = num - num2;
							this.m_tif.m_rawcc += num2 - this.m_tif.m_rawcp;
							if (!this.m_tif.flushData1())
							{
								return false;
							}
							num = this.m_tif.m_rawcp;
							while (num6-- > 0)
							{
								this.m_tif.m_rawdata[num] = this.m_tif.m_rawdata[num2];
								num2++;
								num++;
							}
							num2 = this.m_tif.m_rawcp;
						}
						else
						{
							this.m_tif.m_rawcc += num - this.m_tif.m_rawcp;
							if (!this.m_tif.flushData1())
							{
								return false;
							}
							num = this.m_tif.m_rawcp;
						}
					}
					switch (encodingState)
					{
					case PackBitsCodec.EncodingState.BASE:
						if (num5 > 1)
						{
							encodingState = PackBitsCodec.EncodingState.RUN;
							if (num5 > 128)
							{
								int num7 = -127;
								this.m_tif.m_rawdata[num] = (byte)num7;
								num++;
								this.m_tif.m_rawdata[num] = (byte)num4;
								num++;
								num5 -= 128;
								break;
							}
							this.m_tif.m_rawdata[num] = (byte)(-num5 + 1);
							num++;
							this.m_tif.m_rawdata[num] = (byte)num4;
							num++;
						}
						else
						{
							num2 = num;
							this.m_tif.m_rawdata[num] = 0;
							num++;
							this.m_tif.m_rawdata[num] = (byte)num4;
							num++;
							encodingState = PackBitsCodec.EncodingState.LITERAL;
						}
						flag = true;
						break;
					case PackBitsCodec.EncodingState.LITERAL:
						if (num5 > 1)
						{
							encodingState = PackBitsCodec.EncodingState.LITERAL_RUN;
							if (num5 > 128)
							{
								int num8 = -127;
								this.m_tif.m_rawdata[num] = (byte)num8;
								num++;
								this.m_tif.m_rawdata[num] = (byte)num4;
								num++;
								num5 -= 128;
								break;
							}
							this.m_tif.m_rawdata[num] = (byte)(-num5 + 1);
							num++;
							this.m_tif.m_rawdata[num] = (byte)num4;
							num++;
						}
						else
						{
							byte[] rawdata = this.m_tif.m_rawdata;
							int num9 = num2;
							rawdata[num9] += 1;
							if (this.m_tif.m_rawdata[num2] == 127)
							{
								encodingState = PackBitsCodec.EncodingState.BASE;
							}
							this.m_tif.m_rawdata[num] = (byte)num4;
							num++;
						}
						flag = true;
						break;
					case PackBitsCodec.EncodingState.RUN:
						if (num5 > 1)
						{
							if (num5 > 128)
							{
								int num10 = -127;
								this.m_tif.m_rawdata[num] = (byte)num10;
								num++;
								this.m_tif.m_rawdata[num] = (byte)num4;
								num++;
								num5 -= 128;
								break;
							}
							this.m_tif.m_rawdata[num] = (byte)(-num5 + 1);
							num++;
							this.m_tif.m_rawdata[num] = (byte)num4;
							num++;
						}
						else
						{
							num2 = num;
							this.m_tif.m_rawdata[num] = 0;
							num++;
							this.m_tif.m_rawdata[num] = (byte)num4;
							num++;
							encodingState = PackBitsCodec.EncodingState.LITERAL;
						}
						flag = true;
						break;
					case PackBitsCodec.EncodingState.LITERAL_RUN:
					{
						int num11 = -1;
						if (num5 == 1 && this.m_tif.m_rawdata[num - 2] == (byte)num11 && this.m_tif.m_rawdata[num2] < 126)
						{
							byte[] rawdata2 = this.m_tif.m_rawdata;
							int num12 = num2;
							rawdata2[num12] += 2;
							encodingState = ((this.m_tif.m_rawdata[num2] == 127) ? PackBitsCodec.EncodingState.BASE : PackBitsCodec.EncodingState.LITERAL);
							this.m_tif.m_rawdata[num - 2] = this.m_tif.m_rawdata[num - 1];
						}
						else
						{
							encodingState = PackBitsCodec.EncodingState.RUN;
						}
						break;
					}
					}
				}
			}
			this.m_tif.m_rawcc += num - this.m_tif.m_rawcp;
			this.m_tif.m_rawcp = num;
			return true;
		}

		bool PackBitsEncodeChunk(byte[] buffer, int offset, int count, short plane)
		{
			while (count > 0)
			{
				int num = this.m_rowsize;
				if (count < num)
				{
					num = count;
				}
				if (!this.PackBitsEncode(buffer, offset, num, plane))
				{
					return false;
				}
				offset += num;
				count -= num;
			}
			return true;
		}

		bool PackBitsDecode(byte[] buffer, int offset, int count, short plane)
		{
			int num = this.m_tif.m_rawcp;
			int num2 = this.m_tif.m_rawcc;
			while (num2 > 0 && count > 0)
			{
				int num3 = (int)this.m_tif.m_rawdata[num];
				num++;
				num2--;
				if (num3 >= 128)
				{
					num3 -= 256;
				}
				if (num3 < 0)
				{
					if (num3 != -128)
					{
						num3 = -num3 + 1;
						if (count < num3)
						{
							Tiff.ErrorExt(this.m_tif, this.m_tif.m_clientdata, this.m_tif.m_name, "PackBitsDecode: discarding {0} bytes to avoid buffer overrun", new object[] { num3 - count });
							num3 = count;
						}
						count -= num3;
						int num4 = (int)this.m_tif.m_rawdata[num];
						num++;
						num2--;
						while (num3-- > 0)
						{
							buffer[offset] = (byte)num4;
							offset++;
						}
					}
				}
				else
				{
					if (count < num3 + 1)
					{
						Tiff.ErrorExt(this.m_tif, this.m_tif.m_clientdata, this.m_tif.m_name, "PackBitsDecode: discarding {0} bytes to avoid buffer overrun", new object[] { num3 - count + 1 });
						num3 = count - 1;
					}
					Buffer.BlockCopy(this.m_tif.m_rawdata, num, buffer, offset, ++num3);
					offset += num3;
					count -= num3;
					num += num3;
					num2 -= num3;
				}
			}
			this.m_tif.m_rawcp = num;
			this.m_tif.m_rawcc = num2;
			if (count > 0)
			{
				Tiff.ErrorExt(this.m_tif, this.m_tif.m_clientdata, this.m_tif.m_name, "PackBitsDecode: Not enough data for scanline {0}", new object[] { this.m_tif.m_row });
				return false;
			}
			return true;
		}

		int m_rowsize;

		enum EncodingState
		{
			BASE,
			LITERAL,
			RUN,
			LITERAL_RUN
		}
	}
}
