using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class BorderRenderHelper
	{
		public static DoubleCollection GetStrokeDashArrayByBorderStyle(CellBorderStyle borderStyle)
		{
			switch (borderStyle)
			{
			case CellBorderStyle.None:
				return null;
			case CellBorderStyle.Hair:
				return new DoubleCollection { 1.5, 1.5 };
			case CellBorderStyle.Dotted:
				return new DoubleCollection { 2.0, 2.0 };
			case CellBorderStyle.DashDotDot:
				return new DoubleCollection { 8.0, 2.0, 2.0, 2.0, 2.0, 2.0 };
			case CellBorderStyle.DashDot:
				return new DoubleCollection { 8.0, 2.0, 2.0, 2.0 };
			case CellBorderStyle.Dashed:
				return new DoubleCollection { 3.0, 1.0 };
			case CellBorderStyle.Thin:
				return null;
			case CellBorderStyle.MediumDashDotDot:
				return new DoubleCollection { 4.0, 1.0, 1.0, 1.0, 1.0, 1.0 };
			case CellBorderStyle.MediumDashDot:
			case CellBorderStyle.SlantDashDot:
				return new DoubleCollection { 4.0, 1.0, 1.0, 1.0 };
			case CellBorderStyle.MediumDashed:
				return new DoubleCollection { 2.0, 1.0 };
			case CellBorderStyle.Double:
			case CellBorderStyle.Medium:
				return null;
			case CellBorderStyle.Thick:
				return null;
			default:
				return null;
			}
		}

		public static CellBorder GetRightBorderToDisplay(WorksheetRenderUpdateContext context, int rowIndex, int columnIndex)
		{
			CellBorder rightBorder = context.GetRightBorder(WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, columnIndex));
			if (columnIndex == SpreadsheetDefaultValues.ColumnCount - 1)
			{
				return rightBorder;
			}
			CellBorder leftBorder = context.GetLeftBorder(WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, columnIndex + 1));
			return CellBorder.GetWithMaxPriority(leftBorder, rightBorder, context.CurrentTheme.ColorScheme);
		}

		public static CellBorder GetBottomBorderToDisplay(WorksheetRenderUpdateContext context, int rowIndex, int columnIndex)
		{
			CellBorder bottomBorder = context.GetBottomBorder(WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, columnIndex));
			if (rowIndex == SpreadsheetDefaultValues.RowCount - 1)
			{
				return bottomBorder;
			}
			CellBorder topBorder = context.GetTopBorder(WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex + 1, columnIndex));
			return CellBorder.GetWithMaxPriority(bottomBorder, topBorder, context.CurrentTheme.ColorScheme);
		}
	}
}
