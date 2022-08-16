using System;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	class RowsPropertyBag : RowColumnPropertyBagBase
	{
		protected override WidthHeightBase HiddenSize
		{
			get
			{
				return RowsPropertyBag.HiddenHeight;
			}
		}

		protected override long ToItemIndex
		{
			get
			{
				return (long)(SpreadsheetDefaultValues.RowCount - 1);
			}
		}

		public RowsPropertyBag()
		{
			base.RegisterProperty<RowHeight>(RowsPropertyBag.HeightProperty);
		}

		internal override int GetRowColumnIndex(int rowIndex, int columnIndex)
		{
			return rowIndex;
		}

		public void CopyPropertiesFrom(RowsPropertyBag fromProperties, long fromIndex, long toIndex)
		{
			base.CopyPropertiesFrom(fromProperties, fromIndex, toIndex);
		}

		public ICompressedList<RowHeight> GetRowHeightPropertyValueRespectingHidden()
		{
			return base.GetSizePropertyValueRespectingHidden<RowHeight>(RowsPropertyBag.HeightProperty);
		}

		internal void Sort(CellRange sortedRange, int[] sortedIndexes)
		{
			long fromIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(sortedRange.FromIndex.RowIndex, 0);
			long toIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(sortedRange.ToIndex.RowIndex, 0);
			base.GetPropertyValueCollection<RowHeight>(RowsPropertyBag.HeightProperty).Sort(fromIndex, toIndex, sortedIndexes);
			this.OnPropertyChanged(RowsPropertyBag.HeightProperty, fromIndex, toIndex);
		}

		public static readonly IPropertyDefinition<RowHeight> HeightProperty = new PropertyDefinition<RowHeight>("RowHeight", true, StylePropertyGroup.None, new RowHeight(SpreadsheetDefaultValues.DefaultRowHeight, false), true);

		static readonly RowHeight HiddenHeight = new RowHeight(0.0, true);
	}
}
