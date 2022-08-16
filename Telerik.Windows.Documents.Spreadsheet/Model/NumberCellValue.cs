using System;
using System.Globalization;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class NumberCellValue : CellValueBase<double>
	{
		public override CellValueType ValueType
		{
			get
			{
				return CellValueType.Number;
			}
		}

		public override string RawValue
		{
			get
			{
				return base.Value.ToString(FormatHelper.DefaultSpreadsheetCulture.CultureInfo);
			}
		}

		internal NumberCellValue(double doubleValue)
			: base(doubleValue)
		{
		}

		internal string ToString(FormatStringType formatType)
		{
			if (formatType == FormatStringType.Number)
			{
				return base.Value.ToString();
			}
			if (formatType != FormatStringType.DateTime)
			{
				throw new ArgumentException("Invalid format string type.");
			}
			if (this.ToDateTime() == null)
			{
				return base.Value.ToString();
			}
			return this.ToDateTime().ToString();
		}

		string GetEditModeFormatString(string value, CellValueFormat format)
		{
			double num = double.Parse(value, FormatHelper.DefaultSpreadsheetCulture.CultureInfo);
			bool flag = num - Math.Floor(num) != 0.0;
			bool flag2 = Math.Floor(num) != 0.0;
			if (format.FormatStringInfo.FormatType == FormatStringType.DateTime && !flag2)
			{
				return FormatHelper.LongTimePattern;
			}
			if (format.FormatStringInfo.FormatType == FormatStringType.DateTime && flag)
			{
				return FormatHelper.ShortDateLongTimePattern;
			}
			if (format.FormatStringInfo.FormatType == FormatStringType.DateTime)
			{
				return FormatHelper.ShortDatePattern;
			}
			if (format.FormatStringInfo.FormatType == FormatStringType.Number && format.FormatString.Contains("%"))
			{
				double num2;
				bool flag3 = double.TryParse(value, NumberStyles.Any, FormatHelper.DefaultSpreadsheetCulture.CultureInfo, out num2);
				if (flag3)
				{
					string text = (num2 * 100.0).ToString(FormatHelper.DefaultSpreadsheetCulture.CultureInfo);
					string result = string.Empty;
					int num3;
					if ((num3 = text.IndexOf(FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.PercentDecimalSeparator)) != -1)
					{
						int num4 = 0;
						int num5 = num3 + 1;
						while (num5 < text.Length && char.IsDigit(text[num5]))
						{
							num4++;
							num5++;
						}
						result = string.Format("0.{0}%", string.Concat<char>(Enumerable.Repeat<char>('0', num4)));
					}
					else
					{
						result = "0%";
					}
					return result;
				}
			}
			if (!(format.FormatStringInfo.FormatType == FormatStringType.Number))
			{
				return string.Empty;
			}
			double num6 = Math.Abs(num);
			if (num6 == 0.0 || (num6 >= FormatHelper.MinGeneralNumber && num6 <= FormatHelper.MaxGeneralNumber))
			{
				return FormatHelper.GeneralNumberEditFormatString;
			}
			return FormatHelper.GeneralNumberScientificEditFormatString;
		}

		protected override string GetValueAsStringOverride(CellValueFormat format)
		{
			if (format.FormatStringInfo.FormatType == FormatStringType.DateTime)
			{
				string valueAsString = base.GetValueAsString(CellValueFormat.GeneralFormat);
				string editModeFormatString = this.GetEditModeFormatString(valueAsString, format);
				if (base.Value < 0.0 || base.Value > FormatHelper.MaxDoubleValueTranslatableToDateTime)
				{
					return this.ToString(format.FormatStringInfo.FormatType.Value);
				}
				return this.ToDateTime().Value.RoundMinutes().ToString(editModeFormatString, FormatHelper.DefaultSpreadsheetCulture.CultureInfo);
			}
			else
			{
				if (format.FormatStringInfo.FormatType == FormatStringType.Number)
				{
					string editModeFormatString2 = this.GetEditModeFormatString(this.RawValue, format);
					return base.Value.ToString(editModeFormatString2, FormatHelper.DefaultSpreadsheetCulture.CultureInfo);
				}
				return base.Value.ToString();
			}
		}

		public override bool Equals(object obj)
		{
			NumberCellValue numberCellValue = obj as NumberCellValue;
			return numberCellValue != null && base.Value.Equals(numberCellValue.Value);
		}

		public override int GetHashCode()
		{
			return base.Value.GetHashCode();
		}
	}
}
