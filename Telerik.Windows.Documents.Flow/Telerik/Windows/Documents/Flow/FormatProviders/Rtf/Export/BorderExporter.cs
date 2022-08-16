using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.BorderEvaluation;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Media;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Export
{
	static class BorderExporter
	{
		public static void ExportParagraphBorders(ParagraphBorders borders, ExportContext context, RtfWriter rtfWriter)
		{
			BorderExporter.ExportBorder("brdrt", borders.Top, context, rtfWriter);
			BorderExporter.ExportBorder("brdrb", borders.Bottom, context, rtfWriter);
			BorderExporter.ExportBorder("brdrl", borders.Left, context, rtfWriter);
			BorderExporter.ExportBorder("brdrr", borders.Right, context, rtfWriter);
			BorderExporter.ExportBorder("brdrbtw", borders.Between, context, rtfWriter);
		}

		public static void ExportTableBorders(TableBorders borders, ExportContext context, RtfWriter rtfWriter)
		{
			if (borders == null || borders == DocumentDefaultStyleSettings.TableBorders)
			{
				return;
			}
			BorderExporter.ExportBorder("trbrdrt", borders.Top, context, rtfWriter);
			BorderExporter.ExportBorder("trbrdrb", borders.Bottom, context, rtfWriter);
			BorderExporter.ExportBorder("trbrdrl", borders.Left, context, rtfWriter);
			BorderExporter.ExportBorder("trbrdrr", borders.Right, context, rtfWriter);
			BorderExporter.ExportBorder("trbrdrh", borders.InsideHorizontal, context, rtfWriter);
			BorderExporter.ExportBorder("trbrdrv", borders.InsideVertical, context, rtfWriter);
		}

		public static void ExportCellBorders(Table table, TableCell cell, TableBorderGrid borderGrid, ExportContext context, RtfWriter rtfWriter)
		{
			if (table.HasCellSpacing)
			{
				Border border = ((cell.Borders.Top.Style == BorderStyle.Inherit) ? table.Borders.InsideHorizontal : cell.Borders.Top);
				BorderExporter.ExportBorder("clbrdrt", border, context, rtfWriter);
				Border border2 = ((cell.Borders.Bottom.Style == BorderStyle.Inherit) ? table.Borders.InsideHorizontal : cell.Borders.Bottom);
				BorderExporter.ExportBorder("clbrdrb", border2, context, rtfWriter);
				Border border3 = ((cell.Borders.Left.Style == BorderStyle.Inherit) ? table.Borders.InsideVertical : cell.Borders.Left);
				BorderExporter.ExportBorder("clbrdrl", border3, context, rtfWriter);
				Border border4 = ((cell.Borders.Right.Style == BorderStyle.Inherit) ? table.Borders.InsideVertical : cell.Borders.Right);
				BorderExporter.ExportBorder("clbrdrr", border4, context, rtfWriter);
			}
			else
			{
				Border border5 = borderGrid.GetTopHorizontalBorder(cell.GridRowIndex, cell.GridColumnIndex).Border;
				BorderExporter.ExportBorder("clbrdrt", border5, context, rtfWriter);
				Border border6 = borderGrid.GetTopHorizontalBorder(cell.GridRowIndex + cell.RowSpan, cell.GridColumnIndex).Border;
				BorderExporter.ExportBorder("clbrdrb", border6, context, rtfWriter);
				Border border7 = borderGrid.GetLeftVerticalBorder(cell.GridRowIndex, cell.GridColumnIndex).Border;
				BorderExporter.ExportBorder("clbrdrl", border7, context, rtfWriter);
				Border border8 = borderGrid.GetLeftVerticalBorder(cell.GridRowIndex, cell.GridColumnIndex + cell.ColumnSpan).Border;
				BorderExporter.ExportBorder("clbrdrr", border8, context, rtfWriter);
			}
			if (cell.Borders.DiagonalDown.Style != BorderStyle.Inherit && cell.Borders.DiagonalDown.Style != BorderStyle.None)
			{
				BorderExporter.ExportBorder("cldglu", cell.Borders.DiagonalDown, context, rtfWriter);
			}
			if (cell.Borders.DiagonalUp.Style != BorderStyle.Inherit && cell.Borders.DiagonalUp.Style != BorderStyle.None)
			{
				BorderExporter.ExportBorder("cldgll", cell.Borders.DiagonalUp, context, rtfWriter);
			}
		}

		public static void ExportCellBordersFromStyle(TableCellBorders borders, ExportContext context, RtfWriter rtfWriter)
		{
			if (borders == null || borders == DocumentDefaultStyleSettings.TableCellBorders)
			{
				return;
			}
			BorderExporter.ExportBorder("clbrdrt", borders.Top, context, rtfWriter);
			BorderExporter.ExportBorder("clbrdrb", borders.Bottom, context, rtfWriter);
			BorderExporter.ExportBorder("clbrdrl", borders.Left, context, rtfWriter);
			BorderExporter.ExportBorder("clbrdrr", borders.Right, context, rtfWriter);
			BorderExporter.ExportBorder("cldglu", borders.DiagonalDown, context, rtfWriter);
			BorderExporter.ExportBorder("cldgll", borders.DiagonalUp, context, rtfWriter);
		}

		static void ExportBorder(string tag, Border border, ExportContext context, RtfWriter rtfWriter)
		{
			rtfWriter.WriteTag(tag);
			if (border.Thickness != 0.0)
			{
				rtfWriter.WriteTag(RtfHelper.BorderMapper.GetFromValue(border.Style));
				rtfWriter.WriteTag("brdrw", Math.Min(255, Unit.DipToTwipI(border.Thickness)));
				Color actualValue = border.Color.GetActualValue(context.Document.Theme);
				if (!RtfHelper.IsTransparentColor(actualValue))
				{
					rtfWriter.WriteTag("brdrcf", context.ColorTable[actualValue]);
					return;
				}
			}
			else
			{
				rtfWriter.WriteTag("brdrnone");
			}
		}
	}
}
