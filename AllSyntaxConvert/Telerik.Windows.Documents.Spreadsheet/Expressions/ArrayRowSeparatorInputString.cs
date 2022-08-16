using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	class ArrayRowSeparatorInputString : UserInputStringBase
	{
		public override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			return cultureInfo.ArrayRowSeparator;
		}

		public override UserInputStringBase Clone()
		{
			return new ArrayRowSeparatorInputString();
		}
	}
}
