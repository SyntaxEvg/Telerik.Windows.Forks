using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.DefaultStyles
{
	class ParagraphDefaultPropertiesElement : DocxElementBase
	{
		public ParagraphDefaultPropertiesElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.paragraphPropertiesChildElement = base.RegisterChildElement<ParagraphPropertiesElement>("pPr");
		}

		public override string ElementName
		{
			get
			{
				return "pPrDefault";
			}
		}

		ParagraphPropertiesElement ParagraphPropertiesElement
		{
			get
			{
				return this.paragraphPropertiesChildElement.Element;
			}
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return true;
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			ParagraphProperties paragraphProperties = context.Document.DefaultStyle.ParagraphProperties;
			if (paragraphProperties.HasLocalValues())
			{
				base.CreateElement(this.paragraphPropertiesChildElement);
				this.ParagraphPropertiesElement.SetAssociatedFlowModelElement(paragraphProperties);
			}
		}

		protected override void OnBeforeReadChildElement(IDocxImportContext context, OpenXmlElementBase element)
		{
			string elementName;
			if ((elementName = element.ElementName) != null)
			{
				if (!(elementName == "pPr"))
				{
					return;
				}
				this.ParagraphPropertiesElement.SetAssociatedFlowModelElement(context.Document.DefaultStyle.ParagraphProperties);
			}
		}

		readonly OpenXmlChildElement<ParagraphPropertiesElement> paragraphPropertiesChildElement;
	}
}
