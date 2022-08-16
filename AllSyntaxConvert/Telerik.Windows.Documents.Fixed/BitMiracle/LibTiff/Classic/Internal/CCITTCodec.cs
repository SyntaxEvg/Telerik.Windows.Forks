using System;

namespace BitMiracle.LibTiff.Classic.Internal
{
	class CCITTCodec : TiffCodec
	{
		public CCITTCodec(Tiff tif, Compression scheme, string name)
			: base(tif, scheme, name)
		{
			this.m_tagMethods = new CCITTCodecTagMethods();
		}

		void cleanState()
		{
			this.m_mode = FaxMode.CLASSIC;
			this.m_groupoptions = Group3Opt.UNKNOWN;
			this.m_cleanfaxdata = CleanFaxData.CLEAN;
			this.m_badfaxlines = 0;
			this.m_badfaxrun = 0;
			this.m_recvparams = 0;
			this.m_subaddress = null;
			this.m_recvtime = 0;
			this.m_faxdcs = null;
			this.fill = null;
			this.m_rw_mode = 0;
			this.m_rowbytes = 0;
			this.m_rowpixels = 0;
			this.m_decoder = CCITTCodec.Decoder.useFax3_1DDecoder;
			this.m_bitmap = null;
			this.m_data = 0;
			this.m_bit = 0;
			this.m_EOLcnt = 0;
			this.m_runs = null;
			this.m_refruns = 0;
			this.m_curruns = 0;
			this.m_a0 = 0;
			this.m_RunLength = 0;
			this.m_thisrun = 0;
			this.m_pa = 0;
			this.m_pb = 0;
			this.m_encoder = CCITTCodec.Fax3Encoder.useFax1DEncoder;
			this.m_encodingFax4 = false;
			this.m_refline = null;
			this.m_k = 0;
			this.m_maxk = 0;
			this.m_line = 0;
			this.m_buffer = null;
			this.m_offset = 0;
		}

