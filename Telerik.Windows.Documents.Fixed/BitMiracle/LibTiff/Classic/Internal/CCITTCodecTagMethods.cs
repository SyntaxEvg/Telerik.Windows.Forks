using System;
using System.IO;

namespace BitMiracle.LibTiff.Classic.Internal
{
	class CCITTCodecTagMethods : TiffTagMethods
	{
		public override bool SetField(Tiff tif, TiffTag tag, FieldValue[] ap)
		{
			CCITTCodec ccittcodec = tif.m_currentCodec as CCITTCodec;
			if (tag <= TiffTag.CONSECUTIVEBADFAXLINES)
			{
				switch (tag)
				{
				case TiffTag.GROUP3OPTIONS:
					if (tif.m_dir.td_compression == Compression.CCITTFAX3)
					{
						ccittcodec.m_groupoptions = (Group3Opt)ap[0].ToShort();
						goto IL_1A7;
					}
					goto IL_1A7;
				case TiffTag.GROUP4OPTIONS:
					if (tif.m_dir.td_compression == Compression.CCITTFAX4)
					{
						ccittcodec.m_groupoptions = (Group3Opt)ap[0].ToShort();
						goto IL_1A7;
					}
					goto IL_1A7;
				default:
					switch (tag)
					{
					case TiffTag.BADFAXLINES:
						ccittcodec.m_badfaxlines = ap[0].ToInt();
						goto IL_1A7;
					case TiffTag.CLEANFAXDATA:
						ccittcodec.m_cleanfaxdata = (CleanFaxData)ap[0].ToByte();
						goto IL_1A7;
					case TiffTag.CONSECUTIVEBADFAXLINES:
						ccittcodec.m_badfaxrun = ap[0].ToInt();
						goto IL_1A7;
					}
					break;
				}
			}
			else
			{
				switch (tag)
				{
				case TiffTag.FAXRECVPARAMS:
					ccittcodec.m_recvparams = ap[0].ToInt();
					goto IL_1A7;
				case TiffTag.FAXSUBADDRESS:
					Tiff.setString(out ccittcodec.m_subaddress, ap[0].ToString());
					goto IL_1A7;
				case TiffTag.FAXRECVTIME:
					ccittcodec.m_recvtime = ap[0].ToInt();
					goto IL_1A7;
				case TiffTag.FAXDCS:
					Tiff.setString(out ccittcodec.m_faxdcs, ap[0].ToString());
					goto IL_1A7;
				default:
					if (tag == TiffTag.FAXMODE)
					{
						ccittcodec.m_mode = (FaxMode)ap[0].ToShort();
						return true;
					}
					if (tag == TiffTag.FAXFILLFUNC)
					{
						ccittcodec.fill = ap[0].Value as Tiff.FaxFillFunc;
						return true;
					}
					break;
				}
			}
			return base.SetField(tif, tag, ap);
			IL_1A7:
			TiffFieldInfo tiffFieldInfo = tif.FieldWithTag(tag);
			if (tiffFieldInfo != null)
			{
				tif.setFieldBit((int)tiffFieldInfo.Bit);
				tif.m_flags |= TiffFlags.DIRTYDIRECT;
				return true;
			}
			return false;
		}

		public override FieldValue[] GetField(Tiff tif, TiffTag tag)
		{
			CCITTCodec ccittcodec = tif.m_currentCodec as CCITTCodec;
			FieldValue[] array = new FieldValue[1];
			if (tag <= TiffTag.CONSECUTIVEBADFAXLINES)
			{
				switch (tag)
				{
				case TiffTag.GROUP3OPTIONS:
				case TiffTag.GROUP4OPTIONS:
					array[0].Set(ccittcodec.m_groupoptions);
					return array;
				default:
					switch (tag)
					{
					case TiffTag.BADFAXLINES:
						array[0].Set(ccittcodec.m_badfaxlines);
						return array;
					case TiffTag.CLEANFAXDATA:
						array[0].Set(ccittcodec.m_cleanfaxdata);
						return array;
					case TiffTag.CONSECUTIVEBADFAXLINES:
						array[0].Set(ccittcodec.m_badfaxrun);
						return array;
					}
					break;
				}
			}
			else
			{
				switch (tag)
				{
				case TiffTag.FAXRECVPARAMS:
					array[0].Set(ccittcodec.m_recvparams);
					return array;
				case TiffTag.FAXSUBADDRESS:
					array[0].Set(ccittcodec.m_subaddress);
					return array;
				case TiffTag.FAXRECVTIME:
					array[0].Set(ccittcodec.m_recvtime);
					return array;
				case TiffTag.FAXDCS:
					array[0].Set(ccittcodec.m_faxdcs);
					return array;
				default:
					if (tag == TiffTag.FAXMODE)
					{
						array[0].Set(ccittcodec.m_mode);
						return array;
					}
					if (tag == TiffTag.FAXFILLFUNC)
					{
						array[0].Set(ccittcodec.fill);
						return array;
					}
					break;
				}
			}
			return base.GetField(tif, tag);
		}

