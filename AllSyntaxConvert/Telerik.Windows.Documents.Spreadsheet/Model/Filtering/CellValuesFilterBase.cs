using System;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Filtering
{
	public abstract class CellValuesFilterBase : FilterBase<ICellValue>
	{
		protected override IPropertyDefinition<ICellValue> PropertyDefinition
		{
			get
			{
				return CellPropertyDefinitions.ValueProperty;
			}
		}

		protected CellValuesFilterBase(int relativeColumnIndex)
			: base(relativeColumnIndex)
		{
		}
	}
}
