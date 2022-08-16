using System;
using System.Globalization;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core
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

		public string NumberGroupSeparator
		{
			get
			{
				return this.cultureInfo.NumberFormat.NumberGroupSeparator;
			}
		}

		public string NumberDecimalSeparator
		{
			get
			{
				return this.cultureInfo.NumberFormat.NumberDecimalSeparator;
			}
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
			Func<int, int, bool> comparer = (int a, int j) => a >= j;
			Func<int, int, bool> comparer2 = (int a, int j) => a == j;
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

		static bool IsValidSymbol(char c, string groupSeparator, string decimalSeparator)
		{
			return char.IsDigit(c) || OpenXmlCultureInfo.IsCharEqualTo(c, new string[] { groupSeparator, decimalSeparator });
		}

		bool ApplyAdditionalNumberConstraints(string value)
		{
			return value.IndexOf(this.NumberGroupSeparator) != -1 && OpenXmlCultureInfo.CheckGroupSeparatorsPositionedCorrectly(value, this.NumberGroupSeparator, this.NumberDecimalSeparator, this.CultureInfo.NumberFormat.NumberGroupSizes);
		}

		readonly CultureInfo cultureInfo;
	}
}
