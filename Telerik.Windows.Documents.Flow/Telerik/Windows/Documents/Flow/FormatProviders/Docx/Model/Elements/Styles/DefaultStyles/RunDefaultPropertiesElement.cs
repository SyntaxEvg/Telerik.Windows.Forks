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
	class RunDefaultPropertiesElement : DocxElementBase
	{
		public RunDefaultPropertiesElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.runPropertiesChildElement = base.RegisterChildElement<RunPropertiesElement>("rPr");
		}

		public override string ElementName
		{
			get
			{
				return "rPrDefault";
			}
		}

		RunPropertiesElement RunPropertiesElement
		{
			get
			{
				return this.runPropertiesChildElement.Element;
			}
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return true;
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			CharacterProperties characterProperties = context.Document.DefaultStyle.CharacterProperties;
			if (characterProperties.HasLocalValues())
			{
				base.CreateElement(this.runPropertiesChildElement);
				this.RunPropertiesElement.SetAssociatedFlowModelElement(characterProperties);
			}
		}

		protected override void OnBeforeReadChildElement(IDocxImportContext context, OpenXmlElementBase element)
		{
			string elementName;
			if ((elementName = element.ElementName) != null)
			{
				if (!(elementName == "rPr"))
				{
					return;
				}
				this.RunPropertiesElement.SetAssociatedFlowModelElement(context.Document.DefaultStyle.CharacterProperties);
			}
		}

		readonly OpenXmlChildElement<RunPropertiesElement> runPropertiesChildElement;
	}
}
