using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	static class LegacyConverters
	{
		public static NumericAttributeToPixelUnitTypeConverter NumericAttributeToPixelUnitTypeConverter { get; set; } = new NumericAttributeToPixelUnitTypeConverter();
	}
}
