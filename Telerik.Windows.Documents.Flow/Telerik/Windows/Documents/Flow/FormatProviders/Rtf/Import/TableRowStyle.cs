using System;
using System.Collections.Generic;
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
	class TableRowStyle
	{
		public TableRowStyle()
		{
			this.CellStyles = new List<TableCellStyle>();
			this.Borders = new RtfRowBorders();
			this.CellPadding = new RtfPadding();
			this.DefaultCellSpacing = new RtfCellSpacing();
			this.AutoFit = TableLayoutType.FixedWidth;
			this.TableLooks = DocumentDefaultStyleSettings.TableLooks;
			this.CanSplit = true;
			this.RepeatOnEveryPage = false;
		}

		public TableRowHeight RowHeight { get; set; }

		public TableLooks TableLooks { get; set; }

		public int? ColumnBanding { get; set; }

		public int? RowBanding { get; set; }

		public int PreferredWidthValue { get; set; }

		public int PreferredWidthUnitType { get; set; }

		public double DefaultCellPadding { get; set; }

		public RtfPadding CellPadding { get; set; }

		public RtfCellSpacing DefaultCellSpacing { get; set; }

		public List<TableCellStyle> CellStyles { get; set; }

		public RtfRowBorders Borders { get; set; }

		public TableLayoutType AutoFit { get; set; }

		public FlowDirection FlowDirection { get; set; }

		public int TableIndentValue { get; set; }

		public int TableIndentType { get; set; }

		public int TableRowPosition { get; set; }

		public Alignment TableHorizontalAlignment { get; set; }

		public string TableStyleId { get; set; }

		public Color? PatternColor { get; set; }

		public Color? BackgroundColor { get; set; }

		public ShadingPattern? Pattern { get; set; }

		public bool CanSplit { get; set; }

		public bool RepeatOnEveryPage { get; set; }

		public TableCellStyle CurrentCellStyle
		{
			get
			{
				if (this.CellStyles.Count > 0)
				{
					return this.CellStyles[this.CellStyles.Count - 1];
				}
				return null;
			}
		}

		public void EnsureCurrentCellStyle()
		{
			if (this.CellStyles.Count == 0 || this.CellStyles[this.CellStyles.Count - 1].IsLocked)
			{
				TableCellStyle item = new TableCellStyle(this);
				this.CellStyles.Add(item);
			}
		}

		public void ApplyRowStyle(TableRow row)
		{
			if (this.RowHeight != null)
			{
				row.Height = this.RowHeight;
			}
			row.CanSplit = this.CanSplit;
			row.RepeatOnEveryPage = this.RepeatOnEveryPage;
		}

		public void ApplyTableStyle(Table table)
		{
			if (!string.IsNullOrEmpty(this.TableStyleId))
			{
				table.StyleId = this.TableStyleId;
			}
			table.Alignment = TableRowStyle.GetHorizontalAlignment(this.TableHorizontalAlignment, this.FlowDirection);
			table.Borders = this.Borders.CreateBorders();
			table.TableCellSpacing = Math.Round(this.DefaultCellSpacing.Left.ValueInDips);
			table.TableCellPadding = TableRowStyle.CalculatePadding(this.CellPadding, this.DefaultCellPadding);
			if (this.PreferredWidthUnitType != 0)
			{
				table.PreferredWidth = RtfHelper.ConvertTableWidthUnitType(this.PreferredWidthUnitType, this.PreferredWidthValue);
			}
			table.LayoutType = this.AutoFit;
			table.FlowDirection = this.FlowDirection;
			table.Indent = RtfHelper.GetTableIndentFormRtf(this.TableIndentValue, this.TableIndentType);
			table.Looks = this.TableLooks;
			if (this.TableIndentValue == 0 && this.TableIndentType == 0)
			{
				table.Indent = Unit.TwipToDip((double)this.TableRowPosition);
			}
			if (this.BackgroundColor != null)
			{
				table.Shading.BackgroundColor = new ThemableColor(this.BackgroundColor.Value);
			}
			if (this.PatternColor != null)
			{
				table.Shading.PatternColor = new ThemableColor(this.PatternColor.Value);
			}
			if (this.Pattern != null)
			{
				table.Shading.Pattern = this.Pattern.Value;
			}
			if (this.RowBanding != null)
			{
				table.Properties.RowBanding.LocalValue = this.RowBanding;
			}
			if (this.ColumnBanding != null)
			{
				table.Properties.ColumnBanding.LocalValue = this.ColumnBanding;
			}
		}

		static Alignment GetHorizontalAlignment(Alignment alignment, FlowDirection flowDirection)
		{
			Alignment result = alignment;
			if (flowDirection == FlowDirection.RightToLeft)
			{
				if (alignment == Alignment.Left)
				{
					result = Alignment.Right;
				}
				else if (alignment == Alignment.Right)
				{
					result = Alignment.Left;
				}
			}
			return result;
		}

		static Padding CalculatePadding(RtfPadding rowPadding, double rowDefault)
		{
			double paddingValue = TableRowStyle.GetPaddingValue(rowPadding.Left, rowDefault, DocumentDefaultStyleSettings.TableTableCellPadding.Left);
			double paddingValue2 = TableRowStyle.GetPaddingValue(rowPadding.Top, rowDefault, DocumentDefaultStyleSettings.TableTableCellPadding.Top);
			double paddingValue3 = TableRowStyle.GetPaddingValue(rowPadding.Right, rowDefault, DocumentDefaultStyleSettings.TableTableCellPadding.Right);
			double paddingValue4 = TableRowStyle.GetPaddingValue(rowPadding.Bottom, rowDefault, DocumentDefaultStyleSettings.TableTableCellPadding.Bottom);
			return new Padding(paddingValue, paddingValue2, paddingValue3, paddingValue4);
		}

		static double GetPaddingValue(RtfPaddingValue rowPaddingValue, double rowDefault, double modelDefault)
		{
			if (!rowPaddingValue.HasValue)
			{
				return modelDefault;
			}
			if (rowPaddingValue.UnitType != 0)
			{
				return Unit.TwipToDip((double)rowPaddingValue.Value);
			}
			return rowDefault;
		}
	}
}
