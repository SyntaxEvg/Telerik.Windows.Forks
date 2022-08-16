using System;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetCellBordersCommandContext : SetBordersCommandContextBase
	{
		public SetCellBordersCommandContext(Worksheet worksheet, CellRange cellRange, CellBorders newValue)
			: base(worksheet, cellRange, newValue)
		{
		}

		public override ICompressedList<CellBorder> GetBorderProperty(IPropertyDefinition<CellBorder> borderProperty, CellRange range)
		{
			return base.Worksheet.Cells.PropertyBag.GetPropertyValue<CellBorder>(borderProperty, range);
		}

		public override void SetBorderProperty(IPropertyDefinition<CellBorder> borderProperty, CellRange range, ICompressedList<CellBorder> values)
		{
			base.Worksheet.Cells.PropertyBag.SetPropertyValue<CellBorder>(borderProperty, range, values);
		}

		protected override void SetValuesInRange(CellRange range, ICompressedList<CellBorder> collection, CellBorder value)
		{
			foreach (LongRange longRange in WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(range))
			{
				collection.SetValue(longRange.Start, longRange.End, value);
			}
		}

		protected override void ClearValuesInRange(CellRange range, ICompressedList collection)
		{
			foreach (LongRange longRange in WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(range))
			{
				collection.ClearValue(longRange.Start, longRange.End);
			}
		}
	}
}
