using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	class ArrayListSeparatorInputString : UserInputStringBase
	{
		public override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			return cultureInfo.ArrayListSeparator;
		}

		public override UserInputStringBase Clone()
		{
			return new ArrayListSeparatorInputString();
		}
	}
}
