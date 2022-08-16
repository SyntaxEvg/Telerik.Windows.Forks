using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	class CellProperty<T> : PropertyBase<T, SetCellPropertyCommandContext<T>>
	{
		protected CellsPropertyBag PropertyBag
		{
			get
			{
				return base.Worksheet.Cells.PropertyBag;
			}
		}

		public CellProperty(Worksheet worksheet, IPropertyDefinition<T> propertyDefinition)
			: base(worksheet, propertyDefinition)
		{
		}

		protected override UndoableWorksheetCommandBase<SetCellPropertyCommandContext<T>> CreateSetPropertyCommand()
		{
			return new SetCellPropertyCommand<T>();
		}

		protected override SetCellPropertyCommandContext<T> CreateSetPropertyCommandContext(Worksheet worksheet, IPropertyDefinition<T> property, CellRange cellRange, T value)
		{
			return new SetCellPropertyCommandContext<T>(base.Worksheet, property, cellRange, value);
		}

		protected override SetCellPropertyCommandContext<T> CreateClearPropertyCommandContext(Worksheet worksheet, IPropertyDefinition<T> property, CellRange cellRange)
		{
			return new ClearCellPropertyCommandContext<T>(base.Worksheet, property, cellRange);
		}

		public override RangePropertyValue<T> GetValue(CellRange cellRange)
		{
			bool isIndeterminate = false;
			IEnumerable<LongRange> enumerable = WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(cellRange);
			bool flag = true;
			T t = default(T);
			T value;
			foreach (LongRange longRange in enumerable)
			{
				ICompressedList<T> propertyValueRespectingStyle = base.Worksheet.Cells.PropertyBag.GetPropertyValueRespectingStyle<T>(base.PropertyDefinition, base.Worksheet, longRange.Start, longRange.End);
				foreach (Range<long, T> range in propertyValueRespectingStyle)
				{
					if (flag)
					{
						t = range.Value;
						flag = false;
					}
					else if (!TelerikHelper.EqualsOfT<T>(t, range.Value))
					{
						value = base.GetDefaultValue();
						isIndeterminate = true;
					}
				}
			}
			value = t;
			return new RangePropertyValue<T>(isIndeterminate, value);
		}

		protected override bool ValidateCellRange(CellRange cellRange)
		{
			return true;
		}
	}
}
