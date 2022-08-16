using System;
using System.Globalization;
using BitMiracle.LibTiff.Classic.Internal;

namespace BitMiracle.LibTiff.Classic
{
	class TiffRgbaImage
	{
		TiffRgbaImage()
		{
		}

		public static TiffRgbaImage Create(Tiff tif, bool stopOnError, out string errorMsg)
		{
			errorMsg = null;
			TiffRgbaImage tiffRgbaImage = new TiffRgbaImage();
			tiffRgbaImage.row_offset = 0;
			tiffRgbaImage.col_offset = 0;
			tiffRgbaImage.redcmap = null;
			tiffRgbaImage.greencmap = null;
			tiffRgbaImage.bluecmap = null;
			tiffRgbaImage.req_orientation = Orientation.BOTLEFT;
			tiffRgbaImage.tif = tif;
			tiffRgbaImage.stoponerr = stopOnError;
			FieldValue[] array = tif.GetFieldDefaulted(TiffTag.BITSPERSAMPLE);
			tiffRgbaImage.bitspersample = array[0].ToShort();
			short num = tiffRgbaImage.bitspersample;
			switch (num)
			{
			case 1:
			case 2:
			case 4:
				goto IL_B7;
			case 3:
				break;
			default:
				if (num == 8 || num == 16)
				{
					goto IL_B7;
				}
				break;
			}
			errorMsg = string.Format(CultureInfo.InvariantCulture, "Sorry, can not handle images with {0}-bit samples", new object[] { tiffRgbaImage.bitspersample });
			return null;
			IL_B7:
			tiffRgbaImage.alpha = ExtraSample.UNSPECIFIED;
			array = tif.GetFieldDefaulted(TiffTag.SAMPLESPERPIXEL);
			tiffRgbaImage.samplesperpixel = array[0].ToShort();
			array = tif.GetFieldDefaulted(TiffTag.EXTRASAMPLES);
			short num2 = array[0].ToShort();
			byte[] array2 = array[1].ToByteArray();
			if (num2 >= 1)
			{
				switch (array2[0])
				{
				case 0:
					if (tiffRgbaImage.samplesperpixel > 3)
					{
						tiffRgbaImage.alpha = ExtraSample.ASSOCALPHA;
					}
					break;
				case 1:
				case 2:
					tiffRgbaImage.alpha = (ExtraSample)array2[0];
					break;
				}
			}
			if (tif.GetField(TiffTag.PHOTOMETRIC) == null)
			{
				tiffRgbaImage.photometric = Photometric.MINISWHITE;
			}
			if (num2 == 0 && tiffRgbaImage.samplesperpixel == 4 && tiffRgbaImage.photometric == Photometric.RGB)
			{
				tiffRgbaImage.alpha = ExtraSample.ASSOCALPHA;
				num2 = 1;
			}
			int num3 = (int)(tiffRgbaImage.samplesperpixel - num2);
			array = tif.GetFieldDefaulted(TiffTag.COMPRESSION);
			Compression compression = (Compression)array[0].ToInt();
			array = tif.GetFieldDefaulted(TiffTag.PLANARCONFIG);
			PlanarConfig planarConfig = (PlanarConfig)array[0].ToShort();
			array = tif.GetField(TiffTag.PHOTOMETRIC);
			if (array == null)
			{
				switch (num3)
				{
				case 1:
					if (tiffRgbaImage.isCCITTCompression())
					{
						tiffRgbaImage.photometric = Photometric.MINISWHITE;
						goto IL_232;
					}
					tiffRgbaImage.photometric = Photometric.MINISBLACK;
					goto IL_232;
				case 3:
					tiffRgbaImage.photometric = Photometric.RGB;
					goto IL_232;
				}
				errorMsg = string.Format(CultureInfo.InvariantCulture, "Missing needed {0} tag", new object[] { "PhotometricInterpretation" });
				return null;
			}
			tiffRgbaImage.photometric = (Photometric)array[0].ToInt();
			IL_232:
			Photometric photometric = tiffRgbaImage.photometric;
			switch (photometric)
			{
			case Photometric.MINISWHITE:
			case Photometric.MINISBLACK:
				if (planarConfig == PlanarConfig.CONTIG && tiffRgbaImage.samplesperpixel != 1 && tiffRgbaImage.bitspersample < 8)
				{
					errorMsg = string.Format(CultureInfo.InvariantCulture, "Sorry, can not handle contiguous data with {0}={1}, and {2}={3} and Bits/Sample={4}", new object[] { "PhotometricInterpretation", tiffRgbaImage.photometric, "Samples/pixel", tiffRgbaImage.samplesperpixel, tiffRgbaImage.bitspersample });
					return null;
				}
				goto IL_69F;
			case Photometric.RGB:
				if (num3 < 3)
				{
					errorMsg = string.Format(CultureInfo.InvariantCulture, "Sorry, can not handle RGB image with {0}={1}", new object[] { "Color channels", num3 });
					return null;
				}
				goto IL_69F;
			case Photometric.PALETTE:
			{
				array = tif.GetField(TiffTag.COLORMAP);
				if (array == null)
				{
					errorMsg = string.Format(CultureInfo.InvariantCulture, "Missing required \"Colormap\" tag", new object[0]);
					return null;
				}
				short[] src = array[0].ToShortArray();
				short[] src2 = array[1].ToShortArray();
				short[] src3 = array[2].ToShortArray();
				int num4 = 1 << (int)tiffRgbaImage.bitspersample;
				tiffRgbaImage.redcmap = new short[num4];
				tiffRgbaImage.greencmap = new short[num4];
				tiffRgbaImage.bluecmap = new short[num4];
				Buffer.BlockCopy(src, 0, tiffRgbaImage.redcmap, 0, num4 * 2);
				Buffer.BlockCopy(src2, 0, tiffRgbaImage.greencmap, 0, num4 * 2);
				Buffer.BlockCopy(src3, 0, tiffRgbaImage.bluecmap, 0, num4 * 2);
				if (planarConfig == PlanarConfig.CONTIG && tiffRgbaImage.samplesperpixel != 1 && tiffRgbaImage.bitspersample < 8)
				{
					errorMsg = string.Format(CultureInfo.InvariantCulture, "Sorry, can not handle contiguous data with {0}={1}, and {2}={3} and Bits/Sample={4}", new object[] { "PhotometricInterpretation", tiffRgbaImage.photometric, "Samples/pixel", tiffRgbaImage.samplesperpixel, tiffRgbaImage.bitspersample });
					return null;
				}
				goto IL_69F;
			}
			case Photometric.MASK:
			case (Photometric)7:
				break;
			case Photometric.SEPARATED:
			{
				array = tif.GetFieldDefaulted(TiffTag.INKSET);
				InkSet inkSet = (InkSet)array[0].ToByte();
				if (inkSet != InkSet.CMYK)
				{
					errorMsg = string.Format(CultureInfo.InvariantCulture, "Sorry, can not handle separated image with {0}={1}", new object[] { "InkSet", inkSet });
					return null;
				}
				if (tiffRgbaImage.samplesperpixel < 4)
				{
					errorMsg = string.Format(CultureInfo.InvariantCulture, "Sorry, can not handle separated image with {0}={1}", new object[] { "Samples/pixel", tiffRgbaImage.samplesperpixel });
					return null;
				}
				goto IL_69F;
			}
			case Photometric.YCBCR:
			{
				if (planarConfig != PlanarConfig.CONTIG)
				{
					goto IL_69F;
				}
				Compression compression2 = compression;
				if (compression2 == Compression.JPEG)
				{
					tif.SetField(TiffTag.JPEGCOLORMODE, new object[] { JpegColorMode.RGB });
					tiffRgbaImage.photometric = Photometric.RGB;
					goto IL_69F;
				}
				goto IL_69F;
			}
			case Photometric.CIELAB:
				goto IL_69F;
			default:
				switch (photometric)
				{
				case Photometric.LOGL:
					if (compression != Compression.SGILOG)
					{
						errorMsg = string.Format(CultureInfo.InvariantCulture, "Sorry, LogL data must have {0}={1}", new object[]
						{
							"Compression",
							Compression.SGILOG
						});
						return null;
					}
					tif.SetField(TiffTag.SGILOGDATAFMT, new object[] { 3 });
					tiffRgbaImage.photometric = Photometric.MINISBLACK;
					tiffRgbaImage.bitspersample = 8;
					goto IL_69F;
				case Photometric.LOGLUV:
					if (compression != Compression.SGILOG && compression != Compression.SGILOG24)
					{
						errorMsg = string.Format(CultureInfo.InvariantCulture, "Sorry, LogLuv data must have {0}={1} or {2}", new object[]
						{
							"Compression",
							Compression.SGILOG,
							Compression.SGILOG24
						});
						return null;
					}
					if (planarConfig != PlanarConfig.CONTIG)
					{
						errorMsg = string.Format(CultureInfo.InvariantCulture, "Sorry, can not handle LogLuv images with {0}={1}", new object[] { "Planarconfiguration", planarConfig });
						return null;
					}
					tif.SetField(TiffTag.SGILOGDATAFMT, new object[] { 3 });
					tiffRgbaImage.photometric = Photometric.RGB;
					tiffRgbaImage.bitspersample = 8;
					goto IL_69F;
				}
				break;
			}
			errorMsg = string.Format(CultureInfo.InvariantCulture, "Sorry, can not handle image with {0}={1}", new object[] { "PhotometricInterpretation", tiffRgbaImage.photometric });
			return null;
			IL_69F:
			tiffRgbaImage.Map = null;
			tiffRgbaImage.BWmap = null;
			tiffRgbaImage.PALmap = null;
			tiffRgbaImage.ycbcr = null;
			tiffRgbaImage.cielab = null;
			array = tif.GetField(TiffTag.IMAGEWIDTH);
			tiffRgbaImage.width = array[0].ToInt();
			array = tif.GetField(TiffTag.IMAGELENGTH);
			tiffRgbaImage.height = array[0].ToInt();
			array = tif.GetFieldDefaulted(TiffTag.ORIENTATION);
			tiffRgbaImage.orientation = (Orientation)array[0].ToByte();
			tiffRgbaImage.isContig = planarConfig != PlanarConfig.SEPARATE || num3 <= 1;
			if (tiffRgbaImage.isContig)
			{
				if (!tiffRgbaImage.pickContigCase())
				{
					errorMsg = "Sorry, can not handle image";
					return null;
				}
			}
			else if (!tiffRgbaImage.pickSeparateCase())
			{
				errorMsg = "Sorry, can not handle image";
				return null;
			}
			return tiffRgbaImage;
		}

