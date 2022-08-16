using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class HyperlinkElement : ParagraphContentElementBase
	{
		public HyperlinkElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.anchorAttribute = base.RegisterAttribute<string>("anchor", OpenXmlNamespaces.WordprocessingMLNamespace, false);
			this.tooltipAttribute = base.RegisterAttribute<string>("tooltip", OpenXmlNamespaces.WordprocessingMLNamespace, false);
			this.relationshipIdAttribute = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("id", OpenXmlNamespaces.OfficeDocumentRelationshipsNamespace, false));
		}

		public override string ElementName
		{
			get
			{
				return "hyperlink";
			}
		}

		public string RelationshipId
		{
			get
			{
				return this.relationshipIdAttribute.Value;
			}
			set
			{
				this.relationshipIdAttribute.Value = value;
			}
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Hyperlink hyperlink = new Hyperlink(context.Document);
			if (this.anchorAttribute.HasValue)
			{
				hyperlink.Uri = this.anchorAttribute.Value;
				hyperlink.IsAnchor = true;
			}
			else if (this.relationshipIdAttribute.HasValue)
			{
				hyperlink.Uri = base.PartsManager.GetRelationshipTarget(base.Part.Name, this.RelationshipId);
			}
			if (this.tooltipAttribute.HasValue)
			{
				hyperlink.ToolTip = this.tooltipAttribute.Value;
			}
			FieldInfo fieldInfo = new FieldInfo(context.Document, hyperlink);
			base.Paragraph.Inlines.Add(fieldInfo.Start);
			base.Paragraph.Inlines.AddRun().Text = hyperlink.CreateHyperlinkCode();
			base.Paragraph.Inlines.Add(fieldInfo.Separator);
			base.MoveInlinesToParagraph();
			base.Paragraph.Inlines.Add(fieldInfo.End);
		}

		readonly OpenXmlAttribute<string> anchorAttribute;

		readonly OpenXmlAttribute<string> tooltipAttribute;

		readonly OpenXmlAttribute<string> relationshipIdAttribute;
	}
}
