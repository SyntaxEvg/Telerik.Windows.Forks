using System;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	class ColumnProperty<T> : RowColumnProperty<T, SetRowColumnPropertyCommandContext<T>>
	{
		protected override RowColumnPropertyBagBase PropertyBag
		{
			get
			{
				return base.Worksheet.Columns.PropertyBag;
			}
		}

		public ColumnProperty(Worksheet worksheet, IPropertyDefinition<T> propertyDefinition)
			: base(worksheet, propertyDefinition)
		{
		}

		internal override int GetRowColumnIndex(CellIndex index)
		{
			return index.ColumnIndex;
		}

		protected override bool ValidateCellRange(CellRange cellRange)
		{
			return cellRange.RowCount == SpreadsheetDefaultValues.RowCount;
		}

		protected override UndoableWorksheetCommandBase<SetRowColumnPropertyCommandContext<T>> CreateSetPropertyCommand()
		{
			return new SetColumnPropertyCommand<T>();
		}
	}
}