		public bool IsContig
		{
			get
			{
				return this.isContig;
			}
		}

		public ExtraSample Alpha
		{
			get
			{
				return this.alpha;
			}
		}

		public int Width
		{
			get
			{
				return this.width;
			}
		}

		public int Height
		{
			get
			{
				return this.height;
			}
		}

		public short BitsPerSample
		{
			get
			{
				return this.bitspersample;
			}
		}

		public short SamplesPerPixel
		{
			get
			{
				return this.samplesperpixel;
			}
		}

		public Orientation Orientation
		{
			get
			{
				return this.orientation;
			}
		}

		public Orientation ReqOrientation
		{
			get
			{
				return this.req_orientation;
			}
			set
			{
				this.req_orientation = value;
			}
		}

		public Photometric Photometric
		{
			get
			{
				return this.photometric;
			}
		}

		public TiffRgbaImage.GetDelegate Get
		{
			get
			{
				return this.get;
			}
			set
			{
				this.get = value;
			}
		}

		public TiffRgbaImage.PutContigDelegate PutContig
		{
			get
			{
				return this.putContig;
			}
			set
			{
				this.putContig = value;
			}
		}

		public TiffRgbaImage.PutSeparateDelegate PutSeparate
		{
			get
			{
				return this.putSeparate;
			}
			set
			{
				this.putSeparate = value;
			}
		}

		public bool GetRaster(int[] raster, int offset, int width, int height)
		{
			if (this.get == null)
			{
				Tiff.ErrorExt(this.tif, this.tif.m_clientdata, this.tif.FileName(), "No \"get\" method setup", new object[0]);
				return false;
			}
			return this.get(this, raster, offset, width, height);
		}

		static int PACK(int r, int g, int b)
		{
			return r | (g << 8) | (b << 16) | -16777216;
		}

		static int PACK4(int r, int g, int b, int a)
		{
			return r | (g << 8) | (b << 16) | (a << 24);
		}

		static int W2B(short v)
		{
			return (v >> 8) & 255;
		}

		static int PACKW(short r, short g, short b)
		{
			return TiffRgbaImage.W2B(r) | (TiffRgbaImage.W2B(g) << 8) | (TiffRgbaImage.W2B(b) << 16) | -16777216;
		}

		static int PACKW4(short r, short g, short b, short a)
		{
			return TiffRgbaImage.W2B(r) | (TiffRgbaImage.W2B(g) << 8) | (TiffRgbaImage.W2B(b) << 16) | (TiffRgbaImage.W2B(a) << 24);
		}

		void CMAP(int x, int i, ref int j)
		{
			this.PALmap[i][j++] = TiffRgbaImage.PACK((int)(this.redcmap[x] & 255), (int)(this.greencmap[x] & 255), (int)(this.bluecmap[x] & 255));
		}

		void GREY(int x, int i, ref int j)
		{
			int num = (int)this.Map[x];
			this.BWmap[i][j++] = TiffRgbaImage.PACK(num, num, num);
		}

		static bool gtTileContig(TiffRgbaImage img, int[] raster, int offset, int width, int height)
		{
			byte[] buffer = new byte[img.tif.TileSize()];
			FieldValue[] field = img.tif.GetField(TiffTag.TILEWIDTH);
			int num = field[0].ToInt();
			field = img.tif.GetField(TiffTag.TILELENGTH);
			int num2 = field[0].ToInt();
			int num3 = img.setorientation();
			int num4;
			int num5;
			if ((num3 & 1) != 0)
			{
				num4 = height - 1;
				num5 = -(num + width);
			}
			else
			{
				num4 = 0;
				num5 = -(num - width);
			}
			bool result = true;
			int num7;
			for (int i = 0; i < height; i += num7)
			{
				int num6 = num2 - (i + img.row_offset) % num2;
				num7 = ((i + num6 > height) ? (height - i) : num6);
				for (int j = 0; j < width; j += num)
				{
					if (img.tif.ReadTile(buffer, 0, j + img.col_offset, i + img.row_offset, 0, 0) < 0 && img.stoponerr)
					{
						result = false;
						break;
					}
					int offset2 = (i + img.row_offset) % num2 * img.tif.TileRowSize();
					if (j + num > width)
					{
						int num8 = width - j;
						int num9 = num - num8;
						img.putContig(img, raster, offset + num4 * width + j, num5 + num9, j, num4, num8, num7, buffer, offset2, num9);
					}
					else
					{
						img.putContig(img, raster, offset + num4 * width + j, num5, j, num4, num, num7, buffer, offset2, 0);
					}
				}
				num4 += (((num3 & 1) != 0) ? (-num7) : num7);
			}
			if ((num3 & 2) != 0)
			{
				for (int k = 0; k < height; k++)
				{
					int l = offset + k * width;
					int num10 = l + width - 1;
					while (l < num10)
					{
						int num11 = raster[l];
						raster[l] = raster[num10];
						raster[num10] = num11;
						l++;
						num10--;
					}
				}
			}
			return result;
		}

		static bool gtTileSeparate(TiffRgbaImage img, int[] raster, int offset, int width, int height)
		{
			int num = img.tif.TileSize();
			byte[] buffer = new byte[((img.alpha != ExtraSample.UNSPECIFIED) ? 4 : 3) * num];
			int num2 = 0;
			int num3 = num2 + num;
			int num4 = num3 + num;
			int num5 = ((img.alpha != ExtraSample.UNSPECIFIED) ? (num4 + num) : (-1));
			FieldValue[] field = img.tif.GetField(TiffTag.TILEWIDTH);
			int num6 = field[0].ToInt();
			field = img.tif.GetField(TiffTag.TILELENGTH);
			int num7 = field[0].ToInt();
			int num8 = img.setorientation();
			int num9;
			int num10;
			if ((num8 & 1) != 0)
			{
				num9 = height - 1;
				num10 = -(num6 + width);
			}
			else
			{
				num9 = 0;
				num10 = -(num6 - width);
			}
			bool result = true;
			int num12;
			for (int i = 0; i < height; i += num12)
			{
				int num11 = num7 - (i + img.row_offset) % num7;
				num12 = ((i + num11 > height) ? (height - i) : num11);
				for (int j = 0; j < width; j += num6)
				{
					if (img.tif.ReadTile(buffer, num2, j + img.col_offset, i + img.row_offset, 0, 0) < 0 && img.stoponerr)
					{
						result = false;
						break;
					}
					if (img.tif.ReadTile(buffer, num3, j + img.col_offset, i + img.row_offset, 0, 1) < 0 && img.stoponerr)
					{
						result = false;
						break;
					}
					if (img.tif.ReadTile(buffer, num4, j + img.col_offset, i + img.row_offset, 0, 2) < 0 && img.stoponerr)
					{
						result = false;
						break;
					}
					if (img.alpha != ExtraSample.UNSPECIFIED && img.tif.ReadTile(buffer, num5, j + img.col_offset, i + img.row_offset, 0, 3) < 0 && img.stoponerr)
					{
						result = false;
						break;
					}
					int num13 = (i + img.row_offset) % num7 * img.tif.TileRowSize();
					if (j + num6 > width)
					{
						int num14 = width - j;
						int num15 = num6 - num14;
						img.putSeparate(img, raster, offset + num9 * width + j, num10 + num15, j, num9, num14, num12, buffer, num2 + num13, num3 + num13, num4 + num13, (img.alpha != ExtraSample.UNSPECIFIED) ? (num5 + num13) : (-1), num15);
					}
					else
					{
						img.putSeparate(img, raster, offset + num9 * width + j, num10, j, num9, num6, num12, buffer, num2 + num13, num3 + num13, num4 + num13, (img.alpha != ExtraSample.UNSPECIFIED) ? (num5 + num13) : (-1), 0);
					}
				}
				num9 += (((num8 & 1) != 0) ? (-num12) : num12);
			}
			if ((num8 & 2) != 0)
			{
				for (int k = 0; k < height; k++)
				{
					int l = offset + k * width;
					int num16 = l + width - 1;
					while (l < num16)
					{
						int num17 = raster[l];
						raster[l] = raster[num16];
						raster[num16] = num17;
						l++;
						num16--;
					}
				}
			}
			return result;
		}

