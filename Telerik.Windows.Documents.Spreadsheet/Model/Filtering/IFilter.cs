using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Filtering
{
	public interface IFilter
	{
		int RelativeColumnIndex { get; }

		object GetValue(Cells cells, int rowIndex, int columnIndex);

		bool ShouldShowValue(object value);
	}
}
