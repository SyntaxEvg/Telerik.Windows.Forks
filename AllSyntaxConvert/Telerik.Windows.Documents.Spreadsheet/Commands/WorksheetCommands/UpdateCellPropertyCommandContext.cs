using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class UpdateCellPropertyCommandContext<T> : UpdatePropertyInRangeCommandContext<T>
	{
		public CellRange CellRange
		{
			get
			{
				return this.cellRange;
			}
		}

		public UpdateCellPropertyCommandContext(Worksheet worksheet, IPropertyDefinition<T> property, CellIndex cellIndex, Func<T, T> newValueTransform)
			: this(worksheet, property, cellIndex.ToCellRange(), newValueTransform)
		{
		}

		public UpdateCellPropertyCommandContext(Worksheet worksheet, IPropertyDefinition<T> property, CellRange cellRange, Func<T, T> newValueTransform)
			: base(worksheet, property, newValueTransform)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			this.cellRange = cellRange;
		}

		readonly CellRange cellRange;
	}
}