		static bool gtStripContig(TiffRgbaImage img, int[] raster, int offset, int width, int height)
		{
			byte[] buffer = new byte[img.tif.StripSize()];
			int num = img.setorientation();
			int num2;
			int rasterShift;
			if ((num & 1) != 0)
			{
				num2 = height - 1;
				rasterShift = -(width + width);
			}
			else
			{
				num2 = 0;
				rasterShift = -(width - width);
			}
			FieldValue[] fieldDefaulted = img.tif.GetFieldDefaulted(TiffTag.ROWSPERSTRIP);
			int num3 = fieldDefaulted[0].ToInt();
			if (num3 == -1)
			{
				num3 = int.MaxValue;
			}
			fieldDefaulted = img.tif.GetFieldDefaulted(TiffTag.YCBCRSUBSAMPLING);
			short num4 = fieldDefaulted[1].ToShort();
			int num5 = img.tif.newScanlineSize();
			int bufferShift = ((width < img.width) ? (img.width - width) : 0);
			bool result = true;
			int num7;
			for (int i = 0; i < height; i += num7)
			{
				int num6 = num3 - (i + img.row_offset) % num3;
				num7 = ((i + num6 > height) ? (height - i) : num6);
				int num8 = num7;
				if (num8 % (int)num4 != 0)
				{
					num8 += (int)num4 - num8 % (int)num4;
				}
				if (img.tif.ReadEncodedStrip(img.tif.ComputeStrip(i + img.row_offset, 0), buffer, 0, ((i + img.row_offset) % num3 + num8) * num5) < 0 && img.stoponerr)
				{
					result = false;
					break;
				}
				int offset2 = (i + img.row_offset) % num3 * num5;
				img.putContig(img, raster, offset + num2 * width, rasterShift, 0, num2, width, num7, buffer, offset2, bufferShift);
				num2 += (((num & 1) != 0) ? (-num7) : num7);
			}
			if ((num & 2) != 0)
			{
				for (int j = 0; j < height; j++)
				{
					int k = offset + j * width;
					int num9 = k + width - 1;
					while (k < num9)
					{
						int num10 = raster[k];
						raster[k] = raster[num9];
						raster[num9] = num10;
						k++;
						num9--;
					}
				}
			}
			return result;
		}

		static bool gtStripSeparate(TiffRgbaImage img, int[] raster, int offset, int width, int height)
		{
			int num = img.tif.StripSize();
			byte[] buffer = new byte[((img.alpha != ExtraSample.UNSPECIFIED) ? 4 : 3) * num];
			int num2 = 0;
			int num3 = num2 + num;
			int num4 = num3 + num;
			int num5 = num4 + num;
			num5 = ((img.alpha != ExtraSample.UNSPECIFIED) ? (num4 + num) : (-1));
			int num6 = img.setorientation();
			int num7;
			int rasterShift;
			if ((num6 & 1) != 0)
			{
				num7 = height - 1;
				rasterShift = -(width + width);
			}
			else
			{
				num7 = 0;
				rasterShift = -(width - width);
			}
			FieldValue[] fieldDefaulted = img.tif.GetFieldDefaulted(TiffTag.ROWSPERSTRIP);
			int num8 = fieldDefaulted[0].ToInt();
			int num9 = img.tif.ScanlineSize();
			int bufferShift = ((width < img.width) ? (img.width - width) : 0);
			bool result = true;
			int num11;
			for (int i = 0; i < height; i += num11)
			{
				int num10 = num8 - (i + img.row_offset) % num8;
				num11 = ((i + num10 > height) ? (height - i) : num10);
				int row = i + img.row_offset;
				if (img.tif.ReadEncodedStrip(img.tif.ComputeStrip(row, 0), buffer, num2, ((i + img.row_offset) % num8 + num11) * num9) < 0 && img.stoponerr)
				{
					result = false;
					break;
				}
				if (img.tif.ReadEncodedStrip(img.tif.ComputeStrip(row, 1), buffer, num3, ((i + img.row_offset) % num8 + num11) * num9) < 0 && img.stoponerr)
				{
					result = false;
					break;
				}
				if (img.tif.ReadEncodedStrip(img.tif.ComputeStrip(row, 2), buffer, num4, ((i + img.row_offset) % num8 + num11) * num9) < 0 && img.stoponerr)
				{
					result = false;
					break;
				}
				if (img.alpha != ExtraSample.UNSPECIFIED && img.tif.ReadEncodedStrip(img.tif.ComputeStrip(row, 3), buffer, num5, ((i + img.row_offset) % num8 + num11) * num9) < 0 && img.stoponerr)
				{
					result = false;
					break;
				}
				int num12 = (i + img.row_offset) % num8 * num9;
				img.putSeparate(img, raster, offset + num7 * width, rasterShift, 0, num7, width, num11, buffer, num2 + num12, num3 + num12, num4 + num12, (img.alpha != ExtraSample.UNSPECIFIED) ? (num5 + num12) : (-1), bufferShift);
				num7 += (((num6 & 1) != 0) ? (-num11) : num11);
			}
			if ((num6 & 2) != 0)
			{
				for (int j = 0; j < height; j++)
				{
					int k = offset + j * width;
					int num13 = k + width - 1;
					while (k < num13)
					{
						int num14 = raster[k];
						raster[k] = raster[num13];
						raster[num13] = num14;
						k++;
						num13--;
					}
				}
			}
			return result;
		}

		bool isCCITTCompression()
		{
			FieldValue[] field = this.tif.GetField(TiffTag.COMPRESSION);
			Compression compression = (Compression)field[0].ToInt();
			return compression == Compression.CCITTFAX3 || compression == Compression.CCITTFAX4 || compression == Compression.CCITTRLE || compression == Compression.CCITTRLEW;
		}

		int setorientation()
		{
			switch (this.orientation)
			{
			case Orientation.TOPLEFT:
			case Orientation.LEFTTOP:
				if (this.req_orientation == Orientation.TOPRIGHT || this.req_orientation == Orientation.RIGHTTOP)
				{
					return 2;
				}
				if (this.req_orientation == Orientation.BOTRIGHT || this.req_orientation == Orientation.RIGHTBOT)
				{
					return 3;
				}
				if (this.req_orientation == Orientation.BOTLEFT || this.req_orientation == Orientation.LEFTBOT)
				{
					return 1;
				}
				return 0;
			case Orientation.TOPRIGHT:
			case Orientation.RIGHTTOP:
				if (this.req_orientation == Orientation.TOPLEFT || this.req_orientation == Orientation.LEFTTOP)
				{
					return 2;
				}
				if (this.req_orientation == Orientation.BOTRIGHT || this.req_orientation == Orientation.RIGHTBOT)
				{
					return 1;
				}
				if (this.req_orientation == Orientation.BOTLEFT || this.req_orientation == Orientation.LEFTBOT)
				{
					return 3;
				}
				return 0;
			case Orientation.BOTRIGHT:
			case Orientation.RIGHTBOT:
				if (this.req_orientation == Orientation.TOPLEFT || this.req_orientation == Orientation.LEFTTOP)
				{
					return 3;
				}
				if (this.req_orientation == Orientation.TOPRIGHT || this.req_orientation == Orientation.RIGHTTOP)
				{
					return 1;
				}
				if (this.req_orientation == Orientation.BOTLEFT || this.req_orientation == Orientation.LEFTBOT)
				{
					return 2;
				}
				return 0;
			case Orientation.BOTLEFT:
			case Orientation.LEFTBOT:
				if (this.req_orientation == Orientation.TOPLEFT || this.req_orientation == Orientation.LEFTTOP)
				{
					return 1;
				}
				if (this.req_orientation == Orientation.TOPRIGHT || this.req_orientation == Orientation.RIGHTTOP)
				{
					return 3;
				}
				if (this.req_orientation == Orientation.BOTRIGHT || this.req_orientation == Orientation.RIGHTBOT)
				{
					return 2;
				}
				return 0;
			default:
				return 0;
			}
		}

