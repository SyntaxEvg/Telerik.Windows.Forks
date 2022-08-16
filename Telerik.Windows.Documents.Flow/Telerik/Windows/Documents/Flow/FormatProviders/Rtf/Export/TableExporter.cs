using System;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Common.Tables;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.BorderEvaluation;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Export
{
	class TableExporter
	{
		public TableExporter(RtfDocumentExporter exporter, Table table)
		{
			this.exporter = exporter;
			this.writer = exporter.Writer;
			this.table = table;
			this.isNestedTable = !(table.Parent is Section);
			if (!this.table.HasCellSpacing)
			{
				this.borderGrid = new TableBorderGrid(this.table);
				this.borderGrid.AssureValid();
			}
		}

		public static void ExportTableStyle(Style style, ExportContext context, RtfWriter rtfWriter)
		{
			BorderExporter.ExportTableBorders(style.TableProperties.Borders.GetActualValue(), context, rtfWriter);
			double? actualValue = style.TableProperties.Indent.GetActualValue();
			if (actualValue != null)
			{
				TableExporter.ExportTableIndent(actualValue.Value, rtfWriter);
			}
			Alignment? actualValue2 = style.TableProperties.Alignment.GetActualValue();
			if (actualValue2 != null)
			{
				FlowDirection? actualValue3 = style.TableProperties.FlowDirection.GetActualValue();
				FlowDirection flowDirection = ((actualValue3 != null) ? actualValue3.Value : FlowDirection.LeftToRight);
				TableExporter.ExportTableHorizontalAlignment(actualValue2.Value, flowDirection, rtfWriter);
			}
			int? actualValue4 = style.TableProperties.RowBanding.GetActualValue();
			if (actualValue4 != null)
			{
				rtfWriter.WriteTag("tscbandsh", actualValue4.Value);
			}
			int? actualValue5 = style.TableProperties.ColumnBanding.GetActualValue();
			if (actualValue5 != null)
			{
				rtfWriter.WriteTag("tscbandsv", actualValue5.Value);
			}
			double? actualValue6 = style.TableProperties.TableCellSpacing.GetActualValue();
			if (actualValue6 != null)
			{
				TableExporter.ExportCellSpacing(actualValue6.Value, rtfWriter);
			}
			Padding actualValue7 = style.TableProperties.TableCellPadding.GetActualValue();
			if (actualValue7 != null)
			{
				TableExporter.ExportCellPaddingForTable(actualValue7, rtfWriter);
			}
			TableExporter.ExportTableShading(style.TableProperties, context, rtfWriter);
			VerticalAlignment? actualValue8 = style.TableCellProperties.VerticalAlignment.GetActualValue();
			if (actualValue8 != null)
			{
				TableExporter.ExportCellVerticalAlignment(actualValue8.Value, rtfWriter, true);
			}
			BorderExporter.ExportCellBordersFromStyle(style.TableCellProperties.Borders.GetActualValue(), context, rtfWriter);
			Padding actualValue9 = style.TableCellProperties.Padding.GetActualValue();
			if (actualValue9 != null)
			{
				TableExporter.ExportCellPaddingForTableCell(actualValue9, rtfWriter);
			}
			TableExporter.ExportCellShading(style.TableCellProperties, context, rtfWriter, true);
		}

		public static void ExportCellVerticalAlignment(VerticalAlignment align, RtfWriter rtfWriter, bool isInStyle)
		{
			switch (align)
			{
			case VerticalAlignment.Top:
				rtfWriter.WriteTag(isInStyle ? "tsvertalt" : "clvertalt");
				return;
			case VerticalAlignment.Bottom:
				rtfWriter.WriteTag(isInStyle ? "tsvertalb" : "clvertalb");
				return;
			case VerticalAlignment.Center:
			case VerticalAlignment.Justified:
				rtfWriter.WriteTag(isInStyle ? "tsvertalc" : "clvertalc");
				return;
			default:
				return;
			}
		}

		public static void ExportPreferredWidth(TableCell cell, RtfWriter rtfWriter)
		{
			if (cell.PreferredWidth != null)
			{
				switch (cell.PreferredWidth.Type)
				{
				case TableWidthUnitType.Fixed:
					rtfWriter.WriteTag("clftsWidth", 3);
					rtfWriter.WriteTag("clwWidth", Unit.DipToTwipI(cell.PreferredWidth.Value));
					return;
				case TableWidthUnitType.Percent:
					rtfWriter.WriteTag("clftsWidth", 2);
					rtfWriter.WriteTag("clwWidth", (int)(cell.PreferredWidth.Value * 50.0));
					break;
				default:
					return;
				}
			}
		}

		public static void ExportCellPaddingForTableCell(Padding cellPadding, RtfWriter rtfWriter)
		{
			rtfWriter.WriteTag("clpadft", 3);
			rtfWriter.WriteTag("clpadt", Unit.DipToTwipI(cellPadding.Left));
			rtfWriter.WriteTag("clpadfr", 3);
			rtfWriter.WriteTag("clpadr", Unit.DipToTwipI(cellPadding.Right));
			rtfWriter.WriteTag("clpadfl", 3);
			rtfWriter.WriteTag("clpadl", Unit.DipToTwipI(cellPadding.Top));
			rtfWriter.WriteTag("clpadfb", 3);
			rtfWriter.WriteTag("clpadb", Unit.DipToTwipI(cellPadding.Bottom));
		}

		public static void ExportCellPaddingForTable(Padding cellPadding, RtfWriter rtfWriter)
		{
			rtfWriter.WriteTag("trpaddfl", 3);
			rtfWriter.WriteTag("trpaddl", Unit.DipToTwipI(cellPadding.Left));
			rtfWriter.WriteTag("trpaddfr", 3);
			rtfWriter.WriteTag("trpaddr", Unit.DipToTwipI(cellPadding.Right));
			rtfWriter.WriteTag("trpaddft", 3);
			rtfWriter.WriteTag("trpaddt", Unit.DipToTwipI(cellPadding.Top));
			rtfWriter.WriteTag("trpaddfb", 3);
			rtfWriter.WriteTag("trpaddb", Unit.DipToTwipI(cellPadding.Bottom));
		}

		public static void ExportCellShading(TableCellProperties cellProps, ExportContext context, RtfWriter writer, bool isInStyle)
		{
			DocumentTheme theme = cellProps.Document.Theme;
			Color actualValue = cellProps.BackgroundColor.GetActualValue().GetActualValue(theme);
			Color actualValue2 = cellProps.ShadingPatternColor.GetActualValue().GetActualValue(theme);
			if (!RtfHelper.IsTransparentColor(actualValue) || !RtfHelper.IsTransparentColor(actualValue2))
			{
				if (!RtfHelper.IsTransparentColor(actualValue))
				{
					string tagName = (isInStyle ? "tscellcbpat" : "clcbpat");
					writer.WriteTag(tagName, context.ColorTable[actualValue]);
				}
				if (!RtfHelper.IsTransparentColor(actualValue2))
				{
					string tagName2 = (isInStyle ? "tscellcfpat" : "clcfpat");
					writer.WriteTag(tagName2, context.ColorTable[actualValue2]);
				}
				string tagName3 = (isInStyle ? "tscellpct" : "clshdng");
				writer.WriteTag(tagName3, RtfHelper.ShadingPatternToRtfTag(cellProps.ShadingPattern.GetActualValue().Value));
			}
		}

		public static void ExportTableShading(TableProperties tableProps, ExportContext context, RtfWriter writer)
		{
			DocumentTheme theme = tableProps.Document.Theme;
			Color actualValue = tableProps.BackgroundColor.GetActualValue().GetActualValue(theme);
			Color actualValue2 = tableProps.ShadingPatternColor.GetActualValue().GetActualValue(theme);
			if (!RtfHelper.IsTransparentColor(actualValue) || !RtfHelper.IsTransparentColor(actualValue2))
			{
				if (!RtfHelper.IsTransparentColor(actualValue))
				{
					writer.WriteTag("trcbpat", context.ColorTable[actualValue]);
				}
				if (!RtfHelper.IsTransparentColor(actualValue2))
				{
					writer.WriteTag("trcfpat", context.ColorTable[actualValue2]);
				}
				writer.WriteTag("trshdng", RtfHelper.ShadingPatternToRtfTag(tableProps.ShadingPattern.GetActualValue().Value));
			}
		}

		public static void ExportTableIndent(double tableIndent, RtfWriter rtfWriter)
		{
			if (tableIndent != 0.0)
			{
				rtfWriter.WriteTag("tblind", Unit.DipToTwipI(tableIndent));
				rtfWriter.WriteTag("tblindtype", 3);
			}
		}

		public static void ExportTableHorizontalAlignment(Alignment alignment, FlowDirection flowDirection, RtfWriter rtfWriter)
		{
			Alignment alignment2 = alignment;
			if (flowDirection == FlowDirection.RightToLeft)
			{
				if (alignment == Alignment.Left)
				{
					alignment2 = Alignment.Right;
				}
				else if (alignment == Alignment.Right)
				{
					alignment2 = Alignment.Left;
				}
			}
			if (alignment2 != Alignment.Left || flowDirection == FlowDirection.RightToLeft)
			{
				rtfWriter.WriteTag(RtfHelper.TableHorzontalAlignmentToRtfTag(alignment2));
			}
		}

		public static void ExportCellSpacing(double cellSpacing, RtfWriter rtfwriter)
		{
			rtfwriter.WriteTag("trgaph", Unit.DipToTwipI(cellSpacing));
			if (cellSpacing > 0.0)
			{
				rtfwriter.WriteTag("trspdl", Unit.DipToTwipI(cellSpacing));
				rtfwriter.WriteTag("trspdfl", 3);
				rtfwriter.WriteTag("trspdr", Unit.DipToTwipI(cellSpacing));
				rtfwriter.WriteTag("trspdfr", 3);
				rtfwriter.WriteTag("trspdt", Unit.DipToTwipI(cellSpacing));
				rtfwriter.WriteTag("trspdft", 3);
				rtfwriter.WriteTag("trspdb", Unit.DipToTwipI(cellSpacing));
				rtfwriter.WriteTag("trspdfb", 3);
			}
		}

		public void ExportTable()
		{
			using (this.writer.WriteGroup())
			{
				int value;
				if (!string.IsNullOrEmpty(this.table.StyleId) && this.exporter.Context.StyleTable.TryGetValue(this.table.StyleId, out value))
				{
					this.exporter.Context.TableStylesStack.Push(new int?(value));
				}
				else
				{
					this.exporter.Context.TableStylesStack.Push(null);
				}
				TableCellDataGrid tableCellDataGrid = TableCellDataGrid.CreateFromTable(this.table);
				foreach (TableRowData tableRowData in tableCellDataGrid.Data)
				{
					foreach (TableCellData tableCellData in tableRowData.TableCellDatas)
					{
						if (tableCellData.Type != TableCellType.Empty && tableCellData.Type != TableCellType.HorizontalyMerged)
						{
							if (tableCellData.Type == TableCellType.VerticallyMerged)
							{
								this.WriteCellEnd();
							}
							else if (tableCellData.Type == TableCellType.Cell)
							{
								if (tableCellData.Cell.Blocks.Count > 0)
								{
									using (IEnumerator<BlockBase> enumerator3 = tableCellData.Cell.Blocks.GetEnumerator())
									{
										while (enumerator3.MoveNext())
										{
											BlockBase block = enumerator3.Current;
											this.exporter.ExportBlock(block);
										}
										goto IL_150;
									}
									goto IL_144;
								}
								goto IL_144;
								IL_150:
								this.WriteCellEnd();
								continue;
								IL_144:
								this.ExportEmptyCell(tableCellData.Cell);
								goto IL_150;
							}
						}
					}
					this.WriteRowProperties(tableRowData);
				}
				this.exporter.Context.TableStylesStack.Pop();
			}
		}

		static void ExportTableLook(TableLooks tableValues, RtfWriter writer)
		{
			TableExporter.ExportTableLook(tableValues, TableLooks.FirstRow, true, writer);
			TableExporter.ExportTableLook(tableValues, TableLooks.LastRow, true, writer);
			TableExporter.ExportTableLook(tableValues, TableLooks.FirstColumn, true, writer);
			TableExporter.ExportTableLook(tableValues, TableLooks.LastColumn, true, writer);
			TableExporter.ExportTableLook(tableValues, TableLooks.BandedRows, false, writer);
			TableExporter.ExportTableLook(tableValues, TableLooks.BandedColumns, false, writer);
		}

		static void ExportTableLook(TableLooks tableValues, TableLooks valueToExport, bool exportIfFlagIsSet, RtfWriter writer)
		{
			if (exportIfFlagIsSet)
			{
				if (tableValues.HasFlag(valueToExport))
				{
					writer.WriteTag(RtfHelper.TableLookMapper.GetFromValue(valueToExport));
					return;
				}
			}
			else if (!tableValues.HasFlag(valueToExport))
			{
				writer.WriteTag(RtfHelper.TableLookMapper.GetFromValue(valueToExport));
			}
		}

		void ExportEmptyCell(TableCell cell)
		{
			this.writer.WriteTag("pard");
			this.writer.WriteTag("intbl");
			int num = RtfHelper.GetNestingLevel(cell);
			num = (num - 1) / 3;
			this.writer.WriteTag("itap", num);
		}

		void WriteCellEnd()
		{
			if (this.isNestedTable)
			{
				this.writer.WriteTag("nestcell");
				return;
			}
			this.writer.WriteTag("cell");
		}

		void WriteRowProperties(TableRowData rowData)
		{
			if (this.isNestedTable)
			{
				using (this.writer.WriteGroup("nesttableprops", true))
				{
					this.ExportRowDefinition(rowData);
					this.writer.WriteTag("nestrow");
					return;
				}
			}
			this.ExportRowDefinition(rowData);
			this.writer.WriteTag("row");
		}

		void ExportRowDefinition(TableRowData rowData)
		{
			TableRow row = rowData.Row;
			int gridRowIndex = row.GridRowIndex;
			this.writer.WriteTag("trowd");
			this.writer.WriteTag("irow", gridRowIndex);
			this.writer.WriteTag("irowband", gridRowIndex);
			int parameter;
			if (!string.IsNullOrEmpty(this.table.StyleId) && this.exporter.Context.StyleTable.TryGetValue(this.table.StyleId, out parameter))
			{
				this.writer.WriteTag("ts", parameter);
			}
			if (row.Table.Properties.FlowDirection.GetActualValue().Value == FlowDirection.LeftToRight)
			{
				this.writer.WriteTag("ltrrow");
			}
			else
			{
				this.writer.WriteTag("rtlrow");
			}
			if (gridRowIndex == row.Table.Rows.Count - 1)
			{
				this.writer.WriteTag("lastrow");
			}
			TableExporter.ExportCellSpacing(this.table.TableCellSpacing, this.writer);
			BorderExporter.ExportTableBorders(this.table.Borders, this.exporter.Context, this.writer);
			TableRowHeight actualValue = row.Properties.Height.GetActualValue();
			if (actualValue.Type == HeightType.Exact && actualValue.Value != 0.0)
			{
				this.writer.WriteTag("trrh", -Unit.DipToTwipI(row.Properties.Height.GetActualValue().Value));
			}
			else if (actualValue.Type == HeightType.AtLeast && actualValue.Value != 0.0)
			{
				this.writer.WriteTag("trrh", Unit.DipToTwipI(row.Properties.Height.GetActualValue().Value));
			}
			if (!row.CanSplit)
			{
				this.writer.WriteTag("trkeep");
			}
			if (row.RepeatOnEveryPage)
			{
				this.writer.WriteTag("trhdr");
			}
			if (this.table.PreferredWidth.Type == TableWidthUnitType.Fixed)
			{
				this.writer.WriteTag("trwWidth", Unit.DipToTwipI(this.table.PreferredWidth.Value));
				this.writer.WriteTag("trftsWidth", 3);
			}
			else if (this.table.PreferredWidth.Type == TableWidthUnitType.Percent)
			{
				this.writer.WriteTag("trwWidth", (int)(this.table.PreferredWidth.Value * 50.0));
				this.writer.WriteTag("trftsWidth", 2);
			}
			this.writer.WriteTag("trautofit", (row.Table.LayoutType == TableLayoutType.FixedWidth) ? 0 : 1);
			TableExporter.ExportTableIndent(this.table.Indent, this.writer);
			TableExporter.ExportCellPaddingForTable(this.table.TableCellPadding, this.writer);
			TableExporter.ExportTableHorizontalAlignment(this.table.Alignment, this.table.FlowDirection, this.writer);
			TableExporter.ExportTableShading(this.table.Properties, this.exporter.Context, this.writer);
			TableExporter.ExportTableLook(this.table.Looks, this.writer);
			foreach (TableCellData tableCellData in rowData.TableCellDatas)
			{
				if (tableCellData.Type != TableCellType.Empty && tableCellData.Type != TableCellType.HorizontalyMerged)
				{
					if (tableCellData.Type == TableCellType.Cell && tableCellData.Cell.RowSpan > 1)
					{
						this.writer.WriteTag("clvmgf");
					}
					else if (tableCellData.Type == TableCellType.VerticallyMerged)
					{
						this.writer.WriteTag("clvmrg");
					}
					this.ExportCellProperties(tableCellData.Cell);
				}
			}
		}

		void ExportCellProperties(TableCell cell)
		{
			TableExporter.ExportCellVerticalAlignment(cell.VerticalAlignment, this.writer, false);
			BorderExporter.ExportCellBorders(this.table, cell, this.borderGrid, this.exporter.Context, this.writer);
			TableExporter.ExportCellShading(cell.Properties, this.exporter.Context, this.writer, false);
			TableExporter.ExportPreferredWidth(cell, this.writer);
			TableExporter.ExportCellPaddingForTableCell(cell.Padding, this.writer);
			string tagName;
			if (RtfHelper.TableCellTextDirectionMapper.TryGetFromValue(cell.TextDirection, out tagName))
			{
				this.writer.WriteTag(tagName);
			}
			if (!cell.CanWrapContent)
			{
				this.writer.WriteTag("clNoWrap");
			}
			if (cell.IgnoreCellMarkerInRowHeightCalculation)
			{
				this.writer.WriteTag("clhidemark");
			}
			int num = cell.GridColumnIndex + cell.ColumnSpan - 1;
			double value = (double)((num + 1) * 200);
			this.writer.WriteTag("cellx", Unit.DipToTwipI(value));
		}

		readonly RtfDocumentExporter exporter;

		readonly RtfWriter writer;

		readonly Table table;

		readonly bool isNestedTable;

		readonly TableBorderGrid borderGrid;
	}
}
