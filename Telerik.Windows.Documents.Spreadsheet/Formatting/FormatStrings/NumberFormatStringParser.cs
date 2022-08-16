using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Builders;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings
{
	class NumberFormatStringParser : TextFormatStringParser
	{
		List<FormatDescriptor> ConditionalFormatDescriptors { get; set; }

		public NumberFormatStringParser(string formatString, FormatStringInfo formatStringInfo)
			: base(formatString, formatStringInfo)
		{
		}

		protected override string ApplyFormatToString(FormatDescriptorItem item, double? doubleValue, DateTime? dateTimeValue, string stringValue)
		{
			if (doubleValue == null)
			{
				return base.ApplyFormatToString(item, doubleValue, dateTimeValue, stringValue);
			}
			if (doubleValue.Value >= FormatHelper.MinimumScientificValue && string.IsNullOrEmpty(item.ApplyFormatToValue(doubleValue)))
			{
				return doubleValue.Value.ToString(this.GetFormatForScientificNumber(doubleValue.Value), FormatHelper.DefaultSpreadsheetCulture.CultureInfo);
			}
			double num = doubleValue.Value;
			if (base.FormatDescriptor == base.NegativeValueFormatDescriptor)
			{
				num *= -1.0;
			}
			num += (double)this.valueToAdd;
			string result = string.Empty;
			try
			{
				string format = item.ApplyFormatToValue(doubleValue);
				result = num.ToString(format, FormatHelper.DefaultSpreadsheetCulture.CultureInfo);
			}
			catch (FormatException)
			{
				result = item.ApplyFormatToValue(doubleValue);
			}
			return result;
		}

		protected override FormatDescriptor ChooseFormatDescriptor(double? doubleValue, DateTime? dateTimeValue)
		{
			if (doubleValue == null)
			{
				return base.TextFormatDescriptor;
			}
			if (base.MainFormatDescriptor == null && this.ConditionalFormatDescriptors != null)
			{
				foreach (FormatDescriptor formatDescriptor in this.ConditionalFormatDescriptors)
				{
					if (formatDescriptor.Condition == null)
					{
						return formatDescriptor;
					}
					if (formatDescriptor.Condition(doubleValue.Value))
					{
						return formatDescriptor;
					}
				}
			}
			if (doubleValue.Value == 0.0 && base.ZeroValueFormatDescriptor != null)
			{
				return base.ZeroValueFormatDescriptor;
			}
			if (doubleValue.Value < 0.0 && base.NegativeValueFormatDescriptor != null)
			{
				return base.NegativeValueFormatDescriptor;
			}
			if (base.MainFormatDescriptor != null)
			{
				return base.MainFormatDescriptor;
			}
			throw new InvalidFormatStringException();
		}

		protected override void SetFormatDescriptors(List<FormatDescriptor> formatDescriptors)
		{
			this.ValidateNumberFormatDescriptors(formatDescriptors);
			if (formatDescriptors.Count == 0)
			{
				return;
			}
			if (formatDescriptors[0].Condition != null)
			{
				this.ConditionalFormatDescriptors = formatDescriptors;
				return;
			}
			if (formatDescriptors.Count >= 1)
			{
				base.MainFormatDescriptor = formatDescriptors[0];
			}
			if (formatDescriptors.Count >= 2)
			{
				base.NegativeValueFormatDescriptor = formatDescriptors[1];
			}
			if (formatDescriptors.Count >= 3)
			{
				base.ZeroValueFormatDescriptor = formatDescriptors[2];
			}
		}

		protected override FormatDescriptor ConstructFormatDescriptor(string sbPart)
		{
			if (sbPart.Contains(FormatHelper.TextPlaceholder))
			{
				return base.ConstructFormatDescriptor(sbPart);
			}
			FormatDescriptor formatDescriptor = new FormatDescriptor();
			if (sbPart.Length == 0)
			{
				formatDescriptor.AddItem(new FormatDescriptorItem(string.Empty, false, false, true));
			}
			else
			{
				string currencySymbol = FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.CurrencySymbol;
				int[] startIndeces = FormatHelper.FindAllOccurrences(sbPart, currencySymbol);
				string text = BuildersHelper.MakeCurrencySymbolInvisible(currencySymbol);
				int[] startIndeces2 = FormatHelper.FindAllOccurrences(sbPart, text);
				bool flag = false;
				for (int i = 0; i < sbPart.Length; i++)
				{
					char c = sbPart[i];
					if (c == '"')
					{
						flag = !flag;
						this.currentSequence += c;
					}
					else if (flag)
					{
						this.currentSequence += c;
					}
					else if (FormatHelper.CheckIfIsCurrencySymbolPart(startIndeces, currencySymbol, i) || FormatHelper.CheckIfIsCurrencySymbolPart(startIndeces2, text, i))
					{
						if (c == '_')
						{
							base.HandleUnderscore(sbPart, i, out i, formatDescriptor, true);
						}
						else if (i > 0 && sbPart[i - 1] != '\\')
						{
							this.currentSequence = this.currentSequence + "\\" + c;
						}
						else
						{
							this.currentSequence += c;
						}
					}
					else if (c == '_')
					{
						base.HandleUnderscore(sbPart, i, out i, formatDescriptor, false);
					}
					else if (c == '[')
					{
						this.HandleSquareBracket(sbPart, i, out i, formatDescriptor);
					}
					else if (c == '*')
					{
						this.HandleStar(sbPart, i, out i, formatDescriptor);
					}
					else if (c == '?')
					{
						this.HandleQuestionMarkSymbol(sbPart, i, out i, formatDescriptor);
					}
					else if (c == '.' && i - 1 > 0 && sbPart[i - 1] != '#' && sbPart[i - 1] != '0')
					{
						base.AddCurrentSequenceToItemsAndClear(formatDescriptor, true);
						this.currentSequence += c;
						base.AddCurrentSequenceToItemsAndClear(formatDescriptor, false);
					}
					else
					{
						this.currentSequence += c;
					}
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
				this.currentSequence += sb[index];
				return;
			}
			bool flag2 = base.TryParseColor(modificator, formatDescriptor);
			if (!flag2)
			{
				flag2 = this.TryParseCondition(modificator, formatDescriptor);
			}
			if (!flag2)
			{
				flag2 = this.TryParseSymbolCulture(modificator, formatDescriptor);
			}
			if (!flag2)
			{
				flag2 = base.TryParseCulture(modificator, formatDescriptor, true);
			}
		}

		void HandleStar(string sbPart, int index, out int newIndex, FormatDescriptor formatDescriptor)
		{
			newIndex = index;
			if (index + 1 < sbPart.Length)
			{
				base.AddCurrentSequenceToItemsAndClear(formatDescriptor, true);
				newIndex = index + 1;
				formatDescriptor.AddItem(new FormatDescriptorItem(sbPart[newIndex].ToString(), false, true, true));
			}
		}

		void HandleQuestionMarkSymbol(string sbPart, int index, out int newIndex, FormatDescriptor formatDescriptor)
		{
			NumberFormatStringParser.<>c__DisplayClass1 CS$<>8__locals1 = new NumberFormatStringParser.<>c__DisplayClass1();
			CS$<>8__locals1.sbPart = sbPart;
			CS$<>8__locals1.<>4__this = this;
			base.GetConsecutiveSymbolRange(CS$<>8__locals1.sbPart, index, out newIndex);
			int num = newIndex - index + 1;
			CS$<>8__locals1.nominatorRequiredLength = num;
			if (newIndex + 1 >= CS$<>8__locals1.sbPart.Length || CS$<>8__locals1.sbPart[newIndex + 1] != '/')
			{
				base.AddCurrentSequenceToItemsAndClear(formatDescriptor, true);
				string text = string.Concat<char>(Enumerable.Repeat<char>(CS$<>8__locals1.sbPart[index], num));
				formatDescriptor.AddItem(new FormatDescriptorItem(text, true, false, true));
				return;
			}
			int newStartIndex = newIndex + 2;
			string upToDigitsString = string.Empty;
			int denominatorRequiredLength = -1;
			bool isNextSymbolQuestionMark = CS$<>8__locals1.sbPart[newStartIndex] == '?';
			if (isNextSymbolQuestionMark)
			{
				base.GetConsecutiveSymbolRange(CS$<>8__locals1.sbPart, newStartIndex, out newIndex);
			}
			else
			{
				this.GetConsecutiveDigitRange(CS$<>8__locals1.sbPart, newStartIndex, out newIndex);
			}
			int newIndexCopy = newIndex;
			Func<double?, string, IEnumerable<FormatDescriptorItem>> getItems = delegate(double? doubleValue, string startSequence)
			{
				List<FormatDescriptorItem> list = new List<FormatDescriptorItem>();
				double num2 = FormatHelper.Round(doubleValue.Value);
				double num3 = doubleValue.Value - num2;
				string upToDigitsString;
				if (isNextSymbolQuestionMark)
				{
					int num4 = newIndexCopy - newStartIndex + 1;
					denominatorRequiredLength = num4;
					upToDigitsString = CS$<>8__locals1.<>4__this.GetUpToDigits(Math.Abs(num3), num4);
				}
				else
				{
					denominatorRequiredLength = newIndexCopy - newStartIndex + 1;
					string s = CS$<>8__locals1.sbPart.Substring(newStartIndex, denominatorRequiredLength);
					upToDigitsString = CS$<>8__locals1.<>4__this.GetFractionStringByDenominator(Math.Abs(num3), int.Parse(s), true);
				}
				if (upToDigitsString.IndexOf('/') == -1)
				{
					if (upToDigitsString == "0" && num2 == 0.0)
					{
						startSequence = startSequence.Replace('#', '0');
					}
					if (!string.IsNullOrEmpty(startSequence))
					{
						list.Add(new FormatDescriptorItem(startSequence, false, false, true));
						startSequence = string.Empty;
					}
					string str = string.Concat<char>(Enumerable.Repeat<char>(FormatHelper.FractionsInvisibleChar, CS$<>8__locals1.nominatorRequiredLength));
					string str2 = string.Concat<char>(Enumerable.Repeat<char>(FormatHelper.FractionsInvisibleChar, denominatorRequiredLength));
					list.Add(new FormatDescriptorItem(str + "/" + str2, true, false, false));
				}
				else
				{
					upToDigitsString = upToDigitsString;
					int num5 = upToDigitsString.IndexOf('/');
					string text2 = upToDigitsString.Substring(0, num5);
					string text3 = upToDigitsString.Substring(num5 + 1);
					if (num3 >= 0.5 && num2 > 0.0)
					{
						CS$<>8__locals1.<>4__this.valueToAdd = -1;
					}
					else if (num3 <= -0.5)
					{
						CS$<>8__locals1.<>4__this.valueToAdd = 1;
					}
					if (num2 == 0.0 && num3 > 0.0)
					{
						startSequence = string.Empty;
					}
					else if (num2 == 0.0 && num3 < 0.0)
					{
						startSequence = "- ";
					}
					if (text2.Length < CS$<>8__locals1.nominatorRequiredLength || text3.Length < denominatorRequiredLength)
					{
						if (!string.IsNullOrEmpty(startSequence))
						{
							list.Add(new FormatDescriptorItem(startSequence, false, false, true));
							startSequence = string.Empty;
						}
						if (text2.Length < CS$<>8__locals1.nominatorRequiredLength)
						{
							string text4 = string.Concat<char>(Enumerable.Repeat<char>(FormatHelper.FractionsInvisibleChar, CS$<>8__locals1.nominatorRequiredLength - text2.Length));
							list.Add(new FormatDescriptorItem(text4, true, false, false));
						}
						list.Add(new FormatDescriptorItem(upToDigitsString, false, false, false));
						if (text3.Length < denominatorRequiredLength)
						{
							string text5 = string.Concat<char>(Enumerable.Repeat<char>(FormatHelper.FractionsInvisibleChar, denominatorRequiredLength - text3.Length));
							list.Add(new FormatDescriptorItem(text5, true, false, false));
						}
					}
					else
					{
						if (!string.IsNullOrEmpty(startSequence))
						{
							list.Add(new FormatDescriptorItem(startSequence, false, false, true));
							startSequence = string.Empty;
						}
						list.Add(new FormatDescriptorItem(upToDigitsString, false, false, false));
					}
				}
				if (!string.IsNullOrEmpty(startSequence))
				{
					list.Add(new FormatDescriptorItem(startSequence, false, false, false));
					startSequence = string.Empty;
				}
				return list;
			};
			formatDescriptor.AddItem(new FormatDescriptorItemComposite(getItems, this.currentSequence));
			this.currentSequence = string.Empty;
		}

		string GetFractionStringByDenominator(double value, int denom, bool isResultStringEmpty)
		{
			int length = denom.ToString().Length;
			double value2 = value * (double)denom;
			double num = Math.Round(value2, length);
			double num2 = 1.0 / (double)(denom * 2);
			if (value < num2)
			{
				if (!isResultStringEmpty)
				{
					return "";
				}
				return "0";
			}
			else
			{
				if (value < 1.0 - num2)
				{
					if (num - 0.5 < 1E-06)
					{
						num += 0.1;
					}
					return Math.Round(num).ToString() + "/" + denom.ToString();
				}
				if (!isResultStringEmpty)
				{
					return "";
				}
				return "1";
			}
		}

		protected void GetConsecutiveDigitRange(string sbPart, int index, out int newIndex)
		{
			newIndex = index;
			bool flag = false;
			for (int i = index; i < sbPart.Length; i++)
			{
				if (!char.IsDigit(sbPart[i]))
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

		string GetUpToDigits(double number, int upTo)
		{
			Tuple<int, int> tuple = null;
			List<Tuple<int, int>> approximateFractions = this.GetApproximateFractions(number);
			for (int i = approximateFractions.Count - 1; i >= 0; i--)
			{
				if (approximateFractions[i].Item2.ToString().Length <= upTo)
				{
					tuple = approximateFractions[i];
					break;
				}
			}
			if (tuple.Item1 == 0)
			{
				return "0";
			}
			if (tuple.Item1 == tuple.Item2)
			{
				return "1";
			}
			return tuple.Item1.ToString() + "/" + tuple.Item2.ToString();
		}

		List<Tuple<int, int>> GetApproximateFractions(double number)
		{
			List<Tuple<int, int>> list = new List<Tuple<int, int>>();
			double[] array = new double[1000];
			array[0] = 0.0;
			array[1] = 1.0;
			double[] array2 = new double[1000];
			array2[0] = 1.0;
			array2[1] = 0.0;
			ulong maxNumerator = this.GetMaxNumerator(number);
			double num = number;
			double num2 = -1.0;
			for (int i = 2; i < 1000; i++)
			{
				double num3 = Math.Floor(num);
				array[i] = num3 * array[i - 1] + array[i - 2];
				if (Math.Abs(array[i]) > maxNumerator)
				{
					return list;
				}
				array2[i] = num3 * array2[i - 1] + array2[i - 2];
				double num4 = array[i] / array2[i];
				if (num4 == num2)
				{
					return list;
				}
				list.Add(new Tuple<int, int>((int)array[i], (int)array2[i]));
				if (num4 == number)
				{
					return list;
				}
				num2 = num4;
				num = 1.0 / (num - num3);
			}
			return list;
		}

		ulong GetMaxNumerator(double f)
		{
			string text = f.ToString(FormatHelper.DefaultSpreadsheetCulture.CultureInfo);
			int num = text.IndexOf("E");
			if (num == -1)
			{
				num = text.IndexOf("e");
			}
			string text2;
			if (num == -1)
			{
				text2 = text;
			}
			else
			{
				text2 = text.Substring(0, num);
			}
			string text3 = null;
			string numberDecimalSeparator = FormatHelper.DefaultSpreadsheetCulture.NumberDecimalSeparator;
			int num2 = text2.ToString(FormatHelper.DefaultSpreadsheetCulture.CultureInfo).IndexOf(numberDecimalSeparator);
			if (num2 == -1)
			{
				text3 = text2;
			}
			else if (num2 == 0)
			{
				text3 = text2.Substring(1, text2.Length);
			}
			else if (num2 < text2.Length)
			{
				int num3 = num2 + 1;
				text3 = text2.Substring(0, num2) + text2.Substring(num3, text2.Length - num3);
			}
			string text4 = text3;
			int length = text4.ToString().Length;
			double num4 = f;
			int num5 = num4.ToString(FormatHelper.DefaultSpreadsheetCulture.CultureInfo).Length;
			if (num4 == 0.0)
			{
				num5 = 0;
			}
			int num6 = length - num5;
			ulong num7 = ulong.Parse(text4);
			int num8 = num6;
			while (num8 > 0 && num7 % 2UL == 0UL)
			{
				num7 /= 2UL;
				num8--;
			}
			int num9 = num6;
			while (num9 > 0 && num7 % 5UL == 0UL)
			{
				num7 /= 5UL;
				num9--;
			}
			return num7;
		}

		bool TryParseSymbolCulture(string modificator, FormatDescriptor formatDescriptor)
		{
			if (string.IsNullOrEmpty(modificator) || modificator[0] != '$')
			{
				return false;
			}
			if (modificator.Length == 4 && !modificator.Contains('-'))
			{
				this.currentSequence += modificator.Substring(1);
				return true;
			}
			int num = modificator.IndexOf('-');
			if (num != -1 && num != 1)
			{
				string text = modificator.Substring(1, num - 1);
				if (text.Contains('.') || text.Contains('\'') || Regex.IsMatch(text, FormatHelper.IsRtlRegExPattern) || text.Equals("R"))
				{
					base.AddCurrentSequenceToItemsAndClear(formatDescriptor, true);
					formatDescriptor.AddItem(new FormatDescriptorItem(text, false, false, false));
				}
				else
				{
					this.currentSequence += text;
				}
				return true;
			}
			return false;
		}

		bool TryParseCondition(string modificator, FormatDescriptor formatDescriptor)
		{
			StringBuilder stringBuilder = new StringBuilder(modificator);
			int startIndex;
			if ((startIndex = modificator.IndexOf("<=")) > -1)
			{
				string s = stringBuilder.Remove(startIndex, 2).ToString();
				decimal number;
				if (decimal.TryParse(s, out number))
				{
					formatDescriptor.Condition = (double num) => (decimal)num <= number;
					return true;
				}
				return false;
			}
			else if ((startIndex = modificator.IndexOf(">=")) > -1)
			{
				string s2 = stringBuilder.Remove(startIndex, 2).ToString();
				decimal number;
				if (decimal.TryParse(s2, out number))
				{
					formatDescriptor.Condition = (double num) => (decimal)num >= number;
					return true;
				}
				return false;
			}
			else if ((startIndex = modificator.IndexOf(">")) > -1)
			{
				string s3 = stringBuilder.Remove(startIndex, 1).ToString();
				decimal number;
				if (decimal.TryParse(s3, out number))
				{
					formatDescriptor.Condition = (double num) => (decimal)num > number;
					return true;
				}
				return false;
			}
			else if ((startIndex = modificator.IndexOf("<")) > -1)
			{
				string s4 = stringBuilder.Remove(startIndex, 1).ToString();
				decimal number;
				if (decimal.TryParse(s4, out number))
				{
					formatDescriptor.Condition = (double num) => (decimal)num < number;
					return true;
				}
				return false;
			}
			else
			{
				if ((startIndex = modificator.IndexOf("=")) <= -1)
				{
					return false;
				}
				string s5 = stringBuilder.Remove(startIndex, 1).ToString();
				decimal number;
				if (decimal.TryParse(s5, out number))
				{
					formatDescriptor.Condition = (double num) => (decimal)num == number;
					return true;
				}
				return false;
			}
		}

		void EscapeItemsText(FormatDescriptor formatDescriptor)
		{
			CultureInfo cultureInfo = formatDescriptor.Culture;
			if (cultureInfo == null)
			{
				cultureInfo = Thread.CurrentThread.CurrentCulture;
			}
			foreach (FormatDescriptorItem formatDescriptorItem in from fpi in formatDescriptor.Items
				where fpi.ApplyFormat
				select fpi)
			{
				formatDescriptorItem.Text = formatDescriptorItem.Text.Replace(cultureInfo.NumberFormat.CurrencyGroupSeparator, ",").Replace(cultureInfo.NumberFormat.CurrencyDecimalSeparator, ".");
			}
		}

		string GetFormatForScientificNumber(double number)
		{
			StringBuilder stringBuilder = new StringBuilder("0");
			string text = number.ToString();
			int num = 0;
			for (int i = 1; i < text.Length; i++)
			{
				double numericValue = char.GetNumericValue(text[i]);
				if (numericValue == 0.0 || i == text.Length - 1 || i == 6)
				{
					num = i - 1;
					break;
				}
			}
			if (num > 0)
			{
				if (num >= 5)
				{
					num = 5;
				}
				stringBuilder.Append(".");
				stringBuilder.Append(string.Concat(Enumerable.Repeat<string>("0", num)));
			}
			stringBuilder.Append("E+00");
			return stringBuilder.ToString();
		}

		void ValidateNumberFormatDescriptors(List<FormatDescriptor> formatStringParts)
		{
			string key = "Spreadsheet_ErrorExpressions_FormatStringIncorrectFormat";
			string message = "Format string is not in the correct format.";
			if ((formatStringParts.Count == 1 && formatStringParts[0].Condition != null) || formatStringParts.Count > 4)
			{
				throw new LocalizableException(message, new ArgumentException(message), key, null);
			}
			if (formatStringParts.Count > 1 && formatStringParts[0].Condition != null)
			{
				for (int i = formatStringParts.Count - 2; i >= 0; i--)
				{
					if (formatStringParts[i].Condition == null)
					{
						throw new LocalizableException(message, new ArgumentException(message), key, null);
					}
				}
			}
			if (formatStringParts.Count > 1 && formatStringParts[0].Condition == null)
			{
				for (int j = 1; j < formatStringParts.Count; j++)
				{
					if (formatStringParts[j].Condition != null)
					{
						throw new LocalizableException(message, new ArgumentException(message), key, null);
					}
				}
			}
			if (formatStringParts.Count > 1 && formatStringParts[formatStringParts.Count - 1].Text.Contains(FormatHelper.TextPlaceholder))
			{
				for (int k = 0; k < formatStringParts.Count - 1; k++)
				{
					if (formatStringParts[k].Text.Contains(FormatHelper.TextPlaceholder))
					{
						throw new LocalizableException(message, new ArgumentException(message), key, null);
					}
				}
			}
		}

		int valueToAdd;
	}
}
