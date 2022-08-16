using System;
using System.Globalization;
using System.Text;

namespace Telerik.Windows.Documents.Globalization
{
	class OpenXmlCultureInfo
	{
		public OpenXmlCultureInfo()
			: this(CultureInfo.CurrentCulture)
		{
		}

		public OpenXmlCultureInfo(CultureInfo cultureInfo)
		{
			this.cultureInfo = cultureInfo;
		}

		public CultureInfo CultureInfo
		{
			get
			{
				return this.cultureInfo;
			}
		}

		public string NumberDecimalSeparator
		{
			get
			{
				return this.cultureInfo.NumberFormat.NumberDecimalSeparator;
			}
		}

		public string NumberGroupSeparator
		{
			get
			{
				return this.cultureInfo.NumberFormat.NumberGroupSeparator;
			}
		}

		public string CurrencyDecimalSeparator
		{
			get
			{
				return this.cultureInfo.NumberFormat.CurrencyDecimalSeparator;
			}
		}

		public string CurrencyGroupSeparator
		{
			get
			{
				return this.cultureInfo.NumberFormat.CurrencyGroupSeparator;
			}
		}

		public string ListSeparator
		{
			get
			{
				string listSeparator = this.cultureInfo.TextInfo.ListSeparator;
				if (listSeparator == this.NumberDecimalSeparator && listSeparator == ",")
				{
					return ";";
				}
				return listSeparator;
			}
		}

		public string ArrayListSeparator
		{
			get
			{
				return this.ListSeparator;
			}
		}

		public string ArrayRowSeparator
		{
			get
			{
				if (this.ArrayListSeparator == ";")
				{
					return "\\";
				}
				return ";";
			}
		}

		public static string TextQualifier
		{
			get
			{
				return "\"";
			}
		}

		public string TimeSeparator
		{
			get
			{
				return this.cultureInfo.DateTimeFormat.TimeSeparator;
			}
		}

		public string DateSeparator
		{
			get
			{
				string text = string.Empty;
				text = this.cultureInfo.DateTimeFormat.DateSeparator;
				return (text == "/") ? "-" : text;
			}
		}

		public static string ClearFormulaValue(string formula)
		{
			StringBuilder stringBuilder = new StringBuilder(formula.Trim());
			if (stringBuilder.Length > 0 && OpenXmlCultureInfo.IsCharEqualTo(stringBuilder[0], new string[] { "=" }))
			{
				stringBuilder.Remove(0, 1);
			}
			return stringBuilder.ToString();
		}

		public static string PrepareFormulaValue(string formula)
		{
			formula = formula.Trim();
			if (!formula.StartsWith("="))
			{
				formula = "=" + formula;
			}
			return formula;
		}

		public bool IsExponent(char ch)
		{
			return char.ToUpper(ch, this.CultureInfo).ToString() == "E";
		}

