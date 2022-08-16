using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders
{
	public interface IBinaryWorkbookFormatProvider : IWorkbookFormatProvider
	{
		Workbook Import(byte[] input);

		byte[] Export(Workbook workbook);
	}
}
