using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class ClearCellPropertyCommandContext<T> : SetCellPropertyCommandContext<T>
	{
		public ClearCellPropertyCommandContext(Worksheet worksheet, IPropertyDefinition<T> property, CellRange cellRange)
			: base(worksheet, property, cellRange, null)
		{
		}
	}
}
