using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.DefaultStyles
{
	class DocumentDefaultStylesElement : DocxElementBase
	{
		public DocumentDefaultStylesElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.runDefaultPropertiesChildElement = base.RegisterChildElement<RunDefaultPropertiesElement>("rPrDefault");
			this.paragraphDefaultPropertiesChildElement = base.RegisterChildElement<ParagraphDefaultPropertiesElement>("pPrDefault");
		}

		public override string ElementName
		{
			get
			{
				return "docDefaults";
			}
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return true;
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			if (context.Document.DefaultStyle.CharacterProperties.HasLocalValues())
			{
				base.CreateElement(this.runDefaultPropertiesChildElement);
			}
			if (context.Document.DefaultStyle.ParagraphProperties.HasLocalValues())
			{
				base.CreateElement(this.paragraphDefaultPropertiesChildElement);
			}
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			base.ReleaseElement(this.runDefaultPropertiesChildElement);
			base.ReleaseElement(this.paragraphDefaultPropertiesChildElement);
		}

		readonly OpenXmlChildElement<RunDefaultPropertiesElement> runDefaultPropertiesChildElement;

		readonly OpenXmlChildElement<ParagraphDefaultPropertiesElement> paragraphDefaultPropertiesChildElement;
	}
}
