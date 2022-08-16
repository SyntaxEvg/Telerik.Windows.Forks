using System;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetCellPropertyCommandContext<T> : SetPropertyInRangeCommandContext<T>
	{
		public CellRange CellRange
		{
			get
			{
				return this.cellRange;
			}
		}

		public SetCellPropertyCommandContext(Worksheet sheet, IPropertyDefinition<T> property, CellRange cellRange, T newValue)
			: this(sheet, property, cellRange, new ValueBox<T>(newValue))
		{
		}

		protected SetCellPropertyCommandContext(Worksheet sheet, IPropertyDefinition<T> property, CellRange cellRange, ValueBox<T> newValue)
			: base(sheet, property, newValue)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			this.cellRange = cellRange;
		}

		internal override void InvalidateLayout()
		{
			if (base.Property == CellPropertyDefinitions.ValueProperty)
			{
				base.Workbook.InvalidateLayout();
				return;
			}
			base.InvalidateLayout();
		}

		readonly CellRange cellRange;
	}
}
