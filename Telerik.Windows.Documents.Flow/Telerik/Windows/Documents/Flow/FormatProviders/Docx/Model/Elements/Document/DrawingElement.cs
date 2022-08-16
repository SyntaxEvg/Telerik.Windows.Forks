using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Drawing;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class DrawingElement : DocumentElementBase
	{
		public DrawingElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.inline = base.RegisterChildElement<InlineElement>("inline");
			this.anchor = base.RegisterChildElement<AnchorElement>("anchor");
		}

		public override string ElementName
		{
			get
			{
				return "drawing";
			}
		}

		public InlineElement InlineElement
		{
			get
			{
				return this.inline.Element;
			}
		}

		public AnchorElement AnchorElement
		{
			get
			{
				return this.anchor.Element;
			}
		}

		public void CopyPropertiesFrom(IDocxExportContext context, ShapeInlineBase shape)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeInlineBase>(shape, "shape");
			base.CreateElement(this.inline);
			this.InlineElement.CopyPropertiesFrom(context, shape);
		}

		public void CopyPropertiesFrom(IDocxExportContext context, ShapeAnchorBase anchor)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeAnchorBase>(anchor, "anchor");
			base.CreateElement(this.anchor);
			this.AnchorElement.CopyPropertiesFrom(context, anchor);
		}

		public InlineBase CreateInline(IDocxImportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			InlineBase result = null;
			if (this.InlineElement != null)
			{
				result = this.InlineElement.CreateShapeInline(context);
				base.ReleaseElement(this.inline);
			}
			if (this.AnchorElement != null)
			{
				result = this.AnchorElement.CreateShapeInline(context);
				base.ReleaseElement(this.anchor);
			}
			return result;
		}

		readonly OpenXmlChildElement<InlineElement> inline;

		readonly OpenXmlChildElement<AnchorElement> anchor;
	}
}
