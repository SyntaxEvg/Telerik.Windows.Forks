using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class NumberExpression : ConstantExpression<double>
	{
		public int IntValue
		{
			get
			{
				return (int)base.Value;
			}
		}

		public NumberExpression(double value)
			: base(value)
		{
			Guard.ThrowExceptionIfInvalidDouble(value, "value");
		}

		internal override string GetValueAsString(SpreadsheetCultureHelper cultureInfo)
		{
			return this.ToString(cultureInfo);
		}

		internal override string ToString(SpreadsheetCultureHelper cultureInfo)
		{
			return cultureInfo.ToString(base.Value);
		}

		internal static RadExpression CreateValidNumberOrErrorExpression(double number)
		{
			if (double.IsInfinity(number))
			{
				return ErrorExpressions.NumberError;
			}
			if (double.IsNaN(number))
			{
				return ErrorExpressions.DivisionByZero;
			}
			return new NumberExpression(number);
		}

		public static readonly NumberExpression Zero = new NumberExpression(0.0);

		public static readonly NumberExpression PI = new NumberExpression(3.141592653589793);

		public static readonly NumberExpression E = new NumberExpression(2.718281828459045);
	}
}
