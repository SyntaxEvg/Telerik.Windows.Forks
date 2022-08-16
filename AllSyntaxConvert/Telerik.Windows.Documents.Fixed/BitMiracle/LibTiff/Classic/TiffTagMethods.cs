using System;
using System.IO;
using BitMiracle.LibTiff.Classic.Internal;

namespace BitMiracle.LibTiff.Classic
{
	class TiffTagMethods
	{
		public virtual bool SetField(Tiff tif, TiffTag tag, FieldValue[] value)
		{
			TiffDirectory dir = tif.m_dir;
			bool flag = true;
			int num = 0;
			int num2 = 0;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			if (tag <= TiffTag.TILELENGTH)
			{
				if (tag <= TiffTag.PAGENUMBER)
				{
					switch (tag)
					{
					case TiffTag.SUBFILETYPE:
						dir.td_subfiletype = (FileType)value[0].ToByte();
						goto IL_EC6;
					case TiffTag.OSUBFILETYPE:
					case (TiffTag)260:
					case (TiffTag)261:
					case TiffTag.CELLWIDTH:
					case TiffTag.CELLLENGTH:
					case (TiffTag)267:
					case (TiffTag)268:
					case TiffTag.DOCUMENTNAME:
					case TiffTag.IMAGEDESCRIPTION:
					case TiffTag.MAKE:
					case TiffTag.MODEL:
					case TiffTag.STRIPOFFSETS:
					case (TiffTag)275:
					case (TiffTag)276:
					case TiffTag.STRIPBYTECOUNTS:
					case TiffTag.PAGENAME:
						break;
					case TiffTag.IMAGEWIDTH:
						dir.td_imagewidth = value[0].ToInt();
						goto IL_EC6;
					case TiffTag.IMAGELENGTH:
						dir.td_imagelength = value[0].ToInt();
						goto IL_EC6;
					case TiffTag.BITSPERSAMPLE:
						dir.td_bitspersample = value[0].ToShort();
						if ((tif.m_flags & TiffFlags.SWAB) != TiffFlags.SWAB)
						{
							goto IL_EC6;
						}
						if (dir.td_bitspersample == 16)
						{
							tif.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmSwab16Bit;
							goto IL_EC6;
						}
						if (dir.td_bitspersample == 24)
						{
							tif.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmSwab24Bit;
							goto IL_EC6;
						}
						if (dir.td_bitspersample == 32)
						{
							tif.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmSwab32Bit;
							goto IL_EC6;
						}
						if (dir.td_bitspersample == 64)
						{
							tif.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmSwab64Bit;
							goto IL_EC6;
						}
						if (dir.td_bitspersample == 128)
						{
							tif.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmSwab64Bit;
							goto IL_EC6;
						}
						goto IL_EC6;
					case TiffTag.COMPRESSION:
					{
						num2 = value[0].ToInt() & 65535;
						Compression compression = (Compression)num2;
						if (tif.fieldSet(7))
						{
							if (dir.td_compression == compression)
							{
								goto IL_EC6;
							}
							tif.m_currentCodec.Cleanup();
							tif.m_flags &= ~TiffFlags.CODERSETUP;
						}
						flag = tif.setCompressionScheme(compression);
						if (flag)
						{
							dir.td_compression = compression;
							goto IL_EC6;
						}
						flag = false;
						goto IL_EC6;
					}
					case TiffTag.PHOTOMETRIC:
						dir.td_photometric = (Photometric)value[0].ToInt();
						goto IL_EC6;
					case TiffTag.THRESHHOLDING:
						dir.td_threshholding = (Threshold)value[0].ToByte();
						goto IL_EC6;
					case TiffTag.FILLORDER:
					{
						num2 = value[0].ToInt();
						FillOrder fillOrder = (FillOrder)num2;
						if (fillOrder != FillOrder.LSB2MSB && fillOrder != FillOrder.MSB2LSB)
						{
							flag3 = true;
							goto IL_EC6;
						}
						dir.td_fillorder = fillOrder;
						goto IL_EC6;
					}
					case TiffTag.ORIENTATION:
					{
						num2 = value[0].ToInt();
						Orientation orientation = (Orientation)num2;
						if (orientation < Orientation.TOPLEFT || Orientation.LEFTBOT < orientation)
						{
							flag3 = true;
							goto IL_EC6;
						}
						dir.td_orientation = orientation;
						goto IL_EC6;
					}
					case TiffTag.SAMPLESPERPIXEL:
						num2 = value[0].ToInt();
						if (num2 == 0)
						{
							flag3 = true;
							goto IL_EC6;
						}
						dir.td_samplesperpixel = (short)num2;
						goto IL_EC6;
					case TiffTag.ROWSPERSTRIP:
						num = value[0].ToInt();
						if (num == 0)
						{
							flag4 = true;
							goto IL_EC6;
						}
						dir.td_rowsperstrip = num;
						if (!tif.fieldSet(2))
						{
							dir.td_tilelength = num;
							dir.td_tilewidth = dir.td_imagewidth;
							goto IL_EC6;
						}
						goto IL_EC6;
					case TiffTag.MINSAMPLEVALUE:
						dir.td_minsamplevalue = value[0].ToShort();
						goto IL_EC6;
					case TiffTag.MAXSAMPLEVALUE:
						dir.td_maxsamplevalue = value[0].ToShort();
						goto IL_EC6;
					case TiffTag.XRESOLUTION:
						dir.td_xresolution = value[0].ToFloat();
						goto IL_EC6;
					case TiffTag.YRESOLUTION:
						dir.td_yresolution = value[0].ToFloat();
						goto IL_EC6;
					case TiffTag.PLANARCONFIG:
					{
						num2 = value[0].ToInt();
						PlanarConfig planarConfig = (PlanarConfig)num2;
						if (planarConfig != PlanarConfig.CONTIG && planarConfig != PlanarConfig.SEPARATE)
						{
							flag3 = true;
							goto IL_EC6;
						}
						dir.td_planarconfig = planarConfig;
						goto IL_EC6;
					}
					case TiffTag.XPOSITION:
						dir.td_xposition = value[0].ToFloat();
						goto IL_EC6;
					case TiffTag.YPOSITION:
						dir.td_yposition = value[0].ToFloat();
						goto IL_EC6;
					default:
						switch (tag)
						{
						case TiffTag.RESOLUTIONUNIT:
						{
							num2 = value[0].ToInt();
							ResUnit resUnit = (ResUnit)num2;
							if (resUnit < ResUnit.NONE || ResUnit.CENTIMETER < resUnit)
							{
								flag3 = true;
								goto IL_EC6;
							}
							dir.td_resolutionunit = resUnit;
							goto IL_EC6;
						}
						case TiffTag.PAGENUMBER:
							dir.td_pagenumber[0] = value[0].ToShort();
							dir.td_pagenumber[1] = value[1].ToShort();
							goto IL_EC6;
						}
						break;
					}
				}
				else
				{
					if (tag == TiffTag.TRANSFERFUNCTION)
					{
						num2 = ((dir.td_samplesperpixel - dir.td_extrasamples > 1) ? 3 : 1);
						for (int i = 0; i < num2; i++)
						{
							Tiff.setShortArray(out dir.td_transferfunction[i], value[0].ToShortArray(), 1 << (int)dir.td_bitspersample);
						}
						goto IL_EC6;
					}
					switch (tag)
					{
					case TiffTag.COLORMAP:
						num = 1 << (int)dir.td_bitspersample;
						Tiff.setShortArray(out dir.td_colormap[0], value[0].ToShortArray(), num);
						Tiff.setShortArray(out dir.td_colormap[1], value[1].ToShortArray(), num);
						Tiff.setShortArray(out dir.td_colormap[2], value[2].ToShortArray(), num);
						goto IL_EC6;
					case TiffTag.HALFTONEHINTS:
						dir.td_halftonehints[0] = value[0].ToShort();
						dir.td_halftonehints[1] = value[1].ToShort();
						goto IL_EC6;
					case TiffTag.TILEWIDTH:
						num = value[0].ToInt();
						if (num % 16 != 0)
						{
							if (tif.m_mode != 0)
							{
								flag4 = true;
								goto IL_EC6;
							}
							Tiff.WarningExt(tif, tif.m_clientdata, tif.m_name, "Nonstandard tile width {0}, convert file", new object[] { num });
						}
						dir.td_tilewidth = num;
						tif.m_flags |= TiffFlags.ISTILED;
						goto IL_EC6;
					case TiffTag.TILELENGTH:
						num = value[0].ToInt();
						if (num % 16 != 0)
						{
							if (tif.m_mode != 0)
							{
								flag4 = true;
								goto IL_EC6;
							}
							Tiff.WarningExt(tif, tif.m_clientdata, tif.m_name, "Nonstandard tile length {0}, convert file", new object[] { num });
						}
						dir.td_tilelength = num;
						tif.m_flags |= TiffFlags.ISTILED;
						goto IL_EC6;
					}
				}
			}
			else if (tag <= TiffTag.SMAXSAMPLEVALUE)
			{
				if (tag != TiffTag.SUBIFD)
				{
					switch (tag)
					{
					case TiffTag.INKNAMES:
					{
						num2 = value[0].ToInt();
						string text = value[1].ToString();
						num2 = TiffTagMethods.checkInkNamesString(tif, num2, text);
						flag = num2 > 0;
						if (num2 > 0)
						{
							TiffTagMethods.setNString(out dir.td_inknames, text, num2);
							dir.td_inknameslen = num2;
							goto IL_EC6;
						}
						goto IL_EC6;
					}
					case TiffTag.EXTRASAMPLES:
						if (!TiffTagMethods.setExtraSamples(dir, ref num2, value))
						{
							flag3 = true;
							goto IL_EC6;
						}
						goto IL_EC6;
					case TiffTag.SAMPLEFORMAT:
					{
						num2 = value[0].ToInt();
						SampleFormat sampleFormat = (SampleFormat)num2;
						if (sampleFormat < SampleFormat.UINT || SampleFormat.COMPLEXIEEEFP < sampleFormat)
						{
							flag3 = true;
							goto IL_EC6;
						}
						dir.td_sampleformat = sampleFormat;
						if (dir.td_sampleformat == SampleFormat.COMPLEXINT && dir.td_bitspersample == 32 && tif.m_postDecodeMethod == Tiff.PostDecodeMethodType.pdmSwab32Bit)
						{
							tif.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmSwab16Bit;
							goto IL_EC6;
						}
						if ((dir.td_sampleformat == SampleFormat.COMPLEXINT || dir.td_sampleformat == SampleFormat.COMPLEXIEEEFP) && dir.td_bitspersample == 64 && tif.m_postDecodeMethod == Tiff.PostDecodeMethodType.pdmSwab64Bit)
						{
							tif.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmSwab32Bit;
							goto IL_EC6;
						}
						goto IL_EC6;
					}
					case TiffTag.SMINSAMPLEVALUE:
						dir.td_sminsamplevalue = value[0].ToDouble();
						goto IL_EC6;
					case TiffTag.SMAXSAMPLEVALUE:
						dir.td_smaxsamplevalue = value[0].ToDouble();
						goto IL_EC6;
					}
				}
				else
				{
					if ((tif.m_flags & TiffFlags.INSUBIFD) != TiffFlags.INSUBIFD)
					{
						dir.td_nsubifd = value[0].ToShort();
						Tiff.setLongArray(out dir.td_subifd, value[1].ToIntArray(), (int)dir.td_nsubifd);
						goto IL_EC6;
					}
					Tiff.ErrorExt(tif, tif.m_clientdata, "vsetfield", "{0}: Sorry, cannot nest SubIFDs", new object[] { tif.m_name });
					flag = false;
					goto IL_EC6;
				}
			}
			else
			{
				switch (tag)
				{
				case TiffTag.YCBCRSUBSAMPLING:
					dir.td_ycbcrsubsampling[0] = value[0].ToShort();
					dir.td_ycbcrsubsampling[1] = value[1].ToShort();
					goto IL_EC6;
				case TiffTag.YCBCRPOSITIONING:
					dir.td_ycbcrpositioning = (YCbCrPosition)value[0].ToByte();
					goto IL_EC6;
				case TiffTag.REFERENCEBLACKWHITE:
					Tiff.setFloatArray(out dir.td_refblackwhite, value[0].ToFloatArray(), 6);
					goto IL_EC6;
				default:
					switch (tag)
					{
					case TiffTag.MATTEING:
						if (value[0].ToShort() != 0)
						{
							dir.td_extrasamples = 1;
						}
						else
						{
							dir.td_extrasamples = 0;
						}
						if (dir.td_extrasamples != 0)
						{
							dir.td_sampleinfo = new ExtraSample[1];
							dir.td_sampleinfo[0] = ExtraSample.ASSOCALPHA;
							goto IL_EC6;
						}
						goto IL_EC6;
					case TiffTag.DATATYPE:
					{
						num2 = value[0].ToInt();
						SampleFormat sampleFormat = SampleFormat.VOID;
						switch (num2)
						{
						case 0:
							sampleFormat = SampleFormat.VOID;
							break;
						case 1:
							sampleFormat = SampleFormat.INT;
							break;
						case 2:
							sampleFormat = SampleFormat.UINT;
							break;
						case 3:
							sampleFormat = SampleFormat.IEEEFP;
							break;
						default:
							flag3 = true;
							break;
						}
						if (!flag3)
						{
							dir.td_sampleformat = sampleFormat;
							goto IL_EC6;
						}
						goto IL_EC6;
					}
					case TiffTag.IMAGEDEPTH:
						dir.td_imagedepth = value[0].ToInt();
						goto IL_EC6;
					case TiffTag.TILEDEPTH:
						num = value[0].ToInt();
						if (num == 0)
						{
							flag4 = true;
							goto IL_EC6;
						}
						dir.td_tiledepth = num;
						goto IL_EC6;
					}
					break;
				}
			}
			TiffFieldInfo tiffFieldInfo = tif.FindFieldInfo(tag, TiffType.NOTYPE);
			if (tiffFieldInfo == null || tiffFieldInfo.Bit != 65)
			{
				Tiff.ErrorExt(tif, tif.m_clientdata, "vsetfield", "{0}: Invalid {1}tag \"{2}\" (not supported by codec)", new object[]
				{
					tif.m_name,
					Tiff.isPseudoTag(tag) ? "pseudo-" : "",
					(tiffFieldInfo != null) ? tiffFieldInfo.Name : "Unknown"
				});
				flag = false;
			}
			else
			{
				int num3 = -1;
				for (int j = 0; j < dir.td_customValueCount; j++)
				{
					if (dir.td_customValues[j].info.Tag == tag)
					{
						num3 = j;
						dir.td_customValues[j].value = null;
						break;
					}
				}
				if (num3 == -1)
				{
					dir.td_customValueCount++;
					TiffTagValue[] td_customValues = Tiff.Realloc(dir.td_customValues, dir.td_customValueCount - 1, dir.td_customValueCount);
					dir.td_customValues = td_customValues;
					num3 = dir.td_customValueCount - 1;
					dir.td_customValues[num3].info = tiffFieldInfo;
					dir.td_customValues[num3].value = null;
					dir.td_customValues[num3].count = 0;
				}
				int num4 = Tiff.dataSize(tiffFieldInfo.Type);
				if (num4 == 0)
				{
					flag = false;
					Tiff.ErrorExt(tif, tif.m_clientdata, "vsetfield", "{0}: Bad field type {1} for \"{2}\"", new object[] { tif.m_name, tiffFieldInfo.Type, tiffFieldInfo.Name });
					flag2 = true;
				}
				else
				{
					int num5 = 0;
					if (tiffFieldInfo.PassCount)
					{
						if (tiffFieldInfo.WriteCount == -3)
						{
							dir.td_customValues[num3].count = value[num5++].ToInt();
						}
						else
						{
							dir.td_customValues[num3].count = value[num5++].ToInt();
						}
					}
					else if (tiffFieldInfo.WriteCount == -1 || tiffFieldInfo.WriteCount == -3)
					{
						dir.td_customValues[num3].count = 1;
					}
					else if (tiffFieldInfo.WriteCount == -2)
					{
						dir.td_customValues[num3].count = (int)dir.td_samplesperpixel;
					}
					else
					{
						dir.td_customValues[num3].count = (int)tiffFieldInfo.WriteCount;
					}
					if (tiffFieldInfo.Type == TiffType.ASCII)
					{
						string s;
						Tiff.setString(out s, value[num5++].ToString());
						dir.td_customValues[num3].value = Tiff.Latin1Encoding.GetBytes(s);
					}
					else
					{
						dir.td_customValues[num3].value = new byte[num4 * dir.td_customValues[num3].count];
						if ((tiffFieldInfo.PassCount || tiffFieldInfo.WriteCount == -1 || tiffFieldInfo.WriteCount == -3 || tiffFieldInfo.WriteCount == -2 || dir.td_customValues[num3].count > 1) && tiffFieldInfo.Tag != TiffTag.PAGENUMBER && tiffFieldInfo.Tag != TiffTag.HALFTONEHINTS && tiffFieldInfo.Tag != TiffTag.YCBCRSUBSAMPLING && tiffFieldInfo.Tag != TiffTag.DOTRANGE)
						{
							byte[] bytes = value[num5++].GetBytes();
							Buffer.BlockCopy(bytes, 0, dir.td_customValues[num3].value, 0, dir.td_customValues[num3].value.Length);
						}
						else
						{
							byte[] value2 = dir.td_customValues[num3].value;
							int num6 = 0;
							int k = 0;
							while (k < dir.td_customValues[num3].count)
							{
								switch (tiffFieldInfo.Type)
								{
								case TiffType.BYTE:
								case TiffType.UNDEFINED:
									value2[num6] = value[num5 + k].GetBytes()[0];
									break;
								case TiffType.ASCII:
									goto IL_E93;
								case TiffType.SHORT:
									Buffer.BlockCopy(BitConverter.GetBytes(value[num5 + k].ToShort()), 0, value2, num6, num4);
									break;
								case TiffType.LONG:
								case TiffType.IFD:
									Buffer.BlockCopy(BitConverter.GetBytes(value[num5 + k].ToInt()), 0, value2, num6, num4);
									break;
								case TiffType.RATIONAL:
								case TiffType.SRATIONAL:
								case TiffType.FLOAT:
									Buffer.BlockCopy(BitConverter.GetBytes(value[num5 + k].ToFloat()), 0, value2, num6, num4);
									break;
								case TiffType.SBYTE:
									value2[num6] = value[num5 + k].ToByte();
									break;
								case TiffType.SSHORT:
									Buffer.BlockCopy(BitConverter.GetBytes(value[num5 + k].ToShort()), 0, value2, num6, num4);
									break;
								case TiffType.SLONG:
									Buffer.BlockCopy(BitConverter.GetBytes(value[num5 + k].ToInt()), 0, value2, num6, num4);
									break;
								case TiffType.DOUBLE:
									Buffer.BlockCopy(BitConverter.GetBytes(value[num5 + k].ToDouble()), 0, value2, num6, num4);
									break;
								default:
									goto IL_E93;
								}
								IL_EA0:
								k++;
								num6 += num4;
								continue;
								IL_E93:
								Array.Clear(value2, num6, num4);
								flag = false;
								goto IL_EA0;
							}
						}
					}
				}
			}
			IL_EC6:
			if (!flag2 && !flag3 && !flag4 && flag)
			{
				tif.setFieldBit((int)tif.FieldWithTag(tag).Bit);
				tif.m_flags |= TiffFlags.DIRTYDIRECT;
			}
			if (flag3)
			{
				Tiff.ErrorExt(tif, tif.m_clientdata, "vsetfield", "{0}: Bad value {1} for \"{2}\" tag", new object[]
				{
					tif.m_name,
					num2,
					tif.FieldWithTag(tag).Name
				});
				return false;
			}
			if (flag4)
			{
				Tiff.ErrorExt(tif, tif.m_clientdata, "vsetfield", "{0}: Bad value {1} for \"{2}\" tag", new object[]
				{
					tif.m_name,
					num,
					tif.FieldWithTag(tag).Name
				});
				return false;
			}
			return flag;
		}

