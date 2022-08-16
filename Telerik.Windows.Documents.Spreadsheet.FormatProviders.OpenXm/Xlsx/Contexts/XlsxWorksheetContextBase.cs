using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	abstract class XlsxWorksheetContextBase
	{
		public XlsxWorksheetContextBase(Worksheet worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			this.worksheet = worksheet;
		}

		public Worksheet Worksheet
		{
			get
			{
				return this.worksheet;
			}
		}

		readonly Worksheet worksheet;
	}
}
