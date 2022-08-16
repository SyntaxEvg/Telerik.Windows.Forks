using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Sorting
{
	public class SortValue
	{
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		public object Value
		{
			get
			{
				return this.value;
			}
		}

		internal SortValue(int index, object value)
		{
			this.index = index;
			this.value = value;
		}

		readonly object value;

		readonly int index;
	}
}
