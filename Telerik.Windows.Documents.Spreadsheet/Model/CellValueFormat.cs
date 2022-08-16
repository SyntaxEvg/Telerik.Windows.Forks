using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class CellValueFormat
	{
		public string FormatString
		{
			get
			{
				return this.formatString;
			}
		}

		public FormatStringInfo FormatStringInfo
		{
			get
			{
				return this.formatStringParser.FormatStringInfo;
			}
		}

		internal string InvariantFormatString
		{
			get
			{
				return this.invariantFormatString;
			}
		}

		public CellValueFormat(string formatString)
			: this(formatString, true)
		{
		}

		internal CellValueFormat(string formatString, bool convertToInvariant)
		{
			Guard.ThrowExceptionIfNull<string>(formatString, "formatString");
			FormatStringType? formatStringType;
			FormatStringValidator.ValidateAndGetFormatStringType(formatString, out formatStringType);
			bool flag = formatStringType != null && formatStringType.Value == FormatStringType.Number;
			string text = formatString;
			if (flag)
			{
				if (convertToInvariant)
				{
					text = FormatHelper.ReplaceCultureSpecificSeparatorsWithDefaults(formatString);
				}
				else
				{
					formatString = FormatHelper.ReplaceDefaultSeparatorsWithCultureSpecific(text);
				}
			}
			this.formatStringParser = TextFormatStringParser.Create(text, formatString);
			this.invariantFormatString = text;
			this.formatString = formatString;
		}

		public CellValueFormatResult GetFormatResult(ICellValue cellValue)
		{
			List<CellValueFormatResultItem> infos = this.formatStringParser.ApplyFormatToValues(cellValue, this);
			Color? foregroundOrDefault = this.formatStringParser.FormatDescriptor.GetForegroundOrDefault();
			CultureInfo cultureOrDefault = this.formatStringParser.FormatDescriptor.GetCultureOrDefault();
			Predicate<double> conditionOrDefault = this.formatStringParser.FormatDescriptor.GetConditionOrDefault();
			return new CellValueFormatResult(infos, foregroundOrDefault, conditionOrDefault, cultureOrDefault);
		}

		public static bool operator ==(CellValueFormat first, CellValueFormat second)
		{
			return object.ReferenceEquals(first, second) || (first != null && first.Equals(second));
		}

		public static bool operator !=(CellValueFormat first, CellValueFormat second)
		{
			return !(first == second);
		}

		public override bool Equals(object obj)
		{
			CellValueFormat cellValueFormat = obj as CellValueFormat;
			return !(cellValueFormat == null) && TelerikHelper.EqualsOfT<string>(this.FormatString, cellValueFormat.FormatString);
		}

		public override int GetHashCode()
		{
			return this.FormatString.GetHashCode();
		}

		public static readonly CellValueFormat GeneralFormat = new CellValueFormat(FormatHelper.GeneralFormatString);

		readonly TextFormatStringParser formatStringParser;

		readonly string formatString;

		readonly string invariantFormatString;
	}
}
