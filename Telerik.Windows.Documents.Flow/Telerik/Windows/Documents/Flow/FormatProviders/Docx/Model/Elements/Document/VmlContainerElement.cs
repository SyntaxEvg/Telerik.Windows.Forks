using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Vector;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.Flow.Model.Watermarks;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class VmlContainerElement : DocxElementBase
	{
		public VmlContainerElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "pict";
			}
		}

		public Watermark Watermark
		{
			get
			{
				return this.watermark;
			}
			set
			{
				Guard.ThrowExceptionIfNull<Watermark>(value, "value");
				this.watermark = value;
			}
		}

		public ImageInline Image
		{
			get
			{
				return this.image;
			}
			set
			{
				Guard.ThrowExceptionIfNull<ImageInline>(value, "value");
				this.image = value;
			}
		}

		protected override bool ShouldImport(IDocxImportContext context)
		{
			return true;
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return this.EnumerateChildElements(context).Any<OpenXmlElementBase>();
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IDocxExportContext context)
		{
			if (this.watermark != null)
			{
				ShapeElement shapeElement = base.CreateElement<ShapeElement>("shape");
				shapeElement.SetAssociatedWatermarkElement(this.watermark);
				yield return shapeElement;
			}
			yield break;
		}

		protected override void OnAfterReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			if (childElement.ElementName == "shape")
			{
				ShapeElement shapeElement = (ShapeElement)childElement;
				if (shapeElement != null)
				{
					switch (shapeElement.ShapeType)
					{
					case ShapeTypes.Unknown:
						break;
					case ShapeTypes.Watermark:
						this.watermark = shapeElement.CreateWatermark();
						return;
					case ShapeTypes.Image:
						this.image = new ImageInline(context.Document, shapeElement.CreateImage());
						break;
					default:
						return;
					}
				}
			}
		}

		protected override void ClearOverride()
		{
			this.watermark = null;
			this.image = null;
		}

		Watermark watermark;

		ImageInline image;
	}
}
