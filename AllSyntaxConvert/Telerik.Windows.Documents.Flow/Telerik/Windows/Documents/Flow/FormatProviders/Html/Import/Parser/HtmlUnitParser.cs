using System;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Media;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Import.Parser
{
	static class HtmlUnitParser
	{
		internal static bool Parse(string value, out string parsedValue, out UnitType? unitType)
		{
			parsedValue = string.Empty;
			unitType = null;
			bool flag = HtmlUnitParser.invalidValueRegex.IsMatch(value);
			if (flag)
			{
				return false;
			}
			Match match = HtmlUnitParser.unitRegex.Match(value);
			if (match.Length > 0)
			{
				parsedValue = value.Substring(0, value.Length - match.Length);
			}
			string value2;
			switch (value2 = match.Value)
			{
			case "px":
				unitType = new UnitType?(UnitType.Dip);
				break;
			case "pt":
				unitType = new UnitType?(UnitType.Point);
				break;
			case "em":
				unitType = new UnitType?(UnitType.Em);
				break;
			case "%":
				unitType = new UnitType?(UnitType.Percent);
				break;
			case "cm":
				unitType = new UnitType?(UnitType.Cm);
				break;
			case "mm":
				unitType = new UnitType?(UnitType.Mm);
				break;
			case "in":
				unitType = new UnitType?(UnitType.Inch);
				break;
			}
			if (unitType == null && HtmlUnitParser.valueRegex.IsMatch(value))
			{
				parsedValue = value;
			}
			return unitType != null && !string.IsNullOrEmpty(parsedValue);
		}

		static readonly Regex invalidValueRegex = new Regex("\\s+", RegexOptions.Compiled);

		static readonly Regex unitRegex = new Regex("[a-z%]{1,2}", RegexOptions.Compiled);

		static readonly Regex valueRegex = new Regex("^[0-9]*$", RegexOptions.Compiled);
	}
}