		bool pickContigCase()
		{
			this.get = (this.tif.IsTiled() ? new TiffRgbaImage.GetDelegate(TiffRgbaImage.gtTileContig) : new TiffRgbaImage.GetDelegate(TiffRgbaImage.gtStripContig));
			this.putContig = null;
			switch (this.photometric)
			{
			case Photometric.MINISWHITE:
			case Photometric.MINISBLACK:
				if (this.buildMap())
				{
					short num = this.bitspersample;
					switch (num)
					{
					case 1:
						this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.put1bitbwtile);
						break;
					case 2:
						this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.put2bitbwtile);
						break;
					case 3:
						break;
					case 4:
						this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.put4bitbwtile);
						break;
					default:
						if (num != 8)
						{
							if (num == 16)
							{
								this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.put16bitbwtile);
							}
						}
						else
						{
							this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.putgreytile);
						}
						break;
					}
				}
				break;
			case Photometric.RGB:
			{
				short num2 = this.bitspersample;
				if (num2 != 8)
				{
					if (num2 == 16)
					{
						if (this.alpha == ExtraSample.ASSOCALPHA)
						{
							this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.putRGBAAcontig16bittile);
						}
						else if (this.alpha == ExtraSample.UNASSALPHA)
						{
							this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.putRGBUAcontig16bittile);
						}
						else
						{
							this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.putRGBcontig16bittile);
						}
					}
				}
				else if (this.alpha == ExtraSample.ASSOCALPHA)
				{
					this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.putRGBAAcontig8bittile);
				}
				else if (this.alpha == ExtraSample.UNASSALPHA)
				{
					this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.putRGBUAcontig8bittile);
				}
				else
				{
					this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.putRGBcontig8bittile);
				}
				break;
			}
			case Photometric.PALETTE:
				if (this.buildMap())
				{
					short num3 = this.bitspersample;
					switch (num3)
					{
					case 1:
						this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.put1bitcmaptile);
						break;
					case 2:
						this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.put2bitcmaptile);
						break;
					case 3:
						break;
					case 4:
						this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.put4bitcmaptile);
						break;
					default:
						if (num3 == 8)
						{
							this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.put8bitcmaptile);
						}
						break;
					}
				}
				break;
			case Photometric.SEPARATED:
				if (this.buildMap() && this.bitspersample == 8)
				{
					if (this.Map == null)
					{
						this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.putRGBcontig8bitCMYKtile);
					}
					else
					{
						this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.putRGBcontig8bitCMYKMaptile);
					}
				}
				break;
			case Photometric.YCBCR:
				if (this.bitspersample == 8 && this.initYCbCrConversion())
				{
					FieldValue[] fieldDefaulted = this.tif.GetFieldDefaulted(TiffTag.YCBCRSUBSAMPLING);
					short num4 = fieldDefaulted[0].ToShort();
					short num5 = fieldDefaulted[1].ToShort();
					int num6 = ((int)((ushort)num4) << 4) | (int)((ushort)num5);
					switch (num6)
					{
					case 17:
						this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.putcontig8bitYCbCr11tile);
						break;
					case 18:
						this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.putcontig8bitYCbCr12tile);
						break;
					default:
						switch (num6)
						{
						case 33:
							this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.putcontig8bitYCbCr21tile);
							break;
						case 34:
							this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.putcontig8bitYCbCr22tile);
							break;
						default:
							switch (num6)
							{
							case 65:
								this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.putcontig8bitYCbCr41tile);
								break;
							case 66:
								this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.putcontig8bitYCbCr42tile);
								break;
							case 68:
								this.putContig = new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.putcontig8bitYCbCr44tile);
								break;
							}
							break;
						}
						break;
					}
				}
				break;
			case Photometric.CIELAB:
				if (this.buildMap() && this.bitspersample == 8)
				{
					this.putContig = this.initCIELabConversion();
				}
				break;
			}
			return this.putContig != null;
		}

		bool pickSeparateCase()
		{
			this.get = (this.tif.IsTiled() ? new TiffRgbaImage.GetDelegate(TiffRgbaImage.gtTileSeparate) : new TiffRgbaImage.GetDelegate(TiffRgbaImage.gtStripSeparate));
			this.putSeparate = null;
			Photometric photometric = this.photometric;
			if (photometric != Photometric.RGB)
			{
				if (photometric == Photometric.YCBCR)
				{
					if (this.bitspersample == 8 && this.samplesperpixel == 3 && this.initYCbCrConversion())
					{
						FieldValue[] fieldDefaulted = this.tif.GetFieldDefaulted(TiffTag.YCBCRSUBSAMPLING);
						short num = fieldDefaulted[0].ToShort();
						short num2 = fieldDefaulted[0].ToShort();
						int num3 = ((int)((ushort)num) << 4) | (int)((ushort)num2);
						if (num3 == 17)
						{
							this.putSeparate = new TiffRgbaImage.PutSeparateDelegate(TiffRgbaImage.putseparate8bitYCbCr11tile);
						}
					}
				}
			}
			else
			{
				short num4 = this.bitspersample;
				if (num4 != 8)
				{
					if (num4 == 16)
					{
						if (this.alpha == ExtraSample.ASSOCALPHA)
						{
							this.putSeparate = new TiffRgbaImage.PutSeparateDelegate(TiffRgbaImage.putRGBAAseparate16bittile);
						}
						else if (this.alpha == ExtraSample.UNASSALPHA)
						{
							this.putSeparate = new TiffRgbaImage.PutSeparateDelegate(TiffRgbaImage.putRGBUAseparate16bittile);
						}
						else
						{
							this.putSeparate = new TiffRgbaImage.PutSeparateDelegate(TiffRgbaImage.putRGBseparate16bittile);
						}
					}
				}
				else if (this.alpha == ExtraSample.ASSOCALPHA)
				{
					this.putSeparate = new TiffRgbaImage.PutSeparateDelegate(TiffRgbaImage.putRGBAAseparate8bittile);
				}
				else if (this.alpha == ExtraSample.UNASSALPHA)
				{
					this.putSeparate = new TiffRgbaImage.PutSeparateDelegate(TiffRgbaImage.putRGBUAseparate8bittile);
				}
				else
				{
					this.putSeparate = new TiffRgbaImage.PutSeparateDelegate(TiffRgbaImage.putRGBseparate8bittile);
				}
			}
			return this.putSeparate != null;
		}

		bool initYCbCrConversion()
		{
			if (this.ycbcr == null)
			{
				this.ycbcr = new TiffYCbCrToRGB();
			}
			FieldValue[] fieldDefaulted = this.tif.GetFieldDefaulted(TiffTag.YCBCRCOEFFICIENTS);
			float[] luma = fieldDefaulted[0].ToFloatArray();
			fieldDefaulted = this.tif.GetFieldDefaulted(TiffTag.REFERENCEBLACKWHITE);
			float[] refBlackWhite = fieldDefaulted[0].ToFloatArray();
			this.ycbcr.Init(luma, refBlackWhite);
			return true;
		}

		TiffRgbaImage.PutContigDelegate initCIELabConversion()
		{
			if (this.cielab == null)
			{
				this.cielab = new TiffCIELabToRGB();
			}
			FieldValue[] fieldDefaulted = this.tif.GetFieldDefaulted(TiffTag.WHITEPOINT);
			float[] array = fieldDefaulted[0].ToFloatArray();
			float[] array2 = new float[3];
			array2[1] = 100f;
			array2[0] = array[0] / array[1] * array2[1];
			array2[2] = (1f - array[0] - array[1]) / array[1] * array2[1];
			this.cielab.Init(TiffRgbaImage.display_sRGB, array2);
			return new TiffRgbaImage.PutContigDelegate(TiffRgbaImage.putcontig8bitCIELab);
		}

		bool buildMap()
		{
			switch (this.photometric)
			{
			case Photometric.MINISWHITE:
			case Photometric.MINISBLACK:
				if (!this.setupMap())
				{
					return false;
				}
				break;
			case Photometric.RGB:
			case Photometric.SEPARATED:
			case Photometric.YCBCR:
				if (this.bitspersample != 8 && !this.setupMap())
				{
					return false;
				}
				break;
			case Photometric.PALETTE:
				if (this.checkcmap() == 16)
				{
					this.cvtcmap();
				}
				else
				{
					Tiff.WarningExt(this.tif, this.tif.m_clientdata, this.tif.FileName(), "Assuming 8-bit colormap", new object[0]);
				}
				if (this.bitspersample <= 8 && !this.makecmap())
				{
					return false;
				}
				break;
			}
			return true;
		}

		bool setupMap()
		{
			int num = (1 << (int)this.bitspersample) - 1;
			if (this.bitspersample == 16)
			{
				num = 255;
			}
			this.Map = new byte[num + 1];
			if (this.photometric == Photometric.MINISWHITE)
			{
				for (int i = 0; i <= num; i++)
				{
					this.Map[i] = (byte)((num - i) * 255 / num);
				}
			}
			else
			{
				for (int j = 0; j <= num; j++)
				{
					this.Map[j] = (byte)(j * 255 / num);
				}
			}
			if (this.bitspersample <= 16 && (this.photometric == Photometric.MINISBLACK || this.photometric == Photometric.MINISWHITE))
			{
				if (!this.makebwmap())
				{
					return false;
				}
				this.Map = null;
			}
			return true;
		}

		int checkcmap()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 1 << (int)this.bitspersample;
			while (num4-- > 0)
			{
				if (this.redcmap[num] >= 256 || this.greencmap[num2] >= 256 || this.bluecmap[num3] >= 256)
				{
					return 16;
				}
				num++;
				num2++;
				num3++;
			}
			return 8;
		}

		void cvtcmap()
		{
			for (int i = (1 << (int)this.bitspersample) - 1; i >= 0; i--)
			{
				this.redcmap[i] = (short)(this.redcmap[i] >> 8);
				this.greencmap[i] = (short)(this.greencmap[i] >> 8);
				this.bluecmap[i] = (short)(this.bluecmap[i] >> 8);
			}
		}

		bool makecmap()
		{
			int num = (int)(8 / this.bitspersample);
			this.PALmap = new int[256][];
			for (int i = 0; i < 256; i++)
			{
				this.PALmap[i] = new int[num];
			}
			for (int j = 0; j < 256; j++)
			{
				int num2 = 0;
				short num3 = this.bitspersample;
				switch (num3)
				{
				case 1:
					this.CMAP(j >> 7, j, ref num2);
					this.CMAP((j >> 6) & 1, j, ref num2);
					this.CMAP((j >> 5) & 1, j, ref num2);
					this.CMAP((j >> 4) & 1, j, ref num2);
					this.CMAP((j >> 3) & 1, j, ref num2);
					this.CMAP((j >> 2) & 1, j, ref num2);
					this.CMAP((j >> 1) & 1, j, ref num2);
					this.CMAP(j & 1, j, ref num2);
					break;
				case 2:
					this.CMAP(j >> 6, j, ref num2);
					this.CMAP((j >> 4) & 3, j, ref num2);
					this.CMAP((j >> 2) & 3, j, ref num2);
					this.CMAP(j & 3, j, ref num2);
					break;
				case 3:
					break;
				case 4:
					this.CMAP(j >> 4, j, ref num2);
					this.CMAP(j & 15, j, ref num2);
					break;
				default:
					if (num3 == 8)
					{
						this.CMAP(j, j, ref num2);
					}
					break;
				}
			}
			return true;
		}

		bool makebwmap()
		{
			int num = (int)(8 / this.bitspersample);
			if (num == 0)
			{
				num = 1;
			}
			this.BWmap = new int[256][];
			for (int i = 0; i < 256; i++)
			{
				this.BWmap[i] = new int[num];
			}
			for (int j = 0; j < 256; j++)
			{
				int num2 = 0;
				short num3 = this.bitspersample;
				switch (num3)
				{
				case 1:
					this.GREY(j >> 7, j, ref num2);
					this.GREY((j >> 6) & 1, j, ref num2);
					this.GREY((j >> 5) & 1, j, ref num2);
					this.GREY((j >> 4) & 1, j, ref num2);
					this.GREY((j >> 3) & 1, j, ref num2);
					this.GREY((j >> 2) & 1, j, ref num2);
					this.GREY((j >> 1) & 1, j, ref num2);
					this.GREY(j & 1, j, ref num2);
					break;
				case 2:
					this.GREY(j >> 6, j, ref num2);
					this.GREY((j >> 4) & 3, j, ref num2);
					this.GREY((j >> 2) & 3, j, ref num2);
					this.GREY(j & 3, j, ref num2);
					break;
				case 3:
					break;
				case 4:
					this.GREY(j >> 4, j, ref num2);
					this.GREY(j & 15, j, ref num2);
					break;
				default:
					if (num3 == 8 || num3 == 16)
					{
						this.GREY(j, j, ref num2);
					}
					break;
				}
			}
			return true;
		}

		void YCbCrtoRGB(out int dst, int Y, int Cb, int Cr)
		{
			int r;
			int g;
			int b;
			this.ycbcr.YCbCrtoRGB(Y, Cb, Cr, out r, out g, out b);
			dst = TiffRgbaImage.PACK(r, g, b);
		}

		static void put8bitcmaptile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			int[][] palmap = img.PALmap;
			int num = (int)img.samplesperpixel;
			while (height-- > 0)
			{
				x = width;
				while (x-- > 0)
				{
					raster[rasterOffset] = palmap[(int)buffer[offset]][0];
					rasterOffset++;
					offset += num;
				}
				rasterOffset += rasterShift;
				offset += bufferShift;
			}
		}

		static void put4bitcmaptile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			int[][] palmap = img.PALmap;
			bufferShift /= 2;
			while (height-- > 0)
			{
				int i;
				for (i = width; i >= 2; i -= 2)
				{
					int[] array = palmap[(int)buffer[offset]];
					offset++;
					for (int j = 0; j < 2; j++)
					{
						raster[rasterOffset] = array[j];
						rasterOffset++;
					}
				}
				if (i != 0)
				{
					int[] array = palmap[(int)buffer[offset]];
					offset++;
					raster[rasterOffset] = array[0];
					rasterOffset++;
				}
				rasterOffset += rasterShift;
				offset += bufferShift;
			}
		}

		static void put2bitcmaptile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			int[][] palmap = img.PALmap;
			bufferShift /= 4;
			while (height-- > 0)
			{
				int i;
				for (i = width; i >= 4; i -= 4)
				{
					int[] array = palmap[(int)buffer[offset]];
					offset++;
					for (int j = 0; j < 4; j++)
					{
						raster[rasterOffset] = array[j];
						rasterOffset++;
					}
				}
				if (i > 0)
				{
					int[] array = palmap[(int)buffer[offset]];
					offset++;
					if (i <= 3 && i > 0)
					{
						for (int k = 0; k < i; k++)
						{
							raster[rasterOffset] = array[k];
							rasterOffset++;
						}
					}
				}
				rasterOffset += rasterShift;
				offset += bufferShift;
			}
		}

		static void put1bitcmaptile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			int[][] palmap = img.PALmap;
			bufferShift /= 8;
			while (height-- > 0)
			{
				int i;
				for (i = width; i >= 8; i -= 8)
				{
					int[] array = palmap[(int)buffer[offset++]];
					int num = 0;
					for (int j = 0; j < 8; j++)
					{
						raster[rasterOffset++] = array[num++];
					}
				}
				if (i > 0)
				{
					int[] array = palmap[(int)buffer[offset++]];
					int num = 0;
					if (i <= 7 && i > 0)
					{
						for (int k = 0; k < i; k++)
						{
							raster[rasterOffset++] = array[num++];
						}
					}
				}
				rasterOffset += rasterShift;
				offset += bufferShift;
			}
		}

		static void putgreytile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			int num = (int)img.samplesperpixel;
			int[][] bwmap = img.BWmap;
			while (height-- > 0)
			{
				x = width;
				while (x-- > 0)
				{
					raster[rasterOffset] = bwmap[(int)buffer[offset]][0];
					rasterOffset++;
					offset += num;
				}
				rasterOffset += rasterShift;
				offset += bufferShift;
			}
		}

		static void put16bitbwtile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			int num = (int)img.samplesperpixel;
			int[][] bwmap = img.BWmap;
			while (height-- > 0)
			{
				short[] array = Tiff.ByteArrayToShorts(buffer, offset, buffer.Length - offset);
				int num2 = 0;
				x = width;
				while (x-- > 0)
				{
					raster[rasterOffset] = bwmap[((int)array[num2] & 65535) >> 8][0];
					rasterOffset++;
					offset += 2 * num;
					num2 += num;
				}
				rasterOffset += rasterShift;
				offset += bufferShift;
			}
		}

		static void put1bitbwtile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			int[][] bwmap = img.BWmap;
			bufferShift /= 8;
			while (height-- > 0)
			{
				int i;
				for (i = width; i >= 8; i -= 8)
				{
					int[] array = bwmap[(int)buffer[offset]];
					offset++;
					for (int j = 0; j < 8; j++)
					{
						raster[rasterOffset] = array[j];
						rasterOffset++;
					}
				}
				if (i > 0)
				{
					int[] array = bwmap[(int)buffer[offset]];
					offset++;
					if (i <= 7 && i > 0)
					{
						for (int k = 0; k < i; k++)
						{
							raster[rasterOffset] = array[k];
							rasterOffset++;
						}
					}
				}
				rasterOffset += rasterShift;
				offset += bufferShift;
			}
		}

		static void put2bitbwtile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			int[][] bwmap = img.BWmap;
			bufferShift /= 4;
			while (height-- > 0)
			{
				int i;
				for (i = width; i >= 4; i -= 4)
				{
					int[] array = bwmap[(int)buffer[offset]];
					offset++;
					for (int j = 0; j < 4; j++)
					{
						raster[rasterOffset] = array[j];
						rasterOffset++;
					}
				}
				if (i > 0)
				{
					int[] array = bwmap[(int)buffer[offset]];
					offset++;
					if (i <= 3 && i > 0)
					{
						for (int k = 0; k < i; k++)
						{
							raster[rasterOffset] = array[k];
							rasterOffset++;
						}
					}
				}
				rasterOffset += rasterShift;
				offset += bufferShift;
			}
		}

		static void put4bitbwtile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			int[][] bwmap = img.BWmap;
			bufferShift /= 2;
			while (height-- > 0)
			{
				int i;
				for (i = width; i >= 2; i -= 2)
				{
					int[] array = bwmap[(int)buffer[offset]];
					offset++;
					for (int j = 0; j < 2; j++)
					{
						raster[rasterOffset] = array[j];
						rasterOffset++;
					}
				}
				if (i != 0)
				{
					int[] array = bwmap[(int)buffer[offset]];
					offset++;
					raster[rasterOffset] = array[0];
					rasterOffset++;
				}
				rasterOffset += rasterShift;
				offset += bufferShift;
			}
		}

		static void putRGBcontig8bittile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			int num = (int)img.samplesperpixel;
			bufferShift *= num;
			while (height-- > 0)
			{
				int i;
				for (i = width; i >= 8; i -= 8)
				{
					for (int j = 0; j < 8; j++)
					{
						raster[rasterOffset] = TiffRgbaImage.PACK((int)buffer[offset], (int)buffer[offset + 1], (int)buffer[offset + 2]);
						rasterOffset++;
						offset += num;
					}
				}
				if (i > 0 && i <= 7 && i > 0)
				{
					for (int k = i; k > 0; k--)
					{
						raster[rasterOffset] = TiffRgbaImage.PACK((int)buffer[offset], (int)buffer[offset + 1], (int)buffer[offset + 2]);
						rasterOffset++;
						offset += num;
					}
				}
				rasterOffset += rasterShift;
				offset += bufferShift;
			}
		}

		static void putRGBAAcontig8bittile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			int num = (int)img.samplesperpixel;
			bufferShift *= num;
			while (height-- > 0)
			{
				int i;
				for (i = width; i >= 8; i -= 8)
				{
					for (int j = 0; j < 8; j++)
					{
						raster[rasterOffset] = TiffRgbaImage.PACK4((int)buffer[offset], (int)buffer[offset + 1], (int)buffer[offset + 2], (int)buffer[offset + 3]);
						rasterOffset++;
						offset += num;
					}
				}
				if (i > 0 && i <= 7 && i > 0)
				{
					for (int k = i; k > 0; k--)
					{
						raster[rasterOffset] = TiffRgbaImage.PACK4((int)buffer[offset], (int)buffer[offset + 1], (int)buffer[offset + 2], (int)buffer[offset + 3]);
						rasterOffset++;
						offset += num;
					}
				}
				rasterOffset += rasterShift;
				offset += bufferShift;
			}
		}

		static void putRGBUAcontig8bittile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			int num = (int)img.samplesperpixel;
			bufferShift *= num;
			while (height-- > 0)
			{
				x = width;
				while (x-- > 0)
				{
					int num2 = (int)buffer[offset + 3];
					int r = ((int)buffer[offset] * num2 + 127) / 255;
					int g = ((int)buffer[offset + 1] * num2 + 127) / 255;
					int b = ((int)buffer[offset + 2] * num2 + 127) / 255;
					raster[rasterOffset] = TiffRgbaImage.PACK4(r, g, b, num2);
					rasterOffset++;
					offset += num;
				}
				rasterOffset += rasterShift;
				offset += bufferShift;
			}
		}

		static void putRGBcontig16bittile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			int num = (int)img.samplesperpixel;
			bufferShift *= num;
			short[] array = Tiff.ByteArrayToShorts(buffer, offset, buffer.Length);
			int num2 = 0;
			while (height-- > 0)
			{
				x = width;
				while (x-- > 0)
				{
					raster[rasterOffset] = TiffRgbaImage.PACKW(array[num2], array[num2 + 1], array[num2 + 2]);
					rasterOffset++;
					num2 += num;
				}
				rasterOffset += rasterShift;
				num2 += bufferShift;
			}
		}

		static void putRGBAAcontig16bittile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			int num = (int)img.samplesperpixel;
			short[] array = Tiff.ByteArrayToShorts(buffer, offset, buffer.Length);
			int num2 = 0;
			bufferShift *= num;
			while (height-- > 0)
			{
				x = width;
				while (x-- > 0)
				{
					raster[rasterOffset] = TiffRgbaImage.PACKW4(array[num2], array[num2 + 1], array[num2 + 2], array[num2 + 3]);
					rasterOffset++;
					num2 += num;
				}
				rasterOffset += rasterShift;
				num2 += bufferShift;
			}
		}

		static void putRGBUAcontig16bittile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			int num = (int)img.samplesperpixel;
			bufferShift *= num;
			short[] array = Tiff.ByteArrayToShorts(buffer, offset, buffer.Length);
			int num2 = 0;
			while (height-- > 0)
			{
				x = width;
				while (x-- > 0)
				{
					int num3 = TiffRgbaImage.W2B(array[num2 + 3]);
					int r = (TiffRgbaImage.W2B(array[num2]) * num3 + 127) / 255;
					int g = (TiffRgbaImage.W2B(array[num2 + 1]) * num3 + 127) / 255;
					int b = (TiffRgbaImage.W2B(array[num2 + 2]) * num3 + 127) / 255;
					raster[rasterOffset] = TiffRgbaImage.PACK4(r, g, b, num3);
					rasterOffset++;
					num2 += num;
				}
				rasterOffset += rasterShift;
				num2 += bufferShift;
			}
		}

		static void putRGBcontig8bitCMYKtile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			int num = (int)img.samplesperpixel;
			bufferShift *= num;
			while (height-- > 0)
			{
				int i;
				for (i = width; i >= 8; i -= 8)
				{
					for (int j = 0; j < 8; j++)
					{
						short num2 = (short)(byte.MaxValue - buffer[offset + 3]);
						short r = (short)(num2 * (short)(byte.MaxValue - buffer[offset]) / 255);
						short g = (short)(num2 * (short)(byte.MaxValue - buffer[offset + 1]) / 255);
						short b = (short)(num2 * (short)(byte.MaxValue - buffer[offset + 2]) / 255);
						raster[rasterOffset] = TiffRgbaImage.PACK((int)r, (int)g, (int)b);
						rasterOffset++;
						offset += num;
					}
				}
				if (i > 0 && i <= 7 && i > 0)
				{
					for (int k = i; k > 0; k--)
					{
						short num3 = (short)(byte.MaxValue - buffer[offset + 3]);
						short r2 = (short)(num3 * (short)(byte.MaxValue - buffer[offset]) / 255);
						short g2 = (short)(num3 * (short)(byte.MaxValue - buffer[offset + 1]) / 255);
						short b2 = (short)(num3 * (short)(byte.MaxValue - buffer[offset + 2]) / 255);
						raster[rasterOffset] = TiffRgbaImage.PACK((int)r2, (int)g2, (int)b2);
						rasterOffset++;
						offset += num;
					}
				}
				rasterOffset += rasterShift;
				offset += bufferShift;
			}
		}

		static void putcontig8bitCIELab(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			bufferShift *= 3;
			while (height-- > 0)
			{
				x = width;
				while (x-- > 0)
				{
					float x2;
					float y2;
					float z;
					img.cielab.CIELabToXYZ((int)buffer[offset], (int)((sbyte)buffer[offset + 1]), (int)((sbyte)buffer[offset + 2]), out x2, out y2, out z);
					int r;
					int g;
					int b;
					img.cielab.XYZToRGB(x2, y2, z, out r, out g, out b);
					raster[rasterOffset] = TiffRgbaImage.PACK(r, g, b);
					rasterOffset++;
					offset += 3;
				}
				rasterOffset += rasterShift;
				offset += bufferShift;
			}
		}

		static void putcontig8bitYCbCr22tile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			bufferShift = bufferShift / 2 * 6;
			int num = rasterOffset + width + rasterShift;
			while (height >= 2)
			{
				for (x = width; x >= 2; x -= 2)
				{
					int cb = (int)buffer[offset + 4];
					int cr = (int)buffer[offset + 5];
					img.YCbCrtoRGB(out raster[rasterOffset], (int)buffer[offset], cb, cr);
					img.YCbCrtoRGB(out raster[rasterOffset + 1], (int)buffer[offset + 1], cb, cr);
					img.YCbCrtoRGB(out raster[num], (int)buffer[offset + 2], cb, cr);
					img.YCbCrtoRGB(out raster[num + 1], (int)buffer[offset + 3], cb, cr);
					rasterOffset += 2;
					num += 2;
					offset += 6;
				}
				if (x == 1)
				{
					int cb2 = (int)buffer[offset + 4];
					int cr2 = (int)buffer[offset + 5];
					img.YCbCrtoRGB(out raster[rasterOffset], (int)buffer[offset], cb2, cr2);
					img.YCbCrtoRGB(out raster[num], (int)buffer[offset + 2], cb2, cr2);
					rasterOffset++;
					num++;
					offset += 6;
				}
				rasterOffset += rasterShift * 2 + width;
				num += rasterShift * 2 + width;
				offset += bufferShift;
				height -= 2;
			}
			if (height == 1)
			{
				for (x = width; x >= 2; x -= 2)
				{
					int cb3 = (int)buffer[offset + 4];
					int cr3 = (int)buffer[offset + 5];
					img.YCbCrtoRGB(out raster[rasterOffset], (int)buffer[offset], cb3, cr3);
					img.YCbCrtoRGB(out raster[rasterOffset + 1], (int)buffer[offset + 1], cb3, cr3);
					rasterOffset += 2;
					num += 2;
					offset += 6;
				}
				if (x == 1)
				{
					int cb4 = (int)buffer[offset + 4];
					int cr4 = (int)buffer[offset + 5];
					img.YCbCrtoRGB(out raster[rasterOffset], (int)buffer[offset], cb4, cr4);
				}
			}
		}

		static void putcontig8bitYCbCr21tile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			bufferShift = bufferShift * 4 / 2;
			do
			{
				x = width >> 1;
				do
				{
					int cb = (int)buffer[offset + 2];
					int cr = (int)buffer[offset + 3];
					img.YCbCrtoRGB(out raster[rasterOffset], (int)buffer[offset], cb, cr);
					img.YCbCrtoRGB(out raster[rasterOffset + 1], (int)buffer[offset + 1], cb, cr);
					rasterOffset += 2;
					offset += 4;
				}
				while (--x != 0);
				if ((width & 1) != 0)
				{
					int cb2 = (int)buffer[offset + 2];
					int cr2 = (int)buffer[offset + 3];
					img.YCbCrtoRGB(out raster[rasterOffset], (int)buffer[offset], cb2, cr2);
					rasterOffset++;
					offset += 4;
				}
				rasterOffset += rasterShift;
				offset += bufferShift;
			}
			while (--height != 0);
		}

		static void putcontig8bitYCbCr44tile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			int num = rasterOffset + width + rasterShift;
			int num2 = num + width + rasterShift;
			int num3 = num2 + width + rasterShift;
			int num4 = 3 * width + 4 * rasterShift;
			bufferShift = bufferShift * 18 / 4;
			if ((height & 3) == 0 && (width & 3) == 0)
			{
				while (height >= 4)
				{
					x = width >> 2;
					do
					{
						int cb = (int)buffer[offset + 16];
						int cr = (int)buffer[offset + 17];
						img.YCbCrtoRGB(out raster[rasterOffset], (int)buffer[offset], cb, cr);
						img.YCbCrtoRGB(out raster[rasterOffset + 1], (int)buffer[offset + 1], cb, cr);
						img.YCbCrtoRGB(out raster[rasterOffset + 2], (int)buffer[offset + 2], cb, cr);
						img.YCbCrtoRGB(out raster[rasterOffset + 3], (int)buffer[offset + 3], cb, cr);
						img.YCbCrtoRGB(out raster[num], (int)buffer[offset + 4], cb, cr);
						img.YCbCrtoRGB(out raster[num + 1], (int)buffer[offset + 5], cb, cr);
						img.YCbCrtoRGB(out raster[num + 2], (int)buffer[offset + 6], cb, cr);
						img.YCbCrtoRGB(out raster[num + 3], (int)buffer[offset + 7], cb, cr);
						img.YCbCrtoRGB(out raster[num2], (int)buffer[offset + 8], cb, cr);
						img.YCbCrtoRGB(out raster[num2 + 1], (int)buffer[offset + 9], cb, cr);
						img.YCbCrtoRGB(out raster[num2 + 2], (int)buffer[offset + 10], cb, cr);
						img.YCbCrtoRGB(out raster[num2 + 3], (int)buffer[offset + 11], cb, cr);
						img.YCbCrtoRGB(out raster[num3], (int)buffer[offset + 12], cb, cr);
						img.YCbCrtoRGB(out raster[num3 + 1], (int)buffer[offset + 13], cb, cr);
						img.YCbCrtoRGB(out raster[num3 + 2], (int)buffer[offset + 14], cb, cr);
						img.YCbCrtoRGB(out raster[num3 + 3], (int)buffer[offset + 15], cb, cr);
						rasterOffset += 4;
						num += 4;
						num2 += 4;
						num3 += 4;
						offset += 18;
					}
					while (--x != 0);
					rasterOffset += num4;
					num += num4;
					num2 += num4;
					num3 += num4;
					offset += bufferShift;
					height -= 4;
				}
				return;
			}
			while (height > 0)
			{
				x = width;
				while (x > 0)
				{
					int cb2 = (int)buffer[offset + 16];
					int cr2 = (int)buffer[offset + 17];
					bool flag = false;
					if (x < 1 || x > 3)
					{
						bool flag2 = false;
						if (height < 1 || height > 3)
						{
							img.YCbCrtoRGB(out raster[num3 + 3], (int)buffer[offset + 15], cb2, cr2);
							flag2 = true;
						}
						if (height == 3 || flag2)
						{
							img.YCbCrtoRGB(out raster[num2 + 3], (int)buffer[offset + 11], cb2, cr2);
							flag2 = true;
						}
						if (height == 2 || flag2)
						{
							img.YCbCrtoRGB(out raster[num + 3], (int)buffer[offset + 7], cb2, cr2);
							flag2 = true;
						}
						if (height == 1 || flag2)
						{
							img.YCbCrtoRGB(out raster[rasterOffset + 3], (int)buffer[offset + 3], cb2, cr2);
						}
						flag = true;
					}
					if (x == 3 || flag)
					{
						bool flag2 = false;
						if (height < 1 || height > 3)
						{
							img.YCbCrtoRGB(out raster[num3 + 2], (int)buffer[offset + 14], cb2, cr2);
							flag2 = true;
						}
						if (height == 3 || flag2)
						{
							img.YCbCrtoRGB(out raster[num2 + 2], (int)buffer[offset + 10], cb2, cr2);
							flag2 = true;
						}
						if (height == 2 || flag2)
						{
							img.YCbCrtoRGB(out raster[num + 2], (int)buffer[offset + 6], cb2, cr2);
							flag2 = true;
						}
						if (height == 1 || flag2)
						{
							img.YCbCrtoRGB(out raster[rasterOffset + 2], (int)buffer[offset + 2], cb2, cr2);
						}
						flag = true;
					}
					if (x == 2 || flag)
					{
						bool flag2 = false;
						if (height < 1 || height > 3)
						{
							img.YCbCrtoRGB(out raster[num3 + 1], (int)buffer[offset + 13], cb2, cr2);
							flag2 = true;
						}
						if (height == 3 || flag2)
						{
							img.YCbCrtoRGB(out raster[num2 + 1], (int)buffer[offset + 9], cb2, cr2);
							flag2 = true;
						}
						if (height == 2 || flag2)
						{
							img.YCbCrtoRGB(out raster[num + 1], (int)buffer[offset + 5], cb2, cr2);
							flag2 = true;
						}
						if (height == 1 || flag2)
						{
							img.YCbCrtoRGB(out raster[rasterOffset + 1], (int)buffer[offset + 1], cb2, cr2);
						}
					}
					if (x == 1 || flag)
					{
						bool flag2 = false;
						if (height < 1 || height > 3)
						{
							img.YCbCrtoRGB(out raster[num3], (int)buffer[offset + 12], cb2, cr2);
							flag2 = true;
						}
						if (height == 3 || flag2)
						{
							img.YCbCrtoRGB(out raster[num2], (int)buffer[offset + 8], cb2, cr2);
							flag2 = true;
						}
						if (height == 2 || flag2)
						{
							img.YCbCrtoRGB(out raster[num], (int)buffer[offset + 4], cb2, cr2);
							flag2 = true;
						}
						if (height == 1 || flag2)
						{
							img.YCbCrtoRGB(out raster[rasterOffset], (int)buffer[offset], cb2, cr2);
						}
					}
					if (x < 4)
					{
						rasterOffset += x;
						num += x;
						num2 += x;
						num3 += x;
						x = 0;
					}
					else
					{
						rasterOffset += 4;
						num += 4;
						num2 += 4;
						num3 += 4;
						x -= 4;
					}
					offset += 18;
				}
				if (height <= 4)
				{
					return;
				}
				height -= 4;
				rasterOffset += num4;
				num += num4;
				num2 += num4;
				num3 += num4;
				offset += bufferShift;
			}
		}

		static void putcontig8bitYCbCr42tile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			int num = rasterOffset + width + rasterShift;
			int num2 = 2 * rasterShift + width;
			bufferShift = bufferShift * 10 / 4;
			if ((height & 3) == 0 && (width & 1) == 0)
			{
				while (height >= 2)
				{
					x = width >> 2;
					do
					{
						int cb = (int)buffer[offset + 8];
						int cr = (int)buffer[offset + 9];
						img.YCbCrtoRGB(out raster[rasterOffset], (int)buffer[offset], cb, cr);
						img.YCbCrtoRGB(out raster[rasterOffset + 1], (int)buffer[offset + 1], cb, cr);
						img.YCbCrtoRGB(out raster[rasterOffset + 2], (int)buffer[offset + 2], cb, cr);
						img.YCbCrtoRGB(out raster[rasterOffset + 3], (int)buffer[offset + 3], cb, cr);
						img.YCbCrtoRGB(out raster[num], (int)buffer[offset + 4], cb, cr);
						img.YCbCrtoRGB(out raster[num + 1], (int)buffer[offset + 5], cb, cr);
						img.YCbCrtoRGB(out raster[num + 2], (int)buffer[offset + 6], cb, cr);
						img.YCbCrtoRGB(out raster[num + 3], (int)buffer[offset + 7], cb, cr);
						rasterOffset += 4;
						num += 4;
						offset += 10;
					}
					while (--x != 0);
					rasterOffset += num2;
					num += num2;
					offset += bufferShift;
					height -= 2;
				}
				return;
			}
			while (height > 0)
			{
				x = width;
				while (x > 0)
				{
					int cb2 = (int)buffer[offset + 8];
					int cr2 = (int)buffer[offset + 9];
					bool flag = false;
					if (x < 1 || x > 3)
					{
						if (height != 1)
						{
							img.YCbCrtoRGB(out raster[num + 3], (int)buffer[offset + 7], cb2, cr2);
						}
						img.YCbCrtoRGB(out raster[rasterOffset + 3], (int)buffer[offset + 3], cb2, cr2);
						flag = true;
					}
					if (x == 3 || flag)
					{
						if (height != 1)
						{
							img.YCbCrtoRGB(out raster[num + 2], (int)buffer[offset + 6], cb2, cr2);
						}
						img.YCbCrtoRGB(out raster[rasterOffset + 2], (int)buffer[offset + 2], cb2, cr2);
						flag = true;
					}
					if (x == 2 || flag)
					{
						if (height != 1)
						{
							img.YCbCrtoRGB(out raster[num + 1], (int)buffer[offset + 5], cb2, cr2);
						}
						img.YCbCrtoRGB(out raster[rasterOffset + 1], (int)buffer[offset + 1], cb2, cr2);
						flag = true;
					}
					if (x == 1 || flag)
					{
						if (height != 1)
						{
							img.YCbCrtoRGB(out raster[num], (int)buffer[offset + 4], cb2, cr2);
						}
						img.YCbCrtoRGB(out raster[rasterOffset], (int)buffer[offset], cb2, cr2);
					}
					if (x < 4)
					{
						rasterOffset += x;
						num += x;
						x = 0;
					}
					else
					{
						rasterOffset += 4;
						num += 4;
						x -= 4;
					}
					offset += 10;
				}
				if (height <= 2)
				{
					return;
				}
				height -= 2;
				rasterOffset += num2;
				num += num2;
				offset += bufferShift;
			}
		}

		static void putcontig8bitYCbCr41tile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			do
			{
				x = width >> 2;
				do
				{
					int cb = (int)buffer[offset + 4];
					int cr = (int)buffer[offset + 5];
					img.YCbCrtoRGB(out raster[rasterOffset], (int)buffer[offset], cb, cr);
					img.YCbCrtoRGB(out raster[rasterOffset + 1], (int)buffer[offset + 1], cb, cr);
					img.YCbCrtoRGB(out raster[rasterOffset + 2], (int)buffer[offset + 2], cb, cr);
					img.YCbCrtoRGB(out raster[rasterOffset + 3], (int)buffer[offset + 3], cb, cr);
					rasterOffset += 4;
					offset += 6;
				}
				while (--x != 0);
				if ((width & 3) != 0)
				{
					int cb2 = (int)buffer[offset + 4];
					int cr2 = (int)buffer[offset + 5];
					int num = width & 3;
					if (num == 3)
					{
						img.YCbCrtoRGB(out raster[rasterOffset + 2], (int)buffer[offset + 2], cb2, cr2);
					}
					if (num == 3 || num == 2)
					{
						img.YCbCrtoRGB(out raster[rasterOffset + 1], (int)buffer[offset + 1], cb2, cr2);
					}
					if (num == 3 || num == 2 || num == 1)
					{
						img.YCbCrtoRGB(out raster[rasterOffset], (int)buffer[offset], cb2, cr2);
					}
					rasterOffset += num;
					offset += 6;
				}
				rasterOffset += rasterShift;
				offset += bufferShift;
			}
			while (--height != 0);
		}

		static void putcontig8bitYCbCr11tile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			bufferShift *= 3;
			do
			{
				x = width;
				do
				{
					int cb = (int)buffer[offset + 1];
					int cr = (int)buffer[offset + 2];
					img.YCbCrtoRGB(out raster[rasterOffset], (int)buffer[offset], cb, cr);
					rasterOffset++;
					offset += 3;
				}
				while (--x != 0);
				rasterOffset += rasterShift;
				offset += bufferShift;
			}
			while (--height != 0);
		}

		static void putcontig8bitYCbCr12tile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			bufferShift = bufferShift / 2 * 4;
			int num = rasterOffset + width + rasterShift;
			while (height >= 2)
			{
				x = width;
				do
				{
					int cb = (int)buffer[offset + 2];
					int cr = (int)buffer[offset + 3];
					img.YCbCrtoRGB(out raster[rasterOffset], (int)buffer[offset], cb, cr);
					img.YCbCrtoRGB(out raster[num], (int)buffer[offset + 1], cb, cr);
					rasterOffset++;
					num++;
					offset += 4;
				}
				while (--x != 0);
				rasterOffset += rasterShift * 2 + width;
				num += rasterShift * 2 + width;
				offset += bufferShift;
				height -= 2;
			}
			if (height == 1)
			{
				x = width;
				do
				{
					int cb2 = (int)buffer[offset + 2];
					int cr2 = (int)buffer[offset + 3];
					img.YCbCrtoRGB(out raster[rasterOffset], (int)buffer[offset], cb2, cr2);
					rasterOffset++;
					offset += 4;
				}
				while (--x != 0);
			}
		}

		static void putRGBseparate8bittile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset1, int offset2, int offset3, int offset4, int bufferShift)
		{
			while (height-- > 0)
			{
				int i;
				for (i = width; i >= 8; i -= 8)
				{
					for (int j = 0; j < 8; j++)
					{
						raster[rasterOffset] = TiffRgbaImage.PACK((int)buffer[offset1], (int)buffer[offset2], (int)buffer[offset3]);
						rasterOffset++;
						offset1++;
						offset2++;
						offset3++;
					}
				}
				if (i > 0 && i <= 7 && i > 0)
				{
					for (int k = i; k > 0; k--)
					{
						raster[rasterOffset] = TiffRgbaImage.PACK((int)buffer[offset1], (int)buffer[offset2], (int)buffer[offset3]);
						rasterOffset++;
						offset1++;
						offset2++;
						offset3++;
					}
				}
				offset1 += bufferShift;
				offset2 += bufferShift;
				offset3 += bufferShift;
				rasterOffset += rasterShift;
			}
		}

		static void putRGBAAseparate8bittile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset1, int offset2, int offset3, int offset4, int bufferShift)
		{
			while (height-- > 0)
			{
				int i;
				for (i = width; i >= 8; i -= 8)
				{
					for (int j = 0; j < 8; j++)
					{
						raster[rasterOffset] = TiffRgbaImage.PACK4((int)buffer[offset1], (int)buffer[offset2], (int)buffer[offset3], (int)buffer[offset4]);
						rasterOffset++;
						offset1++;
						offset2++;
						offset3++;
						offset4++;
					}
				}
				if (i > 0 && i <= 7 && i > 0)
				{
					for (int k = i; k > 0; k--)
					{
						raster[rasterOffset] = TiffRgbaImage.PACK4((int)buffer[offset1], (int)buffer[offset2], (int)buffer[offset3], (int)buffer[offset4]);
						rasterOffset++;
						offset1++;
						offset2++;
						offset3++;
						offset4++;
					}
				}
				offset1 += bufferShift;
				offset2 += bufferShift;
				offset3 += bufferShift;
				offset4 += bufferShift;
				rasterOffset += rasterShift;
			}
		}

		static void putRGBUAseparate8bittile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset1, int offset2, int offset3, int offset4, int bufferShift)
		{
			while (height-- > 0)
			{
				x = width;
				while (x-- > 0)
				{
					int num = (int)buffer[offset4];
					int r = ((int)buffer[offset1] * num + 127) / 255;
					int g = ((int)buffer[offset2] * num + 127) / 255;
					int b = ((int)buffer[offset3] * num + 127) / 255;
					raster[rasterOffset] = TiffRgbaImage.PACK4(r, g, b, num);
					rasterOffset++;
					offset1++;
					offset2++;
					offset3++;
					offset4++;
				}
				offset1 += bufferShift;
				offset2 += bufferShift;
				offset3 += bufferShift;
				offset4 += bufferShift;
				rasterOffset += rasterShift;
			}
		}

		static void putRGBseparate16bittile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset1, int offset2, int offset3, int offset4, int bufferShift)
		{
			short[] array = Tiff.ByteArrayToShorts(buffer, 0, buffer.Length);
			offset1 /= 2;
			offset2 /= 2;
			offset3 /= 2;
			while (height-- > 0)
			{
				for (x = 0; x < width; x++)
				{
					raster[rasterOffset] = TiffRgbaImage.PACKW(array[offset1], array[offset2], array[offset3]);
					rasterOffset++;
					offset1++;
					offset2++;
					offset3++;
				}
				offset1 += bufferShift;
				offset2 += bufferShift;
				offset3 += bufferShift;
				rasterOffset += rasterShift;
			}
		}

		static void putRGBAAseparate16bittile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset1, int offset2, int offset3, int offset4, int bufferShift)
		{
			short[] array = Tiff.ByteArrayToShorts(buffer, 0, buffer.Length);
			offset1 /= 2;
			offset2 /= 2;
			offset3 /= 2;
			offset4 /= 2;
			while (height-- > 0)
			{
				for (x = 0; x < width; x++)
				{
					raster[rasterOffset] = TiffRgbaImage.PACKW4(array[offset1], array[offset2], array[offset3], array[offset4]);
					rasterOffset++;
					offset1++;
					offset2++;
					offset3++;
					offset4++;
				}
				offset1 += bufferShift;
				offset2 += bufferShift;
				offset3 += bufferShift;
				offset4 += bufferShift;
				rasterOffset += rasterShift;
			}
		}

		static void putRGBUAseparate16bittile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset1, int offset2, int offset3, int offset4, int bufferShift)
		{
			short[] array = Tiff.ByteArrayToShorts(buffer, 0, buffer.Length);
			offset1 /= 2;
			offset2 /= 2;
			offset3 /= 2;
			offset4 /= 2;
			while (height-- > 0)
			{
				x = width;
				while (x-- > 0)
				{
					int num = TiffRgbaImage.W2B(array[offset4]);
					int r = (TiffRgbaImage.W2B(array[offset1]) * num + 127) / 255;
					int g = (TiffRgbaImage.W2B(array[offset2]) * num + 127) / 255;
					int b = (TiffRgbaImage.W2B(array[offset3]) * num + 127) / 255;
					raster[rasterOffset] = TiffRgbaImage.PACK4(r, g, b, num);
					rasterOffset++;
					offset1++;
					offset2++;
					offset3++;
					offset4++;
				}
				offset1 += bufferShift;
				offset2 += bufferShift;
				offset3 += bufferShift;
				offset4 += bufferShift;
				rasterOffset += rasterShift;
			}
		}

		static void putseparate8bitYCbCr11tile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset1, int offset2, int offset3, int offset4, int bufferShift)
		{
			while (height-- > 0)
			{
				x = width;
				do
				{
					int r;
					int g;
					int b;
					img.ycbcr.YCbCrtoRGB((int)buffer[offset1], (int)buffer[offset2], (int)buffer[offset3], out r, out g, out b);
					raster[rasterOffset] = TiffRgbaImage.PACK(r, g, b);
					rasterOffset++;
					offset1++;
					offset2++;
					offset3++;
				}
				while (--x != 0);
				offset1 += bufferShift;
				offset2 += bufferShift;
				offset3 += bufferShift;
				rasterOffset += rasterShift;
			}
		}

		static void putRGBcontig8bitCMYKMaptile(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift)
		{
			int num = (int)img.samplesperpixel;
			byte[] map = img.Map;
			bufferShift *= num;
			while (height-- > 0)
			{
				x = width;
				while (x-- > 0)
				{
					short num2 = (short)(byte.MaxValue - buffer[offset + 3]);
					short num3 = (short)(num2 * (short)(byte.MaxValue - buffer[offset]) / 255);
					short num4 = (short)(num2 * (short)(byte.MaxValue - buffer[offset + 1]) / 255);
					short num5 = (short)(num2 * (short)(byte.MaxValue - buffer[offset + 2]) / 255);
					raster[rasterOffset] = TiffRgbaImage.PACK((int)map[(int)num3], (int)map[(int)num4], (int)map[(int)num5]);
					rasterOffset++;
					offset += num;
				}
				offset += bufferShift;
				rasterOffset += rasterShift;
			}
		}

		internal const string photoTag = "PhotometricInterpretation";

		const int A1 = -16777216;

		const int FLIP_VERTICALLY = 1;

		const int FLIP_HORIZONTALLY = 2;

		Tiff tif;

		bool stoponerr;

		bool isContig;

		ExtraSample alpha;

		int width;

		int height;

		short bitspersample;

		short samplesperpixel;

		Orientation orientation;

		Orientation req_orientation;

		Photometric photometric;

		short[] redcmap;

		short[] greencmap;

		short[] bluecmap;

		TiffRgbaImage.GetDelegate get;

		TiffRgbaImage.PutContigDelegate putContig;

		TiffRgbaImage.PutSeparateDelegate putSeparate;

		byte[] Map;

		int[][] BWmap;

		int[][] PALmap;

		TiffYCbCrToRGB ycbcr;

		TiffCIELabToRGB cielab;

		static readonly TiffDisplay display_sRGB = new TiffDisplay(new float[] { 3.241f, 1.5374f, -0.4986f }, new float[] { -0.9692f, 1.876f, 0.0416f }, new float[] { 0.0556f, -0.204f, 1.057f }, 100f, 100f, 100f, 255, 255, 255, 1f, 1f, 1f, 2.4f, 2.4f, 2.4f);

		internal int row_offset;

		internal int col_offset;

		public delegate void PutContigDelegate(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset, int bufferShift);

		public delegate void PutSeparateDelegate(TiffRgbaImage img, int[] raster, int rasterOffset, int rasterShift, int x, int y, int width, int height, byte[] buffer, int offset1, int offset2, int offset3, int offset4, int bufferShift);

		internal delegate bool GetDelegate(TiffRgbaImage img, int[] raster, int offset, int width, int height);
	}
}
