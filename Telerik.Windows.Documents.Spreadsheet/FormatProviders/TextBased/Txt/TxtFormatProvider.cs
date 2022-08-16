using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Core;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Txt
{
	public class TxtFormatProvider : DelimitedValuesFormatProviderBase
	{
		public override string Name
		{
			get
			{
				return TxtFormatProvider.NAME;
			}
		}

		public override string FilesDescription
		{
			get
			{
				return TxtFormatProvider.FILE_DESCRIPTION;
			}
		}

		public override IEnumerable<string> SupportedExtensions
		{
			get
			{
				return TxtFormatProvider.SUPPORTED_EXTENSIONS;
			}
		}

		public override CsvSettings Settings
		{
			get
			{
				return this.csvSettings;
			}
		}

		public TxtFormatProvider()
		{
			this.csvSettings = new CsvSettings
			{
				Delimiter = '\t'
			};
		}

		static readonly string NAME = "TxtFormatProvider";

		static readonly string FILE_DESCRIPTION = "Text (Tab delimited)";

		static readonly IEnumerable<string> SUPPORTED_EXTENSIONS = new string[] { ".txt" };

		readonly CsvSettings csvSettings;
	}
}
