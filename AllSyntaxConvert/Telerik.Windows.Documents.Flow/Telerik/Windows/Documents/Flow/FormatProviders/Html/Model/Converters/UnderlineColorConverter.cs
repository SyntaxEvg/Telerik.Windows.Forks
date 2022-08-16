﻿using System;
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
	class UnderlineColorConverter : StringConverterBase<ThemableColor>
	{
		public override bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out ThemableColor result)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			if (property.Values[0].Value == "underline")
			{
				IStyleProperty styleProperty = properties.GetStyleProperty(Run.ForegroundColorPropertyDefinition);
				if (styleProperty != null)
				{
					result = (ThemableColor)styleProperty.GetActualValueAsObject();
					return true;
				}
			}
			result = null;
			return false;
		}

		public override bool ConvertBack(IHtmlExportContext context, ThemableColor value, DocumentElementPropertiesBase properties, out string result)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ThemableColor>(value, "value");
			result = null;
			return false;
		}
	}
}
