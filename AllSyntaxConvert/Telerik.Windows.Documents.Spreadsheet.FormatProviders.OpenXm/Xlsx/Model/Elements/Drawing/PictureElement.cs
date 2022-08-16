using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Drawing
{
	class PictureElement : WorksheetDrawingElementBase
	{
		public PictureElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.nonVisualPictureProperties = base.RegisterChildElement<NonVisualPicturePropertiesElement>("nvPicPr");
			this.blipFill = base.RegisterChildElement<BlipFillElement>("blipFill");
			this.shapeProperties = base.RegisterChildElement<ShapePropertiesElement>("spPr");
		}

		public override string ElementName
		{
			get
			{
				return "pic";
			}
		}

		public NonVisualPicturePropertiesElement NonVisualPicturePropertiesElement
		{
			get
			{
				return this.nonVisualPictureProperties.Element;
			}
			set
			{
				this.nonVisualPictureProperties.Element = value;
			}
		}

		public BlipFillElement BlipFillElement
		{
			get
			{
				return this.blipFill.Element;
			}
			set
			{
				this.blipFill.Element = value;
			}
		}

		public ShapePropertiesElement ShapePropertiesElement
		{
			get
			{
				return this.shapeProperties.Element;
			}
			set
			{
				this.shapeProperties.Element = value;
			}
		}

		public void CopyPropertiesFrom(IXlsxWorksheetExportContext context, Image image)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Image>(image, "image");
			base.CreateElement(this.nonVisualPictureProperties);
			this.NonVisualPicturePropertiesElement.CopyPropertiesFrom(context, image);
			base.CreateElement(this.blipFill);
			this.BlipFillElement.CopyPropertiesFrom(context, image);
			base.CreateElement(this.shapeProperties);
			this.ShapePropertiesElement.CopyPropertiesFrom(context, image);
		}

		public void CopyPropertiesTo(IXlsxWorksheetImportContext context, Image image)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			if (this.BlipFillElement != null)
			{
				this.BlipFillElement.CopyPropertiesTo(context, image);
				base.ReleaseElement(this.blipFill);
			}
			if (this.NonVisualPicturePropertiesElement != null)
			{
				this.NonVisualPicturePropertiesElement.CopyPropertiesTo(context, image);
				base.ReleaseElement(this.nonVisualPictureProperties);
			}
			if (this.ShapePropertiesElement != null)
			{
				this.ShapePropertiesElement.CopyPropertiesTo(context, image);
				base.ReleaseElement(this.shapeProperties);
			}
		}

		readonly OpenXmlChildElement<NonVisualPicturePropertiesElement> nonVisualPictureProperties;

		readonly OpenXmlChildElement<BlipFillElement> blipFill;

		readonly OpenXmlChildElement<ShapePropertiesElement> shapeProperties;
	}
}
