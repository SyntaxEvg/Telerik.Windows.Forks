using System;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	abstract class SetPropertyInRangeCommandContext<T> : SetPropertyCommandContextBase<T>
	{
		public ValueBox<T> NewValue
		{
			get
			{
				return this.newValue;
			}
		}

		public SetPropertyInRangeCommandContext(Worksheet worksheet, IPropertyDefinition<T> property, ValueBox<T> newValue)
			: base(worksheet, property)
		{
			this.newValue = newValue;
		}

		readonly ValueBox<T> newValue;
	}
}
