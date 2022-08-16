using System;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetRowBordersCommandContext : SetBordersCommandContextBase
	{
		public SetRowBordersCommandContext(Worksheet worksheet, CellRange cellRange, CellBorders newValue)
			: base(worksheet, cellRange, newValue)
		{
		}

		public override ICompressedList<CellBorder> GetBorderProperty(IPropertyDefinition<CellBorder> borderProperty, CellRange range)
		{
			return base.Worksheet.Rows.PropertyBag.GetPropertyValue<CellBorder>(borderProperty, range.FromIndex.RowIndex, range.ToIndex.RowIndex);
		}

		public override void SetBorderProperty(IPropertyDefinition<CellBorder> borderProperty, CellRange range, ICompressedList<CellBorder> values)
		{
			ICompressedList<CellBorder> value = values.GetValue((long)range.FromIndex.RowIndex, (long)range.ToIndex.RowIndex);
			IProperty<CellBorder> property;
			base.Worksheet.Rows.TryGetProperyFromPropertyDefinition<CellBorder>(borderProperty, out property);
			foreach (Range<long, CellBorder> range2 in value.GetNonDefaultRanges())
			{
				base.Worksheet.Rows[(int)range2.Start, (int)range2.End].SetPropertyValue<CellBorder>(property, range2.Value);
			}
		}

		protected override void SetValuesInRange(CellRange range, ICompressedList<CellBorder> collection, CellBorder value)
		{
			collection.SetValue((long)range.FromIndex.RowIndex, (long)range.ToIndex.RowIndex, value);
		}

		protected override void ClearValuesInRange(CellRange range, ICompressedList collection)
		{
			collection.ClearValue((long)range.FromIndex.RowIndex, (long)range.ToIndex.RowIndex);
		}
	}
}
