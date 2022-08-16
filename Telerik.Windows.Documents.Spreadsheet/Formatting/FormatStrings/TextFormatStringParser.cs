using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings
{
	class TextFormatStringParser
	{
		public FormatStringInfo FormatStringInfo
		{
			get
			{
				return this.formatStringInfo;
			}
		}

		internal string FormatString
		{
			get
			{
				return this.formatString;
			}
		}

		internal FormatDescriptor FormatDescriptor
		{
			get
			{
				return this.formatDescriptor;
			}
		}

		protected FormatDescriptor MainFormatDescriptor { get; set; }

		protected FormatDescriptor NegativeValueFormatDescriptor { get; set; }

		protected FormatDescriptor ZeroValueFormatDescriptor { get; set; }

		protected FormatDescriptor TextFormatDescriptor { get; set; }

		public TextFormatStringParser(string formatString, FormatStringInfo formatStringInfo)
		{
			this.formatString = formatString;
			this.formatStringInfo = formatStringInfo;
			List<FormatDescriptor> list = new List<FormatDescriptor>();
			string[] array = this.formatString.Split(new char[] { ';' });
			foreach (string sbPartString in array)
			{
				FormatDescriptor formatDescriptor = this.ConstructFormatDescriptor(sbPartString);
				if (formatDescriptor != null)
				{
					list.Add(formatDescriptor);
				}
			}
			FormatDescriptor formatDescriptor2 = ((list.Count > 0) ? list.Last<FormatDescriptor>() : null);
			if (formatDescriptor2 != null && formatDescriptor2.Text.Contains(FormatHelper.TextPlaceholder))
			{
				this.TextFormatDescriptor = formatDescriptor2;
				list.Remove(formatDescriptor2);
			}
			bool flag = list.Count == 1 && formatDescriptor2.ItemsCount == 1 && formatDescriptor2.Text.Equals(FormatHelper.GeneralFormatName, StringComparison.OrdinalIgnoreCase);
			if (flag)
			{
				formatDescriptor2.Items.First<FormatDescriptorItem>().Text = FormatHelper.GeneralFormatString;
			}
			this.SetFormatDescriptors(list);
		}

		protected virtual string ApplyFormatToString(FormatDescriptorItem item, double? doubleValue, DateTime? dateTimeValue, string stringValue)
		{
			return string.Format(item.GetTextReplacePlaceholder(), stringValue);
		}

		protected virtual void HandleSquareBracket(string sb, int index, out int newIndex, FormatDescriptor formatDescriptor)
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
				this.currentSequence += sb[index];
				return;
			}
			this.TryParseColor(modificator, formatDescriptor);
		}

		protected virtual void SetFormatDescriptors(List<FormatDescriptor> formatDescriptors)
		{
		}

		protected virtual FormatDescriptor ChooseFormatDescriptor(double? doubleValue, DateTime? dateTimeValue)
		{
			return this.TextFormatDescriptor;
		}

		protected virtual FormatDescriptor ConstructFormatDescriptor(string sbPartString)
		{
			if (sbPartString.Contains(FormatHelper.TextPlaceholder))
			{
				FormatDescriptor result = new FormatDescriptor();
				for (int i = 0; i < sbPartString.Length; i++)
				{
					if (sbPartString[i] == '_')
					{
						this.HandleUnderscore(sbPartString, i, out i, result, false);
					}
					else if (sbPartString[i] == '[')
					{
						this.HandleSquareBracket(sbPartString, i, out i, result);
					}
					else
					{
						this.currentSequence += sbPartString[i];
					}
				}
				this.AddCurrentSequenceToItemsAndClear(result, true);
				return result;
			}
			return null;
		}

		public List<CellValueFormatResultItem> ApplyFormatToValues(ICellValue cellValue, CellValueFormat format)
		{
			double? doubleValue = null;
			DateTime? dateTimeValue = null;
			string valueAsString = cellValue.GetValueAsString(format);
			NumberCellValue numberCellValue = cellValue as NumberCellValue;
			if (numberCellValue != null)
			{
				doubleValue = new double?(numberCellValue.Value);
				string text = FormatHelper.StripBrackets(this.FormatString);
				dateTimeValue = numberCellValue.ToDateTime();
				if (dateTimeValue != null && format.FormatStringInfo.FormatType == FormatStringType.DateTime && !text.Contains("0"))
				{
					dateTimeValue = new DateTime?(dateTimeValue.Value.RoundMinutes());
				}
			}
			List<CellValueFormatResultItem> list = new List<CellValueFormatResultItem>();
			this.formatDescriptor = this.ChooseFormatDescriptor(doubleValue, dateTimeValue);
			if (this.FormatDescriptor == null)
			{
				list.Add(new CellValueFormatResultItem(valueAsString, false, false, true));
				return list;
			}
			foreach (FormatDescriptorItem formatDescriptorItem in this.FormatDescriptor.Items.Reverse<FormatDescriptorItem>())
			{
				foreach (FormatDescriptorItem formatDescriptorItem2 in formatDescriptorItem.GetItems(doubleValue).Reverse<FormatDescriptorItem>())
				{
					if (formatDescriptorItem2.ApplyFormat)
					{
						list.Insert(0, new CellValueFormatResultItem(this.ApplyFormatToString(formatDescriptorItem2, doubleValue, dateTimeValue, valueAsString), formatDescriptorItem2.IsTransparent, formatDescriptorItem2.ShouldExpand, formatDescriptorItem2.ApplyFormat));
					}
					else
					{
						list.Insert(0, new CellValueFormatResultItem(formatDescriptorItem2.Text, formatDescriptorItem2.IsTransparent, formatDescriptorItem2.ShouldExpand, formatDescriptorItem2.ApplyFormat));
					}
				}
			}
			return list;
		}

		protected void HandleUnderscore(string sb, int index, out int newIndex, FormatDescriptor formatDescriptor, bool isCurrency = false)
		{
			newIndex = index;
			if (index + 1 >= sb.Length)
			{
				return;
			}
			this.AddCurrentSequenceToItemsAndClear(formatDescriptor, true);
			newIndex = index + 1;
			char c = sb[index + 1];
			bool applyFormat = !isCurrency && c != '.';
			formatDescriptor.AddItem(new FormatDescriptorItem(c.ToString(), true, false, applyFormat));
		}

		protected bool TryParseColor(string modificator, FormatDescriptor formatDescriptor)
		{
			switch (modificator)
			{
			case "Red":
				formatDescriptor.Foreground = new Color?(Colors.Red);
				return true;
			case "Blue":
				formatDescriptor.Foreground = new Color?(Colors.Blue);
				return true;
			case "Green":
				formatDescriptor.Foreground = new Color?(Colors.Green);
				return true;
			case "White":
				formatDescriptor.Foreground = new Color?(Colors.White);
				return true;
			case "Magenta":
				formatDescriptor.Foreground = new Color?(Colors.Magenta);
				return true;
			case "Yellow":
				formatDescriptor.Foreground = new Color?(Colors.Yellow);
				return true;
			case "Cyan":
				formatDescriptor.Foreground = new Color?(Colors.Cyan);
				return true;
			}
			return false;
		}

		protected bool TryParseCulture(string modificator, FormatDescriptor formatDescriptor, bool getDefault = true)
		{
			if (string.IsNullOrEmpty(modificator) || modificator.IndexOf("$-") == -1)
			{
				return false;
			}
			if (getDefault)
			{
				formatDescriptor.Culture = LocaleCodeToCultureInfoResolver.GetCultureOrDefault(modificator);
			}
			else
			{
				formatDescriptor.Culture = LocaleCodeToCultureInfoResolver.GetCulture(modificator);
			}
			return true;
		}

		public bool TryParseSwitchArguments(string modificator, FormatDescriptor formatDescriptor)
		{
			return modificator == TextFormatStringParser.DBNUM1;
		}

		protected void AddCurrentSequenceToItemsAndClear(FormatDescriptor formatDescriptor, bool applyFormatting = true)
		{
			if (!string.IsNullOrEmpty(this.currentSequence))
			{
				formatDescriptor.AddItem(new FormatDescriptorItem(this.currentSequence, false, false, applyFormatting));
				this.currentSequence = string.Empty;
			}
		}

		protected void GetConsecutiveSymbolRange(string sbPart, int index, out int newIndex)
		{
			newIndex = index;
			char c = sbPart[index];
			bool flag = false;
			for (int i = index; i < sbPart.Length; i++)
			{
				if (sbPart[i] != c)
				{
					newIndex = i - 1;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				newIndex = sbPart.Length - 1;
			}
		}

		public static TextFormatStringParser Create(string invariantFormatString, string localizedFormatString)
		{
			Guard.ThrowExceptionIfNull<string>(invariantFormatString, "invariantFormatString");
			Guard.ThrowExceptionIfNull<string>(localizedFormatString, "localizedFormatString");
			FormatStringInfo formatStringInfo = TextFormatStringParser.RetrieveFormatStringInfo(invariantFormatString, localizedFormatString);
			FormatStringType valueOrDefault = formatStringInfo.FormatType.GetValueOrDefault();
			FormatStringType? formatStringType;
			if (formatStringType != null)
			{
				switch (valueOrDefault)
				{
				case FormatStringType.Number:
					return new NumberFormatStringParser(invariantFormatString, formatStringInfo);
				case FormatStringType.DateTime:
					return new DateTimeFormatStringParser(invariantFormatString, formatStringInfo);
				case FormatStringType.Text:
					return new TextFormatStringParser(invariantFormatString, formatStringInfo);
				}
			}
			return new NumberFormatStringParser(invariantFormatString, formatStringInfo);
		}

		static FormatStringInfo RetrieveFormatStringInfo(string invariantFormatString, string localizedFormatString)
		{
			Guard.ThrowExceptionIfNull<string>(invariantFormatString, "invariantFormatString");
			Guard.ThrowExceptionIfNull<string>(localizedFormatString, "localizedFormatString");
			FormatStringInfo formatStringInfo = new FormatStringInfo(null, FormatStringCategoryManager.GetCategoryFromFormatString(localizedFormatString), false, false);
			if ((formatStringInfo.Category == FormatStringCategory.Custom || formatStringInfo.Category == FormatStringCategory.Date || formatStringInfo.Category == FormatStringCategory.Time) && FormatHelper.ContainsTotalHoursMinutesSecondsModifiers(invariantFormatString))
			{
				formatStringInfo.FormatType = new FormatStringType?(FormatStringType.DateTime);
			}
			if (string.IsNullOrEmpty(invariantFormatString) && formatStringInfo.FormatType == null)
			{
				formatStringInfo.FormatType = new FormatStringType?(FormatStringType.Number);
				return formatStringInfo;
			}
			string[] array = invariantFormatString.Split(new char[] { ';' });
			if (array.Count<string>() == 1 && array[0].Contains(FormatHelper.TextPlaceholder))
			{
				formatStringInfo.FormatType = new FormatStringType?(FormatStringType.Text);
				return formatStringInfo;
			}
			for (int i = 0; i < array.Length; i++)
			{
				TextFormatStringParser.GetPartInfo(array[i], formatStringInfo);
			}
			return formatStringInfo;
		}

		static void GetPartInfo(string part, FormatStringInfo formatStringInfo)
		{
			if (part.Contains(FormatHelper.TextPlaceholder))
			{
				return;
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
						if (formatStringInfo.FormatType == null)
						{
							formatStringInfo.FormatType = new FormatStringType?(FormatStringType.Number);
						}
					}
					else if (FormatHelper.DateChars.Contains(part[i]) || FormatHelper.TimeChars.Contains(part[i]))
					{
						if (FormatHelper.TimeChars.Contains(part[i]))
						{
							formatStringInfo.HasTimeChars = true;
						}
						if (FormatHelper.DateChars.Contains(part[i]) && !TextFormatStringParser.IsMinute(part, i))
						{
							formatStringInfo.HasDateChars = true;
						}
						if (formatStringInfo.FormatType == null)
						{
							formatStringInfo.FormatType = new FormatStringType?(FormatStringType.DateTime);
						}
					}
				}
			}
		}

		static bool IsMinute(string part, int index)
		{
			char c = char.ToLower(part[index]);
			char c2 = char.ToUpper(part[index]);
			bool result = false;
			for (int i = index; i < part.Length; i++)
			{
				if (SpreadsheetCultureHelper.IsCharEqualTo(part[i], new string[] { FormatHelper.DefaultSpreadsheetCulture.TimeSeparator }))
				{
					result = true;
				}
				if (part[i] != c && part[i] != c2 && !SpreadsheetCultureHelper.IsCharEqualTo(part[i], new string[] { FormatHelper.DefaultSpreadsheetCulture.TimeSeparator }))
				{
					break;
				}
			}
			for (int j = index; j >= 0; j--)
			{
				if (SpreadsheetCultureHelper.IsCharEqualTo(part[j], new string[] { FormatHelper.DefaultSpreadsheetCulture.TimeSeparator }))
				{
					result = true;
				}
				if (part[j] != c && part[j] != c2 && !SpreadsheetCultureHelper.IsCharEqualTo(part[j], new string[] { FormatHelper.DefaultSpreadsheetCulture.TimeSeparator }))
				{
					break;
				}
			}
			return result;
		}

		static readonly string DBNUM1 = "dbnum1";

		readonly string formatString;

		protected string currentSequence;

		protected FormatDescriptor formatDescriptor;

		readonly FormatStringInfo formatStringInfo;
	}
}
