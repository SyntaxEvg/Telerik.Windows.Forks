using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorkbookCommands;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	static class WorkbookCommands
	{
		public static readonly SetDefaultPropertyValueCommand<RowHeight> SetDefaultRowHeight = new SetDefaultPropertyValueCommand<RowHeight>();

		public static readonly SetRowPropertyCommand<RowHeight> SetRowHeight = new SetRowPropertyCommand<RowHeight>();

		public static readonly SetRowPropertyCommand<bool> SetRowHidden = new SetRowPropertyCommand<bool>();

		public static readonly SetRowPropertyCommand<int> SetRowOutlineLevel = new SetRowPropertyCommand<int>();

		public static readonly InsertRowCommand InsertRow = new InsertRowCommand();

		public static readonly RemoveRowCommand RemoveRow = new RemoveRowCommand();

		public static readonly SetRowPropertyValuesCommand SetRowProperty = new SetRowPropertyValuesCommand();

		public static readonly SetDefaultPropertyValueCommand<ColumnWidth> SetDefaultColumnWidth = new SetDefaultPropertyValueCommand<ColumnWidth>();

		public static readonly SetColumnPropertyCommand<ColumnWidth> SetColumnWidth = new SetColumnPropertyCommand<ColumnWidth>();

		public static readonly SetColumnPropertyCommand<bool> SetColumnHidden = new SetColumnPropertyCommand<bool>();

		public static readonly SetColumnPropertyCommand<int> SetColumnOutlineLevel = new SetColumnPropertyCommand<int>();

		public static readonly InsertColumnCommand InsertColumn = new InsertColumnCommand();

		public static readonly RemoveColumnCommand RemoveColumn = new RemoveColumnCommand();

		public static readonly SetColumnPropertyValuesCommand SetColumnProperty = new SetColumnPropertyValuesCommand();

		public static readonly SetCellPropertyCommand<ICellValue> SetCellValue = new SetCellPropertyCommand<ICellValue>();

		public static readonly UpdateCellPropertyCommand<ICellValue> UpdateCellValue = new UpdateCellPropertyCommand<ICellValue>();

		public static readonly UpdateCellPropertyCommand<CellValueFormat> UpdateCellFormat = new UpdateCellPropertyCommand<CellValueFormat>();

		public static readonly SetCellPropertyCommand<CellValueFormat> SetCellFormat = new SetCellPropertyCommand<CellValueFormat>();

		public static readonly SetCellPropertyCommand<IFill> SetCellFill = new SetCellPropertyCommand<IFill>();

		public static readonly SetBordersCommand<SetCellBordersCommandContext> SetCellBorders = new SetBordersCommand<SetCellBordersCommandContext>();

		public static readonly SetBordersCommand<SetRowBordersCommandContext> SetRowBorders = new SetBordersCommand<SetRowBordersCommandContext>();

		public static readonly SetBordersCommand<SetColumnBordersCommandContext> SetColumnBorders = new SetBordersCommand<SetColumnBordersCommandContext>();

		public static readonly SetCellPropertyCommand<FontFamily> SetFontFamily = new SetCellPropertyCommand<FontFamily>();

		public static readonly SetCellPropertyCommand<double> SetFontSize = new SetCellPropertyCommand<double>();

		public static readonly SetCellPropertyCommand<bool> SetIsBold = new SetCellPropertyCommand<bool>();

		public static readonly SetCellPropertyCommand<bool> SetIsItalic = new SetCellPropertyCommand<bool>();

		public static readonly SetCellPropertyCommand<UnderlineType> SetUnderline = new SetCellPropertyCommand<UnderlineType>();

		public static readonly SetCellPropertyCommand<Color> SetForeColor = new SetCellPropertyCommand<Color>();

		public static readonly SetCellPropertyCommand<RadHorizontalAlignment> SetHorizontalAlignment = new SetCellPropertyCommand<RadHorizontalAlignment>();

		public static readonly SetCellPropertyCommand<RadVerticalAlignment> SetVerticalAlignment = new SetCellPropertyCommand<RadVerticalAlignment>();

		public static readonly SetCellPropertyCommand<int> SetIndent = new SetCellPropertyCommand<int>();

		public static readonly UpdateCellPropertyCommand<int> UpdateIndent = new UpdateCellPropertyCommand<int>();

		public static readonly SetCellPropertyCommand<bool> SetIsWrapped = new SetCellPropertyCommand<bool>();

		public static readonly InsertCellCommand InsertCell = new InsertCellCommand();

		public static readonly RemoveCellCommand RemoveCell = new RemoveCellCommand();

		public static readonly MergeCellsCommand MergeCells = new MergeCellsCommand();

		public static readonly UnmergeCellsCommand UnmergeCells = new UnmergeCellsCommand();

		public static readonly SetCellPropertyValuesCommand SetCellProperty = new SetCellPropertyValuesCommand();

		public static readonly AddHyperlinkCommand AddHyperlink = new AddHyperlinkCommand();

		public static readonly RemoveHyperlinkCommand RemoveHyperlink = new RemoveHyperlinkCommand();

		public static readonly AddSpreadsheetNameCommand AddSpreadsheetName = new AddSpreadsheetNameCommand();

		public static readonly RemoveSpreadsheetNameCommand RemoveSpreadsheetName = new RemoveSpreadsheetNameCommand();

		public static readonly AddShapeCommand AddShape = new AddShapeCommand();

		public static readonly RemoveShapeCommand RemoveShape = new RemoveShapeCommand();

		public static readonly SetShapeNameCommand SetShapeName = new SetShapeNameCommand();

		public static readonly SetShapePositionCommand SetShapePosition = new SetShapePositionCommand();

		public static readonly SetShapeWidthCommand SetShapeWidth = new SetShapeWidthCommand();

		public static readonly SetShapeHeightCommand SetShapeHeight = new SetShapeHeightCommand();

		public static readonly SetShapeFlipCommand SetShapeFlip = new SetShapeFlipCommand();

		public static readonly SetShapeRotationAngleCommand SetShapeRotationAngle = new SetShapeRotationAngleCommand();

		public static readonly SetShapeLockAspectRatioCommand SetShapeLockAspectRatio = new SetShapeLockAspectRatioCommand();

		public static readonly BringShapeForwardCommand BringShapeForward = new BringShapeForwardCommand();

		public static readonly BringShapeToFrontCommand BringShapeToFront = new BringShapeToFrontCommand();

		public static readonly SendShapeBackwardCommand SendShapeBackward = new SendShapeBackwardCommand();

		public static readonly SendShapeToBackCommand SendShapeToBack = new SendShapeToBackCommand();

		public static readonly SetImageImageSourceCommand SetImageImageSource = new SetImageImageSourceCommand();

		public static readonly SetImagePreferRelativeToOriginalResizeCommand SetImagePreferRelativeToOriginalResizeCommand = new SetImagePreferRelativeToOriginalResizeCommand();

		public static readonly AddToPrintAreaCommand AddToPrintArea = new AddToPrintAreaCommand();

		public static readonly RemoveFromPrintAreaCommand RemoveFromPrintArea = new RemoveFromPrintAreaCommand();

		public static readonly AddPageBreakCommand AddPageBreak = new AddPageBreakCommand();

		public static readonly RemovePageBreakCommand RemovePageBreak = new RemovePageBreakCommand();

		public static readonly SetFilterRangeCommand SetFilterRange = new SetFilterRangeCommand();

		public static readonly SetFilterCommand SetFilter = new SetFilterCommand();

		public static readonly RemoveFilterCommand RemoveFilter = new RemoveFilterCommand();

		public static readonly MoveFilterCommand MoveFilter = new MoveFilterCommand();

		public static readonly RearrangeFilterRowsCommand RearrangeFilterRows = new RearrangeFilterRowsCommand();

		public static readonly SetSortRangeCommand SetSortRange = new SetSortRangeCommand();

		public static readonly SetSortCommand SetSort = new SetSortCommand();

		public static readonly ClearSortCommand ClearSort = new ClearSortCommand();

		public static readonly MoveSortConditionCommand MoveSortCondition = new MoveSortConditionCommand();

		public static readonly RemoveSortConditionCommand RemoveSortCondition = new RemoveSortConditionCommand();

		public static readonly AddStyleCommand AddStyle = new AddStyleCommand();

		public static readonly RemoveStyleCommand RemoveStyle = new RemoveStyleCommand();

		public static readonly SetStylePropertyCommand SetStyleProperty = new SetStylePropertyCommand();

		public static readonly SetWorkbookThemeCommand SetWorkbookTheme = new SetWorkbookThemeCommand();

		public static readonly SetStylePropertyGroupIncludedCommand SetStylePropertyIsIncluded = new SetStylePropertyGroupIncludedCommand();

		public static readonly ReapplyCellStyleCommand ReapplyCellStyleCommand = new ReapplyCellStyleCommand();
	}
}
