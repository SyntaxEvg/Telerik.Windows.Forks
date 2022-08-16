using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Borders;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import
{
	class TableCellStyle
	{
		public TableCellStyle(TableRowStyle rowStyle)
		{
			this.rowStyle = rowStyle;
			this.ResetDefaults();
		}

		public bool CanWrapContent { get; set; }

		public bool IgnoreCellMarkerInRowHeight { get; set; }

		public VerticalAlignment VerticalAlignment { get; set; }

		public Alignment TextAlignment { get; set; }

		public RtfPadding Padding { get; set; }

		public Color? PatternColor { get; set; }

		public Color? BackgroundColor { get; set; }

		public ShadingPattern? Pattern { get; set; }

		public int CellXBoundry { get; set; }

		public bool IsFirstInHorizontalRange { get; set; }

		public bool IsInHorizontalRange { get; set; }

		public bool IsFirstInVerticalRange { get; set; }

		public bool IsInVerticalRange { get; set; }

		public bool IsLocked { get; set; }

		public RtfCellBorders Borders { get; set; }

		public int PreferredWidthValue { get; set; }

		public int PreferredWidthUnitType { get; set; }

		public TextDirection TextDirection { get; set; }

		public void ResetDefaults()
		{
			this.Padding = new RtfPadding();
			this.TextAlignment = Alignment.Left;
			this.VerticalAlignment = VerticalAlignment.Top;
			this.TextDirection = DocumentDefaultStyleSettings.TableCellTextDirection;
			this.PatternColor = null;
			this.BackgroundColor = null;
			this.Pattern = null;
			this.CellXBoundry = 0;
			this.Borders = new RtfCellBorders();
			this.CanWrapContent = true;
			this.IgnoreCellMarkerInRowHeight = false;
		}

		public void ApplyCellStyle(TableCell cell)
		{
			cell.VerticalAlignment = this.VerticalAlignment;
			cell.Padding = this.CalculatePadding();
			if (this.PreferredWidthUnitType != 0)
			{
				cell.PreferredWidth = RtfHelper.ConvertTableWidthUnitType(this.PreferredWidthUnitType, this.PreferredWidthValue);
			}
			if (this.BackgroundColor != null)
			{
				cell.Shading.BackgroundColor = new ThemableColor(this.BackgroundColor.Value);
			}
			if (this.PatternColor != null)
			{
				cell.Shading.PatternColor = new ThemableColor(this.PatternColor.Value);
			}
			if (this.Pattern != null)
			{
				cell.Shading.Pattern = this.Pattern.Value;
			}
			cell.TextDirection = this.TextDirection;
			cell.CanWrapContent = this.CanWrapContent;
			cell.IgnoreCellMarkerInRowHeightCalculation = this.IgnoreCellMarkerInRowHeight;
			TableCellBorders tableCellBorders = this.Borders.CreateBorders();
			if (!DocumentDefaultStyleSettings.TableCellBorders.Equals(tableCellBorders))
			{
				cell.Borders = tableCellBorders;
			}
		}

		static double GetPaddingValue(RtfPaddingValue cellPaddingValue, RtfPaddingValue rowPaddingValue, double rowDefault, double cellDefault)
		{
			if (cellPaddingValue.HasValue)
			{
				if (cellPaddingValue.UnitType != 0)
				{
					return Unit.TwipToDip((double)cellPaddingValue.Value);
				}
				return rowDefault;
			}
			else
			{
				if (!rowPaddingValue.HasValue)
				{
					return cellDefault;
				}
				if (rowPaddingValue.UnitType != 0)
				{
					return Unit.TwipToDip((double)rowPaddingValue.Value);
				}
				return rowDefault;
			}
		}

		Padding CalculatePadding()
		{
			int num = (int)this.rowStyle.DefaultCellPadding;
			RtfPadding cellPadding = this.rowStyle.CellPadding;
			double paddingValue = TableCellStyle.GetPaddingValue(this.Padding.Left, cellPadding.Left, (double)num, DocumentDefaultStyleSettings.TableCellPadding.Left);
			double paddingValue2 = TableCellStyle.GetPaddingValue(this.Padding.Top, cellPadding.Top, (double)num, DocumentDefaultStyleSettings.TableCellPadding.Top);
			double paddingValue3 = TableCellStyle.GetPaddingValue(this.Padding.Right, cellPadding.Right, (double)num, DocumentDefaultStyleSettings.TableCellPadding.Right);
			double paddingValue4 = TableCellStyle.GetPaddingValue(this.Padding.Bottom, cellPadding.Bottom, (double)num, DocumentDefaultStyleSettings.TableCellPadding.Bottom);
			return new Padding(paddingValue, paddingValue2, paddingValue3, paddingValue4);
		}

		readonly TableRowStyle rowStyle;
	}
}
