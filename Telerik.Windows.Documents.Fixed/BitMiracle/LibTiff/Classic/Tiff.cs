using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using BitMiracle.LibTiff.Classic.Internal;

namespace BitMiracle.LibTiff.Classic
{
	class Tiff : IDisposable
	{
		static Tiff Open(string fileName, string mode, TiffErrorHandler errorHandler)
		{
			return Tiff.Open(fileName, mode, errorHandler, null);
		}

		static Tiff Open(string fileName, string mode, TiffErrorHandler errorHandler, Tiff.TiffExtendProc extender)
		{
			FileMode mode2;
			FileAccess fileAccess;
			Tiff.getMode(mode, "Open", out mode2, out fileAccess);
			FileStream fileStream = null;
			try
			{
				if (fileAccess == FileAccess.Read)
				{
					fileStream = File.Open(fileName, mode2, fileAccess, FileShare.Read);
				}
				else
				{
					fileStream = File.Open(fileName, mode2, fileAccess);
				}
			}
			catch (Exception ex)
			{
				Tiff.Error("Open", "Failed to open '{0}'. {1}", new object[] { fileName, ex.Message });
				return null;
			}
			Tiff tiff = Tiff.ClientOpen(fileName, mode, fileStream, new TiffStream(), errorHandler, extender);
			if (tiff == null)
			{
				fileStream.Dispose();
			}
			else
			{
				tiff.m_fileStream = fileStream;
			}
			return tiff;
		}

		static Tiff ClientOpen(string name, string mode, object clientData, TiffStream stream, TiffErrorHandler errorHandler)
		{
			return Tiff.ClientOpen(name, mode, clientData, stream, errorHandler, null);
		}

		static Tiff ClientOpen(string name, string mode, object clientData, TiffStream stream, TiffErrorHandler errorHandler, Tiff.TiffExtendProc extender)
		{
			if (mode == null || mode.Length == 0)
			{
				Tiff.ErrorExt(null, clientData, "ClientOpen", "{0}: mode string should contain at least one char", new object[] { name });
				return null;
			}
			FileMode fileMode;
			FileAccess fileAccess;
			int mode2 = Tiff.getMode(mode, "ClientOpen", out fileMode, out fileAccess);
			Tiff tiff = new Tiff();
			tiff.m_name = name;
			tiff.m_mode = mode2 & -769;
			tiff.m_curdir = -1;
			tiff.m_curoff = 0U;
			tiff.m_curstrip = -1;
			tiff.m_row = -1;
			tiff.m_clientdata = clientData;
			if (stream == null)
			{
				Tiff.ErrorExt(tiff, clientData, "ClientOpen", "TiffStream is null pointer.", new object[0]);
				return null;
			}
			tiff.m_stream = stream;
			tiff.m_currentCodec = tiff.m_builtInCodecs[0];
			tiff.m_flags = TiffFlags.MSB2LSB;
			if (mode2 == 0 || mode2 == 2)
			{
				tiff.m_flags |= TiffFlags.STRIPCHOP;
			}
			int length = mode.Length;
			for (int i = 0; i < length; i++)
			{
				char c = mode[i];
				if (c <= 'L')
				{
					switch (c)
					{
					case 'B':
						tiff.m_flags = (tiff.m_flags & ~(TiffFlags.MSB2LSB | TiffFlags.LSB2MSB)) | TiffFlags.MSB2LSB;
						break;
					case 'C':
						if (mode2 == 0)
						{
							tiff.m_flags |= TiffFlags.STRIPCHOP;
						}
						break;
					default:
						if (c != 'H')
						{
							if (c == 'L')
							{
								tiff.m_flags = (tiff.m_flags & ~(TiffFlags.MSB2LSB | TiffFlags.LSB2MSB)) | TiffFlags.LSB2MSB;
							}
						}
						else
						{
							tiff.m_flags = (tiff.m_flags & ~(TiffFlags.MSB2LSB | TiffFlags.LSB2MSB)) | TiffFlags.LSB2MSB;
						}
						break;
					}
				}
				else
				{
					switch (c)
					{
					case 'b':
						if ((mode2 & 256) != 0)
						{
							tiff.m_flags |= TiffFlags.SWAB;
						}
						break;
					case 'c':
						if (mode2 == 0)
						{
							tiff.m_flags &= ~TiffFlags.STRIPCHOP;
						}
						break;
					default:
						if (c != 'h')
						{
							if (c != 'l')
							{
							}
						}
						else
						{
							tiff.m_flags |= TiffFlags.HEADERONLY;
						}
						break;
					}
				}
			}
			if ((tiff.m_mode & 512) != 0 || !tiff.readHeaderOk(ref tiff.m_header))
			{
				if (tiff.m_mode == 0)
				{
					Tiff.ErrorExt(tiff, tiff.m_clientdata, name, "Cannot read TIFF header", new object[0]);
					return null;
				}
				if ((tiff.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
				{
					tiff.m_header.tiff_magic = 19789;
				}
				else
				{
					tiff.m_header.tiff_magic = 18761;
				}
				tiff.m_header.tiff_version = 42;
				if ((tiff.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
				{
					Tiff.SwabShort(ref tiff.m_header.tiff_version);
				}
				tiff.m_header.tiff_diroff = 0U;
				tiff.seekFile(0L, SeekOrigin.Begin);
				if (!tiff.writeHeaderOK(tiff.m_header))
				{
					Tiff.ErrorExt(tiff, tiff.m_clientdata, name, "Error writing TIFF header", new object[0]);
					tiff.m_mode = 0;
					return null;
				}
				tiff.initOrder((int)tiff.m_header.tiff_magic);
				tiff.setupDefaultDirectory();
				tiff.m_diroff = 0U;
				tiff.m_dirlist = null;
				tiff.m_dirlistsize = 0;
				tiff.m_dirnumber = 0;
				return tiff;
			}
			else
			{
				if (tiff.m_header.tiff_magic != 19789 && tiff.m_header.tiff_magic != 18761 && tiff.m_header.tiff_magic != 20549)
				{
					Tiff.ErrorExt(tiff, tiff.m_clientdata, name, "Not a TIFF or MDI file, bad magic number {0} (0x{1:x})", new object[]
					{
						tiff.m_header.tiff_magic,
						tiff.m_header.tiff_magic
					});
					tiff.m_mode = 0;
					return null;
				}
				tiff.initOrder((int)tiff.m_header.tiff_magic);
				if ((tiff.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
				{
					Tiff.SwabShort(ref tiff.m_header.tiff_version);
					Tiff.SwabUInt(ref tiff.m_header.tiff_diroff);
				}
				if (tiff.m_header.tiff_version == 43)
				{
					Tiff.ErrorExt(tiff, tiff.m_clientdata, name, "This is a BigTIFF file. This format not supported\nby this version of LibTiff.Net.", new object[0]);
					tiff.m_mode = 0;
					return null;
				}
				if (tiff.m_header.tiff_version != 42)
				{
					Tiff.ErrorExt(tiff, tiff.m_clientdata, name, "Not a TIFF file, bad version number {0} (0x{1:x})", new object[]
					{
						tiff.m_header.tiff_version,
						tiff.m_header.tiff_version
					});
					tiff.m_mode = 0;
					return null;
				}
				tiff.m_flags |= TiffFlags.MYBUFFER;
				tiff.m_rawcp = 0;
				tiff.m_rawdata = null;
				tiff.m_rawdatasize = 0;
				if ((tiff.m_flags & TiffFlags.HEADERONLY) == TiffFlags.HEADERONLY)
				{
					return tiff;
				}
				char c2 = mode[0];
				if (c2 != 'a')
				{
					if (c2 == 'r')
					{
						tiff.m_nextdiroff = tiff.m_header.tiff_diroff;
						if (tiff.ReadDirectory())
						{
							tiff.m_rawcc = -1;
							tiff.m_flags |= TiffFlags.BUFFERSETUP;
							return tiff;
						}
					}
					tiff.m_mode = 0;
					return null;
				}
				tiff.setupDefaultDirectory();
				return tiff;
			}
		}

		static TiffErrorHandler setErrorHandlerImpl(TiffErrorHandler errorHandler)
		{
			TiffErrorHandler errorHandler2 = Tiff.m_errorHandler;
			Tiff.m_errorHandler = errorHandler;
			return errorHandler2;
		}

		static Tiff.TiffExtendProc setTagExtenderImpl(Tiff.TiffExtendProc extender)
		{
			Tiff.TiffExtendProc extender2 = Tiff.m_extender;
			Tiff.m_extender = extender;
			return extender2;
		}

		static TiffErrorHandler getErrorHandler(Tiff tif)
		{
			return Tiff.m_errorHandler;
		}

		static bool defaultTransferFunction(TiffDirectory td)
		{
			short[][] td_transferfunction = td.td_transferfunction;
			td_transferfunction[0] = null;
			td_transferfunction[1] = null;
			td_transferfunction[2] = null;
			if (td.td_bitspersample >= 30)
			{
				return false;
			}
			int num = 1 << (int)td.td_bitspersample;
			td_transferfunction[0] = new short[num];
			td_transferfunction[0][0] = 0;
			for (int i = 1; i < num; i++)
			{
				double x = (double)i / ((double)num - 1.0);
				td_transferfunction[0][i] = (short)Math.Floor(65535.0 * Math.Pow(x, 2.2) + 0.5);
			}
			if (td.td_samplesperpixel - td.td_extrasamples > 1)
			{
				td_transferfunction[1] = new short[num];
				Buffer.BlockCopy(td_transferfunction[0], 0, td_transferfunction[1], 0, td_transferfunction[0].Length * 2);
				td_transferfunction[2] = new short[num];
				Buffer.BlockCopy(td_transferfunction[0], 0, td_transferfunction[2], 0, td_transferfunction[0].Length * 2);
			}
			return true;
		}

		static void defaultRefBlackWhite(TiffDirectory td)
		{
			td.td_refblackwhite = new float[6];
			if (td.td_photometric == Photometric.YCBCR)
			{
				td.td_refblackwhite[0] = 0f;
				td.td_refblackwhite[1] = (td.td_refblackwhite[3] = (td.td_refblackwhite[5] = 255f));
				td.td_refblackwhite[2] = (td.td_refblackwhite[4] = 128f);
				return;
			}
			for (int i = 0; i < 3; i++)
			{
				td.td_refblackwhite[2 * i] = 0f;
				td.td_refblackwhite[2 * i + 1] = (float)((1L << (int)td.td_bitspersample) - 1L);
			}
		}

		internal static int readInt(byte[] buffer, int offset)
		{
			int num = (int)(buffer[offset++] & byte.MaxValue);
			num += (int)(buffer[offset++] & byte.MaxValue) << 8;
			num += (int)(buffer[offset++] & byte.MaxValue) << 16;
			return num + ((int)buffer[offset++] << 24);
		}

		internal static void writeInt(int value, byte[] buffer, int offset)
		{
			buffer[offset++] = (byte)value;
			buffer[offset++] = (byte)(value >> 8);
			buffer[offset++] = (byte)(value >> 16);
			buffer[offset++] = (byte)(value >> 24);
		}

		internal static short readShort(byte[] buffer, int offset)
		{
			short num = (short)(buffer[offset] & byte.MaxValue);
			return num + (short)((buffer[offset + 1] & byte.MaxValue) << 8);
		}

		internal static void fprintf(Stream fd, string format, params object[] list)
		{
			string s = string.Format(CultureInfo.InvariantCulture, format, list);
			byte[] bytes = Tiff.Latin1Encoding.GetBytes(s);
			fd.Write(bytes, 0, bytes.Length);
		}

		static string encodeOctalString(byte value)
		{
			return string.Format(CultureInfo.InvariantCulture, "\\{0}{1}{2}", new object[]
			{
				(value >> 6) & 7,
				(value >> 3) & 7,
				(int)(value & 7)
			});
		}

		void setupBuiltInCodecs()
		{
			TiffCodec[] array = new TiffCodec[3];
			array[0] = new CCITTCodec(this, Compression.CCITTFAX3, "CCITT Group 3");
			array[1] = new CCITTCodec(this, Compression.CCITTFAX4, "CCITT Group 4");
			this.m_builtInCodecs = array;
		}

		internal static bool isPseudoTag(TiffTag t)
		{
			return t > TiffTag.DCSHUESHIFTVALUES;
		}

		bool isFillOrder(FillOrder o)
		{
			return (this.m_flags & (TiffFlags)o) == (TiffFlags)o;
		}

		static int BITn(int n)
		{
			return 1 << n;
		}

		bool okToChangeTag(TiffTag tag)
		{
			TiffFieldInfo tiffFieldInfo = this.FindFieldInfo(tag, TiffType.NOTYPE);
			if (tiffFieldInfo == null)
			{
				Tiff.ErrorExt(this, this.m_clientdata, "SetField", "{0}: Unknown {1}tag {2}", new object[]
				{
					this.m_name,
					Tiff.isPseudoTag(tag) ? "pseudo-" : "",
					tag
				});
				return false;
			}
			if (tag != TiffTag.IMAGELENGTH && (this.m_flags & TiffFlags.BEENWRITING) == TiffFlags.BEENWRITING && !tiffFieldInfo.OkToChange)
			{
				Tiff.ErrorExt(this, this.m_clientdata, "SetField", "{0}: Cannot modify tag \"{1}\" while writing", new object[] { this.m_name, tiffFieldInfo.Name });
				return false;
			}
			return true;
		}

		void setupDefaultDirectory()
		{
			int n;
			TiffFieldInfo[] fieldInfo = Tiff.getFieldInfo(out n);
			this.setupFieldInfo(fieldInfo, n);
			this.m_dir = new TiffDirectory();
			this.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmNone;
			this.m_foundfield = null;
			this.m_tagmethods = this.m_defaultTagMethods;
			if (Tiff.m_extender != null)
			{
				Tiff.m_extender(this);
			}
			this.SetField(TiffTag.COMPRESSION, new object[] { Compression.NONE });
			this.m_flags &= ~TiffFlags.DIRTYDIRECT;
			this.m_flags &= ~TiffFlags.ISTILED;
			this.m_tilesize = -1;
			this.m_scanlinesize = -1;
		}

		bool advanceDirectory(ref uint nextdir, out long off)
		{
			off = 0L;
			short num;
			if (!this.seekOK((long)((ulong)nextdir)) || !this.readShortOK(out num))
			{
				Tiff.ErrorExt(this, this.m_clientdata, "advanceDirectory", "{0}: Error fetching directory count", new object[] { this.m_name });
				return false;
			}
			if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
			{
				Tiff.SwabShort(ref num);
			}
			off = this.seekFile((long)(num * 12), SeekOrigin.Current);
			if (!this.readUIntOK(out nextdir))
			{
				Tiff.ErrorExt(this, this.m_clientdata, "advanceDirectory", "{0}: Error fetching directory link", new object[] { this.m_name });
				return false;
			}
			if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
			{
				Tiff.SwabUInt(ref nextdir);
			}
			return true;
		}

		internal static void setString(out string cpp, string cp)
		{
			cpp = cp;
		}

		internal static void setShortArray(out short[] wpp, short[] wp, int n)
		{
			wpp = new short[n];
			for (int i = 0; i < n; i++)
			{
				wpp[i] = wp[i];
			}
		}

		internal static void setLongArray(out int[] lpp, int[] lp, int n)
		{
			lpp = new int[n];
			for (int i = 0; i < n; i++)
			{
				lpp[i] = lp[i];
			}
		}

		internal static void setFloatArray(out float[] fpp, float[] fp, int n)
		{
			fpp = new float[n];
			for (int i = 0; i < n; i++)
			{
				fpp[i] = fp[i];
			}
		}

		internal bool fieldSet(int field)
		{
			return (this.m_dir.td_fieldsset[field / 32] & Tiff.BITn(field)) != 0;
		}

		internal void setFieldBit(int field)
		{
			this.m_dir.td_fieldsset[field / 32] |= Tiff.BITn(field);
		}

		internal void clearFieldBit(int field)
		{
			this.m_dir.td_fieldsset[field / 32] &= ~Tiff.BITn(field);
		}

		static TiffFieldInfo[] getFieldInfo(out int size)
		{
			size = Tiff.tiffFieldInfo.Length;
			return Tiff.tiffFieldInfo;
		}

		static TiffFieldInfo[] getExifFieldInfo(out int size)
		{
			size = Tiff.exifFieldInfo.Length;
			return Tiff.exifFieldInfo;
		}

		void setupFieldInfo(TiffFieldInfo[] info, int n)
		{
			this.m_nfields = 0;
			this.MergeFieldInfo(info, n);
		}

		TiffType sampleToTagType()
		{
			int num = Tiff.howMany8((int)this.m_dir.td_bitspersample);
			switch (this.m_dir.td_sampleformat)
			{
			case SampleFormat.UINT:
				if (num <= 1)
				{
					return TiffType.BYTE;
				}
				if (num > 2)
				{
					return TiffType.LONG;
				}
				return TiffType.SHORT;
			case SampleFormat.INT:
				if (num <= 1)
				{
					return TiffType.SBYTE;
				}
				if (num > 2)
				{
					return TiffType.SLONG;
				}
				return TiffType.SSHORT;
			case SampleFormat.IEEEFP:
				if (num != 4)
				{
					return TiffType.DOUBLE;
				}
				return TiffType.FLOAT;
			case SampleFormat.VOID:
				return TiffType.UNDEFINED;
			default:
				return TiffType.UNDEFINED;
			}
		}

		static TiffFieldInfo createAnonFieldInfo(TiffTag tag, TiffType field_type)
		{
			return new TiffFieldInfo(tag, -3, -3, field_type, 65, true, true, null)
			{
				Name = string.Format(CultureInfo.InvariantCulture, "Tag {0}", new object[] { tag })
			};
		}

		internal static int dataSize(TiffType type)
		{
			switch (type)
			{
			case TiffType.BYTE:
			case TiffType.ASCII:
			case TiffType.SBYTE:
			case TiffType.UNDEFINED:
				return 1;
			case TiffType.SHORT:
			case TiffType.SSHORT:
				return 2;
			case TiffType.LONG:
			case TiffType.RATIONAL:
			case TiffType.SLONG:
			case TiffType.SRATIONAL:
			case TiffType.FLOAT:
			case TiffType.IFD:
				return 4;
			case TiffType.DOUBLE:
				return 8;
			default:
				return 0;
			}
		}

		int extractData(TiffDirEntry dir)
		{
			int tdir_type = (int)dir.tdir_type;
			if (this.m_header.tiff_magic == 19789)
			{
				return (int)((dir.tdir_offset >> this.m_typeshift[tdir_type]) & this.m_typemask[tdir_type]);
			}
			return (int)(dir.tdir_offset & this.m_typemask[tdir_type]);
		}

		bool byteCountLooksBad(TiffDirectory td)
		{
			return (td.td_stripbytecount[0] == 0U && td.td_stripoffset[0] != 0U) || (td.td_compression == Compression.NONE && (ulong)td.td_stripbytecount[0] > (ulong)(this.getFileSize() - (long)((ulong)td.td_stripoffset[0]))) || (this.m_mode == 0 && td.td_compression == Compression.NONE && (ulong)td.td_stripbytecount[0] < (ulong)((long)(this.ScanlineSize() * td.td_imagelength)));
		}

		static int howMany8(int x)
		{
			if ((x & 7) == 0)
			{
				return x >> 3;
			}
			return (x >> 3) + 1;
		}

		bool estimateStripByteCounts(TiffDirEntry[] dir, short dircount)
		{
			this.m_dir.td_stripbytecount = new uint[this.m_dir.td_nstrips];
			if (this.m_dir.td_compression != Compression.NONE)
			{
				long num = (long)(10 + dircount * 12 + 4);
				long fileSize = this.getFileSize();
				for (short num2 = 0; num2 < dircount; num2 += 1)
				{
					int num3 = Tiff.DataWidth(dir[(int)num2].tdir_type);
					if (num3 == 0)
					{
						Tiff.ErrorExt(this, this.m_clientdata, "estimateStripByteCounts", "{0}: Cannot determine size of unknown tag type {1}", new object[]
						{
							this.m_name,
							dir[(int)num2].tdir_type
						});
						return false;
					}
					num3 *= dir[(int)num2].tdir_count;
					if (num3 > 4)
					{
						num += (long)num3;
					}
				}
				num = fileSize - num;
				if (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
				{
					num /= (long)this.m_dir.td_samplesperpixel;
				}
				int i;
				for (i = 0; i < this.m_dir.td_nstrips; i++)
				{
					this.m_dir.td_stripbytecount[i] = (uint)num;
				}
				i--;
				if ((ulong)(this.m_dir.td_stripoffset[i] + this.m_dir.td_stripbytecount[i]) > (ulong)fileSize)
				{
					this.m_dir.td_stripbytecount[i] = (uint)(fileSize - (long)((ulong)this.m_dir.td_stripoffset[i]));
				}
			}
			else if (this.IsTiled())
			{
				int num4 = this.TileSize();
				for (int j = 0; j < this.m_dir.td_nstrips; j++)
				{
					this.m_dir.td_stripbytecount[j] = (uint)num4;
				}
			}
			else
			{
				int num5 = this.ScanlineSize();
				int num6 = this.m_dir.td_imagelength / this.m_dir.td_stripsperimage;
				for (int k = 0; k < this.m_dir.td_nstrips; k++)
				{
					this.m_dir.td_stripbytecount[k] = (uint)(num5 * num6);
				}
			}
			this.setFieldBit(24);
			if (!this.fieldSet(17))
			{
				this.m_dir.td_rowsperstrip = this.m_dir.td_imagelength;
			}
			return true;
		}

		void missingRequired(string tagname)
		{
			Tiff.ErrorExt(this, this.m_clientdata, "missingRequired", "{0}: TIFF directory is missing required \"{1}\" field", new object[] { this.m_name, tagname });
		}

		int fetchFailed(TiffDirEntry dir)
		{
			Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Error fetching data for field \"{0}\"", new object[] { this.FieldWithTag(dir.tdir_tag).Name });
			return 0;
		}

		static int readDirectoryFind(TiffDirEntry[] dir, short dircount, TiffTag tagid)
		{
			for (short num = 0; num < dircount; num += 1)
			{
				if (dir[(int)num].tdir_tag == tagid)
				{
					return (int)num;
				}
			}
			return -1;
		}

		bool checkDirOffset(uint diroff)
		{
			if (diroff == 0U)
			{
				return false;
			}
			short num = 0;
			while (num < this.m_dirnumber && this.m_dirlist != null)
			{
				if (this.m_dirlist[(int)num] == diroff)
				{
					return false;
				}
				num += 1;
			}
			this.m_dirnumber += 1;
			if ((int)this.m_dirnumber > this.m_dirlistsize)
			{
				uint[] dirlist = Tiff.Realloc(this.m_dirlist, (int)(this.m_dirnumber - 1), (int)(2 * this.m_dirnumber));
				this.m_dirlistsize = (int)(2 * this.m_dirnumber);
				this.m_dirlist = dirlist;
			}
			this.m_dirlist[(int)(this.m_dirnumber - 1)] = diroff;
			return true;
		}

		short fetchDirectory(uint diroff, out TiffDirEntry[] pdir, out uint nextdiroff)
		{
			this.m_diroff = diroff;
			nextdiroff = 0U;
			pdir = null;
			if (!this.seekOK((long)((ulong)this.m_diroff)))
			{
				Tiff.ErrorExt(this, this.m_clientdata, "fetchDirectory", "{0}: Seek error accessing TIFF directory", new object[] { this.m_name });
				return 0;
			}
			short num;
			if (!this.readShortOK(out num))
			{
				Tiff.ErrorExt(this, this.m_clientdata, "fetchDirectory", "{0}: Can not read TIFF directory count", new object[] { this.m_name });
				return 0;
			}
			if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
			{
				Tiff.SwabShort(ref num);
			}
			TiffDirEntry[] array = new TiffDirEntry[(int)num];
			if (!this.readDirEntryOk(array, num))
			{
				Tiff.ErrorExt(this, this.m_clientdata, "fetchDirectory", "{0}: Can not read TIFF directory", new object[] { this.m_name });
				return 0;
			}
			int num2;
			this.readIntOK(out num2);
			nextdiroff = (uint)num2;
			if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
			{
				num2 = (int)nextdiroff;
				Tiff.SwabLong(ref num2);
				nextdiroff = (uint)num2;
			}
			pdir = array;
			return num;
		}

		bool fetchSubjectDistance(TiffDirEntry dir)
		{
			if (dir.tdir_count != 1 || dir.tdir_type != TiffType.RATIONAL)
			{
				Tiff.WarningExt(this, this.m_clientdata, this.m_name, "incorrect count or type for SubjectDistance, tag ignored", new object[0]);
				return false;
			}
			bool result = false;
			byte[] buffer = new byte[8];
			int num = this.fetchData(dir, buffer);
			if (num != 0)
			{
				int[] array = new int[]
				{
					Tiff.readInt(buffer, 0),
					Tiff.readInt(buffer, 4)
				};
				float num2;
				if (this.cvtRational(dir, array[0], array[1], out num2))
				{
					result = this.SetField(dir.tdir_tag, new object[] { (array[0] != -1) ? num2 : (-num2) });
				}
			}
			return result;
		}

		bool checkDirCount(TiffDirEntry dir, int count)
		{
			if (count > dir.tdir_count)
			{
				Tiff.WarningExt(this, this.m_clientdata, this.m_name, "incorrect count for field \"{0}\" ({1}, expecting {2}); tag ignored", new object[]
				{
					this.FieldWithTag(dir.tdir_tag).Name,
					dir.tdir_count,
					count
				});
				return false;
			}
			if (count < dir.tdir_count)
			{
				Tiff.WarningExt(this, this.m_clientdata, this.m_name, "incorrect count for field \"{0}\" ({1}, expecting {2}); tag trimmed", new object[]
				{
					this.FieldWithTag(dir.tdir_tag).Name,
					dir.tdir_count,
					count
				});
				return true;
			}
			return true;
		}

		int fetchData(TiffDirEntry dir, byte[] buffer)
		{
			int num = Tiff.DataWidth(dir.tdir_type);
			int num2 = dir.tdir_count * num;
			if (dir.tdir_count == 0 || num == 0 || num2 / num != dir.tdir_count)
			{
				this.fetchFailed(dir);
			}
			if (!this.seekOK((long)((ulong)dir.tdir_offset)))
			{
				this.fetchFailed(dir);
			}
			if (!this.readOK(buffer, num2))
			{
				this.fetchFailed(dir);
			}
			if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
			{
				switch (dir.tdir_type)
				{
				case TiffType.SHORT:
				case TiffType.SSHORT:
				{
					short[] array = Tiff.ByteArrayToShorts(buffer, 0, num2);
					Tiff.SwabArrayOfShort(array, dir.tdir_count);
					Tiff.ShortsToByteArray(array, 0, dir.tdir_count, buffer, 0);
					break;
				}
				case TiffType.LONG:
				case TiffType.SLONG:
				case TiffType.FLOAT:
				{
					int[] array2 = Tiff.ByteArrayToInts(buffer, 0, num2);
					Tiff.SwabArrayOfLong(array2, dir.tdir_count);
					Tiff.IntsToByteArray(array2, 0, dir.tdir_count, buffer, 0);
					break;
				}
				case TiffType.RATIONAL:
				case TiffType.SRATIONAL:
				{
					int[] array3 = Tiff.ByteArrayToInts(buffer, 0, num2);
					Tiff.SwabArrayOfLong(array3, 2 * dir.tdir_count);
					Tiff.IntsToByteArray(array3, 0, 2 * dir.tdir_count, buffer, 0);
					break;
				}
				case TiffType.DOUBLE:
					Tiff.swab64BitData(buffer, 0, num2);
					break;
				}
			}
			return num2;
		}

		int fetchString(TiffDirEntry dir, out string cp)
		{
			byte[] array;
			if (dir.tdir_count <= 4)
			{
				int tdir_offset = (int)dir.tdir_offset;
				if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
				{
					Tiff.SwabLong(ref tdir_offset);
				}
				array = new byte[4];
				Tiff.writeInt(tdir_offset, array, 0);
				cp = Tiff.Latin1Encoding.GetString(array, 0, dir.tdir_count);
				return 1;
			}
			array = new byte[dir.tdir_count];
			int result = this.fetchData(dir, array);
			cp = Tiff.Latin1Encoding.GetString(array, 0, dir.tdir_count);
			return result;
		}

		bool cvtRational(TiffDirEntry dir, int num, int denom, out float rv)
		{
			if (denom == 0)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "{0}: Rational with zero denominator (num = {1})", new object[]
				{
					this.FieldWithTag(dir.tdir_tag).Name,
					num
				});
				rv = float.NaN;
				return false;
			}
			rv = (float)num / (float)denom;
			return true;
		}

		float fetchRational(TiffDirEntry dir)
		{
			byte[] buffer = new byte[8];
			int num = this.fetchData(dir, buffer);
			if (num != 0)
			{
				int[] array = new int[]
				{
					Tiff.readInt(buffer, 0),
					Tiff.readInt(buffer, 4)
				};
				float result;
				bool flag = this.cvtRational(dir, array[0], array[1], out result);
				if (flag)
				{
					return result;
				}
			}
			return 1f;
		}

		float fetchFloat(TiffDirEntry dir)
		{
			int value = this.extractData(dir);
			return BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
		}

		bool fetchByteArray(TiffDirEntry dir, byte[] v)
		{
			if (dir.tdir_count <= 4)
			{
				int tdir_count = dir.tdir_count;
				if (this.m_header.tiff_magic == 19789)
				{
					if (tdir_count == 4)
					{
						v[3] = (byte)(dir.tdir_offset & 255U);
					}
					if (tdir_count >= 3)
					{
						v[2] = (byte)((dir.tdir_offset >> 8) & 255U);
					}
					if (tdir_count >= 2)
					{
						v[1] = (byte)((dir.tdir_offset >> 16) & 255U);
					}
					if (tdir_count >= 1)
					{
						v[0] = (byte)(dir.tdir_offset >> 24);
					}
				}
				else
				{
					if (tdir_count == 4)
					{
						v[3] = (byte)(dir.tdir_offset >> 24);
					}
					if (tdir_count >= 3)
					{
						v[2] = (byte)((dir.tdir_offset >> 16) & 255U);
					}
					if (tdir_count >= 2)
					{
						v[1] = (byte)((dir.tdir_offset >> 8) & 255U);
					}
					if (tdir_count >= 1)
					{
						v[0] = (byte)(dir.tdir_offset & 255U);
					}
				}
				return true;
			}
			return this.fetchData(dir, v) != 0;
		}

		bool fetchShortArray(TiffDirEntry dir, short[] v)
		{
			if (dir.tdir_count <= 2)
			{
				int tdir_count = dir.tdir_count;
				if (this.m_header.tiff_magic == 19789)
				{
					if (tdir_count == 2)
					{
						v[1] = (short)(dir.tdir_offset & 65535U);
					}
					if (tdir_count >= 1)
					{
						v[0] = (short)(dir.tdir_offset >> 16);
					}
				}
				else
				{
					if (tdir_count == 2)
					{
						v[1] = (short)(dir.tdir_offset >> 16);
					}
					if (tdir_count >= 1)
					{
						v[0] = (short)(dir.tdir_offset & 65535U);
					}
				}
				return true;
			}
			int num = dir.tdir_count * 2;
			byte[] array = new byte[num];
			int num2 = this.fetchData(dir, array);
			if (num2 != 0)
			{
				Buffer.BlockCopy(array, 0, v, 0, array.Length);
			}
			return num2 != 0;
		}

		bool fetchShortPair(TiffDirEntry dir)
		{
			if (dir.tdir_count > 2)
			{
				Tiff.WarningExt(this, this.m_clientdata, this.m_name, "unexpected count for field \"{0}\", {1}, expected 2; ignored", new object[]
				{
					this.FieldWithTag(dir.tdir_tag).Name,
					dir.tdir_count
				});
				return false;
			}
			TiffType tdir_type = dir.tdir_type;
			switch (tdir_type)
			{
			case TiffType.BYTE:
				break;
			case TiffType.ASCII:
				return false;
			case TiffType.SHORT:
				goto IL_C2;
			default:
				switch (tdir_type)
				{
				case TiffType.SBYTE:
					break;
				case TiffType.UNDEFINED:
					return false;
				case TiffType.SSHORT:
					goto IL_C2;
				default:
					return false;
				}
				break;
			}
			byte[] array = new byte[4];
			return this.fetchByteArray(dir, array) && this.SetField(dir.tdir_tag, new object[]
			{
				array[0],
				array[1]
			});
			IL_C2:
			short[] array2 = new short[2];
			return this.fetchShortArray(dir, array2) && this.SetField(dir.tdir_tag, new object[]
			{
				array2[0],
				array2[1]
			});
		}

		bool fetchLongArray(TiffDirEntry dir, int[] v)
		{
			if (dir.tdir_count == 1)
			{
				v[0] = (int)dir.tdir_offset;
				return true;
			}
			int num = dir.tdir_count * 4;
			byte[] array = new byte[num];
			int num2 = this.fetchData(dir, array);
			if (num2 != 0)
			{
				Buffer.BlockCopy(array, 0, v, 0, array.Length);
			}
			return num2 != 0;
		}

		bool fetchRationalArray(TiffDirEntry dir, float[] v)
		{
			bool flag = false;
			byte[] buffer = new byte[dir.tdir_count * Tiff.DataWidth(dir.tdir_type)];
			if (this.fetchData(dir, buffer) != 0)
			{
				int num = 0;
				int[] array = new int[2];
				for (int i = 0; i < dir.tdir_count; i++)
				{
					array[0] = Tiff.readInt(buffer, num);
					num += 4;
					array[1] = Tiff.readInt(buffer, num);
					num += 4;
					flag = this.cvtRational(dir, array[0], array[1], out v[i]);
					if (!flag)
					{
						break;
					}
				}
			}
			return flag;
		}

		bool fetchFloatArray(TiffDirEntry dir, float[] v)
		{
			if (dir.tdir_count == 1)
			{
				v[0] = BitConverter.ToSingle(BitConverter.GetBytes(dir.tdir_offset), 0);
				return true;
			}
			int num = Tiff.DataWidth(dir.tdir_type);
			int num2 = dir.tdir_count * num;
			byte[] array = new byte[num2];
			int num3 = this.fetchData(dir, array);
			if (num3 != 0)
			{
				int num4 = 0;
				for (int i = 0; i < num3 / 4; i++)
				{
					v[i] = BitConverter.ToSingle(array, num4);
					num4 += 4;
				}
			}
			return num3 != 0;
		}

		bool fetchDoubleArray(TiffDirEntry dir, double[] v)
		{
			int num = Tiff.DataWidth(dir.tdir_type);
			int num2 = dir.tdir_count * num;
			byte[] array = new byte[num2];
			int num3 = this.fetchData(dir, array);
			if (num3 != 0)
			{
				int num4 = 0;
				for (int i = 0; i < num3 / 8; i++)
				{
					v[i] = BitConverter.ToDouble(array, num4);
					num4 += 8;
				}
			}
			return num3 != 0;
		}

		bool fetchAnyArray(TiffDirEntry dir, double[] v)
		{
			switch (dir.tdir_type)
			{
			case TiffType.BYTE:
			case TiffType.SBYTE:
			{
				byte[] array = new byte[dir.tdir_count];
				bool flag = this.fetchByteArray(dir, array);
				if (flag)
				{
					for (int i = dir.tdir_count - 1; i >= 0; i--)
					{
						v[i] = (double)array[i];
					}
				}
				if (!flag)
				{
					return false;
				}
				return true;
			}
			case TiffType.SHORT:
			case TiffType.SSHORT:
			{
				short[] array2 = new short[dir.tdir_count];
				bool flag = this.fetchShortArray(dir, array2);
				if (flag)
				{
					for (int i = dir.tdir_count - 1; i >= 0; i--)
					{
						v[i] = (double)array2[i];
					}
				}
				if (!flag)
				{
					return false;
				}
				return true;
			}
			case TiffType.LONG:
			case TiffType.SLONG:
			{
				int[] array3 = new int[dir.tdir_count];
				bool flag = this.fetchLongArray(dir, array3);
				if (flag)
				{
					for (int i = dir.tdir_count - 1; i >= 0; i--)
					{
						v[i] = (double)array3[i];
					}
				}
				if (!flag)
				{
					return false;
				}
				return true;
			}
			case TiffType.RATIONAL:
			case TiffType.SRATIONAL:
			{
				float[] array4 = new float[dir.tdir_count];
				bool flag = this.fetchRationalArray(dir, array4);
				if (flag)
				{
					for (int i = dir.tdir_count - 1; i >= 0; i--)
					{
						v[i] = (double)array4[i];
					}
				}
				if (!flag)
				{
					return false;
				}
				return true;
			}
			case TiffType.FLOAT:
			{
				float[] array5 = new float[dir.tdir_count];
				bool flag = this.fetchFloatArray(dir, array5);
				if (flag)
				{
					for (int i = dir.tdir_count - 1; i >= 0; i--)
					{
						v[i] = (double)array5[i];
					}
				}
				if (!flag)
				{
					return false;
				}
				return true;
			}
			case TiffType.DOUBLE:
				return this.fetchDoubleArray(dir, v);
			}
			Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "cannot read TIFF_ANY type {0} for field \"{1}\"", new object[]
			{
				dir.tdir_type,
				this.FieldWithTag(dir.tdir_tag).Name
			});
			return false;
		}

		bool fetchNormalTag(TiffDirEntry dir)
		{
			bool flag = false;
			TiffFieldInfo tiffFieldInfo = this.FieldWithTag(dir.tdir_tag);
			if (dir.tdir_count > 1)
			{
				switch (dir.tdir_type)
				{
				case TiffType.BYTE:
				case TiffType.SBYTE:
				{
					byte[] array = new byte[dir.tdir_count];
					flag = this.fetchByteArray(dir, array);
					if (flag)
					{
						if (tiffFieldInfo.PassCount)
						{
							flag = this.SetField(dir.tdir_tag, new object[] { dir.tdir_count, array });
						}
						else
						{
							flag = this.SetField(dir.tdir_tag, new object[] { array });
						}
					}
					break;
				}
				case TiffType.ASCII:
				case TiffType.UNDEFINED:
				{
					string text;
					flag = this.fetchString(dir, out text) != 0;
					if (flag)
					{
						if (tiffFieldInfo.PassCount)
						{
							flag = this.SetField(dir.tdir_tag, new object[] { dir.tdir_count, text });
						}
						else
						{
							flag = this.SetField(dir.tdir_tag, new object[] { text });
						}
					}
					break;
				}
				case TiffType.SHORT:
				case TiffType.SSHORT:
				{
					short[] array2 = new short[dir.tdir_count];
					flag = this.fetchShortArray(dir, array2);
					if (flag)
					{
						if (tiffFieldInfo.PassCount)
						{
							flag = this.SetField(dir.tdir_tag, new object[] { dir.tdir_count, array2 });
						}
						else
						{
							flag = this.SetField(dir.tdir_tag, new object[] { array2 });
						}
					}
					break;
				}
				case TiffType.LONG:
				case TiffType.SLONG:
				{
					int[] array3 = new int[dir.tdir_count];
					flag = this.fetchLongArray(dir, array3);
					if (flag)
					{
						if (tiffFieldInfo.PassCount)
						{
							flag = this.SetField(dir.tdir_tag, new object[] { dir.tdir_count, array3 });
						}
						else
						{
							flag = this.SetField(dir.tdir_tag, new object[] { array3 });
						}
					}
					break;
				}
				case TiffType.RATIONAL:
				case TiffType.SRATIONAL:
				{
					float[] array4 = new float[dir.tdir_count];
					flag = this.fetchRationalArray(dir, array4);
					if (flag)
					{
						if (tiffFieldInfo.PassCount)
						{
							flag = this.SetField(dir.tdir_tag, new object[] { dir.tdir_count, array4 });
						}
						else
						{
							flag = this.SetField(dir.tdir_tag, new object[] { array4 });
						}
					}
					break;
				}
				case TiffType.FLOAT:
				{
					float[] array5 = new float[dir.tdir_count];
					flag = this.fetchFloatArray(dir, array5);
					if (flag)
					{
						if (tiffFieldInfo.PassCount)
						{
							flag = this.SetField(dir.tdir_tag, new object[] { dir.tdir_count, array5 });
						}
						else
						{
							flag = this.SetField(dir.tdir_tag, new object[] { array5 });
						}
					}
					break;
				}
				case TiffType.DOUBLE:
				{
					double[] array6 = new double[dir.tdir_count];
					flag = this.fetchDoubleArray(dir, array6);
					if (flag)
					{
						if (tiffFieldInfo.PassCount)
						{
							flag = this.SetField(dir.tdir_tag, new object[] { dir.tdir_count, array6 });
						}
						else
						{
							flag = this.SetField(dir.tdir_tag, new object[] { array6 });
						}
					}
					break;
				}
				}
			}
			else if (this.checkDirCount(dir, 1))
			{
				switch (dir.tdir_type)
				{
				case TiffType.BYTE:
				case TiffType.SHORT:
				case TiffType.SBYTE:
				case TiffType.SSHORT:
				{
					TiffType type = tiffFieldInfo.Type;
					if (type != TiffType.LONG && type != TiffType.SLONG)
					{
						short num = (short)this.extractData(dir);
						if (tiffFieldInfo.PassCount)
						{
							short[] array7 = new short[] { num };
							flag = this.SetField(dir.tdir_tag, new object[] { 1, array7 });
						}
						else
						{
							flag = this.SetField(dir.tdir_tag, new object[] { num });
						}
					}
					else
					{
						int num2 = this.extractData(dir);
						if (tiffFieldInfo.PassCount)
						{
							int[] array8 = new int[] { num2 };
							flag = this.SetField(dir.tdir_tag, new object[] { 1, array8 });
						}
						else
						{
							flag = this.SetField(dir.tdir_tag, new object[] { num2 });
						}
					}
					break;
				}
				case TiffType.ASCII:
				case TiffType.UNDEFINED:
				{
					string text2;
					flag = this.fetchString(dir, out text2) != 0;
					if (flag)
					{
						if (tiffFieldInfo.PassCount)
						{
							flag = this.SetField(dir.tdir_tag, new object[] { 1, text2 });
						}
						else
						{
							flag = this.SetField(dir.tdir_tag, new object[] { text2 });
						}
					}
					break;
				}
				case TiffType.LONG:
				case TiffType.SLONG:
				{
					int num2 = this.extractData(dir);
					if (tiffFieldInfo.PassCount)
					{
						int[] array9 = new int[] { num2 };
						flag = this.SetField(dir.tdir_tag, new object[] { 1, array9 });
					}
					else
					{
						flag = this.SetField(dir.tdir_tag, new object[] { num2 });
					}
					break;
				}
				case TiffType.RATIONAL:
				case TiffType.SRATIONAL:
				case TiffType.FLOAT:
				{
					float num3 = ((dir.tdir_type == TiffType.FLOAT) ? this.fetchFloat(dir) : this.fetchRational(dir));
					if (tiffFieldInfo.PassCount)
					{
						float[] array10 = new float[] { num3 };
						flag = this.SetField(dir.tdir_tag, new object[] { 1, array10 });
					}
					else
					{
						flag = this.SetField(dir.tdir_tag, new object[] { num3 });
					}
					break;
				}
				case TiffType.DOUBLE:
				{
					double[] array11 = new double[1];
					flag = this.fetchDoubleArray(dir, array11);
					if (flag)
					{
						if (tiffFieldInfo.PassCount)
						{
							flag = this.SetField(dir.tdir_tag, new object[] { 1, array11 });
						}
						else
						{
							flag = this.SetField(dir.tdir_tag, new object[] { array11[0] });
						}
					}
					break;
				}
				}
			}
			return flag;
		}

		bool fetchPerSampleShorts(TiffDirEntry dir, out short pl)
		{
			pl = 0;
			short td_samplesperpixel = this.m_dir.td_samplesperpixel;
			bool result = false;
			if (this.checkDirCount(dir, (int)td_samplesperpixel))
			{
				short[] array = new short[dir.tdir_count];
				if (this.fetchShortArray(dir, array))
				{
					int num = dir.tdir_count;
					if ((int)td_samplesperpixel < num)
					{
						num = (int)td_samplesperpixel;
					}
					bool flag = false;
					ushort num2 = 1;
					while ((int)num2 < num)
					{
						if (array[(int)num2] != array[0])
						{
							Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Cannot handle different per-sample values for field \"{0}\"", new object[] { this.FieldWithTag(dir.tdir_tag).Name });
							flag = true;
							break;
						}
						num2 += 1;
					}
					if (!flag)
					{
						pl = array[0];
						result = true;
					}
				}
			}
			return result;
		}

		bool fetchPerSampleLongs(TiffDirEntry dir, out int pl)
		{
			pl = 0;
			short td_samplesperpixel = this.m_dir.td_samplesperpixel;
			bool result = false;
			if (this.checkDirCount(dir, (int)td_samplesperpixel))
			{
				int[] array = new int[dir.tdir_count];
				if (this.fetchLongArray(dir, array))
				{
					int num = dir.tdir_count;
					if ((int)td_samplesperpixel < num)
					{
						num = (int)td_samplesperpixel;
					}
					bool flag = false;
					ushort num2 = 1;
					while ((int)num2 < num)
					{
						if (array[(int)num2] != array[0])
						{
							Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Cannot handle different per-sample values for field \"{0}\"", new object[] { this.FieldWithTag(dir.tdir_tag).Name });
							flag = true;
							break;
						}
						num2 += 1;
					}
					if (!flag)
					{
						pl = array[0];
						result = true;
					}
				}
			}
			return result;
		}

		bool fetchPerSampleAnys(TiffDirEntry dir, out double pl)
		{
			pl = 0.0;
			short td_samplesperpixel = this.m_dir.td_samplesperpixel;
			bool result = false;
			if (this.checkDirCount(dir, (int)td_samplesperpixel))
			{
				double[] array = new double[dir.tdir_count];
				if (this.fetchAnyArray(dir, array))
				{
					int num = dir.tdir_count;
					if ((int)td_samplesperpixel < num)
					{
						num = (int)td_samplesperpixel;
					}
					bool flag = false;
					ushort num2 = 1;
					while ((int)num2 < num)
					{
						if (array[(int)num2] != array[0])
						{
							Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Cannot handle different per-sample values for field \"{0}\"", new object[] { this.FieldWithTag(dir.tdir_tag).Name });
							flag = true;
							break;
						}
						num2 += 1;
					}
					if (!flag)
					{
						pl = array[0];
						result = true;
					}
				}
			}
			return result;
		}

