using System;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Exceptions;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Styles;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Media;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.TagHandlers
{
	static class TableStyleHandlers
	{
		public static void InitializeTableStyleHandlers(Dictionary<string, ControlTagHandler> tagHandlers)
		{
			tagHandlers["trowd"] = new ControlTagHandler(TableStyleHandlers.ResetRowSettingsHandler);
			tagHandlers["tsrowd"] = new ControlTagHandler(TableStyleHandlers.ResetRowSettingsHandler);
			tagHandlers["tcelld"] = new ControlTagHandler(TableStyleHandlers.ResetCellSettingsHandler);
			tagHandlers["tblind"] = new ControlTagHandler(TableStyleHandlers.TableIndentHandler);
			tagHandlers["tblindtype"] = new ControlTagHandler(TableStyleHandlers.TableIndentTypeHandler);
			tagHandlers["trleft"] = new ControlTagHandler(TableStyleHandlers.TableRowPositionHandler);
			tagHandlers["trql"] = new ControlTagHandler(TableStyleHandlers.TableAlignemntHandler);
			tagHandlers["trqc"] = new ControlTagHandler(TableStyleHandlers.TableAlignemntHandler);
			tagHandlers["trqr"] = new ControlTagHandler(TableStyleHandlers.TableAlignemntHandler);
			tagHandlers["tbllkhdrrows"] = new ControlTagHandler(TableStyleHandlers.TableLooksHandler);
			tagHandlers["tbllklastrow"] = new ControlTagHandler(TableStyleHandlers.TableLooksHandler);
			tagHandlers["tbllkhdrcols"] = new ControlTagHandler(TableStyleHandlers.TableLooksHandler);
			tagHandlers["tbllklastcol"] = new ControlTagHandler(TableStyleHandlers.TableLooksHandler);
			tagHandlers["tbllknorowband"] = new ControlTagHandler(TableStyleHandlers.BandedTableLooksHandler);
			tagHandlers["tbllknocolband"] = new ControlTagHandler(TableStyleHandlers.BandedTableLooksHandler);
			tagHandlers["trcbpat"] = new ControlTagHandler(TableStyleHandlers.TableRowShadingBackgroundHandler);
			tagHandlers["trcfpat"] = new ControlTagHandler(TableStyleHandlers.TableRowShadingForegroundHandler);
			tagHandlers["trshdng"] = new ControlTagHandler(TableStyleHandlers.TableRowShadingPercentHandler);
			tagHandlers["trkeep"] = new ControlTagHandler(TableStyleHandlers.TableRowNoSplitHandler);
			tagHandlers["trhdr"] = new ControlTagHandler(TableStyleHandlers.TableRowRepeatOnEveryPageHandler);
			tagHandlers["tscbandsv"] = new ControlTagHandler(TableStyleHandlers.TagTableColumnBandingHandler);
			tagHandlers["tscbandsh"] = new ControlTagHandler(TableStyleHandlers.TagTableRowBandingHandler);
			tagHandlers["trrh"] = new ControlTagHandler(TableStyleHandlers.RowSettingsHandler);
			tagHandlers["trgaph"] = new ControlTagHandler(TableStyleHandlers.RowSettingsHandler);
			tagHandlers["trautofit"] = new ControlTagHandler(TableStyleHandlers.RowSettingsHandler);
			tagHandlers["trwWidth"] = new ControlTagHandler(TableStyleHandlers.RowSettingsHandler);
			tagHandlers["trftsWidth"] = new ControlTagHandler(TableStyleHandlers.RowSettingsHandler);
			tagHandlers["ltrrow"] = new ControlTagHandler(TableStyleHandlers.RowLtrHandler);
			tagHandlers["rtlrow"] = new ControlTagHandler(TableStyleHandlers.RowRtlHandler);
			tagHandlers["taprtl"] = new ControlTagHandler(TableStyleHandlers.TableRtlHandler);
			tagHandlers["trbrdrt"] = new ControlTagHandler(TableStyleHandlers.RowBordersHandler);
			tagHandlers["trbrdrl"] = new ControlTagHandler(TableStyleHandlers.RowBordersHandler);
			tagHandlers["trbrdrb"] = new ControlTagHandler(TableStyleHandlers.RowBordersHandler);
			tagHandlers["trbrdrr"] = new ControlTagHandler(TableStyleHandlers.RowBordersHandler);
			tagHandlers["trbrdrh"] = new ControlTagHandler(TableStyleHandlers.RowBordersHandler);
			tagHandlers["trbrdrv"] = new ControlTagHandler(TableStyleHandlers.RowBordersHandler);
			tagHandlers["clbrdrt"] = new ControlTagHandler(TableStyleHandlers.CellBordersHandler);
			tagHandlers["clbrdrl"] = new ControlTagHandler(TableStyleHandlers.CellBordersHandler);
			tagHandlers["clbrdrb"] = new ControlTagHandler(TableStyleHandlers.CellBordersHandler);
			tagHandlers["clbrdrr"] = new ControlTagHandler(TableStyleHandlers.CellBordersHandler);
			tagHandlers["cldglu"] = new ControlTagHandler(TableStyleHandlers.CellBordersHandler);
			tagHandlers["cldgll"] = new ControlTagHandler(TableStyleHandlers.CellBordersHandler);
			tagHandlers["clvertalt"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["tsvertalt"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["clvertalc"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["tsvertalc"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["clvertalb"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["tsvertalb"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["clcbpat"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["clcbpatraw"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["clcfpat"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["clcfpatraw"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["tscellcfpat"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["clshdng"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["clshdngraw"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["tscellpct"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["tscellcbpat"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["cellx"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["clwWidth"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["clftsWidth"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["cltxlrtb"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["cltxtbrl"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["cltxbtlr"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["cltxlrtbv"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["cltxtbrlv"] = new ControlTagHandler(TableStyleHandlers.CellSettingsHandler);
			tagHandlers["clmgf"] = new ControlTagHandler(TableStyleHandlers.CellMergeSettingsHandler);
			tagHandlers["clmrg"] = new ControlTagHandler(TableStyleHandlers.CellMergeSettingsHandler);
			tagHandlers["clvmgf"] = new ControlTagHandler(TableStyleHandlers.CellMergeSettingsHandler);
			tagHandlers["clvmrg"] = new ControlTagHandler(TableStyleHandlers.CellMergeSettingsHandler);
			tagHandlers["clNoWrap"] = new ControlTagHandler(TableStyleHandlers.CellNoWrapTextHandler);
			tagHandlers["clhidemark"] = new ControlTagHandler(TableStyleHandlers.CellIgnoreCellMarkerInRowHeightHandler);
			tagHandlers["clpadt"] = new ControlTagHandler(TableStyleHandlers.CellPaddingHandler);
			tagHandlers["clpadl"] = new ControlTagHandler(TableStyleHandlers.CellPaddingHandler);
			tagHandlers["clpadr"] = new ControlTagHandler(TableStyleHandlers.CellPaddingHandler);
			tagHandlers["clpadb"] = new ControlTagHandler(TableStyleHandlers.CellPaddingHandler);
			tagHandlers["clpadft"] = new ControlTagHandler(TableStyleHandlers.CellPaddingHandler);
			tagHandlers["clpadfl"] = new ControlTagHandler(TableStyleHandlers.CellPaddingHandler);
			tagHandlers["clpadfr"] = new ControlTagHandler(TableStyleHandlers.CellPaddingHandler);
			tagHandlers["clpadfb"] = new ControlTagHandler(TableStyleHandlers.CellPaddingHandler);
			tagHandlers["trpaddl"] = new ControlTagHandler(TableStyleHandlers.RowCellPaddingHandler);
			tagHandlers["trpaddt"] = new ControlTagHandler(TableStyleHandlers.RowCellPaddingHandler);
			tagHandlers["trpaddr"] = new ControlTagHandler(TableStyleHandlers.RowCellPaddingHandler);
			tagHandlers["trpaddb"] = new ControlTagHandler(TableStyleHandlers.RowCellPaddingHandler);
			tagHandlers["trpaddfl"] = new ControlTagHandler(TableStyleHandlers.RowCellPaddingHandler);
			tagHandlers["trpaddft"] = new ControlTagHandler(TableStyleHandlers.RowCellPaddingHandler);
			tagHandlers["trpaddfr"] = new ControlTagHandler(TableStyleHandlers.RowCellPaddingHandler);
			tagHandlers["trpaddfb"] = new ControlTagHandler(TableStyleHandlers.RowCellPaddingHandler);
			tagHandlers["trspdl"] = new ControlTagHandler(TableStyleHandlers.RowCellSpacingHandler);
			tagHandlers["trspdt"] = new ControlTagHandler(TableStyleHandlers.RowCellSpacingHandler);
			tagHandlers["trspdr"] = new ControlTagHandler(TableStyleHandlers.RowCellSpacingHandler);
			tagHandlers["trspdb"] = new ControlTagHandler(TableStyleHandlers.RowCellSpacingHandler);
			tagHandlers["trspdfl"] = new ControlTagHandler(TableStyleHandlers.RowCellSpacingHandler);
			tagHandlers["trspdft"] = new ControlTagHandler(TableStyleHandlers.RowCellSpacingHandler);
			tagHandlers["trspdfr"] = new ControlTagHandler(TableStyleHandlers.RowCellSpacingHandler);
			tagHandlers["trspdfb"] = new ControlTagHandler(TableStyleHandlers.RowCellSpacingHandler);
			BorderHandlers.InitializeBorderHandlers(tagHandlers);
		}

		static void ResetRowSettingsHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, new string[] { "trowd", "tsrowd" });
			context.CurrentStyle.ResetRowStyle();
		}

		static void ResetCellSettingsHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "tcelld");
			context.CurrentRowStyle.EnsureCurrentCellStyle();
			context.CurrentRowStyle.CurrentCellStyle.ResetDefaults();
		}

		static void RowBordersHandler(RtfTag tag, RtfImportContext context)
		{
			string name;
			if ((name = tag.Name) != null)
			{
				if (PImplD_E95A4CFF-6AEE-4974-AC65-107846A83AB8}.method0x6001e5c-1 == null)
				{
					PImplD_E95A4CFF-6AEE-4974-AC65-107846A83AB8}.method0x6001e5c-1 = new Dictionary<string, int>(6)
					{
						{ "trbrdrt", 0 },
						{ "trbrdrl", 1 },
						{ "trbrdrb", 2 },
						{ "trbrdrr", 3 },
						{ "trbrdrh", 4 },
						{ "trbrdrv", 5 }
					};
				}
				int num;
				if (PImplD_E95A4CFF-6AEE-4974-AC65-107846A83AB8}.method0x6001e5c-1.TryGetValue(name, out num))
				{
					switch (num)
					{
					case 0:
						context.CurrentStyle.CurrentBorder = context.CurrentRowStyle.Borders.Top;
						break;
					case 1:
						context.CurrentStyle.CurrentBorder = context.CurrentRowStyle.Borders.Left;
						break;
					case 2:
						context.CurrentStyle.CurrentBorder = context.CurrentRowStyle.Borders.Bottom;
						break;
					case 3:
						context.CurrentStyle.CurrentBorder = context.CurrentRowStyle.Borders.Right;
						break;
					case 4:
						context.CurrentStyle.CurrentBorder = context.CurrentRowStyle.Borders.InnerHorizontal;
						break;
					case 5:
						context.CurrentStyle.CurrentBorder = context.CurrentRowStyle.Borders.InnerVertical;
						break;
					default:
						goto IL_156;
					}
					context.CurrentStyle.CurrentBorder.HasValue = true;
					context.CurrentStyle.CurrentBorderedElementType = BorderedElementType.TableRow;
					return;
				}
			}
			IL_156:
			throw new RtfUnexpectedElementException("RowBorders control word", tag.Name);
		}

		static void CellBordersHandler(RtfTag tag, RtfImportContext context)
		{
			context.CurrentRowStyle.EnsureCurrentCellStyle();
			string name;
			if ((name = tag.Name) != null)
			{
				if (PImplD_E95A4CFF-6AEE-4974-AC65-107846A83AB8}.method0x6001e5d-1 == null)
				{
					PImplD_E95A4CFF-6AEE-4974-AC65-107846A83AB8}.method0x6001e5d-1 = new Dictionary<string, int>(6)
					{
						{ "clbrdrt", 0 },
						{ "clbrdrl", 1 },
						{ "clbrdrb", 2 },
						{ "clbrdrr", 3 },
						{ "cldgll", 4 },
						{ "cldglu", 5 }
					};
				}
				int num;
				if (PImplD_E95A4CFF-6AEE-4974-AC65-107846A83AB8}.method0x6001e5d-1.TryGetValue(name, out num))
				{
					switch (num)
					{
					case 0:
						context.CurrentStyle.CurrentBorder = context.CurrentRowStyle.CurrentCellStyle.Borders.Top;
						break;
					case 1:
						context.CurrentStyle.CurrentBorder = context.CurrentRowStyle.CurrentCellStyle.Borders.Left;
						break;
					case 2:
						context.CurrentStyle.CurrentBorder = context.CurrentRowStyle.CurrentCellStyle.Borders.Bottom;
						break;
					case 3:
						context.CurrentStyle.CurrentBorder = context.CurrentRowStyle.CurrentCellStyle.Borders.Right;
						break;
					case 4:
						context.CurrentStyle.CurrentBorder = context.CurrentRowStyle.CurrentCellStyle.Borders.DiagonalLowerLeft;
						break;
					case 5:
						context.CurrentStyle.CurrentBorder = context.CurrentRowStyle.CurrentCellStyle.Borders.DiagonalUpperLeft;
						break;
					default:
						goto IL_17F;
					}
					context.CurrentStyle.CurrentBorder.HasValue = true;
					context.CurrentStyle.CurrentBorderedElementType = BorderedElementType.TableCell;
					return;
				}
			}
			IL_17F:
			throw new RtfUnexpectedElementException("CellBorders control word", tag.Name);
		}

		static void CellSettingsHandler(RtfTag tag, RtfImportContext context)
		{
			context.CurrentRowStyle.EnsureCurrentCellStyle();
			string name;
			if ((name = tag.Name) != null)
			{
				if (PImplD_E95A4CFF-6AEE-4974-AC65-107846A83AB8}.method0x6001e5e-1 == null)
				{
					PImplD_E95A4CFF-6AEE-4974-AC65-107846A83AB8}.method0x6001e5e-1 = new Dictionary<string, int>(23)
					{
						{ "clvertalt", 0 },
						{ "tsvertalt", 1 },
						{ "clvertalc", 2 },
						{ "tsvertalc", 3 },
						{ "clvertalb", 4 },
						{ "tsvertalb", 5 },
						{ "clcbpat", 6 },
						{ "clcbpatraw", 7 },
						{ "tscellcbpat", 8 },
						{ "clcfpat", 9 },
						{ "clcfpatraw", 10 },
						{ "tscellcfpat", 11 },
						{ "clshdng", 12 },
						{ "clshdngraw", 13 },
						{ "tscellpct", 14 },
						{ "cellx", 15 },
						{ "clwWidth", 16 },
						{ "clftsWidth", 17 },
						{ "cltxlrtb", 18 },
						{ "cltxtbrl", 19 },
						{ "cltxbtlr", 20 },
						{ "cltxlrtbv", 21 },
						{ "cltxtbrlv", 22 }
					};
				}
				int num;
				if (PImplD_E95A4CFF-6AEE-4974-AC65-107846A83AB8}.method0x6001e5e-1.TryGetValue(name, out num))
				{
					switch (num)
					{
					case 0:
					case 1:
						context.CurrentRowStyle.CurrentCellStyle.VerticalAlignment = VerticalAlignment.Top;
						return;
					case 2:
					case 3:
						context.CurrentRowStyle.CurrentCellStyle.VerticalAlignment = VerticalAlignment.Center;
						return;
					case 4:
					case 5:
						context.CurrentRowStyle.CurrentCellStyle.VerticalAlignment = VerticalAlignment.Bottom;
						return;
					case 6:
					case 7:
					case 8:
					{
						RtfColor rtfColor;
						if (context.ColorTable.TryGetColor(tag.ValueAsNumber, out rtfColor))
						{
							context.CurrentRowStyle.CurrentCellStyle.BackgroundColor = new Color?(rtfColor.Color);
							return;
						}
						break;
					}
					case 9:
					case 10:
					case 11:
					{
						RtfColor rtfColor;
						if (context.ColorTable.TryGetColor(tag.ValueAsNumber, out rtfColor))
						{
							context.CurrentRowStyle.CurrentCellStyle.PatternColor = new Color?(rtfColor.Color);
							return;
						}
						break;
					}
					case 12:
					case 13:
					case 14:
						context.CurrentRowStyle.CurrentCellStyle.Pattern = new ShadingPattern?(RtfHelper.RtfTagToShadingPattern(tag));
						return;
					case 15:
						context.CurrentRowStyle.CurrentCellStyle.CellXBoundry = tag.ValueAsNumber;
						context.CurrentRowStyle.CurrentCellStyle.IsLocked = true;
						return;
					case 16:
						context.CurrentRowStyle.CurrentCellStyle.PreferredWidthValue = tag.ValueAsNumber;
						return;
					case 17:
						context.CurrentRowStyle.CurrentCellStyle.PreferredWidthUnitType = tag.ValueAsNumber;
						return;
					case 18:
					case 19:
					case 20:
					case 21:
					case 22:
					{
						TextDirection textDirection;
						if (RtfHelper.TableCellTextDirectionMapper.TryGetToValue(tag.Name, out textDirection))
						{
							context.CurrentRowStyle.CurrentCellStyle.TextDirection = textDirection;
							return;
						}
						break;
					}
					default:
						goto IL_305;
					}
					return;
				}
			}
			IL_305:
			throw new RtfUnexpectedElementException("CellSettings control word", tag.Name);
		}

		static void CellMergeSettingsHandler(RtfTag tag, RtfImportContext context)
		{
			context.CurrentRowStyle.EnsureCurrentCellStyle();
			string name;
			if ((name = tag.Name) != null)
			{
				if (name == "clmgf")
				{
					context.CurrentRowStyle.CurrentCellStyle.IsFirstInHorizontalRange = true;
					return;
				}
				if (name == "clmrg")
				{
					context.CurrentRowStyle.CurrentCellStyle.IsInHorizontalRange = true;
					return;
				}
				if (name == "clvmgf")
				{
					context.CurrentRowStyle.CurrentCellStyle.IsFirstInVerticalRange = true;
					return;
				}
				if (name == "clvmrg")
				{
					context.CurrentRowStyle.CurrentCellStyle.IsInVerticalRange = true;
					return;
				}
			}
			throw new RtfUnexpectedElementException("CellMergeSettings control word", tag.Name);
		}

		static void CellPaddingHandler(RtfTag tag, RtfImportContext context)
		{
			context.CurrentRowStyle.EnsureCurrentCellStyle();
			string name;
			switch (name = tag.Name)
			{
			case "clpadt":
				context.CurrentRowStyle.CurrentCellStyle.Padding.Left.Value = tag.ValueAsNumber;
				return;
			case "clpadl":
				context.CurrentRowStyle.CurrentCellStyle.Padding.Top.Value = tag.ValueAsNumber;
				return;
			case "clpadr":
				context.CurrentRowStyle.CurrentCellStyle.Padding.Right.Value = tag.ValueAsNumber;
				return;
			case "clpadb":
				context.CurrentRowStyle.CurrentCellStyle.Padding.Bottom.Value = tag.ValueAsNumber;
				return;
			case "clpadft":
				context.CurrentRowStyle.CurrentCellStyle.Padding.Left.UnitType = tag.ValueAsNumber;
				return;
			case "clpadfl":
				context.CurrentRowStyle.CurrentCellStyle.Padding.Top.UnitType = tag.ValueAsNumber;
				return;
			case "clpadfr":
				context.CurrentRowStyle.CurrentCellStyle.Padding.Right.UnitType = tag.ValueAsNumber;
				return;
			case "clpadfb":
				context.CurrentRowStyle.CurrentCellStyle.Padding.Bottom.UnitType = tag.ValueAsNumber;
				return;
			}
			throw new RtfUnexpectedElementException("cell padding control word", tag.Name);
		}

		static void CellNoWrapTextHandler(RtfTag tag, RtfImportContext context)
		{
			context.CurrentRowStyle.EnsureCurrentCellStyle();
			context.CurrentRowStyle.CurrentCellStyle.CanWrapContent = false;
		}

		static void CellIgnoreCellMarkerInRowHeightHandler(RtfTag tag, RtfImportContext context)
		{
			context.CurrentRowStyle.EnsureCurrentCellStyle();
			context.CurrentRowStyle.CurrentCellStyle.IgnoreCellMarkerInRowHeight = true;
		}

		static void RowCellSpacingHandler(RtfTag tag, RtfImportContext context)
		{
			context.CurrentRowStyle.EnsureCurrentCellStyle();
			string name;
			switch (name = tag.Name)
			{
			case "trspdl":
				context.CurrentRowStyle.DefaultCellSpacing.Left.RtfValue = tag.ValueAsNumber;
				return;
			case "trspdt":
				context.CurrentRowStyle.DefaultCellSpacing.Top.RtfValue = tag.ValueAsNumber;
				return;
			case "trspdr":
				context.CurrentRowStyle.DefaultCellSpacing.Right.RtfValue = tag.ValueAsNumber;
				return;
			case "trspdb":
				context.CurrentRowStyle.DefaultCellSpacing.Bottom.RtfValue = tag.ValueAsNumber;
				return;
			case "trspdfl":
				context.CurrentRowStyle.DefaultCellSpacing.Left.UnitType = tag.ValueAsNumber;
				return;
			case "trspdft":
				context.CurrentRowStyle.DefaultCellSpacing.Top.UnitType = tag.ValueAsNumber;
				return;
			case "trspdfr":
				context.CurrentRowStyle.DefaultCellSpacing.Right.UnitType = tag.ValueAsNumber;
				return;
			case "trspdfb":
				context.CurrentRowStyle.DefaultCellSpacing.Bottom.UnitType = tag.ValueAsNumber;
				return;
			}
			throw new RtfUnexpectedElementException("cell spacing control word", tag.Name);
		}

		static void RowCellPaddingHandler(RtfTag tag, RtfImportContext context)
		{
			context.CurrentRowStyle.EnsureCurrentCellStyle();
			string name;
			switch (name = tag.Name)
			{
			case "trpaddl":
				context.CurrentRowStyle.CellPadding.Left.Value = tag.ValueAsNumber;
				return;
			case "trpaddt":
				context.CurrentRowStyle.CellPadding.Top.Value = tag.ValueAsNumber;
				return;
			case "trpaddr":
				context.CurrentRowStyle.CellPadding.Right.Value = tag.ValueAsNumber;
				return;
			case "trpaddb":
				context.CurrentRowStyle.CellPadding.Bottom.Value = tag.ValueAsNumber;
				return;
			case "trpaddfl":
				context.CurrentRowStyle.CellPadding.Left.UnitType = tag.ValueAsNumber;
				return;
			case "trpaddft":
				context.CurrentRowStyle.CellPadding.Top.UnitType = tag.ValueAsNumber;
				return;
			case "trpaddfr":
				context.CurrentRowStyle.CellPadding.Right.UnitType = tag.ValueAsNumber;
				return;
			case "trpaddfb":
				context.CurrentRowStyle.CellPadding.Bottom.UnitType = tag.ValueAsNumber;
				return;
			}
			throw new RtfUnexpectedElementException("cell spacing control word", tag.Name);
		}

		static void RowSettingsHandler(RtfTag tag, RtfImportContext context)
		{
			string name;
			if ((name = tag.Name) != null)
			{
				if (!(name == "trrh"))
				{
					if (name == "trgaph")
					{
						context.CurrentRowStyle.DefaultCellPadding = Unit.TwipToDip((double)tag.ValueAsNumber);
						return;
					}
					if (name == "trwWidth")
					{
						context.CurrentRowStyle.PreferredWidthValue = tag.ValueAsNumber;
						return;
					}
					if (name == "trftsWidth")
					{
						context.CurrentRowStyle.PreferredWidthUnitType = tag.ValueAsNumber;
						return;
					}
					if (name == "trautofit")
					{
						context.CurrentRowStyle.AutoFit = ((tag.ValueAsNumber == 1) ? TableLayoutType.AutoFit : TableLayoutType.FixedWidth);
						return;
					}
				}
				else
				{
					if (tag.HasValue && tag.ValueAsNumber != 0)
					{
						HeightType type = ((tag.ValueAsNumber > 0) ? HeightType.AtLeast : HeightType.Exact);
						double value = Unit.TwipToDip((double)Math.Abs(tag.ValueAsNumber));
						context.CurrentRowStyle.RowHeight = new TableRowHeight(type, value);
						return;
					}
					return;
				}
			}
			throw new RtfUnexpectedElementException("row settings control word", tag.Name);
		}

		static void RowLtrHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "ltrrow");
			context.CurrentRowStyle.FlowDirection = FlowDirection.LeftToRight;
		}

		static void RowRtlHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "rtlrow");
			context.CurrentRowStyle.FlowDirection = FlowDirection.RightToLeft;
		}

		static void TableRtlHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "taprtl");
			context.CurrentRowStyle.FlowDirection = FlowDirection.RightToLeft;
		}

		static void TableIndentHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "tblind");
			context.CurrentRowStyle.TableIndentValue = tag.ValueAsNumber;
		}

		static void TableIndentTypeHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "tblindtype");
			context.CurrentRowStyle.TableIndentType = tag.ValueAsNumber;
		}

		static void TagTableColumnBandingHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "tscbandsv");
			context.CurrentRowStyle.ColumnBanding = new int?(tag.ValueAsNumber);
		}

		static void TagTableRowBandingHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "tscbandsh");
			context.CurrentRowStyle.RowBanding = new int?(tag.ValueAsNumber);
		}

		static void TableRowPositionHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, "trleft");
			context.CurrentRowStyle.TableRowPosition = tag.ValueAsNumber;
		}

		static void TableAlignemntHandler(RtfTag tag, RtfImportContext context)
		{
			Util.EnsureTagName(tag, new string[] { "trql", "trqc", "trqr" });
			context.CurrentRowStyle.TableHorizontalAlignment = RtfHelper.GetTableHorzontalAlignmentFormRtf(tag.Name);
		}

		static void TableLooksHandler(RtfTag tag, RtfImportContext context)
		{
			context.CurrentRowStyle.TableLooks |= RtfHelper.TableLookMapper.GetToValue(tag.Name);
		}

		static void BandedTableLooksHandler(RtfTag tag, RtfImportContext context)
		{
			context.CurrentRowStyle.TableLooks &= ~RtfHelper.TableLookMapper.GetToValue(tag.Name);
		}

		static void TableRowShadingBackgroundHandler(RtfTag tag, RtfImportContext context)
		{
			RtfColor rtfColor;
			if (context.ColorTable.TryGetColor(tag.ValueAsNumber, out rtfColor))
			{
				context.CurrentRowStyle.BackgroundColor = new Color?(rtfColor.Color);
			}
		}

		static void TableRowShadingForegroundHandler(RtfTag tag, RtfImportContext context)
		{
			RtfColor rtfColor;
			if (context.ColorTable.TryGetColor(tag.ValueAsNumber, out rtfColor))
			{
				context.CurrentRowStyle.PatternColor = new Color?(rtfColor.Color);
			}
		}

		static void TableRowShadingPercentHandler(RtfTag tag, RtfImportContext context)
		{
			context.CurrentRowStyle.Pattern = new ShadingPattern?(RtfHelper.RtfTagToShadingPattern(tag));
		}

		static void TableRowNoSplitHandler(RtfTag tag, RtfImportContext context)
		{
			context.CurrentRowStyle.CanSplit = false;
		}

		static void TableRowRepeatOnEveryPageHandler(RtfTag tag, RtfImportContext context)
		{
			context.CurrentRowStyle.RepeatOnEveryPage = true;
		}
	}
}
