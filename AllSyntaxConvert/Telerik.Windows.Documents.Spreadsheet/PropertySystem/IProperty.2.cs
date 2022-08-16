using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	interface IProperty<T> : IProperty
	{
		T GetDefaultValue();

		RangePropertyValue<T> GetValue(CellRange cellRange);

		void SetValue(CellRange cellRange, T value);
	}
}