		bool fetchStripThing(TiffDirEntry dir, int nstrips, ref int[] lpp)
		{
			this.checkDirCount(dir, nstrips);
			if (lpp == null)
			{
				lpp = new int[nstrips];
			}
			else
			{
				Array.Clear(lpp, 0, lpp.Length);
			}
			bool flag;
			if (dir.tdir_type == TiffType.SHORT)
			{
				short[] array = new short[dir.tdir_count];
				flag = this.fetchShortArray(dir, array);
				if (flag)
				{
					for (int i = 0; i < nstrips; i++)
					{
						if (i >= dir.tdir_count)
						{
							break;
						}
						lpp[i] = (int)array[i];
					}
				}
			}
			else if (nstrips != dir.tdir_count)
			{
				int[] array2 = new int[dir.tdir_count];
				flag = this.fetchLongArray(dir, array2);
				if (flag)
				{
					for (int j = 0; j < nstrips; j++)
					{
						if (j >= dir.tdir_count)
						{
							break;
						}
						lpp[j] = array2[j];
					}
				}
			}
			else
			{
				flag = this.fetchLongArray(dir, lpp);
			}
			return flag;
		}

		bool fetchStripThing(TiffDirEntry dir, int nstrips, ref uint[] lpp)
		{
			int[] array = null;
			if (lpp != null)
			{
				array = new int[lpp.Length];
			}
			bool flag = this.fetchStripThing(dir, nstrips, ref array);
			if (flag)
			{
				if (lpp == null)
				{
					lpp = new uint[array.Length];
				}
				Buffer.BlockCopy(array, 0, lpp, 0, array.Length * 4);
			}
			return flag;
		}

		bool fetchRefBlackWhite(TiffDirEntry dir)
		{
			if (dir.tdir_type == TiffType.RATIONAL)
			{
				bool flag = this.fetchNormalTag(dir);
				if (flag)
				{
					for (int i = 0; i < this.m_dir.td_refblackwhite.Length; i++)
					{
						if (this.m_dir.td_refblackwhite[i] > 1f)
						{
							return true;
						}
					}
				}
			}
			dir.tdir_type = TiffType.LONG;
			int[] array = new int[dir.tdir_count];
			bool flag2 = this.fetchLongArray(dir, array);
			dir.tdir_type = TiffType.RATIONAL;
			if (flag2)
			{
				float[] array2 = new float[dir.tdir_count];
				for (int j = 0; j < dir.tdir_count; j++)
				{
					array2[j] = (float)array[j];
				}
				flag2 = this.SetField(dir.tdir_tag, new object[] { array2 });
			}
			return flag2;
		}

		void chopUpSingleUncompressedStrip()
		{
			uint num = this.m_dir.td_stripbytecount[0];
			uint num2 = this.m_dir.td_stripoffset[0];
			int num3 = this.VTileSize(1);
			uint num4;
			int num5;
			if (num3 > 8192)
			{
				num4 = (uint)num3;
				num5 = 1;
			}
			else
			{
				if (num3 <= 0)
				{
					return;
				}
				num5 = 8192 / num3;
				num4 = (uint)(num3 * num5);
			}
			if (num5 >= this.m_dir.td_rowsperstrip)
			{
				return;
			}
			uint num6 = Tiff.howMany(num, num4);
			if (num6 == 0U)
			{
				return;
			}
			uint[] array = new uint[num6];
			uint[] array2 = new uint[num6];
			int num7 = 0;
			while ((long)num7 < (long)((ulong)num6))
			{
				if (num4 > num)
				{
					num4 = num;
				}
				array[num7] = num4;
				array2[num7] = num2;
				num2 += num4;
				num -= num4;
				num7++;
			}
			this.m_dir.td_nstrips = (int)num6;
			this.m_dir.td_stripsperimage = (int)num6;
			this.SetField(TiffTag.ROWSPERSTRIP, new object[] { num5 });
			this.m_dir.td_stripbytecount = array;
			this.m_dir.td_stripoffset = array2;
			this.m_dir.td_stripbytecountsorted = true;
		}

		internal static int roundUp(int x, int y)
		{
			return Tiff.howMany(x, y) * y;
		}

		internal static int howMany(int x, int y)
		{
			long num = ((long)x + ((long)y - 1L)) / (long)y;
			if (num > 2147483647L)
			{
				return 0;
			}
			return (int)num;
		}

		internal static uint howMany(uint x, uint y)
		{
			long num = (long)(((ulong)x + ((ulong)y - 1UL)) / (ulong)y);
			if (num > (long)((ulong)(-1)))
			{
				return 0U;
			}
			return (uint)num;
		}

		uint insertData(TiffType type, int v)
		{
			if (this.m_header.tiff_magic == 19789)
			{
				return (uint)((uint)(v & (int)this.m_typemask[(int)type]) << this.m_typeshift[(int)type]);
			}
			return (uint)(v & (int)this.m_typemask[(int)type]);
		}

		static void resetFieldBit(int[] fields, short f)
		{
			fields[(int)(f / 32)] &= ~Tiff.BITn((int)f);
		}

		static bool fieldSet(int[] fields, short f)
		{
			return (fields[(int)(f / 32)] & Tiff.BITn((int)f)) != 0;
		}

		bool writeRational(TiffType type, TiffTag tag, ref TiffDirEntry dir, float v)
		{
			dir.tdir_tag = tag;
			dir.tdir_type = type;
			dir.tdir_count = 1;
			return this.writeRationalArray(ref dir, new float[] { v });
		}

		bool writeRationalPair(TiffDirEntry[] entries, int dirOffset, TiffType type, TiffTag tag1, float v1, TiffTag tag2, float v2)
		{
			return this.writeRational(type, tag1, ref entries[dirOffset], v1) && this.writeRational(type, tag2, ref entries[dirOffset + 1], v2);
		}

