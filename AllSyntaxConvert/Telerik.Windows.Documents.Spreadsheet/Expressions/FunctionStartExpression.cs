using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	class FunctionStartExpression : ConstantExpression
	{
		internal override string GetValueAsString(SpreadsheetCultureHelper cultureInfo)
		{
			return string.Empty;
		}

		internal override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			return string.Empty;
		}
	}
}
