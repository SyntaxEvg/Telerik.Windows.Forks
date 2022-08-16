using System;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class StringExpression : ConstantExpression<string>
	{
		public StringExpression(string value)
			: base(value)
		{
			Guard.ThrowExceptionIfNull<string>(value, "value");
		}

		internal override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("\"");
			stringBuilder.Append(base.ToString(cultureInfo).Replace("\"", "\"\""));
			stringBuilder.Append("\"");
			return stringBuilder.ToString();
		}

		public static readonly StringExpression Empty = new StringExpression(string.Empty);
	}
}
