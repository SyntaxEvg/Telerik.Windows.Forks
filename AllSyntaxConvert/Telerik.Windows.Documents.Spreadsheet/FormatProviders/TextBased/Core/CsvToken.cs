using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.TextBased.Core
{
	class CsvToken
	{
		public CsvTokenType TokenType
		{
			get
			{
				return this.tokenType;
			}
		}

		public string Value
		{
			get
			{
				return this.value;
			}
		}

		public CsvToken(CsvTokenType tokenType, string value)
		{
			Guard.ThrowExceptionIfNullOrEmpty(value, "value");
			this.tokenType = tokenType;
			this.value = value;
		}

		readonly CsvTokenType tokenType;

		readonly string value;
	}
}
