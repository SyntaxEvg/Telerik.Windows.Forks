using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	static class CssValueTypesHelper
	{
		public static bool IsPercentValue(HtmlStylePropertyValue val)
		{
			return val.UnitType != null && val.UnitType.Value == UnitType.Percent;
		}

		public static bool IsLengthValue(HtmlStylePropertyValue val)
		{
			return val.UnitType != null && val.UnitType.Value != UnitType.Percent;
		}

		public static bool IsNumberValue(HtmlStylePropertyValue val)
		{
			return val.UnitType == null && !double.IsNaN(val.ValueAsDouble);
		}

		public static bool TryConvertNonRelativeLengthValue(HtmlStylePropertyValue val, out double result)
		{
			if (val.UnitType != null && CssValueTypesHelper.IsLengthValue(val) && !Unit.IsRelativeUnitType(val.UnitType.Value))
			{
				result = Unit.UnitToDip(val.ValueAsDouble, val.UnitType.Value);
				return true;
			}
			result = double.NaN;
			return false;
		}

		public static string ConvertToPixels(IHtmlExportContext context, double value)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			return string.Format("{0}px", CssValueTypesHelper.ToString(context, value));
		}

		public static string ConvertToPercents(IHtmlExportContext context, double value)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			return string.Format("{0}%", CssValueTypesHelper.ToString(context, value));
		}

		public static string ToString(IHtmlExportContext context, double value)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			return value.ToString(context.Culture);
		}

		public static string ToLower(IHtmlExportContext context, string value)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			return value.ToLower(context.Culture);
		}
	}
}
