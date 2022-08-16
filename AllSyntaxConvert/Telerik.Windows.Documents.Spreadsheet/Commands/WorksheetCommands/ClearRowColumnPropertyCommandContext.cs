using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class ClearRowColumnPropertyCommandContext<T> : SetRowColumnPropertyCommandContext<T>
	{
		public ClearRowColumnPropertyCommandContext(Worksheet worksheet, IPropertyDefinition<T> property, int fromIndex, int toIndex)
			: base(worksheet, property, fromIndex, toIndex, null)
		{
		}
	}
}
