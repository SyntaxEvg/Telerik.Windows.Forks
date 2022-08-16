using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	static class CellBoxRenderHelper
	{
		public static bool ShouldDisplayContentAsSharps(long index, WorksheetRenderUpdateContext updateContext)
		{
			ICellValue cellValue = updateContext.GetCellValue(index);
			NumberCellValue numberCellValue = cellValue as NumberCellValue;
			FormulaCellValue formulaCellValue = cellValue as FormulaCellValue;
			if (numberCellValue == null)
			{
				if (formulaCellValue != null)
				{
					numberCellValue = formulaCellValue.GetResultValueAsCellValue() as NumberCellValue;
				}
				if (numberCellValue == null)
				{
					return false;
				}
			}
			CellLayoutBox visibleCellBox = updateContext.GetVisibleCellBox(index);
			Size cellContentSize = updateContext.GetCellContentSize(visibleCellBox);
			CellValueFormat cellFormat = updateContext.GetCellFormat(index);
			return !cellContentSize.Width.LessOrEqualDouble(visibleCellBox.Size.Width) || (cellFormat.FormatStringInfo.FormatType == FormatStringType.DateTime && numberCellValue.ToDateTime() == null);
		}

		public static Thickness CalculateIndentPadding(long index, WorksheetRenderUpdateContext updateContext)
		{
			Guard.ThrowExceptionIfNull<WorksheetRenderUpdateContext>(updateContext, "updateContext");
			double num = 0.0;
			double num2 = 0.0;
			if (updateContext.GetCellValue(index) != EmptyCellValue.EmptyValue)
			{
				num = SpreadsheetDefaultValues.LeftCellMargin;
				bool isWrapped = updateContext.GetIsWrapped(index);
				if (isWrapped)
				{
					num2 = SpreadsheetDefaultValues.RightCellMargin;
				}
			}
			Thickness result = default(Thickness);
			if (!CellBoxRenderHelper.ShouldDisplayContentAsSharps(index, updateContext))
			{
				double num3 = (double)updateContext.GetIndentPropertyValue(index) * SpreadsheetDefaultValues.IndentStep;
				if (num3 != 0.0)
				{
					RadHorizontalAlignment horizontalAlignment = updateContext.GetHorizontalAlignment(index);
					if (horizontalAlignment == RadHorizontalAlignment.Left)
					{
						result = new Thickness(num3, 0.0, 0.0, 0.0);
					}
					else if (horizontalAlignment == RadHorizontalAlignment.Right)
					{
						result = new Thickness(0.0, 0.0, num3, 0.0);
					}
				}
			}
			result.Left += num;
			result.Right += num2;
			return result;
		}

		public static Rect CalculateActualBoundingRectangleByContentAlignment(CellLayoutBox cellBox, WorksheetRenderUpdateContext updateContext)
		{
			Guard.ThrowExceptionIfNull<WorksheetRenderUpdateContext>(updateContext, "updateContext");
			Guard.ThrowExceptionIfNull<CellLayoutBox>(cellBox, "cellBox");
			Size cellContentSize = updateContext.GetCellContentSize(cellBox);
			if (CellBoxRenderHelper.ShouldDisplayContentAsSharps(cellBox.LongIndex, updateContext))
			{
				cellContentSize.Width = cellBox.Size.Width;
			}
			RadHorizontalAlignment horizontalAlignment = updateContext.GetHorizontalAlignment(cellBox.LongIndex);
			ICellValue cellValue = updateContext.GetCellValue(cellBox.LongIndex);
			Rect result = default(Rect);
			TextAlignment horizontalAlignment2 = CellBoxRenderHelper.GetHorizontalAlignment(horizontalAlignment, cellValue);
			double width = cellBox.Width;
			if (updateContext.GetIsWrapped(cellBox.LongIndex))
			{
				result.X = cellBox.Left;
				result.Width = width;
			}
			else
			{
				switch (horizontalAlignment2)
				{
				case TextAlignment.Left:
				case TextAlignment.Justify:
					result.X = cellBox.Left;
					break;
				case TextAlignment.Right:
					result.X = cellBox.Left + (width - cellContentSize.Width);
					break;
				case TextAlignment.Center:
					result.X = cellBox.Left + (width - cellContentSize.Width) / 2.0;
					break;
				}
				result.Width = cellContentSize.Width;
			}
			RadVerticalAlignment verticalAlignment = updateContext.GetVerticalAlignment(cellBox.LongIndex);
			double height = cellBox.Height;
			switch (verticalAlignment)
			{
			case RadVerticalAlignment.Bottom:
			case RadVerticalAlignment.Justify:
			case RadVerticalAlignment.Distributed:
			case RadVerticalAlignment.Undetermined:
				result.Y = cellBox.Top + (height - cellContentSize.Height);
				break;
			case RadVerticalAlignment.Center:
				result.Y = cellBox.Top + (height - cellContentSize.Height) / 2.0;
				break;
			case RadVerticalAlignment.Top:
				result.Y = cellBox.Top;
				break;
			}
			result.Height = cellContentSize.Height;
			return result;
		}

		public static Rect CalculateCellClipping(CellLayoutBox cellBox, WorksheetRenderUpdateContext updateContext)
		{
			CellLayoutBox cellLayoutBox;
			CellLayoutBox cellLayoutBox2;
			updateContext.GetPreviousNextNonEmptyNonMergedCells(cellBox.LongIndex, out cellLayoutBox, out cellLayoutBox2);
			Rect actualBoundingRectangleByContentAlignment = updateContext.GetActualBoundingRectangleByContentAlignment(cellBox);
			double num = Math.Max(0.0, cellLayoutBox.BoundingRectangle.Left - actualBoundingRectangleByContentAlignment.Left);
			double num2 = System.Math.Min(actualBoundingRectangleByContentAlignment.Width, cellLayoutBox2.BoundingRectangle.Right - actualBoundingRectangleByContentAlignment.Left);
			double num3 = cellBox.Height - CellBorder.Default.Thickness;
			num3 = Math.Max(0.0, num3);
			return new Rect(num, Math.Max(0.0, cellBox.Top - actualBoundingRectangleByContentAlignment.Top), Math.Max(0.0, num2 - num), num3);
		}

		public static Rect CalculateCellClippedBoundingRectangle(CellLayoutBox cellBox, WorksheetRenderUpdateContext updateContext)
		{
			CellLayoutBox cellLayoutBox;
			CellLayoutBox cellLayoutBox2;
			updateContext.GetPreviousNextNonEmptyNonMergedCells(cellBox.LongIndex, out cellLayoutBox, out cellLayoutBox2);
			Rect actualBoundingRectangleByContentAlignment = updateContext.GetActualBoundingRectangleByContentAlignment(cellBox);
			double num = Math.Max(0.0, cellLayoutBox.BoundingRectangle.Left - actualBoundingRectangleByContentAlignment.Left);
			double num2 = System.Math.Min(actualBoundingRectangleByContentAlignment.Width, cellLayoutBox2.BoundingRectangle.Right - actualBoundingRectangleByContentAlignment.Left);
			double width = Math.Max(0.0, num2 - num);
			double num3 = cellBox.Height - CellBorder.Default.Thickness;
			num3 = Math.Max(0.0, num3);
			return new Rect(actualBoundingRectangleByContentAlignment.Left + num, actualBoundingRectangleByContentAlignment.Top + 0.0, width, num3);
		}

		public static Thickness GetPaddingForVerticalAlignment(RadVerticalAlignment verticalAlignment, double desiredHeight, double finalHeight)
		{
			finalHeight -= 1.0;
			double num = 0.0;
			double bottom = 0.0;
			double num2 = Math.Max(0.0, finalHeight - desiredHeight);
			switch (verticalAlignment)
			{
			case RadVerticalAlignment.Bottom:
			case RadVerticalAlignment.Justify:
			case RadVerticalAlignment.Distributed:
				num = num2;
				break;
			case RadVerticalAlignment.Center:
				num = num2 / 2.0;
				bottom = num;
				break;
			case RadVerticalAlignment.Top:
				num = 0.0;
				bottom = num2;
				break;
			}
			return new Thickness(0.0, num, 0.0, bottom);
		}

		public static TextAlignment GetHorizontalAlignment(RadHorizontalAlignment horizontalAlignment, ICellValue value)
		{
			TextAlignment result = TextAlignment.Left;
			if (horizontalAlignment == RadHorizontalAlignment.General)
			{
				switch (value.ResultValueType)
				{
				case CellValueType.Number:
					return TextAlignment.Right;
				case CellValueType.Boolean:
					return TextAlignment.Center;
				case CellValueType.Text:
					return TextAlignment.Left;
				case CellValueType.Error:
				{
					FormulaCellValue formulaCellValue = value as FormulaCellValue;
					if (formulaCellValue != null && formulaCellValue.IsValueCyclicReferenceError())
					{
						return TextAlignment.Right;
					}
					return TextAlignment.Center;
				}
				}
				result = TextAlignment.Justify;
			}
			else
			{
				switch (horizontalAlignment)
				{
				case RadHorizontalAlignment.Left:
					result = TextAlignment.Left;
					break;
				case RadHorizontalAlignment.Center:
				case RadHorizontalAlignment.CenterContinuous:
					result = TextAlignment.Center;
					break;
				case RadHorizontalAlignment.Right:
					result = TextAlignment.Right;
					break;
				case RadHorizontalAlignment.Justify:
					result = TextAlignment.Justify;
					break;
				}
			}
			return result;
		}
	}
}
