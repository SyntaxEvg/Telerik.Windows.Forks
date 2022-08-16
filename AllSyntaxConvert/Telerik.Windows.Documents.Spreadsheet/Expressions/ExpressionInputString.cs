using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	class ExpressionInputString : UserInputStringBase<RadExpression>
	{
		public ExpressionInputString(RadExpression expression)
			: base(expression)
		{
		}

		public override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			return base.Expression.ToString(cultureInfo);
		}

		public override UserInputStringBase Clone()
		{
			return new ExpressionInputString(base.Expression);
		}
	}
}
