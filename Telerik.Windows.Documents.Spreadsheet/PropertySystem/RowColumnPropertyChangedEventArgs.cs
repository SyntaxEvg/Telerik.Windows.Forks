using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	public class RowColumnPropertyChangedEventArgs : EventArgs
	{
		public IPropertyDefinition Property
		{
			get
			{
				return this.property;
			}
		}

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

		public RowColumnPropertyChangedEventArgs(IPropertyDefinition property, int fromIndex, int toIndex)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition>(property, "property");
			Guard.ThrowExceptionIfLessThan<int>(0, fromIndex, "fromIndex");
			Guard.ThrowExceptionIfLessThan<int>(0, toIndex, "toIndex");
			this.property = property;
			this.fromIndex = fromIndex;
			this.toIndex = toIndex;
		}

		readonly IPropertyDefinition property;

		readonly int fromIndex;

		readonly int toIndex;
	}
}
