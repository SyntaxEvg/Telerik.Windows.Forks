using System;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetRowColumnPropertyCommandContext<T> : SetPropertyInRangeCommandContext<T>
	{
		public int FromIndex
		{
			get
			{
				return this.fromIndex;
			}
		}

		public int ToIndex
		{
			get
			{
				return this.toIndex;
			}
		}

		public SetRowColumnPropertyCommandContext(Worksheet worksheet, IPropertyDefinition<T> property, int fromIndex, int toIndex, T newValue)
			: this(worksheet, property, fromIndex, toIndex, new ValueBox<T>(newValue))
		{
		}

		protected SetRowColumnPropertyCommandContext(Worksheet worksheet, IPropertyDefinition<T> property, int fromIndex, int toIndex, ValueBox<T> newValue)
			: base(worksheet, property, newValue)
		{
			Guard.ThrowExceptionIfLessThan<int>(fromIndex, toIndex, "toIndex");
			this.fromIndex = fromIndex;
			this.toIndex = toIndex;
		}

		readonly int fromIndex;

		readonly int toIndex;
	}
}
