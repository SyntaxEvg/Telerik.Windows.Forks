using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class DifferentialFormatInfo
	{
		public FontInfo? FontInfo { get; set; }

		public IFill Fill { get; set; }
	}
}
