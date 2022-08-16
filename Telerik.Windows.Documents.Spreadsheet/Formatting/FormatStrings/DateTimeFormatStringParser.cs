using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings
{
	class DateTimeFormatStringParser : TextFormatStringParser
	{
		public DateTimeFormatStringParser(string formatString, FormatStringInfo formatStringInfo)
			: base(formatString, formatStringInfo)
		{
		}

		void HandleZeroInDateTimePart(string sbPart, int index, out int newIndex)
		{
			base.GetConsecutiveSymbolRange(sbPart, index, out newIndex);
			int count = newIndex - index + 1;
			this.currentSequence += string.Concat<char>(Enumerable.Repeat<char>(FormatHelper.MilliSecondsChar, count));
		}

		void HandleAmPm(string sbPart, int index, out int newIndex)
		{
			newIndex = index;
			if (index + 4 < sbPart.Length && sbPart[index + 1] == 'm' && sbPart[index + 2] == '/' && sbPart[index + 3] == 'p' && sbPart[index + 1] == 'm')
			{
				newIndex = index + 4;
				this.currentSequence += "tt";
				return;
			}
			this.currentSequence += sbPart[index];
		}

		void HandleHourSymbol(string sbPart, int index, out int newIndex)
		{
			base.GetConsecutiveSymbolRange(sbPart, index, out newIndex);
			int length = newIndex - index + 1;
			string text = sbPart.ToString().Substring(index, length);
			if (!sbPart.ToString().Contains("am/pm") && !sbPart.ToString().Contains("tt"))
			{
				text = text.ToUpper();
			}
			this.currentSequence += text;
		}

		void HandleMonthMinuteSymbol(string sbPart, int index, out int newIndex, FormatDescriptor formatDescriptor)
		{
			base.GetConsecutiveSymbolRange(sbPart, index, out newIndex);
			int num = newIndex - index + 1;
			bool flag = this.IsPositionInDatePart(sbPart, index, newIndex);
			string text = sbPart.Substring(index, num);
			if (flag && num == 5)
			{
				base.AddCurrentSequenceToItemsAndClear(formatDescriptor, true);
				formatDescriptor.AddItem(new FormatDescriptorItem(new Func<DateTime?, string>(this.ApplyFirstLetterOfMonthToValue), false, false, true));
				return;
			}
			if (flag)
			{
				this.currentSequence += text.ToUpper();
				return;
			}
			this.currentSequence += text.ToLower();
		}

		string ApplyFirstLetterOfMonthToValue(DateTime? dateTimeValue)
		{
			return this.EscapeString(dateTimeValue.Value.ToString("MMM").Substring(0, 1));
		}

		string EscapeString(string str)
		{
			return string.Format("\"{0}\"", str);
		}

		bool IsPositionInDatePart(string sbPart, int start, int end)
		{
			bool result = true;
			if (start - 2 >= 0 && SpreadsheetCultureHelper.IsCharEqualTo(sbPart[start - 1], new string[] { FormatHelper.DefaultSpreadsheetCulture.TimeSeparator }) && (sbPart[start - 2] == DateTimeFormatStringParser.HourChar || sbPart[start - 2] == DateTimeFormatStringParser.SecondsChar))
			{
				result = false;
			}
			else if (end + 2 < sbPart.Length && SpreadsheetCultureHelper.IsCharEqualTo(sbPart[end + 1], new string[] { FormatHelper.DefaultSpreadsheetCulture.TimeSeparator }) && sbPart[end + 2] == DateTimeFormatStringParser.SecondsChar)
			{
				result = false;
			}
			return result;
		}

		bool TryParseTotalHoursMinutesSeconds(string modificator, FormatDescriptor formatDescriptor)
		{
			Func<DateTime?, string> applyFormatToDateTimeValue;
			if (modificator == DateTimeFormatStringParser.HourChar.ToString())
			{
				applyFormatToDateTimeValue = new Func<DateTime?, string>(this.GetTotalHours);
			}
			else if (modificator == DateTimeFormatStringParser.MinuteChar.ToString())
			{
				applyFormatToDateTimeValue = new Func<DateTime?, string>(this.GetTotalMinutes);
			}
			else
			{
				if (!(modificator == DateTimeFormatStringParser.SecondsChar.ToString()))
				{
					throw new ArgumentException("Invalid modificator.");
				}
				applyFormatToDateTimeValue = new Func<DateTime?, string>(this.GetTotalSeconds);
			}
			base.AddCurrentSequenceToItemsAndClear(formatDescriptor, true);
			formatDescriptor.AddItem(new FormatDescriptorItem(applyFormatToDateTimeValue, false, false, true));
			return true;
		}

		string GetTotalHours(DateTime? dateTimeValue)
		{
			DateTime t = new DateTime(1900, 2, 28);
			DateTime value = ((dateTimeValue <= t) ? FormatHelper.StartDate.AddDays(-1.0) : FormatHelper.StartDate.AddDays(-2.0));
			return ((long)dateTimeValue.Value.Subtract(value).TotalHours).ToString();
		}

		string GetTotalMinutes(DateTime? dateTimeValue)
		{
			DateTime t = new DateTime(1900, 2, 28);
			DateTime value = ((dateTimeValue <= t) ? FormatHelper.StartDate.AddDays(-1.0) : FormatHelper.StartDate.AddDays(-2.0));
			return ((long)dateTimeValue.Value.Subtract(value).TotalMinutes).ToString();
		}

		string GetTotalSeconds(DateTime? dateTimeValue)
		{
			DateTime t = new DateTime(1900, 2, 28);
			DateTime value = ((dateTimeValue <= t) ? FormatHelper.StartDate.AddDays(-1.0) : FormatHelper.StartDate.AddDays(-2.0));
			TimeSpan timeSpan = dateTimeValue.Value.Subtract(value);
			long num = (long)timeSpan.TotalSeconds;
			if ((double)timeSpan.Milliseconds > 0.5)
			{
				num += 1L;
			}
			return num.ToString();
		}

		void ValidateDateTimeFormatDescriptor(List<FormatDescriptor> formatStringParts)
		{
			if (formatStringParts == null || formatStringParts.Count == 0 || formatStringParts.Count > 2 || (formatStringParts.Count == 2 && !formatStringParts[1].Text.Contains(FormatHelper.TextPlaceholder)))
			{
				throw new LocalizableException("Format string is not in the correct format.", new ArgumentException("Format string is not in the correct format."), "Spreadsheet_ErrorExpressions_FormatStringIncorrectFormat", null);
			}
		}

		protected override FormatDescriptor ChooseFormatDescriptor(double? doubleValue, DateTime? dateTimeValue)
		{
			if (dateTimeValue == null && doubleValue != null)
			{
				FormatDescriptor formatDescriptor = new FormatDescriptor();
				formatDescriptor.AddItem(new FormatDescriptorItem("#", false, true, true));
				return formatDescriptor;
			}
			if (dateTimeValue == null && base.TextFormatDescriptor != null)
			{
				return base.TextFormatDescriptor;
			}
			return base.MainFormatDescriptor;
		}

		protected override string ApplyFormatToString(FormatDescriptorItem item, double? doubleValue, DateTime? dateTimeValue, string stringValue)
		{
			if (doubleValue == null && dateTimeValue == null && !item.Text.Contains(FormatHelper.TextPlaceholder))
			{
				return stringValue;
			}
			if (dateTimeValue == null)
			{
				return base.ApplyFormatToString(item, doubleValue, dateTimeValue, stringValue);
			}
			string text = TextHelper.EscapeInvariantDateTimeSeparators(item.ApplyFormatToValue(dateTimeValue));
			int num;
			if (int.TryParse(text, out num))
			{
				return text;
			}
			return dateTimeValue.Value.ToString(text, FormatHelper.DefaultSpreadsheetCulture.CultureInfo);
		}

		protected override void SetFormatDescriptors(List<FormatDescriptor> formatDescriptors)
		{
			this.ValidateDateTimeFormatDescriptor(formatDescriptors);
			base.MainFormatDescriptor = formatDescriptors[0];
		}

		protected override FormatDescriptor ConstructFormatDescriptor(string sbPart)
		{
			if (sbPart.ToString().Contains(FormatHelper.TextPlaceholder))
			{
				return base.ConstructFormatDescriptor(sbPart);
			}
			sbPart = sbPart.ToLower();
			FormatDescriptor formatDescriptor = new FormatDescriptor();
			for (int i = 0; i < sbPart.Length; i++)
			{
				if (sbPart[i] == '[')
				{
					this.HandleSquareBracket(sbPart, i, out i, formatDescriptor);
				}
				else if (sbPart[i] == 'h')
				{
					this.HandleHourSymbol(sbPart, i, out i);
				}
				else if (sbPart[i] == 'm')
				{
					this.HandleMonthMinuteSymbol(sbPart, i, out i, formatDescriptor);
				}
				else if (sbPart[i] == 'a')
				{
					this.HandleAmPm(sbPart, i, out i);
				}
				else if (sbPart[i] == '0')
				{
					this.HandleZeroInDateTimePart(sbPart, i, out i);
				}
				else
				{
					this.currentSequence += sbPart[i];
				}
			}
			base.AddCurrentSequenceToItemsAndClear(formatDescriptor, true);
			return formatDescriptor;
		}

		protected override void HandleSquareBracket(string sb, int index, out int newIndex, FormatDescriptor formatDescriptor)
		{
			string modificator = string.Empty;
			newIndex = index;
			bool flag = false;
			for (int i = index; i < sb.Length; i++)
			{
				if (sb[i] == ']')
				{
					modificator = sb.Substring(index + 1, i - index - 1);
					newIndex = i;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return;
			}
			if (base.TryParseSwitchArguments(modificator, formatDescriptor))
			{
				return;
			}
			if (!base.TryParseCulture(modificator, formatDescriptor, true))
			{
				bool flag2 = this.TryParseTotalHoursMinutesSeconds(modificator, formatDescriptor);
			}
		}

		static readonly char HourChar = 'h';

		static readonly char MinuteChar = 'm';

		static readonly char SecondsChar = 's';
	}
}
