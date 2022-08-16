using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class AutomaticSpacingBeforeConverter : StringConverterBase<bool>
	{
		public override bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out bool result)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			double num;
			if (HtmlConverters.SpacingBeforeConverter.Convert(context, property, properties, out num))
			{
				result = false;
				return true;
			}
			result = false;
			return false;
		}

		public override bool ConvertBack(IHtmlExportContext context, bool value, DocumentElementPropertiesBase properties, out string result)
		{
			throw new NotSupportedException();
		}
	}
}