		bool writeDirectory(bool done)
		{
			if (this.m_mode == 0)
			{
				return true;
			}
			if (done)
			{
				if ((this.m_flags & TiffFlags.POSTENCODE) == TiffFlags.POSTENCODE)
				{
					this.m_flags &= ~TiffFlags.POSTENCODE;
					if (!this.m_currentCodec.PostEncode())
					{
						Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Error post-encoding before directory write", new object[0]);
						return false;
					}
				}
				this.m_currentCodec.Close();
				if (this.m_rawcc > 0 && (this.m_flags & TiffFlags.BEENWRITING) == TiffFlags.BEENWRITING && !this.flushData1())
				{
					Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Error flushing data before directory write", new object[0]);
					return false;
				}
				if ((this.m_flags & TiffFlags.MYBUFFER) == TiffFlags.MYBUFFER && this.m_rawdata != null)
				{
					this.m_rawdata = null;
					this.m_rawcc = 0;
					this.m_rawdatasize = 0;
				}
				this.m_flags &= ~(TiffFlags.BUFFERSETUP | TiffFlags.BEENWRITING);
			}
			int num = 0;
			for (int i = 0; i <= 127; i++)
			{
				if (this.fieldSet(i) && i != 65)
				{
					num += ((i < 5) ? 2 : 1);
				}
			}
			num += this.m_dir.td_customValueCount;
			int num2 = num * 12;
			TiffDirEntry[] array = new TiffDirEntry[num];
			for (int j = 0; j < num; j++)
			{
				array[j] = new TiffDirEntry();
			}
			if (this.m_diroff == 0U && !this.linkDirectory())
			{
				return false;
			}
			this.m_dataoff = this.m_diroff + 2U + (uint)num2 + 4U;
			if ((this.m_dataoff & 1U) != 0U)
			{
				this.m_dataoff += 1U;
			}
			this.seekFile((long)((ulong)this.m_dataoff), SeekOrigin.Begin);
			this.m_curdir += 1;
			int num3 = 0;
			int[] array2 = new int[4];
			Buffer.BlockCopy(this.m_dir.td_fieldsset, 0, array2, 0, 16);
			if (Tiff.fieldSet(array2, 31) && this.m_dir.td_extrasamples == 0)
			{
				Tiff.resetFieldBit(array2, 31);
				num--;
				num2 -= 12;
			}
			int num4 = 0;
			int k = this.m_nfields;
			while (k > 0)
			{
				TiffFieldInfo tiffFieldInfo = this.m_fieldinfo[num4];
				if (tiffFieldInfo.Bit == 65)
				{
					bool flag = false;
					for (int l = 0; l < this.m_dir.td_customValueCount; l++)
					{
						flag |= this.m_dir.td_customValues[l].info == tiffFieldInfo;
					}
					if (flag)
					{
						goto IL_267;
					}
				}
				else if (Tiff.fieldSet(array2, tiffFieldInfo.Bit))
				{
					goto IL_267;
				}
				IL_73A:
				k--;
				num4++;
				continue;
				IL_267:
				short bit = tiffFieldInfo.Bit;
				if (bit <= 39)
				{
					switch (bit)
					{
					case 1:
						this.setupShortLong(TiffTag.IMAGEWIDTH, ref array[num3++], this.m_dir.td_imagewidth);
						this.setupShortLong(TiffTag.IMAGELENGTH, ref array[num3], this.m_dir.td_imagelength);
						goto IL_71B;
					case 2:
						this.setupShortLong(TiffTag.TILEWIDTH, ref array[num3++], this.m_dir.td_tilewidth);
						this.setupShortLong(TiffTag.TILELENGTH, ref array[num3], this.m_dir.td_tilelength);
						goto IL_71B;
					case 3:
						if (!this.writeRationalPair(array, num3, TiffType.RATIONAL, TiffTag.XRESOLUTION, this.m_dir.td_xresolution, TiffTag.YRESOLUTION, this.m_dir.td_yresolution))
						{
							return false;
						}
						num3++;
						goto IL_71B;
					case 4:
						if (!this.writeRationalPair(array, num3, TiffType.RATIONAL, TiffTag.XPOSITION, this.m_dir.td_xposition, TiffTag.YPOSITION, this.m_dir.td_yposition))
						{
							return false;
						}
						num3++;
						goto IL_71B;
					case 5:
					case 9:
					case 10:
					case 11:
					case 12:
					case 13:
					case 14:
					case 15:
					case 16:
					case 20:
					case 21:
					case 22:
						goto IL_6E0;
					case 6:
					case 18:
					case 19:
						break;
					case 7:
						this.setupShort(TiffTag.COMPRESSION, ref array[num3], (short)this.m_dir.td_compression);
						goto IL_71B;
					case 8:
						this.setupShort(TiffTag.PHOTOMETRIC, ref array[num3], (short)this.m_dir.td_photometric);
						goto IL_71B;
					case 17:
						this.setupShortLong(TiffTag.ROWSPERSTRIP, ref array[num3], this.m_dir.td_rowsperstrip);
						goto IL_71B;
					case 23:
						goto IL_5E7;
					case 24:
					{
						TiffTag tiffTag = (this.IsTiled() ? TiffTag.TILEBYTECOUNTS : TiffTag.STRIPBYTECOUNTS);
						if (tiffTag != tiffFieldInfo.Tag)
						{
							goto IL_73A;
						}
						array[num3].tdir_tag = tiffTag;
						array[num3].tdir_type = TiffType.LONG;
						array[num3].tdir_count = this.m_dir.td_nstrips;
						if (!this.writeLongArray(ref array[num3], this.m_dir.td_stripbytecount))
						{
							return false;
						}
						goto IL_71B;
					}
					case 25:
					{
						TiffTag tiffTag = (this.IsTiled() ? TiffTag.TILEOFFSETS : TiffTag.STRIPOFFSETS);
						if (tiffTag != tiffFieldInfo.Tag)
						{
							goto IL_73A;
						}
						array[num3].tdir_tag = tiffTag;
						array[num3].tdir_type = TiffType.LONG;
						array[num3].tdir_count = this.m_dir.td_nstrips;
						if (!this.writeLongArray(ref array[num3], this.m_dir.td_stripoffset))
						{
							return false;
						}
						goto IL_71B;
					}
					case 26:
						if (!this.writeShortTable(TiffTag.COLORMAP, ref array[num3], 3, this.m_dir.td_colormap))
						{
							return false;
						}
						goto IL_71B;
					default:
						switch (bit)
						{
						case 32:
							break;
						case 33:
						case 34:
							if (!this.writePerSampleAnys(this.sampleToTagType(), tiffFieldInfo.Tag, ref array[num3]))
							{
								return false;
							}
							goto IL_71B;
						case 35:
						case 36:
						case 38:
							goto IL_6E0;
						case 37:
						case 39:
							goto IL_5E7;
						default:
							goto IL_6E0;
						}
						break;
					}
					if (!this.writePerSampleShorts(tiffFieldInfo.Tag, ref array[num3]))
					{
						return false;
					}
					goto IL_71B;
					IL_5E7:
					if (!this.setupShortPair(tiffFieldInfo.Tag, ref array[num3]))
					{
						return false;
					}
				}
				else
				{
					switch (bit)
					{
					case 44:
						if (!this.writeTransferFunction(ref array[num3]))
						{
							return false;
						}
						break;
					case 45:
						goto IL_6E0;
					case 46:
						if (!this.writeInkNames(ref array[num3]))
						{
							return false;
						}
						break;
					default:
						if (bit != 49)
						{
							goto IL_6E0;
						}
						array[num3].tdir_tag = tiffFieldInfo.Tag;
						array[num3].tdir_type = TiffType.LONG;
						array[num3].tdir_count = (int)this.m_dir.td_nsubifd;
						if (!this.writeLongArray(ref array[num3], this.m_dir.td_subifd))
						{
							return false;
						}
						if (array[num3].tdir_count > 0)
						{
							this.m_flags |= TiffFlags.INSUBIFD;
							this.m_nsubifd = (short)array[num3].tdir_count;
							if (array[num3].tdir_count > 1)
							{
								this.m_subifdoff = array[num3].tdir_offset;
							}
							else
							{
								this.m_subifdoff = this.m_diroff + 2U + (uint)(num3 * 12) + 4U + 4U;
							}
						}
						break;
					}
				}
				IL_71B:
				num3++;
				if (tiffFieldInfo.Bit != 65)
				{
					Tiff.resetFieldBit(array2, tiffFieldInfo.Bit);
					goto IL_73A;
				}
				goto IL_73A;
				IL_6E0:
				if (tiffFieldInfo.Tag == TiffTag.DOTRANGE)
				{
					if (!this.setupShortPair(tiffFieldInfo.Tag, ref array[num3]))
					{
						return false;
					}
					goto IL_71B;
				}
				else
				{
					if (!this.writeNormalTag(ref array[num3], tiffFieldInfo))
					{
						return false;
					}
					goto IL_71B;
				}
			}
			short num5 = (short)num;
			uint nextdiroff = this.m_nextdiroff;
			if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
			{
				num3 = 0;
				while (num5 != 0)
				{
					short num6 = (short)array[num3].tdir_tag;
					Tiff.SwabShort(ref num6);
					array[num3].tdir_tag = (TiffTag)((ushort)num6);
					num6 = (short)array[num3].tdir_type;
					Tiff.SwabShort(ref num6);
					array[num3].tdir_type = (TiffType)num6;
					Tiff.SwabLong(ref array[num3].tdir_count);
					Tiff.SwabUInt(ref array[num3].tdir_offset);
					num3++;
					num5 -= 1;
				}
				num5 = (short)num;
				Tiff.SwabShort(ref num5);
				Tiff.SwabUInt(ref nextdiroff);
			}
			this.seekFile((long)((ulong)this.m_diroff), SeekOrigin.Begin);
			if (!this.writeShortOK(num5))
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Error writing directory count", new object[0]);
				return false;
			}
			if (!this.writeDirEntryOK(array, num2 / 12))
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Error writing directory contents", new object[0]);
				return false;
			}
			if (!this.writeIntOK((int)nextdiroff))
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Error writing directory link", new object[0]);
				return false;
			}
			if (done)
			{
				this.FreeDirectory();
				this.m_flags &= ~TiffFlags.DIRTYDIRECT;
				this.m_currentCodec.Cleanup();
				this.CreateDirectory();
			}
			return true;
		}

		bool writeNormalTag(ref TiffDirEntry dir, TiffFieldInfo fip)
		{
			short num = fip.WriteCount;
			dir.tdir_tag = fip.Tag;
			dir.tdir_type = fip.Type;
			dir.tdir_count = (int)num;
			switch (fip.Type)
			{
			case TiffType.BYTE:
			case TiffType.SBYTE:
				if (fip.PassCount)
				{
					byte[] cp;
					if (num == -3)
					{
						FieldValue[] field = this.GetField(fip.Tag);
						int tdir_count = field[0].ToInt();
						cp = field[1].ToByteArray();
						dir.tdir_count = tdir_count;
					}
					else
					{
						FieldValue[] field2 = this.GetField(fip.Tag);
						num = field2[0].ToShort();
						cp = field2[1].ToByteArray();
						dir.tdir_count = (int)num;
					}
					if (!this.writeByteArray(ref dir, cp))
					{
						return false;
					}
				}
				else if (num == 1)
				{
					byte[] array = new byte[1];
					FieldValue[] field3 = this.GetField(fip.Tag);
					array[0] = field3[0].ToByte();
					if (!this.writeByteArray(ref dir, array))
					{
						return false;
					}
				}
				else
				{
					FieldValue[] field4 = this.GetField(fip.Tag);
					byte[] cp2 = field4[0].ToByteArray();
					if (!this.writeByteArray(ref dir, cp2))
					{
						return false;
					}
				}
				break;
			case TiffType.ASCII:
			{
				FieldValue[] field5 = this.GetField(fip.Tag);
				string s;
				if (fip.PassCount)
				{
					s = field5[1].ToString();
				}
				else
				{
					s = field5[0].ToString();
				}
				byte[] bytes = Tiff.Latin1Encoding.GetBytes(s);
				byte[] array2 = new byte[bytes.Length + 1];
				Buffer.BlockCopy(bytes, 0, array2, 0, bytes.Length);
				dir.tdir_count = array2.Length;
				if (!this.writeByteArray(ref dir, array2))
				{
					return false;
				}
				break;
			}
			case TiffType.SHORT:
			case TiffType.SSHORT:
				if (fip.PassCount)
				{
					short[] v;
					if (num == -3)
					{
						FieldValue[] field6 = this.GetField(fip.Tag);
						int tdir_count2 = field6[0].ToInt();
						v = field6[1].ToShortArray();
						dir.tdir_count = tdir_count2;
					}
					else
					{
						FieldValue[] field7 = this.GetField(fip.Tag);
						num = field7[0].ToShort();
						v = field7[1].ToShortArray();
						dir.tdir_count = (int)num;
					}
					if (!this.writeShortArray(ref dir, v))
					{
						return false;
					}
				}
				else if (num == 1)
				{
					FieldValue[] field8 = this.GetField(fip.Tag);
					short v2 = field8[0].ToShort();
					dir.tdir_offset = this.insertData(dir.tdir_type, (int)v2);
				}
				else
				{
					FieldValue[] field9 = this.GetField(fip.Tag);
					short[] v3 = field9[0].ToShortArray();
					if (!this.writeShortArray(ref dir, v3))
					{
						return false;
					}
				}
				break;
			case TiffType.LONG:
			case TiffType.SLONG:
			case TiffType.IFD:
				if (fip.PassCount)
				{
					int[] v4;
					if (num == -3)
					{
						FieldValue[] field10 = this.GetField(fip.Tag);
						int tdir_count3 = field10[0].ToInt();
						v4 = field10[1].ToIntArray();
						dir.tdir_count = tdir_count3;
					}
					else
					{
						FieldValue[] field11 = this.GetField(fip.Tag);
						num = field11[0].ToShort();
						v4 = field11[1].ToIntArray();
						dir.tdir_count = (int)num;
					}
					if (!this.writeLongArray(ref dir, v4))
					{
						return false;
					}
				}
				else if (num == 1)
				{
					FieldValue[] field12 = this.GetField(fip.Tag);
					dir.tdir_offset = field12[0].ToUInt();
				}
				else
				{
					FieldValue[] field13 = this.GetField(fip.Tag);
					int[] v5 = field13[0].ToIntArray();
					if (!this.writeLongArray(ref dir, v5))
					{
						return false;
					}
				}
				break;
			case TiffType.RATIONAL:
			case TiffType.SRATIONAL:
				if (fip.PassCount)
				{
					float[] v6;
					if (num == -3)
					{
						FieldValue[] field14 = this.GetField(fip.Tag);
						int tdir_count4 = field14[0].ToInt();
						v6 = field14[1].ToFloatArray();
						dir.tdir_count = tdir_count4;
					}
					else
					{
						FieldValue[] field15 = this.GetField(fip.Tag);
						num = field15[0].ToShort();
						v6 = field15[1].ToFloatArray();
						dir.tdir_count = (int)num;
					}
					if (!this.writeRationalArray(ref dir, v6))
					{
						return false;
					}
				}
				else if (num == 1)
				{
					float[] array3 = new float[1];
					FieldValue[] field16 = this.GetField(fip.Tag);
					array3[0] = field16[0].ToFloat();
					if (!this.writeRationalArray(ref dir, array3))
					{
						return false;
					}
				}
				else
				{
					FieldValue[] field17 = this.GetField(fip.Tag);
					float[] v7 = field17[0].ToFloatArray();
					if (!this.writeRationalArray(ref dir, v7))
					{
						return false;
					}
				}
				break;
			case TiffType.UNDEFINED:
			{
				byte[] cp3;
				if (num == -1)
				{
					FieldValue[] field18 = this.GetField(fip.Tag);
					num = field18[0].ToShort();
					cp3 = field18[1].ToByteArray();
					dir.tdir_count = (int)num;
				}
				else if (num == -3)
				{
					FieldValue[] field19 = this.GetField(fip.Tag);
					int tdir_count5 = field19[0].ToInt();
					cp3 = field19[1].ToByteArray();
					dir.tdir_count = tdir_count5;
				}
				else
				{
					FieldValue[] field20 = this.GetField(fip.Tag);
					cp3 = field20[0].ToByteArray();
				}
				if (!this.writeByteArray(ref dir, cp3))
				{
					return false;
				}
				break;
			}
			case TiffType.FLOAT:
				if (fip.PassCount)
				{
					float[] v8;
					if (num == -3)
					{
						FieldValue[] field21 = this.GetField(fip.Tag);
						int tdir_count6 = field21[0].ToInt();
						v8 = field21[1].ToFloatArray();
						dir.tdir_count = tdir_count6;
					}
					else
					{
						FieldValue[] field22 = this.GetField(fip.Tag);
						num = field22[0].ToShort();
						v8 = field22[1].ToFloatArray();
						dir.tdir_count = (int)num;
					}
					if (!this.writeFloatArray(ref dir, v8))
					{
						return false;
					}
				}
				else if (num == 1)
				{
					float[] array4 = new float[1];
					FieldValue[] field23 = this.GetField(fip.Tag);
					array4[0] = field23[0].ToFloat();
					if (!this.writeFloatArray(ref dir, array4))
					{
						return false;
					}
				}
				else
				{
					FieldValue[] field24 = this.GetField(fip.Tag);
					float[] v9 = field24[0].ToFloatArray();
					if (!this.writeFloatArray(ref dir, v9))
					{
						return false;
					}
				}
				break;
			case TiffType.DOUBLE:
				if (fip.PassCount)
				{
					double[] v10;
					if (num == -3)
					{
						FieldValue[] field25 = this.GetField(fip.Tag);
						int tdir_count7 = field25[0].ToInt();
						v10 = field25[1].ToDoubleArray();
						dir.tdir_count = tdir_count7;
					}
					else
					{
						FieldValue[] field26 = this.GetField(fip.Tag);
						num = field26[0].ToShort();
						v10 = field26[1].ToDoubleArray();
						dir.tdir_count = (int)num;
					}
					if (!this.writeDoubleArray(ref dir, v10))
					{
						return false;
					}
				}
				else if (num == 1)
				{
					double[] array5 = new double[1];
					FieldValue[] field27 = this.GetField(fip.Tag);
					array5[0] = field27[0].ToDouble();
					if (!this.writeDoubleArray(ref dir, array5))
					{
						return false;
					}
				}
				else
				{
					FieldValue[] field28 = this.GetField(fip.Tag);
					double[] v11 = field28[0].ToDoubleArray();
					if (!this.writeDoubleArray(ref dir, v11))
					{
						return false;
					}
				}
				break;
			}
			return true;
		}

		void setupShortLong(TiffTag tag, ref TiffDirEntry dir, int v)
		{
			dir.tdir_tag = tag;
			dir.tdir_count = 1;
			if ((long)v > 65535L)
			{
				dir.tdir_type = TiffType.LONG;
				dir.tdir_offset = (uint)v;
				return;
			}
			dir.tdir_type = TiffType.SHORT;
			dir.tdir_offset = this.insertData(TiffType.SHORT, v);
		}

		void setupShort(TiffTag tag, ref TiffDirEntry dir, short v)
		{
			dir.tdir_tag = tag;
			dir.tdir_count = 1;
			dir.tdir_type = TiffType.SHORT;
			dir.tdir_offset = this.insertData(TiffType.SHORT, (int)v);
		}

		bool writePerSampleShorts(TiffTag tag, ref TiffDirEntry dir)
		{
			short[] array = new short[(int)this.m_dir.td_samplesperpixel];
			FieldValue[] field = this.GetField(tag);
			short num = field[0].ToShort();
			for (short num2 = 0; num2 < this.m_dir.td_samplesperpixel; num2 += 1)
			{
				array[(int)num2] = num;
			}
			dir.tdir_tag = tag;
			dir.tdir_type = TiffType.SHORT;
			dir.tdir_count = (int)this.m_dir.td_samplesperpixel;
			return this.writeShortArray(ref dir, array);
		}

		bool writePerSampleAnys(TiffType type, TiffTag tag, ref TiffDirEntry dir)
		{
			double[] array = new double[(int)this.m_dir.td_samplesperpixel];
			FieldValue[] field = this.GetField(tag);
			double num = field[0].ToDouble();
			for (short num2 = 0; num2 < this.m_dir.td_samplesperpixel; num2 += 1)
			{
				array[(int)num2] = num;
			}
			return this.writeAnyArray(type, tag, ref dir, (int)this.m_dir.td_samplesperpixel, array);
		}

		bool setupShortPair(TiffTag tag, ref TiffDirEntry dir)
		{
			short[] array = new short[2];
			FieldValue[] field = this.GetField(tag);
			array[0] = field[0].ToShort();
			array[1] = field[1].ToShort();
			dir.tdir_tag = tag;
			dir.tdir_type = TiffType.SHORT;
			dir.tdir_count = 2;
			return this.writeShortArray(ref dir, array);
		}

		bool writeShortTable(TiffTag tag, ref TiffDirEntry dir, int n, short[][] table)
		{
			dir.tdir_tag = tag;
			dir.tdir_type = TiffType.SHORT;
			dir.tdir_count = 1 << (int)this.m_dir.td_bitspersample;
			uint dataoff = this.m_dataoff;
			for (int i = 0; i < n; i++)
			{
				if (!this.writeData(ref dir, table[i], dir.tdir_count))
				{
					return false;
				}
			}
			dir.tdir_count *= n;
			dir.tdir_offset = dataoff;
			return true;
		}

		bool writeByteArray(ref TiffDirEntry dir, byte[] cp)
		{
			if (dir.tdir_count <= 4)
			{
				if (this.m_header.tiff_magic == 19789)
				{
					dir.tdir_offset = (uint)((uint)cp[0] << 24);
					if (dir.tdir_count >= 2)
					{
						dir.tdir_offset |= (uint)((uint)cp[1] << 16);
					}
					if (dir.tdir_count >= 3)
					{
						dir.tdir_offset |= (uint)((uint)cp[2] << 8);
					}
					if (dir.tdir_count == 4)
					{
						dir.tdir_offset |= (uint)cp[3];
					}
				}
				else
				{
					dir.tdir_offset = (uint)cp[0];
					if (dir.tdir_count >= 2)
					{
						dir.tdir_offset |= (uint)((uint)cp[1] << 8);
					}
					if (dir.tdir_count >= 3)
					{
						dir.tdir_offset |= (uint)((uint)cp[2] << 16);
					}
					if (dir.tdir_count == 4)
					{
						dir.tdir_offset |= (uint)((uint)cp[3] << 24);
					}
				}
				return true;
			}
			return this.writeData(ref dir, cp, dir.tdir_count);
		}

		bool writeShortArray(ref TiffDirEntry dir, short[] v)
		{
			if (dir.tdir_count <= 2)
			{
				if (this.m_header.tiff_magic == 19789)
				{
					dir.tdir_offset = (uint)((uint)v[0] << 16);
					if (dir.tdir_count == 2)
					{
						dir.tdir_offset |= (uint)v[1] & 65535U;
					}
				}
				else
				{
					dir.tdir_offset = (uint)v[0] & 65535U;
					if (dir.tdir_count == 2)
					{
						dir.tdir_offset |= (uint)((uint)v[1] << 16);
					}
				}
				return true;
			}
			return this.writeData(ref dir, v, dir.tdir_count);
		}

		bool writeLongArray(ref TiffDirEntry dir, int[] v)
		{
			if (dir.tdir_count == 1)
			{
				dir.tdir_offset = (uint)v[0];
				return true;
			}
			return this.writeData(ref dir, v, dir.tdir_count);
		}

		bool writeLongArray(ref TiffDirEntry dir, uint[] v)
		{
			int[] array = new int[v.Length];
			Buffer.BlockCopy(v, 0, array, 0, v.Length * 4);
			return this.writeLongArray(ref dir, array);
		}

		bool writeRationalArray(ref TiffDirEntry dir, float[] v)
		{
			int[] array = new int[2 * dir.tdir_count];
			for (int i = 0; i < dir.tdir_count; i++)
			{
				int num = 1;
				float num2 = v[i];
				if (num2 < 0f)
				{
					if (dir.tdir_type == TiffType.RATIONAL)
					{
						Tiff.WarningExt(this, this.m_clientdata, this.m_name, "\"{0}\": Information lost writing value ({1:G}) as (unsigned) RATIONAL", new object[]
						{
							this.FieldWithTag(dir.tdir_tag).Name,
							num2
						});
						num2 = 0f;
					}
					else
					{
						num2 = -num2;
						num = -1;
					}
				}
				int num3 = 1;
				if (num2 > 0f)
				{
					while (num2 < 268435460f && (long)num3 < 268435456L)
					{
						num2 *= 8f;
						num3 *= 8;
					}
				}
				array[2 * i] = (int)((double)num * ((double)num2 + 0.5));
				array[2 * i + 1] = num3;
			}
			return this.writeData(ref dir, array, 2 * dir.tdir_count);
		}

		bool writeFloatArray(ref TiffDirEntry dir, float[] v)
		{
			if (dir.tdir_count == 1)
			{
				dir.tdir_offset = BitConverter.ToUInt32(BitConverter.GetBytes(v[0]), 0);
				return true;
			}
			return this.writeData(ref dir, v, dir.tdir_count);
		}

		bool writeDoubleArray(ref TiffDirEntry dir, double[] v)
		{
			return this.writeData(ref dir, v, dir.tdir_count);
		}

		bool writeAnyArray(TiffType type, TiffTag tag, ref TiffDirEntry dir, int n, double[] v)
		{
			dir.tdir_tag = tag;
			dir.tdir_type = type;
			dir.tdir_count = n;
			bool flag = false;
			switch (type)
			{
			case TiffType.BYTE:
			case TiffType.SBYTE:
			{
				byte[] array = new byte[n];
				for (int i = 0; i < n; i++)
				{
					array[i] = (byte)v[i];
				}
				if (!this.writeByteArray(ref dir, array))
				{
					flag = true;
					goto IL_137;
				}
				goto IL_137;
			}
			case TiffType.SHORT:
			case TiffType.SSHORT:
			{
				short[] array2 = new short[n];
				for (int j = 0; j < n; j++)
				{
					array2[j] = (short)v[j];
				}
				if (!this.writeShortArray(ref dir, array2))
				{
					flag = true;
					goto IL_137;
				}
				goto IL_137;
			}
			case TiffType.LONG:
			case TiffType.SLONG:
			{
				int[] array3 = new int[n];
				for (int k = 0; k < n; k++)
				{
					array3[k] = (int)v[k];
				}
				if (!this.writeLongArray(ref dir, array3))
				{
					flag = true;
					goto IL_137;
				}
				goto IL_137;
			}
			case TiffType.FLOAT:
			{
				float[] array4 = new float[n];
				for (int l = 0; l < n; l++)
				{
					array4[l] = (float)v[l];
				}
				if (!this.writeFloatArray(ref dir, array4))
				{
					flag = true;
					goto IL_137;
				}
				goto IL_137;
			}
			case TiffType.DOUBLE:
				if (!this.writeDoubleArray(ref dir, v))
				{
					flag = true;
					goto IL_137;
				}
				goto IL_137;
			}
			flag = true;
			IL_137:
			return !flag;
		}

		bool writeTransferFunction(ref TiffDirEntry dir)
		{
			int num = (int)(this.m_dir.td_samplesperpixel - this.m_dir.td_extrasamples);
			int n = 1;
			bool flag = false;
			int elementCount = 1 << (int)this.m_dir.td_bitspersample;
			if (num < 0 || num > 2)
			{
				if (Tiff.Compare(this.m_dir.td_transferfunction[0], this.m_dir.td_transferfunction[2], elementCount) != 0)
				{
					n = 3;
				}
				else
				{
					flag = true;
				}
			}
			if ((num == 2 || flag) && Tiff.Compare(this.m_dir.td_transferfunction[0], this.m_dir.td_transferfunction[1], elementCount) != 0)
			{
				n = 3;
			}
			return this.writeShortTable(TiffTag.TRANSFERFUNCTION, ref dir, n, this.m_dir.td_transferfunction);
		}

		bool writeInkNames(ref TiffDirEntry dir)
		{
			dir.tdir_tag = TiffTag.INKNAMES;
			dir.tdir_type = TiffType.ASCII;
			byte[] bytes = Tiff.Latin1Encoding.GetBytes(this.m_dir.td_inknames);
			dir.tdir_count = bytes.Length;
			return this.writeByteArray(ref dir, bytes);
		}

		bool writeData(ref TiffDirEntry dir, byte[] buffer, int count)
		{
			dir.tdir_offset = this.m_dataoff;
			count = dir.tdir_count * Tiff.DataWidth(dir.tdir_type);
			if (this.seekOK((long)((ulong)dir.tdir_offset)) && this.writeOK(buffer, 0, count))
			{
				this.m_dataoff += (uint)((count + 1) & -2);
				return true;
			}
			Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Error writing data for field \"{0}\"", new object[] { this.FieldWithTag(dir.tdir_tag).Name });
			return false;
		}

		bool writeData(ref TiffDirEntry dir, short[] buffer, int count)
		{
			if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
			{
				Tiff.SwabArrayOfShort(buffer, count);
			}
			int num = count * 2;
			byte[] array = new byte[num];
			Tiff.ShortsToByteArray(buffer, 0, count, array, 0);
			return this.writeData(ref dir, array, num);
		}

		bool writeData(ref TiffDirEntry dir, int[] cp, int cc)
		{
			if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
			{
				Tiff.SwabArrayOfLong(cp, cc);
			}
			int num = cc * 4;
			byte[] array = new byte[num];
			Tiff.IntsToByteArray(cp, 0, cc, array, 0);
			return this.writeData(ref dir, array, num);
		}

		bool writeData(ref TiffDirEntry dir, float[] cp, int cc)
		{
			int[] array = new int[cc];
			for (int i = 0; i < cc; i++)
			{
				byte[] bytes = BitConverter.GetBytes(cp[i]);
				array[i] = BitConverter.ToInt32(bytes, 0);
			}
			return this.writeData(ref dir, array, cc);
		}

		bool writeData(ref TiffDirEntry dir, double[] buffer, int count)
		{
			if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
			{
				Tiff.SwabArrayOfDouble(buffer, count);
			}
			byte[] array = new byte[count * 8];
			Buffer.BlockCopy(buffer, 0, array, 0, array.Length);
			return this.writeData(ref dir, array, count * 8);
		}

		bool linkDirectory()
		{
			this.m_diroff = (uint)((this.seekFile(0L, SeekOrigin.End) + 1L) & -2L);
			uint diroff = this.m_diroff;
			if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
			{
				Tiff.SwabUInt(ref diroff);
			}
			if ((this.m_flags & TiffFlags.INSUBIFD) == TiffFlags.INSUBIFD)
			{
				this.seekFile((long)((ulong)this.m_subifdoff), SeekOrigin.Begin);
				if (!this.writeIntOK((int)diroff))
				{
					Tiff.ErrorExt(this, this.m_clientdata, "linkDirectory", "{0}: Error writing SubIFD directory link", new object[] { this.m_name });
					return false;
				}
				this.m_nsubifd -= 1;
				if (this.m_nsubifd != 0)
				{
					this.m_subifdoff += 4U;
				}
				else
				{
					this.m_flags &= ~TiffFlags.INSUBIFD;
				}
				return true;
			}
			else
			{
				if (this.m_header.tiff_diroff != 0U)
				{
					uint tiff_diroff = this.m_header.tiff_diroff;
					short num;
					while (this.seekOK((long)((ulong)tiff_diroff)) && this.readShortOK(out num))
					{
						if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
						{
							Tiff.SwabShort(ref num);
						}
						this.seekFile((long)(num * 12), SeekOrigin.Current);
						if (!this.readUIntOK(out tiff_diroff))
						{
							Tiff.ErrorExt(this, this.m_clientdata, "linkDirectory", "Error fetching directory link", new object[0]);
							return false;
						}
						if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
						{
							Tiff.SwabUInt(ref tiff_diroff);
						}
						if (tiff_diroff == 0U)
						{
							long num2 = this.seekFile(0L, SeekOrigin.Current);
							this.seekFile(num2 - 4L, SeekOrigin.Begin);
							if (!this.writeIntOK((int)diroff))
							{
								Tiff.ErrorExt(this, this.m_clientdata, "linkDirectory", "Error writing directory link", new object[0]);
								return false;
							}
							return true;
						}
					}
					Tiff.ErrorExt(this, this.m_clientdata, "linkDirectory", "Error fetching directory count", new object[0]);
					return false;
				}
				this.m_header.tiff_diroff = this.m_diroff;
				this.seekFile(4L, SeekOrigin.Begin);
				if (!this.writeIntOK((int)diroff))
				{
					Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Error writing TIFF header", new object[0]);
					return false;
				}
				return true;
			}
		}

		Tiff()
		{
			this.m_clientdata = 0;
			this.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmNone;
			this.setupBuiltInCodecs();
			this.m_defaultTagMethods = new TiffTagMethods();
			if (Tiff.m_errorHandler == null)
			{
				Tiff.m_errorHandler = new TiffErrorHandler();
			}
		}

		void Dispose(bool disposing)
		{
			if (!this.m_disposed)
			{
				if (disposing)
				{
					this.Close();
					if (this.m_fileStream != null)
					{
						this.m_fileStream.Dispose();
					}
				}
				this.m_disposed = true;
			}
		}

		bool WriteCustomDirectory(out long pdiroff)
		{
			pdiroff = -1L;
			if (this.m_mode == 0)
			{
				return true;
			}
			int num = 0;
			for (int i = 0; i <= 127; i++)
			{
				if (this.fieldSet(i) && i != 65)
				{
					num += ((i < 5) ? 2 : 1);
				}
			}
			num += this.m_dir.td_customValueCount;
			int num2 = num * 12;
			TiffDirEntry[] array = new TiffDirEntry[num];
			this.m_diroff = (uint)((this.seekFile(0L, SeekOrigin.End) + 1L) & -2L);
			this.m_dataoff = this.m_diroff + 2U + (uint)num2 + 4U;
			if ((this.m_dataoff & 1U) != 0U)
			{
				this.m_dataoff += 1U;
			}
			this.seekFile((long)((ulong)this.m_dataoff), SeekOrigin.Begin);
			int[] array2 = new int[4];
			Buffer.BlockCopy(this.m_dir.td_fieldsset, 0, array2, 0, 16);
			int num3 = 0;
			int j = this.m_nfields;
			while (j > 0)
			{
				TiffFieldInfo tiffFieldInfo = this.m_fieldinfo[num3];
				if (tiffFieldInfo.Bit == 65)
				{
					bool flag = false;
					for (int k = 0; k < this.m_dir.td_customValueCount; k++)
					{
						flag |= this.m_dir.td_customValues[k].info == tiffFieldInfo;
					}
					if (flag)
					{
						goto IL_137;
					}
				}
				else if (Tiff.fieldSet(array2, tiffFieldInfo.Bit))
				{
					goto IL_137;
				}
				IL_150:
				j--;
				num3++;
				continue;
				IL_137:
				if (tiffFieldInfo.Bit != 65)
				{
					Tiff.resetFieldBit(array2, tiffFieldInfo.Bit);
					goto IL_150;
				}
				goto IL_150;
			}
			short num4 = (short)num;
			pdiroff = (long)((ulong)this.m_nextdiroff);
			if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
			{
				for (int l = 0; l < (int)num4; l++)
				{
					TiffDirEntry tiffDirEntry = array[l];
					short num5 = (short)tiffDirEntry.tdir_tag;
					Tiff.SwabShort(ref num5);
					tiffDirEntry.tdir_tag = (TiffTag)((ushort)num5);
					num5 = (short)tiffDirEntry.tdir_type;
					Tiff.SwabShort(ref num5);
					tiffDirEntry.tdir_type = (TiffType)num5;
					Tiff.SwabLong(ref tiffDirEntry.tdir_count);
					Tiff.SwabUInt(ref tiffDirEntry.tdir_offset);
				}
				num4 = (short)num;
				Tiff.SwabShort(ref num4);
				int num6 = (int)pdiroff;
				Tiff.SwabLong(ref num6);
				pdiroff = (long)num6;
			}
			this.seekFile((long)((ulong)this.m_diroff), SeekOrigin.Begin);
			if (!this.writeShortOK(num4))
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Error writing directory count", new object[0]);
				return false;
			}
			if (!this.writeDirEntryOK(array, num2 / 12))
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Error writing directory contents", new object[0]);
				return false;
			}
			if (!this.writeIntOK((int)pdiroff))
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Error writing directory link", new object[0]);
				return false;
			}
			return true;
		}

		internal static void SwabUInt(ref uint lp)
		{
			byte[] array = new byte[]
			{
				(byte)lp,
				(byte)(lp >> 8),
				(byte)(lp >> 16),
				(byte)(lp >> 24)
			};
			byte b = array[3];
			array[3] = array[0];
			array[0] = b;
			b = array[2];
			array[2] = array[1];
			array[1] = b;
			lp = (uint)(array[0] & byte.MaxValue);
			lp += (uint)((uint)(array[1] & byte.MaxValue) << 8);
			lp += (uint)((uint)(array[2] & byte.MaxValue) << 16);
			lp += (uint)((uint)array[3] << 24);
		}

		internal static uint[] Realloc(uint[] buffer, int elementCount, int newElementCount)
		{
			uint[] array = new uint[newElementCount];
			if (buffer != null)
			{
				int num = System.Math.Min(elementCount, newElementCount);
				Buffer.BlockCopy(buffer, 0, array, 0, num * 4);
			}
			return array;
		}

		internal static TiffFieldInfo[] Realloc(TiffFieldInfo[] buffer, int elementCount, int newElementCount)
		{
			TiffFieldInfo[] array = new TiffFieldInfo[newElementCount];
			if (buffer != null)
			{
				int length = System.Math.Min(elementCount, newElementCount);
				Array.Copy(buffer, array, length);
			}
			return array;
		}

		internal static TiffTagValue[] Realloc(TiffTagValue[] buffer, int elementCount, int newElementCount)
		{
			TiffTagValue[] array = new TiffTagValue[newElementCount];
			if (buffer != null)
			{
				int length = System.Math.Min(elementCount, newElementCount);
				Array.Copy(buffer, array, length);
			}
			return array;
		}

		internal bool setCompressionScheme(Compression scheme)
		{
			TiffCodec tiffCodec = this.FindCodec(scheme);
			if (tiffCodec == null)
			{
				tiffCodec = this.m_builtInCodecs[0];
			}
			this.m_decodestatus = tiffCodec.CanDecode;
			this.m_flags &= ~(TiffFlags.NOBITREV | TiffFlags.NOREADRAW);
			this.m_currentCodec = tiffCodec;
			return tiffCodec.Init();
		}

		void postDecode(byte[] buffer, int offset, int count)
		{
			switch (this.m_postDecodeMethod)
			{
			case Tiff.PostDecodeMethodType.pdmSwab16Bit:
				Tiff.swab16BitData(buffer, offset, count);
				return;
			case Tiff.PostDecodeMethodType.pdmSwab24Bit:
				Tiff.swab24BitData(buffer, offset, count);
				return;
			case Tiff.PostDecodeMethodType.pdmSwab32Bit:
				Tiff.swab32BitData(buffer, offset, count);
				return;
			case Tiff.PostDecodeMethodType.pdmSwab64Bit:
				Tiff.swab64BitData(buffer, offset, count);
				return;
			default:
				return;
			}
		}

		void initOrder(int magic)
		{
			this.m_typemask = Tiff.typemask;
			if (magic == 19789)
			{
				this.m_typeshift = Tiff.bigTypeshift;
				this.m_flags |= TiffFlags.SWAB;
				return;
			}
			this.m_typeshift = Tiff.litTypeshift;
		}

		static int getMode(string mode, string module, out FileMode m, out FileAccess a)
		{
			m = (FileMode)0;
			a = (FileAccess)0;
			int result = -1;
			if (mode.Length == 0)
			{
				return result;
			}
			char c = mode[0];
			if (c != 'a')
			{
				if (c != 'r')
				{
					if (c != 'w')
					{
						Tiff.ErrorExt(null, 0, module, "\"{0}\": Bad mode", new object[] { mode });
					}
					else
					{
						m = FileMode.Create;
						a = FileAccess.ReadWrite;
						result = 770;
					}
				}
				else
				{
					m = FileMode.Open;
					a = FileAccess.Read;
					result = 0;
					if (mode.Length > 1 && mode[1] == '+')
					{
						a = FileAccess.ReadWrite;
						result = 2;
					}
				}
			}
			else
			{
				m = FileMode.Open;
				a = FileAccess.ReadWrite;
				result = 258;
			}
			return result;
		}

		static void printField(Stream fd, TiffFieldInfo fip, int value_count, object raw_data)
		{
			Tiff.fprintf(fd, "  {0}: ", new object[] { fip.Name });
			byte[] array = raw_data as byte[];
			sbyte[] array2 = raw_data as sbyte[];
			short[] array3 = raw_data as short[];
			ushort[] array4 = raw_data as ushort[];
			int[] array5 = raw_data as int[];
			uint[] array6 = raw_data as uint[];
			float[] array7 = raw_data as float[];
			double[] array8 = raw_data as double[];
			string text = raw_data as string;
			for (int i = 0; i < value_count; i++)
			{
				if (fip.Type == TiffType.BYTE || fip.Type == TiffType.SBYTE)
				{
					if (array != null)
					{
						Tiff.fprintf(fd, "{0}", new object[] { array[i] });
					}
					else if (array2 != null)
					{
						Tiff.fprintf(fd, "{0}", new object[] { array2[i] });
					}
				}
				else if (fip.Type == TiffType.UNDEFINED)
				{
					if (array != null)
					{
						Tiff.fprintf(fd, "0x{0:x}", new object[] { array[i] });
					}
				}
				else if (fip.Type == TiffType.SHORT || fip.Type == TiffType.SSHORT)
				{
					if (array3 != null)
					{
						Tiff.fprintf(fd, "{0}", new object[] { array3[i] });
					}
					else if (array4 != null)
					{
						Tiff.fprintf(fd, "{0}", new object[] { array4[i] });
					}
				}
				else if (fip.Type == TiffType.LONG || fip.Type == TiffType.SLONG)
				{
					if (array5 != null)
					{
						Tiff.fprintf(fd, "{0}", new object[] { array5[i] });
					}
					else if (array6 != null)
					{
						Tiff.fprintf(fd, "{0}", new object[] { array6[i] });
					}
				}
				else if (fip.Type == TiffType.RATIONAL || fip.Type == TiffType.SRATIONAL || fip.Type == TiffType.FLOAT)
				{
					if (array7 != null)
					{
						Tiff.fprintf(fd, "{0}", new object[] { array7[i] });
					}
				}
				else if (fip.Type == TiffType.IFD)
				{
					if (array5 != null)
					{
						Tiff.fprintf(fd, "0x{0:x}", new object[] { array5[i] });
					}
					else if (array6 != null)
					{
						Tiff.fprintf(fd, "0x{0:x}", new object[] { array6[i] });
					}
				}
				else if (fip.Type == TiffType.ASCII)
				{
					if (text != null)
					{
						Tiff.fprintf(fd, "{0}", new object[] { text });
						break;
					}
					break;
				}
				else
				{
					if (fip.Type != TiffType.DOUBLE && fip.Type != TiffType.FLOAT)
					{
						Tiff.fprintf(fd, "<unsupported data type in printField>", new object[0]);
						break;
					}
					if (array7 != null)
					{
						Tiff.fprintf(fd, "{0}", new object[] { array7[i] });
					}
					else if (array8 != null)
					{
						Tiff.fprintf(fd, "{0}", new object[] { array8[i] });
					}
				}
				if (i < value_count - 1)
				{
					Tiff.fprintf(fd, ",", new object[0]);
				}
			}
			Tiff.fprintf(fd, "\r\n", new object[0]);
		}

		bool prettyPrintField(Stream fd, TiffTag tag, int value_count, object raw_data)
		{
			FieldValue fieldValue = new FieldValue(raw_data);
			short[] array = fieldValue.ToShortArray();
			float[] array2 = fieldValue.ToFloatArray();
			double[] array3 = fieldValue.ToDoubleArray();
			if (tag <= TiffTag.REFERENCEBLACKWHITE)
			{
				if (tag <= TiffTag.INKSET)
				{
					if (tag != TiffTag.WHITEPOINT)
					{
						if (tag == TiffTag.INKSET)
						{
							if (array != null)
							{
								Tiff.fprintf(fd, "  Ink Set: ", new object[0]);
								InkSet inkSet = (InkSet)array[0];
								if (inkSet == InkSet.CMYK)
								{
									Tiff.fprintf(fd, "CMYK\n", new object[0]);
								}
								else
								{
									Tiff.fprintf(fd, "{0} (0x{1:x})\n", new object[]
									{
										array[0],
										array[0]
									});
								}
								return true;
							}
							return false;
						}
					}
					else
					{
						if (array2 != null)
						{
							Tiff.fprintf(fd, "  White Point: {0:G}-{1:G}\n", new object[]
							{
								array2[0],
								array2[1]
							});
							return true;
						}
						return false;
					}
				}
				else if (tag != TiffTag.DOTRANGE)
				{
					if (tag == TiffTag.REFERENCEBLACKWHITE)
					{
						if (array2 != null)
						{
							Tiff.fprintf(fd, "  Reference Black/White:\n", new object[0]);
							for (short num = 0; num < 3; num += 1)
							{
								Tiff.fprintf(fd, "    {0,2:D}: {1,5:G} {2,5:G}\n", new object[]
								{
									num,
									array2[(int)(2 * num)],
									array2[(int)(2 * num + 1)]
								});
							}
							return true;
						}
						return false;
					}
				}
				else
				{
					if (array != null)
					{
						Tiff.fprintf(fd, "  Dot Range: {0}-{1}\n", new object[]
						{
							array[0],
							array[1]
						});
						return true;
					}
					return false;
				}
			}
			else if (tag <= TiffTag.RICHTIFFIPTC)
			{
				if (tag != TiffTag.XMLPACKET)
				{
					if (tag == TiffTag.RICHTIFFIPTC)
					{
						Tiff.fprintf(fd, "  RichTIFFIPTC Data: <present>, {0} bytes\n", new object[] { value_count * 4 });
						return true;
					}
				}
				else
				{
					string text = raw_data as string;
					if (text != null)
					{
						Tiff.fprintf(fd, "  XMLPacket (XMP Metadata):\n", new object[0]);
						Tiff.fprintf(fd, text.Substring(0, value_count), new object[0]);
						Tiff.fprintf(fd, "\n", new object[0]);
						return true;
					}
					return false;
				}
			}
			else
			{
				if (tag == TiffTag.PHOTOSHOP)
				{
					Tiff.fprintf(fd, "  Photoshop Data: <present>, {0} bytes\n", new object[] { value_count });
					return true;
				}
				if (tag == TiffTag.ICCPROFILE)
				{
					Tiff.fprintf(fd, "  ICC Profile: <present>, {0} bytes\n", new object[] { value_count });
					return true;
				}
				if (tag == TiffTag.STONITS)
				{
					if (array3 != null)
					{
						Tiff.fprintf(fd, "  Sample to Nits conversion factor: {0:e4}\n", new object[] { array3[0] });
						return true;
					}
					return false;
				}
			}
			return false;
		}

		static void printAscii(Stream fd, string cp)
		{
			int num = 0;
			while (cp[num] != '\0')
			{
				if (!char.IsControl(cp[num]))
				{
					Tiff.fprintf(fd, "{0}", new object[] { cp[num] });
				}
				else
				{
					string text = "\tt\bb\rr\nn\vv";
					int num2 = 0;
					while (text[num2] != '\0' && text[num2++] != cp[num])
					{
						num2++;
					}
					if (text[num2] != '\0')
					{
						Tiff.fprintf(fd, "\\{0}", new object[] { text[num2] });
					}
					else
					{
						Tiff.fprintf(fd, "\\{0}", new object[] { Tiff.encodeOctalString((byte)(cp[num] & 'ÿ')) });
					}
				}
				num++;
			}
		}

		int readFile(byte[] buf, int offset, int size)
		{
			return this.m_stream.Read(this.m_clientdata, buf, offset, size);
		}

		long seekFile(long off, SeekOrigin whence)
		{
			return this.m_stream.Seek(this.m_clientdata, off, whence);
		}

		long getFileSize()
		{
			return this.m_stream.Size(this.m_clientdata);
		}

		bool readOK(byte[] buf, int size)
		{
			return this.readFile(buf, 0, size) == size;
		}

		bool readShortOK(out short value)
		{
			byte[] array = new byte[2];
			bool flag = this.readOK(array, 2);
			value = 0;
			if (flag)
			{
				value = (short)(array[0] & byte.MaxValue);
				value += (short)((array[1] & byte.MaxValue) << 8);
			}
			return flag;
		}

		bool readUIntOK(out uint value)
		{
			int num;
			bool flag = this.readIntOK(out num);
			if (flag)
			{
				value = (uint)num;
			}
			else
			{
				value = 0U;
			}
			return flag;
		}

		bool readIntOK(out int value)
		{
			byte[] array = new byte[4];
			bool flag = this.readOK(array, 4);
			value = 0;
			if (flag)
			{
				value = (int)(array[0] & byte.MaxValue);
				value += (int)(array[1] & byte.MaxValue) << 8;
				value += (int)(array[2] & byte.MaxValue) << 16;
				value += (int)array[3] << 24;
			}
			return flag;
		}

		bool readDirEntryOk(TiffDirEntry[] dir, short dircount)
		{
			int num = 12;
			int num2 = num * (int)dircount;
			byte[] array = new byte[num2];
			bool flag = this.readOK(array, num2);
			if (flag)
			{
				Tiff.readDirEntry(dir, dircount, array, 0);
			}
			return flag;
		}

		static void readDirEntry(TiffDirEntry[] dir, short dircount, byte[] bytes, int offset)
		{
			int num = offset;
			for (int i = 0; i < (int)dircount; i++)
			{
				TiffDirEntry tiffDirEntry = new TiffDirEntry();
				tiffDirEntry.tdir_tag = (TiffTag)((ushort)Tiff.readShort(bytes, num));
				num += 2;
				tiffDirEntry.tdir_type = (TiffType)Tiff.readShort(bytes, num);
				num += 2;
				tiffDirEntry.tdir_count = Tiff.readInt(bytes, num);
				num += 4;
				tiffDirEntry.tdir_offset = (uint)Tiff.readInt(bytes, num);
				num += 4;
				dir[i] = tiffDirEntry;
			}
		}

		bool readHeaderOk(ref TiffHeader header)
		{
			bool flag = this.readShortOK(out header.tiff_magic);
			if (flag)
			{
				flag = this.readShortOK(out header.tiff_version);
			}
			if (flag)
			{
				flag = this.readUIntOK(out header.tiff_diroff);
			}
			return flag;
		}

		bool seekOK(long off)
		{
			return this.seekFile(off, SeekOrigin.Begin) == off;
		}

		bool seek(int row, short sample)
		{
			if (row >= this.m_dir.td_imagelength)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "{0}: Row out of range, max {1}", new object[]
				{
					row,
					this.m_dir.td_imagelength
				});
				return false;
			}
			int num;
			if (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
			{
				if (sample >= this.m_dir.td_samplesperpixel)
				{
					Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "{0}: Sample out of range, max {1}", new object[]
					{
						sample,
						this.m_dir.td_samplesperpixel
					});
					return false;
				}
				if (this.m_dir.td_rowsperstrip != -1)
				{
					num = (int)sample * this.m_dir.td_stripsperimage + row / this.m_dir.td_rowsperstrip;
				}
				else
				{
					num = 0;
				}
			}
			else if (this.m_dir.td_rowsperstrip != -1)
			{
				num = row / this.m_dir.td_rowsperstrip;
			}
			else
			{
				num = 0;
			}
			if (num != this.m_curstrip)
			{
				if (!this.fillStrip(num))
				{
					return false;
				}
			}
			else if (row < this.m_row && !this.startStrip(num))
			{
				return false;
			}
			if (row != this.m_row)
			{
				if (!this.m_currentCodec.Seek(row - this.m_row))
				{
					return false;
				}
				this.m_row = row;
			}
			return true;
		}

		int readRawStrip1(int strip, byte[] buf, int offset, int size, string module)
		{
			if (!this.seekOK((long)((ulong)this.m_dir.td_stripoffset[strip])))
			{
				Tiff.ErrorExt(this, this.m_clientdata, module, "{0}: Seek error at scanline {1}, strip {2}", new object[] { this.m_name, this.m_row, strip });
				return -1;
			}
			int num = this.readFile(buf, offset, size);
			if (num != size)
			{
				Tiff.ErrorExt(this, this.m_clientdata, module, "{0}: Read error at scanline {1}; got {2} bytes, expected {3}", new object[] { this.m_name, this.m_row, num, size });
				return -1;
			}
			return size;
		}

		int readRawTile1(int tile, byte[] buf, int offset, int size, string module)
		{
			if (!this.seekOK((long)((ulong)this.m_dir.td_stripoffset[tile])))
			{
				Tiff.ErrorExt(this, this.m_clientdata, module, "{0}: Seek error at row {1}, col {2}, tile {3}", new object[] { this.m_name, this.m_row, this.m_col, tile });
				return -1;
			}
			int num = this.readFile(buf, offset, size);
			if (num != size)
			{
				Tiff.ErrorExt(this, this.m_clientdata, module, "{0}: Read error at row {1}, col {2}; got {3} bytes, expected {4}", new object[] { this.m_name, this.m_row, this.m_col, num, size });
				return -1;
			}
			return size;
		}

		bool startStrip(int strip)
		{
			if ((this.m_flags & TiffFlags.CODERSETUP) != TiffFlags.CODERSETUP)
			{
				if (!this.m_currentCodec.SetupDecode())
				{
					return false;
				}
				this.m_flags |= TiffFlags.CODERSETUP;
			}
			this.m_curstrip = strip;
			this.m_row = strip % this.m_dir.td_stripsperimage * this.m_dir.td_rowsperstrip;
			this.m_rawcp = 0;
			if ((this.m_flags & TiffFlags.NOREADRAW) == TiffFlags.NOREADRAW)
			{
				this.m_rawcc = 0;
			}
			else
			{
				this.m_rawcc = (int)this.m_dir.td_stripbytecount[strip];
			}
			return this.m_currentCodec.PreDecode((short)(strip / this.m_dir.td_stripsperimage));
		}

		bool startTile(int tile)
		{
			if ((this.m_flags & TiffFlags.CODERSETUP) != TiffFlags.CODERSETUP)
			{
				if (!this.m_currentCodec.SetupDecode())
				{
					return false;
				}
				this.m_flags |= TiffFlags.CODERSETUP;
			}
			this.m_curtile = tile;
			this.m_row = tile % Tiff.howMany(this.m_dir.td_imagewidth, this.m_dir.td_tilewidth) * this.m_dir.td_tilelength;
			this.m_col = tile % Tiff.howMany(this.m_dir.td_imagelength, this.m_dir.td_tilelength) * this.m_dir.td_tilewidth;
			this.m_rawcp = 0;
			if ((this.m_flags & TiffFlags.NOREADRAW) == TiffFlags.NOREADRAW)
			{
				this.m_rawcc = 0;
			}
			else
			{
				this.m_rawcc = (int)this.m_dir.td_stripbytecount[tile];
			}
			return this.m_currentCodec.PreDecode((short)(tile / this.m_dir.td_stripsperimage));
		}

		bool checkRead(bool tiles)
		{
			if (this.m_mode == 1)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "File not open for reading", new object[0]);
				return false;
			}
			if (tiles ^ this.IsTiled())
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, tiles ? "Can not read tiles from a stripped image" : "Can not read scanlines from a tiled image", new object[0]);
				return false;
			}
			return true;
		}

		static void swab16BitData(byte[] buffer, int offset, int count)
		{
			short[] array = Tiff.ByteArrayToShorts(buffer, offset, count);
			Tiff.SwabArrayOfShort(array, count / 2);
			Tiff.ShortsToByteArray(array, 0, count / 2, buffer, offset);
		}

		static void swab24BitData(byte[] buffer, int offset, int count)
		{
			Tiff.SwabArrayOfTriples(buffer, offset, count / 3);
		}

		static void swab32BitData(byte[] buffer, int offset, int count)
		{
			int[] array = Tiff.ByteArrayToInts(buffer, offset, count);
			Tiff.SwabArrayOfLong(array, count / 4);
			Tiff.IntsToByteArray(array, 0, count / 4, buffer, offset);
		}

		static void swab64BitData(byte[] buffer, int offset, int count)
		{
			int num = count / 8;
			double[] array = new double[num];
			int num2 = offset;
			for (int i = 0; i < num; i++)
			{
				array[i] = BitConverter.ToDouble(buffer, num2);
				num2 += 8;
			}
			Tiff.SwabArrayOfDouble(array, num);
			num2 = offset;
			for (int j = 0; j < num; j++)
			{
				byte[] bytes = BitConverter.GetBytes(array[j]);
				Buffer.BlockCopy(bytes, 0, buffer, num2, bytes.Length);
				num2 += bytes.Length;
			}
		}

		internal bool fillStrip(int strip)
		{
			if ((this.m_flags & TiffFlags.NOREADRAW) != TiffFlags.NOREADRAW)
			{
				int num = (int)this.m_dir.td_stripbytecount[strip];
				if (num <= 0)
				{
					Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "{0}: Invalid strip byte count, strip {1}", new object[] { num, strip });
					return false;
				}
				if (num > this.m_rawdatasize)
				{
					this.m_curstrip = -1;
					if ((this.m_flags & TiffFlags.MYBUFFER) != TiffFlags.MYBUFFER)
					{
						Tiff.ErrorExt(this, this.m_clientdata, "fillStrip", "{0}: Data buffer too small to hold strip {1}", new object[] { this.m_name, strip });
						return false;
					}
					this.ReadBufferSetup(null, Tiff.roundUp(num, 1024));
				}
				if (this.readRawStrip1(strip, this.m_rawdata, 0, num, "fillStrip") != num)
				{
					return false;
				}
				if (!this.isFillOrder(this.m_dir.td_fillorder) && (this.m_flags & TiffFlags.NOBITREV) != TiffFlags.NOBITREV)
				{
					Tiff.ReverseBits(this.m_rawdata, num);
				}
			}
			return this.startStrip(strip);
		}

		internal bool fillTile(int tile)
		{
			if ((this.m_flags & TiffFlags.NOREADRAW) != TiffFlags.NOREADRAW)
			{
				int num = (int)this.m_dir.td_stripbytecount[tile];
				if (num <= 0)
				{
					Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "{0}: Invalid tile byte count, tile {1}", new object[] { num, tile });
					return false;
				}
				if (num > this.m_rawdatasize)
				{
					this.m_curtile = -1;
					if ((this.m_flags & TiffFlags.MYBUFFER) != TiffFlags.MYBUFFER)
					{
						Tiff.ErrorExt(this, this.m_clientdata, "fillTile", "{0}: Data buffer too small to hold tile {1}", new object[] { this.m_name, tile });
						return false;
					}
					this.ReadBufferSetup(null, Tiff.roundUp(num, 1024));
				}
				if (this.readRawTile1(tile, this.m_rawdata, 0, num, "fillTile") != num)
				{
					return false;
				}
				if (!this.isFillOrder(this.m_dir.td_fillorder) && (this.m_flags & TiffFlags.NOBITREV) != TiffFlags.NOBITREV)
				{
					Tiff.ReverseBits(this.m_rawdata, num);
				}
			}
			return this.startTile(tile);
		}

		int summarize(int summand1, int summand2, string where)
		{
			int num = summand1 + summand2;
			if (num - summand1 != summand2)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Integer overflow in {0}", new object[] { where });
				num = 0;
			}
			return num;
		}

		int multiply(int nmemb, int elem_size, string where)
		{
			int num = nmemb * elem_size;
			if (elem_size != 0 && num / elem_size != nmemb)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Integer overflow in {0}", new object[] { where });
				num = 0;
			}
			return num;
		}

		internal int newScanlineSize()
		{
			int nmemb;
			if (this.m_dir.td_planarconfig == PlanarConfig.CONTIG)
			{
				if (this.m_dir.td_photometric == Photometric.YCBCR && !this.IsUpSampled())
				{
					FieldValue[] field = this.GetField(TiffTag.YCBCRSUBSAMPLING);
					ushort num = field[0].ToUShort();
					ushort num2 = field[1].ToUShort();
					if (num * num2 == 0)
					{
						Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Invalid YCbCr subsampling", new object[0]);
						return 0;
					}
					return ((this.m_dir.td_imagewidth + (int)num - 1) / (int)num * (int)(num * num2 + 2) * (int)this.m_dir.td_bitspersample + 7) / 8 / (int)num2;
				}
				else
				{
					nmemb = this.multiply(this.m_dir.td_imagewidth, (int)this.m_dir.td_samplesperpixel, "TIFFScanlineSize");
				}
			}
			else
			{
				nmemb = this.m_dir.td_imagewidth;
			}
			return Tiff.howMany8(this.multiply(nmemb, (int)this.m_dir.td_bitspersample, "TIFFScanlineSize"));
		}

		internal int oldScanlineSize()
		{
			int num = this.multiply((int)this.m_dir.td_bitspersample, this.m_dir.td_imagewidth, "TIFFScanlineSize");
			if (this.m_dir.td_planarconfig == PlanarConfig.CONTIG)
			{
				num = this.multiply(num, (int)this.m_dir.td_samplesperpixel, "TIFFScanlineSize");
			}
			return Tiff.howMany8(num);
		}

		bool writeCheckStrips(string module)
		{
			return (this.m_flags & TiffFlags.BEENWRITING) == TiffFlags.BEENWRITING || this.WriteCheck(false, module);
		}

		bool writeCheckTiles(string module)
		{
			return (this.m_flags & TiffFlags.BEENWRITING) == TiffFlags.BEENWRITING || this.WriteCheck(true, module);
		}

		void bufferCheck()
		{
			if ((this.m_flags & TiffFlags.BUFFERSETUP) != TiffFlags.BUFFERSETUP || this.m_rawdata == null)
			{
				this.WriteBufferSetup(null, -1);
			}
		}

		bool writeOK(byte[] buffer, int offset, int count)
		{
			try
			{
				this.m_stream.Write(this.m_clientdata, buffer, offset, count);
			}
			catch (Exception)
			{
				Tiff.Warning(this, "writeOK", "Failed to write {0} bytes", new object[] { count });
				return false;
			}
			return true;
		}

		bool writeHeaderOK(TiffHeader header)
		{
			bool flag = this.writeShortOK(header.tiff_magic);
			if (flag)
			{
				flag = this.writeShortOK(header.tiff_version);
			}
			if (flag)
			{
				flag = this.writeIntOK((int)header.tiff_diroff);
			}
			return flag;
		}

		bool writeDirEntryOK(TiffDirEntry[] entries, int count)
		{
			bool flag = true;
			for (int i = 0; i < count; i++)
			{
				flag = this.writeShortOK((short)entries[i].tdir_tag);
				if (flag)
				{
					flag = this.writeShortOK((short)entries[i].tdir_type);
				}
				if (flag)
				{
					flag = this.writeIntOK(entries[i].tdir_count);
				}
				if (flag)
				{
					flag = this.writeIntOK((int)entries[i].tdir_offset);
				}
				if (!flag)
				{
					break;
				}
			}
			return flag;
		}

		bool writeShortOK(short value)
		{
			return this.writeOK(new byte[]
			{
				(byte)value,
				(byte)(value >> 8)
			}, 0, 2);
		}

		bool writeIntOK(int value)
		{
			return this.writeOK(new byte[]
			{
				(byte)value,
				(byte)(value >> 8),
				(byte)(value >> 16),
				(byte)(value >> 24)
			}, 0, 4);
		}

		bool isUnspecified(int f)
		{
			return this.fieldSet(f) && this.m_dir.td_imagelength == 0;
		}

		bool growStrips(int delta)
		{
			uint[] td_stripoffset = Tiff.Realloc(this.m_dir.td_stripoffset, this.m_dir.td_nstrips, this.m_dir.td_nstrips + delta);
			uint[] td_stripbytecount = Tiff.Realloc(this.m_dir.td_stripbytecount, this.m_dir.td_nstrips, this.m_dir.td_nstrips + delta);
			this.m_dir.td_stripoffset = td_stripoffset;
			this.m_dir.td_stripbytecount = td_stripbytecount;
			Array.Clear(this.m_dir.td_stripoffset, this.m_dir.td_nstrips, delta);
			Array.Clear(this.m_dir.td_stripbytecount, this.m_dir.td_nstrips, delta);
			this.m_dir.td_nstrips += delta;
			return true;
		}

		bool appendToStrip(int strip, byte[] buffer, int offset, int count)
		{
			if (this.m_dir.td_stripoffset[strip] == 0U || this.m_curoff == 0U)
			{
				if (this.m_dir.td_stripbytecount[strip] != 0U && this.m_dir.td_stripoffset[strip] != 0U && (ulong)this.m_dir.td_stripbytecount[strip] >= (ulong)((long)count))
				{
					if (!this.seekOK((long)((ulong)this.m_dir.td_stripoffset[strip])))
					{
						Tiff.ErrorExt(this, this.m_clientdata, "appendToStrip", "Seek error at scanline {0}", new object[] { this.m_row });
						return false;
					}
				}
				else
				{
					this.m_dir.td_stripoffset[strip] = (uint)this.seekFile(0L, SeekOrigin.End);
				}
				this.m_curoff = this.m_dir.td_stripoffset[strip];
				this.m_dir.td_stripbytecount[strip] = 0U;
			}
			if (!this.writeOK(buffer, offset, count))
			{
				Tiff.ErrorExt(this, this.m_clientdata, "appendToStrip", "Write error at scanline {0}", new object[] { this.m_row });
				return false;
			}
			this.m_curoff += (uint)count;
			this.m_dir.td_stripbytecount[strip] += (uint)count;
			return true;
		}

		internal bool flushData1()
		{
			if (this.m_rawcc > 0)
			{
				if (!this.isFillOrder(this.m_dir.td_fillorder) && (this.m_flags & TiffFlags.NOBITREV) != TiffFlags.NOBITREV)
				{
					Tiff.ReverseBits(this.m_rawdata, this.m_rawcc);
				}
				if (!this.appendToStrip(this.IsTiled() ? this.m_curtile : this.m_curstrip, this.m_rawdata, 0, this.m_rawcc))
				{
					return false;
				}
				this.m_rawcc = 0;
				this.m_rawcp = 0;
			}
			return true;
		}

		public static string GetVersion()
		{
			return string.Format(CultureInfo.InvariantCulture, "LibTiff.Net, Version {0}\nCopyright (C) 2008-2011, Bit Miracle.", new object[] { Tiff.AssemblyVersion });
		}

		public static string AssemblyVersion
		{
			get
			{
				return "";
			}
		}

		public static int GetR(int abgr)
		{
			return abgr & 255;
		}

		public static int GetG(int abgr)
		{
			return (abgr >> 8) & 255;
		}

		public static int GetB(int abgr)
		{
			return (abgr >> 16) & 255;
		}

		public static int GetA(int abgr)
		{
			return (abgr >> 24) & 255;
		}

		public TiffCodec FindCodec(Compression scheme)
		{
			for (Tiff.codecList codecList = this.m_registeredCodecs; codecList != null; codecList = codecList.next)
			{
				if (codecList.codec.m_scheme == scheme)
				{
					return codecList.codec;
				}
			}
			int num = 0;
			while (this.m_builtInCodecs[num] != null)
			{
				TiffCodec tiffCodec = this.m_builtInCodecs[num];
				if (tiffCodec.m_scheme == scheme)
				{
					return tiffCodec;
				}
				num++;
			}
			return null;
		}

		public void RegisterCodec(TiffCodec codec)
		{
			if (codec == null)
			{
				throw new ArgumentNullException("codec");
			}
			this.m_registeredCodecs = new Tiff.codecList
			{
				codec = codec,
				next = this.m_registeredCodecs
			};
		}

		public void UnRegisterCodec(TiffCodec codec)
		{
			if (this.m_registeredCodecs == null)
			{
				return;
			}
			if (this.m_registeredCodecs.codec == codec)
			{
				Tiff.codecList next = this.m_registeredCodecs.next;
				this.m_registeredCodecs = next;
				return;
			}
			for (Tiff.codecList codecList = this.m_registeredCodecs; codecList != null; codecList = codecList.next)
			{
				if (codecList.next != null && codecList.next.codec == codec)
				{
					Tiff.codecList next = codecList.next.next;
					codecList.next = next;
					return;
				}
			}
			Tiff.ErrorExt(this, 0, "UnRegisterCodec", "Cannot remove compression scheme {0}; not registered", new object[] { codec.m_name });
		}

		public bool IsCodecConfigured(Compression scheme)
		{
			TiffCodec tiffCodec = this.FindCodec(scheme);
			return tiffCodec != null && (tiffCodec.CanEncode || tiffCodec.CanDecode);
		}

		public TiffCodec[] GetConfiguredCodecs()
		{
			int num = 0;
			int num2 = 0;
			while (this.m_builtInCodecs[num2] != null)
			{
				if (this.m_builtInCodecs[num2] != null && this.IsCodecConfigured(this.m_builtInCodecs[num2].m_scheme))
				{
					num++;
				}
				num2++;
			}
			for (Tiff.codecList codecList = this.m_registeredCodecs; codecList != null; codecList = codecList.next)
			{
				num++;
			}
			TiffCodec[] array = new TiffCodec[num];
			int num3 = 0;
			for (Tiff.codecList codecList2 = this.m_registeredCodecs; codecList2 != null; codecList2 = codecList2.next)
			{
				array[num3++] = codecList2.codec;
			}
			int num4 = 0;
			while (this.m_builtInCodecs[num4] != null)
			{
				if (this.m_builtInCodecs[num4] != null && this.IsCodecConfigured(this.m_builtInCodecs[num4].m_scheme))
				{
					array[num3++] = this.m_builtInCodecs[num4];
				}
				num4++;
			}
			return array;
		}

		public static byte[] Realloc(byte[] array, int size)
		{
			byte[] array2 = new byte[size];
			if (array != null)
			{
				int count = System.Math.Min(array.Length, size);
				Buffer.BlockCopy(array, 0, array2, 0, count);
			}
			return array2;
		}

		public static int[] Realloc(int[] array, int size)
		{
			int[] array2 = new int[size];
			if (array != null)
			{
				int num = System.Math.Min(array.Length, size);
				Buffer.BlockCopy(array, 0, array2, 0, num * 4);
			}
			return array2;
		}

		public static int Compare(short[] first, short[] second, int elementCount)
		{
			for (int i = 0; i < elementCount; i++)
			{
				if (first[i] != second[i])
				{
					return (int)(first[i] - second[i]);
				}
			}
			return 0;
		}

		public static Tiff Open(string fileName, string mode)
		{
			return Tiff.Open(fileName, mode, null, null);
		}

		public static Tiff ClientOpen(string name, string mode, object clientData, TiffStream stream)
		{
			return Tiff.ClientOpen(name, mode, clientData, stream, null, null);
		}

		public void Close()
		{
			this.Flush();
			this.m_stream.Close(this.m_clientdata);
			if (this.m_fileStream != null)
			{
				this.m_fileStream.Close();
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public int GetTagListCount()
		{
			return this.m_dir.td_customValueCount;
		}

		public int GetTagListEntry(int index)
		{
			if (index < 0 || index >= this.m_dir.td_customValueCount)
			{
				return -1;
			}
			return (int)this.m_dir.td_customValues[index].info.Tag;
		}

		public void MergeFieldInfo(TiffFieldInfo[] info, int count)
		{
			this.m_foundfield = null;
			if (this.m_nfields > 0)
			{
				this.m_fieldinfo = Tiff.Realloc(this.m_fieldinfo, this.m_nfields, this.m_nfields + count);
			}
			else
			{
				this.m_fieldinfo = new TiffFieldInfo[count];
			}
			for (int i = 0; i < count; i++)
			{
				if (this.FindFieldInfo(info[i].Tag, info[i].Type) == null)
				{
					this.m_fieldinfo[this.m_nfields] = info[i];
					this.m_nfields++;
				}
			}
			IComparer comparer = new TagCompare();
			Array.Sort(this.m_fieldinfo, 0, this.m_nfields, comparer);
		}

		public TiffFieldInfo FindFieldInfo(TiffTag tag, TiffType type)
		{
			if (this.m_foundfield != null && this.m_foundfield.Tag == tag && (type == TiffType.NOTYPE || type == this.m_foundfield.Type))
			{
				return this.m_foundfield;
			}
			if (this.m_fieldinfo == null)
			{
				return null;
			}
			this.m_foundfield = null;
			foreach (TiffFieldInfo tiffFieldInfo in this.m_fieldinfo)
			{
				if (tiffFieldInfo != null && tiffFieldInfo.Tag == tag && (type == TiffType.NOTYPE || type == tiffFieldInfo.Type))
				{
					this.m_foundfield = tiffFieldInfo;
					break;
				}
			}
			return this.m_foundfield;
		}

		public TiffFieldInfo FindFieldInfoByName(string name, TiffType type)
		{
			if (this.m_foundfield != null && this.m_foundfield.Name == name && (type == TiffType.NOTYPE || type == this.m_foundfield.Type))
			{
				return this.m_foundfield;
			}
			if (this.m_fieldinfo == null)
			{
				return null;
			}
			this.m_foundfield = null;
			foreach (TiffFieldInfo tiffFieldInfo in this.m_fieldinfo)
			{
				if (tiffFieldInfo != null && tiffFieldInfo.Name == name && (type == TiffType.NOTYPE || type == tiffFieldInfo.Type))
				{
					this.m_foundfield = tiffFieldInfo;
					break;
				}
			}
			return this.m_foundfield;
		}

		public TiffFieldInfo FieldWithTag(TiffTag tag)
		{
			TiffFieldInfo tiffFieldInfo = this.FindFieldInfo(tag, TiffType.NOTYPE);
			if (tiffFieldInfo != null)
			{
				return tiffFieldInfo;
			}
			Tiff.ErrorExt(this, this.m_clientdata, "FieldWithTag", "Internal error, unknown tag 0x{0:x}", new object[] { tag });
			return null;
		}

		public TiffFieldInfo FieldWithName(string name)
		{
			TiffFieldInfo tiffFieldInfo = this.FindFieldInfoByName(name, TiffType.NOTYPE);
			if (tiffFieldInfo != null)
			{
				return tiffFieldInfo;
			}
			Tiff.ErrorExt(this, this.m_clientdata, "FieldWithName", "Internal error, unknown tag {0}", new object[] { name });
			return null;
		}

		public TiffTagMethods GetTagMethods()
		{
			return this.m_tagmethods;
		}

		public TiffTagMethods SetTagMethods(TiffTagMethods methods)
		{
			TiffTagMethods tagmethods = this.m_tagmethods;
			if (methods != null)
			{
				this.m_tagmethods = methods;
			}
			return tagmethods;
		}

		public object GetClientInfo(string name)
		{
			Tiff.clientInfoLink clientInfoLink = this.m_clientinfo;
			while (clientInfoLink != null && clientInfoLink.name != name)
			{
				clientInfoLink = clientInfoLink.next;
			}
			if (clientInfoLink != null)
			{
				return clientInfoLink.data;
			}
			return null;
		}

		public void SetClientInfo(object data, string name)
		{
			Tiff.clientInfoLink clientInfoLink = this.m_clientinfo;
			while (clientInfoLink != null && clientInfoLink.name != name)
			{
				clientInfoLink = clientInfoLink.next;
			}
			if (clientInfoLink != null)
			{
				clientInfoLink.data = data;
				return;
			}
			this.m_clientinfo = new Tiff.clientInfoLink
			{
				next = this.m_clientinfo,
				name = name,
				data = data
			};
		}

		public bool Flush()
		{
			if (this.m_mode != 0)
			{
				if (!this.FlushData())
				{
					return false;
				}
				if ((this.m_flags & TiffFlags.DIRTYDIRECT) == TiffFlags.DIRTYDIRECT && !this.WriteDirectory())
				{
					return false;
				}
			}
			return true;
		}

		public bool FlushData()
		{
			if ((this.m_flags & TiffFlags.BEENWRITING) != TiffFlags.BEENWRITING)
			{
				return false;
			}
			if ((this.m_flags & TiffFlags.POSTENCODE) == TiffFlags.POSTENCODE)
			{
				this.m_flags &= ~TiffFlags.POSTENCODE;
				if (!this.m_currentCodec.PostEncode())
				{
					return false;
				}
			}
			return this.flushData1();
		}

		public FieldValue[] GetField(TiffTag tag)
		{
			TiffFieldInfo tiffFieldInfo = this.FindFieldInfo(tag, TiffType.NOTYPE);
			if (tiffFieldInfo != null && (Tiff.isPseudoTag(tag) || this.fieldSet((int)tiffFieldInfo.Bit)))
			{
				return this.m_tagmethods.GetField(this, tag);
			}
			return null;
		}

		public FieldValue[] GetFieldDefaulted(TiffTag tag)
		{
			TiffDirectory dir = this.m_dir;
			FieldValue[] array = this.GetField(tag);
			if (array != null)
			{
				return array;
			}
			if (tag <= TiffTag.PLANARCONFIG)
			{
				if (tag <= TiffTag.BITSPERSAMPLE)
				{
					if (tag != TiffTag.SUBFILETYPE)
					{
						if (tag == TiffTag.BITSPERSAMPLE)
						{
							array = new FieldValue[1];
							array[0].Set(dir.td_bitspersample);
						}
					}
					else
					{
						array = new FieldValue[1];
						array[0].Set(dir.td_subfiletype);
					}
				}
				else if (tag != TiffTag.THRESHHOLDING)
				{
					if (tag != TiffTag.FILLORDER)
					{
						switch (tag)
						{
						case TiffTag.ORIENTATION:
							array = new FieldValue[1];
							array[0].Set(dir.td_orientation);
							break;
						case TiffTag.SAMPLESPERPIXEL:
							array = new FieldValue[1];
							array[0].Set(dir.td_samplesperpixel);
							break;
						case TiffTag.ROWSPERSTRIP:
							array = new FieldValue[1];
							array[0].Set(dir.td_rowsperstrip);
							break;
						case TiffTag.MINSAMPLEVALUE:
							array = new FieldValue[1];
							array[0].Set(dir.td_minsamplevalue);
							break;
						case TiffTag.MAXSAMPLEVALUE:
							array = new FieldValue[1];
							array[0].Set(dir.td_maxsamplevalue);
							break;
						case TiffTag.PLANARCONFIG:
							array = new FieldValue[1];
							array[0].Set(dir.td_planarconfig);
							break;
						}
					}
					else
					{
						array = new FieldValue[1];
						array[0].Set(dir.td_fillorder);
					}
				}
				else
				{
					array = new FieldValue[1];
					array[0].Set(dir.td_threshholding);
				}
			}
			else if (tag <= TiffTag.WHITEPOINT)
			{
				if (tag != TiffTag.RESOLUTIONUNIT)
				{
					if (tag != TiffTag.TRANSFERFUNCTION)
					{
						if (tag == TiffTag.WHITEPOINT)
						{
							float[] o = new float[] { 0.34574193f, 0.35856044f };
							array = new FieldValue[1];
							array[0].Set(o);
						}
					}
					else
					{
						if (dir.td_transferfunction[0] == null && !Tiff.defaultTransferFunction(dir))
						{
							Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "No space for \"TransferFunction\" tag", new object[0]);
							return null;
						}
						array = new FieldValue[3];
						array[0].Set(dir.td_transferfunction[0]);
						if (dir.td_samplesperpixel - dir.td_extrasamples > 1)
						{
							array[1].Set(dir.td_transferfunction[1]);
							array[2].Set(dir.td_transferfunction[2]);
						}
					}
				}
				else
				{
					array = new FieldValue[1];
					array[0].Set(dir.td_resolutionunit);
				}
			}
			else
			{
				switch (tag)
				{
				case TiffTag.INKSET:
					array = new FieldValue[1];
					array[0].Set(InkSet.CMYK);
					break;
				case TiffTag.INKNAMES:
				case (TiffTag)335:
				case TiffTag.TARGETPRINTER:
					break;
				case TiffTag.NUMBEROFINKS:
					array = new FieldValue[1];
					array[0].Set(4);
					break;
				case TiffTag.DOTRANGE:
					array = new FieldValue[2];
					array[0].Set(0);
					array[1].Set((1 << (int)dir.td_bitspersample) - 1);
					break;
				case TiffTag.EXTRASAMPLES:
					array = new FieldValue[2];
					array[0].Set(dir.td_extrasamples);
					array[1].Set(dir.td_sampleinfo);
					break;
				case TiffTag.SAMPLEFORMAT:
					array = new FieldValue[1];
					array[0].Set(dir.td_sampleformat);
					break;
				default:
					switch (tag)
					{
					case TiffTag.YCBCRCOEFFICIENTS:
					{
						float[] o2 = new float[] { 0.299f, 0.587f, 0.114f };
						array = new FieldValue[1];
						array[0].Set(o2);
						break;
					}
					case TiffTag.YCBCRSUBSAMPLING:
						array = new FieldValue[2];
						array[0].Set(dir.td_ycbcrsubsampling[0]);
						array[1].Set(dir.td_ycbcrsubsampling[1]);
						break;
					case TiffTag.YCBCRPOSITIONING:
						array = new FieldValue[1];
						array[0].Set(dir.td_ycbcrpositioning);
						break;
					case TiffTag.REFERENCEBLACKWHITE:
						if (dir.td_refblackwhite == null)
						{
							Tiff.defaultRefBlackWhite(dir);
						}
						array = new FieldValue[1];
						array[0].Set(dir.td_refblackwhite);
						break;
					default:
						switch (tag)
						{
						case TiffTag.MATTEING:
							array = new FieldValue[1];
							array[0].Set(dir.td_extrasamples == 1 && dir.td_sampleinfo[0] == ExtraSample.ASSOCALPHA);
							break;
						case TiffTag.DATATYPE:
							array = new FieldValue[1];
							array[0].Set(dir.td_sampleformat - 1);
							break;
						case TiffTag.IMAGEDEPTH:
							array = new FieldValue[1];
							array[0].Set(dir.td_imagedepth);
							break;
						case TiffTag.TILEDEPTH:
							array = new FieldValue[1];
							array[0].Set(dir.td_tiledepth);
							break;
						}
						break;
					}
					break;
				}
			}
			return array;
		}

		public bool ReadDirectory()
		{
			this.m_diroff = this.m_nextdiroff;
			if (this.m_diroff == 0U)
			{
				return false;
			}
			if (!this.checkDirOffset(this.m_nextdiroff))
			{
				return false;
			}
			this.m_currentCodec.Cleanup();
			this.m_curdir += 1;
			TiffDirEntry[] array;
			short num = this.fetchDirectory(this.m_nextdiroff, out array, out this.m_nextdiroff);
			if (num == 0)
			{
				Tiff.ErrorExt(this, this.m_clientdata, "ReadDirectory", "{0}: Failed to read directory at offset {1}", new object[] { this.m_name, this.m_nextdiroff });
				return false;
			}
			this.m_flags &= ~TiffFlags.BEENWRITING;
			this.FreeDirectory();
			this.setupDefaultDirectory();
			this.SetField(TiffTag.PLANARCONFIG, new object[] { PlanarConfig.CONTIG });
			for (int i = 0; i < (int)num; i++)
			{
				TiffDirEntry tiffDirEntry = array[i];
				if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
				{
					short num2 = (short)tiffDirEntry.tdir_tag;
					Tiff.SwabShort(ref num2);
					tiffDirEntry.tdir_tag = (TiffTag)((ushort)num2);
					num2 = (short)tiffDirEntry.tdir_type;
					Tiff.SwabShort(ref num2);
					tiffDirEntry.tdir_type = (TiffType)num2;
					Tiff.SwabLong(ref tiffDirEntry.tdir_count);
					Tiff.SwabUInt(ref tiffDirEntry.tdir_offset);
				}
				if (tiffDirEntry.tdir_tag == TiffTag.SAMPLESPERPIXEL)
				{
					if (!this.fetchNormalTag(array[i]))
					{
						return false;
					}
					tiffDirEntry.tdir_tag = TiffTag.IGNORE;
				}
			}
			int num3 = 0;
			bool flag = false;
			bool flag2 = false;
			for (int j = 0; j < (int)num; j++)
			{
				if (array[j].tdir_tag != TiffTag.IGNORE)
				{
					if (num3 >= this.m_nfields)
					{
						num3 = 0;
					}
					if (array[j].tdir_tag < this.m_fieldinfo[num3].Tag)
					{
						if (!flag)
						{
							Tiff.WarningExt(this, this.m_clientdata, "ReadDirectory", "{0}: invalid TIFF directory; tags are not sorted in ascending order", new object[] { this.m_name });
							flag = true;
						}
						num3 = 0;
					}
					while (num3 < this.m_nfields && this.m_fieldinfo[num3].Tag < array[j].tdir_tag)
					{
						num3++;
					}
					if (num3 >= this.m_nfields || this.m_fieldinfo[num3].Tag != array[j].tdir_tag)
					{
						flag2 = true;
					}
					else if (this.m_fieldinfo[num3].Bit == 0)
					{
						array[j].tdir_tag = TiffTag.IGNORE;
					}
					else
					{
						TiffFieldInfo tiffFieldInfo = this.m_fieldinfo[num3];
						bool flag3 = false;
						while (array[j].tdir_type != tiffFieldInfo.Type && num3 < this.m_nfields && tiffFieldInfo.Type != TiffType.NOTYPE)
						{
							tiffFieldInfo = this.m_fieldinfo[++num3];
							if (num3 >= this.m_nfields || tiffFieldInfo.Tag != array[j].tdir_tag)
							{
								Tiff.WarningExt(this, this.m_clientdata, "ReadDirectory", "{0}: wrong data type {1} for \"{2}\"; tag ignored", new object[]
								{
									this.m_name,
									array[j].tdir_type,
									this.m_fieldinfo[num3 - 1].Name
								});
								array[j].tdir_tag = TiffTag.IGNORE;
								flag3 = true;
								break;
							}
						}
						if (!flag3)
						{
							if (tiffFieldInfo.ReadCount != -1 && tiffFieldInfo.ReadCount != -3)
							{
								int count = (int)tiffFieldInfo.ReadCount;
								if (tiffFieldInfo.ReadCount == -2)
								{
									count = (int)this.m_dir.td_samplesperpixel;
								}
								if (!this.checkDirCount(array[j], count))
								{
									array[j].tdir_tag = TiffTag.IGNORE;
									goto IL_511;
								}
							}
							TiffTag tdir_tag = array[j].tdir_tag;
							if (tdir_tag <= TiffTag.STRIPBYTECOUNTS)
							{
								switch (tdir_tag)
								{
								case TiffTag.IMAGEWIDTH:
								case TiffTag.IMAGELENGTH:
									goto IL_4F9;
								case TiffTag.BITSPERSAMPLE:
									goto IL_511;
								case TiffTag.COMPRESSION:
								{
									if (array[j].tdir_count != 1)
									{
										short num5;
										if (array[j].tdir_type == TiffType.LONG)
										{
											int num4;
											if (!this.fetchPerSampleLongs(array[j], out num4) || !this.SetField(array[j].tdir_tag, new object[] { num4 }))
											{
												return false;
											}
										}
										else if (!this.fetchPerSampleShorts(array[j], out num5) || !this.SetField(array[j].tdir_tag, new object[] { num5 }))
										{
											return false;
										}
										array[j].tdir_tag = TiffTag.IGNORE;
										goto IL_511;
									}
									int num6 = this.extractData(array[j]);
									if (!this.SetField(array[j].tdir_tag, new object[] { num6 }))
									{
										return false;
									}
									goto IL_511;
								}
								default:
									if (tdir_tag != TiffTag.STRIPOFFSETS)
									{
										switch (tdir_tag)
										{
										case TiffTag.ROWSPERSTRIP:
											goto IL_4F9;
										case TiffTag.STRIPBYTECOUNTS:
											break;
										default:
											goto IL_511;
										}
									}
									break;
								}
							}
							else if (tdir_tag <= TiffTag.TILEBYTECOUNTS)
							{
								if (tdir_tag == TiffTag.PLANARCONFIG)
								{
									goto IL_4F9;
								}
								switch (tdir_tag)
								{
								case TiffTag.TILEWIDTH:
								case TiffTag.TILELENGTH:
									goto IL_4F9;
								case TiffTag.TILEOFFSETS:
								case TiffTag.TILEBYTECOUNTS:
									break;
								default:
									goto IL_511;
								}
							}
							else
							{
								if (tdir_tag == TiffTag.EXTRASAMPLES)
								{
									goto IL_4F9;
								}
								switch (tdir_tag)
								{
								case TiffTag.IMAGEDEPTH:
								case TiffTag.TILEDEPTH:
									goto IL_4F9;
								default:
									goto IL_511;
								}
							}
							this.setFieldBit((int)tiffFieldInfo.Bit);
							goto IL_511;
							IL_4F9:
							if (!this.fetchNormalTag(array[j]))
							{
								return false;
							}
							array[j].tdir_tag = TiffTag.IGNORE;
						}
					}
				}
				IL_511:;
			}
			if (flag2)
			{
				num3 = 0;
				for (int k = 0; k < (int)num; k++)
				{
					if (array[k].tdir_tag != TiffTag.IGNORE)
					{
						if (num3 >= this.m_nfields || array[k].tdir_tag < this.m_fieldinfo[num3].Tag)
						{
							num3 = 0;
						}
						while (num3 < this.m_nfields && this.m_fieldinfo[num3].Tag < array[k].tdir_tag)
						{
							num3++;
						}
						if (num3 >= this.m_nfields || this.m_fieldinfo[num3].Tag != array[k].tdir_tag)
						{
							Tiff.WarningExt(this, this.m_clientdata, "ReadDirectory", "{0}: unknown field with tag {1} (0x{2:x}) encountered", new object[]
							{
								this.m_name,
								(ushort)array[k].tdir_tag,
								(ushort)array[k].tdir_tag
							});
							this.MergeFieldInfo(new TiffFieldInfo[] { Tiff.createAnonFieldInfo(array[k].tdir_tag, array[k].tdir_type) }, 1);
							num3 = 0;
							while (num3 < this.m_nfields && this.m_fieldinfo[num3].Tag < array[k].tdir_tag)
							{
								num3++;
							}
						}
						TiffFieldInfo tiffFieldInfo2 = this.m_fieldinfo[num3];
						while (array[k].tdir_type != tiffFieldInfo2.Type && num3 < this.m_nfields && tiffFieldInfo2.Type != TiffType.NOTYPE)
						{
							tiffFieldInfo2 = this.m_fieldinfo[++num3];
							if (num3 >= this.m_nfields || tiffFieldInfo2.Tag != array[k].tdir_tag)
							{
								Tiff.WarningExt(this, this.m_clientdata, "ReadDirectory", "{0}: wrong data type {1} for \"{2}\"; tag ignored", new object[]
								{
									this.m_name,
									array[k].tdir_type,
									this.m_fieldinfo[num3 - 1].Name
								});
								array[k].tdir_tag = TiffTag.IGNORE;
								break;
							}
						}
					}
				}
			}
			if (this.m_dir.td_compression == Compression.OJPEG && this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
			{
				int num7 = Tiff.readDirectoryFind(array, num, TiffTag.STRIPOFFSETS);
				if (num7 != -1 && array[num7].tdir_count == 1)
				{
					num7 = Tiff.readDirectoryFind(array, num, TiffTag.STRIPBYTECOUNTS);
					if (num7 != -1 && array[num7].tdir_count == 1)
					{
						this.m_dir.td_planarconfig = PlanarConfig.CONTIG;
						Tiff.WarningExt(this, this.m_clientdata, "ReadDirectory", "Planarconfig tag value assumed incorrect, assuming data is contig instead of chunky", new object[0]);
					}
				}
			}
			if (!this.fieldSet(1))
			{
				this.missingRequired("ImageLength");
				return false;
			}
			if (!this.fieldSet(2))
			{
				this.m_dir.td_nstrips = this.NumberOfStrips();
				this.m_dir.td_tilewidth = this.m_dir.td_imagewidth;
				this.m_dir.td_tilelength = this.m_dir.td_rowsperstrip;
				this.m_dir.td_tiledepth = this.m_dir.td_imagedepth;
				this.m_flags &= ~TiffFlags.ISTILED;
			}
			else
			{
				this.m_dir.td_nstrips = this.NumberOfTiles();
				this.m_flags |= TiffFlags.ISTILED;
			}
			if (this.m_dir.td_nstrips == 0)
			{
				Tiff.ErrorExt(this, this.m_clientdata, "ReadDirectory", "{0}: cannot handle zero number of {1}", new object[]
				{
					this.m_name,
					this.IsTiled() ? "tiles" : "strips"
				});
				return false;
			}
			this.m_dir.td_stripsperimage = this.m_dir.td_nstrips;
			if (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
			{
				this.m_dir.td_stripsperimage /= (int)this.m_dir.td_samplesperpixel;
			}
			if (!this.fieldSet(25))
			{
				if (this.m_dir.td_compression != Compression.OJPEG || this.IsTiled() || this.m_dir.td_nstrips != 1)
				{
					this.missingRequired(this.IsTiled() ? "TileOffsets" : "StripOffsets");
					return false;
				}
				this.setFieldBit(25);
			}
			for (int l = 0; l < (int)num; l++)
			{
				if (array[l].tdir_tag != TiffTag.IGNORE)
				{
					TiffTag tdir_tag2 = array[l].tdir_tag;
					if (tdir_tag2 <= TiffTag.PAGENUMBER)
					{
						if (tdir_tag2 <= TiffTag.BITSPERSAMPLE)
						{
							if (tdir_tag2 != TiffTag.OSUBFILETYPE)
							{
								if (tdir_tag2 != TiffTag.BITSPERSAMPLE)
								{
									goto IL_D3A;
								}
							}
							else
							{
								FileType fileType = (FileType)0;
								switch (this.extractData(array[l]))
								{
								case 2:
									fileType = FileType.REDUCEDIMAGE;
									break;
								case 3:
									fileType = FileType.PAGE;
									break;
								}
								if (fileType != (FileType)0)
								{
									this.SetField(TiffTag.SUBFILETYPE, new object[] { fileType });
									goto IL_D45;
								}
								goto IL_D45;
							}
						}
						else
						{
							if (tdir_tag2 == TiffTag.STRIPOFFSETS)
							{
								goto IL_B5B;
							}
							switch (tdir_tag2)
							{
							case TiffTag.STRIPBYTECOUNTS:
								goto IL_B82;
							case TiffTag.MINSAMPLEVALUE:
							case TiffTag.MAXSAMPLEVALUE:
								break;
							default:
								if (tdir_tag2 != TiffTag.PAGENUMBER)
								{
									goto IL_D3A;
								}
								goto IL_CCF;
							}
						}
					}
					else if (tdir_tag2 <= TiffTag.TILEBYTECOUNTS)
					{
						if (tdir_tag2 != TiffTag.TRANSFERFUNCTION)
						{
							switch (tdir_tag2)
							{
							case TiffTag.COLORMAP:
								break;
							case TiffTag.HALFTONEHINTS:
								goto IL_CCF;
							case TiffTag.TILEWIDTH:
							case TiffTag.TILELENGTH:
								goto IL_D3A;
							case TiffTag.TILEOFFSETS:
								goto IL_B5B;
							case TiffTag.TILEBYTECOUNTS:
								goto IL_B82;
							default:
								goto IL_D3A;
							}
						}
						int num8 = 1 << (int)this.m_dir.td_bitspersample;
						if ((array[l].tdir_tag == TiffTag.COLORMAP || array[l].tdir_count != num8) && !this.checkDirCount(array[l], 3 * num8))
						{
							goto IL_D45;
						}
						byte[] buffer = new byte[array[l].tdir_count * 2];
						if (this.fetchData(array[l], buffer) == 0)
						{
							goto IL_D45;
						}
						int num9 = 1 << (int)this.m_dir.td_bitspersample;
						if (array[l].tdir_count == num9)
						{
							short[] array2 = Tiff.ByteArrayToShorts(buffer, 0, array[l].tdir_count * 2);
							this.SetField(array[l].tdir_tag, new object[] { array2, array2, array2 });
							goto IL_D45;
						}
						num8 *= 2;
						short[] array3 = Tiff.ByteArrayToShorts(buffer, 0, num8);
						short[] array4 = Tiff.ByteArrayToShorts(buffer, num8, num8);
						short[] array5 = Tiff.ByteArrayToShorts(buffer, 2 * num8, num8);
						this.SetField(array[l].tdir_tag, new object[] { array3, array4, array5 });
						goto IL_D45;
					}
					else
					{
						switch (tdir_tag2)
						{
						case TiffTag.DOTRANGE:
							goto IL_CCF;
						case TiffTag.TARGETPRINTER:
						case TiffTag.EXTRASAMPLES:
							goto IL_D3A;
						case TiffTag.SAMPLEFORMAT:
							break;
						case TiffTag.SMINSAMPLEVALUE:
						case TiffTag.SMAXSAMPLEVALUE:
						{
							double num10;
							if (!this.fetchPerSampleAnys(array[l], out num10) || !this.SetField(array[l].tdir_tag, new object[] { num10 }))
							{
								return false;
							}
							goto IL_D45;
						}
						default:
							switch (tdir_tag2)
							{
							case TiffTag.YCBCRSUBSAMPLING:
								goto IL_CCF;
							case TiffTag.YCBCRPOSITIONING:
								goto IL_D3A;
							case TiffTag.REFERENCEBLACKWHITE:
								this.fetchRefBlackWhite(array[l]);
								goto IL_D45;
							default:
								if (tdir_tag2 != TiffTag.DATATYPE)
								{
									goto IL_D3A;
								}
								break;
							}
							break;
						}
					}
					if (array[l].tdir_count == 1)
					{
						int num11 = this.extractData(array[l]);
						if (!this.SetField(array[l].tdir_tag, new object[] { num11 }))
						{
							return false;
						}
						goto IL_D45;
					}
					else if (array[l].tdir_tag == TiffTag.BITSPERSAMPLE && array[l].tdir_type == TiffType.LONG)
					{
						int num12;
						if (!this.fetchPerSampleLongs(array[l], out num12) || !this.SetField(array[l].tdir_tag, new object[] { num12 }))
						{
							return false;
						}
						goto IL_D45;
					}
					else
					{
						short num13;
						if (!this.fetchPerSampleShorts(array[l], out num13) || !this.SetField(array[l].tdir_tag, new object[] { num13 }))
						{
							return false;
						}
						goto IL_D45;
					}
					IL_B5B:
					if (!this.fetchStripThing(array[l], this.m_dir.td_nstrips, ref this.m_dir.td_stripoffset))
					{
						return false;
					}
					goto IL_D45;
					IL_B82:
					if (!this.fetchStripThing(array[l], this.m_dir.td_nstrips, ref this.m_dir.td_stripbytecount))
					{
						return false;
					}
					goto IL_D45;
					IL_CCF:
					this.fetchShortPair(array[l]);
					goto IL_D45;
					IL_D3A:
					this.fetchNormalTag(array[l]);
				}
				IL_D45:;
			}
			if (this.m_dir.td_compression == Compression.OJPEG)
			{
				if (!this.fieldSet(8))
				{
					Tiff.WarningExt(this, this.m_clientdata, "ReadDirectory", "Photometric tag is missing, assuming data is YCbCr", new object[0]);
					if (!this.SetField(TiffTag.PHOTOMETRIC, new object[] { Photometric.YCBCR }))
					{
						return false;
					}
				}
				else if (this.m_dir.td_photometric == Photometric.RGB)
				{
					this.m_dir.td_photometric = Photometric.YCBCR;
					Tiff.WarningExt(this, this.m_clientdata, "ReadDirectory", "Photometric tag value assumed incorrect, assuming data is YCbCr instead of RGB", new object[0]);
				}
				if (!this.fieldSet(6))
				{
					Tiff.WarningExt(this, this.m_clientdata, "ReadDirectory", "BitsPerSample tag is missing, assuming 8 bits per sample", new object[0]);
					if (!this.SetField(TiffTag.BITSPERSAMPLE, new object[] { 8 }))
					{
						return false;
					}
				}
				if (!this.fieldSet(16))
				{
					if (this.m_dir.td_photometric == Photometric.RGB || this.m_dir.td_photometric == Photometric.YCBCR)
					{
						Tiff.WarningExt(this, this.m_clientdata, "ReadDirectory", "SamplesPerPixel tag is missing, assuming correct SamplesPerPixel value is 3", new object[0]);
						if (!this.SetField(TiffTag.SAMPLESPERPIXEL, new object[] { 3 }))
						{
							return false;
						}
					}
					else if (this.m_dir.td_photometric == Photometric.MINISWHITE || this.m_dir.td_photometric == Photometric.MINISBLACK)
					{
						Tiff.WarningExt(this, this.m_clientdata, "ReadDirectory", "SamplesPerPixel tag is missing, assuming correct SamplesPerPixel value is 1", new object[0]);
						if (!this.SetField(TiffTag.SAMPLESPERPIXEL, new object[] { 1 }))
						{
							return false;
						}
					}
				}
			}
			if (this.m_dir.td_photometric == Photometric.PALETTE && !this.fieldSet(26))
			{
				this.missingRequired("Colormap");
				return false;
			}
			if (this.m_dir.td_compression != Compression.OJPEG)
			{
				if (!this.fieldSet(24))
				{
					if ((this.m_dir.td_planarconfig == PlanarConfig.CONTIG && this.m_dir.td_nstrips > 1) || (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE && this.m_dir.td_nstrips != (int)this.m_dir.td_samplesperpixel))
					{
						this.missingRequired("StripByteCounts");
						return false;
					}
					Tiff.WarningExt(this, this.m_clientdata, "ReadDirectory", "{0}: TIFF directory is missing required \"{1}\" field, calculating from imagelength", new object[]
					{
						this.m_name,
						this.FieldWithTag(TiffTag.STRIPBYTECOUNTS).Name
					});
					if (!this.estimateStripByteCounts(array, num))
					{
						return false;
					}
				}
				else if (this.m_dir.td_nstrips == 1 && this.m_dir.td_stripoffset[0] != 0U && this.byteCountLooksBad(this.m_dir))
				{
					Tiff.WarningExt(this, this.m_clientdata, "ReadDirectory", "{0}: Bogus \"{1}\" field, ignoring and calculating from imagelength", new object[]
					{
						this.m_name,
						this.FieldWithTag(TiffTag.STRIPBYTECOUNTS).Name
					});
					if (!this.estimateStripByteCounts(array, num))
					{
						return false;
					}
				}
				else if (this.m_dir.td_planarconfig == PlanarConfig.CONTIG && this.m_dir.td_nstrips > 2 && this.m_dir.td_compression == Compression.NONE && this.m_dir.td_stripbytecount[0] != this.m_dir.td_stripbytecount[1])
				{
					Tiff.WarningExt(this, this.m_clientdata, "ReadDirectory", "{0}: Wrong \"{1}\" field, ignoring and calculating from imagelength", new object[]
					{
						this.m_name,
						this.FieldWithTag(TiffTag.STRIPBYTECOUNTS).Name
					});
					if (!this.estimateStripByteCounts(array, num))
					{
						return false;
					}
				}
			}
			array = null;
			if (!this.fieldSet(19))
			{
				this.m_dir.td_maxsamplevalue = (short)((1 << (int)this.m_dir.td_bitspersample) - 1);
			}
			if (this.m_dir.td_nstrips > 1)
			{
				this.m_dir.td_stripbytecountsorted = true;
				for (int m = 1; m < this.m_dir.td_nstrips; m++)
				{
					if (this.m_dir.td_stripoffset[m - 1] > this.m_dir.td_stripoffset[m])
					{
						this.m_dir.td_stripbytecountsorted = false;
						break;
					}
				}
			}
			if (!this.fieldSet(7))
			{
				this.SetField(TiffTag.COMPRESSION, new object[] { Compression.NONE });
			}
			if (this.m_dir.td_nstrips == 1 && this.m_dir.td_compression == Compression.NONE && (this.m_flags & TiffFlags.STRIPCHOP) == TiffFlags.STRIPCHOP && (this.m_flags & TiffFlags.ISTILED) != TiffFlags.ISTILED)
			{
				this.chopUpSingleUncompressedStrip();
			}
			this.m_row = -1;
			this.m_curstrip = -1;
			this.m_col = -1;
			this.m_curtile = -1;
			this.m_tilesize = -1;
			this.m_scanlinesize = this.ScanlineSize();
			if (this.m_scanlinesize == 0)
			{
				Tiff.ErrorExt(this, this.m_clientdata, "ReadDirectory", "{0}: cannot handle zero scanline size", new object[] { this.m_name });
				return false;
			}
			if (this.IsTiled())
			{
				this.m_tilesize = this.TileSize();
				if (this.m_tilesize == 0)
				{
					Tiff.ErrorExt(this, this.m_clientdata, "ReadDirectory", "{0}: cannot handle zero tile size", new object[] { this.m_name });
					return false;
				}
			}
			else if (this.StripSize() == 0)
			{
				Tiff.ErrorExt(this, this.m_clientdata, "ReadDirectory", "{0}: cannot handle zero strip size", new object[] { this.m_name });
				return false;
			}
			return true;
		}

		public bool ReadCustomDirectory(long offset, TiffFieldInfo[] info, int count)
		{
			this.setupFieldInfo(info, count);
			TiffDirEntry[] array;
			uint num2;
			short num = this.fetchDirectory((uint)offset, out array, out num2);
			if (num == 0)
			{
				Tiff.ErrorExt(this, this.m_clientdata, "ReadCustomDirectory", "{0}: Failed to read custom directory at offset {1}", new object[] { this.m_name, offset });
				return false;
			}
			this.FreeDirectory();
			this.m_dir = new TiffDirectory();
			int num3 = 0;
			for (short num4 = 0; num4 < num; num4 += 1)
			{
				if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
				{
					short num5 = (short)array[(int)num4].tdir_tag;
					Tiff.SwabShort(ref num5);
					array[(int)num4].tdir_tag = (TiffTag)((ushort)num5);
					num5 = (short)array[(int)num4].tdir_type;
					Tiff.SwabShort(ref num5);
					array[(int)num4].tdir_type = (TiffType)num5;
					Tiff.SwabLong(ref array[(int)num4].tdir_count);
					Tiff.SwabUInt(ref array[(int)num4].tdir_offset);
				}
				if (num3 < this.m_nfields)
				{
					if (array[(int)num4].tdir_tag != TiffTag.IGNORE)
					{
						while (num3 < this.m_nfields && this.m_fieldinfo[num3].Tag < array[(int)num4].tdir_tag)
						{
							num3++;
						}
						if (num3 >= this.m_nfields || this.m_fieldinfo[num3].Tag != array[(int)num4].tdir_tag)
						{
							Tiff.WarningExt(this, this.m_clientdata, "ReadCustomDirectory", "{0}: unknown field with tag {1} (0x{2:x}) encountered", new object[]
							{
								this.m_name,
								(ushort)array[(int)num4].tdir_tag,
								(ushort)array[(int)num4].tdir_tag
							});
							this.MergeFieldInfo(new TiffFieldInfo[] { Tiff.createAnonFieldInfo(array[(int)num4].tdir_tag, array[(int)num4].tdir_type) }, 1);
							num3 = 0;
							while (num3 < this.m_nfields && this.m_fieldinfo[num3].Tag < array[(int)num4].tdir_tag)
							{
								num3++;
							}
						}
						if (this.m_fieldinfo[num3].Bit == 0)
						{
							array[(int)num4].tdir_tag = TiffTag.IGNORE;
						}
						else
						{
							TiffFieldInfo tiffFieldInfo = this.m_fieldinfo[num3];
							while (array[(int)num4].tdir_type != tiffFieldInfo.Type && num3 < this.m_nfields && tiffFieldInfo.Type != TiffType.NOTYPE)
							{
								tiffFieldInfo = this.m_fieldinfo[++num3];
								if (num3 >= this.m_nfields || tiffFieldInfo.Tag != array[(int)num4].tdir_tag)
								{
									Tiff.WarningExt(this, this.m_clientdata, "ReadCustomDirectory", "{0}: wrong data type {1} for \"{2}\"; tag ignored", new object[]
									{
										this.m_name,
										array[(int)num4].tdir_type,
										this.m_fieldinfo[num3 - 1].Name
									});
									array[(int)num4].tdir_tag = TiffTag.IGNORE;
								}
							}
							if (tiffFieldInfo.ReadCount != -1 && tiffFieldInfo.ReadCount != -3)
							{
								int count2 = (int)tiffFieldInfo.ReadCount;
								if (tiffFieldInfo.ReadCount == -2)
								{
									count2 = (int)this.m_dir.td_samplesperpixel;
								}
								if (!this.checkDirCount(array[(int)num4], count2))
								{
									array[(int)num4].tdir_tag = TiffTag.IGNORE;
									goto IL_333;
								}
							}
							TiffTag tdir_tag = array[(int)num4].tdir_tag;
							if (tdir_tag == TiffTag.EXIF_SUBJECTDISTANCE)
							{
								this.fetchSubjectDistance(array[(int)num4]);
							}
							else
							{
								this.fetchNormalTag(array[(int)num4]);
							}
						}
					}
				}
				IL_333:;
			}
			return true;
		}

		public bool ReadEXIFDirectory(long offset)
		{
			int count;
			TiffFieldInfo[] info = Tiff.getExifFieldInfo(out count);
			return this.ReadCustomDirectory(offset, info, count);
		}

		public int ScanlineSize()
		{
			int num2;
			if (this.m_dir.td_planarconfig == PlanarConfig.CONTIG)
			{
				if (this.m_dir.td_photometric == Photometric.YCBCR && !this.IsUpSampled())
				{
					FieldValue[] fieldDefaulted = this.GetFieldDefaulted(TiffTag.YCBCRSUBSAMPLING);
					short num = fieldDefaulted[0].ToShort();
					if (num == 0)
					{
						Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Invalid YCbCr subsampling", new object[0]);
						return 0;
					}
					num2 = Tiff.roundUp(this.m_dir.td_imagewidth, (int)num);
					num2 = Tiff.howMany8(this.multiply(num2, (int)this.m_dir.td_bitspersample, "ScanlineSize"));
					return this.summarize(num2, this.multiply(2, num2 / (int)num, "VStripSize"), "VStripSize");
				}
				else
				{
					num2 = this.multiply(this.m_dir.td_imagewidth, (int)this.m_dir.td_samplesperpixel, "ScanlineSize");
				}
			}
			else
			{
				num2 = this.m_dir.td_imagewidth;
			}
			return Tiff.howMany8(this.multiply(num2, (int)this.m_dir.td_bitspersample, "ScanlineSize"));
		}

		public int RasterScanlineSize()
		{
			int num = this.multiply((int)this.m_dir.td_bitspersample, this.m_dir.td_imagewidth, "RasterScanlineSize");
			if (this.m_dir.td_planarconfig == PlanarConfig.CONTIG)
			{
				num = this.multiply(num, (int)this.m_dir.td_samplesperpixel, "RasterScanlineSize");
				return Tiff.howMany8(num);
			}
			return this.multiply(Tiff.howMany8(num), (int)this.m_dir.td_samplesperpixel, "RasterScanlineSize");
		}

		public int DefaultStripSize(int estimate)
		{
			return this.m_currentCodec.DefStripSize(estimate);
		}

		public int StripSize()
		{
			int num = this.m_dir.td_rowsperstrip;
			if (num > this.m_dir.td_imagelength)
			{
				num = this.m_dir.td_imagelength;
			}
			return this.VStripSize(num);
		}

		public int VStripSize(int rowCount)
		{
			if (rowCount == -1)
			{
				rowCount = this.m_dir.td_imagelength;
			}
			if (this.m_dir.td_planarconfig != PlanarConfig.CONTIG || this.m_dir.td_photometric != Photometric.YCBCR || this.IsUpSampled())
			{
				return this.multiply(rowCount, this.ScanlineSize(), "VStripSize");
			}
			FieldValue[] fieldDefaulted = this.GetFieldDefaulted(TiffTag.YCBCRSUBSAMPLING);
			short num = fieldDefaulted[0].ToShort();
			short num2 = fieldDefaulted[1].ToShort();
			int num3 = (int)(num * num2);
			if (num3 == 0)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Invalid YCbCr subsampling", new object[0]);
				return 0;
			}
			int nmemb = Tiff.roundUp(this.m_dir.td_imagewidth, (int)num);
			int num4 = Tiff.howMany8(this.multiply(nmemb, (int)this.m_dir.td_bitspersample, "VStripSize"));
			rowCount = Tiff.roundUp(rowCount, (int)num2);
			num4 = this.multiply(rowCount, num4, "VStripSize");
			return this.summarize(num4, this.multiply(2, num4 / num3, "VStripSize"), "VStripSize");
		}

		public long RawStripSize(int strip)
		{
			long num = (long)((ulong)this.m_dir.td_stripbytecount[strip]);
			if (num <= 0L)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "{0}: Invalid strip byte count, strip {1}", new object[] { num, strip });
				num = -1L;
			}
			return num;
		}

		public int ComputeStrip(int row, short plane)
		{
			int num = 0;
			if (this.m_dir.td_rowsperstrip != -1)
			{
				num = row / this.m_dir.td_rowsperstrip;
			}
			if (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
			{
				if (plane >= this.m_dir.td_samplesperpixel)
				{
					Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "{0}: Sample out of range, max {1}", new object[]
					{
						plane,
						this.m_dir.td_samplesperpixel
					});
					return 0;
				}
				num += (int)plane * this.m_dir.td_stripsperimage;
			}
			return num;
		}

		public int NumberOfStrips()
		{
			int num = ((this.m_dir.td_rowsperstrip == -1) ? 1 : Tiff.howMany(this.m_dir.td_imagelength, this.m_dir.td_rowsperstrip));
			if (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
			{
				num = this.multiply(num, (int)this.m_dir.td_samplesperpixel, "NumberOfStrips");
			}
			return num;
		}

		public void DefaultTileSize(ref int width, ref int height)
		{
			this.m_currentCodec.DefTileSize(ref width, ref height);
		}

		public int TileSize()
		{
			return this.VTileSize(this.m_dir.td_tilelength);
		}

		public int VTileSize(int rowCount)
		{
			if (this.m_dir.td_tilelength == 0 || this.m_dir.td_tilewidth == 0 || this.m_dir.td_tiledepth == 0)
			{
				return 0;
			}
			int num2;
			if (this.m_dir.td_planarconfig == PlanarConfig.CONTIG && this.m_dir.td_photometric == Photometric.YCBCR && !this.IsUpSampled())
			{
				int nmemb = Tiff.roundUp(this.m_dir.td_tilewidth, (int)this.m_dir.td_ycbcrsubsampling[0]);
				int elem_size = Tiff.howMany8(this.multiply(nmemb, (int)this.m_dir.td_bitspersample, "VTileSize"));
				int num = (int)(this.m_dir.td_ycbcrsubsampling[0] * this.m_dir.td_ycbcrsubsampling[1]);
				if (num == 0)
				{
					Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Invalid YCbCr subsampling", new object[0]);
					return 0;
				}
				rowCount = Tiff.roundUp(rowCount, (int)this.m_dir.td_ycbcrsubsampling[1]);
				num2 = this.multiply(rowCount, elem_size, "VTileSize");
				num2 = this.summarize(num2, this.multiply(2, num2 / num, "VTileSize"), "VTileSize");
			}
			else
			{
				num2 = this.multiply(rowCount, this.TileRowSize(), "VTileSize");
			}
			return this.multiply(num2, this.m_dir.td_tiledepth, "VTileSize");
		}

		public long RawTileSize(int tile)
		{
			return this.RawStripSize(tile);
		}

		public int TileRowSize()
		{
			if (this.m_dir.td_tilelength == 0 || this.m_dir.td_tilewidth == 0)
			{
				return 0;
			}
			int num = this.multiply((int)this.m_dir.td_bitspersample, this.m_dir.td_tilewidth, "TileRowSize");
			if (this.m_dir.td_planarconfig == PlanarConfig.CONTIG)
			{
				num = this.multiply(num, (int)this.m_dir.td_samplesperpixel, "TileRowSize");
			}
			return Tiff.howMany8(num);
		}

		public int ComputeTile(int x, int y, int z, short plane)
		{
			if (this.m_dir.td_imagedepth == 1)
			{
				z = 0;
			}
			int num = this.m_dir.td_tilewidth;
			if (num == -1)
			{
				num = this.m_dir.td_imagewidth;
			}
			int num2 = this.m_dir.td_tilelength;
			if (num2 == -1)
			{
				num2 = this.m_dir.td_imagelength;
			}
			int num3 = this.m_dir.td_tiledepth;
			if (num3 == -1)
			{
				num3 = this.m_dir.td_imagedepth;
			}
			int result = 1;
			if (num != 0 && num2 != 0 && num3 != 0)
			{
				int num4 = Tiff.howMany(this.m_dir.td_imagewidth, num);
				int num5 = Tiff.howMany(this.m_dir.td_imagelength, num2);
				int num6 = Tiff.howMany(this.m_dir.td_imagedepth, num3);
				if (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
				{
					result = num4 * num5 * num6 * (int)plane + num4 * num5 * (z / num3) + num4 * (y / num2) + x / num;
				}
				else
				{
					result = num4 * num5 * (z / num3) + num4 * (y / num2) + x / num;
				}
			}
			return result;
		}

		public bool CheckTile(int x, int y, int z, short plane)
		{
			if (x >= this.m_dir.td_imagewidth)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "{0}: Col out of range, max {1}", new object[]
				{
					x,
					this.m_dir.td_imagewidth - 1
				});
				return false;
			}
			if (y >= this.m_dir.td_imagelength)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "{0}: Row out of range, max {1}", new object[]
				{
					y,
					this.m_dir.td_imagelength - 1
				});
				return false;
			}
			if (z >= this.m_dir.td_imagedepth)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "{0}: Depth out of range, max {1}", new object[]
				{
					z,
					this.m_dir.td_imagedepth - 1
				});
				return false;
			}
			if (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE && plane >= this.m_dir.td_samplesperpixel)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "{0}: Sample out of range, max {1}", new object[]
				{
					plane,
					(int)(this.m_dir.td_samplesperpixel - 1)
				});
				return false;
			}
			return true;
		}

		public int NumberOfTiles()
		{
			int num = this.m_dir.td_tilewidth;
			if (num == -1)
			{
				num = this.m_dir.td_imagewidth;
			}
			int num2 = this.m_dir.td_tilelength;
			if (num2 == -1)
			{
				num2 = this.m_dir.td_imagelength;
			}
			int num3 = this.m_dir.td_tiledepth;
			if (num3 == -1)
			{
				num3 = this.m_dir.td_imagedepth;
			}
			int num4;
			if (num == 0 || num2 == 0 || num3 == 0)
			{
				num4 = 0;
			}
			else
			{
				num4 = this.multiply(this.multiply(Tiff.howMany(this.m_dir.td_imagewidth, num), Tiff.howMany(this.m_dir.td_imagelength, num2), "NumberOfTiles"), Tiff.howMany(this.m_dir.td_imagedepth, num3), "NumberOfTiles");
			}
			if (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
			{
				num4 = this.multiply(num4, (int)this.m_dir.td_samplesperpixel, "NumberOfTiles");
			}
			return num4;
		}

		public object Clientdata()
		{
			return this.m_clientdata;
		}

		public object SetClientdata(object data)
		{
			object clientdata = this.m_clientdata;
			this.m_clientdata = data;
			return clientdata;
		}

		public int GetMode()
		{
			return this.m_mode;
		}

		public int SetMode(int mode)
		{
			int mode2 = this.m_mode;
			this.m_mode = mode;
			return mode2;
		}

		public bool IsTiled()
		{
			return (this.m_flags & TiffFlags.ISTILED) == TiffFlags.ISTILED;
		}

		public bool IsByteSwapped()
		{
			return (this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB;
		}

		public bool IsUpSampled()
		{
			return (this.m_flags & TiffFlags.UPSAMPLED) == TiffFlags.UPSAMPLED;
		}

		public bool IsMSB2LSB()
		{
			return this.isFillOrder(FillOrder.MSB2LSB);
		}

		public bool IsBigEndian()
		{
			return this.m_header.tiff_magic == 19789;
		}

		public TiffStream GetStream()
		{
			return this.m_stream;
		}

		public int CurrentRow()
		{
			return this.m_row;
		}

		public short CurrentDirectory()
		{
			return this.m_curdir;
		}

		public short NumberOfDirectories()
		{
			uint tiff_diroff = this.m_header.tiff_diroff;
			short num = 0;
			long num2;
			while (tiff_diroff != 0U && this.advanceDirectory(ref tiff_diroff, out num2))
			{
				num += 1;
			}
			return num;
		}

		public long CurrentDirOffset()
		{
			return (long)((ulong)this.m_diroff);
		}

		public int CurrentStrip()
		{
			return this.m_curstrip;
		}

		public int CurrentTile()
		{
			return this.m_curtile;
		}

		public void ReadBufferSetup(byte[] buffer, int size)
		{
			this.m_rawdata = null;
			if (buffer != null)
			{
				this.m_rawdatasize = size;
				this.m_rawdata = buffer;
				this.m_flags &= ~TiffFlags.MYBUFFER;
				return;
			}
			this.m_rawdatasize = Tiff.roundUp(size, 1024);
			if (this.m_rawdatasize > 0)
			{
				this.m_rawdata = new byte[this.m_rawdatasize];
			}
			else
			{
				Tiff.ErrorExt(this, this.m_clientdata, "ReadBufferSetup", "{0}: No space for data buffer at scanline {1}", new object[] { this.m_name, this.m_row });
				this.m_rawdatasize = 0;
			}
			this.m_flags |= TiffFlags.MYBUFFER;
		}

		public void WriteBufferSetup(byte[] buffer, int size)
		{
			if (this.m_rawdata != null)
			{
				if ((this.m_flags & TiffFlags.MYBUFFER) == TiffFlags.MYBUFFER)
				{
					this.m_flags &= ~TiffFlags.MYBUFFER;
				}
				this.m_rawdata = null;
			}
			if (size == -1)
			{
				size = (this.IsTiled() ? this.m_tilesize : this.StripSize());
				if (size < 8192)
				{
					size = 8192;
				}
				buffer = null;
			}
			if (buffer == null)
			{
				buffer = new byte[size];
				this.m_flags |= TiffFlags.MYBUFFER;
			}
			else
			{
				this.m_flags &= ~TiffFlags.MYBUFFER;
			}
			this.m_rawdata = buffer;
			this.m_rawdatasize = size;
			this.m_rawcc = 0;
			this.m_rawcp = 0;
			this.m_flags |= TiffFlags.BUFFERSETUP;
		}

		public bool SetupStrips()
		{
			if (this.IsTiled())
			{
				this.m_dir.td_stripsperimage = (this.isUnspecified(2) ? ((int)this.m_dir.td_samplesperpixel) : this.NumberOfTiles());
			}
			else
			{
				this.m_dir.td_stripsperimage = (this.isUnspecified(17) ? ((int)this.m_dir.td_samplesperpixel) : this.NumberOfStrips());
			}
			this.m_dir.td_nstrips = this.m_dir.td_stripsperimage;
			if (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
			{
				this.m_dir.td_stripsperimage /= (int)this.m_dir.td_samplesperpixel;
			}
			this.m_dir.td_stripoffset = new uint[this.m_dir.td_nstrips];
			this.m_dir.td_stripbytecount = new uint[this.m_dir.td_nstrips];
			this.setFieldBit(25);
			this.setFieldBit(24);
			return true;
		}

		public bool WriteCheck(bool tiles, string method)
		{
			if (this.m_mode == 0)
			{
				Tiff.ErrorExt(this, this.m_clientdata, method, "{0}: File not open for writing", new object[] { this.m_name });
				return false;
			}
			if (tiles ^ this.IsTiled())
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, tiles ? "Can not write tiles to a stripped image" : "Can not write scanlines to a tiled image", new object[0]);
				return false;
			}
			if (!this.fieldSet(1))
			{
				Tiff.ErrorExt(this, this.m_clientdata, method, "{0}: Must set \"ImageWidth\" before writing data", new object[] { this.m_name });
				return false;
			}
			if (this.m_dir.td_samplesperpixel == 1)
			{
				if (!this.fieldSet(20))
				{
					this.m_dir.td_planarconfig = PlanarConfig.CONTIG;
				}
			}
			else if (!this.fieldSet(20))
			{
				Tiff.ErrorExt(this, this.m_clientdata, method, "{0}: Must set \"PlanarConfiguration\" before writing data", new object[] { this.m_name });
				return false;
			}
			if (this.m_dir.td_stripoffset == null && !this.SetupStrips())
			{
				this.m_dir.td_nstrips = 0;
				Tiff.ErrorExt(this, this.m_clientdata, method, "{0}: No space for {1} arrays", new object[]
				{
					this.m_name,
					this.IsTiled() ? "tile" : "strip"
				});
				return false;
			}
			this.m_tilesize = (this.IsTiled() ? this.TileSize() : (-1));
			this.m_scanlinesize = this.ScanlineSize();
			this.m_flags |= TiffFlags.BEENWRITING;
			return true;
		}

		public void FreeDirectory()
		{
			if (this.m_dir != null)
			{
				this.clearFieldBit(39);
				this.clearFieldBit(40);
				this.m_dir = null;
			}
		}

		public void CreateDirectory()
		{
			this.setupDefaultDirectory();
			this.m_diroff = 0U;
			this.m_nextdiroff = 0U;
			this.m_curoff = 0U;
			this.m_row = -1;
			this.m_curstrip = -1;
		}

		public bool LastDirectory()
		{
			return this.m_nextdiroff == 0U;
		}

		public bool SetDirectory(short number)
		{
			uint tiff_diroff = this.m_header.tiff_diroff;
			short num = number;
			while (num > 0 && tiff_diroff != 0U)
			{
				long num2;
				if (!this.advanceDirectory(ref tiff_diroff, out num2))
				{
					return false;
				}
				num -= 1;
			}
			this.m_nextdiroff = tiff_diroff;
			this.m_curdir = number - num - 1;
			this.m_dirnumber = 0;
			return this.ReadDirectory();
		}

		public bool SetSubDirectory(long offset)
		{
			this.m_nextdiroff = (uint)offset;
			this.m_dirnumber = 0;
			return this.ReadDirectory();
		}

		public bool UnlinkDirectory(short number)
		{
			if (this.m_mode == 0)
			{
				Tiff.ErrorExt(this, this.m_clientdata, "UnlinkDirectory", "Can not unlink directory in read-only file", new object[0]);
				return false;
			}
			uint tiff_diroff = this.m_header.tiff_diroff;
			long off = 4L;
			for (int i = (int)(number - 1); i > 0; i--)
			{
				if (tiff_diroff == 0U)
				{
					Tiff.ErrorExt(this, this.m_clientdata, "UnlinkDirectory", "Directory {0} does not exist", new object[] { number });
					return false;
				}
				if (!this.advanceDirectory(ref tiff_diroff, out off))
				{
					return false;
				}
			}
			long num;
			if (!this.advanceDirectory(ref tiff_diroff, out num))
			{
				return false;
			}
			this.seekFile(off, SeekOrigin.Begin);
			if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
			{
				Tiff.SwabUInt(ref tiff_diroff);
			}
			if (!this.writeIntOK((int)tiff_diroff))
			{
				Tiff.ErrorExt(this, this.m_clientdata, "UnlinkDirectory", "Error writing directory link", new object[0]);
				return false;
			}
			this.m_currentCodec.Cleanup();
			if ((this.m_flags & TiffFlags.MYBUFFER) == TiffFlags.MYBUFFER && this.m_rawdata != null)
			{
				this.m_rawdata = null;
				this.m_rawcc = 0;
			}
			this.m_flags &= ~(TiffFlags.BUFFERSETUP | TiffFlags.BEENWRITING | TiffFlags.POSTENCODE);
			this.FreeDirectory();
			this.setupDefaultDirectory();
			this.m_diroff = 0U;
			this.m_nextdiroff = 0U;
			this.m_curoff = 0U;
			this.m_row = -1;
			this.m_curstrip = -1;
			return true;
		}

		public bool SetField(TiffTag tag, params object[] value)
		{
			return this.okToChangeTag(tag) && this.m_tagmethods.SetField(this, tag, FieldValue.FromParams(value));
		}

		public bool WriteDirectory()
		{
			return this.writeDirectory(true);
		}

		public bool CheckpointDirectory()
		{
			if (this.m_dir.td_stripoffset == null)
			{
				this.SetupStrips();
			}
			bool result = this.writeDirectory(false);
			this.SetWriteOffset(this.seekFile(0L, SeekOrigin.End));
			return result;
		}

		public bool RewriteDirectory()
		{
			if (this.m_diroff == 0U)
			{
				return this.WriteDirectory();
			}
			if (this.m_header.tiff_diroff != this.m_diroff)
			{
				uint tiff_diroff = this.m_header.tiff_diroff;
				short num;
				while (this.seekOK((long)((ulong)tiff_diroff)) && this.readShortOK(out num))
				{
					if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
					{
						Tiff.SwabShort(ref num);
					}
					this.seekFile((long)(num * 12), SeekOrigin.Current);
					if (!this.readUIntOK(out tiff_diroff))
					{
						Tiff.ErrorExt(this, this.m_clientdata, "RewriteDirectory", "Error fetching directory link", new object[0]);
						return false;
					}
					if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
					{
						Tiff.SwabUInt(ref tiff_diroff);
					}
					if (tiff_diroff == this.m_diroff || tiff_diroff == 0U)
					{
						long num2 = this.seekFile(0L, SeekOrigin.Current);
						this.seekFile(num2 - 4L, SeekOrigin.Begin);
						this.m_diroff = 0U;
						if (!this.writeIntOK((int)this.m_diroff))
						{
							Tiff.ErrorExt(this, this.m_clientdata, "RewriteDirectory", "Error writing directory link", new object[0]);
							return false;
						}
						goto IL_173;
					}
				}
				Tiff.ErrorExt(this, this.m_clientdata, "RewriteDirectory", "Error fetching directory count", new object[0]);
				return false;
			}
			this.m_header.tiff_diroff = 0U;
			this.m_diroff = 0U;
			this.seekFile(4L, SeekOrigin.Begin);
			if (!this.writeIntOK((int)this.m_header.tiff_diroff))
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Error updating TIFF header", new object[0]);
				return false;
			}
			IL_173:
			return this.WriteDirectory();
		}

		public void PrintDirectory(Stream stream)
		{
			this.PrintDirectory(stream, TiffPrintFlags.NONE);
		}

		public void PrintDirectory(Stream stream, TiffPrintFlags flags)
		{
			Tiff.fprintf(stream, "TIFF Directory at offset 0x{0:x} ({1})\r\n", new object[] { this.m_diroff, this.m_diroff });
			if (this.fieldSet(5))
			{
				Tiff.fprintf(stream, "  Subfile Type:", new object[0]);
				string text = " ";
				if ((this.m_dir.td_subfiletype & FileType.REDUCEDIMAGE) != (FileType)0)
				{
					Tiff.fprintf(stream, "{0}reduced-resolution image", new object[] { text });
					text = "/";
				}
				if ((this.m_dir.td_subfiletype & FileType.PAGE) != (FileType)0)
				{
					Tiff.fprintf(stream, "{0}multi-page document", new object[] { text });
					text = "/";
				}
				if ((this.m_dir.td_subfiletype & FileType.MASK) != (FileType)0)
				{
					Tiff.fprintf(stream, "{0}transparency mask", new object[] { text });
				}
				Tiff.fprintf(stream, " ({0} = 0x{1:x})\r\n", new object[]
				{
					this.m_dir.td_subfiletype,
					this.m_dir.td_subfiletype
				});
			}
			if (this.fieldSet(1))
			{
				Tiff.fprintf(stream, "  Image Width: {0} Image Length: {1}", new object[]
				{
					this.m_dir.td_imagewidth,
					this.m_dir.td_imagelength
				});
				if (this.fieldSet(35))
				{
					Tiff.fprintf(stream, " Image Depth: {0}", new object[] { this.m_dir.td_imagedepth });
				}
				Tiff.fprintf(stream, "\r\n", new object[0]);
			}
			if (this.fieldSet(2))
			{
				Tiff.fprintf(stream, "  Tile Width: {0} Tile Length: {1}", new object[]
				{
					this.m_dir.td_tilewidth,
					this.m_dir.td_tilelength
				});
				if (this.fieldSet(36))
				{
					Tiff.fprintf(stream, " Tile Depth: {0}", new object[] { this.m_dir.td_tiledepth });
				}
				Tiff.fprintf(stream, "\r\n", new object[0]);
			}
			if (this.fieldSet(3))
			{
				Tiff.fprintf(stream, "  Resolution: {0:G}, {1:G}", new object[]
				{
					this.m_dir.td_xresolution,
					this.m_dir.td_yresolution
				});
				if (this.fieldSet(22))
				{
					switch (this.m_dir.td_resolutionunit)
					{
					case ResUnit.NONE:
						Tiff.fprintf(stream, " (unitless)", new object[0]);
						break;
					case ResUnit.INCH:
						Tiff.fprintf(stream, " pixels/inch", new object[0]);
						break;
					case ResUnit.CENTIMETER:
						Tiff.fprintf(stream, " pixels/cm", new object[0]);
						break;
					default:
						Tiff.fprintf(stream, " (unit {0} = 0x{1:x})", new object[]
						{
							this.m_dir.td_resolutionunit,
							this.m_dir.td_resolutionunit
						});
						break;
					}
				}
				Tiff.fprintf(stream, "\r\n", new object[0]);
			}
			if (this.fieldSet(4))
			{
				Tiff.fprintf(stream, "  Position: {0:G}, {1:G}\r\n", new object[]
				{
					this.m_dir.td_xposition,
					this.m_dir.td_yposition
				});
			}
			if (this.fieldSet(6))
			{
				Tiff.fprintf(stream, "  Bits/Sample: {0}\r\n", new object[] { this.m_dir.td_bitspersample });
			}
			if (this.fieldSet(32))
			{
				Tiff.fprintf(stream, "  Sample Format: ", new object[0]);
				switch (this.m_dir.td_sampleformat)
				{
				case SampleFormat.UINT:
					Tiff.fprintf(stream, "unsigned integer\r\n", new object[0]);
					break;
				case SampleFormat.INT:
					Tiff.fprintf(stream, "signed integer\r\n", new object[0]);
					break;
				case SampleFormat.IEEEFP:
					Tiff.fprintf(stream, "IEEE floating point\r\n", new object[0]);
					break;
				case SampleFormat.VOID:
					Tiff.fprintf(stream, "void\r\n", new object[0]);
					break;
				case SampleFormat.COMPLEXINT:
					Tiff.fprintf(stream, "complex signed integer\r\n", new object[0]);
					break;
				case SampleFormat.COMPLEXIEEEFP:
					Tiff.fprintf(stream, "complex IEEE floating point\r\n", new object[0]);
					break;
				default:
					Tiff.fprintf(stream, "{0} (0x{1:x})\r\n", new object[]
					{
						this.m_dir.td_sampleformat,
						this.m_dir.td_sampleformat
					});
					break;
				}
			}
			if (this.fieldSet(7))
			{
				TiffCodec tiffCodec = this.FindCodec(this.m_dir.td_compression);
				Tiff.fprintf(stream, "  Compression Scheme: ", new object[0]);
				if (tiffCodec != null)
				{
					Tiff.fprintf(stream, "{0}\r\n", new object[] { tiffCodec.m_name });
				}
				else
				{
					Tiff.fprintf(stream, "{0} (0x{1:x})\r\n", new object[]
					{
						this.m_dir.td_compression,
						this.m_dir.td_compression
					});
				}
			}
			if (this.fieldSet(8))
			{
				Tiff.fprintf(stream, "  Photometric Interpretation: ", new object[0]);
				if (this.m_dir.td_photometric < (Photometric)Tiff.photoNames.Length)
				{
					Tiff.fprintf(stream, "{0}\r\n", new object[] { Tiff.photoNames[(int)this.m_dir.td_photometric] });
				}
				else
				{
					switch (this.m_dir.td_photometric)
					{
					case Photometric.LOGL:
						Tiff.fprintf(stream, "CIE Log2(L)\r\n", new object[0]);
						break;
					case Photometric.LOGLUV:
						Tiff.fprintf(stream, "CIE Log2(L) (u',v')\r\n", new object[0]);
						break;
					default:
						Tiff.fprintf(stream, "{0} (0x{1:x})\r\n", new object[]
						{
							this.m_dir.td_photometric,
							this.m_dir.td_photometric
						});
						break;
					}
				}
			}
			if (this.fieldSet(31) && this.m_dir.td_extrasamples != 0)
			{
				Tiff.fprintf(stream, "  Extra Samples: {0}<", new object[] { this.m_dir.td_extrasamples });
				string text2 = "";
				for (short num = 0; num < this.m_dir.td_extrasamples; num += 1)
				{
					switch (this.m_dir.td_sampleinfo[(int)num])
					{
					case ExtraSample.UNSPECIFIED:
						Tiff.fprintf(stream, "{0}unspecified", new object[] { text2 });
						break;
					case ExtraSample.ASSOCALPHA:
						Tiff.fprintf(stream, "{0}assoc-alpha", new object[] { text2 });
						break;
					case ExtraSample.UNASSALPHA:
						Tiff.fprintf(stream, "{0}unassoc-alpha", new object[] { text2 });
						break;
					default:
						Tiff.fprintf(stream, "{0}{1} (0x{2:x})", new object[]
						{
							text2,
							this.m_dir.td_sampleinfo[(int)num],
							this.m_dir.td_sampleinfo[(int)num]
						});
						break;
					}
					text2 = ", ";
				}
				Tiff.fprintf(stream, ">\r\n", new object[0]);
			}
			if (this.fieldSet(46))
			{
				Tiff.fprintf(stream, "  Ink Names: ", new object[0]);
				string td_inknames = this.m_dir.td_inknames;
				char[] separator = new char[1];
				string[] array = td_inknames.Split(separator);
				for (int i = 0; i < array.Length; i++)
				{
					Tiff.printAscii(stream, array[i]);
					Tiff.fprintf(stream, ", ", new object[0]);
				}
				Tiff.fprintf(stream, "\r\n", new object[0]);
			}
			if (this.fieldSet(9))
			{
				Tiff.fprintf(stream, "  Thresholding: ", new object[0]);
				switch (this.m_dir.td_threshholding)
				{
				case Threshold.BILEVEL:
					Tiff.fprintf(stream, "bilevel art scan\r\n", new object[0]);
					break;
				case Threshold.HALFTONE:
					Tiff.fprintf(stream, "halftone or dithered scan\r\n", new object[0]);
					break;
				case Threshold.ERRORDIFFUSE:
					Tiff.fprintf(stream, "error diffused\r\n", new object[0]);
					break;
				default:
					Tiff.fprintf(stream, "{0} (0x{1:x})\r\n", new object[]
					{
						this.m_dir.td_threshholding,
						this.m_dir.td_threshholding
					});
					break;
				}
			}
			if (this.fieldSet(10))
			{
				Tiff.fprintf(stream, "  FillOrder: ", new object[0]);
				switch (this.m_dir.td_fillorder)
				{
				case FillOrder.MSB2LSB:
					Tiff.fprintf(stream, "msb-to-lsb\r\n", new object[0]);
					break;
				case FillOrder.LSB2MSB:
					Tiff.fprintf(stream, "lsb-to-msb\r\n", new object[0]);
					break;
				default:
					Tiff.fprintf(stream, "{0} (0x{1:x})\r\n", new object[]
					{
						this.m_dir.td_fillorder,
						this.m_dir.td_fillorder
					});
					break;
				}
			}
			if (this.fieldSet(39))
			{
				FieldValue[] field = this.GetField(TiffTag.YCBCRSUBSAMPLING);
				short num2 = field[0].ToShort();
				short num3 = field[1].ToShort();
				Tiff.fprintf(stream, "  YCbCr Subsampling: {0}, {1}\r\n", new object[] { num2, num3 });
			}
			if (this.fieldSet(40))
			{
				Tiff.fprintf(stream, "  YCbCr Positioning: ", new object[0]);
				switch (this.m_dir.td_ycbcrpositioning)
				{
				case YCbCrPosition.CENTERED:
					Tiff.fprintf(stream, "centered\r\n", new object[0]);
					break;
				case YCbCrPosition.COSITED:
					Tiff.fprintf(stream, "cosited\r\n", new object[0]);
					break;
				default:
					Tiff.fprintf(stream, "{0} (0x{1:x})\r\n", new object[]
					{
						this.m_dir.td_ycbcrpositioning,
						this.m_dir.td_ycbcrpositioning
					});
					break;
				}
			}
			if (this.fieldSet(37))
			{
				Tiff.fprintf(stream, "  Halftone Hints: light {0} dark {1}\r\n", new object[]
				{
					this.m_dir.td_halftonehints[0],
					this.m_dir.td_halftonehints[1]
				});
			}
			if (this.fieldSet(15))
			{
				Tiff.fprintf(stream, "  Orientation: ", new object[0]);
				if (this.m_dir.td_orientation < (Orientation)Tiff.orientNames.Length)
				{
					Tiff.fprintf(stream, "{0}\r\n", new object[] { Tiff.orientNames[(int)this.m_dir.td_orientation] });
				}
				else
				{
					Tiff.fprintf(stream, "{0} (0x{1:x})\r\n", new object[]
					{
						this.m_dir.td_orientation,
						this.m_dir.td_orientation
					});
				}
			}
			if (this.fieldSet(16))
			{
				Tiff.fprintf(stream, "  Samples/Pixel: {0}\r\n", new object[] { this.m_dir.td_samplesperpixel });
			}
			if (this.fieldSet(17))
			{
				Tiff.fprintf(stream, "  Rows/Strip: ", new object[0]);
				if (this.m_dir.td_rowsperstrip == -1)
				{
					Tiff.fprintf(stream, "(infinite)\r\n", new object[0]);
				}
				else
				{
					Tiff.fprintf(stream, "{0}\r\n", new object[] { this.m_dir.td_rowsperstrip });
				}
			}
			if (this.fieldSet(18))
			{
				Tiff.fprintf(stream, "  Min Sample Value: {0}\r\n", new object[] { this.m_dir.td_minsamplevalue });
			}
			if (this.fieldSet(19))
			{
				Tiff.fprintf(stream, "  Max Sample Value: {0}\r\n", new object[] { this.m_dir.td_maxsamplevalue });
			}
			if (this.fieldSet(33))
			{
				Tiff.fprintf(stream, "  SMin Sample Value: {0:G}\r\n", new object[] { this.m_dir.td_sminsamplevalue });
			}
			if (this.fieldSet(34))
			{
				Tiff.fprintf(stream, "  SMax Sample Value: {0:G}\r\n", new object[] { this.m_dir.td_smaxsamplevalue });
			}
			if (this.fieldSet(20))
			{
				Tiff.fprintf(stream, "  Planar Configuration: ", new object[0]);
				switch (this.m_dir.td_planarconfig)
				{
				case PlanarConfig.CONTIG:
					Tiff.fprintf(stream, "single image plane\r\n", new object[0]);
					break;
				case PlanarConfig.SEPARATE:
					Tiff.fprintf(stream, "separate image planes\r\n", new object[0]);
					break;
				default:
					Tiff.fprintf(stream, "{0} (0x{1:x})\r\n", new object[]
					{
						this.m_dir.td_planarconfig,
						this.m_dir.td_planarconfig
					});
					break;
				}
			}
			if (this.fieldSet(23))
			{
				Tiff.fprintf(stream, "  Page Number: {0}-{1}\r\n", new object[]
				{
					this.m_dir.td_pagenumber[0],
					this.m_dir.td_pagenumber[1]
				});
			}
			if (this.fieldSet(26))
			{
				Tiff.fprintf(stream, "  Color Map: ", new object[0]);
				if ((flags & TiffPrintFlags.COLORMAP) != TiffPrintFlags.NONE)
				{
					Tiff.fprintf(stream, "\r\n", new object[0]);
					int num4 = 1 << (int)this.m_dir.td_bitspersample;
					for (int j = 0; j < num4; j++)
					{
						Tiff.fprintf(stream, "   {0,5}: {1,5} {2,5} {3,5}\r\n", new object[]
						{
							j,
							this.m_dir.td_colormap[0][j],
							this.m_dir.td_colormap[1][j],
							this.m_dir.td_colormap[2][j]
						});
					}
				}
				else
				{
					Tiff.fprintf(stream, "(present)\r\n", new object[0]);
				}
			}
			if (this.fieldSet(44))
			{
				Tiff.fprintf(stream, "  Transfer Function: ", new object[0]);
				if ((flags & TiffPrintFlags.CURVES) != TiffPrintFlags.NONE)
				{
					Tiff.fprintf(stream, "\r\n", new object[0]);
					int num5 = 1 << (int)this.m_dir.td_bitspersample;
					for (int k = 0; k < num5; k++)
					{
						Tiff.fprintf(stream, "    {0,2}: {0,5}", new object[]
						{
							k,
							this.m_dir.td_transferfunction[0][k]
						});
						for (short num6 = 1; num6 < this.m_dir.td_samplesperpixel; num6 += 1)
						{
							Tiff.fprintf(stream, " {0,5}", new object[] { this.m_dir.td_transferfunction[(int)num6][k] });
						}
						Tiff.fprintf(stream, "\r\n", new object[0]);
					}
				}
				else
				{
					Tiff.fprintf(stream, "(present)\r\n", new object[0]);
				}
			}
			if (this.fieldSet(49) && this.m_dir.td_subifd != null)
			{
				Tiff.fprintf(stream, "  SubIFD Offsets:", new object[0]);
				for (short num7 = 0; num7 < this.m_dir.td_nsubifd; num7 += 1)
				{
					Tiff.fprintf(stream, " {0,5}", new object[] { this.m_dir.td_subifd[(int)num7] });
				}
				Tiff.fprintf(stream, "\r\n", new object[0]);
			}
			int tagListCount = this.GetTagListCount();
			for (int l = 0; l < tagListCount; l++)
			{
				TiffTag tagListEntry = (TiffTag)this.GetTagListEntry(l);
				TiffFieldInfo tiffFieldInfo = this.FieldWithTag(tagListEntry);
				if (tiffFieldInfo != null)
				{
					int num8;
					byte[] array2;
					if (tiffFieldInfo.PassCount)
					{
						FieldValue[] field2 = this.GetField(tagListEntry);
						if (field2 == null)
						{
							goto IL_11F5;
						}
						num8 = field2[0].ToInt();
						array2 = field2[1].ToByteArray();
					}
					else
					{
						if (tiffFieldInfo.ReadCount == -1 || tiffFieldInfo.ReadCount == -3)
						{
							num8 = 1;
						}
						else if (tiffFieldInfo.ReadCount == -2)
						{
							num8 = (int)this.m_dir.td_samplesperpixel;
						}
						else
						{
							num8 = (int)tiffFieldInfo.ReadCount;
						}
						if ((tiffFieldInfo.Type == TiffType.ASCII || tiffFieldInfo.ReadCount == -1 || tiffFieldInfo.ReadCount == -3 || tiffFieldInfo.ReadCount == -2 || num8 > 1) && tiffFieldInfo.Tag != TiffTag.PAGENUMBER && tiffFieldInfo.Tag != TiffTag.HALFTONEHINTS && tiffFieldInfo.Tag != TiffTag.YCBCRSUBSAMPLING && tiffFieldInfo.Tag != TiffTag.DOTRANGE)
						{
							FieldValue[] field3 = this.GetField(tagListEntry);
							if (field3 == null)
							{
								goto IL_11F5;
							}
							array2 = field3[0].ToByteArray();
						}
						else if (tiffFieldInfo.Tag != TiffTag.PAGENUMBER && tiffFieldInfo.Tag != TiffTag.HALFTONEHINTS && tiffFieldInfo.Tag != TiffTag.YCBCRSUBSAMPLING && tiffFieldInfo.Tag != TiffTag.DOTRANGE)
						{
							array2 = new byte[Tiff.dataSize(tiffFieldInfo.Type) * num8];
							FieldValue[] field4 = this.GetField(tagListEntry);
							if (field4 == null)
							{
								goto IL_11F5;
							}
							array2 = field4[0].ToByteArray();
						}
						else
						{
							array2 = new byte[Tiff.dataSize(tiffFieldInfo.Type) * num8];
							FieldValue[] field5 = this.GetField(tagListEntry);
							if (field5 == null)
							{
								goto IL_11F5;
							}
							byte[] array3 = field5[0].ToByteArray();
							byte[] array4 = field5[1].ToByteArray();
							Buffer.BlockCopy(array3, 0, array2, 0, array3.Length);
							Buffer.BlockCopy(array4, 0, array2, Tiff.dataSize(tiffFieldInfo.Type), array4.Length);
						}
					}
					if (!this.prettyPrintField(stream, tagListEntry, num8, array2))
					{
						Tiff.printField(stream, tiffFieldInfo, num8, array2);
					}
				}
				IL_11F5:;
			}
			this.m_tagmethods.PrintDir(this, stream, flags);
			if ((flags & TiffPrintFlags.STRIPS) != TiffPrintFlags.NONE && this.fieldSet(25))
			{
				Tiff.fprintf(stream, "  {0} {1}:\r\n", new object[]
				{
					this.m_dir.td_nstrips,
					this.IsTiled() ? "Tiles" : "Strips"
				});
				for (int m = 0; m < this.m_dir.td_nstrips; m++)
				{
					Tiff.fprintf(stream, "    {0,3}: [{0,8}, {0,8}]\r\n", new object[]
					{
						m,
						this.m_dir.td_stripoffset[m],
						this.m_dir.td_stripbytecount[m]
					});
				}
			}
		}

		public bool ReadScanline(byte[] buffer, int row)
		{
			return this.ReadScanline(buffer, 0, row, 0);
		}

		public bool ReadScanline(byte[] buffer, int row, short plane)
		{
			return this.ReadScanline(buffer, 0, row, plane);
		}

		public bool ReadScanline(byte[] buffer, int offset, int row, short plane)
		{
			if (!this.checkRead(false))
			{
				return false;
			}
			bool flag = this.seek(row, plane);
			if (flag)
			{
				flag = this.m_currentCodec.DecodeRow(buffer, offset, this.m_scanlinesize, plane);
				this.m_row = row + 1;
				if (flag)
				{
					this.postDecode(buffer, offset, this.m_scanlinesize);
				}
			}
			return flag;
		}

		public bool WriteScanline(byte[] buffer, int row)
		{
			return this.WriteScanline(buffer, 0, row, 0);
		}

		public bool WriteScanline(byte[] buffer, int row, short plane)
		{
			return this.WriteScanline(buffer, 0, row, plane);
		}

		public bool WriteScanline(byte[] buffer, int offset, int row, short plane)
		{
			if (!this.writeCheckStrips("WriteScanline"))
			{
				return false;
			}
			this.bufferCheck();
			bool flag = false;
			if (row >= this.m_dir.td_imagelength)
			{
				if (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
				{
					Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Can not change \"ImageLength\" when using separate planes", new object[0]);
					return false;
				}
				this.m_dir.td_imagelength = row + 1;
				flag = true;
			}
			int num;
			if (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
			{
				if (plane >= this.m_dir.td_samplesperpixel)
				{
					Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "{0}: Sample out of range, max {1}", new object[]
					{
						plane,
						this.m_dir.td_samplesperpixel
					});
					return false;
				}
				if (this.m_dir.td_rowsperstrip != -1)
				{
					num = (int)plane * this.m_dir.td_stripsperimage + row / this.m_dir.td_rowsperstrip;
				}
				else
				{
					num = 0;
				}
			}
			else if (this.m_dir.td_rowsperstrip != -1)
			{
				num = row / this.m_dir.td_rowsperstrip;
			}
			else
			{
				num = 0;
			}
			if (num >= this.m_dir.td_nstrips && !this.growStrips(1))
			{
				return false;
			}
			if (num != this.m_curstrip)
			{
				if (!this.FlushData())
				{
					return false;
				}
				this.m_curstrip = num;
				if (num >= this.m_dir.td_stripsperimage && flag)
				{
					this.m_dir.td_stripsperimage = Tiff.howMany(this.m_dir.td_imagelength, this.m_dir.td_rowsperstrip);
				}
				this.m_row = num % this.m_dir.td_stripsperimage * this.m_dir.td_rowsperstrip;
				if ((this.m_flags & TiffFlags.CODERSETUP) != TiffFlags.CODERSETUP)
				{
					if (!this.m_currentCodec.SetupEncode())
					{
						return false;
					}
					this.m_flags |= TiffFlags.CODERSETUP;
				}
				this.m_rawcc = 0;
				this.m_rawcp = 0;
				if (this.m_dir.td_stripbytecount[num] > 0U)
				{
					this.m_dir.td_stripbytecount[num] = 0U;
					this.m_curoff = 0U;
				}
				if (!this.m_currentCodec.PreEncode(plane))
				{
					return false;
				}
				this.m_flags |= TiffFlags.POSTENCODE;
			}
			if (row != this.m_row)
			{
				if (row < this.m_row)
				{
					this.m_row = num % this.m_dir.td_stripsperimage * this.m_dir.td_rowsperstrip;
					this.m_rawcp = 0;
				}
				if (!this.m_currentCodec.Seek(row - this.m_row))
				{
					return false;
				}
				this.m_row = row;
			}
			this.postDecode(buffer, offset, this.m_scanlinesize);
			bool result = this.m_currentCodec.EncodeRow(buffer, offset, this.m_scanlinesize, plane);
			this.m_row = row + 1;
			return result;
		}

		public bool ReadRGBAImage(int width, int height, int[] raster)
		{
			return this.ReadRGBAImage(width, height, raster, false);
		}

		public bool ReadRGBAImage(int width, int height, int[] raster, bool stopOnError)
		{
			return this.ReadRGBAImageOriented(width, height, raster, Orientation.BOTLEFT, stopOnError);
		}

		public bool ReadRGBAImageOriented(int width, int height, int[] raster, Orientation orientation)
		{
			return this.ReadRGBAImageOriented(width, height, raster, orientation, false);
		}

		public bool ReadRGBAImageOriented(int width, int height, int[] raster, Orientation orientation, bool stopOnError)
		{
			bool result = false;
			string text;
			if (this.RGBAImageOK(out text))
			{
				TiffRgbaImage tiffRgbaImage = TiffRgbaImage.Create(this, stopOnError, out text);
				if (tiffRgbaImage != null)
				{
					tiffRgbaImage.ReqOrientation = orientation;
					result = tiffRgbaImage.GetRaster(raster, (height - tiffRgbaImage.Height) * width, width, tiffRgbaImage.Height);
				}
			}
			else
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.FileName(), "{0}", new object[] { text });
				result = false;
			}
			return result;
		}

		public bool ReadRGBAStrip(int row, int[] raster)
		{
			if (this.IsTiled())
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.FileName(), "Can't use ReadRGBAStrip() with tiled file.", new object[0]);
				return false;
			}
			FieldValue[] fieldDefaulted = this.GetFieldDefaulted(TiffTag.ROWSPERSTRIP);
			int num = fieldDefaulted[0].ToInt();
			if (row % num != 0)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.FileName(), "Row passed to ReadRGBAStrip() must be first in a strip.", new object[0]);
				return false;
			}
			string text;
			if (!this.RGBAImageOK(out text))
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.FileName(), "{0}", new object[] { text });
				return false;
			}
			TiffRgbaImage tiffRgbaImage = TiffRgbaImage.Create(this, false, out text);
			if (tiffRgbaImage != null)
			{
				tiffRgbaImage.row_offset = row;
				tiffRgbaImage.col_offset = 0;
				int height = num;
				if (row + num > tiffRgbaImage.Height)
				{
					height = tiffRgbaImage.Height - row;
				}
				return tiffRgbaImage.GetRaster(raster, 0, tiffRgbaImage.Width, height);
			}
			return true;
		}

		public bool ReadRGBATile(int col, int row, int[] raster)
		{
			if (!this.IsTiled())
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.FileName(), "Can't use ReadRGBATile() with stripped file.", new object[0]);
				return false;
			}
			FieldValue[] fieldDefaulted = this.GetFieldDefaulted(TiffTag.TILEWIDTH);
			int num = fieldDefaulted[0].ToInt();
			fieldDefaulted = this.GetFieldDefaulted(TiffTag.TILELENGTH);
			int num2 = fieldDefaulted[0].ToInt();
			if (col % num != 0 || row % num2 != 0)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.FileName(), "Row/col passed to ReadRGBATile() must be topleft corner of a tile.", new object[0]);
				return false;
			}
			string text;
			TiffRgbaImage tiffRgbaImage = TiffRgbaImage.Create(this, false, out text);
			if (!this.RGBAImageOK(out text) || tiffRgbaImage == null)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.FileName(), "{0}", new object[] { text });
				return false;
			}
			int num3;
			if (row + num2 > tiffRgbaImage.Height)
			{
				num3 = tiffRgbaImage.Height - row;
			}
			else
			{
				num3 = num2;
			}
			int num4;
			if (col + num > tiffRgbaImage.Width)
			{
				num4 = tiffRgbaImage.Width - col;
			}
			else
			{
				num4 = num;
			}
			tiffRgbaImage.row_offset = row;
			tiffRgbaImage.col_offset = col;
			bool raster2 = tiffRgbaImage.GetRaster(raster, 0, num4, num3);
			if (num4 == num && num3 == num2)
			{
				return raster2;
			}
			for (int i = 0; i < num3; i++)
			{
				Buffer.BlockCopy(raster, (num3 - i - 1) * num4 * 4, raster, (num2 - i - 1) * num * 4, num4 * 4);
				Array.Clear(raster, (num2 - i - 1) * num + num4, num - num4);
			}
			for (int j = num3; j < num2; j++)
			{
				Array.Clear(raster, (num2 - j - 1) * num, num);
			}
			return raster2;
		}

		public bool RGBAImageOK(out string errorMsg)
		{
			errorMsg = null;
			if (!this.m_decodestatus)
			{
				errorMsg = "Sorry, requested compression method is not configured";
				return false;
			}
			short td_bitspersample = this.m_dir.td_bitspersample;
			switch (td_bitspersample)
			{
			case 1:
			case 2:
			case 4:
				goto IL_76;
			case 3:
				break;
			default:
				if (td_bitspersample == 8 || td_bitspersample == 16)
				{
					goto IL_76;
				}
				break;
			}
			errorMsg = string.Format(CultureInfo.InvariantCulture, "Sorry, can not handle images with {0}-bit samples", new object[] { this.m_dir.td_bitspersample });
			return false;
			IL_76:
			int num = (int)(this.m_dir.td_samplesperpixel - this.m_dir.td_extrasamples);
			FieldValue[] array = this.GetField(TiffTag.PHOTOMETRIC);
			Photometric photometric;
			if (array == null)
			{
				switch (num)
				{
				case 1:
					photometric = Photometric.MINISBLACK;
					goto IL_F9;
				case 3:
					photometric = Photometric.RGB;
					goto IL_F9;
				}
				errorMsg = string.Format(CultureInfo.InvariantCulture, "Missing needed {0} tag", new object[] { "PhotometricInterpretation" });
				return false;
			}
			photometric = (Photometric)array[0].Value;
			IL_F9:
			Photometric photometric2 = photometric;
			switch (photometric2)
			{
			case Photometric.MINISWHITE:
			case Photometric.MINISBLACK:
			case Photometric.PALETTE:
				if (this.m_dir.td_planarconfig == PlanarConfig.CONTIG && this.m_dir.td_samplesperpixel != 1 && this.m_dir.td_bitspersample < 8)
				{
					errorMsg = string.Format(CultureInfo.InvariantCulture, "Sorry, can not handle contiguous data with {0}={1}, and {2}={3} and Bits/Sample={4}", new object[]
					{
						"PhotometricInterpretation",
						photometric,
						"Samples/pixel",
						this.m_dir.td_samplesperpixel,
						this.m_dir.td_bitspersample
					});
					return false;
				}
				return true;
			case Photometric.RGB:
				if (num < 3)
				{
					errorMsg = string.Format(CultureInfo.InvariantCulture, "Sorry, can not handle RGB image with {0}={1}", new object[] { "Color channels", num });
					return false;
				}
				return true;
			case Photometric.MASK:
			case (Photometric)7:
				break;
			case Photometric.SEPARATED:
			{
				array = this.GetFieldDefaulted(TiffTag.INKSET);
				InkSet inkSet = (InkSet)array[0].ToByte();
				if (inkSet != InkSet.CMYK)
				{
					errorMsg = string.Format(CultureInfo.InvariantCulture, "Sorry, can not handle separated image with {0}={1}", new object[] { "InkSet", inkSet });
					return false;
				}
				if (this.m_dir.td_samplesperpixel < 4)
				{
					errorMsg = string.Format(CultureInfo.InvariantCulture, "Sorry, can not handle separated image with {0}={1}", new object[]
					{
						"Samples/pixel",
						this.m_dir.td_samplesperpixel
					});
					return false;
				}
				return true;
			}
			case Photometric.YCBCR:
			case Photometric.CIELAB:
				return true;
			default:
				switch (photometric2)
				{
				case Photometric.LOGL:
					if (this.m_dir.td_compression != Compression.SGILOG)
					{
						errorMsg = string.Format(CultureInfo.InvariantCulture, "Sorry, LogL data must have {0}={1}", new object[]
						{
							"Compression",
							Compression.SGILOG
						});
						return false;
					}
					return true;
				case Photometric.LOGLUV:
					if (this.m_dir.td_compression != Compression.SGILOG && this.m_dir.td_compression != Compression.SGILOG24)
					{
						errorMsg = string.Format(CultureInfo.InvariantCulture, "Sorry, LogLuv data must have {0}={1} or {2}", new object[]
						{
							"Compression",
							Compression.SGILOG,
							Compression.SGILOG24
						});
						return false;
					}
					if (this.m_dir.td_planarconfig != PlanarConfig.CONTIG)
					{
						errorMsg = string.Format(CultureInfo.InvariantCulture, "Sorry, can not handle LogLuv images with {0}={1}", new object[]
						{
							"Planarconfiguration",
							this.m_dir.td_planarconfig
						});
						return false;
					}
					return true;
				}
				break;
			}
			errorMsg = string.Format(CultureInfo.InvariantCulture, "Sorry, can not handle image with {0}={1}", new object[] { "PhotometricInterpretation", photometric });
			return false;
		}

		public string FileName()
		{
			return this.m_name;
		}

		public string SetFileName(string name)
		{
			string name2 = this.m_name;
			this.m_name = name;
			return name2;
		}

		public static void Error(Tiff tif, string method, string format, params object[] args)
		{
			TiffErrorHandler errorHandler = Tiff.getErrorHandler(tif);
			if (errorHandler == null)
			{
				return;
			}
			errorHandler.ErrorHandler(tif, method, format, args);
			errorHandler.ErrorHandlerExt(tif, null, method, format, args);
		}

		public static void Error(string method, string format, params object[] args)
		{
			Tiff.Error(null, method, format, args);
		}

		public static void ErrorExt(Tiff tif, object clientData, string method, string format, params object[] args)
		{
			TiffErrorHandler errorHandler = Tiff.getErrorHandler(tif);
			if (errorHandler == null)
			{
				return;
			}
			errorHandler.ErrorHandler(tif, method, format, args);
			errorHandler.ErrorHandlerExt(tif, clientData, method, format, args);
		}

		public static void ErrorExt(object clientData, string method, string format, params object[] args)
		{
			Tiff.ErrorExt(null, clientData, method, format, args);
		}

		public static void Warning(Tiff tif, string method, string format, params object[] args)
		{
			TiffErrorHandler errorHandler = Tiff.getErrorHandler(tif);
			if (errorHandler == null)
			{
				return;
			}
			errorHandler.WarningHandler(tif, method, format, args);
			errorHandler.WarningHandlerExt(tif, null, method, format, args);
		}

		public static void Warning(string method, string format, params object[] args)
		{
			Tiff.Warning(null, method, format, args);
		}

		public static void WarningExt(Tiff tif, object clientData, string method, string format, params object[] args)
		{
			TiffErrorHandler errorHandler = Tiff.getErrorHandler(tif);
			if (errorHandler == null)
			{
				return;
			}
			errorHandler.WarningHandler(tif, method, format, args);
			errorHandler.WarningHandlerExt(tif, clientData, method, format, args);
		}

		public static void WarningExt(object clientData, string method, string format, params object[] args)
		{
			Tiff.WarningExt(null, clientData, method, format, args);
		}

		public static TiffErrorHandler SetErrorHandler(TiffErrorHandler errorHandler)
		{
			return Tiff.setErrorHandlerImpl(errorHandler);
		}

		public static Tiff.TiffExtendProc SetTagExtender(Tiff.TiffExtendProc extender)
		{
			return Tiff.setTagExtenderImpl(extender);
		}

		public int ReadTile(byte[] buffer, int offset, int x, int y, int z, short plane)
		{
			if (!this.checkRead(true) || !this.CheckTile(x, y, z, plane))
			{
				return -1;
			}
			return this.ReadEncodedTile(this.ComputeTile(x, y, z, plane), buffer, offset, -1);
		}

		public int ReadEncodedTile(int tile, byte[] buffer, int offset, int count)
		{
			if (!this.checkRead(true))
			{
				return -1;
			}
			if (tile >= this.m_dir.td_nstrips)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "{0}: Tile out of range, max {1}", new object[]
				{
					tile,
					this.m_dir.td_nstrips
				});
				return -1;
			}
			if (count == -1)
			{
				count = this.m_tilesize;
			}
			else if (count > this.m_tilesize)
			{
				count = this.m_tilesize;
			}
			if (this.fillTile(tile) && this.m_currentCodec.DecodeTile(buffer, offset, count, (short)(tile / this.m_dir.td_stripsperimage)))
			{
				this.postDecode(buffer, offset, count);
				return count;
			}
			return -1;
		}

		public int ReadRawTile(int tile, byte[] buffer, int offset, int count)
		{
			if (!this.checkRead(true))
			{
				return -1;
			}
			if (tile >= this.m_dir.td_nstrips)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "{0}: Tile out of range, max {1}", new object[]
				{
					tile,
					this.m_dir.td_nstrips
				});
				return -1;
			}
			if ((this.m_flags & TiffFlags.NOREADRAW) == TiffFlags.NOREADRAW)
			{
				Tiff.ErrorExt(this.m_clientdata, this.m_name, "Compression scheme does not support access to raw uncompressed data", new object[0]);
				return -1;
			}
			uint num = this.m_dir.td_stripbytecount[tile];
			if (count != -1 && count < (int)num)
			{
				num = (uint)count;
			}
			return this.readRawTile1(tile, buffer, offset, (int)num, "ReadRawTile");
		}

		public int WriteTile(byte[] buffer, int x, int y, int z, short plane)
		{
			return this.WriteTile(buffer, 0, x, y, z, plane);
		}

		public int WriteTile(byte[] buffer, int offset, int x, int y, int z, short plane)
		{
			if (!this.CheckTile(x, y, z, plane))
			{
				return -1;
			}
			return this.WriteEncodedTile(this.ComputeTile(x, y, z, plane), buffer, offset, -1);
		}

		public int ReadEncodedStrip(int strip, byte[] buffer, int offset, int count)
		{
			if (!this.checkRead(false))
			{
				return -1;
			}
			if (strip >= this.m_dir.td_nstrips)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "{0}: Strip out of range, max {1}", new object[]
				{
					strip,
					this.m_dir.td_nstrips
				});
				return -1;
			}
			int num;
			if (this.m_dir.td_rowsperstrip >= this.m_dir.td_imagelength)
			{
				num = 1;
			}
			else
			{
				num = (this.m_dir.td_imagelength + this.m_dir.td_rowsperstrip - 1) / this.m_dir.td_rowsperstrip;
			}
			int num2 = strip % num;
			int num3 = this.m_dir.td_imagelength % this.m_dir.td_rowsperstrip;
			if (num2 != num - 1 || num3 == 0)
			{
				num3 = this.m_dir.td_rowsperstrip;
			}
			int num4 = this.VStripSize(num3);
			if (count == -1)
			{
				count = num4;
			}
			else if (count > num4)
			{
				count = num4;
			}
			if (this.fillStrip(strip) && this.m_currentCodec.DecodeStrip(buffer, offset, count, (short)(strip / this.m_dir.td_stripsperimage)))
			{
				this.postDecode(buffer, offset, count);
				return count;
			}
			return -1;
		}

		public int ReadRawStrip(int strip, byte[] buffer, int offset, int count)
		{
			if (!this.checkRead(false))
			{
				return -1;
			}
			if (strip >= this.m_dir.td_nstrips)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "{0}: Strip out of range, max {1}", new object[]
				{
					strip,
					this.m_dir.td_nstrips
				});
				return -1;
			}
			if ((this.m_flags & TiffFlags.NOREADRAW) == TiffFlags.NOREADRAW)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Compression scheme does not support access to raw uncompressed data", new object[0]);
				return -1;
			}
			uint num = this.m_dir.td_stripbytecount[strip];
			if (num <= 0U)
			{
				Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "{0}: Invalid strip byte count, strip {1}", new object[] { num, strip });
				return -1;
			}
			if (count != -1 && count < (int)num)
			{
				num = (uint)count;
			}
			return this.readRawStrip1(strip, buffer, offset, (int)num, "ReadRawStrip");
		}

		public int WriteEncodedStrip(int strip, byte[] buffer, int count)
		{
			return this.WriteEncodedStrip(strip, buffer, 0, count);
		}

		public int WriteEncodedStrip(int strip, byte[] buffer, int offset, int count)
		{
			if (!this.writeCheckStrips("WriteEncodedStrip"))
			{
				return -1;
			}
			if (strip >= this.m_dir.td_nstrips)
			{
				if (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
				{
					Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Can not grow image by strips when using separate planes", new object[0]);
					return -1;
				}
				if (!this.growStrips(1))
				{
					return -1;
				}
				this.m_dir.td_stripsperimage = Tiff.howMany(this.m_dir.td_imagelength, this.m_dir.td_rowsperstrip);
			}
			this.bufferCheck();
			this.m_curstrip = strip;
			this.m_row = strip % this.m_dir.td_stripsperimage * this.m_dir.td_rowsperstrip;
			if ((this.m_flags & TiffFlags.CODERSETUP) != TiffFlags.CODERSETUP)
			{
				if (!this.m_currentCodec.SetupEncode())
				{
					return -1;
				}
				this.m_flags |= TiffFlags.CODERSETUP;
			}
			this.m_rawcc = 0;
			this.m_rawcp = 0;
			if (this.m_dir.td_stripbytecount[strip] > 0U)
			{
				this.m_curoff = 0U;
			}
			this.m_flags &= ~TiffFlags.POSTENCODE;
			short plane = (short)(strip / this.m_dir.td_stripsperimage);
			if (!this.m_currentCodec.PreEncode(plane))
			{
				return -1;
			}
			this.postDecode(buffer, offset, count);
			if (!this.m_currentCodec.EncodeStrip(buffer, offset, count, plane))
			{
				return 0;
			}
			if (!this.m_currentCodec.PostEncode())
			{
				return -1;
			}
			if (!this.isFillOrder(this.m_dir.td_fillorder) && (this.m_flags & TiffFlags.NOBITREV) != TiffFlags.NOBITREV)
			{
				Tiff.ReverseBits(this.m_rawdata, this.m_rawcc);
			}
			if (this.m_rawcc > 0 && !this.appendToStrip(strip, this.m_rawdata, 0, this.m_rawcc))
			{
				return -1;
			}
			this.m_rawcc = 0;
			this.m_rawcp = 0;
			return count;
		}

		public int WriteRawStrip(int strip, byte[] buffer, int count)
		{
			return this.WriteRawStrip(strip, buffer, 0, count);
		}

		public int WriteRawStrip(int strip, byte[] buffer, int offset, int count)
		{
			if (!this.writeCheckStrips("WriteRawStrip"))
			{
				return -1;
			}
			if (strip >= this.m_dir.td_nstrips)
			{
				if (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
				{
					Tiff.ErrorExt(this, this.m_clientdata, this.m_name, "Can not grow image by strips when using separate planes", new object[0]);
					return -1;
				}
				if (strip >= this.m_dir.td_stripsperimage)
				{
					this.m_dir.td_stripsperimage = Tiff.howMany(this.m_dir.td_imagelength, this.m_dir.td_rowsperstrip);
				}
				if (!this.growStrips(1))
				{
					return -1;
				}
			}
			this.m_curstrip = strip;
			this.m_row = strip % this.m_dir.td_stripsperimage * this.m_dir.td_rowsperstrip;
			if (!this.appendToStrip(strip, buffer, offset, count))
			{
				return -1;
			}
			return count;
		}

		public int WriteEncodedTile(int tile, byte[] buffer, int count)
		{
			return this.WriteEncodedTile(tile, buffer, 0, count);
		}

		public int WriteEncodedTile(int tile, byte[] buffer, int offset, int count)
		{
			if (!this.writeCheckTiles("WriteEncodedTile"))
			{
				return -1;
			}
			if (tile >= this.m_dir.td_nstrips)
			{
				Tiff.ErrorExt(this, this.m_clientdata, "WriteEncodedTile", "{0}: Tile {1} out of range, max {2}", new object[]
				{
					this.m_name,
					tile,
					this.m_dir.td_nstrips
				});
				return -1;
			}
			this.bufferCheck();
			this.m_curtile = tile;
			this.m_rawcc = 0;
			this.m_rawcp = 0;
			if (this.m_dir.td_stripbytecount[tile] > 0U)
			{
				this.m_curoff = 0U;
			}
			this.m_row = tile % Tiff.howMany(this.m_dir.td_imagelength, this.m_dir.td_tilelength) * this.m_dir.td_tilelength;
			this.m_col = tile % Tiff.howMany(this.m_dir.td_imagewidth, this.m_dir.td_tilewidth) * this.m_dir.td_tilewidth;
			if ((this.m_flags & TiffFlags.CODERSETUP) != TiffFlags.CODERSETUP)
			{
				if (!this.m_currentCodec.SetupEncode())
				{
					return -1;
				}
				this.m_flags |= TiffFlags.CODERSETUP;
			}
			this.m_flags &= ~TiffFlags.POSTENCODE;
			short plane = (short)(tile / this.m_dir.td_stripsperimage);
			if (!this.m_currentCodec.PreEncode(plane))
			{
				return -1;
			}
			if (count < 1 || count > this.m_tilesize)
			{
				count = this.m_tilesize;
			}
			this.postDecode(buffer, offset, count);
			if (!this.m_currentCodec.EncodeTile(buffer, offset, count, plane))
			{
				return 0;
			}
			if (!this.m_currentCodec.PostEncode())
			{
				return -1;
			}
			if (!this.isFillOrder(this.m_dir.td_fillorder) && (this.m_flags & TiffFlags.NOBITREV) != TiffFlags.NOBITREV)
			{
				Tiff.ReverseBits(this.m_rawdata, this.m_rawcc);
			}
			if (this.m_rawcc > 0 && !this.appendToStrip(tile, this.m_rawdata, 0, this.m_rawcc))
			{
				return -1;
			}
			this.m_rawcc = 0;
			this.m_rawcp = 0;
			return count;
		}

		public int WriteRawTile(int tile, byte[] buffer, int count)
		{
			return this.WriteRawTile(tile, buffer, 0, count);
		}

		public int WriteRawTile(int tile, byte[] buffer, int offset, int count)
		{
			if (!this.writeCheckTiles("WriteRawTile"))
			{
				return -1;
			}
			if (tile >= this.m_dir.td_nstrips)
			{
				Tiff.ErrorExt(this, this.m_clientdata, "WriteRawTile", "{0}: Tile {1} out of range, max {2}", new object[]
				{
					this.m_name,
					tile,
					this.m_dir.td_nstrips
				});
				return -1;
			}
			if (!this.appendToStrip(tile, buffer, offset, count))
			{
				return -1;
			}
			return count;
		}

		public void SetWriteOffset(long offset)
		{
			this.m_curoff = (uint)offset;
		}

		public static int DataWidth(TiffType type)
		{
			switch (type)
			{
			case TiffType.NOTYPE:
			case TiffType.BYTE:
			case TiffType.ASCII:
			case TiffType.SBYTE:
			case TiffType.UNDEFINED:
				return 1;
			case TiffType.SHORT:
			case TiffType.SSHORT:
				return 2;
			case TiffType.LONG:
			case TiffType.SLONG:
			case TiffType.FLOAT:
			case TiffType.IFD:
				return 4;
			case TiffType.RATIONAL:
			case TiffType.SRATIONAL:
			case TiffType.DOUBLE:
				return 8;
			default:
				return 0;
			}
		}

		public static void SwabShort(ref short value)
		{
			byte[] array = new byte[]
			{
				(byte)value,
				(byte)(value >> 8)
			};
			byte b = array[1];
			array[1] = array[0];
			array[0] = b;
			value = (short)(array[0] & byte.MaxValue);
			value += (short)((array[1] & byte.MaxValue) << 8);
		}

		public static void SwabLong(ref int value)
		{
			byte[] array = new byte[]
			{
				(byte)value,
				(byte)(value >> 8),
				(byte)(value >> 16),
				(byte)(value >> 24)
			};
			byte b = array[3];
			array[3] = array[0];
			array[0] = b;
			b = array[2];
			array[2] = array[1];
			array[1] = b;
			value = (int)(array[0] & byte.MaxValue);
			value += (int)(array[1] & byte.MaxValue) << 8;
			value += (int)(array[2] & byte.MaxValue) << 16;
			value += (int)array[3] << 24;
		}

		public static void SwabDouble(ref double value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			int[] array = new int[2];
			array[0] = BitConverter.ToInt32(bytes, 0);
			array[0] = BitConverter.ToInt32(bytes, 4);
			Tiff.SwabArrayOfLong(array, 2);
			int num = array[0];
			array[0] = array[1];
			array[1] = num;
			Buffer.BlockCopy(BitConverter.GetBytes(array[0]), 0, bytes, 0, 4);
			Buffer.BlockCopy(BitConverter.GetBytes(array[1]), 0, bytes, 4, 4);
			value = BitConverter.ToDouble(bytes, 0);
		}

		public static void SwabArrayOfShort(short[] array, int count)
		{
			Tiff.SwabArrayOfShort(array, 0, count);
		}

		public static void SwabArrayOfShort(short[] array, int offset, int count)
		{
			byte[] array2 = new byte[2];
			int i = 0;
			while (i < count)
			{
				array2[0] = (byte)array[offset];
				array2[1] = (byte)(array[offset] >> 8);
				byte b = array2[1];
				array2[1] = array2[0];
				array2[0] = b;
				array[offset] = (short)(array2[0] & byte.MaxValue);
				int num = offset;
				array[num] += (short)((array2[1] & byte.MaxValue) << 8);
				i++;
				offset++;
			}
		}

		public static void SwabArrayOfTriples(byte[] array, int count)
		{
			Tiff.SwabArrayOfTriples(array, 0, count);
		}

		public static void SwabArrayOfTriples(byte[] array, int offset, int count)
		{
			while (count-- > 0)
			{
				byte b = array[offset + 2];
				array[offset + 2] = array[offset];
				array[offset] = b;
				offset += 3;
			}
		}

		public static void SwabArrayOfLong(int[] array, int count)
		{
			Tiff.SwabArrayOfLong(array, 0, count);
		}

		public static void SwabArrayOfLong(int[] array, int offset, int count)
		{
			byte[] array2 = new byte[4];
			int i = 0;
			while (i < count)
			{
				array2[0] = (byte)array[offset];
				array2[1] = (byte)(array[offset] >> 8);
				array2[2] = (byte)(array[offset] >> 16);
				array2[3] = (byte)(array[offset] >> 24);
				byte b = array2[3];
				array2[3] = array2[0];
				array2[0] = b;
				b = array2[2];
				array2[2] = array2[1];
				array2[1] = b;
				array[offset] = (int)(array2[0] & byte.MaxValue);
				array[offset] += (int)(array2[1] & byte.MaxValue) << 8;
				array[offset] += (int)(array2[2] & byte.MaxValue) << 16;
				array[offset] += (int)array2[3] << 24;
				i++;
				offset++;
			}
		}

		public static void SwabArrayOfDouble(double[] array, int count)
		{
			Tiff.SwabArrayOfDouble(array, 0, count);
		}

		public static void SwabArrayOfDouble(double[] array, int offset, int count)
		{
			int[] array2 = new int[count * 8 / 4];
			Buffer.BlockCopy(array, offset * 8, array2, 0, array2.Length * 4);
			Tiff.SwabArrayOfLong(array2, array2.Length);
			int num = 0;
			while (count-- > 0)
			{
				int num2 = array2[num];
				array2[num] = array2[num + 1];
				array2[num + 1] = num2;
				num += 2;
			}
			Buffer.BlockCopy(array2, 0, array, offset * 8, array2.Length * 4);
		}

		public static void ReverseBits(byte[] buffer, int count)
		{
			Tiff.ReverseBits(buffer, 0, count);
		}

		public static void ReverseBits(byte[] buffer, int offset, int count)
		{
			while (count > 8)
			{
				buffer[offset] = Tiff.TIFFBitRevTable[(int)buffer[offset]];
				buffer[offset + 1] = Tiff.TIFFBitRevTable[(int)buffer[offset + 1]];
				buffer[offset + 2] = Tiff.TIFFBitRevTable[(int)buffer[offset + 2]];
				buffer[offset + 3] = Tiff.TIFFBitRevTable[(int)buffer[offset + 3]];
				buffer[offset + 4] = Tiff.TIFFBitRevTable[(int)buffer[offset + 4]];
				buffer[offset + 5] = Tiff.TIFFBitRevTable[(int)buffer[offset + 5]];
				buffer[offset + 6] = Tiff.TIFFBitRevTable[(int)buffer[offset + 6]];
				buffer[offset + 7] = Tiff.TIFFBitRevTable[(int)buffer[offset + 7]];
				offset += 8;
				count -= 8;
			}
			while (count-- > 0)
			{
				buffer[offset] = Tiff.TIFFBitRevTable[(int)buffer[offset]];
				offset++;
			}
		}

		public static byte[] GetBitRevTable(bool reversed)
		{
			if (!reversed)
			{
				return Tiff.TIFFNoBitRevTable;
			}
			return Tiff.TIFFBitRevTable;
		}

		public static int[] ByteArrayToInts(byte[] buffer, int offset, int count)
		{
			int num = count / 4;
			int[] array = new int[num];
			Buffer.BlockCopy(buffer, offset, array, 0, num * 4);
			return array;
		}

		public static void IntsToByteArray(int[] source, int srcOffset, int srcCount, byte[] bytes, int offset)
		{
			Buffer.BlockCopy(source, srcOffset * 4, bytes, offset, srcCount * 4);
		}

		public static short[] ByteArrayToShorts(byte[] buffer, int offset, int count)
		{
			int num = count / 2;
			short[] array = new short[num];
			Buffer.BlockCopy(buffer, offset, array, 0, num * 2);
			return array;
		}

		public static void ShortsToByteArray(short[] source, int srcOffset, int srcCount, byte[] bytes, int offset)
		{
			Buffer.BlockCopy(source, srcOffset * 2, bytes, offset, srcCount * 2);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Tiff()
		{
			int[] array = new int[13];
			Tiff.litTypeshift = array;
			Tiff.photoNames = new string[] { "min-is-white", "min-is-black", "RGB color", "palette color (RGB from colormap)", "transparency mask", "separated", "YCbCr", "7 (0x7)", "CIE L*a*b*" };
			Tiff.orientNames = new string[] { "0 (0x0)", "row 0 top, col 0 lhs", "row 0 top, col 0 rhs", "row 0 bottom, col 0 rhs", "row 0 bottom, col 0 lhs", "row 0 lhs, col 0 top", "row 0 rhs, col 0 top", "row 0 rhs, col 0 bottom", "row 0 lhs, col 0 bottom" };
			Tiff.TIFFBitRevTable = new byte[]
			{
				0, 128, 64, 192, 32, 160, 96, 224, 16, 144,
				80, 208, 48, 176, 112, 240, 8, 136, 72, 200,
				40, 168, 104, 232, 24, 152, 88, 216, 56, 184,
				120, 248, 4, 132, 68, 196, 36, 164, 100, 228,
				20, 148, 84, 212, 52, 180, 116, 244, 12, 140,
				76, 204, 44, 172, 108, 236, 28, 156, 92, 220,
				60, 188, 124, 252, 2, 130, 66, 194, 34, 162,
				98, 226, 18, 146, 82, 210, 50, 178, 114, 242,
				10, 138, 74, 202, 42, 170, 106, 234, 26, 154,
				90, 218, 58, 186, 122, 250, 6, 134, 70, 198,
				38, 166, 102, 230, 22, 150, 86, 214, 54, 182,
				118, 246, 14, 142, 78, 206, 46, 174, 110, 238,
				30, 158, 94, 222, 62, 190, 126, 254, 1, 129,
				65, 193, 33, 161, 97, 225, 17, 145, 81, 209,
				49, 177, 113, 241, 9, 137, 73, 201, 41, 169,
				105, 233, 25, 153, 89, 217, 57, 185, 121, 249,
				5, 133, 69, 197, 37, 165, 101, 229, 21, 149,
				85, 213, 53, 181, 117, 245, 13, 141, 77, 205,
				45, 173, 109, 237, 29, 157, 93, 221, 61, 189,
				125, 253, 3, 131, 67, 195, 35, 163, 99, 227,
				19, 147, 83, 211, 51, 179, 115, 243, 11, 139,
				75, 203, 43, 171, 107, 235, 27, 155, 91, 219,
				59, 187, 123, 251, 7, 135, 71, 199, 39, 167,
				103, 231, 23, 151, 87, 215, 55, 183, 119, 247,
				15, 143, 79, 207, 47, 175, 111, 239, 31, 159,
				95, 223, 63, 191, 127, byte.MaxValue
			};
			Tiff.TIFFNoBitRevTable = new byte[]
			{
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
				10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
				20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
				30, 31, 32, 33, 34, 35, 36, 37, 38, 39,
				40, 41, 42, 43, 44, 45, 46, 47, 48, 49,
				50, 51, 52, 53, 54, 55, 56, 57, 58, 59,
				60, 61, 62, 63, 64, 65, 66, 67, 68, 69,
				70, 71, 72, 73, 74, 75, 76, 77, 78, 79,
				80, 81, 82, 83, 84, 85, 86, 87, 88, 89,
				90, 91, 92, 93, 94, 95, 96, 97, 98, 99,
				100, 101, 102, 103, 104, 105, 106, 107, 108, 109,
				110, 111, 112, 113, 114, 115, 116, 117, 118, 119,
				120, 121, 122, 123, 124, 125, 126, 127, 128, 129,
				130, 131, 132, 133, 134, 135, 136, 137, 138, 139,
				140, 141, 142, 143, 144, 145, 146, 147, 148, 149,
				150, 151, 152, 153, 154, 155, 156, 157, 158, 159,
				160, 161, 162, 163, 164, 165, 166, 167, 168, 169,
				170, 171, 172, 173, 174, 175, 176, 177, 178, 179,
				180, 181, 182, 183, 184, 185, 186, 187, 188, 189,
				190, 191, 192, 193, 194, 195, 196, 197, 198, 199,
				200, 201, 202, 203, 204, 205, 206, 207, 208, 209,
				210, 211, 212, 213, 214, 215, 216, 217, 218, 219,
				220, 221, 222, 223, 224, 225, 226, 227, 228, 229,
				230, 231, 232, 233, 234, 235, 236, 237, 238, 239,
				240, 241, 242, 243, 244, 245, 246, 247, 248, 249,
				250, 251, 252, 253, 254, byte.MaxValue
			};
		}

		const int TIFF_VERSION = 42;

		const int TIFF_BIGTIFF_VERSION = 43;

		const short TIFF_BIGENDIAN = 19789;

		const short TIFF_LITTLEENDIAN = 18761;

		const short MDI_LITTLEENDIAN = 20549;

		const float D50_X0 = 96.425f;

		const float D50_Y0 = 100f;

		const float D50_Z0 = 82.468f;

		internal const int STRIP_SIZE_DEFAULT = 8192;

		internal const TiffFlags STRIPCHOP_DEFAULT = TiffFlags.STRIPCHOP;

		internal const bool DEFAULT_EXTRASAMPLE_AS_ALPHA = true;

		internal const bool CHECK_JPEG_YCBCR_SUBSAMPLING = true;

		const int NOSTRIP = -1;

		const int NOTILE = -1;

		internal const int O_RDONLY = 0;

		internal const int O_WRONLY = 1;

		internal const int O_CREAT = 256;

		internal const int O_TRUNC = 512;

		internal const int O_RDWR = 2;

		static TiffErrorHandler m_errorHandler;

		static Tiff.TiffExtendProc m_extender;

		static readonly TiffFieldInfo[] tiffFieldInfo = new TiffFieldInfo[]
		{
			new TiffFieldInfo(TiffTag.SUBFILETYPE, 1, 1, TiffType.LONG, 5, true, false, "SubfileType"),
			new TiffFieldInfo(TiffTag.SUBFILETYPE, 1, 1, TiffType.SHORT, 5, true, false, "SubfileType"),
			new TiffFieldInfo(TiffTag.OSUBFILETYPE, 1, 1, TiffType.SHORT, 5, true, false, "OldSubfileType"),
			new TiffFieldInfo(TiffTag.IMAGEWIDTH, 1, 1, TiffType.LONG, 1, false, false, "ImageWidth"),
			new TiffFieldInfo(TiffTag.IMAGEWIDTH, 1, 1, TiffType.SHORT, 1, false, false, "ImageWidth"),
			new TiffFieldInfo(TiffTag.IMAGELENGTH, 1, 1, TiffType.LONG, 1, true, false, "ImageLength"),
			new TiffFieldInfo(TiffTag.IMAGELENGTH, 1, 1, TiffType.SHORT, 1, true, false, "ImageLength"),
			new TiffFieldInfo(TiffTag.BITSPERSAMPLE, -1, -1, TiffType.SHORT, 6, false, false, "BitsPerSample"),
			new TiffFieldInfo(TiffTag.BITSPERSAMPLE, -1, -1, TiffType.LONG, 6, false, false, "BitsPerSample"),
			new TiffFieldInfo(TiffTag.COMPRESSION, -1, 1, TiffType.SHORT, 7, false, false, "Compression"),
			new TiffFieldInfo(TiffTag.COMPRESSION, -1, 1, TiffType.LONG, 7, false, false, "Compression"),
			new TiffFieldInfo(TiffTag.PHOTOMETRIC, 1, 1, TiffType.SHORT, 8, false, false, "PhotometricInterpretation"),
			new TiffFieldInfo(TiffTag.PHOTOMETRIC, 1, 1, TiffType.LONG, 8, false, false, "PhotometricInterpretation"),
			new TiffFieldInfo(TiffTag.THRESHHOLDING, 1, 1, TiffType.SHORT, 9, true, false, "Threshholding"),
			new TiffFieldInfo(TiffTag.CELLWIDTH, 1, 1, TiffType.SHORT, 0, true, false, "CellWidth"),
			new TiffFieldInfo(TiffTag.CELLLENGTH, 1, 1, TiffType.SHORT, 0, true, false, "CellLength"),
			new TiffFieldInfo(TiffTag.FILLORDER, 1, 1, TiffType.SHORT, 10, false, false, "FillOrder"),
			new TiffFieldInfo(TiffTag.DOCUMENTNAME, -1, -1, TiffType.ASCII, 65, true, false, "DocumentName"),
			new TiffFieldInfo(TiffTag.IMAGEDESCRIPTION, -1, -1, TiffType.ASCII, 65, true, false, "ImageDescription"),
			new TiffFieldInfo(TiffTag.MAKE, -1, -1, TiffType.ASCII, 65, true, false, "Make"),
			new TiffFieldInfo(TiffTag.MODEL, -1, -1, TiffType.ASCII, 65, true, false, "Model"),
			new TiffFieldInfo(TiffTag.STRIPOFFSETS, -1, -1, TiffType.LONG, 25, false, false, "StripOffsets"),
			new TiffFieldInfo(TiffTag.STRIPOFFSETS, -1, -1, TiffType.SHORT, 25, false, false, "StripOffsets"),
			new TiffFieldInfo(TiffTag.ORIENTATION, 1, 1, TiffType.SHORT, 15, false, false, "Orientation"),
			new TiffFieldInfo(TiffTag.SAMPLESPERPIXEL, 1, 1, TiffType.SHORT, 16, false, false, "SamplesPerPixel"),
			new TiffFieldInfo(TiffTag.ROWSPERSTRIP, 1, 1, TiffType.LONG, 17, false, false, "RowsPerStrip"),
			new TiffFieldInfo(TiffTag.ROWSPERSTRIP, 1, 1, TiffType.SHORT, 17, false, false, "RowsPerStrip"),
			new TiffFieldInfo(TiffTag.STRIPBYTECOUNTS, -1, -1, TiffType.LONG, 24, false, false, "StripByteCounts"),
			new TiffFieldInfo(TiffTag.STRIPBYTECOUNTS, -1, -1, TiffType.SHORT, 24, false, false, "StripByteCounts"),
			new TiffFieldInfo(TiffTag.MINSAMPLEVALUE, -2, -1, TiffType.SHORT, 18, true, false, "MinSampleValue"),
			new TiffFieldInfo(TiffTag.MAXSAMPLEVALUE, -2, -1, TiffType.SHORT, 19, true, false, "MaxSampleValue"),
			new TiffFieldInfo(TiffTag.XRESOLUTION, 1, 1, TiffType.RATIONAL, 3, true, false, "XResolution"),
			new TiffFieldInfo(TiffTag.YRESOLUTION, 1, 1, TiffType.RATIONAL, 3, true, false, "YResolution"),
			new TiffFieldInfo(TiffTag.PLANARCONFIG, 1, 1, TiffType.SHORT, 20, false, false, "PlanarConfiguration"),
			new TiffFieldInfo(TiffTag.PAGENAME, -1, -1, TiffType.ASCII, 65, true, false, "PageName"),
			new TiffFieldInfo(TiffTag.XPOSITION, 1, 1, TiffType.RATIONAL, 4, true, false, "XPosition"),
			new TiffFieldInfo(TiffTag.YPOSITION, 1, 1, TiffType.RATIONAL, 4, true, false, "YPosition"),
			new TiffFieldInfo(TiffTag.FREEOFFSETS, -1, -1, TiffType.LONG, 0, false, false, "FreeOffsets"),
			new TiffFieldInfo(TiffTag.FREEBYTECOUNTS, -1, -1, TiffType.LONG, 0, false, false, "FreeByteCounts"),
			new TiffFieldInfo(TiffTag.GRAYRESPONSEUNIT, 1, 1, TiffType.SHORT, 0, true, false, "GrayResponseUnit"),
			new TiffFieldInfo(TiffTag.GRAYRESPONSECURVE, -1, -1, TiffType.SHORT, 0, true, false, "GrayResponseCurve"),
			new TiffFieldInfo(TiffTag.RESOLUTIONUNIT, 1, 1, TiffType.SHORT, 22, true, false, "ResolutionUnit"),
			new TiffFieldInfo(TiffTag.PAGENUMBER, 2, 2, TiffType.SHORT, 23, true, false, "PageNumber"),
			new TiffFieldInfo(TiffTag.COLORRESPONSEUNIT, 1, 1, TiffType.SHORT, 0, true, false, "ColorResponseUnit"),
			new TiffFieldInfo(TiffTag.TRANSFERFUNCTION, -1, -1, TiffType.SHORT, 44, true, false, "TransferFunction"),
			new TiffFieldInfo(TiffTag.SOFTWARE, -1, -1, TiffType.ASCII, 65, true, false, "Software"),
			new TiffFieldInfo(TiffTag.DATETIME, 20, 20, TiffType.ASCII, 65, true, false, "DateTime"),
			new TiffFieldInfo(TiffTag.ARTIST, -1, -1, TiffType.ASCII, 65, true, false, "Artist"),
			new TiffFieldInfo(TiffTag.HOSTCOMPUTER, -1, -1, TiffType.ASCII, 65, true, false, "HostComputer"),
			new TiffFieldInfo(TiffTag.WHITEPOINT, 2, 2, TiffType.RATIONAL, 65, true, false, "WhitePoint"),
			new TiffFieldInfo(TiffTag.PRIMARYCHROMATICITIES, 6, 6, TiffType.RATIONAL, 65, true, false, "PrimaryChromaticities"),
			new TiffFieldInfo(TiffTag.COLORMAP, -1, -1, TiffType.SHORT, 26, true, false, "ColorMap"),
			new TiffFieldInfo(TiffTag.HALFTONEHINTS, 2, 2, TiffType.SHORT, 37, true, false, "HalftoneHints"),
			new TiffFieldInfo(TiffTag.TILEWIDTH, 1, 1, TiffType.LONG, 2, false, false, "TileWidth"),
			new TiffFieldInfo(TiffTag.TILEWIDTH, 1, 1, TiffType.SHORT, 2, false, false, "TileWidth"),
			new TiffFieldInfo(TiffTag.TILELENGTH, 1, 1, TiffType.LONG, 2, false, false, "TileLength"),
			new TiffFieldInfo(TiffTag.TILELENGTH, 1, 1, TiffType.SHORT, 2, false, false, "TileLength"),
			new TiffFieldInfo(TiffTag.TILEOFFSETS, -1, 1, TiffType.LONG, 25, false, false, "TileOffsets"),
			new TiffFieldInfo(TiffTag.TILEBYTECOUNTS, -1, 1, TiffType.LONG, 24, false, false, "TileByteCounts"),
			new TiffFieldInfo(TiffTag.TILEBYTECOUNTS, -1, 1, TiffType.SHORT, 24, false, false, "TileByteCounts"),
			new TiffFieldInfo(TiffTag.SUBIFD, -1, -1, TiffType.IFD, 49, true, true, "SubIFD"),
			new TiffFieldInfo(TiffTag.SUBIFD, -1, -1, TiffType.LONG, 49, true, true, "SubIFD"),
			new TiffFieldInfo(TiffTag.INKSET, 1, 1, TiffType.SHORT, 65, false, false, "InkSet"),
			new TiffFieldInfo(TiffTag.INKNAMES, -1, -1, TiffType.ASCII, 46, true, true, "InkNames"),
			new TiffFieldInfo(TiffTag.NUMBEROFINKS, 1, 1, TiffType.SHORT, 65, true, false, "NumberOfInks"),
			new TiffFieldInfo(TiffTag.DOTRANGE, 2, 2, TiffType.SHORT, 65, false, false, "DotRange"),
			new TiffFieldInfo(TiffTag.DOTRANGE, 2, 2, TiffType.BYTE, 65, false, false, "DotRange"),
			new TiffFieldInfo(TiffTag.TARGETPRINTER, -1, -1, TiffType.ASCII, 65, true, false, "TargetPrinter"),
			new TiffFieldInfo(TiffTag.EXTRASAMPLES, -1, -1, TiffType.SHORT, 31, false, true, "ExtraSamples"),
			new TiffFieldInfo(TiffTag.EXTRASAMPLES, -1, -1, TiffType.BYTE, 31, false, true, "ExtraSamples"),
			new TiffFieldInfo(TiffTag.SAMPLEFORMAT, -1, -1, TiffType.SHORT, 32, false, false, "SampleFormat"),
			new TiffFieldInfo(TiffTag.SMINSAMPLEVALUE, -2, -1, TiffType.NOTYPE, 33, true, false, "SMinSampleValue"),
			new TiffFieldInfo(TiffTag.SMAXSAMPLEVALUE, -2, -1, TiffType.NOTYPE, 34, true, false, "SMaxSampleValue"),
			new TiffFieldInfo(TiffTag.CLIPPATH, -1, -3, TiffType.BYTE, 65, false, true, "ClipPath"),
			new TiffFieldInfo(TiffTag.XCLIPPATHUNITS, 1, 1, TiffType.SLONG, 65, false, false, "XClipPathUnits"),
			new TiffFieldInfo(TiffTag.XCLIPPATHUNITS, 1, 1, TiffType.SSHORT, 65, false, false, "XClipPathUnits"),
			new TiffFieldInfo(TiffTag.XCLIPPATHUNITS, 1, 1, TiffType.SBYTE, 65, false, false, "XClipPathUnits"),
			new TiffFieldInfo(TiffTag.YCLIPPATHUNITS, 1, 1, TiffType.SLONG, 65, false, false, "YClipPathUnits"),
			new TiffFieldInfo(TiffTag.YCLIPPATHUNITS, 1, 1, TiffType.SSHORT, 65, false, false, "YClipPathUnits"),
			new TiffFieldInfo(TiffTag.YCLIPPATHUNITS, 1, 1, TiffType.SBYTE, 65, false, false, "YClipPathUnits"),
			new TiffFieldInfo(TiffTag.YCBCRCOEFFICIENTS, 3, 3, TiffType.RATIONAL, 65, false, false, "YCbCrCoefficients"),
			new TiffFieldInfo(TiffTag.YCBCRSUBSAMPLING, 2, 2, TiffType.SHORT, 39, false, false, "YCbCrSubsampling"),
			new TiffFieldInfo(TiffTag.YCBCRPOSITIONING, 1, 1, TiffType.SHORT, 40, false, false, "YCbCrPositioning"),
			new TiffFieldInfo(TiffTag.REFERENCEBLACKWHITE, 6, 6, TiffType.RATIONAL, 41, true, false, "ReferenceBlackWhite"),
			new TiffFieldInfo(TiffTag.REFERENCEBLACKWHITE, 6, 6, TiffType.LONG, 41, true, false, "ReferenceBlackWhite"),
			new TiffFieldInfo(TiffTag.XMLPACKET, -3, -3, TiffType.BYTE, 65, false, true, "XMLPacket"),
			new TiffFieldInfo(TiffTag.MATTEING, 1, 1, TiffType.SHORT, 31, false, false, "Matteing"),
			new TiffFieldInfo(TiffTag.DATATYPE, -2, -1, TiffType.SHORT, 32, false, false, "DataType"),
			new TiffFieldInfo(TiffTag.IMAGEDEPTH, 1, 1, TiffType.LONG, 35, false, false, "ImageDepth"),
			new TiffFieldInfo(TiffTag.IMAGEDEPTH, 1, 1, TiffType.SHORT, 35, false, false, "ImageDepth"),
			new TiffFieldInfo(TiffTag.TILEDEPTH, 1, 1, TiffType.LONG, 36, false, false, "TileDepth"),
			new TiffFieldInfo(TiffTag.TILEDEPTH, 1, 1, TiffType.SHORT, 36, false, false, "TileDepth"),
			new TiffFieldInfo(TiffTag.PIXAR_IMAGEFULLWIDTH, 1, 1, TiffType.LONG, 65, true, false, "ImageFullWidth"),
			new TiffFieldInfo(TiffTag.PIXAR_IMAGEFULLLENGTH, 1, 1, TiffType.LONG, 65, true, false, "ImageFullLength"),
			new TiffFieldInfo(TiffTag.PIXAR_TEXTUREFORMAT, -1, -1, TiffType.ASCII, 65, true, false, "TextureFormat"),
			new TiffFieldInfo(TiffTag.PIXAR_WRAPMODES, -1, -1, TiffType.ASCII, 65, true, false, "TextureWrapModes"),
			new TiffFieldInfo(TiffTag.PIXAR_FOVCOT, 1, 1, TiffType.FLOAT, 65, true, false, "FieldOfViewCotangent"),
			new TiffFieldInfo(TiffTag.PIXAR_MATRIX_WORLDTOSCREEN, 16, 16, TiffType.FLOAT, 65, true, false, "MatrixWorldToScreen"),
			new TiffFieldInfo(TiffTag.PIXAR_MATRIX_WORLDTOCAMERA, 16, 16, TiffType.FLOAT, 65, true, false, "MatrixWorldToCamera"),
			new TiffFieldInfo(TiffTag.COPYRIGHT, -1, -1, TiffType.ASCII, 65, true, false, "Copyright"),
			new TiffFieldInfo(TiffTag.RICHTIFFIPTC, -3, -3, TiffType.LONG, 65, false, true, "RichTIFFIPTC"),
			new TiffFieldInfo(TiffTag.PHOTOSHOP, -3, -3, TiffType.BYTE, 65, false, true, "Photoshop"),
			new TiffFieldInfo(TiffTag.EXIFIFD, 1, 1, TiffType.LONG, 65, false, false, "EXIFIFDOffset"),
			new TiffFieldInfo(TiffTag.ICCPROFILE, -3, -3, TiffType.UNDEFINED, 65, false, true, "ICC Profile"),
			new TiffFieldInfo(TiffTag.GPSIFD, 1, 1, TiffType.LONG, 65, false, false, "GPSIFDOffset"),
			new TiffFieldInfo(TiffTag.STONITS, 1, 1, TiffType.DOUBLE, 65, false, false, "StoNits"),
			new TiffFieldInfo(TiffTag.INTEROPERABILITYIFD, 1, 1, TiffType.LONG, 65, false, false, "InteroperabilityIFDOffset"),
			new TiffFieldInfo(TiffTag.DNGVERSION, 4, 4, TiffType.BYTE, 65, false, false, "DNGVersion"),
			new TiffFieldInfo(TiffTag.DNGBACKWARDVERSION, 4, 4, TiffType.BYTE, 65, false, false, "DNGBackwardVersion"),
			new TiffFieldInfo(TiffTag.UNIQUECAMERAMODEL, -1, -1, TiffType.ASCII, 65, true, false, "UniqueCameraModel"),
			new TiffFieldInfo(TiffTag.LOCALIZEDCAMERAMODEL, -1, -1, TiffType.ASCII, 65, true, false, "LocalizedCameraModel"),
			new TiffFieldInfo(TiffTag.LOCALIZEDCAMERAMODEL, -1, -1, TiffType.BYTE, 65, true, true, "LocalizedCameraModel"),
			new TiffFieldInfo(TiffTag.CFAPLANECOLOR, -1, -1, TiffType.BYTE, 65, false, true, "CFAPlaneColor"),
			new TiffFieldInfo(TiffTag.CFALAYOUT, 1, 1, TiffType.SHORT, 65, false, false, "CFALayout"),
			new TiffFieldInfo(TiffTag.LINEARIZATIONTABLE, -1, -1, TiffType.SHORT, 65, false, true, "LinearizationTable"),
			new TiffFieldInfo(TiffTag.BLACKLEVELREPEATDIM, 2, 2, TiffType.SHORT, 65, false, false, "BlackLevelRepeatDim"),
			new TiffFieldInfo(TiffTag.BLACKLEVEL, -1, -1, TiffType.LONG, 65, false, true, "BlackLevel"),
			new TiffFieldInfo(TiffTag.BLACKLEVEL, -1, -1, TiffType.SHORT, 65, false, true, "BlackLevel"),
			new TiffFieldInfo(TiffTag.BLACKLEVEL, -1, -1, TiffType.RATIONAL, 65, false, true, "BlackLevel"),
			new TiffFieldInfo(TiffTag.BLACKLEVELDELTAH, -1, -1, TiffType.SRATIONAL, 65, false, true, "BlackLevelDeltaH"),
			new TiffFieldInfo(TiffTag.BLACKLEVELDELTAV, -1, -1, TiffType.SRATIONAL, 65, false, true, "BlackLevelDeltaV"),
			new TiffFieldInfo(TiffTag.WHITELEVEL, -2, -2, TiffType.LONG, 65, false, false, "WhiteLevel"),
			new TiffFieldInfo(TiffTag.WHITELEVEL, -2, -2, TiffType.SHORT, 65, false, false, "WhiteLevel"),
			new TiffFieldInfo(TiffTag.DEFAULTSCALE, 2, 2, TiffType.RATIONAL, 65, false, false, "DefaultScale"),
			new TiffFieldInfo(TiffTag.BESTQUALITYSCALE, 1, 1, TiffType.RATIONAL, 65, false, false, "BestQualityScale"),
			new TiffFieldInfo(TiffTag.DEFAULTCROPORIGIN, 2, 2, TiffType.LONG, 65, false, false, "DefaultCropOrigin"),
			new TiffFieldInfo(TiffTag.DEFAULTCROPORIGIN, 2, 2, TiffType.SHORT, 65, false, false, "DefaultCropOrigin"),
			new TiffFieldInfo(TiffTag.DEFAULTCROPORIGIN, 2, 2, TiffType.RATIONAL, 65, false, false, "DefaultCropOrigin"),
			new TiffFieldInfo(TiffTag.DEFAULTCROPSIZE, 2, 2, TiffType.LONG, 65, false, false, "DefaultCropSize"),
			new TiffFieldInfo(TiffTag.DEFAULTCROPSIZE, 2, 2, TiffType.SHORT, 65, false, false, "DefaultCropSize"),
			new TiffFieldInfo(TiffTag.DEFAULTCROPSIZE, 2, 2, TiffType.RATIONAL, 65, false, false, "DefaultCropSize"),
			new TiffFieldInfo(TiffTag.COLORMATRIX1, -1, -1, TiffType.SRATIONAL, 65, false, true, "ColorMatrix1"),
			new TiffFieldInfo(TiffTag.COLORMATRIX2, -1, -1, TiffType.SRATIONAL, 65, false, true, "ColorMatrix2"),
			new TiffFieldInfo(TiffTag.CAMERACALIBRATION1, -1, -1, TiffType.SRATIONAL, 65, false, true, "CameraCalibration1"),
			new TiffFieldInfo(TiffTag.CAMERACALIBRATION2, -1, -1, TiffType.SRATIONAL, 65, false, true, "CameraCalibration2"),
			new TiffFieldInfo(TiffTag.REDUCTIONMATRIX1, -1, -1, TiffType.SRATIONAL, 65, false, true, "ReductionMatrix1"),
			new TiffFieldInfo(TiffTag.REDUCTIONMATRIX2, -1, -1, TiffType.SRATIONAL, 65, false, true, "ReductionMatrix2"),
			new TiffFieldInfo(TiffTag.ANALOGBALANCE, -1, -1, TiffType.RATIONAL, 65, false, true, "AnalogBalance"),
			new TiffFieldInfo(TiffTag.ASSHOTNEUTRAL, -1, -1, TiffType.SHORT, 65, false, true, "AsShotNeutral"),
			new TiffFieldInfo(TiffTag.ASSHOTNEUTRAL, -1, -1, TiffType.RATIONAL, 65, false, true, "AsShotNeutral"),
			new TiffFieldInfo(TiffTag.ASSHOTWHITEXY, 2, 2, TiffType.RATIONAL, 65, false, false, "AsShotWhiteXY"),
			new TiffFieldInfo(TiffTag.BASELINEEXPOSURE, 1, 1, TiffType.SRATIONAL, 65, false, false, "BaselineExposure"),
			new TiffFieldInfo(TiffTag.BASELINENOISE, 1, 1, TiffType.RATIONAL, 65, false, false, "BaselineNoise"),
			new TiffFieldInfo(TiffTag.BASELINESHARPNESS, 1, 1, TiffType.RATIONAL, 65, false, false, "BaselineSharpness"),
			new TiffFieldInfo(TiffTag.BAYERGREENSPLIT, 1, 1, TiffType.LONG, 65, false, false, "BayerGreenSplit"),
			new TiffFieldInfo(TiffTag.LINEARRESPONSELIMIT, 1, 1, TiffType.RATIONAL, 65, false, false, "LinearResponseLimit"),
			new TiffFieldInfo(TiffTag.CAMERASERIALNUMBER, -1, -1, TiffType.ASCII, 65, true, false, "CameraSerialNumber"),
			new TiffFieldInfo(TiffTag.LENSINFO, 4, 4, TiffType.RATIONAL, 65, false, false, "LensInfo"),
			new TiffFieldInfo(TiffTag.CHROMABLURRADIUS, 1, 1, TiffType.RATIONAL, 65, false, false, "ChromaBlurRadius"),
			new TiffFieldInfo(TiffTag.ANTIALIASSTRENGTH, 1, 1, TiffType.RATIONAL, 65, false, false, "AntiAliasStrength"),
			new TiffFieldInfo(TiffTag.SHADOWSCALE, 1, 1, TiffType.RATIONAL, 65, false, false, "ShadowScale"),
			new TiffFieldInfo(TiffTag.DNGPRIVATEDATA, -1, -1, TiffType.BYTE, 65, false, true, "DNGPrivateData"),
			new TiffFieldInfo(TiffTag.MAKERNOTESAFETY, 1, 1, TiffType.SHORT, 65, false, false, "MakerNoteSafety"),
			new TiffFieldInfo(TiffTag.CALIBRATIONILLUMINANT1, 1, 1, TiffType.SHORT, 65, false, false, "CalibrationIlluminant1"),
			new TiffFieldInfo(TiffTag.CALIBRATIONILLUMINANT2, 1, 1, TiffType.SHORT, 65, false, false, "CalibrationIlluminant2"),
			new TiffFieldInfo(TiffTag.RAWDATAUNIQUEID, 16, 16, TiffType.BYTE, 65, false, false, "RawDataUniqueID"),
			new TiffFieldInfo(TiffTag.ORIGINALRAWFILENAME, -1, -1, TiffType.ASCII, 65, true, false, "OriginalRawFileName"),
			new TiffFieldInfo(TiffTag.ORIGINALRAWFILENAME, -1, -1, TiffType.BYTE, 65, true, true, "OriginalRawFileName"),
			new TiffFieldInfo(TiffTag.ORIGINALRAWFILEDATA, -1, -1, TiffType.UNDEFINED, 65, false, true, "OriginalRawFileData"),
			new TiffFieldInfo(TiffTag.ACTIVEAREA, 4, 4, TiffType.LONG, 65, false, false, "ActiveArea"),
			new TiffFieldInfo(TiffTag.ACTIVEAREA, 4, 4, TiffType.SHORT, 65, false, false, "ActiveArea"),
			new TiffFieldInfo(TiffTag.MASKEDAREAS, -1, -1, TiffType.LONG, 65, false, true, "MaskedAreas"),
			new TiffFieldInfo(TiffTag.ASSHOTICCPROFILE, -1, -1, TiffType.UNDEFINED, 65, false, true, "AsShotICCProfile"),
			new TiffFieldInfo(TiffTag.ASSHOTPREPROFILEMATRIX, -1, -1, TiffType.SRATIONAL, 65, false, true, "AsShotPreProfileMatrix"),
			new TiffFieldInfo(TiffTag.CURRENTICCPROFILE, -1, -1, TiffType.UNDEFINED, 65, false, true, "CurrentICCProfile"),
			new TiffFieldInfo(TiffTag.CURRENTPREPROFILEMATRIX, -1, -1, TiffType.SRATIONAL, 65, false, true, "CurrentPreProfileMatrix")
		};

		static readonly TiffFieldInfo[] exifFieldInfo = new TiffFieldInfo[]
		{
			new TiffFieldInfo(TiffTag.EXIF_EXPOSURETIME, 1, 1, TiffType.RATIONAL, 65, true, false, "ExposureTime"),
			new TiffFieldInfo(TiffTag.EXIF_FNUMBER, 1, 1, TiffType.RATIONAL, 65, true, false, "FNumber"),
			new TiffFieldInfo(TiffTag.EXIF_EXPOSUREPROGRAM, 1, 1, TiffType.SHORT, 65, true, false, "ExposureProgram"),
			new TiffFieldInfo(TiffTag.EXIF_SPECTRALSENSITIVITY, -1, -1, TiffType.ASCII, 65, true, false, "SpectralSensitivity"),
			new TiffFieldInfo(TiffTag.EXIF_ISOSPEEDRATINGS, -1, -1, TiffType.SHORT, 65, true, true, "ISOSpeedRatings"),
			new TiffFieldInfo(TiffTag.EXIF_OECF, -1, -1, TiffType.UNDEFINED, 65, true, true, "OptoelectricConversionFactor"),
			new TiffFieldInfo(TiffTag.EXIF_EXIFVERSION, 4, 4, TiffType.UNDEFINED, 65, true, false, "ExifVersion"),
			new TiffFieldInfo(TiffTag.EXIF_DATETIMEORIGINAL, 20, 20, TiffType.ASCII, 65, true, false, "DateTimeOriginal"),
			new TiffFieldInfo(TiffTag.EXIF_DATETIMEDIGITIZED, 20, 20, TiffType.ASCII, 65, true, false, "DateTimeDigitized"),
			new TiffFieldInfo(TiffTag.EXIF_COMPONENTSCONFIGURATION, 4, 4, TiffType.UNDEFINED, 65, true, false, "ComponentsConfiguration"),
			new TiffFieldInfo(TiffTag.EXIF_COMPRESSEDBITSPERPIXEL, 1, 1, TiffType.RATIONAL, 65, true, false, "CompressedBitsPerPixel"),
			new TiffFieldInfo(TiffTag.EXIF_SHUTTERSPEEDVALUE, 1, 1, TiffType.SRATIONAL, 65, true, false, "ShutterSpeedValue"),
			new TiffFieldInfo(TiffTag.EXIF_APERTUREVALUE, 1, 1, TiffType.RATIONAL, 65, true, false, "ApertureValue"),
			new TiffFieldInfo(TiffTag.EXIF_BRIGHTNESSVALUE, 1, 1, TiffType.SRATIONAL, 65, true, false, "BrightnessValue"),
			new TiffFieldInfo(TiffTag.EXIF_EXPOSUREBIASVALUE, 1, 1, TiffType.SRATIONAL, 65, true, false, "ExposureBiasValue"),
			new TiffFieldInfo(TiffTag.EXIF_MAXAPERTUREVALUE, 1, 1, TiffType.RATIONAL, 65, true, false, "MaxApertureValue"),
			new TiffFieldInfo(TiffTag.EXIF_SUBJECTDISTANCE, 1, 1, TiffType.RATIONAL, 65, true, false, "SubjectDistance"),
			new TiffFieldInfo(TiffTag.EXIF_METERINGMODE, 1, 1, TiffType.SHORT, 65, true, false, "MeteringMode"),
			new TiffFieldInfo(TiffTag.EXIF_LIGHTSOURCE, 1, 1, TiffType.SHORT, 65, true, false, "LightSource"),
			new TiffFieldInfo(TiffTag.EXIF_FLASH, 1, 1, TiffType.SHORT, 65, true, false, "Flash"),
			new TiffFieldInfo(TiffTag.EXIF_FOCALLENGTH, 1, 1, TiffType.RATIONAL, 65, true, false, "FocalLength"),
			new TiffFieldInfo(TiffTag.EXIF_SUBJECTAREA, -1, -1, TiffType.SHORT, 65, true, true, "SubjectArea"),
			new TiffFieldInfo(TiffTag.EXIF_MAKERNOTE, -1, -1, TiffType.UNDEFINED, 65, true, true, "MakerNote"),
			new TiffFieldInfo(TiffTag.EXIF_USERCOMMENT, -1, -1, TiffType.UNDEFINED, 65, true, true, "UserComment"),
			new TiffFieldInfo(TiffTag.EXIF_SUBSECTIME, -1, -1, TiffType.ASCII, 65, true, false, "SubSecTime"),
			new TiffFieldInfo(TiffTag.EXIF_SUBSECTIMEORIGINAL, -1, -1, TiffType.ASCII, 65, true, false, "SubSecTimeOriginal"),
			new TiffFieldInfo(TiffTag.EXIF_SUBSECTIMEDIGITIZED, -1, -1, TiffType.ASCII, 65, true, false, "SubSecTimeDigitized"),
			new TiffFieldInfo(TiffTag.EXIF_FLASHPIXVERSION, 4, 4, TiffType.UNDEFINED, 65, true, false, "FlashpixVersion"),
			new TiffFieldInfo(TiffTag.EXIF_COLORSPACE, 1, 1, TiffType.SHORT, 65, true, false, "ColorSpace"),
			new TiffFieldInfo(TiffTag.EXIF_PIXELXDIMENSION, 1, 1, TiffType.LONG, 65, true, false, "PixelXDimension"),
			new TiffFieldInfo(TiffTag.EXIF_PIXELXDIMENSION, 1, 1, TiffType.SHORT, 65, true, false, "PixelXDimension"),
			new TiffFieldInfo(TiffTag.EXIF_PIXELYDIMENSION, 1, 1, TiffType.LONG, 65, true, false, "PixelYDimension"),
			new TiffFieldInfo(TiffTag.EXIF_PIXELYDIMENSION, 1, 1, TiffType.SHORT, 65, true, false, "PixelYDimension"),
			new TiffFieldInfo(TiffTag.EXIF_RELATEDSOUNDFILE, 13, 13, TiffType.ASCII, 65, true, false, "RelatedSoundFile"),
			new TiffFieldInfo(TiffTag.EXIF_FLASHENERGY, 1, 1, TiffType.RATIONAL, 65, true, false, "FlashEnergy"),
			new TiffFieldInfo(TiffTag.EXIF_SPATIALFREQUENCYRESPONSE, -1, -1, TiffType.UNDEFINED, 65, true, true, "SpatialFrequencyResponse"),
			new TiffFieldInfo(TiffTag.EXIF_FOCALPLANEXRESOLUTION, 1, 1, TiffType.RATIONAL, 65, true, false, "FocalPlaneXResolution"),
			new TiffFieldInfo(TiffTag.EXIF_FOCALPLANEYRESOLUTION, 1, 1, TiffType.RATIONAL, 65, true, false, "FocalPlaneYResolution"),
			new TiffFieldInfo(TiffTag.EXIF_FOCALPLANERESOLUTIONUNIT, 1, 1, TiffType.SHORT, 65, true, false, "FocalPlaneResolutionUnit"),
			new TiffFieldInfo(TiffTag.EXIF_SUBJECTLOCATION, 2, 2, TiffType.SHORT, 65, true, false, "SubjectLocation"),
			new TiffFieldInfo(TiffTag.EXIF_EXPOSUREINDEX, 1, 1, TiffType.RATIONAL, 65, true, false, "ExposureIndex"),
			new TiffFieldInfo(TiffTag.EXIF_SENSINGMETHOD, 1, 1, TiffType.SHORT, 65, true, false, "SensingMethod"),
			new TiffFieldInfo(TiffTag.EXIF_FILESOURCE, 1, 1, TiffType.UNDEFINED, 65, true, false, "FileSource"),
			new TiffFieldInfo(TiffTag.EXIF_SCENETYPE, 1, 1, TiffType.UNDEFINED, 65, true, false, "SceneType"),
			new TiffFieldInfo(TiffTag.EXIF_CFAPATTERN, -1, -1, TiffType.UNDEFINED, 65, true, true, "CFAPattern"),
			new TiffFieldInfo(TiffTag.EXIF_CUSTOMRENDERED, 1, 1, TiffType.SHORT, 65, true, false, "CustomRendered"),
			new TiffFieldInfo(TiffTag.EXIF_EXPOSUREMODE, 1, 1, TiffType.SHORT, 65, true, false, "ExposureMode"),
			new TiffFieldInfo(TiffTag.EXIF_WHITEBALANCE, 1, 1, TiffType.SHORT, 65, true, false, "WhiteBalance"),
			new TiffFieldInfo(TiffTag.EXIF_DIGITALZOOMRATIO, 1, 1, TiffType.RATIONAL, 65, true, false, "DigitalZoomRatio"),
			new TiffFieldInfo(TiffTag.EXIF_FOCALLENGTHIN35MMFILM, 1, 1, TiffType.SHORT, 65, true, false, "FocalLengthIn35mmFilm"),
			new TiffFieldInfo(TiffTag.EXIF_SCENECAPTURETYPE, 1, 1, TiffType.SHORT, 65, true, false, "SceneCaptureType"),
			new TiffFieldInfo(TiffTag.EXIF_GAINCONTROL, 1, 1, TiffType.RATIONAL, 65, true, false, "GainControl"),
			new TiffFieldInfo(TiffTag.EXIF_CONTRAST, 1, 1, TiffType.SHORT, 65, true, false, "Contrast"),
			new TiffFieldInfo(TiffTag.EXIF_SATURATION, 1, 1, TiffType.SHORT, 65, true, false, "Saturation"),
			new TiffFieldInfo(TiffTag.EXIF_SHARPNESS, 1, 1, TiffType.SHORT, 65, true, false, "Sharpness"),
			new TiffFieldInfo(TiffTag.EXIF_DEVICESETTINGDESCRIPTION, -1, -1, TiffType.UNDEFINED, 65, true, true, "DeviceSettingDescription"),
			new TiffFieldInfo(TiffTag.EXIF_SUBJECTDISTANCERANGE, 1, 1, TiffType.SHORT, 65, true, false, "SubjectDistanceRange"),
			new TiffFieldInfo(TiffTag.EXIF_IMAGEUNIQUEID, 33, 33, TiffType.ASCII, 65, true, false, "ImageUniqueID")
		};

		internal static readonly Encoding Latin1Encoding = Encoding.GetEncoding("Latin1");

		internal string m_name;

		internal int m_mode;

		internal TiffFlags m_flags;

		internal uint m_diroff;

		internal TiffDirectory m_dir;

		internal int m_row;

		internal int m_curstrip;

		internal int m_curtile;

		internal int m_tilesize;

		internal TiffCodec m_currentCodec;

		internal int m_scanlinesize;

		internal byte[] m_rawdata;

		internal int m_rawdatasize;

		internal int m_rawcp;

		internal int m_rawcc;

		internal object m_clientdata;

		internal Tiff.PostDecodeMethodType m_postDecodeMethod;

		internal TiffTagMethods m_tagmethods;

		uint m_nextdiroff;

		uint[] m_dirlist;

		int m_dirlistsize;

		short m_dirnumber;

		TiffHeader m_header;

		int[] m_typeshift;

		uint[] m_typemask;

		short m_curdir;

		uint m_curoff;

		uint m_dataoff;

		short m_nsubifd;

		uint m_subifdoff;

		int m_col;

		bool m_decodestatus;

		TiffFieldInfo[] m_fieldinfo;

		int m_nfields;

		TiffFieldInfo m_foundfield;

		Tiff.clientInfoLink m_clientinfo;

		TiffCodec[] m_builtInCodecs;

		Tiff.codecList m_registeredCodecs;

		TiffTagMethods m_defaultTagMethods;

		bool m_disposed;

		Stream m_fileStream;

		TiffStream m_stream;

		static readonly uint[] typemask = new uint[]
		{
			0U, 255U, uint.MaxValue, 65535U, uint.MaxValue, uint.MaxValue, 255U, 255U, 65535U, uint.MaxValue,
			uint.MaxValue, uint.MaxValue, uint.MaxValue
		};

		static readonly int[] bigTypeshift = new int[]
		{
			0, 24, 0, 16, 0, 0, 24, 24, 16, 0,
			0, 0, 0
		};

		static readonly int[] litTypeshift;

		static readonly string[] photoNames;

		static readonly string[] orientNames;

		static readonly byte[] TIFFBitRevTable;

		static readonly byte[] TIFFNoBitRevTable;

		internal enum PostDecodeMethodType
		{
			pdmNone,
			pdmSwab16Bit,
			pdmSwab24Bit,
			pdmSwab32Bit,
			pdmSwab64Bit
		}

		class codecList
		{
			public Tiff.codecList next;

			public TiffCodec codec;
		}

		class clientInfoLink
		{
			public Tiff.clientInfoLink next;

			public object data;

			public string name;
		}

		internal delegate void TiffExtendProc(Tiff tif);

		internal delegate void FaxFillFunc(byte[] buffer, int offset, int[] runs, int thisRunOffset, int nextRunOffset, int width);
	}
}
