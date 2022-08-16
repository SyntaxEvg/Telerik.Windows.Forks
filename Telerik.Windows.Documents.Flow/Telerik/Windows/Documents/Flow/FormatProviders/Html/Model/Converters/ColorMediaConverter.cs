using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class ColorMediaConverter : StringConverterBase<Color>
	{
		public override bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out Color result)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			bool result2;
			try
			{
				object obj = ColorConverter.ConvertFromString(property.Values[0].Value);
				result = (Color)obj;
				result2 = true;
			}
			catch (FormatException)
			{
				result = Colors.Transparent;
				result2 = false;
			}
			return result2;
		}

		public override bool ConvertBack(IHtmlExportContext context, Color value, DocumentElementPropertiesBase properties, out string result)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Color>(value, "value");
			string text = "#" + value.R.ToString("X2") + value.G.ToString("X2") + value.B.ToString("X2");
			result = text;
			return true;
		}
	}
}
