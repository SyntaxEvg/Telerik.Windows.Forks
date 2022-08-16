using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class ThemeElement : OpenXmlPartRootElementBase
	{
		public ThemeElement(OpenXmlPartsManager partsManager, OpenXmlPartBase part)
			: base(partsManager, part)
		{
			this.name = base.RegisterAttribute<string>("name", string.Empty, false);
			this.themeElements = base.RegisterChildElement<ThemeElementsElement>("themeElements");
		}

		public override string ElementName
		{
			get
			{
				return "theme";
			}
		}

		public string Name
		{
			get
			{
				return this.name.Value;
			}
			set
			{
				this.name.Value = value;
			}
		}

		public ThemeElementsElement ThemeElementsElement
		{
			get
			{
				return this.themeElements.Element;
			}
			set
			{
				this.themeElements.Element = value;
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.DrawingMLNamespace;
			}
		}

		protected override void OnBeforeWrite(IOpenXmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			this.Name = context.Theme.Name;
			base.CreateElement(this.themeElements);
		}

		protected override void OnAfterRead(IOpenXmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			if (this.ThemeElementsElement != null)
			{
				ThemeColorScheme colorScheme = this.ThemeElementsElement.CreateColorScheme(context);
				ThemeFontScheme fontScheme = this.ThemeElementsElement.CreateFontScheme(context);
				context.Theme = new DocumentTheme(this.Name, colorScheme, fontScheme);
			}
			base.ReleaseElement(this.themeElements);
		}

		readonly OpenXmlAttribute<string> name;

		readonly OpenXmlChildElement<ThemeElementsElement> themeElements;
	}
}
