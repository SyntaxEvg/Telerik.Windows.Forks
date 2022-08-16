using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class ThemableFontFamilyConverter : StringConverterBase<ThemableFontFamily>
	{
		public override bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out ThemableFontFamily result)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			string unparsedValue = property.UnparsedValue;
			IEnumerable<string> source = from s in unparsedValue.Split(new char[] { ',' })
				select s.Trim(new char[] { '\'', '"' });
			string text = source.First<string>();
			GenericHtmlFonts genericFonts = context.Settings.GenericFonts;
			string a;
			if ((a = text) != null)
			{
				if (a == "cursive")
				{
					result = genericFonts.Cursive;
					return true;
				}
				if (a == "fantasy")
				{
					result = genericFonts.Fantasy;
					return true;
				}
				if (a == "monospace")
				{
					result = genericFonts.Monospace;
					return true;
				}
				if (a == "sans-serif")
				{
					result = genericFonts.SansSerif;
					return true;
				}
				if (a == "serif")
				{
					result = genericFonts.Serif;
					return true;
				}
			}
			text = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(text);
			result = new ThemableFontFamily(text);
			return true;
		}

		public override bool ConvertBack(IHtmlExportContext context, ThemableFontFamily value, DocumentElementPropertiesBase properties, out string result)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ThemableFontFamily>(value, "value");
			result = value.GetActualValue(context.Document.Theme).ToString();
			return true;
		}
	}
}
