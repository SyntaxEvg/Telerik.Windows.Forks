using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders
{
	public interface IWorkbookFormatProvider
	{
		string Name { get; }

		string FilesDescription { get; }

		IEnumerable<string> SupportedExtensions { get; }

		bool CanImport { get; }

		bool CanExport { get; }

		Workbook Import(Stream input);

		void Export(Workbook workbook, Stream output);
	}
}