		public bool IsLiteral(string input, params string[] literals)
		{
			string a = input.ToUpper(this.cultureInfo);
			for (int i = 0; i < literals.Length; i++)
			{
				if (a == literals[i])
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsCharEqualTo(char ch, params string[] values)
		{
			string a = ch.ToString();
			for (int i = 0; i < values.Length; i++)
			{
				if (a == values[i])
				{
					return true;
				}
			}
			return false;
		}

		public bool TryParseGeneral(string value, out double result)
		{
			value = value.Trim(new char[] { ' ' });
			NumberStyles style = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;
			return double.TryParse(value, style, this.cultureInfo, out result);
		}

		public bool TryParseNumber(string value, out double result)
		{
			result = 0.0;
			bool flag = value.Contains(this.NumberGroupSeparator);
			return flag && double.TryParse(value, NumberStyles.Number, this.cultureInfo, out result) && this.ApplyAdditionalNumberConstraints(value);
		}

		public bool TryParseDouble(string value, out double result)
		{
			return this.TryParseGeneral(value, out result) || this.TryParseNumber(value, out result);
		}

		public bool TryParseCurrency(string value, out double result)
		{
			result = 0.0;
			NumberStyles style = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowCurrencySymbol;
			bool flag = value.Contains(this.cultureInfo.NumberFormat.CurrencySymbol);
			return flag && double.TryParse(value, style, this.cultureInfo, out result) && OpenXmlCultureInfo.CheckGroupSeparatorsPositionedCorrectly(value, this.CurrencyGroupSeparator, this.CurrencyDecimalSeparator, this.CultureInfo.NumberFormat.CurrencyGroupSizes);
		}

		public bool TryParseDateTime(string value, out DateTime result)
		{
			return DateTime.TryParse(value, this.cultureInfo, DateTimeStyles.NoCurrentDateDefault, out result) && !value.Contains(Environment.NewLine) && (value.Contains(this.DateSeparator) || value.Contains("/") || value.Contains("-") || value.Contains(" "));
		}

		public static bool TryParseTimeSpan(string value, out TimeSpan timeSpan)
		{
			return TimeSpan.TryParse(value, out timeSpan) && !value.Contains(Environment.NewLine);
		}

		public bool TryParsePercent(string value, out double result)
		{
			result = 0.0;
			if (value.IndexOf(this.cultureInfo.NumberFormat.PercentSymbol) == -1)
			{
				return false;
			}
			string s = value.Replace(this.cultureInfo.NumberFormat.PercentSymbol, string.Empty);
			bool result2 = double.TryParse(s, NumberStyles.Any, this.cultureInfo, out result);
			result /= 100.0;
			return result2;
		}

		public bool TryParseScientific(string value, out double result)
		{
			NumberStyles style = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent;
			return double.TryParse(value, style, this.cultureInfo, out result) && OpenXmlCultureInfo.CheckGroupSeparatorsPositionedCorrectly(value, this.NumberGroupSeparator, this.NumberDecimalSeparator, this.CultureInfo.NumberFormat.NumberGroupSizes) && !value.Contains(Environment.NewLine);
		}

		public bool IsDecimalSeparator(char ch)
		{
			return ch == this.NumberDecimalSeparator[0] && this.NumberDecimalSeparator.Length == 1;
		}

		public string ToString(double value)
		{
			return value.ToString(this.CultureInfo);
		}

		static bool CheckGroupSeparatorsPositionedCorrectly(string value, string groupSeparator, string decimalSeparator, int[] groupSizes)
		{
			string text = OpenXmlCultureInfo.GetValueWithoutFractionalPart(value, decimalSeparator);
			text = OpenXmlCultureInfo.RemoveLeadingAndTrailingNonDigitSymbols(text, groupSeparator, decimalSeparator);
			string[] array = text.Split(new string[] { groupSeparator }, StringSplitOptions.None);
			if (array.Length == 1)
			{
				return true;
			}
			int i;
			Func<int, int, bool> comparer = (int a, int b) => a >= b;
			Func<int, int, bool> comparer2 = (int a, int b) => a == b;
			if (!OpenXmlCultureInfo.CompareValues(groupSizes, array[0].Length, comparer))
			{
				return false;
			}
			for (i = 1; i < array.Length; i++)
			{
				if (!OpenXmlCultureInfo.CompareValues(groupSizes, array[i].Length, comparer2))
				{
					return false;
				}
			}
			return true;
		}

		static bool CompareValues(int[] availableValues, int requiredValue, Func<int, int, bool> comparer)
		{
			bool result = false;
			foreach (int arg in availableValues)
			{
				if (comparer(arg, requiredValue))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		static string GetValueWithoutFractionalPart(string value, string decimalSeparator)
		{
			string result = value;
			int num = value.IndexOf(decimalSeparator);
			if (num != -1)
			{
				result = value.Substring(0, num);
			}
			return result;
		}

		static string RemoveLeadingAndTrailingNonDigitSymbols(string value, string groupSeparator, string decimalSeparator)
		{
			int num = 0;
			for (int i = 0; i < value.Length; i++)
			{
				if (OpenXmlCultureInfo.IsValidSymbol(value[i], groupSeparator, decimalSeparator))
				{
					num = i;
					break;
				}
			}
			int num2 = value.Length - 1;
			for (int j = num2; j >= 0; j--)
			{
				if (OpenXmlCultureInfo.IsValidSymbol(value[j], groupSeparator, decimalSeparator))
				{
					num2 = j;
					break;
				}
			}
			return value.Substring(num, num2 - num + 1);
		}

		static bool IsValidSymbol(char c, string groupSeparator, string decimalSeparator)
		{
			return char.IsDigit(c) || OpenXmlCultureInfo.IsCharEqualTo(c, new string[] { groupSeparator, decimalSeparator });
		}

		bool ApplyAdditionalNumberConstraints(string value)
		{
			return value.IndexOf(this.NumberGroupSeparator) != -1 && OpenXmlCultureInfo.CheckGroupSeparatorsPositionedCorrectly(value, this.NumberGroupSeparator, this.NumberDecimalSeparator, this.CultureInfo.NumberFormat.NumberGroupSizes);
		}

		internal const string NullErrorValue = "#NULL!";

		internal const string DivisionByZeroErrorValue = "#DIV/0!";

		internal const string ValueErrorValue = "#VALUE!";

		internal const string ReferenceErrorValue = "#REF!";

		internal const string NameErrorValue = "#NAME?";

		internal const string NumberErrorValue = "#NUM!";

		internal const string NotAvailableErrorValue = "#N/A";

		internal const string ArrayLeftBracket = "{";

		internal const string ArrayRightBracket = "}";

		internal const string Ampersand = "&";

		internal const string Colon = ":";

		internal const string Exponent = "E";

		internal const string UnaryPlus = "+";

		internal const string UnaryMinus = "-";

		internal const string Percent = "%";

		internal const string Plus = "+";

		internal const string Minus = "-";

		internal const string Multiplication = "*";

		internal const string Division = "/";

		internal const string Power = "^";

		internal const string Equal = "=";

		internal const string GreaterThan = ">";

		internal const string GreaterThanOrEqualTo = ">=";

		internal const string LessThan = "<";

		internal const string LessThanOrEqualTo = "<=";

		internal const string NotEqual = "<>";

		internal const string LeftParenthesis = "(";

		internal const string RightParenthesis = ")";

		internal const string LeftSquareBracket = "[";

		internal const string RightSquareBracket = "]";

		internal const string TrueLiteral = "TRUE";

		internal const string FalseLiteral = "FALSE";

		internal const string DoubleQuote = "\"";

		internal const string SingleQuote = "'";

		internal const string Comma = ",";

		internal const string Semicolon = ";";

		internal const string FunctionStart = "=";

		internal const string Hash = "#";

		internal const string ExclamationMark = "!";

		internal const string QuestionMark = "?";

		internal const string Period = ".";

		internal const string Ellipsis = "...";

		internal const string Space = " ";

		internal const string Underscore = "_";

		internal const string Prime = "`";

		internal const string VerticalBar = "|";

		internal const string Tilde = "~";

		internal const string DollarSign = "$";

		const string Backslash = "\\";

		readonly CultureInfo cultureInfo;
	}
}
