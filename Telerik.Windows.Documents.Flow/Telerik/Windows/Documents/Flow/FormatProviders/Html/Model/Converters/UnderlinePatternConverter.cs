using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class UnderlinePatternConverter : StringConverterBase<UnderlinePattern>
	{
		public override bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out UnderlinePattern result)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			if (property.Values[0].Value == "underline")
			{
				result = UnderlinePattern.Single;
				return true;
			}
			result = UnderlinePattern.None;
			return false;
		}

		public override bool ConvertBack(IHtmlExportContext context, UnderlinePattern value, DocumentElementPropertiesBase properties, out string result)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<UnderlinePattern>(value, "value");
			if (value != UnderlinePattern.None)
			{
				StyleProperty<ThemableColor> styleProperty = properties.GetStyleProperty(Run.UnderlineColorPropertyDefinition) as StyleProperty<ThemableColor>;
				if (styleProperty != null)
				{
					ThemableColor actualValue = styleProperty.GetActualValue();
					if (actualValue.IsAutomatic || actualValue.GetActualValue(context.Document.Theme).A > 0)
					{
						result = "underline";
						return true;
					}
				}
			}
			result = null;
			return false;
		}
	}
}
