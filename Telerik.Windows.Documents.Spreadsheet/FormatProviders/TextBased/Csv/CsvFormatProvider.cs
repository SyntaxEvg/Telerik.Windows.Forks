using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Core;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Csv
{
	public class CsvFormatProvider : DelimitedValuesFormatProviderBase
	{
		public override string Name
		{
			get
			{
				return CsvFormatProvider.NAME;
			}
		}

		public override string FilesDescription
		{
			get
			{
				return CsvFormatProvider.FILE_DESCRIPTION;
			}
		}

		public override IEnumerable<string> SupportedExtensions
		{
			get
			{
				return CsvFormatProvider.SUPPORTED_EXTENSIONS;
			}
		}

		public override CsvSettings Settings
		{
			get
			{
				return this.csvSettings;
			}
		}

		public CsvFormatProvider()
		{
			this.csvSettings = new CsvSettings();
		}

		static readonly string NAME = "CsvFormatProvider";

		static readonly string FILE_DESCRIPTION = "CSV (Comma delimited)";

		static readonly IEnumerable<string> SUPPORTED_EXTENSIONS = new string[] { ".csv" };

		readonly CsvSettings csvSettings;
	}
}
