using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.DefaultStyles;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles
{
	class StylesElement : DocxPartRootElementBase
	{
		public StylesElement(DocxPartsManager partsManager, OpenXmlPartBase part)
			: base(partsManager, part)
		{
			this.docDefaultsChildElement = base.RegisterChildElement<DocumentDefaultStylesElement>("docDefaults");
		}

		public override string ElementName
		{
			get
			{
				return "styles";
			}
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return true;
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			base.CreateElement(this.docDefaultsChildElement);
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			foreach (Style style in context.Document.StyleRepository.Styles)
			{
				StyleElement styleElement = base.CreateElement<StyleElement>("style");
				styleElement.Style = style;
				yield return styleElement;
			}
			yield break;
		}

		protected override void OnAfterReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			string elementName;
			if ((elementName = childElement.ElementName) != null)
			{
				if (!(elementName == "style"))
				{
					return;
				}
				context.Document.StyleRepository.Add((childElement as StyleElement).Style);
			}
		}

		readonly OpenXmlChildElement<DocumentDefaultStylesElement> docDefaultsChildElement;
	}
}