		public override void PrintDir(Tiff tif, Stream fd, TiffPrintFlags flags)
		{
			CCITTCodec ccittcodec = tif.m_currentCodec as CCITTCodec;
			if (tif.fieldSet(73))
			{
				string text = " ";
				if (tif.m_dir.td_compression == Compression.CCITTFAX4)
				{
					Tiff.fprintf(fd, "  Group 4 Options:", new object[0]);
					if ((ccittcodec.m_groupoptions & Group3Opt.UNCOMPRESSED) != (Group3Opt)0)
					{
						Tiff.fprintf(fd, "{0}uncompressed data", new object[] { text });
					}
				}
				else
				{
					Tiff.fprintf(fd, "  Group 3 Options:", new object[0]);
					if ((ccittcodec.m_groupoptions & Group3Opt.ENCODING2D) != (Group3Opt)0)
					{
						Tiff.fprintf(fd, "{0}2-d encoding", new object[] { text });
						text = "+";
					}
					if ((ccittcodec.m_groupoptions & Group3Opt.FILLBITS) != (Group3Opt)0)
					{
						Tiff.fprintf(fd, "{0}EOL padding", new object[] { text });
						text = "+";
					}
					if ((ccittcodec.m_groupoptions & Group3Opt.UNCOMPRESSED) != (Group3Opt)0)
					{
						Tiff.fprintf(fd, "{0}uncompressed data", new object[] { text });
					}
				}
				Tiff.fprintf(fd, " ({0} = 0x{1:x})\n", new object[] { ccittcodec.m_groupoptions, ccittcodec.m_groupoptions });
			}
			if (tif.fieldSet(67))
			{
				Tiff.fprintf(fd, "  Fax Data:", new object[0]);
				switch (ccittcodec.m_cleanfaxdata)
				{
				case CleanFaxData.CLEAN:
					Tiff.fprintf(fd, " clean", new object[0]);
					break;
				case CleanFaxData.REGENERATED:
					Tiff.fprintf(fd, " receiver regenerated", new object[0]);
					break;
				case CleanFaxData.UNCLEAN:
					Tiff.fprintf(fd, " uncorrected errors", new object[0]);
					break;
				}
				Tiff.fprintf(fd, " ({0} = 0x{1:x})\n", new object[] { ccittcodec.m_cleanfaxdata, ccittcodec.m_cleanfaxdata });
			}
			if (tif.fieldSet(66))
			{
				Tiff.fprintf(fd, "  Bad Fax Lines: {0}\n", new object[] { ccittcodec.m_badfaxlines });
			}
			if (tif.fieldSet(68))
			{
				Tiff.fprintf(fd, "  Consecutive Bad Fax Lines: {0}\n", new object[] { ccittcodec.m_badfaxrun });
			}
			if (tif.fieldSet(69))
			{
				Tiff.fprintf(fd, "  Fax Receive Parameters: {0,8:x}\n", new object[] { ccittcodec.m_recvparams });
			}
			if (tif.fieldSet(70))
			{
				Tiff.fprintf(fd, "  Fax SubAddress: {0}\n", new object[] { ccittcodec.m_subaddress });
			}
			if (tif.fieldSet(71))
			{
				Tiff.fprintf(fd, "  Fax Receive Time: {0} secs\n", new object[] { ccittcodec.m_recvtime });
			}
			if (tif.fieldSet(72))
			{
				Tiff.fprintf(fd, "  Fax DCS: {0}\n", new object[] { ccittcodec.m_faxdcs });
			}
		}
	}
}
