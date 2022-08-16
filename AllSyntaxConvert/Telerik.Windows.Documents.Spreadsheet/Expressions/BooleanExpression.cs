using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class BooleanExpression : ConstantExpression<bool>
	{
		BooleanExpression(bool value)
			: base(value)
		{
		}

		internal override string GetValueAsString(SpreadsheetCultureHelper cultureInfo)
		{
			if (base.Value)
			{
				return "TRUE";
			}
			return "FALSE";
		}

		public static readonly BooleanExpression True = new BooleanExpression(true);

		public static readonly BooleanExpression False = new BooleanExpression(false);
	}
}
