using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Model.Drawing.Shapes;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities
{
	static class RtfHelper
	{
		static RtfHelper()
		{
			RtfHelper.TableLookMapper = new ValueMapper<string, TableLooks>();
			RtfHelper.TableLookMapper.AddPair("tbllkhdrrows", TableLooks.FirstRow);
			RtfHelper.TableLookMapper.AddPair("tbllklastrow", TableLooks.LastRow);
			RtfHelper.TableLookMapper.AddPair("tbllkhdrcols", TableLooks.FirstColumn);
			RtfHelper.TableLookMapper.AddPair("tbllklastcol", TableLooks.LastColumn);
			RtfHelper.TableLookMapper.AddPair("tbllknorowband", TableLooks.BandedRows);
			RtfHelper.TableLookMapper.AddPair("tbllknocolband", TableLooks.BandedColumns);
			RtfHelper.BorderMapper = new ValueMapper<string, BorderStyle>("brdrs", BorderStyle.Single);
			RtfHelper.BorderMapper.AddPair("brdrnone", BorderStyle.None);
			RtfHelper.BorderMapper.AddPair("brdrsh", BorderStyle.Single);
			RtfHelper.BorderMapper.AddPair("brdrdb", BorderStyle.Double);
			RtfHelper.BorderMapper.AddPair("brdrdot", BorderStyle.Dotted);
			RtfHelper.BorderMapper.AddPair("brdrdash", BorderStyle.Dashed);
			RtfHelper.BorderMapper.AddPair("brdrhair", BorderStyle.Single);
			RtfHelper.BorderMapper.AddPair("brdrdashsm", BorderStyle.DashSmallGap);
			RtfHelper.BorderMapper.AddPair("brdrdashd", BorderStyle.DotDash);
			RtfHelper.BorderMapper.AddPair("brdrdashdd", BorderStyle.DotDotDash);
			RtfHelper.BorderMapper.AddPair("brdrdashdot", BorderStyle.DotDash);
			RtfHelper.BorderMapper.AddPair("brdrdashdotdot", BorderStyle.DotDotDash);
			RtfHelper.BorderMapper.AddPair("brdrinset", BorderStyle.Inset);
			RtfHelper.BorderMapper.AddPair("brdroutset", BorderStyle.Outset);
			RtfHelper.BorderMapper.AddPair("brdrtriple", BorderStyle.Triple);
			RtfHelper.BorderMapper.AddPair("brdrtnthsg", BorderStyle.ThickThinSmallGap);
			RtfHelper.BorderMapper.AddPair("brdrthtnsg", BorderStyle.ThinThickSmallGap);
			RtfHelper.BorderMapper.AddPair("brdrtnthtnsg", BorderStyle.ThinThickThinSmallGap);
			RtfHelper.BorderMapper.AddPair("brdrtnthmg", BorderStyle.ThickThinMediumGap);
			RtfHelper.BorderMapper.AddPair("brdrthtnmg", BorderStyle.ThinThickMediumGap);
			RtfHelper.BorderMapper.AddPair("brdrtnthtnmg", BorderStyle.ThinThickThinMediumGap);
			RtfHelper.BorderMapper.AddPair("brdrtnthlg", BorderStyle.ThickThinLargeGap);
			RtfHelper.BorderMapper.AddPair("brdrthtnlg", BorderStyle.ThinThickLargeGap);
			RtfHelper.BorderMapper.AddPair("brdrtnthtnlg", BorderStyle.ThinThickThinLargeGap);
			RtfHelper.BorderMapper.AddPair("brdrwavy", BorderStyle.Wave);
			RtfHelper.BorderMapper.AddPair("brdrwavydb", BorderStyle.DoubleWave);
			RtfHelper.BorderMapper.AddPair("brdrdashdotstr", BorderStyle.DashDotStroked);
			RtfHelper.BorderMapper.AddPair("brdremboss", BorderStyle.ThreeDEmboss);
			RtfHelper.BorderMapper.AddPair("brdrengrave", BorderStyle.ThreeDEngrave);
			RtfHelper.BorderMapper.AddPair("brdrnone", BorderStyle.Inherit);
			RtfHelper.BorderMapper.AddPair("brdrs", BorderStyle.Single);
			RtfHelper.SectionTypeMapper = new ValueMapper<string, SectionType>("sbkpage", SectionType.NextPage);
			RtfHelper.SectionTypeMapper.AddPair("sbkpage", SectionType.NextPage);
			RtfHelper.SectionTypeMapper.AddPair("sbkodd", SectionType.OddPage);
			RtfHelper.SectionTypeMapper.AddPair("sbkeven", SectionType.EvenPage);
			RtfHelper.SectionTypeMapper.AddPair("sbknone", SectionType.Continuous);
			RtfHelper.SectionTypeMapper.AddPair("sbkcol", SectionType.NextColumn);
			RtfHelper.DocumentViewTypeMapper = new ValueMapper<int, DocumentViewType>();
			RtfHelper.DocumentViewTypeMapper.AddPair(0, DocumentViewType.None);
			RtfHelper.DocumentViewTypeMapper.AddPair(1, DocumentViewType.PrintLayout);
			RtfHelper.DocumentViewTypeMapper.AddPair(2, DocumentViewType.Outline);
			RtfHelper.DocumentViewTypeMapper.AddPair(3, DocumentViewType.MasterPages);
			RtfHelper.DocumentViewTypeMapper.AddPair(4, DocumentViewType.Normal);
			RtfHelper.DocumentViewTypeMapper.AddPair(5, DocumentViewType.WebLayout);
			RtfHelper.WrappingStyleMapper = new ValueMapper<int, ShapeWrappingType>(2, ShapeWrappingType.Square);
			RtfHelper.WrappingStyleMapper.AddPair(1, ShapeWrappingType.TopAndBottom);
			RtfHelper.WrappingStyleMapper.AddPair(2, ShapeWrappingType.Square);
			RtfHelper.WrappingStyleMapper.AddPair(3, ShapeWrappingType.None);
			RtfHelper.TextWrapMapper = new ValueMapper<int, TextWrap>(0, TextWrap.BothSides);
			RtfHelper.TextWrapMapper.AddPair(0, TextWrap.BothSides);
			RtfHelper.TextWrapMapper.AddPair(1, TextWrap.LeftOnly);
			RtfHelper.TextWrapMapper.AddPair(2, TextWrap.RightOnly);
			RtfHelper.TextWrapMapper.AddPair(3, TextWrap.Largest);
			RtfHelper.VerticalRelativeFromMapper = new ValueMapper<int, VerticalRelativeFrom>(0, VerticalRelativeFrom.Margin);
			RtfHelper.VerticalRelativeFromMapper.AddPair(0, VerticalRelativeFrom.Margin);
			RtfHelper.VerticalRelativeFromMapper.AddPair(1, VerticalRelativeFrom.Page);
			RtfHelper.VerticalRelativeFromMapper.AddPair(2, VerticalRelativeFrom.Paragraph);
			RtfHelper.VerticalRelativeFromMapper.AddPair(3, VerticalRelativeFrom.Line);
			RtfHelper.VerticalRelativeFromMapper.AddPair(4, VerticalRelativeFrom.TopMargin);
			RtfHelper.VerticalRelativeFromMapper.AddPair(5, VerticalRelativeFrom.BottomMargin);
			RtfHelper.VerticalRelativeFromMapper.AddPair(6, VerticalRelativeFrom.InsideMargin);
			RtfHelper.VerticalRelativeFromMapper.AddPair(7, VerticalRelativeFrom.OutsideMargin);
			RtfHelper.HorizontalRelativeFromMapper = new ValueMapper<int, HorizontalRelativeFrom>(0, HorizontalRelativeFrom.Margin);
			RtfHelper.HorizontalRelativeFromMapper.AddPair(0, HorizontalRelativeFrom.Margin);
			RtfHelper.HorizontalRelativeFromMapper.AddPair(1, HorizontalRelativeFrom.Page);
			RtfHelper.HorizontalRelativeFromMapper.AddPair(2, HorizontalRelativeFrom.Column);
			RtfHelper.HorizontalRelativeFromMapper.AddPair(3, HorizontalRelativeFrom.Character);
			RtfHelper.HorizontalRelativeFromMapper.AddPair(4, HorizontalRelativeFrom.LeftMargin);
			RtfHelper.HorizontalRelativeFromMapper.AddPair(5, HorizontalRelativeFrom.RightMargin);
			RtfHelper.HorizontalRelativeFromMapper.AddPair(6, HorizontalRelativeFrom.InsideMargin);
			RtfHelper.HorizontalRelativeFromMapper.AddPair(7, HorizontalRelativeFrom.OutsideMargin);
			RtfHelper.RelativeVerticalAlignmentMapper = new ValueMapper<int, RelativeVerticalAlignment>(1, RelativeVerticalAlignment.Top);
			RtfHelper.RelativeVerticalAlignmentMapper.AddPair(1, RelativeVerticalAlignment.Top);
			RtfHelper.RelativeVerticalAlignmentMapper.AddPair(2, RelativeVerticalAlignment.Center);
			RtfHelper.RelativeVerticalAlignmentMapper.AddPair(3, RelativeVerticalAlignment.Bottom);
			RtfHelper.RelativeVerticalAlignmentMapper.AddPair(4, RelativeVerticalAlignment.Inside);
			RtfHelper.RelativeVerticalAlignmentMapper.AddPair(5, RelativeVerticalAlignment.Outside);
			RtfHelper.RelativeHorizontalAlignmentMapper = new ValueMapper<int, RelativeHorizontalAlignment>(1, RelativeHorizontalAlignment.Left);
			RtfHelper.RelativeHorizontalAlignmentMapper.AddPair(1, RelativeHorizontalAlignment.Left);
			RtfHelper.RelativeHorizontalAlignmentMapper.AddPair(2, RelativeHorizontalAlignment.Center);
			RtfHelper.RelativeHorizontalAlignmentMapper.AddPair(3, RelativeHorizontalAlignment.Right);
			RtfHelper.RelativeHorizontalAlignmentMapper.AddPair(4, RelativeHorizontalAlignment.Inside);
			RtfHelper.RelativeHorizontalAlignmentMapper.AddPair(5, RelativeHorizontalAlignment.Outside);
			RtfHelper.LineBreakTextWrappingMapper = new ValueMapper<int, TextWrappingRestartLocation>(0, TextWrappingRestartLocation.NextLine);
			RtfHelper.LineBreakTextWrappingMapper.AddPair(1, TextWrappingRestartLocation.NextTextRegionUnblockedOnLeft);
			RtfHelper.LineBreakTextWrappingMapper.AddPair(2, TextWrappingRestartLocation.NextTextRegionUnblockedOnRight);
			RtfHelper.LineBreakTextWrappingMapper.AddPair(3, TextWrappingRestartLocation.NextFullLine);
			RtfHelper.ListLevelAlignmentMapper = new ValueMapper<int, Alignment>(0, Alignment.Left);
			RtfHelper.ListLevelAlignmentMapper.AddPair(1, Alignment.Center);
			RtfHelper.ListLevelAlignmentMapper.AddPair(2, Alignment.Right);
			RtfHelper.NumberingStyleMapper = new ValueMapper<string, NumberingStyle>();
			RtfHelper.NumberingStyleMapper.AddPair("pgndec", NumberingStyle.Decimal);
			RtfHelper.NumberingStyleMapper.AddPair("pgnucrm", NumberingStyle.UpperRoman);
			RtfHelper.NumberingStyleMapper.AddPair("pgnlcrm", NumberingStyle.LowerRoman);
			RtfHelper.NumberingStyleMapper.AddPair("pgnucltr", NumberingStyle.UpperLetter);
			RtfHelper.NumberingStyleMapper.AddPair("pgnlcltr", NumberingStyle.LowerLetter);
			RtfHelper.NumberingStyleMapper.AddPair("pgnchosung", NumberingStyle.Chosung);
			RtfHelper.NumberingStyleMapper.AddPair("pgnganada", NumberingStyle.KoreanDigital2);
			RtfHelper.NumberingStyleMapper.AddPair("pgncnum", NumberingStyle.DecimalEnclosedCircle);
			RtfHelper.NumberingStyleMapper.AddPair("pgnbidia", NumberingStyle.ArabicAbjad);
			RtfHelper.NumberingStyleMapper.AddPair("pgnhindia", NumberingStyle.HindiVowels);
			RtfHelper.NumberingStyleMapper.AddPair("pgnhindib", NumberingStyle.HindiConsonants);
			RtfHelper.NumberingStyleMapper.AddPair("pgnhindic", NumberingStyle.HindiNumbers);
			RtfHelper.NumberingStyleMapper.AddPair("pgnhindid", NumberingStyle.HindiCounting);
			RtfHelper.NumberingStyleMapper.AddPair("pgnthaia", NumberingStyle.ThaiLetters);
			RtfHelper.NumberingStyleMapper.AddPair("pgnthaib", NumberingStyle.ThaiNumbers);
			RtfHelper.NumberingStyleMapper.AddPair("pgnthaic", NumberingStyle.ThaiCounting);
			RtfHelper.NumberingStyleMapper.AddPair("pgnvieta", NumberingStyle.VietnameseCounting);
			RtfHelper.ChapterSeparatorTypeMapper = new ValueMapper<string, ChapterSeparatorType>();
			RtfHelper.ChapterSeparatorTypeMapper.AddPair("pgnhnsc", ChapterSeparatorType.Colon);
			RtfHelper.ChapterSeparatorTypeMapper.AddPair("pgnhnsm", ChapterSeparatorType.EmDash);
			RtfHelper.ChapterSeparatorTypeMapper.AddPair("pgnhnsn", ChapterSeparatorType.EnDash);
			RtfHelper.ChapterSeparatorTypeMapper.AddPair("pgnhnsh", ChapterSeparatorType.Hyphen);
			RtfHelper.ChapterSeparatorTypeMapper.AddPair("pgnhnsp", ChapterSeparatorType.Period);
			RtfHelper.TabStopTypeMapper = new ValueMapper<string, TabStopType>();
			RtfHelper.TabStopTypeMapper.AddPair(string.Empty, TabStopType.Left);
			RtfHelper.TabStopTypeMapper.AddPair(string.Empty, TabStopType.Clear);
			RtfHelper.TabStopTypeMapper.AddPair("tqc", TabStopType.Center);
			RtfHelper.TabStopTypeMapper.AddPair("tqdec", TabStopType.Decimal);
			RtfHelper.TabStopTypeMapper.AddPair("tqr", TabStopType.Right);
			RtfHelper.TabStopLeaderMapper = new ValueMapper<string, TabStopLeader>();
			RtfHelper.TabStopLeaderMapper.AddPair(string.Empty, TabStopLeader.None);
			RtfHelper.TabStopLeaderMapper.AddPair("tldot", TabStopLeader.Dot);
			RtfHelper.TabStopLeaderMapper.AddPair("tlmdot", TabStopLeader.MiddleDot);
			RtfHelper.TabStopLeaderMapper.AddPair("tlhyph", TabStopLeader.Hyphen);
			RtfHelper.TabStopLeaderMapper.AddPair("tlul", TabStopLeader.Underscore);
			RtfHelper.TabStopLeaderMapper.AddPair("tlth", TabStopLeader.Underscore);
			RtfHelper.TableCellTextDirectionMapper = new ValueMapper<string, TextDirection>();
			RtfHelper.TableCellTextDirectionMapper.AddPair("cltxlrtb", TextDirection.LeftToRightTopToBottom);
			RtfHelper.TableCellTextDirectionMapper.AddPair("cltxbtlr", TextDirection.BottomToTopLeftToRight);
			RtfHelper.TableCellTextDirectionMapper.AddPair("cltxlrtbv", TextDirection.LeftToRightTopToBottomRotated);
			RtfHelper.TableCellTextDirectionMapper.AddPair("cltxtbrl", TextDirection.TopToBottomRightToLeft);
			RtfHelper.TableCellTextDirectionMapper.AddPair("cltxtbrlv", TextDirection.TopToBottomRightToLeftRotated);
		}

		public static string AlignmentToRtfTag(Alignment align, bool isRtl)
		{
			if (isRtl)
			{
				if (align == Alignment.Left)
				{
					align = Alignment.Right;
				}
				else if (align == Alignment.Right)
				{
					align = Alignment.Left;
				}
			}
			return RtfHelper.alignmentToRtfTag[align];
		}

		public static Alignment RtfTagToAlignment(RtfTag tag)
		{
			Alignment result = Alignment.Left;
			string name;
			switch (name = tag.Name)
			{
			case "ql":
				result = Alignment.Left;
				break;
			case "qc":
				result = Alignment.Center;
				break;
			case "qr":
				result = Alignment.Right;
				break;
			case "qj":
				result = Alignment.Justified;
				break;
			case "qd":
				result = Alignment.Distribute;
				break;
			case "qt":
				result = Alignment.ThaiDistribute;
				break;
			case "qk":
				if (tag.ValueAsNumber <= 0)
				{
					result = Alignment.LowKashida;
				}
				else if (tag.ValueAsNumber <= 10)
				{
					result = Alignment.MediumKashida;
				}
				else
				{
					result = Alignment.HighKashida;
				}
				break;
			}
			return result;
		}

		public static UnderlinePattern RtfTagToUndelineDecoration(string tag)
		{
			UnderlinePattern result = UnderlinePattern.Single;
			RtfHelper.rtfTagToUndelineDecoration.TryGetValue(tag, out result);
			return result;
		}

		public static string UndelineDecorationToRtfTag(UnderlinePattern decoration)
		{
			string result = "ul";
			RtfHelper.undelineDecorationToRtfTag.TryGetValue(decoration, out result);
			return result;
		}

		public static VerticalRelativeFrom RtfValueToVerticalRelativeFrom(int rtfValue, VerticalRelativeFrom defaultValue)
		{
			if (RtfHelper.VerticalRelativeFromMapper.ContainsFromValue(rtfValue))
			{
				return RtfHelper.VerticalRelativeFromMapper.GetToValue(rtfValue);
			}
			return defaultValue;
		}

		public static HorizontalRelativeFrom RtfValueToHorizontalRelativeFrom(int rtfValue, HorizontalRelativeFrom defaultValue)
		{
			if (RtfHelper.HorizontalRelativeFromMapper.ContainsFromValue(rtfValue))
			{
				return RtfHelper.HorizontalRelativeFromMapper.GetToValue(rtfValue);
			}
			return defaultValue;
		}

		public static string GetGroupText(RtfGroup group, bool recoursive = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			RtfHelper.GetGroupText(group, stringBuilder, recoursive);
			return stringBuilder.ToString();
		}

		public static byte[] GetBytesFromHexString(string hexString)
		{
			if (string.IsNullOrEmpty(hexString))
			{
				return new byte[0];
			}
			if (RtfHelper.hexCharValues == null)
			{
				RtfHelper.hexCharValues = new int[128];
				for (int i = 0; i < 10; i++)
				{
					RtfHelper.hexCharValues[48 + i] = i;
				}
				for (int j = 0; j < 6; j++)
				{
					RtfHelper.hexCharValues[97 + j] = 10 + j;
					RtfHelper.hexCharValues[65 + j] = 10 + j;
				}
			}
			byte[] array = new byte[hexString.Length >> 1];
			int k = 0;
			int length = hexString.Length;
			while (k < length)
			{
				array[k >> 1] = (byte)((RtfHelper.hexCharValues[(int)hexString[k]] << 4) | RtfHelper.hexCharValues[(int)hexString[k + 1]]);
				k += 2;
			}
			return array;
		}

		public static TableWidthUnit ConvertTableWidthUnitType(int unitType, int parameter)
		{
			TableWidthUnit result;
			if (unitType == 2)
			{
				result = new TableWidthUnit(TableWidthUnitType.Percent, (double)((float)parameter) / 50.0);
			}
			else if (unitType == 3)
			{
				result = new TableWidthUnit(TableWidthUnitType.Fixed, (double)Unit.TwipToDipF((double)parameter));
			}
			else
			{
				result = new TableWidthUnit(TableWidthUnitType.Auto);
			}
			return result;
		}

		public static double GetTableIndentFormRtf(int value, int type)
		{
			double result = 0.0;
			switch (type)
			{
			case 0:
			case 2:
				result = 0.0;
				break;
			case 1:
			case 3:
				result = Unit.TwipToDip((double)value);
				break;
			}
			return result;
		}

		public static Alignment GetTableHorzontalAlignmentFormRtf(string tag)
		{
			Alignment result = Alignment.Left;
			if (tag != null)
			{
				if (!(tag == "trqc"))
				{
					if (tag == "trqr")
					{
						result = Alignment.Right;
					}
				}
				else
				{
					result = Alignment.Center;
				}
			}
			return result;
		}

		public static string TableHorzontalAlignmentToRtfTag(Alignment align)
		{
			string result = "trql";
			switch (align)
			{
			case Alignment.Center:
				result = "trqc";
				break;
			case Alignment.Right:
				result = "trqr";
				break;
			}
			return result;
		}

		public static Size CalculateImageSizeForRotateAngle(Size orginalSize, double rotateAngle)
		{
			double num = Math.Abs(rotateAngle) % 180.0;
			if (num > 45.0 && num < 135.0)
			{
				return new Size(orginalSize.Height, orginalSize.Width);
			}
			return orginalSize;
		}

		public static int ConvertDateTimeToRtfInt(DateTime date)
		{
			if (date.Year < 1900)
			{
				return 0;
			}
			int num = date.Minute;
			num |= date.Hour << 6;
			num |= date.Day << 11;
			num |= date.Month << 16;
			return num | (date.Year - 1900 << 20);
		}

		public static DateTime ConvertRtfIntToDateTime(int numeric)
		{
			if (numeric == 0)
			{
				return DateTime.MinValue;
			}
			int minute = numeric & 63;
			int hour = (numeric & 1984) >> 6;
			int day = (numeric & 63488) >> 11;
			int month = (numeric & 983040) >> 16;
			int year = ((numeric & 535822336) >> 20) + 1900;
			DateTime result;
			try
			{
				result = new DateTime(year, month, day, hour, minute, 0, 0);
			}
			catch (ArgumentOutOfRangeException)
			{
				result = DateTime.MinValue;
			}
			return result;
		}

		public static string ConvertNumberedListFormatToRtfLevelText(string text)
		{
			for (int i = 0; i < 9; i++)
			{
				text = text.Replace("%" + (i + 1), "{" + i + "}");
			}
			return text;
		}

		internal static double RtfValueToAngle(int rtfAngle)
		{
			return (double)rtfAngle / RtfHelper.RtfAngleFactor;
		}

		internal static int AngleToRtfValue(double angle)
		{
			return (int)Math.Round(angle * RtfHelper.RtfAngleFactor);
		}

		internal static double RtfFixedPointSizeToDouble(int value)
		{
			return (double)value / RtfHelper.RtfAngleFactor;
		}

		internal static double DoubleToRtfFixedPointSize(double value)
		{
			return Math.Round(value * RtfHelper.RtfAngleFactor);
		}

		internal static Color IntToColor(int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			return Color.FromArgb(byte.MaxValue, bytes[0], bytes[1], bytes[2]);
		}

		internal static int ColorToInt(Color color)
		{
			return ((int)color.B << 16) | ((int)color.G << 8) | (int)color.R;
		}

		internal static ShadingPattern RtfTagToShadingPattern(RtfTag tag)
		{
			if (tag.Name == "chshdng" || tag.Name == "shading" || tag.Name == "clshdng" || tag.Name == "clshdngraw" || tag.Name == "tscellpct" || tag.Name == "trshdng")
			{
				double num = (double)tag.ValueAsNumber / 100.0;
				int num2 = 0;
				while (num2 < RtfHelper.ShadingPrecentToPattern.Count - 1 && RtfHelper.ShadingPrecentToPattern[num2].Item1 < num)
				{
					num2++;
				}
				return RtfHelper.ShadingPrecentToPattern[num2].Item2;
			}
			return ShadingPattern.Solid;
		}

		internal static int ShadingPatternToRtfTag(ShadingPattern pattern)
		{
			double num = 100.0;
			RtfHelper.PatternToShadingPrecent.TryGetValue(pattern, out num);
			return (int)(num * 100.0);
		}

		internal static int GetNestingLevel(DocumentElementBase element)
		{
			DocumentElementBase parent = element.Parent;
			int num = 0;
			while (parent != null)
			{
				num++;
				parent = parent.Parent;
			}
			return num;
		}

		internal static Size EnsureShapeSize(Size shapeSize)
		{
			if (shapeSize.IsEmpty)
			{
				return ShapeBase.DefaultSize;
			}
			return shapeSize;
		}

		internal static bool IsTransparentColor(Color color)
		{
			return color.A == 0;
		}

		static void GetGroupText(RtfGroup group, StringBuilder sb, bool recoursive)
		{
			foreach (RtfElement rtfElement in group.Elements)
			{
				RtfGroup rtfGroup = rtfElement as RtfGroup;
				if (recoursive && rtfGroup != null && !rtfGroup.IsExtensionDestination)
				{
					RtfHelper.GetGroupText(rtfGroup, sb, recoursive);
				}
				RtfText rtfText = rtfElement as RtfText;
				if (rtfText != null)
				{
					sb.Append(rtfText.Text);
				}
			}
		}

		public static readonly double DefaultLineSpacing = 240.0;

		public static readonly int DefaultFloatingBlockHorizontalMargin = 114305;

		public static readonly double RtfAngleFactor = 65536.0;

		public static readonly int ImageShapeType = 75;

		public static readonly int TextShapeType = 136;

		public static readonly ValueMapper<string, TableLooks> TableLookMapper;

		public static readonly ValueMapper<string, BorderStyle> BorderMapper;

		public static readonly ValueMapper<string, SectionType> SectionTypeMapper;

		public static readonly ValueMapper<int, DocumentViewType> DocumentViewTypeMapper;

		public static readonly ValueMapper<int, ShapeWrappingType> WrappingStyleMapper;

		public static readonly ValueMapper<int, TextWrap> TextWrapMapper;

		public static readonly ValueMapper<int, VerticalRelativeFrom> VerticalRelativeFromMapper;

		public static readonly ValueMapper<int, HorizontalRelativeFrom> HorizontalRelativeFromMapper;

		public static readonly ValueMapper<int, RelativeVerticalAlignment> RelativeVerticalAlignmentMapper;

		public static readonly ValueMapper<int, RelativeHorizontalAlignment> RelativeHorizontalAlignmentMapper;

		public static readonly ValueMapper<int, TextWrappingRestartLocation> LineBreakTextWrappingMapper;

		public static readonly ValueMapper<int, Alignment> ListLevelAlignmentMapper;

		public static readonly ValueMapper<string, NumberingStyle> NumberingStyleMapper;

		public static readonly ValueMapper<string, ChapterSeparatorType> ChapterSeparatorTypeMapper;

		public static readonly ValueMapper<string, TabStopType> TabStopTypeMapper;

		public static readonly ValueMapper<string, TabStopLeader> TabStopLeaderMapper;

		public static readonly ValueMapper<string, TextDirection> TableCellTextDirectionMapper;

		static readonly Dictionary<Alignment, string> alignmentToRtfTag = new Dictionary<Alignment, string>
		{
			{
				Alignment.Center,
				"qc"
			},
			{
				Alignment.Justified,
				"qj"
			},
			{
				Alignment.Left,
				"ql"
			},
			{
				Alignment.Right,
				"qr"
			},
			{
				Alignment.Distribute,
				"qd"
			},
			{
				Alignment.ThaiDistribute,
				"qt"
			},
			{
				Alignment.LowKashida,
				"qk"
			},
			{
				Alignment.MediumKashida,
				"qk"
			},
			{
				Alignment.HighKashida,
				"qk"
			}
		};

		static readonly Dictionary<UnderlinePattern, string> undelineDecorationToRtfTag = new Dictionary<UnderlinePattern, string>
		{
			{
				UnderlinePattern.None,
				"ulnone"
			},
			{
				UnderlinePattern.Single,
				"ul"
			},
			{
				UnderlinePattern.Double,
				"uldb"
			},
			{
				UnderlinePattern.Thick,
				"ulth"
			},
			{
				UnderlinePattern.Wave,
				"ulwave"
			},
			{
				UnderlinePattern.Dotted,
				"uld"
			},
			{
				UnderlinePattern.Dash,
				"uldash"
			},
			{
				UnderlinePattern.DotDash,
				"uldashd"
			},
			{
				UnderlinePattern.DotDotDash,
				"uldashdd"
			}
		};

		static readonly Dictionary<string, UnderlinePattern> rtfTagToUndelineDecoration = new Dictionary<string, UnderlinePattern>
		{
			{
				"ulnone",
				UnderlinePattern.None
			},
			{
				"ul",
				UnderlinePattern.Single
			},
			{
				"uld",
				UnderlinePattern.Dotted
			},
			{
				"ulthd",
				UnderlinePattern.DottedHeavy
			},
			{
				"uldash",
				UnderlinePattern.Dash
			},
			{
				"ulthdash",
				UnderlinePattern.DashedHeavy
			},
			{
				"ulldash",
				UnderlinePattern.DashLong
			},
			{
				"ulthldash",
				UnderlinePattern.DashLongHeavy
			},
			{
				"uldashd",
				UnderlinePattern.DotDash
			},
			{
				"ulthdashd",
				UnderlinePattern.DashDotHeavy
			},
			{
				"uldashdd",
				UnderlinePattern.DotDotDash
			},
			{
				"ulthdashdd",
				UnderlinePattern.DashDotDotHeavy
			},
			{
				"uldb",
				UnderlinePattern.Double
			},
			{
				"ulth",
				UnderlinePattern.Thick
			},
			{
				"ulw",
				UnderlinePattern.Words
			},
			{
				"ulwave",
				UnderlinePattern.Wave
			},
			{
				"ulhwave",
				UnderlinePattern.WavyHeavy
			},
			{
				"ululdbwave",
				UnderlinePattern.WavyDouble
			}
		};

		static readonly Dictionary<ShadingPattern, double> PatternToShadingPrecent = new Dictionary<ShadingPattern, double>
		{
			{
				ShadingPattern.Percent5,
				5.0
			},
			{
				ShadingPattern.Percent10,
				10.0
			},
			{
				ShadingPattern.Percent12,
				12.5
			},
			{
				ShadingPattern.Percent15,
				15.0
			},
			{
				ShadingPattern.Percent20,
				20.0
			},
			{
				ShadingPattern.Percent25,
				25.0
			},
			{
				ShadingPattern.Percent30,
				30.0
			},
			{
				ShadingPattern.Percent35,
				35.0
			},
			{
				ShadingPattern.Percent37,
				37.5
			},
			{
				ShadingPattern.Percent40,
				40.0
			},
			{
				ShadingPattern.Percent45,
				45.0
			},
			{
				ShadingPattern.Percent50,
				50.0
			},
			{
				ShadingPattern.Percent55,
				55.0
			},
			{
				ShadingPattern.Percent60,
				60.0
			},
			{
				ShadingPattern.Percent62,
				62.5
			},
			{
				ShadingPattern.Percent65,
				65.0
			},
			{
				ShadingPattern.Percent70,
				70.0
			},
			{
				ShadingPattern.Percent75,
				75.0
			},
			{
				ShadingPattern.Percent80,
				80.0
			},
			{
				ShadingPattern.Percent85,
				85.0
			},
			{
				ShadingPattern.Percent87,
				87.5
			},
			{
				ShadingPattern.Percent90,
				90.0
			},
			{
				ShadingPattern.Percent95,
				95.0
			}
		};

		static readonly List<Tuple<double, ShadingPattern>> ShadingPrecentToPattern = new List<Tuple<double, ShadingPattern>>
		{
			new Tuple<double, ShadingPattern>(0.0, ShadingPattern.Clear),
			new Tuple<double, ShadingPattern>(5.0, ShadingPattern.Percent5),
			new Tuple<double, ShadingPattern>(10.0, ShadingPattern.Percent10),
			new Tuple<double, ShadingPattern>(12.5, ShadingPattern.Percent12),
			new Tuple<double, ShadingPattern>(15.0, ShadingPattern.Percent15),
			new Tuple<double, ShadingPattern>(20.0, ShadingPattern.Percent20),
			new Tuple<double, ShadingPattern>(25.0, ShadingPattern.Percent25),
			new Tuple<double, ShadingPattern>(30.0, ShadingPattern.Percent30),
			new Tuple<double, ShadingPattern>(35.0, ShadingPattern.Percent35),
			new Tuple<double, ShadingPattern>(37.5, ShadingPattern.Percent37),
			new Tuple<double, ShadingPattern>(40.0, ShadingPattern.Percent40),
			new Tuple<double, ShadingPattern>(45.0, ShadingPattern.Percent45),
			new Tuple<double, ShadingPattern>(50.0, ShadingPattern.Percent50),
			new Tuple<double, ShadingPattern>(55.0, ShadingPattern.Percent55),
			new Tuple<double, ShadingPattern>(60.0, ShadingPattern.Percent60),
			new Tuple<double, ShadingPattern>(62.5, ShadingPattern.Percent62),
			new Tuple<double, ShadingPattern>(65.0, ShadingPattern.Percent65),
			new Tuple<double, ShadingPattern>(70.0, ShadingPattern.Percent70),
			new Tuple<double, ShadingPattern>(75.0, ShadingPattern.Percent75),
			new Tuple<double, ShadingPattern>(80.0, ShadingPattern.Percent80),
			new Tuple<double, ShadingPattern>(85.0, ShadingPattern.Percent85),
			new Tuple<double, ShadingPattern>(87.5, ShadingPattern.Percent87),
			new Tuple<double, ShadingPattern>(90.0, ShadingPattern.Percent90),
			new Tuple<double, ShadingPattern>(95.0, ShadingPattern.Percent95)
		};

		static int[] hexCharValues;
	}
}
