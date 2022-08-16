using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Pictures
{
	abstract class PictureFillElementBase : PictureElementBase
	{
		public PictureFillElementBase(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.blip = base.RegisterChildElement<BlipElement>("blip");
			this.stretch = base.RegisterChildElement<StretchElement>("stretch");
		}

		public override string ElementName
		{
			get
			{
				return "blipFill";
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		public BlipElement BlipElement
		{
			get
			{
				return this.blip.Element;
			}
			set
			{
				this.blip.Element = value;
			}
		}

		public StretchElement StretchElement
		{
			get
			{
				return this.stretch.Element;
			}
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, Image image)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Image>(image, "image");
			base.CreateElement(this.blip);
			this.BlipElement.RelationshipId = this.CreateRelationshipIdFromImageSource(context, image.ImageSource);
			base.CreateElement(this.stretch);
			this.StretchElement.CopyPropertiesFrom(context, image);
		}

		public void CopyPropertiesTo(IOpenXmlImportContext context, Image image)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Image>(image, "image");
			if (this.BlipElement != null)
			{
				image.ImageSource = this.CreateImageSourceFromRelationshipId(context, this.BlipElement.RelationshipId);
				base.ReleaseElement(this.blip);
			}
			if (this.StretchElement != null)
			{
				this.StretchElement.CopyPropertiesTo(context, image);
				base.ReleaseElement(this.stretch);
			}
		}

		protected abstract string CreateRelationshipIdFromImageSource(IOpenXmlExportContext context, ImageSource imageSource);

		protected abstract ImageSource CreateImageSourceFromRelationshipId(IOpenXmlImportContext context, string relationshipId);

		readonly OpenXmlChildElement<BlipElement> blip;

		readonly OpenXmlChildElement<StretchElement> stretch;
	}
}
