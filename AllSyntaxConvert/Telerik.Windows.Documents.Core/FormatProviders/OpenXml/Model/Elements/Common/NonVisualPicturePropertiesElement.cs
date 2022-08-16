using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common
{
	class NonVisualPicturePropertiesElement : OpenXmlElementBase
	{
		public NonVisualPicturePropertiesElement(OpenXmlPartsManager partsManager, OpenXmlNamespace ns)
			: base(partsManager)
		{
			Guard.ThrowExceptionIfNull<OpenXmlNamespace>(ns, "ns");
			this.ns = ns;
			this.nonVisualDrawingPropertiesElement = base.RegisterChildElement<NonVisualDrawingPropertiesElement>("cNvPr");
			this.nonVisualPictureDrawingPropertiesElement = base.RegisterChildElement<NonVisualPictureDrawingPropertiesElement>("cNvPicPr");
		}

		public override string ElementName
		{
			get
			{
				return "nvPicPr";
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return this.ns;
			}
		}

		public NonVisualPictureDrawingPropertiesElement NonVisualPictureDrawingPropertiesElement
		{
			get
			{
				return this.nonVisualPictureDrawingPropertiesElement.Element;
			}
			set
			{
				this.nonVisualPictureDrawingPropertiesElement.Element = value;
			}
		}

		public NonVisualDrawingPropertiesElement NonVisualDrawingPropertiesElement
		{
			get
			{
				return this.nonVisualDrawingPropertiesElement.Element;
			}
			set
			{
				this.nonVisualDrawingPropertiesElement.Element = value;
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, Image image)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Image>(image, "image");
			base.CreateElement(this.nonVisualPictureDrawingPropertiesElement);
			base.CreateElement(this.nonVisualDrawingPropertiesElement);
			this.NonVisualDrawingPropertiesElement.Id = image.Id;
			this.NonVisualDrawingPropertiesElement.Name = image.Name;
			this.NonVisualPictureDrawingPropertiesElement.CopyPropertiesFrom(context, image);
		}

		public void CopyPropertiesTo(IOpenXmlImportContext context, Image image)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Image>(image, "image");
			image.Name = this.NonVisualDrawingPropertiesElement.Name;
			this.NonVisualPictureDrawingPropertiesElement.CopyPropertiesTo(context, image);
			base.ReleaseElement(this.nonVisualPictureDrawingPropertiesElement);
			base.ReleaseElement(this.nonVisualDrawingPropertiesElement);
		}

		readonly OpenXmlNamespace ns;

		readonly OpenXmlChildElement<NonVisualDrawingPropertiesElement> nonVisualDrawingPropertiesElement;

		readonly OpenXmlChildElement<NonVisualPictureDrawingPropertiesElement> nonVisualPictureDrawingPropertiesElement;
	}
}
