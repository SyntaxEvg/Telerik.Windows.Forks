using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders
{
	public interface ITextBasedWorkbookFormatProvider : IWorkbookFormatProvider
	{
		Workbook Import(string input);

		string Export(Workbook workbook);
	}
}
