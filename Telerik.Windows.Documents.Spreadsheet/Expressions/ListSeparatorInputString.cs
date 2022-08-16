using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	class ListSeparatorInputString : UserInputStringBase
	{
		public override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			return cultureInfo.ListSeparator;
		}

		public override UserInputStringBase Clone()
		{
			return new ListSeparatorInputString();
		}
	}
}
