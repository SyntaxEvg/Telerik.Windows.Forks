using System;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetColumnBordersCommandContext : SetBordersCommandContextBase
	{
		public SetColumnBordersCommandContext(Worksheet worksheet, CellRange cellRange, CellBorders newValue)
			: base(worksheet, cellRange, newValue)
		{
		}

		public override ICompressedList<CellBorder> GetBorderProperty(IPropertyDefinition<CellBorder> borderProperty, CellRange range)
		{
			return base.Worksheet.Columns.PropertyBag.GetPropertyValue<CellBorder>(borderProperty, range.FromIndex.ColumnIndex, range.ToIndex.ColumnIndex);
		}

		public override void SetBorderProperty(IPropertyDefinition<CellBorder> borderProperty, CellRange range, ICompressedList<CellBorder> values)
		{
			ICompressedList<CellBorder> value = values.GetValue((long)range.FromIndex.ColumnIndex, (long)range.ToIndex.ColumnIndex);
			IProperty<CellBorder> property;
			base.Worksheet.Columns.TryGetProperyFromPropertyDefinition<CellBorder>(borderProperty, out property);
			foreach (Range<long, CellBorder> range2 in value.GetNonDefaultRanges())
			{
				base.Worksheet.Columns[(int)range2.Start, (int)range2.End].SetPropertyValue<CellBorder>(property, range2.Value);
			}
		}

		protected override void SetValuesInRange(CellRange range, ICompressedList<CellBorder> collection, CellBorder value)
		{
			collection.SetValue((long)range.FromIndex.ColumnIndex, (long)range.ToIndex.ColumnIndex, value);
		}

		protected override void ClearValuesInRange(CellRange range, ICompressedList collection)
		{
			collection.ClearValue((long)range.FromIndex.ColumnIndex, (long)range.ToIndex.ColumnIndex);
		}
	}
}
