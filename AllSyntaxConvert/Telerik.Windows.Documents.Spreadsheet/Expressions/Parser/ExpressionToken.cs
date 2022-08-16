using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Parser
{
	class ExpressionToken
	{
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		public ExpressionTokenType TokenType
		{
			get
			{
				return this.tokenType;
			}
		}

		public double NumberValue
		{
			get
			{
				return this.numberValue.Value;
			}
		}

		public ExpressionToken(ExpressionTokenType type, string value)
		{
			Guard.ThrowExceptionIfNull<string>(value, "value");
			this.tokenType = type;
			this.value = value;
		}

		public ExpressionToken(double value, string valueAsString)
			: this(ExpressionTokenType.Number, valueAsString)
		{
			this.numberValue = new double?(value);
		}

		public override string ToString()
		{
			return this.Value;
		}

		public static readonly ExpressionToken UnaryPlus = new ExpressionToken(ExpressionTokenType.UnaryPlus, "+");

		public static readonly ExpressionToken UnaryMinus = new ExpressionToken(ExpressionTokenType.UnaryMinus, "-");

		public static readonly ExpressionToken Percent = new ExpressionToken(ExpressionTokenType.Percent, "%");

		public static readonly ExpressionToken Plus = new ExpressionToken(ExpressionTokenType.Plus, "+");

		public static readonly ExpressionToken Minus = new ExpressionToken(ExpressionTokenType.Minus, "-");

		public static readonly ExpressionToken Multiply = new ExpressionToken(ExpressionTokenType.Multiply, "*");

		public static readonly ExpressionToken Divide = new ExpressionToken(ExpressionTokenType.Divide, "/");

		public static readonly ExpressionToken Power = new ExpressionToken(ExpressionTokenType.Power, "^");

		public static readonly ExpressionToken Equal = new ExpressionToken(ExpressionTokenType.Equal, "=");

		public static readonly ExpressionToken LessThan = new ExpressionToken(ExpressionTokenType.LessThan, "<");

		public static readonly ExpressionToken LessThanOrEqualTo = new ExpressionToken(ExpressionTokenType.LessThanOrEqualTo, "<=");

		public static readonly ExpressionToken GreaterThan = new ExpressionToken(ExpressionTokenType.GreaterThan, ">");

		public static readonly ExpressionToken GreaterThanOrEqualTo = new ExpressionToken(ExpressionTokenType.GreaterThanOrEqualTo, ">=");

		public static readonly ExpressionToken NotEqual = new ExpressionToken(ExpressionTokenType.NotEqual, "<>");

		public static readonly ExpressionToken Ampersand = new ExpressionToken(ExpressionTokenType.Ampersand, "&");

		public static readonly ExpressionToken Range = new ExpressionToken(ExpressionTokenType.Range, ":");

		public static readonly ExpressionToken LeftParenthesis = new ExpressionToken(ExpressionTokenType.LeftParenthesis, "(");

		public static readonly ExpressionToken RightParenthesis = new ExpressionToken(ExpressionTokenType.RightParenthesis, ")");

		public static readonly ExpressionToken True = new ExpressionToken(ExpressionTokenType.True, "TRUE");

		public static readonly ExpressionToken False = new ExpressionToken(ExpressionTokenType.False, "FALSE");

		internal static readonly ExpressionToken MissingValue = new ExpressionToken(ExpressionTokenType.MissingValue, string.Empty);

		internal static readonly ExpressionToken FunctionStart = new ExpressionToken(ExpressionTokenType.FunctionStart, "f");

		public static readonly ExpressionToken NullError = new ExpressionToken(ExpressionTokenType.Error, "#NULL!");

		public static readonly ExpressionToken DivisionByZeroError = new ExpressionToken(ExpressionTokenType.Error, "#DIV/0!");

		public static readonly ExpressionToken ValueError = new ExpressionToken(ExpressionTokenType.Error, "#VALUE!");

		public static readonly ExpressionToken ReferenceError = new ExpressionToken(ExpressionTokenType.Error, "#REF!");

		public static readonly ExpressionToken NameError = new ExpressionToken(ExpressionTokenType.Error, "#NAME?");

		public static readonly ExpressionToken NumberError = new ExpressionToken(ExpressionTokenType.Error, "#NUM!");

		public static readonly ExpressionToken NotAvailableError = new ExpressionToken(ExpressionTokenType.Error, "#N/A");

		readonly ExpressionTokenType tokenType;

		readonly string value;

		readonly double? numberValue;
	}
}
