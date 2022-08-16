using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Measurement;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
{
	static class TextHelper
	{
		public static string SanitizeNewLines(string input)
		{
			StringBuilder stringBuilder = new StringBuilder(input.Length);
			bool flag = false;
			int i = 0;
			int length = input.Length;
			while (i < length)
			{
				if (input[i] == '\r')
				{
					if (flag)
					{
						stringBuilder.Append(LineBreak.NewLine);
					}
					flag = true;
				}
				else if (input[i] == '\n')
				{
					stringBuilder.Append(LineBreak.NewLine);
					flag = false;
				}
				else
				{
					if (flag)
					{
						stringBuilder.Append(LineBreak.NewLine);
						flag = false;
					}
					stringBuilder.Append(input[i]);
				}
				i++;
			}
			if (flag)
			{
				stringBuilder.Append(LineBreak.NewLine);
			}
			return stringBuilder.ToString();
		}

		public static string GetClipboardEncodedText(string input)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in TextHelper.GetValues(input))
			{
				stringBuilder.Append(TextHelper.DecodeValue(value, "\""));
			}
			return stringBuilder.ToString();
		}

		public static string GetClipboardDecodedText(string input)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in TextHelper.GetValues(input))
			{
				stringBuilder.Append(TextHelper.EncodeValue(value, "\""));
			}
			return stringBuilder.ToString();
		}

		static IEnumerable<string> GetValues(string input)
		{
			StringBuilder builder = new StringBuilder();
			bool isValueEscapedCorrectly = false;
			bool requireDoubleQuote = false;
			for (int i = 0; i < input.Length; i++)
			{
				if (TextHelper.IsStartOfValue(input, i, isValueEscapedCorrectly))
				{
					isValueEscapedCorrectly = input[i] == "\""[0];
				}
				else if (input[i] == "\""[0])
				{
					if (isValueEscapedCorrectly)
					{
						requireDoubleQuote = !requireDoubleQuote;
					}
				}
				else
				{
					if (requireDoubleQuote)
					{
						requireDoubleQuote = false;
						isValueEscapedCorrectly = false;
					}
					if (TextHelper.IsEndOfValue(input, i, isValueEscapedCorrectly))
					{
						if (builder.Length > 0)
						{
							yield return builder.ToString();
							builder.Clear();
							isValueEscapedCorrectly = false;
						}
						yield return input[i].ToString();
					}
				}
				if (!TextHelper.IsEndOfValue(input, i, isValueEscapedCorrectly))
				{
					builder.Append(input[i]);
				}
			}
			if (builder.Length != 0)
			{
				yield return builder.ToString();
			}
			yield break;
		}

		internal static string DecodeValue(string value, string textQualifier)
		{
			Guard.ThrowExceptionIfNullOrEmpty(value, "value");
			if (value.Length < 2 || value.Contains("\n") || value.Contains("\t"))
			{
				return value;
			}
			string text = TextHelper.StripSurroundingQuotes(value, textQualifier);
			return text.Replace(textQualifier + textQualifier, textQualifier);
		}

		internal static string EncodeValue(string value, string textQualifier)
		{
			Guard.ThrowExceptionIfNullOrEmpty(value, "value");
			if (value.Contains("\n") || value.Contains("\t") || !value.Contains(textQualifier))
			{
				return value;
			}
			return TextHelper.GetEscapedTextValue(value, textQualifier);
		}

		internal static string GetEscapedTextValue(string value, string textQualifier)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(textQualifier);
			stringBuilder.Append(value.Replace(textQualifier, textQualifier + textQualifier));
			stringBuilder.Append(textQualifier);
			return stringBuilder.ToString();
		}

		internal static string EncodeValue(string value)
		{
			Guard.ThrowExceptionIfNullOrEmpty(value, "value");
			string text = "\"";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(text);
			stringBuilder.Append(value.Replace(text, text + text));
			stringBuilder.Append(text);
			return stringBuilder.ToString();
		}

		static bool IsStartOfValue(string value, int position, bool escapeSpecialSymbols)
		{
			if (position == 0)
			{
				return !TextHelper.IsSpecialSymbol(value[position]);
			}
			return !TextHelper.IsSpecialSymbol(value[position]) && TextHelper.IsSpecialSymbol(value[position - 1]) && !escapeSpecialSymbols;
		}

		static bool IsEndOfValue(string value, int position, bool escapeSpecialSymbols)
		{
			if (position == 0)
			{
				return TextHelper.IsSpecialSymbol(value[position]);
			}
			return TextHelper.IsSpecialSymbol(value[position]) && !escapeSpecialSymbols;
		}

		static bool IsSpecialSymbol(char character)
		{
			return character == '\t' || character == '\n';
		}

		static string StripSurroundingQuotes(string value, string textQualifier)
		{
			string text = value;
			if (value.Length > 1 && text[0] == textQualifier[0] && text[text.Length - 1] == textQualifier[0])
			{
				text = text.Substring(1, text.Length - 2);
			}
			return text;
		}

		public static string DecodeWorksheetName(string input)
		{
			char c = "'"[0];
			if (!string.IsNullOrEmpty(input) && input[0] == c && input[input.Length - 1] == c)
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = false;
				for (int i = 1; i < input.Length - 1; i++)
				{
					if (!flag)
					{
						stringBuilder.Append(input[i]);
						if (i < input.Length - 2 && input[i] == c && input[i + 1] == c)
						{
							flag = true;
						}
					}
					else
					{
						flag = false;
					}
				}
				return stringBuilder.ToString();
			}
			return input;
		}

		public static string EncodeWorksheetName(string input, bool encodeRegardlessOfContent = false)
		{
			string text = "'";
			if (TextHelper.ShouldEscapeSheetName(input) || encodeRegardlessOfContent)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(text);
				stringBuilder.Append(input.Replace(text, text + text));
				stringBuilder.Append(text);
				return stringBuilder.ToString();
			}
			return input;
		}

		internal static string EscapeInvariantDateTimeSeparators(string value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			for (int i = 0; i < value.Length; i++)
			{
				char c = value[i];
				if (c == '"')
				{
					num++;
				}
				bool flag = i >= 1 && value[i - 1] == '\\';
				bool flag2 = i >= 1 && value[i - 1] == '"' && num % 2 == 0;
				bool flag3 = !flag && !flag2;
				if (SpreadsheetCultureHelper.IsCharEqualTo(value[i], new string[] { "/", ":" }) && flag3)
				{
					stringBuilder.Append("\"");
					stringBuilder.Append(c);
					stringBuilder.Append("\"");
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		static bool ShouldEscapeSheetName(string input)
		{
			Guard.ThrowExceptionIfNullOrEmpty(input, "input");
			if (!char.IsLetter(input[0]) && !SpreadsheetCultureHelper.IsCharEqualTo(input[0], new string[] { "_" }))
			{
				return true;
			}
			for (int i = 0; i < input.Length; i++)
			{
				if (!char.IsLetter(input[i]) && !char.IsDigit(input[i]) && !SpreadsheetCultureHelper.IsCharEqualTo(input[i], new string[] { "_", "." }))
				{
					return true;
				}
			}
			return false;
		}

		public static string BuildAbsoluteCellReferenceString(string worksheetName, CellRange selectedRange, SpreadsheetCultureHelper cultureInfo = null)
		{
			return TextHelper.BuildAbsoluteCellReferenceString(worksheetName, Enumerable.Repeat<CellRange>(selectedRange, 1), cultureInfo);
		}

		public static string BuildAbsoluteCellReferenceString(string worksheetName, IEnumerable<CellRange> selectedRanges, SpreadsheetCultureHelper cultureInfo = null)
		{
			Guard.ThrowExceptionIfNullOrEmpty(worksheetName, "worksheetName");
			Guard.ThrowExceptionIfNull<IEnumerable<CellRange>>(selectedRanges, "selectedRanges");
			Guard.ThrowExceptionIfLessThan<int>(1, selectedRanges.Count<CellRange>(), "selectedRanges");
			cultureInfo = cultureInfo ?? FormatHelper.DefaultSpreadsheetCulture;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("=");
			List<CellRange> list = selectedRanges.ToList<CellRange>();
			for (int i = 0; i < list.Count; i++)
			{
				CellRange cellRange = list[i];
				stringBuilder.Append(TextHelper.EncodeWorksheetName(worksheetName, false));
				stringBuilder.Append("!");
				stringBuilder.Append(NameConverter.ConvertCellIndexToAbsoluteName(cellRange.FromIndex));
				if (cellRange.ColumnCount > 1 || cellRange.RowCount > 1)
				{
					stringBuilder.Append(":");
					stringBuilder.Append(NameConverter.ConvertCellIndexToAbsoluteName(cellRange.ToIndex));
				}
				if (i < list.Count - 1)
				{
					stringBuilder.Append(cultureInfo.ListSeparator);
					stringBuilder.Append(" ");
				}
			}
			return stringBuilder.ToString();
		}
	}
}
