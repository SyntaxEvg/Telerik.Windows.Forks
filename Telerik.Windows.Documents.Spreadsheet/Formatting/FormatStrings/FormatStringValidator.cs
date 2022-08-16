using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings
{
	static class FormatStringValidator
	{
		public static void Validate(string formatString)
		{
			FormatStringType? formatStringType;
			bool param = FormatStringValidator.IsValid(formatString, out formatStringType);
			Guard.ThrowExceptionIfFalse(param, "isValid");
		}

		public static bool ValidateAndGetFormatStringType(string formatString, out FormatStringType? formatStringType)
		{
			return FormatStringValidator.IsValid(formatString, out formatStringType);
		}

		static bool IsValid(string formatString, out FormatStringType? formatStringType)
		{
			formatStringType = null;
			if (string.IsNullOrEmpty(formatString))
			{
				return true;
			}
			if (FormatHelper.ContainsTotalHoursMinutesSecondsModifiers(formatString))
			{
				formatStringType = new FormatStringType?(FormatStringType.DateTime);
			}
			formatString = FormatHelper.StripBrackets(formatString);
			string[] array = formatString.Split(new char[] { ';' });
			if (array.Count<string>() == 1 && array[0].Contains(FormatHelper.TextPlaceholder))
			{
				return true;
			}
			Dictionary<FormatStringType, int> formatStringTypeToOccurenceCountDictionary = FormatStringValidator.GetFormatStringTypeToOccurenceCountDictionary();
			for (int i = 0; i < array.Length; i++)
			{
				formatStringType = FormatStringValidator.GetPartType(array[i], formatStringType);
				if (formatStringType == null)
				{
					return false;
				}
				Dictionary<FormatStringType, int> dictionary;
				FormatStringType value;
				(dictionary = formatStringTypeToOccurenceCountDictionary)[value = formatStringType.Value] = dictionary[value] + 1;
			}
			formatStringType = new FormatStringType?((from kv in formatStringTypeToOccurenceCountDictionary
				orderby kv.Value descending
				select kv).FirstOrDefault<KeyValuePair<FormatStringType, int>>().Key);
			return true;
		}

		static Dictionary<FormatStringType, int> GetFormatStringTypeToOccurenceCountDictionary()
		{
			return new Dictionary<FormatStringType, int>
			{
				{
					FormatStringType.Number,
					0
				},
				{
					FormatStringType.DateTime,
					0
				},
				{
					FormatStringType.Text,
					0
				}
			};
		}

		static FormatStringType? GetPartType(string part, FormatStringType? formatType)
		{
			if (part.Contains(FormatHelper.TextPlaceholder))
			{
				return new FormatStringType?(FormatStringType.Text);
			}
			if (!string.IsNullOrEmpty(part))
			{
				part = FormatHelper.StripBrackets(part);
				part = FormatHelper.StripUnderscores(part);
				part = FormatHelper.StripCurrencyEscapingsAndSlashes(part);
			}
			bool flag = false;
			for (int i = 0; i < part.Length; i++)
			{
				if (part[i] == '"')
				{
					flag = !flag;
				}
				else if (!flag)
				{
					if (FormatHelper.NumberChars.Contains(part[i]))
					{
						if (formatType == null || formatType == FormatStringType.Text)
						{
							formatType = new FormatStringType?(FormatStringType.Number);
						}
						else if (formatType == FormatStringType.DateTime && !FormatStringValidator.IsZeroValidInDateTimeContext(part, i))
						{
							return null;
						}
					}
					else if (FormatHelper.DateChars.Contains(part[i]) || FormatHelper.TimeChars.Contains(part[i]))
					{
						if (formatType == null || formatType == FormatStringType.Text)
						{
							formatType = new FormatStringType?(FormatStringType.DateTime);
						}
						else if (formatType != FormatStringType.DateTime)
						{
							return null;
						}
					}
				}
			}
			return formatType;
		}

		static bool IsZeroValidInDateTimeContext(string part, int index)
		{
			bool flag = false;
			if (part[index] == '0')
			{
				int num = 1;
				flag = index - num >= 0;
				while (flag && part[index - num] != '.')
				{
					flag = index - num >= 0;
					flag = part[index - num] == '0';
					num++;
				}
			}
			return flag;
		}
	}
}
