using System;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	class ColumnsPropertyBag : RowColumnPropertyBagBase
	{
		protected override WidthHeightBase HiddenSize
		{
			get
			{
				return ColumnsPropertyBag.HiddenWidth;
			}
		}

		protected override long ToItemIndex
		{
			get
			{
				return (long)(SpreadsheetDefaultValues.ColumnCount - 1);
			}
		}

		public ColumnsPropertyBag()
		{
			base.RegisterProperty<ColumnWidth>(ColumnsPropertyBag.WidthProperty);
		}

		internal override int GetRowColumnIndex(int rowIndex, int columnIndex)
		{
			return columnIndex;
		}

		public void CopyPropertiesFrom(ColumnsPropertyBag fromProperties, long fromIndex, long toIndex)
		{
			base.CopyPropertiesFrom(fromProperties, fromIndex, toIndex);
		}

		public ICompressedList<ColumnWidth> GetColumnWidthPropertyValueRespectingHidden()
		{
			return base.GetSizePropertyValueRespectingHidden<ColumnWidth>(ColumnsPropertyBag.WidthProperty);
		}

		public static readonly IPropertyDefinition<ColumnWidth> WidthProperty = new PropertyDefinition<ColumnWidth>("ColumnWidth", true, StylePropertyGroup.None, new ColumnWidth(SpreadsheetDefaultValues.DefaultColumnWidth, false), true);

		static readonly ColumnWidth HiddenWidth = new ColumnWidth(0.0, true);
	}
}
