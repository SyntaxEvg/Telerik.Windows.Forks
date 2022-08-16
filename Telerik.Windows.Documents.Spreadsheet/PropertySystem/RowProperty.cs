using System;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	class RowProperty<T> : RowColumnProperty<T, SetRowColumnPropertyCommandContext<T>>
	{
		protected override RowColumnPropertyBagBase PropertyBag
		{
			get
			{
				return base.Worksheet.Rows.PropertyBag;
			}
		}

		public RowProperty(Worksheet worksheet, IPropertyDefinition<T> propertyDefinition)
			: base(worksheet, propertyDefinition)
		{
		}

		internal override int GetRowColumnIndex(CellIndex index)
		{
			return index.RowIndex;
		}

		protected override bool ValidateCellRange(CellRange cellRange)
		{
			return cellRange.ColumnCount == SpreadsheetDefaultValues.ColumnCount;
		}

		protected override UndoableWorksheetCommandBase<SetRowColumnPropertyCommandContext<T>> CreateSetPropertyCommand()
		{
			return new SetRowPropertyCommand<T>();
		}
	}
}