		public virtual FieldValue[] GetField(Tiff tif, TiffTag tag)
		{
			TiffDirectory dir = tif.m_dir;
			FieldValue[] array = null;
			if (tag <= TiffTag.TRANSFERFUNCTION)
			{
				switch (tag)
				{
				case TiffTag.SUBFILETYPE:
					array = new FieldValue[1];
					array[0].Set(dir.td_subfiletype);
					return array;
				case TiffTag.OSUBFILETYPE:
				case (TiffTag)260:
				case (TiffTag)261:
				case TiffTag.CELLWIDTH:
				case TiffTag.CELLLENGTH:
				case (TiffTag)267:
				case (TiffTag)268:
				case TiffTag.DOCUMENTNAME:
				case TiffTag.IMAGEDESCRIPTION:
				case TiffTag.MAKE:
				case TiffTag.MODEL:
				case (TiffTag)275:
				case (TiffTag)276:
				case TiffTag.PAGENAME:
					goto IL_840;
				case TiffTag.IMAGEWIDTH:
					array = new FieldValue[1];
					array[0].Set(dir.td_imagewidth);
					return array;
				case TiffTag.IMAGELENGTH:
					array = new FieldValue[1];
					array[0].Set(dir.td_imagelength);
					return array;
				case TiffTag.BITSPERSAMPLE:
					array = new FieldValue[1];
					array[0].Set(dir.td_bitspersample);
					return array;
				case TiffTag.COMPRESSION:
					array = new FieldValue[1];
					array[0].Set(dir.td_compression);
					return array;
				case TiffTag.PHOTOMETRIC:
					array = new FieldValue[1];
					array[0].Set(dir.td_photometric);
					return array;
				case TiffTag.THRESHHOLDING:
					array = new FieldValue[1];
					array[0].Set(dir.td_threshholding);
					return array;
				case TiffTag.FILLORDER:
					array = new FieldValue[1];
					array[0].Set(dir.td_fillorder);
					return array;
				case TiffTag.STRIPOFFSETS:
					break;
				case TiffTag.ORIENTATION:
					array = new FieldValue[1];
					array[0].Set(dir.td_orientation);
					return array;
				case TiffTag.SAMPLESPERPIXEL:
					array = new FieldValue[1];
					array[0].Set(dir.td_samplesperpixel);
					return array;
				case TiffTag.ROWSPERSTRIP:
					array = new FieldValue[1];
					array[0].Set(dir.td_rowsperstrip);
					return array;
				case TiffTag.STRIPBYTECOUNTS:
					goto IL_534;
				case TiffTag.MINSAMPLEVALUE:
					array = new FieldValue[1];
					array[0].Set(dir.td_minsamplevalue);
					return array;
				case TiffTag.MAXSAMPLEVALUE:
					array = new FieldValue[1];
					array[0].Set(dir.td_maxsamplevalue);
					return array;
				case TiffTag.XRESOLUTION:
					array = new FieldValue[1];
					array[0].Set(dir.td_xresolution);
					return array;
				case TiffTag.YRESOLUTION:
					array = new FieldValue[1];
					array[0].Set(dir.td_yresolution);
					return array;
				case TiffTag.PLANARCONFIG:
					array = new FieldValue[1];
					array[0].Set(dir.td_planarconfig);
					return array;
				case TiffTag.XPOSITION:
					array = new FieldValue[1];
					array[0].Set(dir.td_xposition);
					return array;
				case TiffTag.YPOSITION:
					array = new FieldValue[1];
					array[0].Set(dir.td_yposition);
					return array;
				default:
					switch (tag)
					{
					case TiffTag.RESOLUTIONUNIT:
						array = new FieldValue[1];
						array[0].Set(dir.td_resolutionunit);
						return array;
					case TiffTag.PAGENUMBER:
						array = new FieldValue[2];
						array[0].Set(dir.td_pagenumber[0]);
						array[1].Set(dir.td_pagenumber[1]);
						return array;
					default:
						if (tag != TiffTag.TRANSFERFUNCTION)
						{
							goto IL_840;
						}
						array = new FieldValue[3];
						array[0].Set(dir.td_transferfunction[0]);
						if (dir.td_samplesperpixel - dir.td_extrasamples > 1)
						{
							array[1].Set(dir.td_transferfunction[1]);
							array[2].Set(dir.td_transferfunction[2]);
							return array;
						}
						return array;
					}
					break;
				}
			}
			else
			{
				switch (tag)
				{
				case TiffTag.COLORMAP:
					array = new FieldValue[3];
					array[0].Set(dir.td_colormap[0]);
					array[1].Set(dir.td_colormap[1]);
					array[2].Set(dir.td_colormap[2]);
					return array;
				case TiffTag.HALFTONEHINTS:
					array = new FieldValue[2];
					array[0].Set(dir.td_halftonehints[0]);
					array[1].Set(dir.td_halftonehints[1]);
					return array;
				case TiffTag.TILEWIDTH:
					array = new FieldValue[1];
					array[0].Set(dir.td_tilewidth);
					return array;
				case TiffTag.TILELENGTH:
					array = new FieldValue[1];
					array[0].Set(dir.td_tilelength);
					return array;
				case TiffTag.TILEOFFSETS:
					break;
				case TiffTag.TILEBYTECOUNTS:
					goto IL_534;
				case TiffTag.BADFAXLINES:
				case TiffTag.CLEANFAXDATA:
				case TiffTag.CONSECUTIVEBADFAXLINES:
				case (TiffTag)329:
				case (TiffTag)331:
				case TiffTag.INKSET:
				case TiffTag.NUMBEROFINKS:
				case (TiffTag)335:
				case TiffTag.DOTRANGE:
				case TiffTag.TARGETPRINTER:
					goto IL_840;
				case TiffTag.SUBIFD:
					array = new FieldValue[2];
					array[0].Set(dir.td_nsubifd);
					array[1].Set(dir.td_subifd);
					return array;
				case TiffTag.INKNAMES:
					array = new FieldValue[1];
					array[0].Set(dir.td_inknames);
					return array;
				case TiffTag.EXTRASAMPLES:
					array = new FieldValue[2];
					array[0].Set(dir.td_extrasamples);
					array[1].Set(dir.td_sampleinfo);
					return array;
				case TiffTag.SAMPLEFORMAT:
					array = new FieldValue[1];
					array[0].Set(dir.td_sampleformat);
					return array;
				case TiffTag.SMINSAMPLEVALUE:
					array = new FieldValue[1];
					array[0].Set(dir.td_sminsamplevalue);
					return array;
				case TiffTag.SMAXSAMPLEVALUE:
					array = new FieldValue[1];
					array[0].Set(dir.td_smaxsamplevalue);
					return array;
				default:
					switch (tag)
					{
					case TiffTag.YCBCRSUBSAMPLING:
						array = new FieldValue[2];
						array[0].Set(dir.td_ycbcrsubsampling[0]);
						array[1].Set(dir.td_ycbcrsubsampling[1]);
						return array;
					case TiffTag.YCBCRPOSITIONING:
						array = new FieldValue[1];
						array[0].Set(dir.td_ycbcrpositioning);
						return array;
					case TiffTag.REFERENCEBLACKWHITE:
						if (dir.td_refblackwhite != null)
						{
							array = new FieldValue[1];
							array[0].Set(dir.td_refblackwhite);
							return array;
						}
						return array;
					default:
						switch (tag)
						{
						case TiffTag.MATTEING:
							array = new FieldValue[1];
							array[0].Set(dir.td_extrasamples == 1 && dir.td_sampleinfo[0] == ExtraSample.ASSOCALPHA);
							return array;
						case TiffTag.DATATYPE:
							switch (dir.td_sampleformat)
							{
							case SampleFormat.UINT:
								array = new FieldValue[1];
								array[0].Set(2);
								return array;
							case SampleFormat.INT:
								array = new FieldValue[1];
								array[0].Set(1);
								return array;
							case SampleFormat.IEEEFP:
								array = new FieldValue[1];
								array[0].Set(3);
								return array;
							case SampleFormat.VOID:
								array = new FieldValue[1];
								array[0].Set(0);
								return array;
							default:
								return array;
							}
							break;
						case TiffTag.IMAGEDEPTH:
							array = new FieldValue[1];
							array[0].Set(dir.td_imagedepth);
							return array;
						case TiffTag.TILEDEPTH:
							array = new FieldValue[1];
							array[0].Set(dir.td_tiledepth);
							return array;
						default:
							goto IL_840;
						}
						break;
					}
					break;
				}
			}
			array = new FieldValue[1];
			array[0].Set(dir.td_stripoffset);
			return array;
			IL_534:
			array = new FieldValue[1];
			array[0].Set(dir.td_stripbytecount);
			return array;
			IL_840:
			TiffFieldInfo tiffFieldInfo = tif.FindFieldInfo(tag, TiffType.NOTYPE);
			if (tiffFieldInfo == null || tiffFieldInfo.Bit != 65)
			{
				Tiff.ErrorExt(tif, tif.m_clientdata, "_TIFFVGetField", "{0}: Invalid {1}tag \"{2}\" (not supported by codec)", new object[]
				{
					tif.m_name,
					Tiff.isPseudoTag(tag) ? "pseudo-" : "",
					(tiffFieldInfo != null) ? tiffFieldInfo.Name : "Unknown"
				});
				array = null;
			}
			else
			{
				array = null;
				int i = 0;
				while (i < dir.td_customValueCount)
				{
					TiffTagValue tiffTagValue = dir.td_customValues[i];
					if (tiffTagValue.info.Tag == tag)
					{
						if (tiffFieldInfo.PassCount)
						{
							array = new FieldValue[2];
							if (tiffFieldInfo.ReadCount == -3)
							{
								array[0].Set(tiffTagValue.count);
							}
							else
							{
								array[0].Set(tiffTagValue.count);
							}
							array[1].Set(tiffTagValue.value);
							break;
						}
						if ((tiffFieldInfo.Type == TiffType.ASCII || tiffFieldInfo.ReadCount == -1 || tiffFieldInfo.ReadCount == -3 || tiffFieldInfo.ReadCount == -2 || tiffTagValue.count > 1) && tiffFieldInfo.Tag != TiffTag.PAGENUMBER && tiffFieldInfo.Tag != TiffTag.HALFTONEHINTS && tiffFieldInfo.Tag != TiffTag.YCBCRSUBSAMPLING && tiffFieldInfo.Tag != TiffTag.DOTRANGE)
						{
							array = new FieldValue[1];
							byte[] array2 = tiffTagValue.value;
							if (tiffFieldInfo.Type == TiffType.ASCII && tiffTagValue.value.Length > 0 && tiffTagValue.value[tiffTagValue.value.Length - 1] == 0)
							{
								array2 = new byte[Math.Max(tiffTagValue.value.Length - 1, 0)];
								Buffer.BlockCopy(tiffTagValue.value, 0, array2, 0, array2.Length);
							}
							array[0].Set(array2);
							break;
						}
						array = new FieldValue[tiffTagValue.count];
						byte[] value = tiffTagValue.value;
						int num = 0;
						int j = 0;
						while (j < tiffTagValue.count)
						{
							switch (tiffFieldInfo.Type)
							{
							case TiffType.BYTE:
							case TiffType.SBYTE:
							case TiffType.UNDEFINED:
								array[j].Set(value[num]);
								break;
							case TiffType.ASCII:
								goto IL_B25;
							case TiffType.SHORT:
							case TiffType.SSHORT:
								array[j].Set(BitConverter.ToInt16(value, num));
								break;
							case TiffType.LONG:
							case TiffType.SLONG:
							case TiffType.IFD:
								array[j].Set(BitConverter.ToInt32(value, num));
								break;
							case TiffType.RATIONAL:
							case TiffType.SRATIONAL:
							case TiffType.FLOAT:
								array[j].Set(BitConverter.ToSingle(value, num));
								break;
							case TiffType.DOUBLE:
								array[j].Set(BitConverter.ToDouble(value, num));
								break;
							default:
								goto IL_B25;
							}
							IL_B27:
							j++;
							num += Tiff.dataSize(tiffTagValue.info.Type);
							continue;
							IL_B25:
							array = null;
							goto IL_B27;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
			return array;
		}

		public virtual void PrintDir(Tiff tif, Stream stream, TiffPrintFlags flags)
		{
		}

		static bool setExtraSamples(TiffDirectory td, ref int v, FieldValue[] ap)
		{
			v = ap[0].ToInt();
			if (v > (int)td.td_samplesperpixel)
			{
				return false;
			}
			byte[] array = ap[1].ToByteArray();
			if (v > 0 && array == null)
			{
				return false;
			}
			for (int i = 0; i < v; i++)
			{
				if (array[i] > 2)
				{
					if (i >= v - 1)
					{
						return false;
					}
					short num = BitConverter.ToInt16(array, i);
					if (num == 999)
					{
						array[i] = 2;
					}
				}
			}
			td.td_extrasamples = (short)v;
			td.td_sampleinfo = new ExtraSample[(int)td.td_extrasamples];
			for (int j = 0; j < (int)td.td_extrasamples; j++)
			{
				td.td_sampleinfo[j] = (ExtraSample)array[j];
			}
			return true;
		}

		static int checkInkNamesString(Tiff tif, int slen, string s)
		{
			bool flag = false;
			short num = tif.m_dir.td_samplesperpixel;
			if (slen > 0)
			{
				int num2 = 0;
				while (num > 0)
				{
					while (s[num2] != '\0')
					{
						if (num2 >= slen)
						{
							flag = true;
							break;
						}
						num2++;
					}
					if (flag)
					{
						break;
					}
					num2++;
					num -= 1;
				}
				if (!flag)
				{
					return num2;
				}
			}
			Tiff.ErrorExt(tif, tif.m_clientdata, "TIFFSetField", "{0}: Invalid InkNames value; expecting {1} names, found {2}", new object[]
			{
				tif.m_name,
				tif.m_dir.td_samplesperpixel,
				(int)(tif.m_dir.td_samplesperpixel - num)
			});
			return 0;
		}

		static void setNString(out string cpp, string cp, int n)
		{
			cpp = cp.Substring(0, n);
		}

		const short DATATYPE_VOID = 0;

		const short DATATYPE_INT = 1;

		const short DATATYPE_UINT = 2;

		const short DATATYPE_IEEEFP = 3;
	}
}
