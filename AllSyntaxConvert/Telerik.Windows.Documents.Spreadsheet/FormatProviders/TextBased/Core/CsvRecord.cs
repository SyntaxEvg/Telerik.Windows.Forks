using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Core
{
	class CsvRecord
	{
		public CsvRecord(IEnumerable<string> values)
		{
			this.values = values;
		}

		public IEnumerable<string> GetValues()
		{
			return this.values;
		}

		readonly IEnumerable<string> values;
	}
}