		public override bool Init()
		{
			Compression scheme = this.m_scheme;
			switch (scheme)
			{
			case Compression.CCITTRLE:
				return this.TIFFInitCCITTRLE();
			case Compression.CCITTFAX3:
				return this.TIFFInitCCITTFax3();
			case Compression.CCITTFAX4:
				return this.TIFFInitCCITTFax4();
			default:
				return scheme == Compression.CCITTRLEW && this.TIFFInitCCITTRLEW();
			}
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

		public override bool SetupDecode()
		{
			return this.setupState();
		}

		public override bool PreDecode(short plane)
		{
			this.m_bit = 0;
			this.m_data = 0;
			this.m_EOLcnt = 0;
			this.m_bitmap = Tiff.GetBitRevTable(this.m_tif.m_dir.td_fillorder != FillOrder.LSB2MSB);
			if (this.m_refruns >= 0)
			{
				this.m_runs[this.m_refruns] = this.m_rowpixels;
				this.m_runs[this.m_refruns + 1] = 0;
			}
			this.m_line = 0;
			return true;
		}

		public override bool DecodeRow(byte[] buffer, int offset, int count, short plane)
		{
			switch (this.m_decoder)
			{
			case CCITTCodec.Decoder.useFax3_1DDecoder:
				return this.Fax3Decode1D(buffer, offset, count);
			case CCITTCodec.Decoder.useFax3_2DDecoder:
				return this.Fax3Decode2D(buffer, offset, count);
			case CCITTCodec.Decoder.useFax4Decoder:
				return this.Fax4Decode(buffer, offset, count);
			case CCITTCodec.Decoder.useFax3RLEDecoder:
				return this.Fax3DecodeRLE(buffer, offset, count);
			default:
				return false;
			}
		}

		public override bool DecodeStrip(byte[] buffer, int offset, int count, short plane)
		{
			return this.DecodeRow(buffer, offset, count, plane);
		}

		public override bool DecodeTile(byte[] buffer, int offset, int count, short plane)
		{
			return this.DecodeRow(buffer, offset, count, plane);
		}

		public override bool SetupEncode()
		{
			return this.setupState();
		}

		public override bool PreEncode(short plane)
		{
			this.m_bit = 8;
			this.m_data = 0;
			this.m_encoder = CCITTCodec.Fax3Encoder.useFax1DEncoder;
			if (this.m_refline != null)
			{
				Array.Clear(this.m_refline, 0, this.m_refline.Length);
			}
			if (this.is2DEncoding())
			{
				float num = this.m_tif.m_dir.td_yresolution;
				if (this.m_tif.m_dir.td_resolutionunit == ResUnit.CENTIMETER)
				{
					num *= 2.54f;
				}
				this.m_maxk = ((num > 150f) ? 4 : 2);
				this.m_k = this.m_maxk - 1;
			}
			else
			{
				this.m_maxk = 0;
				this.m_k = 0;
			}
			this.m_line = 0;
			return true;
		}

		public override bool PostEncode()
		{
			if (this.m_encodingFax4)
			{
				return this.Fax4PostEncode();
			}
			return this.Fax3PostEncode();
		}

		public override bool EncodeRow(byte[] buffer, int offset, int count, short plane)
		{
			if (this.m_encodingFax4)
			{
				return this.Fax4Encode(buffer, offset, count);
			}
			return this.Fax3Encode(buffer, offset, count);
		}

		public override bool EncodeStrip(byte[] buffer, int offset, int count, short plane)
		{
			return this.EncodeRow(buffer, offset, count, plane);
		}

		public override bool EncodeTile(byte[] buffer, int offset, int count, short plane)
		{
			return this.EncodeRow(buffer, offset, count, plane);
		}

		public override void Close()
		{
			if ((this.m_mode & FaxMode.NORTC) == FaxMode.CLASSIC)
			{
				int num = 1;
				int num2 = 12;
				if (this.is2DEncoding())
				{
					bool flag = (num << 1 != 0) | (this.m_encoder == CCITTCodec.Fax3Encoder.useFax1DEncoder);
					if (flag)
					{
						num = 1;
					}
					else
					{
						num = 0;
					}
					num2++;
				}
				for (int i = 0; i < 6; i++)
				{
					this.putBits(num, num2);
				}
				this.flushBits();
			}
		}

		public override void Cleanup()
		{
			this.m_tif.m_tagmethods = this.m_parentTagMethods;
		}

		bool is2DEncoding()
		{
			return (this.m_groupoptions & Group3Opt.ENCODING2D) != (Group3Opt)0;
		}

		void CHECK_b1(ref int b1)
		{
			if (this.m_pa != this.m_thisrun)
			{
				while (b1 <= this.m_a0 && b1 < this.m_rowpixels)
				{
					b1 += this.m_runs[this.m_pb] + this.m_runs[this.m_pb + 1];
					this.m_pb += 2;
				}
			}
		}

		static void SWAP(ref int a, ref int b)
		{
			int num = a;
			a = b;
			b = num;
		}

		static bool isLongAligned(int offset)
		{
			return offset % 4 == 0;
		}

		static bool isShortAligned(int offset)
		{
			return offset % 2 == 0;
		}

		static void FILL(int n, byte[] cp, ref int offset, byte value)
		{
			if (n <= 7 && n > 0)
			{
				for (int i = n; i > 0; i--)
				{
					cp[offset + i - 1] = value;
				}
				offset += n;
			}
		}

		static void fax3FillRuns(byte[] buffer, int offset, int[] runs, int thisRunOffset, int nextRunOffset, int width)
		{
			if (((nextRunOffset - thisRunOffset) & 1) != 0)
			{
				runs[nextRunOffset] = 0;
				nextRunOffset++;
			}
			int num = 0;
			while (thisRunOffset < nextRunOffset)
			{
				int num2 = runs[thisRunOffset];
				if (num + num2 > width || num2 > width)
				{
					runs[thisRunOffset] = width - num;
					num2 = runs[thisRunOffset];
				}
				if (num2 != 0)
				{
					int i = offset + (num >> 3);
					int num3 = num & 7;
					if (num2 > 8 - num3)
					{
						if (num3 != 0)
						{
							int num4 = i;
							buffer[num4] &= (byte)(255 << 8 - num3);
							i++;
							num2 -= 8 - num3;
						}
						int num5 = num2 >> 3;
						if (num5 != 0)
						{
							if (num5 / 4 > 1)
							{
								while (num5 != 0 && !CCITTCodec.isLongAligned(i))
								{
									buffer[i] = 0;
									i++;
									num5--;
								}
								int num6 = num5 - num5 % 4;
								num5 -= num6;
								int num7 = num6 + i;
								while (i < num7)
								{
									buffer[i] = 0;
									i++;
								}
							}
							CCITTCodec.FILL(num5, buffer, ref i, 0);
							num2 &= 7;
						}
						if (num2 != 0)
						{
							int num8 = i;
							buffer[num8] &= (byte)(255 >> num2);
						}
					}
					else
					{
						int num9 = i;
						buffer[num9] &= (byte)(~(byte)(CCITTCodec.fillMasks[num2] >> num3));
					}
					num += runs[thisRunOffset];
				}
				num2 = runs[thisRunOffset + 1];
				if (num + num2 > width || num2 > width)
				{
					runs[thisRunOffset + 1] = width - num;
					num2 = runs[thisRunOffset + 1];
				}
				if (num2 != 0)
				{
					int j = offset + (num >> 3);
					int num10 = num & 7;
					if (num2 > 8 - num10)
					{
						if (num10 != 0)
						{
							int num11 = j;
							buffer[num11] |= (byte)(255 >> num10);
							j++;
							num2 -= 8 - num10;
						}
						int num12 = num2 >> 3;
						if (num12 != 0)
						{
							if (num12 / 4 > 1)
							{
								while (num12 != 0 && !CCITTCodec.isLongAligned(j))
								{
									buffer[j] = byte.MaxValue;
									j++;
									num12--;
								}
								int num13 = num12 - num12 % 4;
								num12 -= num13;
								int num14 = num13 + j;
								while (j < num14)
								{
									buffer[j] = byte.MaxValue;
									j++;
								}
							}
							CCITTCodec.FILL(num12, buffer, ref j, byte.MaxValue);
							num2 &= 7;
						}
						if (num2 != 0)
						{
							int num15 = j;
							buffer[num15] |= (byte)(65280 >> num2);
						}
					}
					else
					{
						int num16 = j;
						buffer[num16] |= (byte)(CCITTCodec.fillMasks[num2] >> num10);
					}
					num += runs[thisRunOffset + 1];
				}
				thisRunOffset += 2;
			}
		}

		static int find0span(byte[] bp, int bpOffset, int bs, int be)
		{
			int num = bpOffset + (bs >> 3);
			int i = be - bs;
			int num2 = bs & 7;
			int num3 = 0;
			if (i > 0 && num2 != 0)
			{
				num3 = (int)CCITTCodec.m_zeroruns[((int)bp[num] << num2) & 255];
				if (num3 > 8 - num2)
				{
					num3 = 8 - num2;
				}
				if (num3 > i)
				{
					num3 = i;
				}
				if (num2 + num3 < 8)
				{
					return num3;
				}
				i -= num3;
				num++;
			}
			if (i >= 64)
			{
				while (!CCITTCodec.isLongAligned(num))
				{
					if (bp[num] != 0)
					{
						return num3 + (int)CCITTCodec.m_zeroruns[(int)bp[num]];
					}
					num3 += 8;
					i -= 8;
					num++;
				}
				while (i >= 32)
				{
					bool flag = true;
					for (int j = 0; j < 4; j++)
					{
						if (bp[num + j] != 0)
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
					num3 += 32;
					i -= 32;
					num += 4;
				}
			}
			while (i >= 8)
			{
				if (bp[num] != 0)
				{
					return num3 + (int)CCITTCodec.m_zeroruns[(int)bp[num]];
				}
				num3 += 8;
				i -= 8;
				num++;
			}
			if (i > 0)
			{
				num2 = (int)CCITTCodec.m_zeroruns[(int)bp[num]];
				num3 += ((num2 > i) ? i : num2);
			}
			return num3;
		}

		static int find1span(byte[] bp, int bpOffset, int bs, int be)
		{
			int num = bpOffset + (bs >> 3);
			int num2 = bs & 7;
			int num3 = 0;
			int i = be - bs;
			if (i > 0 && num2 != 0)
			{
				num3 = (int)CCITTCodec.m_oneruns[((int)bp[num] << num2) & 255];
				if (num3 > 8 - num2)
				{
					num3 = 8 - num2;
				}
				if (num3 > i)
				{
					num3 = i;
				}
				if (num2 + num3 < 8)
				{
					return num3;
				}
				i -= num3;
				num++;
			}
			if (i >= 64)
			{
				while (!CCITTCodec.isLongAligned(num))
				{
					if (bp[num] != 255)
					{
						return num3 + (int)CCITTCodec.m_oneruns[(int)bp[num]];
					}
					num3 += 8;
					i -= 8;
					num++;
				}
				while (i >= 32)
				{
					bool flag = true;
					for (int j = 0; j < 4; j++)
					{
						if (bp[num + j] != 255)
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
					num3 += 32;
					i -= 32;
					num += 4;
				}
			}
			while (i >= 8)
			{
				if (bp[num] != 255)
				{
					return num3 + (int)CCITTCodec.m_oneruns[(int)bp[num]];
				}
				num3 += 8;
				i -= 8;
				num++;
			}
			if (i > 0)
			{
				num2 = (int)CCITTCodec.m_oneruns[(int)bp[num]];
				num3 += ((num2 > i) ? i : num2);
			}
			return num3;
		}

		static int finddiff(byte[] bp, int bpOffset, int _bs, int _be, int _color)
		{
			if (_color != 0)
			{
				return _bs + CCITTCodec.find1span(bp, bpOffset, _bs, _be);
			}
			return _bs + CCITTCodec.find0span(bp, bpOffset, _bs, _be);
		}

		static int finddiff2(byte[] bp, int bpOffset, int _bs, int _be, int _color)
		{
			if (_bs < _be)
			{
				return CCITTCodec.finddiff(bp, bpOffset, _bs, _be, _color);
			}
			return _be;
		}

		bool EndOfData()
		{
			return this.m_tif.m_rawcp >= this.m_tif.m_rawcc;
		}

		int GetBits(int n)
		{
			return this.m_data & ((1 << n) - 1);
		}

		void ClrBits(int n)
		{
			this.m_bit -= n;
			this.m_data >>= n;
		}

		bool NeedBits8(int n)
		{
			if (this.m_bit < n)
			{
				if (this.EndOfData())
				{
					if (this.m_bit == 0)
					{
						return false;
					}
					this.m_bit = n;
				}
				else
				{
					this.m_data |= (int)this.m_bitmap[(int)this.m_tif.m_rawdata[this.m_tif.m_rawcp]] << this.m_bit;
					this.m_tif.m_rawcp++;
					this.m_bit += 8;
				}
			}
			return true;
		}

		bool NeedBits16(int n)
		{
			if (this.m_bit < n)
			{
				if (this.EndOfData())
				{
					if (this.m_bit == 0)
					{
						return false;
					}
					this.m_bit = n;
				}
				else
				{
					this.m_data |= (int)this.m_bitmap[(int)this.m_tif.m_rawdata[this.m_tif.m_rawcp]] << this.m_bit;
					this.m_tif.m_rawcp++;
					this.m_bit += 8;
					if (this.m_bit < n)
					{
						if (this.EndOfData())
						{
							this.m_bit = n;
						}
						else
						{
							this.m_data |= (int)this.m_bitmap[(int)this.m_tif.m_rawdata[this.m_tif.m_rawcp]] << this.m_bit;
							this.m_tif.m_rawcp++;
							this.m_bit += 8;
						}
					}
				}
			}
			return true;
		}

		bool LOOKUP8(out CCITTCodec.faxTableEntry TabEnt, int wid)
		{
			if (!this.NeedBits8(wid))
			{
				TabEnt = default(CCITTCodec.faxTableEntry);
				return false;
			}
			TabEnt = CCITTCodec.faxTableEntry.FromArray(CCITTCodec.m_faxMainTable, this.GetBits(wid));
			this.ClrBits((int)TabEnt.Width);
			return true;
		}

		bool LOOKUP16(out CCITTCodec.faxTableEntry TabEnt, int wid, bool useBlack)
		{
			if (!this.NeedBits16(wid))
			{
				TabEnt = default(CCITTCodec.faxTableEntry);
				return false;
			}
			if (useBlack)
			{
				TabEnt = CCITTCodec.faxTableEntry.FromArray(CCITTCodec.m_faxBlackTable, this.GetBits(wid));
			}
			else
			{
				TabEnt = CCITTCodec.faxTableEntry.FromArray(CCITTCodec.m_faxWhiteTable, this.GetBits(wid));
			}
			this.ClrBits((int)TabEnt.Width);
			return true;
		}

		bool SYNC_EOL()
		{
			if (this.m_EOLcnt == 0)
			{
				while (this.NeedBits16(11))
				{
					if (this.GetBits(11) == 0)
					{
						goto IL_27;
					}
					this.ClrBits(1);
				}
				return false;
			}
			IL_27:
			while (this.NeedBits8(8))
			{
				if (this.GetBits(8) != 0)
				{
					while (this.GetBits(1) == 0)
					{
						this.ClrBits(1);
					}
					this.ClrBits(1);
					this.m_EOLcnt = 0;
					return true;
				}
				this.ClrBits(8);
			}
			return false;
		}

		bool setupState()
		{
			if (this.m_tif.m_dir.td_bitspersample != 1)
			{
				Tiff.ErrorExt(this.m_tif, this.m_tif.m_clientdata, this.m_tif.m_name, "Bits/sample must be 1 for Group 3/4 encoding/decoding", new object[0]);
				return false;
			}
			int num;
			int num2;
			if (this.m_tif.IsTiled())
			{
				num = this.m_tif.TileRowSize();
				num2 = this.m_tif.m_dir.td_tilewidth;
			}
			else
			{
				num = this.m_tif.ScanlineSize();
				num2 = this.m_tif.m_dir.td_imagewidth;
			}
			this.m_rowbytes = num;
			this.m_rowpixels = num2;
			bool flag = (this.m_groupoptions & Group3Opt.ENCODING2D) != (Group3Opt)0 || this.m_tif.m_dir.td_compression == Compression.CCITTFAX4;
			this.m_runs = null;
			int num3 = Tiff.roundUp(num2, 32);
			if (flag)
			{
				long num4 = (long)num3 * 2L;
				if (num4 > 2147483647L)
				{
					Tiff.ErrorExt(this.m_tif, this.m_tif.m_clientdata, this.m_tif.m_name, "Row pixels integer overflow (rowpixels {0})", new object[] { num2 });
					return false;
				}
				num3 = (int)num4;
			}
			if (num3 == 0 || (long)num3 * 2L > 2147483647L)
			{
				Tiff.ErrorExt(this.m_tif, this.m_tif.m_clientdata, this.m_tif.m_name, "Row pixels integer overflow (rowpixels {0})", new object[] { num2 });
				return false;
			}
			this.m_runs = new int[2 * num3];
			this.m_curruns = 0;
			if (flag)
			{
				this.m_refruns = num3;
			}
			else
			{
				this.m_refruns = -1;
			}
			if (this.m_tif.m_dir.td_compression == Compression.CCITTFAX3 && this.is2DEncoding())
			{
				this.m_decoder = CCITTCodec.Decoder.useFax3_2DDecoder;
			}
			if (flag)
			{
				this.m_refline = new byte[num + 1];
			}
			else
			{
				this.m_refline = null;
			}
			return true;
		}

		void Fax3Unexpected(string module)
		{
			Tiff.ErrorExt(this.m_tif, this.m_tif.m_clientdata, module, "{0}: Bad code word at line {1} of {2} {3} (x {4})", new object[]
			{
				this.m_tif.m_name,
				this.m_line,
				this.m_tif.IsTiled() ? "tile" : "strip",
				this.m_tif.IsTiled() ? this.m_tif.m_curtile : this.m_tif.m_curstrip,
				this.m_a0
			});
		}

		void Fax3Extension(string module)
		{
			Tiff.ErrorExt(this.m_tif, this.m_tif.m_clientdata, module, "{0}: Uncompressed data (not supported) at line {1} of {2} {3} (x {4})", new object[]
			{
				this.m_tif.m_name,
				this.m_line,
				this.m_tif.IsTiled() ? "tile" : "strip",
				this.m_tif.IsTiled() ? this.m_tif.m_curtile : this.m_tif.m_curstrip,
				this.m_a0
			});
		}

		void Fax3BadLength(string module)
		{
			Tiff.WarningExt(this.m_tif, this.m_tif.m_clientdata, module, "{0}: {1} at line {2} of {3} {4} (got {5}, expected {6})", new object[]
			{
				this.m_tif.m_name,
				(this.m_a0 < this.m_rowpixels) ? "Premature EOL" : "Line length mismatch",
				this.m_line,
				this.m_tif.IsTiled() ? "tile" : "strip",
				this.m_tif.IsTiled() ? this.m_tif.m_curtile : this.m_tif.m_curstrip,
				this.m_a0,
				this.m_rowpixels
			});
		}

		void Fax3PrematureEOF(string module)
		{
			Tiff.WarningExt(this.m_tif, this.m_tif.m_clientdata, module, "{0}: Premature EOF at line {1} of {2} {3} (x {4})", new object[]
			{
				this.m_tif.m_name,
				this.m_line,
				this.m_tif.IsTiled() ? "tile" : "strip",
				this.m_tif.IsTiled() ? this.m_tif.m_curtile : this.m_tif.m_curstrip,
				this.m_a0
			});
		}

		bool Fax3Decode1D(byte[] buffer, int offset, int count)
		{
			this.m_thisrun = this.m_curruns;
			while (count > 0)
			{
				this.m_a0 = 0;
				this.m_RunLength = 0;
				this.m_pa = this.m_thisrun;
				if (!this.SYNC_EOL())
				{
					this.CLEANUP_RUNS("Fax3Decode1D");
				}
				else
				{
					bool flag = this.EXPAND1D("Fax3Decode1D");
					if (flag)
					{
						this.fill(buffer, offset, this.m_runs, this.m_thisrun, this.m_pa, this.m_rowpixels);
						offset += this.m_rowbytes;
						count -= this.m_rowbytes;
						this.m_line++;
						continue;
					}
				}
				this.fill(buffer, offset, this.m_runs, this.m_thisrun, this.m_pa, this.m_rowpixels);
				return false;
			}
			return true;
		}

		bool Fax3Decode2D(byte[] buffer, int offset, int count)
		{
			while (count > 0)
			{
				this.m_a0 = 0;
				this.m_RunLength = 0;
				this.m_pa = this.m_curruns;
				this.m_thisrun = this.m_curruns;
				bool flag = false;
				if (!this.SYNC_EOL())
				{
					flag = true;
				}
				if (!flag && !this.NeedBits8(1))
				{
					flag = true;
				}
				if (!flag)
				{
					int bits = this.GetBits(1);
					this.ClrBits(1);
					this.m_pb = this.m_refruns;
					int b = this.m_runs[this.m_pb];
					this.m_pb++;
					bool flag2;
					if (bits != 0)
					{
						flag2 = this.EXPAND1D("Fax3Decode2D");
					}
					else
					{
						flag2 = this.EXPAND2D("Fax3Decode2D", b);
					}
					if (flag2)
					{
						this.fill(buffer, offset, this.m_runs, this.m_thisrun, this.m_pa, this.m_rowpixels);
						this.SETVALUE(0);
						CCITTCodec.SWAP(ref this.m_curruns, ref this.m_refruns);
						offset += this.m_rowbytes;
						count -= this.m_rowbytes;
						this.m_line++;
						continue;
					}
				}
				else
				{
					this.CLEANUP_RUNS("Fax3Decode2D");
				}
				this.fill(buffer, offset, this.m_runs, this.m_thisrun, this.m_pa, this.m_rowpixels);
				return false;
			}
			return true;
		}

		bool Fax3Encode1DRow()
		{
			int num = 0;
			do
			{
				int num2 = CCITTCodec.find0span(this.m_buffer, this.m_offset, num, this.m_rowpixels);
				this.putspan(num2, false);
				num += num2;
				if (num >= this.m_rowpixels)
				{
					break;
				}
				num2 = CCITTCodec.find1span(this.m_buffer, this.m_offset, num, this.m_rowpixels);
				this.putspan(num2, true);
				num += num2;
			}
			while (num < this.m_rowpixels);
			if ((this.m_mode & (FaxMode)12) != FaxMode.CLASSIC)
			{
				if (this.m_bit != 8)
				{
					this.flushBits();
				}
				if ((this.m_mode & FaxMode.WORDALIGN) != FaxMode.CLASSIC && !CCITTCodec.isShortAligned(this.m_tif.m_rawcp))
				{
					this.flushBits();
				}
			}
			return true;
		}

		bool Fax3Encode2DRow()
		{
			int num = 0;
			int num2 = ((CCITTCodec.Fax3Encode2DRow_Pixel(this.m_buffer, this.m_offset, 0) != 0) ? 0 : CCITTCodec.finddiff(this.m_buffer, this.m_offset, 0, this.m_rowpixels, 0));
			int num3 = ((CCITTCodec.Fax3Encode2DRow_Pixel(this.m_refline, 0, 0) != 0) ? 0 : CCITTCodec.finddiff(this.m_refline, 0, 0, this.m_rowpixels, 0));
			for (;;)
			{
				int num4 = CCITTCodec.finddiff2(this.m_refline, 0, num3, this.m_rowpixels, CCITTCodec.Fax3Encode2DRow_Pixel(this.m_refline, 0, num3));
				if (num4 >= num2)
				{
					int num5 = num3 - num2;
					if (-3 > num5 || num5 > 3)
					{
						int num6 = CCITTCodec.finddiff2(this.m_buffer, this.m_offset, num2, this.m_rowpixels, CCITTCodec.Fax3Encode2DRow_Pixel(this.m_buffer, this.m_offset, num2));
						this.putcode(CCITTCodec.m_horizcode);
						if (num + num2 == 0 || CCITTCodec.Fax3Encode2DRow_Pixel(this.m_buffer, this.m_offset, num) == 0)
						{
							this.putspan(num2 - num, false);
							this.putspan(num6 - num2, true);
						}
						else
						{
							this.putspan(num2 - num, true);
							this.putspan(num6 - num2, false);
						}
						num = num6;
					}
					else
					{
						this.putcode(CCITTCodec.m_vcodes[num5 + 3]);
						num = num2;
					}
				}
				else
				{
					this.putcode(CCITTCodec.m_passcode);
					num = num4;
				}
				if (num >= this.m_rowpixels)
				{
					break;
				}
				num2 = CCITTCodec.finddiff(this.m_buffer, this.m_offset, num, this.m_rowpixels, CCITTCodec.Fax3Encode2DRow_Pixel(this.m_buffer, this.m_offset, num));
				int color;
				if (CCITTCodec.Fax3Encode2DRow_Pixel(this.m_buffer, this.m_offset, num) == 0)
				{
					color = 1;
				}
				else
				{
					color = 0;
				}
				num3 = CCITTCodec.finddiff(this.m_refline, 0, num, this.m_rowpixels, color);
				num3 = CCITTCodec.finddiff(this.m_refline, 0, num3, this.m_rowpixels, CCITTCodec.Fax3Encode2DRow_Pixel(this.m_buffer, this.m_offset, num));
			}
			return true;
		}

		static int Fax3Encode2DRow_Pixel(byte[] buf, int bufOffset, int ix)
		{
			return (buf[Math.Min(bufOffset + (ix >> 3), buf.Length - 1)] >> 7 - (ix & 7)) & 1;
		}

		bool Fax3Encode(byte[] buffer, int offset, int count)
		{
			this.m_buffer = buffer;
			this.m_offset = offset;
			while (count > 0)
			{
				if ((this.m_mode & FaxMode.NOEOL) == FaxMode.CLASSIC)
				{
					this.Fax3PutEOL();
				}
				if (this.is2DEncoding())
				{
					if (this.m_encoder == CCITTCodec.Fax3Encoder.useFax1DEncoder)
					{
						if (!this.Fax3Encode1DRow())
						{
							return false;
						}
						this.m_encoder = CCITTCodec.Fax3Encoder.useFax2DEncoder;
					}
					else
					{
						if (!this.Fax3Encode2DRow())
						{
							return false;
						}
						this.m_k--;
					}
					if (this.m_k == 0)
					{
						this.m_encoder = CCITTCodec.Fax3Encoder.useFax1DEncoder;
						this.m_k = this.m_maxk - 1;
					}
					else
					{
						Buffer.BlockCopy(this.m_buffer, this.m_offset, this.m_refline, 0, this.m_rowbytes);
					}
				}
				else if (!this.Fax3Encode1DRow())
				{
					return false;
				}
				this.m_offset += this.m_rowbytes;
				count -= this.m_rowbytes;
			}
			return true;
		}

		bool Fax3PostEncode()
		{
			if (this.m_bit != 8)
			{
				this.flushBits();
			}
			return true;
		}

		void InitCCITTFax3()
		{
			this.m_tif.MergeFieldInfo(CCITTCodec.m_faxFieldInfo, CCITTCodec.m_faxFieldInfo.Length);
			this.cleanState();
			this.m_rw_mode = this.m_tif.m_mode;
			this.m_parentTagMethods = this.m_tif.m_tagmethods;
			this.m_tif.m_tagmethods = this.m_tagMethods;
			this.m_groupoptions = (Group3Opt)0;
			this.m_recvparams = 0;
			this.m_subaddress = null;
			this.m_faxdcs = null;
			if (this.m_rw_mode == 0)
			{
				this.m_tif.m_flags |= TiffFlags.NOBITREV;
			}
			this.m_runs = null;
			this.m_tif.SetField(TiffTag.FAXFILLFUNC, new object[]
			{
				new Tiff.FaxFillFunc(CCITTCodec.fax3FillRuns)
			});
			this.m_refline = null;
			this.m_decoder = CCITTCodec.Decoder.useFax3_1DDecoder;
			this.m_encodingFax4 = false;
		}

		bool TIFFInitCCITTFax3()
		{
			this.InitCCITTFax3();
			this.m_tif.MergeFieldInfo(CCITTCodec.m_fax3FieldInfo, CCITTCodec.m_fax3FieldInfo.Length);
			return this.m_tif.SetField(TiffTag.FAXMODE, new object[] { FaxMode.NORTC });
		}

		void flushBits()
		{
			if (this.m_tif.m_rawcc >= this.m_tif.m_rawdatasize)
			{
				this.m_tif.flushData1();
			}
			this.m_tif.m_rawdata[this.m_tif.m_rawcp] = (byte)this.m_data;
			this.m_tif.m_rawcp++;
			this.m_tif.m_rawcc++;
			this.m_data = 0;
			this.m_bit = 8;
		}

		void putBits(int bits, int length)
		{
			while (length > this.m_bit)
			{
				this.m_data |= bits >> length - this.m_bit;
				length -= this.m_bit;
				this.flushBits();
			}
			this.m_data |= (bits & CCITTCodec.m_msbmask[length]) << this.m_bit - length;
			this.m_bit -= length;
			if (this.m_bit == 0)
			{
				this.flushBits();
			}
		}

		void putcode(CCITTCodec.tableEntry te)
		{
			this.putBits((int)te.code, (int)te.length);
		}

		void putspan(int span, bool useBlack)
		{
			short[] array;
			if (useBlack)
			{
				array = CCITTCodec.m_faxBlackCodes;
			}
			else
			{
				array = CCITTCodec.m_faxWhiteCodes;
			}
			CCITTCodec.tableEntry tableEntry = CCITTCodec.tableEntry.FromArray(array, 103);
			while (span >= 2624)
			{
				this.putBits((int)tableEntry.code, (int)tableEntry.length);
				span -= (int)tableEntry.runlen;
			}
			if (span >= 64)
			{
				tableEntry = CCITTCodec.tableEntry.FromArray(array, 63 + (span >> 6));
				this.putBits((int)tableEntry.code, (int)tableEntry.length);
				span -= (int)tableEntry.runlen;
			}
			tableEntry = CCITTCodec.tableEntry.FromArray(array, span);
			this.putBits((int)tableEntry.code, (int)tableEntry.length);
		}

		void Fax3PutEOL()
		{
			if ((this.m_groupoptions & Group3Opt.FILLBITS) != (Group3Opt)0)
			{
				int num = 4;
				if (num != this.m_bit)
				{
					if (num > this.m_bit)
					{
						num = this.m_bit + (8 - num);
					}
					else
					{
						num = this.m_bit - num;
					}
					this.putBits(0, num);
				}
			}
			int num2 = 1;
			int num3 = 12;
			if (this.is2DEncoding())
			{
				num2 <<= 1;
				if (this.m_encoder == CCITTCodec.Fax3Encoder.useFax1DEncoder)
				{
					num2++;
				}
				num3++;
			}
			this.putBits(num2, num3);
		}

		void SETVALUE(int x)
		{
			this.m_runs[this.m_pa] = this.m_RunLength + x;
			this.m_pa++;
			this.m_a0 += x;
			this.m_RunLength = 0;
		}

		void CLEANUP_RUNS(string module)
		{
			if (this.m_RunLength != 0)
			{
				this.SETVALUE(0);
			}
			if (this.m_a0 != this.m_rowpixels)
			{
				this.Fax3BadLength(module);
				while (this.m_a0 > this.m_rowpixels && this.m_pa > this.m_thisrun)
				{
					this.m_pa--;
					this.m_a0 -= this.m_runs[this.m_pa];
				}
				if (this.m_a0 < this.m_rowpixels)
				{
					if (this.m_a0 < 0)
					{
						this.m_a0 = 0;
					}
					if (((this.m_pa - this.m_thisrun) & 1) != 0)
					{
						this.SETVALUE(0);
					}
					this.SETVALUE(this.m_rowpixels - this.m_a0);
					return;
				}
				if (this.m_a0 > this.m_rowpixels)
				{
					this.SETVALUE(this.m_rowpixels);
					this.SETVALUE(0);
				}
			}
		}

		void handlePrematureEOFinExpand2D(string module)
		{
			this.Fax3PrematureEOF(module);
			this.CLEANUP_RUNS(module);
		}

		bool EXPAND1D(string module)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			IL_06:
			CCITTCodec.faxTableEntry faxTableEntry;
			while (this.LOOKUP16(out faxTableEntry, 12, false))
			{
				switch (faxTableEntry.State)
				{
				case 7:
					this.SETVALUE(faxTableEntry.Param);
					flag2 = true;
					break;
				case 8:
				case 10:
					goto IL_95;
				case 9:
				case 11:
					this.m_a0 += faxTableEntry.Param;
					this.m_RunLength += faxTableEntry.Param;
					break;
				case 12:
					this.m_rowpixels = 1;
					flag = true;
					break;
				default:
					goto IL_95;
				}
				IL_9E:
				if (flag || flag2)
				{
					if (!flag && this.m_a0 < this.m_rowpixels)
					{
						while (this.LOOKUP16(out faxTableEntry, 13, true))
						{
							switch (faxTableEntry.State)
							{
							case 8:
								this.SETVALUE(faxTableEntry.Param);
								flag3 = true;
								break;
							case 9:
								goto IL_149;
							case 10:
							case 11:
								this.m_a0 += faxTableEntry.Param;
								this.m_RunLength += faxTableEntry.Param;
								break;
							case 12:
								this.m_EOLcnt = 1;
								flag = true;
								break;
							default:
								goto IL_149;
							}
							IL_152:
							if (!flag && !flag3)
							{
								continue;
							}
							if (!flag && this.m_a0 < this.m_rowpixels)
							{
								if (this.m_runs[this.m_pa - 1] == 0 && this.m_runs[this.m_pa - 2] == 0)
								{
									this.m_pa -= 2;
								}
								flag2 = false;
								flag3 = false;
								goto IL_06;
							}
							goto IL_1A5;
							IL_149:
							this.Fax3Unexpected(module);
							flag = true;
							goto IL_152;
						}
						this.Fax3PrematureEOF(module);
						this.CLEANUP_RUNS(module);
						return false;
					}
					IL_1A5:
					this.CLEANUP_RUNS(module);
					return true;
				}
				continue;
				IL_95:
				this.Fax3Unexpected(module);
				flag = true;
				goto IL_9E;
			}
			this.Fax3PrematureEOF(module);
			this.CLEANUP_RUNS(module);
			return false;
		}

		bool EXPAND2D(string module, int b1)
		{
			bool flag = false;
			IL_48F:
			while (this.m_a0 < this.m_rowpixels)
			{
				CCITTCodec.faxTableEntry faxTableEntry;
				if (!this.LOOKUP8(out faxTableEntry, 7))
				{
					this.handlePrematureEOFinExpand2D(module);
					return false;
				}
				switch (faxTableEntry.State)
				{
				case 1:
					this.CHECK_b1(ref b1);
					b1 += this.m_runs[this.m_pb];
					this.m_pb++;
					this.m_RunLength += b1 - this.m_a0;
					this.m_a0 = b1;
					b1 += this.m_runs[this.m_pb];
					this.m_pb++;
					continue;
				case 2:
					if (((this.m_pa - this.m_thisrun) & 1) != 0)
					{
						while (this.LOOKUP16(out faxTableEntry, 13, true))
						{
							bool flag2 = false;
							switch (faxTableEntry.State)
							{
							case 8:
								this.SETVALUE(faxTableEntry.Param);
								flag2 = true;
								break;
							case 9:
								goto IL_154;
							case 10:
							case 11:
								this.m_a0 += faxTableEntry.Param;
								this.m_RunLength += faxTableEntry.Param;
								break;
							default:
								goto IL_154;
							}
							IL_15D:
							if (!flag2 && !flag)
							{
								continue;
							}
							if (!flag)
							{
								while (this.LOOKUP16(out faxTableEntry, 12, false))
								{
									bool flag3 = false;
									switch (faxTableEntry.State)
									{
									case 7:
										this.SETVALUE(faxTableEntry.Param);
										flag3 = true;
										break;
									case 8:
									case 10:
										goto IL_1E7;
									case 9:
									case 11:
										this.m_a0 += faxTableEntry.Param;
										this.m_RunLength += faxTableEntry.Param;
										break;
									default:
										goto IL_1E7;
									}
									IL_1F0:
									if (!flag3 && !flag)
									{
										continue;
									}
									if (flag)
									{
										goto IL_48F;
									}
									goto IL_326;
									IL_1E7:
									this.Fax3Unexpected(module);
									flag = true;
									goto IL_1F0;
								}
								this.handlePrematureEOFinExpand2D(module);
								return false;
							}
							goto IL_48F;
							IL_154:
							this.Fax3Unexpected(module);
							flag = true;
							goto IL_15D;
						}
						this.handlePrematureEOFinExpand2D(module);
						return false;
					}
					while (this.LOOKUP16(out faxTableEntry, 12, false))
					{
						bool flag4 = false;
						switch (faxTableEntry.State)
						{
						case 7:
							this.SETVALUE(faxTableEntry.Param);
							flag4 = true;
							break;
						case 8:
						case 10:
							goto IL_281;
						case 9:
						case 11:
							this.m_a0 += faxTableEntry.Param;
							this.m_RunLength += faxTableEntry.Param;
							break;
						default:
							goto IL_281;
						}
						IL_28A:
						if (!flag4 && !flag)
						{
							continue;
						}
						if (!flag)
						{
							while (this.LOOKUP16(out faxTableEntry, 13, true))
							{
								bool flag5 = false;
								switch (faxTableEntry.State)
								{
								case 8:
									this.SETVALUE(faxTableEntry.Param);
									flag5 = true;
									break;
								case 9:
									goto IL_313;
								case 10:
								case 11:
									this.m_a0 += faxTableEntry.Param;
									this.m_RunLength += faxTableEntry.Param;
									break;
								default:
									goto IL_313;
								}
								IL_31C:
								if (flag5 || flag)
								{
									goto IL_326;
								}
								continue;
								IL_313:
								this.Fax3Unexpected(module);
								flag = true;
								goto IL_31C;
							}
							this.handlePrematureEOFinExpand2D(module);
							return false;
						}
						goto IL_48F;
						IL_281:
						this.Fax3Unexpected(module);
						flag = true;
						goto IL_28A;
					}
					this.handlePrematureEOFinExpand2D(module);
					return false;
					IL_326:
					if (!flag)
					{
						this.CHECK_b1(ref b1);
						continue;
					}
					continue;
				case 3:
					this.CHECK_b1(ref b1);
					this.SETVALUE(b1 - this.m_a0);
					b1 += this.m_runs[this.m_pb];
					this.m_pb++;
					continue;
				case 4:
					this.CHECK_b1(ref b1);
					this.SETVALUE(b1 - this.m_a0 + faxTableEntry.Param);
					b1 += this.m_runs[this.m_pb];
					this.m_pb++;
					continue;
				case 5:
					this.CHECK_b1(ref b1);
					this.SETVALUE(b1 - this.m_a0 - faxTableEntry.Param);
					this.m_pb--;
					b1 -= this.m_runs[this.m_pb];
					continue;
				case 6:
					this.m_runs[this.m_pa] = this.m_rowpixels - this.m_a0;
					this.m_pa++;
					this.Fax3Extension(module);
					flag = true;
					continue;
				case 12:
					this.m_runs[this.m_pa] = this.m_rowpixels - this.m_a0;
					this.m_pa++;
					if (!this.NeedBits8(4))
					{
						this.handlePrematureEOFinExpand2D(module);
						return false;
					}
					if (this.GetBits(4) != 0)
					{
						this.Fax3Unexpected(module);
					}
					this.ClrBits(4);
					this.m_EOLcnt = 1;
					flag = true;
					continue;
				}
				this.Fax3Unexpected(module);
				flag = true;
			}
			if (!flag && this.m_RunLength != 0)
			{
				if (this.m_RunLength + this.m_a0 < this.m_rowpixels)
				{
					if (!this.NeedBits8(1))
					{
						this.handlePrematureEOFinExpand2D(module);
						return false;
					}
					if (this.GetBits(1) == 0)
					{
						this.Fax3Unexpected(module);
						flag = true;
					}
					if (!flag)
					{
						this.ClrBits(1);
					}
				}
				if (!flag)
				{
					this.SETVALUE(0);
				}
			}
			this.CLEANUP_RUNS(module);
			return true;
		}

		bool TIFFInitCCITTRLE()
		{
			this.InitCCITTFax3();
			this.m_decoder = CCITTCodec.Decoder.useFax3RLEDecoder;
			return this.m_tif.SetField(TiffTag.FAXMODE, new object[] { (FaxMode)7 });
		}

		bool TIFFInitCCITTRLEW()
		{
			this.InitCCITTFax3();
			this.m_decoder = CCITTCodec.Decoder.useFax3RLEDecoder;
			return this.m_tif.SetField(TiffTag.FAXMODE, new object[] { (FaxMode)11 });
		}

		bool Fax3DecodeRLE(byte[] buffer, int offset, int count)
		{
			int curruns = this.m_curruns;
			while (count > 0)
			{
				this.m_a0 = 0;
				this.m_RunLength = 0;
				this.m_pa = curruns;
				bool flag = this.EXPAND1D("Fax3DecodeRLE");
				if (!flag)
				{
					this.fill(buffer, offset, this.m_runs, curruns, this.m_pa, this.m_rowpixels);
					return false;
				}
				this.fill(buffer, offset, this.m_runs, curruns, this.m_pa, this.m_rowpixels);
				if ((this.m_mode & FaxMode.BYTEALIGN) != FaxMode.CLASSIC)
				{
					int n = this.m_bit - (this.m_bit & -8);
					this.ClrBits(n);
				}
				else if ((this.m_mode & FaxMode.WORDALIGN) != FaxMode.CLASSIC)
				{
					int n2 = this.m_bit - (this.m_bit & -16);
					this.ClrBits(n2);
					if (this.m_bit == 0 && !CCITTCodec.isShortAligned(this.m_tif.m_rawcp))
					{
						this.m_tif.m_rawcp++;
					}
				}
				offset += this.m_rowbytes;
				count -= this.m_rowbytes;
				this.m_line++;
			}
			return true;
		}

		bool TIFFInitCCITTFax4()
		{
			this.InitCCITTFax3();
			this.m_tif.MergeFieldInfo(CCITTCodec.m_fax4FieldInfo, CCITTCodec.m_fax4FieldInfo.Length);
			this.m_decoder = CCITTCodec.Decoder.useFax4Decoder;
			this.m_encodingFax4 = true;
			return this.m_tif.SetField(TiffTag.FAXMODE, new object[] { FaxMode.NORTC });
		}

		bool Fax4Decode(byte[] buffer, int offset, int count)
		{
			while (count > 0)
			{
				this.m_a0 = 0;
				this.m_RunLength = 0;
				this.m_thisrun = this.m_curruns;
				this.m_pa = this.m_curruns;
				this.m_pb = this.m_refruns;
				int b = this.m_runs[this.m_pb];
				this.m_pb++;
				bool flag = this.EXPAND2D("Fax4Decode", b);
				if (flag && this.m_EOLcnt != 0)
				{
					flag = false;
				}
				if (!flag)
				{
					this.NeedBits16(13);
					this.ClrBits(13);
					this.fill(buffer, offset, this.m_runs, this.m_thisrun, this.m_pa, this.m_rowpixels);
					return false;
				}
				this.fill(buffer, offset, this.m_runs, this.m_thisrun, this.m_pa, this.m_rowpixels);
				this.SETVALUE(0);
				CCITTCodec.SWAP(ref this.m_curruns, ref this.m_refruns);
				offset += this.m_rowbytes;
				count -= this.m_rowbytes;
				this.m_line++;
			}
			return true;
		}

		bool Fax4Encode(byte[] buffer, int offset, int count)
		{
			this.m_buffer = buffer;
			this.m_offset = offset;
			while (count > 0)
			{
				if (!this.Fax3Encode2DRow())
				{
					return false;
				}
				Buffer.BlockCopy(this.m_buffer, this.m_offset, this.m_refline, 0, this.m_rowbytes);
				this.m_offset += this.m_rowbytes;
				count -= this.m_rowbytes;
			}
			return true;
		}

		bool Fax4PostEncode()
		{
			this.putBits(1, 12);
			this.putBits(1, 12);
			if (this.m_bit != 8)
			{
				this.flushBits();
			}
			return true;
		}

		public const int FIELD_BADFAXLINES = 66;

		public const int FIELD_CLEANFAXDATA = 67;

		public const int FIELD_BADFAXRUN = 68;

		public const int FIELD_RECVPARAMS = 69;

		public const int FIELD_SUBADDRESS = 70;

		public const int FIELD_RECVTIME = 71;

		public const int FIELD_FAXDCS = 72;

		public const int FIELD_OPTIONS = 73;

		const int EOL_CODE = 1;

		const byte S_Null = 0;

		const byte S_Pass = 1;

		const byte S_Horiz = 2;

		const byte S_V0 = 3;

		const byte S_VR = 4;

		const byte S_VL = 5;

		const byte S_Ext = 6;

		const byte S_TermW = 7;

		const byte S_TermB = 8;

		const byte S_MakeUpW = 9;

		const byte S_MakeUpB = 10;

		const byte S_MakeUp = 11;

		const byte S_EOL = 12;

		const short G3CODE_EOL = -1;

		const short G3CODE_INVALID = -2;

		const short G3CODE_EOF = -3;

		const short G3CODE_INCOMP = -4;

		internal FaxMode m_mode;

		internal Group3Opt m_groupoptions;

		internal CleanFaxData m_cleanfaxdata;

		internal int m_badfaxlines;

		internal int m_badfaxrun;

		internal int m_recvparams;

		internal string m_subaddress;

		internal int m_recvtime;

		internal string m_faxdcs;

		internal Tiff.FaxFillFunc fill;

		static readonly TiffFieldInfo[] m_faxFieldInfo = new TiffFieldInfo[]
		{
			new TiffFieldInfo(TiffTag.FAXMODE, 0, 0, TiffType.NOTYPE, 0, false, false, "FaxMode"),
			new TiffFieldInfo(TiffTag.FAXFILLFUNC, 0, 0, TiffType.NOTYPE, 0, false, false, "FaxFillFunc"),
			new TiffFieldInfo(TiffTag.BADFAXLINES, 1, 1, TiffType.LONG, 66, true, false, "BadFaxLines"),
			new TiffFieldInfo(TiffTag.BADFAXLINES, 1, 1, TiffType.SHORT, 66, true, false, "BadFaxLines"),
			new TiffFieldInfo(TiffTag.CLEANFAXDATA, 1, 1, TiffType.SHORT, 67, true, false, "CleanFaxData"),
			new TiffFieldInfo(TiffTag.CONSECUTIVEBADFAXLINES, 1, 1, TiffType.LONG, 68, true, false, "ConsecutiveBadFaxLines"),
			new TiffFieldInfo(TiffTag.CONSECUTIVEBADFAXLINES, 1, 1, TiffType.SHORT, 68, true, false, "ConsecutiveBadFaxLines"),
			new TiffFieldInfo(TiffTag.FAXRECVPARAMS, 1, 1, TiffType.LONG, 69, true, false, "FaxRecvParams"),
			new TiffFieldInfo(TiffTag.FAXSUBADDRESS, -1, -1, TiffType.ASCII, 70, true, false, "FaxSubAddress"),
			new TiffFieldInfo(TiffTag.FAXRECVTIME, 1, 1, TiffType.LONG, 71, true, false, "FaxRecvTime"),
			new TiffFieldInfo(TiffTag.FAXDCS, -1, -1, TiffType.ASCII, 72, true, false, "FaxDcs")
		};

		static readonly TiffFieldInfo[] m_fax3FieldInfo = new TiffFieldInfo[]
		{
			new TiffFieldInfo(TiffTag.GROUP3OPTIONS, 1, 1, TiffType.LONG, 73, false, false, "Group3Options")
		};

		static readonly TiffFieldInfo[] m_fax4FieldInfo = new TiffFieldInfo[]
		{
			new TiffFieldInfo(TiffTag.GROUP4OPTIONS, 1, 1, TiffType.LONG, 73, false, false, "Group4Options")
		};

		TiffTagMethods m_parentTagMethods;

		TiffTagMethods m_tagMethods;

		int m_rw_mode;

		int m_rowbytes;

		int m_rowpixels;

		CCITTCodec.Decoder m_decoder;

		byte[] m_bitmap;

		int m_data;

		int m_bit;

		int m_EOLcnt;

		int[] m_runs;

		int m_refruns;

		int m_curruns;

		int m_a0;

		int m_RunLength;

		int m_thisrun;

		int m_pa;

		int m_pb;

		CCITTCodec.Fax3Encoder m_encoder;

		bool m_encodingFax4;

		byte[] m_refline;

		int m_k;

		int m_maxk;

		int m_line;

		byte[] m_buffer;

		int m_offset;

		static readonly int[] m_faxMainTable = new int[]
		{
			12, 7, 0, 3, 1, 0, 5, 3, 1, 3,
			1, 0, 2, 3, 0, 3, 1, 0, 4, 3,
			1, 3, 1, 0, 1, 4, 0, 3, 1, 0,
			5, 3, 1, 3, 1, 0, 2, 3, 0, 3,
			1, 0, 4, 3, 1, 3, 1, 0, 5, 6,
			2, 3, 1, 0, 5, 3, 1, 3, 1, 0,
			2, 3, 0, 3, 1, 0, 4, 3, 1, 3,
			1, 0, 1, 4, 0, 3, 1, 0, 5, 3,
			1, 3, 1, 0, 2, 3, 0, 3, 1, 0,
			4, 3, 1, 3, 1, 0, 5, 7, 3, 3,
			1, 0, 5, 3, 1, 3, 1, 0, 2, 3,
			0, 3, 1, 0, 4, 3, 1, 3, 1, 0,
			1, 4, 0, 3, 1, 0, 5, 3, 1, 3,
			1, 0, 2, 3, 0, 3, 1, 0, 4, 3,
			1, 3, 1, 0, 4, 6, 2, 3, 1, 0,
			5, 3, 1, 3, 1, 0, 2, 3, 0, 3,
			1, 0, 4, 3, 1, 3, 1, 0, 1, 4,
			0, 3, 1, 0, 5, 3, 1, 3, 1, 0,
			2, 3, 0, 3, 1, 0, 4, 3, 1, 3,
			1, 0, 6, 7, 0, 3, 1, 0, 5, 3,
			1, 3, 1, 0, 2, 3, 0, 3, 1, 0,
			4, 3, 1, 3, 1, 0, 1, 4, 0, 3,
			1, 0, 5, 3, 1, 3, 1, 0, 2, 3,
			0, 3, 1, 0, 4, 3, 1, 3, 1, 0,
			5, 6, 2, 3, 1, 0, 5, 3, 1, 3,
			1, 0, 2, 3, 0, 3, 1, 0, 4, 3,
			1, 3, 1, 0, 1, 4, 0, 3, 1, 0,
			5, 3, 1, 3, 1, 0, 2, 3, 0, 3,
			1, 0, 4, 3, 1, 3, 1, 0, 4, 7,
			3, 3, 1, 0, 5, 3, 1, 3, 1, 0,
			2, 3, 0, 3, 1, 0, 4, 3, 1, 3,
			1, 0, 1, 4, 0, 3, 1, 0, 5, 3,
			1, 3, 1, 0, 2, 3, 0, 3, 1, 0,
			4, 3, 1, 3, 1, 0, 4, 6, 2, 3,
			1, 0, 5, 3, 1, 3, 1, 0, 2, 3,
			0, 3, 1, 0, 4, 3, 1, 3, 1, 0,
			1, 4, 0, 3, 1, 0, 5, 3, 1, 3,
			1, 0, 2, 3, 0, 3, 1, 0, 4, 3,
			1, 3, 1, 0
		};

		static readonly int[] m_faxWhiteTable = new int[]
		{
			12, 11, 0, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 6, 12, 7, 5, 9, 9, 6,
			1664, 7, 4, 6, 7, 7, 20, 9, 5, 128,
			7, 7, 24, 7, 6, 14, 7, 7, 28, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 7,
			23, 7, 4, 3, 7, 7, 27, 7, 4, 5,
			7, 8, 39, 7, 6, 16, 9, 8, 576, 7,
			4, 6, 7, 7, 19, 7, 5, 8, 7, 8,
			55, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 8, 45, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 8,
			53, 7, 5, 9, 9, 8, 448, 7, 4, 6,
			7, 8, 35, 9, 5, 128, 7, 8, 51, 7,
			6, 15, 7, 8, 63, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 6, 13, 7, 4, 3,
			9, 9, 1472, 7, 4, 5, 7, 8, 43, 7,
			6, 17, 9, 9, 1216, 7, 4, 6, 7, 6,
			1, 7, 5, 8, 9, 6, 192, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 8, 29, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 6, 12, 7, 5, 9,
			9, 6, 1664, 7, 4, 6, 7, 8, 33, 9,
			5, 128, 7, 8, 49, 7, 6, 14, 7, 8,
			61, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 8, 47, 7, 4, 3, 7, 8, 59, 7,
			4, 5, 7, 8, 41, 7, 6, 16, 9, 9,
			960, 7, 4, 6, 7, 8, 31, 7, 5, 8,
			7, 8, 57, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 7,
			22, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 7, 26, 7, 5, 9, 9, 9, 704, 7,
			4, 6, 7, 8, 37, 9, 5, 128, 7, 7,
			25, 7, 6, 15, 9, 8, 320, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 6, 13, 7,
			4, 3, 7, 7, 18, 7, 4, 5, 7, 7,
			21, 7, 6, 17, 9, 7, 256, 7, 4, 6,
			7, 6, 1, 7, 5, 8, 9, 6, 192, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 11, 11, 1792, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 6, 12, 7,
			5, 9, 9, 6, 1664, 7, 4, 6, 7, 7,
			20, 9, 5, 128, 7, 7, 24, 7, 6, 14,
			7, 7, 28, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 7, 23, 7, 4, 3, 7, 7,
			27, 7, 4, 5, 7, 8, 40, 7, 6, 16,
			9, 9, 832, 7, 4, 6, 7, 7, 19, 7,
			5, 8, 7, 8, 56, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 8, 46, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 8, 54, 7, 5, 9, 9, 8,
			512, 7, 4, 6, 7, 8, 36, 9, 5, 128,
			7, 8, 52, 7, 6, 15, 7, 8, 0, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 6,
			13, 7, 4, 3, 9, 9, 1600, 7, 4, 5,
			7, 8, 44, 7, 6, 17, 9, 9, 1344, 7,
			4, 6, 7, 6, 1, 7, 5, 8, 9, 6,
			192, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 8, 30, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 6,
			12, 7, 5, 9, 9, 6, 1664, 7, 4, 6,
			7, 8, 34, 9, 5, 128, 7, 8, 50, 7,
			6, 14, 7, 8, 62, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 8, 48, 7, 4, 3,
			7, 8, 60, 7, 4, 5, 7, 8, 42, 7,
			6, 16, 9, 9, 1088, 7, 4, 6, 7, 8,
			32, 7, 5, 8, 7, 8, 58, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 7, 22, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 7, 26, 7, 5, 9,
			9, 8, 640, 7, 4, 6, 7, 8, 38, 9,
			5, 128, 7, 7, 25, 7, 6, 15, 9, 8,
			384, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 6, 13, 7, 4, 3, 7, 7, 18, 7,
			4, 5, 7, 7, 21, 7, 6, 17, 9, 7,
			256, 7, 4, 6, 7, 6, 1, 7, 5, 8,
			9, 6, 192, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 0, 0,
			0, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 6, 12, 7, 5, 9, 9, 6, 1664, 7,
			4, 6, 7, 7, 20, 9, 5, 128, 7, 7,
			24, 7, 6, 14, 7, 7, 28, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 7, 23, 7,
			4, 3, 7, 7, 27, 7, 4, 5, 7, 8,
			39, 7, 6, 16, 9, 8, 576, 7, 4, 6,
			7, 7, 19, 7, 5, 8, 7, 8, 55, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 8, 45, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 8, 53, 7,
			5, 9, 9, 8, 448, 7, 4, 6, 7, 8,
			35, 9, 5, 128, 7, 8, 51, 7, 6, 15,
			7, 8, 63, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 6, 13, 7, 4, 3, 9, 9,
			1536, 7, 4, 5, 7, 8, 43, 7, 6, 17,
			9, 9, 1280, 7, 4, 6, 7, 6, 1, 7,
			5, 8, 9, 6, 192, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 8, 29, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 6, 12, 7, 5, 9, 9, 6,
			1664, 7, 4, 6, 7, 8, 33, 9, 5, 128,
			7, 8, 49, 7, 6, 14, 7, 8, 61, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 8,
			47, 7, 4, 3, 7, 8, 59, 7, 4, 5,
			7, 8, 41, 7, 6, 16, 9, 9, 1024, 7,
			4, 6, 7, 8, 31, 7, 5, 8, 7, 8,
			57, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 7, 22, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 7,
			26, 7, 5, 9, 9, 9, 768, 7, 4, 6,
			7, 8, 37, 9, 5, 128, 7, 7, 25, 7,
			6, 15, 9, 8, 320, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 6, 13, 7, 4, 3,
			7, 7, 18, 7, 4, 5, 7, 7, 21, 7,
			6, 17, 9, 7, 256, 7, 4, 6, 7, 6,
			1, 7, 5, 8, 9, 6, 192, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 11, 11, 1856, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 6, 12, 7, 5, 9,
			9, 6, 1664, 7, 4, 6, 7, 7, 20, 9,
			5, 128, 7, 7, 24, 7, 6, 14, 7, 7,
			28, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 7, 23, 7, 4, 3, 7, 7, 27, 7,
			4, 5, 7, 8, 40, 7, 6, 16, 9, 9,
			896, 7, 4, 6, 7, 7, 19, 7, 5, 8,
			7, 8, 56, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 8,
			46, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 8, 54, 7, 5, 9, 9, 8, 512, 7,
			4, 6, 7, 8, 36, 9, 5, 128, 7, 8,
			52, 7, 6, 15, 7, 8, 0, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 6, 13, 7,
			4, 3, 9, 9, 1728, 7, 4, 5, 7, 8,
			44, 7, 6, 17, 9, 9, 1408, 7, 4, 6,
			7, 6, 1, 7, 5, 8, 9, 6, 192, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 8, 30, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 6, 12, 7,
			5, 9, 9, 6, 1664, 7, 4, 6, 7, 8,
			34, 9, 5, 128, 7, 8, 50, 7, 6, 14,
			7, 8, 62, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 8, 48, 7, 4, 3, 7, 8,
			60, 7, 4, 5, 7, 8, 42, 7, 6, 16,
			9, 9, 1152, 7, 4, 6, 7, 8, 32, 7,
			5, 8, 7, 8, 58, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 7, 22, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 7, 26, 7, 5, 9, 9, 8,
			640, 7, 4, 6, 7, 8, 38, 9, 5, 128,
			7, 7, 25, 7, 6, 15, 9, 8, 384, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 6,
			13, 7, 4, 3, 7, 7, 18, 7, 4, 5,
			7, 7, 21, 7, 6, 17, 9, 7, 256, 7,
			4, 6, 7, 6, 1, 7, 5, 8, 9, 6,
			192, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 0, 0, 0, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 6,
			12, 7, 5, 9, 9, 6, 1664, 7, 4, 6,
			7, 7, 20, 9, 5, 128, 7, 7, 24, 7,
			6, 14, 7, 7, 28, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 7, 23, 7, 4, 3,
			7, 7, 27, 7, 4, 5, 7, 8, 39, 7,
			6, 16, 9, 8, 576, 7, 4, 6, 7, 7,
			19, 7, 5, 8, 7, 8, 55, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 8, 45, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 8, 53, 7, 5, 9,
			9, 8, 448, 7, 4, 6, 7, 8, 35, 9,
			5, 128, 7, 8, 51, 7, 6, 15, 7, 8,
			63, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 6, 13, 7, 4, 3, 9, 9, 1472, 7,
			4, 5, 7, 8, 43, 7, 6, 17, 9, 9,
			1216, 7, 4, 6, 7, 6, 1, 7, 5, 8,
			9, 6, 192, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 8,
			29, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 6, 12, 7, 5, 9, 9, 6, 1664, 7,
			4, 6, 7, 8, 33, 9, 5, 128, 7, 8,
			49, 7, 6, 14, 7, 8, 61, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 8, 47, 7,
			4, 3, 7, 8, 59, 7, 4, 5, 7, 8,
			41, 7, 6, 16, 9, 9, 960, 7, 4, 6,
			7, 8, 31, 7, 5, 8, 7, 8, 57, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 7, 22, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 7, 26, 7,
			5, 9, 9, 9, 704, 7, 4, 6, 7, 8,
			37, 9, 5, 128, 7, 7, 25, 7, 6, 15,
			9, 8, 320, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 6, 13, 7, 4, 3, 7, 7,
			18, 7, 4, 5, 7, 7, 21, 7, 6, 17,
			9, 7, 256, 7, 4, 6, 7, 6, 1, 7,
			5, 8, 9, 6, 192, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			11, 12, 2112, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 6, 12, 7, 5, 9, 9, 6,
			1664, 7, 4, 6, 7, 7, 20, 9, 5, 128,
			7, 7, 24, 7, 6, 14, 7, 7, 28, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 7,
			23, 7, 4, 3, 7, 7, 27, 7, 4, 5,
			7, 8, 40, 7, 6, 16, 9, 9, 832, 7,
			4, 6, 7, 7, 19, 7, 5, 8, 7, 8,
			56, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 8, 46, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 8,
			54, 7, 5, 9, 9, 8, 512, 7, 4, 6,
			7, 8, 36, 9, 5, 128, 7, 8, 52, 7,
			6, 15, 7, 8, 0, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 6, 13, 7, 4, 3,
			9, 9, 1600, 7, 4, 5, 7, 8, 44, 7,
			6, 17, 9, 9, 1344, 7, 4, 6, 7, 6,
			1, 7, 5, 8, 9, 6, 192, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 8, 30, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 6, 12, 7, 5, 9,
			9, 6, 1664, 7, 4, 6, 7, 8, 34, 9,
			5, 128, 7, 8, 50, 7, 6, 14, 7, 8,
			62, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 8, 48, 7, 4, 3, 7, 8, 60, 7,
			4, 5, 7, 8, 42, 7, 6, 16, 9, 9,
			1088, 7, 4, 6, 7, 8, 32, 7, 5, 8,
			7, 8, 58, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 7,
			22, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 7, 26, 7, 5, 9, 9, 8, 640, 7,
			4, 6, 7, 8, 38, 9, 5, 128, 7, 7,
			25, 7, 6, 15, 9, 8, 384, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 6, 13, 7,
			4, 3, 7, 7, 18, 7, 4, 5, 7, 7,
			21, 7, 6, 17, 9, 7, 256, 7, 4, 6,
			7, 6, 1, 7, 5, 8, 9, 6, 192, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 0, 0, 0, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 6, 12, 7,
			5, 9, 9, 6, 1664, 7, 4, 6, 7, 7,
			20, 9, 5, 128, 7, 7, 24, 7, 6, 14,
			7, 7, 28, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 7, 23, 7, 4, 3, 7, 7,
			27, 7, 4, 5, 7, 8, 39, 7, 6, 16,
			9, 8, 576, 7, 4, 6, 7, 7, 19, 7,
			5, 8, 7, 8, 55, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 8, 45, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 8, 53, 7, 5, 9, 9, 8,
			448, 7, 4, 6, 7, 8, 35, 9, 5, 128,
			7, 8, 51, 7, 6, 15, 7, 8, 63, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 6,
			13, 7, 4, 3, 9, 9, 1536, 7, 4, 5,
			7, 8, 43, 7, 6, 17, 9, 9, 1280, 7,
			4, 6, 7, 6, 1, 7, 5, 8, 9, 6,
			192, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 8, 29, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 6,
			12, 7, 5, 9, 9, 6, 1664, 7, 4, 6,
			7, 8, 33, 9, 5, 128, 7, 8, 49, 7,
			6, 14, 7, 8, 61, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 8, 47, 7, 4, 3,
			7, 8, 59, 7, 4, 5, 7, 8, 41, 7,
			6, 16, 9, 9, 1024, 7, 4, 6, 7, 8,
			31, 7, 5, 8, 7, 8, 57, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 7, 22, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 7, 26, 7, 5, 9,
			9, 9, 768, 7, 4, 6, 7, 8, 37, 9,
			5, 128, 7, 7, 25, 7, 6, 15, 9, 8,
			320, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 6, 13, 7, 4, 3, 7, 7, 18, 7,
			4, 5, 7, 7, 21, 7, 6, 17, 9, 7,
			256, 7, 4, 6, 7, 6, 1, 7, 5, 8,
			9, 6, 192, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 11, 12,
			2368, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 6, 12, 7, 5, 9, 9, 6, 1664, 7,
			4, 6, 7, 7, 20, 9, 5, 128, 7, 7,
			24, 7, 6, 14, 7, 7, 28, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 7, 23, 7,
			4, 3, 7, 7, 27, 7, 4, 5, 7, 8,
			40, 7, 6, 16, 9, 9, 896, 7, 4, 6,
			7, 7, 19, 7, 5, 8, 7, 8, 56, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 8, 46, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 8, 54, 7,
			5, 9, 9, 8, 512, 7, 4, 6, 7, 8,
			36, 9, 5, 128, 7, 8, 52, 7, 6, 15,
			7, 8, 0, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 6, 13, 7, 4, 3, 9, 9,
			1728, 7, 4, 5, 7, 8, 44, 7, 6, 17,
			9, 9, 1408, 7, 4, 6, 7, 6, 1, 7,
			5, 8, 9, 6, 192, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 8, 30, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 6, 12, 7, 5, 9, 9, 6,
			1664, 7, 4, 6, 7, 8, 34, 9, 5, 128,
			7, 8, 50, 7, 6, 14, 7, 8, 62, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 8,
			48, 7, 4, 3, 7, 8, 60, 7, 4, 5,
			7, 8, 42, 7, 6, 16, 9, 9, 1152, 7,
			4, 6, 7, 8, 32, 7, 5, 8, 7, 8,
			58, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 7, 22, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 7,
			26, 7, 5, 9, 9, 8, 640, 7, 4, 6,
			7, 8, 38, 9, 5, 128, 7, 7, 25, 7,
			6, 15, 9, 8, 384, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 6, 13, 7, 4, 3,
			7, 7, 18, 7, 4, 5, 7, 7, 21, 7,
			6, 17, 9, 7, 256, 7, 4, 6, 7, 6,
			1, 7, 5, 8, 9, 6, 192, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 0, 0, 0, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 6, 12, 7, 5, 9,
			9, 6, 1664, 7, 4, 6, 7, 7, 20, 9,
			5, 128, 7, 7, 24, 7, 6, 14, 7, 7,
			28, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 7, 23, 7, 4, 3, 7, 7, 27, 7,
			4, 5, 7, 8, 39, 7, 6, 16, 9, 8,
			576, 7, 4, 6, 7, 7, 19, 7, 5, 8,
			7, 8, 55, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 8,
			45, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 8, 53, 7, 5, 9, 9, 8, 448, 7,
			4, 6, 7, 8, 35, 9, 5, 128, 7, 8,
			51, 7, 6, 15, 7, 8, 63, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 6, 13, 7,
			4, 3, 9, 9, 1472, 7, 4, 5, 7, 8,
			43, 7, 6, 17, 9, 9, 1216, 7, 4, 6,
			7, 6, 1, 7, 5, 8, 9, 6, 192, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 8, 29, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 6, 12, 7,
			5, 9, 9, 6, 1664, 7, 4, 6, 7, 8,
			33, 9, 5, 128, 7, 8, 49, 7, 6, 14,
			7, 8, 61, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 8, 47, 7, 4, 3, 7, 8,
			59, 7, 4, 5, 7, 8, 41, 7, 6, 16,
			9, 9, 960, 7, 4, 6, 7, 8, 31, 7,
			5, 8, 7, 8, 57, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 7, 22, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 7, 26, 7, 5, 9, 9, 9,
			704, 7, 4, 6, 7, 8, 37, 9, 5, 128,
			7, 7, 25, 7, 6, 15, 9, 8, 320, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 6,
			13, 7, 4, 3, 7, 7, 18, 7, 4, 5,
			7, 7, 21, 7, 6, 17, 9, 7, 256, 7,
			4, 6, 7, 6, 1, 7, 5, 8, 9, 6,
			192, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 11, 12, 1984, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 6,
			12, 7, 5, 9, 9, 6, 1664, 7, 4, 6,
			7, 7, 20, 9, 5, 128, 7, 7, 24, 7,
			6, 14, 7, 7, 28, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 7, 23, 7, 4, 3,
			7, 7, 27, 7, 4, 5, 7, 8, 40, 7,
			6, 16, 9, 9, 832, 7, 4, 6, 7, 7,
			19, 7, 5, 8, 7, 8, 56, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 8, 46, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 8, 54, 7, 5, 9,
			9, 8, 512, 7, 4, 6, 7, 8, 36, 9,
			5, 128, 7, 8, 52, 7, 6, 15, 7, 8,
			0, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 6, 13, 7, 4, 3, 9, 9, 1600, 7,
			4, 5, 7, 8, 44, 7, 6, 17, 9, 9,
			1344, 7, 4, 6, 7, 6, 1, 7, 5, 8,
			9, 6, 192, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 8,
			30, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 6, 12, 7, 5, 9, 9, 6, 1664, 7,
			4, 6, 7, 8, 34, 9, 5, 128, 7, 8,
			50, 7, 6, 14, 7, 8, 62, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 8, 48, 7,
			4, 3, 7, 8, 60, 7, 4, 5, 7, 8,
			42, 7, 6, 16, 9, 9, 1088, 7, 4, 6,
			7, 8, 32, 7, 5, 8, 7, 8, 58, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 7, 22, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 7, 26, 7,
			5, 9, 9, 8, 640, 7, 4, 6, 7, 8,
			38, 9, 5, 128, 7, 7, 25, 7, 6, 15,
			9, 8, 384, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 6, 13, 7, 4, 3, 7, 7,
			18, 7, 4, 5, 7, 7, 21, 7, 6, 17,
			9, 7, 256, 7, 4, 6, 7, 6, 1, 7,
			5, 8, 9, 6, 192, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			0, 0, 0, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 6, 12, 7, 5, 9, 9, 6,
			1664, 7, 4, 6, 7, 7, 20, 9, 5, 128,
			7, 7, 24, 7, 6, 14, 7, 7, 28, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 7,
			23, 7, 4, 3, 7, 7, 27, 7, 4, 5,
			7, 8, 39, 7, 6, 16, 9, 8, 576, 7,
			4, 6, 7, 7, 19, 7, 5, 8, 7, 8,
			55, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 8, 45, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 8,
			53, 7, 5, 9, 9, 8, 448, 7, 4, 6,
			7, 8, 35, 9, 5, 128, 7, 8, 51, 7,
			6, 15, 7, 8, 63, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 6, 13, 7, 4, 3,
			9, 9, 1536, 7, 4, 5, 7, 8, 43, 7,
			6, 17, 9, 9, 1280, 7, 4, 6, 7, 6,
			1, 7, 5, 8, 9, 6, 192, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 8, 29, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 6, 12, 7, 5, 9,
			9, 6, 1664, 7, 4, 6, 7, 8, 33, 9,
			5, 128, 7, 8, 49, 7, 6, 14, 7, 8,
			61, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 8, 47, 7, 4, 3, 7, 8, 59, 7,
			4, 5, 7, 8, 41, 7, 6, 16, 9, 9,
			1024, 7, 4, 6, 7, 8, 31, 7, 5, 8,
			7, 8, 57, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 7,
			22, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 7, 26, 7, 5, 9, 9, 9, 768, 7,
			4, 6, 7, 8, 37, 9, 5, 128, 7, 7,
			25, 7, 6, 15, 9, 8, 320, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 6, 13, 7,
			4, 3, 7, 7, 18, 7, 4, 5, 7, 7,
			21, 7, 6, 17, 9, 7, 256, 7, 4, 6,
			7, 6, 1, 7, 5, 8, 9, 6, 192, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 11, 11, 1920, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 6, 12, 7,
			5, 9, 9, 6, 1664, 7, 4, 6, 7, 7,
			20, 9, 5, 128, 7, 7, 24, 7, 6, 14,
			7, 7, 28, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 7, 23, 7, 4, 3, 7, 7,
			27, 7, 4, 5, 7, 8, 40, 7, 6, 16,
			9, 9, 896, 7, 4, 6, 7, 7, 19, 7,
			5, 8, 7, 8, 56, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 8, 46, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 8, 54, 7, 5, 9, 9, 8,
			512, 7, 4, 6, 7, 8, 36, 9, 5, 128,
			7, 8, 52, 7, 6, 15, 7, 8, 0, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 6,
			13, 7, 4, 3, 9, 9, 1728, 7, 4, 5,
			7, 8, 44, 7, 6, 17, 9, 9, 1408, 7,
			4, 6, 7, 6, 1, 7, 5, 8, 9, 6,
			192, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 8, 30, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 6,
			12, 7, 5, 9, 9, 6, 1664, 7, 4, 6,
			7, 8, 34, 9, 5, 128, 7, 8, 50, 7,
			6, 14, 7, 8, 62, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 8, 48, 7, 4, 3,
			7, 8, 60, 7, 4, 5, 7, 8, 42, 7,
			6, 16, 9, 9, 1152, 7, 4, 6, 7, 8,
			32, 7, 5, 8, 7, 8, 58, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 7, 22, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 7, 26, 7, 5, 9,
			9, 8, 640, 7, 4, 6, 7, 8, 38, 9,
			5, 128, 7, 7, 25, 7, 6, 15, 9, 8,
			384, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 6, 13, 7, 4, 3, 7, 7, 18, 7,
			4, 5, 7, 7, 21, 7, 6, 17, 9, 7,
			256, 7, 4, 6, 7, 6, 1, 7, 5, 8,
			9, 6, 192, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 0, 0,
			0, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 6, 12, 7, 5, 9, 9, 6, 1664, 7,
			4, 6, 7, 7, 20, 9, 5, 128, 7, 7,
			24, 7, 6, 14, 7, 7, 28, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 7, 23, 7,
			4, 3, 7, 7, 27, 7, 4, 5, 7, 8,
			39, 7, 6, 16, 9, 8, 576, 7, 4, 6,
			7, 7, 19, 7, 5, 8, 7, 8, 55, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 8, 45, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 8, 53, 7,
			5, 9, 9, 8, 448, 7, 4, 6, 7, 8,
			35, 9, 5, 128, 7, 8, 51, 7, 6, 15,
			7, 8, 63, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 6, 13, 7, 4, 3, 9, 9,
			1472, 7, 4, 5, 7, 8, 43, 7, 6, 17,
			9, 9, 1216, 7, 4, 6, 7, 6, 1, 7,
			5, 8, 9, 6, 192, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 8, 29, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 6, 12, 7, 5, 9, 9, 6,
			1664, 7, 4, 6, 7, 8, 33, 9, 5, 128,
			7, 8, 49, 7, 6, 14, 7, 8, 61, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 8,
			47, 7, 4, 3, 7, 8, 59, 7, 4, 5,
			7, 8, 41, 7, 6, 16, 9, 9, 960, 7,
			4, 6, 7, 8, 31, 7, 5, 8, 7, 8,
			57, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 7, 22, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 7,
			26, 7, 5, 9, 9, 9, 704, 7, 4, 6,
			7, 8, 37, 9, 5, 128, 7, 7, 25, 7,
			6, 15, 9, 8, 320, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 6, 13, 7, 4, 3,
			7, 7, 18, 7, 4, 5, 7, 7, 21, 7,
			6, 17, 9, 7, 256, 7, 4, 6, 7, 6,
			1, 7, 5, 8, 9, 6, 192, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 11, 12, 2240, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 6, 12, 7, 5, 9,
			9, 6, 1664, 7, 4, 6, 7, 7, 20, 9,
			5, 128, 7, 7, 24, 7, 6, 14, 7, 7,
			28, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 7, 23, 7, 4, 3, 7, 7, 27, 7,
			4, 5, 7, 8, 40, 7, 6, 16, 9, 9,
			832, 7, 4, 6, 7, 7, 19, 7, 5, 8,
			7, 8, 56, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 8,
			46, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 8, 54, 7, 5, 9, 9, 8, 512, 7,
			4, 6, 7, 8, 36, 9, 5, 128, 7, 8,
			52, 7, 6, 15, 7, 8, 0, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 6, 13, 7,
			4, 3, 9, 9, 1600, 7, 4, 5, 7, 8,
			44, 7, 6, 17, 9, 9, 1344, 7, 4, 6,
			7, 6, 1, 7, 5, 8, 9, 6, 192, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 8, 30, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 6, 12, 7,
			5, 9, 9, 6, 1664, 7, 4, 6, 7, 8,
			34, 9, 5, 128, 7, 8, 50, 7, 6, 14,
			7, 8, 62, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 8, 48, 7, 4, 3, 7, 8,
			60, 7, 4, 5, 7, 8, 42, 7, 6, 16,
			9, 9, 1088, 7, 4, 6, 7, 8, 32, 7,
			5, 8, 7, 8, 58, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 7, 22, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 7, 26, 7, 5, 9, 9, 8,
			640, 7, 4, 6, 7, 8, 38, 9, 5, 128,
			7, 7, 25, 7, 6, 15, 9, 8, 384, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 6,
			13, 7, 4, 3, 7, 7, 18, 7, 4, 5,
			7, 7, 21, 7, 6, 17, 9, 7, 256, 7,
			4, 6, 7, 6, 1, 7, 5, 8, 9, 6,
			192, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 0, 0, 0, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 6,
			12, 7, 5, 9, 9, 6, 1664, 7, 4, 6,
			7, 7, 20, 9, 5, 128, 7, 7, 24, 7,
			6, 14, 7, 7, 28, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 7, 23, 7, 4, 3,
			7, 7, 27, 7, 4, 5, 7, 8, 39, 7,
			6, 16, 9, 8, 576, 7, 4, 6, 7, 7,
			19, 7, 5, 8, 7, 8, 55, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 8, 45, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 8, 53, 7, 5, 9,
			9, 8, 448, 7, 4, 6, 7, 8, 35, 9,
			5, 128, 7, 8, 51, 7, 6, 15, 7, 8,
			63, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 6, 13, 7, 4, 3, 9, 9, 1536, 7,
			4, 5, 7, 8, 43, 7, 6, 17, 9, 9,
			1280, 7, 4, 6, 7, 6, 1, 7, 5, 8,
			9, 6, 192, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 8,
			29, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 6, 12, 7, 5, 9, 9, 6, 1664, 7,
			4, 6, 7, 8, 33, 9, 5, 128, 7, 8,
			49, 7, 6, 14, 7, 8, 61, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 8, 47, 7,
			4, 3, 7, 8, 59, 7, 4, 5, 7, 8,
			41, 7, 6, 16, 9, 9, 1024, 7, 4, 6,
			7, 8, 31, 7, 5, 8, 7, 8, 57, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 7, 22, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 7, 26, 7,
			5, 9, 9, 9, 768, 7, 4, 6, 7, 8,
			37, 9, 5, 128, 7, 7, 25, 7, 6, 15,
			9, 8, 320, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 6, 13, 7, 4, 3, 7, 7,
			18, 7, 4, 5, 7, 7, 21, 7, 6, 17,
			9, 7, 256, 7, 4, 6, 7, 6, 1, 7,
			5, 8, 9, 6, 192, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			11, 12, 2496, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 6, 12, 7, 5, 9, 9, 6,
			1664, 7, 4, 6, 7, 7, 20, 9, 5, 128,
			7, 7, 24, 7, 6, 14, 7, 7, 28, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 7,
			23, 7, 4, 3, 7, 7, 27, 7, 4, 5,
			7, 8, 40, 7, 6, 16, 9, 9, 896, 7,
			4, 6, 7, 7, 19, 7, 5, 8, 7, 8,
			56, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 8, 46, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 8,
			54, 7, 5, 9, 9, 8, 512, 7, 4, 6,
			7, 8, 36, 9, 5, 128, 7, 8, 52, 7,
			6, 15, 7, 8, 0, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 6, 13, 7, 4, 3,
			9, 9, 1728, 7, 4, 5, 7, 8, 44, 7,
			6, 17, 9, 9, 1408, 7, 4, 6, 7, 6,
			1, 7, 5, 8, 9, 6, 192, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 8, 30, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 6, 12, 7, 5, 9,
			9, 6, 1664, 7, 4, 6, 7, 8, 34, 9,
			5, 128, 7, 8, 50, 7, 6, 14, 7, 8,
			62, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 8, 48, 7, 4, 3, 7, 8, 60, 7,
			4, 5, 7, 8, 42, 7, 6, 16, 9, 9,
			1152, 7, 4, 6, 7, 8, 32, 7, 5, 8,
			7, 8, 58, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 7,
			22, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 7, 26, 7, 5, 9, 9, 8, 640, 7,
			4, 6, 7, 8, 38, 9, 5, 128, 7, 7,
			25, 7, 6, 15, 9, 8, 384, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 6, 13, 7,
			4, 3, 7, 7, 18, 7, 4, 5, 7, 7,
			21, 7, 6, 17, 9, 7, 256, 7, 4, 6,
			7, 6, 1, 7, 5, 8, 9, 6, 192, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 12, 11, 0, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 6, 12, 7,
			5, 9, 9, 6, 1664, 7, 4, 6, 7, 7,
			20, 9, 5, 128, 7, 7, 24, 7, 6, 14,
			7, 7, 28, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 7, 23, 7, 4, 3, 7, 7,
			27, 7, 4, 5, 7, 8, 39, 7, 6, 16,
			9, 8, 576, 7, 4, 6, 7, 7, 19, 7,
			5, 8, 7, 8, 55, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 8, 45, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 8, 53, 7, 5, 9, 9, 8,
			448, 7, 4, 6, 7, 8, 35, 9, 5, 128,
			7, 8, 51, 7, 6, 15, 7, 8, 63, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 6,
			13, 7, 4, 3, 9, 9, 1472, 7, 4, 5,
			7, 8, 43, 7, 6, 17, 9, 9, 1216, 7,
			4, 6, 7, 6, 1, 7, 5, 8, 9, 6,
			192, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 8, 29, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 6,
			12, 7, 5, 9, 9, 6, 1664, 7, 4, 6,
			7, 8, 33, 9, 5, 128, 7, 8, 49, 7,
			6, 14, 7, 8, 61, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 8, 47, 7, 4, 3,
			7, 8, 59, 7, 4, 5, 7, 8, 41, 7,
			6, 16, 9, 9, 960, 7, 4, 6, 7, 8,
			31, 7, 5, 8, 7, 8, 57, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 7, 22, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 7, 26, 7, 5, 9,
			9, 9, 704, 7, 4, 6, 7, 8, 37, 9,
			5, 128, 7, 7, 25, 7, 6, 15, 9, 8,
			320, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 6, 13, 7, 4, 3, 7, 7, 18, 7,
			4, 5, 7, 7, 21, 7, 6, 17, 9, 7,
			256, 7, 4, 6, 7, 6, 1, 7, 5, 8,
			9, 6, 192, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 11, 11,
			1792, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 6, 12, 7, 5, 9, 9, 6, 1664, 7,
			4, 6, 7, 7, 20, 9, 5, 128, 7, 7,
			24, 7, 6, 14, 7, 7, 28, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 7, 23, 7,
			4, 3, 7, 7, 27, 7, 4, 5, 7, 8,
			40, 7, 6, 16, 9, 9, 832, 7, 4, 6,
			7, 7, 19, 7, 5, 8, 7, 8, 56, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 8, 46, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 8, 54, 7,
			5, 9, 9, 8, 512, 7, 4, 6, 7, 8,
			36, 9, 5, 128, 7, 8, 52, 7, 6, 15,
			7, 8, 0, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 6, 13, 7, 4, 3, 9, 9,
			1600, 7, 4, 5, 7, 8, 44, 7, 6, 17,
			9, 9, 1344, 7, 4, 6, 7, 6, 1, 7,
			5, 8, 9, 6, 192, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 8, 30, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 6, 12, 7, 5, 9, 9, 6,
			1664, 7, 4, 6, 7, 8, 34, 9, 5, 128,
			7, 8, 50, 7, 6, 14, 7, 8, 62, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 8,
			48, 7, 4, 3, 7, 8, 60, 7, 4, 5,
			7, 8, 42, 7, 6, 16, 9, 9, 1088, 7,
			4, 6, 7, 8, 32, 7, 5, 8, 7, 8,
			58, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 7, 22, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 7,
			26, 7, 5, 9, 9, 8, 640, 7, 4, 6,
			7, 8, 38, 9, 5, 128, 7, 7, 25, 7,
			6, 15, 9, 8, 384, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 6, 13, 7, 4, 3,
			7, 7, 18, 7, 4, 5, 7, 7, 21, 7,
			6, 17, 9, 7, 256, 7, 4, 6, 7, 6,
			1, 7, 5, 8, 9, 6, 192, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 0, 0, 0, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 6, 12, 7, 5, 9,
			9, 6, 1664, 7, 4, 6, 7, 7, 20, 9,
			5, 128, 7, 7, 24, 7, 6, 14, 7, 7,
			28, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 7, 23, 7, 4, 3, 7, 7, 27, 7,
			4, 5, 7, 8, 39, 7, 6, 16, 9, 8,
			576, 7, 4, 6, 7, 7, 19, 7, 5, 8,
			7, 8, 55, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 8,
			45, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 8, 53, 7, 5, 9, 9, 8, 448, 7,
			4, 6, 7, 8, 35, 9, 5, 128, 7, 8,
			51, 7, 6, 15, 7, 8, 63, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 6, 13, 7,
			4, 3, 9, 9, 1536, 7, 4, 5, 7, 8,
			43, 7, 6, 17, 9, 9, 1280, 7, 4, 6,
			7, 6, 1, 7, 5, 8, 9, 6, 192, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 8, 29, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 6, 12, 7,
			5, 9, 9, 6, 1664, 7, 4, 6, 7, 8,
			33, 9, 5, 128, 7, 8, 49, 7, 6, 14,
			7, 8, 61, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 8, 47, 7, 4, 3, 7, 8,
			59, 7, 4, 5, 7, 8, 41, 7, 6, 16,
			9, 9, 1024, 7, 4, 6, 7, 8, 31, 7,
			5, 8, 7, 8, 57, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 7, 22, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 7, 26, 7, 5, 9, 9, 9,
			768, 7, 4, 6, 7, 8, 37, 9, 5, 128,
			7, 7, 25, 7, 6, 15, 9, 8, 320, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 6,
			13, 7, 4, 3, 7, 7, 18, 7, 4, 5,
			7, 7, 21, 7, 6, 17, 9, 7, 256, 7,
			4, 6, 7, 6, 1, 7, 5, 8, 9, 6,
			192, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 11, 11, 1856, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 6,
			12, 7, 5, 9, 9, 6, 1664, 7, 4, 6,
			7, 7, 20, 9, 5, 128, 7, 7, 24, 7,
			6, 14, 7, 7, 28, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 7, 23, 7, 4, 3,
			7, 7, 27, 7, 4, 5, 7, 8, 40, 7,
			6, 16, 9, 9, 896, 7, 4, 6, 7, 7,
			19, 7, 5, 8, 7, 8, 56, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 8, 46, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 8, 54, 7, 5, 9,
			9, 8, 512, 7, 4, 6, 7, 8, 36, 9,
			5, 128, 7, 8, 52, 7, 6, 15, 7, 8,
			0, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 6, 13, 7, 4, 3, 9, 9, 1728, 7,
			4, 5, 7, 8, 44, 7, 6, 17, 9, 9,
			1408, 7, 4, 6, 7, 6, 1, 7, 5, 8,
			9, 6, 192, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 8,
			30, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 6, 12, 7, 5, 9, 9, 6, 1664, 7,
			4, 6, 7, 8, 34, 9, 5, 128, 7, 8,
			50, 7, 6, 14, 7, 8, 62, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 8, 48, 7,
			4, 3, 7, 8, 60, 7, 4, 5, 7, 8,
			42, 7, 6, 16, 9, 9, 1152, 7, 4, 6,
			7, 8, 32, 7, 5, 8, 7, 8, 58, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 7, 22, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 7, 26, 7,
			5, 9, 9, 8, 640, 7, 4, 6, 7, 8,
			38, 9, 5, 128, 7, 7, 25, 7, 6, 15,
			9, 8, 384, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 6, 13, 7, 4, 3, 7, 7,
			18, 7, 4, 5, 7, 7, 21, 7, 6, 17,
			9, 7, 256, 7, 4, 6, 7, 6, 1, 7,
			5, 8, 9, 6, 192, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			0, 0, 0, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 6, 12, 7, 5, 9, 9, 6,
			1664, 7, 4, 6, 7, 7, 20, 9, 5, 128,
			7, 7, 24, 7, 6, 14, 7, 7, 28, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 7,
			23, 7, 4, 3, 7, 7, 27, 7, 4, 5,
			7, 8, 39, 7, 6, 16, 9, 8, 576, 7,
			4, 6, 7, 7, 19, 7, 5, 8, 7, 8,
			55, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 8, 45, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 8,
			53, 7, 5, 9, 9, 8, 448, 7, 4, 6,
			7, 8, 35, 9, 5, 128, 7, 8, 51, 7,
			6, 15, 7, 8, 63, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 6, 13, 7, 4, 3,
			9, 9, 1472, 7, 4, 5, 7, 8, 43, 7,
			6, 17, 9, 9, 1216, 7, 4, 6, 7, 6,
			1, 7, 5, 8, 9, 6, 192, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 8, 29, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 6, 12, 7, 5, 9,
			9, 6, 1664, 7, 4, 6, 7, 8, 33, 9,
			5, 128, 7, 8, 49, 7, 6, 14, 7, 8,
			61, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 8, 47, 7, 4, 3, 7, 8, 59, 7,
			4, 5, 7, 8, 41, 7, 6, 16, 9, 9,
			960, 7, 4, 6, 7, 8, 31, 7, 5, 8,
			7, 8, 57, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 7,
			22, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 7, 26, 7, 5, 9, 9, 9, 704, 7,
			4, 6, 7, 8, 37, 9, 5, 128, 7, 7,
			25, 7, 6, 15, 9, 8, 320, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 6, 13, 7,
			4, 3, 7, 7, 18, 7, 4, 5, 7, 7,
			21, 7, 6, 17, 9, 7, 256, 7, 4, 6,
			7, 6, 1, 7, 5, 8, 9, 6, 192, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 11, 12, 2176, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 6, 12, 7,
			5, 9, 9, 6, 1664, 7, 4, 6, 7, 7,
			20, 9, 5, 128, 7, 7, 24, 7, 6, 14,
			7, 7, 28, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 7, 23, 7, 4, 3, 7, 7,
			27, 7, 4, 5, 7, 8, 40, 7, 6, 16,
			9, 9, 832, 7, 4, 6, 7, 7, 19, 7,
			5, 8, 7, 8, 56, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 8, 46, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 8, 54, 7, 5, 9, 9, 8,
			512, 7, 4, 6, 7, 8, 36, 9, 5, 128,
			7, 8, 52, 7, 6, 15, 7, 8, 0, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 6,
			13, 7, 4, 3, 9, 9, 1600, 7, 4, 5,
			7, 8, 44, 7, 6, 17, 9, 9, 1344, 7,
			4, 6, 7, 6, 1, 7, 5, 8, 9, 6,
			192, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 8, 30, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 6,
			12, 7, 5, 9, 9, 6, 1664, 7, 4, 6,
			7, 8, 34, 9, 5, 128, 7, 8, 50, 7,
			6, 14, 7, 8, 62, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 8, 48, 7, 4, 3,
			7, 8, 60, 7, 4, 5, 7, 8, 42, 7,
			6, 16, 9, 9, 1088, 7, 4, 6, 7, 8,
			32, 7, 5, 8, 7, 8, 58, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 7, 22, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 7, 26, 7, 5, 9,
			9, 8, 640, 7, 4, 6, 7, 8, 38, 9,
			5, 128, 7, 7, 25, 7, 6, 15, 9, 8,
			384, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 6, 13, 7, 4, 3, 7, 7, 18, 7,
			4, 5, 7, 7, 21, 7, 6, 17, 9, 7,
			256, 7, 4, 6, 7, 6, 1, 7, 5, 8,
			9, 6, 192, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 0, 0,
			0, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 6, 12, 7, 5, 9, 9, 6, 1664, 7,
			4, 6, 7, 7, 20, 9, 5, 128, 7, 7,
			24, 7, 6, 14, 7, 7, 28, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 7, 23, 7,
			4, 3, 7, 7, 27, 7, 4, 5, 7, 8,
			39, 7, 6, 16, 9, 8, 576, 7, 4, 6,
			7, 7, 19, 7, 5, 8, 7, 8, 55, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 8, 45, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 8, 53, 7,
			5, 9, 9, 8, 448, 7, 4, 6, 7, 8,
			35, 9, 5, 128, 7, 8, 51, 7, 6, 15,
			7, 8, 63, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 6, 13, 7, 4, 3, 9, 9,
			1536, 7, 4, 5, 7, 8, 43, 7, 6, 17,
			9, 9, 1280, 7, 4, 6, 7, 6, 1, 7,
			5, 8, 9, 6, 192, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 8, 29, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 6, 12, 7, 5, 9, 9, 6,
			1664, 7, 4, 6, 7, 8, 33, 9, 5, 128,
			7, 8, 49, 7, 6, 14, 7, 8, 61, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 8,
			47, 7, 4, 3, 7, 8, 59, 7, 4, 5,
			7, 8, 41, 7, 6, 16, 9, 9, 1024, 7,
			4, 6, 7, 8, 31, 7, 5, 8, 7, 8,
			57, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 7, 22, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 7,
			26, 7, 5, 9, 9, 9, 768, 7, 4, 6,
			7, 8, 37, 9, 5, 128, 7, 7, 25, 7,
			6, 15, 9, 8, 320, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 6, 13, 7, 4, 3,
			7, 7, 18, 7, 4, 5, 7, 7, 21, 7,
			6, 17, 9, 7, 256, 7, 4, 6, 7, 6,
			1, 7, 5, 8, 9, 6, 192, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 11, 12, 2432, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 6, 12, 7, 5, 9,
			9, 6, 1664, 7, 4, 6, 7, 7, 20, 9,
			5, 128, 7, 7, 24, 7, 6, 14, 7, 7,
			28, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 7, 23, 7, 4, 3, 7, 7, 27, 7,
			4, 5, 7, 8, 40, 7, 6, 16, 9, 9,
			896, 7, 4, 6, 7, 7, 19, 7, 5, 8,
			7, 8, 56, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 8,
			46, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 8, 54, 7, 5, 9, 9, 8, 512, 7,
			4, 6, 7, 8, 36, 9, 5, 128, 7, 8,
			52, 7, 6, 15, 7, 8, 0, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 6, 13, 7,
			4, 3, 9, 9, 1728, 7, 4, 5, 7, 8,
			44, 7, 6, 17, 9, 9, 1408, 7, 4, 6,
			7, 6, 1, 7, 5, 8, 9, 6, 192, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 8, 30, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 6, 12, 7,
			5, 9, 9, 6, 1664, 7, 4, 6, 7, 8,
			34, 9, 5, 128, 7, 8, 50, 7, 6, 14,
			7, 8, 62, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 8, 48, 7, 4, 3, 7, 8,
			60, 7, 4, 5, 7, 8, 42, 7, 6, 16,
			9, 9, 1152, 7, 4, 6, 7, 8, 32, 7,
			5, 8, 7, 8, 58, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 7, 22, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 7, 26, 7, 5, 9, 9, 8,
			640, 7, 4, 6, 7, 8, 38, 9, 5, 128,
			7, 7, 25, 7, 6, 15, 9, 8, 384, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 6,
			13, 7, 4, 3, 7, 7, 18, 7, 4, 5,
			7, 7, 21, 7, 6, 17, 9, 7, 256, 7,
			4, 6, 7, 6, 1, 7, 5, 8, 9, 6,
			192, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 0, 0, 0, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 6,
			12, 7, 5, 9, 9, 6, 1664, 7, 4, 6,
			7, 7, 20, 9, 5, 128, 7, 7, 24, 7,
			6, 14, 7, 7, 28, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 7, 23, 7, 4, 3,
			7, 7, 27, 7, 4, 5, 7, 8, 39, 7,
			6, 16, 9, 8, 576, 7, 4, 6, 7, 7,
			19, 7, 5, 8, 7, 8, 55, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 8, 45, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 8, 53, 7, 5, 9,
			9, 8, 448, 7, 4, 6, 7, 8, 35, 9,
			5, 128, 7, 8, 51, 7, 6, 15, 7, 8,
			63, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 6, 13, 7, 4, 3, 9, 9, 1472, 7,
			4, 5, 7, 8, 43, 7, 6, 17, 9, 9,
			1216, 7, 4, 6, 7, 6, 1, 7, 5, 8,
			9, 6, 192, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 8,
			29, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 6, 12, 7, 5, 9, 9, 6, 1664, 7,
			4, 6, 7, 8, 33, 9, 5, 128, 7, 8,
			49, 7, 6, 14, 7, 8, 61, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 8, 47, 7,
			4, 3, 7, 8, 59, 7, 4, 5, 7, 8,
			41, 7, 6, 16, 9, 9, 960, 7, 4, 6,
			7, 8, 31, 7, 5, 8, 7, 8, 57, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 7, 22, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 7, 26, 7,
			5, 9, 9, 9, 704, 7, 4, 6, 7, 8,
			37, 9, 5, 128, 7, 7, 25, 7, 6, 15,
			9, 8, 320, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 6, 13, 7, 4, 3, 7, 7,
			18, 7, 4, 5, 7, 7, 21, 7, 6, 17,
			9, 7, 256, 7, 4, 6, 7, 6, 1, 7,
			5, 8, 9, 6, 192, 9, 5, 64, 7, 5,
			10, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			11, 12, 2048, 7, 4, 3, 7, 5, 11, 7,
			4, 5, 7, 6, 12, 7, 5, 9, 9, 6,
			1664, 7, 4, 6, 7, 7, 20, 9, 5, 128,
			7, 7, 24, 7, 6, 14, 7, 7, 28, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 7,
			23, 7, 4, 3, 7, 7, 27, 7, 4, 5,
			7, 8, 40, 7, 6, 16, 9, 9, 832, 7,
			4, 6, 7, 7, 19, 7, 5, 8, 7, 8,
			56, 9, 5, 64, 7, 5, 10, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 8, 46, 7,
			4, 3, 7, 5, 11, 7, 4, 5, 7, 8,
			54, 7, 5, 9, 9, 8, 512, 7, 4, 6,
			7, 8, 36, 9, 5, 128, 7, 8, 52, 7,
			6, 15, 7, 8, 0, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 7, 6, 13, 7, 4, 3,
			9, 9, 1600, 7, 4, 5, 7, 8, 44, 7,
			6, 17, 9, 9, 1344, 7, 4, 6, 7, 6,
			1, 7, 5, 8, 9, 6, 192, 9, 5, 64,
			7, 5, 10, 7, 4, 4, 7, 4, 2, 7,
			4, 7, 7, 8, 30, 7, 4, 3, 7, 5,
			11, 7, 4, 5, 7, 6, 12, 7, 5, 9,
			9, 6, 1664, 7, 4, 6, 7, 8, 34, 9,
			5, 128, 7, 8, 50, 7, 6, 14, 7, 8,
			62, 7, 4, 4, 7, 4, 2, 7, 4, 7,
			7, 8, 48, 7, 4, 3, 7, 8, 60, 7,
			4, 5, 7, 8, 42, 7, 6, 16, 9, 9,
			1088, 7, 4, 6, 7, 8, 32, 7, 5, 8,
			7, 8, 58, 9, 5, 64, 7, 5, 10, 7,
			4, 4, 7, 4, 2, 7, 4, 7, 7, 7,
			22, 7, 4, 3, 7, 5, 11, 7, 4, 5,
			7, 7, 26, 7, 5, 9, 9, 8, 640, 7,
			4, 6, 7, 8, 38, 9, 5, 128, 7, 7,
			25, 7, 6, 15, 9, 8, 384, 7, 4, 4,
			7, 4, 2, 7, 4, 7, 7, 6, 13, 7,
			4, 3, 7, 7, 18, 7, 4, 5, 7, 7,
			21, 7, 6, 17, 9, 7, 256, 7, 4, 6,
			7, 6, 1, 7, 5, 8, 9, 6, 192, 9,
			5, 64, 7, 5, 10, 7, 4, 4, 7, 4,
			2, 7, 4, 7, 0, 0, 0, 7, 4, 3,
			7, 5, 11, 7, 4, 5, 7, 6, 12, //"Not showing all elements because this array is too big (12288 elements)"
		};

		static readonly int[] m_faxBlackTable = new int[]
		{
			12, 11, 0, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 9, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 7,
			10, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 8, 13, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 8, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 9, 15, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 10, 18, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 9, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 7, 11, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 10,
			17, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 8, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 7, 12, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 11, 11, 1792, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			9, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 7, 10, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 11, 23, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 8, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 11,
			20, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 11, 25, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 9, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 7, 11, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 8, 14, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 8, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 7, 12, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 0, 0,
			0, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 9, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 7, 10, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 8, 13, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			8, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 10, 12, 128, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 12, 56, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 9, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 7,
			11, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 12, 30, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 8, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 7, 12, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 11, 11, 1856, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 9, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 7, 10, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 12,
			57, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 8, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 11, 21, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 12, 54, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			9, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 7, 11, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 8, 14, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 8, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 7,
			12, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 0, 0, 0, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 9, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 7, 10, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 8, 13, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 8, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 9, 15, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 12,
			52, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 9, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 7, 11, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 12, 48, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			8, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 7, 12, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			11, 12, 2112, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 9, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 7,
			10, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 12, 44, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 8, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 12, 36, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 10, 12, 384, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 9, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 7, 11, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 8,
			14, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 8, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 7, 12, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 0, 0, 0, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			9, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 7, 10, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 8, 13, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 8, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 12,
			28, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 12, 60, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 9, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 7, 11, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 12, 40, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 8, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 7, 12, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 11, 12,
			2368, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 9, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 7, 10, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 10, 16, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			8, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 10, 0, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			10, 10, 64, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 9, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 7,
			11, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 8, 14, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 8, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 7, 12, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 0, 0, 0, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 9, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 7, 10, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 8,
			13, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 8, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 9, 15, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 10, 18, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			9, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 7, 11, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 10, 17, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 8, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 7,
			12, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 11, 12, 1984, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 9, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 7, 10, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 12, 50, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 8, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 12, 34, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 10, 13,
			1664, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 9, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 7, 11, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 8, 14, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			8, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 7, 12, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			0, 0, 0, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 9, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 7,
			10, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 8, 13, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 8, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 12, 26, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 10, 13, 1408, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 9, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 7, 11, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 12,
			32, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 8, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 7, 12, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 11, 11, 1920, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			9, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 7, 10, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 12, 61, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 8, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 12,
			42, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 10, 13, 1024, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 9, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 7, 11, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 8, 14, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 8, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 7, 12, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 0, 0,
			0, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 9, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 7, 10, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 8, 13, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			8, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 9, 15, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			10, 13, 768, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 9, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 7,
			11, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 12, 62, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 8, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 7, 12, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 11, 12, 2240, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 9, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 7, 10, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 12,
			46, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 8, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 12, 38, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 10, 13, 512, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			9, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 7, 11, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 8, 14, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 8, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 7,
			12, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 0, 0, 0, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 9, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 7, 10, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 8, 13, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 8, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 11, 19, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 11,
			24, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 9, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 7, 11, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 11, 22, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			8, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 7, 12, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			11, 12, 2496, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 9, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 7,
			10, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 10, 16, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 8, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 10, 0, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 10, 10, 64, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 9, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 7, 11, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 8,
			14, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 8, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 7, 12, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 12, 11, 0, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			9, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 7, 10, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 8, 13, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 8, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 9,
			15, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 10, 18, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 9, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 7, 11, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 10, 17, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 8, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 7, 12, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 11, 11,
			1792, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 9, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 7, 10, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 11, 23, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			8, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 11, 20, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 11, 25, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 9, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 7,
			11, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 8, 14, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 8, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 7, 12, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 0, 0, 0, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 9, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 7, 10, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 8,
			13, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 8, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 10, 12, 192, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 10, 13, 1280, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			9, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 7, 11, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 12, 31, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 8, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 7,
			12, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 11, 11, 1856, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 9, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 7, 10, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 12, 58, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 8, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 11, 21, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 10, 13,
			896, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 9, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 7, 11, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 8, 14, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			8, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 7, 12, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			0, 0, 0, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 9, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 7,
			10, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 8, 13, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 8, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 9, 15, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 10, 13, 640, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 9, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 7, 11, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 12,
			49, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 8, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 7, 12, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 11, 12, 2176, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			9, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 7, 10, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 12, 45, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 8, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 12,
			37, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 10, 12, 448, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 9, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 7, 11, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 8, 14, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 8, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 7, 12, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 0, 0,
			0, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 9, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 7, 10, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 8, 13, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			8, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 12, 29, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			10, 13, 1536, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 9, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 7,
			11, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 12, 41, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 8, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 7, 12, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 11, 12, 2432, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 9, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 7, 10, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 10,
			16, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 8, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 10, 0, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 10, 10, 64, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			9, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 7, 11, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 8, 14, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 8, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 7,
			12, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 0, 0, 0, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 9, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 7, 10, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 8, 13, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 8, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 9, 15, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 10,
			18, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 9, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 7, 11, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 10, 17, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 6,
			8, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 7, 12, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 5, 7, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			11, 12, 2048, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 6, 9, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 7,
			10, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 5, 7, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 12, 51, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 6, 8, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 12, 35, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 5,
			7, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 5, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 10, 12, 320, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 6, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 6, 9, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			5, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 7, 11, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 6, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 8, 5, 7, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 5, 8,
			2, 3, 8, 3, 4, 8, 2, 2, 8, 8,
			14, 8, 2, 3, 8, 3, 1, 8, 2, 2,
			8, 4, 6, 8, 2, 3, 8, 3, 4, 8,
			2, 2, 8, 6, 8, 8, 2, 3, 8, 3,
			1, 8, 2, 2, 8, 4, 5, 8, 2, 3,
			8, 3, 4, 8, 2, 2, 8, 7, 12, 8,
			2, 3, 8, 3, 1, 8, 2, 2, 8, 4,
			6, 8, 2, 3, 8, 3, 4, 8, 2, 2,
			8, 5, 7, 8, 2, 3, 8, 3, 1, 8,
			2, 2, 8, 4, 5, 8, 2, 3, 8, 3,
			4, 8, 2, 2, 0, 0, 0, 8, 2, 3,
			8, 3, 1, 8, 2, 2, 8, 4, 6,// "Not showing all elements because this array is too big (24576 elements)"
		};

		static readonly short[] m_faxWhiteCodes = new short[]
		{
			8, 53, 0, 6, 7, 1, 4, 7, 2, 4,
			8, 3, 4, 11, 4, 4, 12, 5, 4, 14,
			6, 4, 15, 7, 5, 19, 8, 5, 20, 9,
			5, 7, 10, 5, 8, 11, 6, 8, 12, 6,
			3, 13, 6, 52, 14, 6, 53, 15, 6, 42,
			16, 6, 43, 17, 7, 39, 18, 7, 12, 19,
			7, 8, 20, 7, 23, 21, 7, 3, 22, 7,
			4, 23, 7, 40, 24, 7, 43, 25, 7, 19,
			26, 7, 36, 27, 7, 24, 28, 8, 2, 29,
			8, 3, 30, 8, 26, 31, 8, 27, 32, 8,
			18, 33, 8, 19, 34, 8, 20, 35, 8, 21,
			36, 8, 22, 37, 8, 23, 38, 8, 40, 39,
			8, 41, 40, 8, 42, 41, 8, 43, 42, 8,
			44, 43, 8, 45, 44, 8, 4, 45, 8, 5,
			46, 8, 10, 47, 8, 11, 48, 8, 82, 49,
			8, 83, 50, 8, 84, 51, 8, 85, 52, 8,
			36, 53, 8, 37, 54, 8, 88, 55, 8, 89,
			56, 8, 90, 57, 8, 91, 58, 8, 74, 59,
			8, 75, 60, 8, 50, 61, 8, 51, 62, 8,
			52, 63, 5, 27, 64, 5, 18, 128, 6, 23,
			192, 7, 55, 256, 8, 54, 320, 8, 55, 384,
			8, 100, 448, 8, 101, 512, 8, 104, 576, 8,
			103, 640, 9, 204, 704, 9, 205, 768, 9, 210,
			832, 9, 211, 896, 9, 212, 960, 9, 213, 1024,
			9, 214, 1088, 9, 215, 1152, 9, 216, 1216, 9,
			217, 1280, 9, 218, 1344, 9, 219, 1408, 9, 152,
			1472, 9, 153, 1536, 9, 154, 1600, 6, 24, 1664,
			9, 155, 1728, 11, 8, 1792, 11, 12, 1856, 11,
			13, 1920, 12, 18, 1984, 12, 19, 2048, 12, 20,
			2112, 12, 21, 2176, 12, 22, 2240, 12, 23, 2304,
			12, 28, 2368, 12, 29, 2432, 12, 30, 2496, 12,
			31, 2560, 12, 1, -1, 9, 1, -2, 10, 1,
			-2, 11, 1, -2, 12, 0, -2
		};

		static readonly short[] m_faxBlackCodes = new short[]
		{
			10, 55, 0, 3, 2, 1, 2, 3, 2, 2,
			2, 3, 3, 3, 4, 4, 3, 5, 4, 2,
			6, 5, 3, 7, 6, 5, 8, 6, 4, 9,
			7, 4, 10, 7, 5, 11, 7, 7, 12, 8,
			4, 13, 8, 7, 14, 9, 24, 15, 10, 23,
			16, 10, 24, 17, 10, 8, 18, 11, 103, 19,
			11, 104, 20, 11, 108, 21, 11, 55, 22, 11,
			40, 23, 11, 23, 24, 11, 24, 25, 12, 202,
			26, 12, 203, 27, 12, 204, 28, 12, 205, 29,
			12, 104, 30, 12, 105, 31, 12, 106, 32, 12,
			107, 33, 12, 210, 34, 12, 211, 35, 12, 212,
			36, 12, 213, 37, 12, 214, 38, 12, 215, 39,
			12, 108, 40, 12, 109, 41, 12, 218, 42, 12,
			219, 43, 12, 84, 44, 12, 85, 45, 12, 86,
			46, 12, 87, 47, 12, 100, 48, 12, 101, 49,
			12, 82, 50, 12, 83, 51, 12, 36, 52, 12,
			55, 53, 12, 56, 54, 12, 39, 55, 12, 40,
			56, 12, 88, 57, 12, 89, 58, 12, 43, 59,
			12, 44, 60, 12, 90, 61, 12, 102, 62, 12,
			103, 63, 10, 15, 64, 12, 200, 128, 12, 201,
			192, 12, 91, 256, 12, 51, 320, 12, 52, 384,
			12, 53, 448, 13, 108, 512, 13, 109, 576, 13,
			74, 640, 13, 75, 704, 13, 76, 768, 13, 77,
			832, 13, 114, 896, 13, 115, 960, 13, 116, 1024,
			13, 117, 1088, 13, 118, 1152, 13, 119, 1216, 13,
			82, 1280, 13, 83, 1344, 13, 84, 1408, 13, 85,
			1472, 13, 90, 1536, 13, 91, 1600, 13, 100, 1664,
			13, 101, 1728, 11, 8, 1792, 11, 12, 1856, 11,
			13, 1920, 12, 18, 1984, 12, 19, 2048, 12, 20,
			2112, 12, 21, 2176, 12, 22, 2240, 12, 23, 2304,
			12, 28, 2368, 12, 29, 2432, 12, 30, 2496, 12,
			31, 2560, 12, 1, -1, 9, 1, -2, 10, 1,
			-2, 11, 1, -2, 12, 0, -2
		};

		static readonly CCITTCodec.tableEntry m_horizcode = new CCITTCodec.tableEntry(3, 1, 0);

		static readonly CCITTCodec.tableEntry m_passcode = new CCITTCodec.tableEntry(4, 1, 0);

		static readonly CCITTCodec.tableEntry[] m_vcodes = new CCITTCodec.tableEntry[]
		{
			new CCITTCodec.tableEntry(7, 3, 0),
			new CCITTCodec.tableEntry(6, 3, 0),
			new CCITTCodec.tableEntry(3, 3, 0),
			new CCITTCodec.tableEntry(1, 1, 0),
			new CCITTCodec.tableEntry(3, 2, 0),
			new CCITTCodec.tableEntry(6, 2, 0),
			new CCITTCodec.tableEntry(7, 2, 0)
		};

		static readonly int[] m_msbmask = new int[] { 0, 1, 3, 7, 15, 31, 63, 127, 255 };

		static readonly byte[] m_zeroruns = new byte[]
		{
			8, 7, 6, 6, 5, 5, 5, 5, 4, 4,
			4, 4, 4, 4, 4, 4, 3, 3, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			3, 3, 2, 2, 2, 2, 2, 2, 2, 2,
			2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
			2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
			2, 2, 2, 2, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0
		};

		static readonly byte[] m_oneruns = new byte[]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			1, 1, 2, 2, 2, 2, 2, 2, 2, 2,
			2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
			2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
			2, 2, 2, 2, 3, 3, 3, 3, 3, 3,
			3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
			4, 4, 4, 4, 4, 4, 4, 4, 5, 5,
			5, 5, 6, 6, 7, 8
		};

		static readonly byte[] fillMasks = new byte[] { 0, 128, 192, 224, 240, 248, 252, 254, byte.MaxValue };

		struct tableEntry
		{
			public tableEntry(short _length, short _code, short _runlen)
			{
				this.length = _length;
				this.code = _code;
				this.runlen = _runlen;
			}

			public static CCITTCodec.tableEntry FromArray(short[] array, int entryNumber)
			{
				int num = entryNumber * 3;
				return new CCITTCodec.tableEntry(array[num], array[num + 1], array[num + 2]);
			}

			public short length;

			public short code;

			public short runlen;
		}

		struct faxTableEntry
		{
			public faxTableEntry(byte _State, byte _Width, int _Param)
			{
				this.State = _State;
				this.Width = _Width;
				this.Param = _Param;
			}

			public static CCITTCodec.faxTableEntry FromArray(int[] array, int entryNumber)
			{
				int num = entryNumber * 3;
				return new CCITTCodec.faxTableEntry((byte)array[num], (byte)array[num + 1], array[num + 2]);
			}

			public byte State;

			public byte Width;

			public int Param;
		}

		enum Decoder
		{
			useFax3_1DDecoder,
			useFax3_2DDecoder,
			useFax4Decoder,
			useFax3RLEDecoder
		}

		enum Fax3Encoder
		{
			useFax1DEncoder,
			useFax2DEncoder
		}
	}
}
