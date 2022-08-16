using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	abstract class FontElementBase : ThemeElementBase
	{
		public FontElementBase(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.typeface = base.RegisterAttribute<string>("typeface", false);
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.DrawingMLNamespace;
			}
		}

		public string Typeface
		{
			get
			{
				return this.typeface.Value;
			}
			set
			{
				this.typeface.Value = value;
			}
		}

		public void SetFontName(ThemeFont themeFont)
		{
			this.Typeface = ((themeFont == null) ? string.Empty : themeFont.FontFamily.Source);
		}

		public string GetFontName(IOpenXmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			if (this.typeface.HasValue)
			{
				return this.Typeface;
			}
			return null;
		}

		readonly OpenXmlAttribute<string> typeface;
	}
}
