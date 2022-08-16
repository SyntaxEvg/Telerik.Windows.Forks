using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Pictures
{
	class PictureElement : PictureElementBase
	{
		public PictureElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.nonVisualPictureProperties = base.RegisterChildElement<NonVisualPicturePropertiesElement>("nvPicPr");
			this.pictureFill = base.RegisterChildElement<PictureFillElementBase>("blipFill");
			this.shapeProperties = base.RegisterChildElement<ShapePropertiesElement>("spPr", "xdr:spPr");
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

		public PictureFillElementBase PictureFillElement
		{
			get
			{
				return this.pictureFill.Element;
			}
			set
			{
				this.pictureFill.Element = value;
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

		public void CopyPropertiesFrom(IOpenXmlExportContext context, Image image)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Image>(image, "image");
			base.CreateElement(this.nonVisualPictureProperties);
			this.NonVisualPicturePropertiesElement.CopyPropertiesFrom(context, image);
			base.CreateElement(this.pictureFill);
			this.PictureFillElement.CopyPropertiesFrom(context, image);
			base.CreateElement(this.shapeProperties);
			this.ShapePropertiesElement.CopyPropertiesFrom(context, image);
		}

		public Image CopyPropertiesTo(IOpenXmlImportContext context, Image image)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			if (this.PictureFillElement != null)
			{
				this.PictureFillElement.CopyPropertiesTo(context, image);
				base.ReleaseElement(this.pictureFill);
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
			return image;
		}

		readonly OpenXmlChildElement<NonVisualPicturePropertiesElement> nonVisualPictureProperties;

		readonly OpenXmlChildElement<PictureFillElementBase> pictureFill;

		readonly OpenXmlChildElement<ShapePropertiesElement> shapeProperties;
	}
}
