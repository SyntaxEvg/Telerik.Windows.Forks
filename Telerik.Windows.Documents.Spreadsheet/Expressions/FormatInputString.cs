using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	class FormatInputString : UserInputStringBase
	{
		public FormatInputString(string value)
		{
			this.stringValue = value;
		}

		public override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			return this.stringValue;
		}

		public override UserInputStringBase Clone()
		{
			return new FormatInputString(this.stringValue);
		}

		readonly string stringValue;
	}
}
