using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class ThemableColorConverter : StringConverterBase<ThemableColor>
	{
		public override bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out ThemableColor result)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			bool result2;
			try
			{
				object obj = ColorConverter.ConvertFromString(property.Values[0].Value);
				result = new ThemableColor((Color)obj);
				result2 = true;
			}
			catch (FormatException)
			{
				result = null;
				result2 = false;
			}
			return result2;
		}

		public override bool ConvertBack(IHtmlExportContext context, ThemableColor value, DocumentElementPropertiesBase properties, out string result)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ThemableColor>(value, "value");
			Color actualValue = value.GetActualValue(context.Document.Theme);
			string text = "#" + actualValue.R.ToString("X2") + actualValue.G.ToString("X2") + actualValue.B.ToString("X2");
			result = text;
			return true;
		}
	}
}
