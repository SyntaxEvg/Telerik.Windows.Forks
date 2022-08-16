using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common
{
	class NonVisualPictureDrawingPropertiesElement : OpenXmlElementBase
	{
		public NonVisualPictureDrawingPropertiesElement(OpenXmlPartsManager partsManager, OpenXmlNamespace ns)
			: base(partsManager)
		{
			Guard.ThrowExceptionIfNull<OpenXmlNamespace>(ns, "ns");
			this.ns = ns;
			this.pictureLocking = base.RegisterChildElement<PictureLockingElement>("picLocks");
			this.preferRelativeResize = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("preferRelativeResize", false));
		}

		public override string ElementName
		{
			get
			{
				return "cNvPicPr";
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return this.ns;
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		public PictureLockingElement PictureLockingElement
		{
			get
			{
				return this.pictureLocking.Element;
			}
			set
			{
				this.pictureLocking.Element = value;
			}
		}

		public int PreferRelativeResize
		{
			get
			{
				if (this.preferRelativeResize.HasValue)
				{
					return this.preferRelativeResize.Value;
				}
				return 1;
			}
			set
			{
				this.preferRelativeResize.Value = value;
			}
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, Image image)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Image>(image, "image");
			base.CreateElement(this.pictureLocking);
			if (!image.PreferRelativeToOriginalResize)
			{
				this.PreferRelativeResize = 0;
			}
			this.PictureLockingElement.NoChangeAspect = (image.LockAspectRatio ? 1 : 0);
		}

		public void CopyPropertiesTo(IOpenXmlImportContext context, Image image)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Image>(image, "image");
			image.PreferRelativeToOriginalResize = this.PreferRelativeResize != 0;
			if (this.PictureLockingElement != null)
			{
				image.LockAspectRatio = this.PictureLockingElement.NoChangeAspect == 1;
			}
			else
			{
				image.LockAspectRatio = false;
			}
			base.ReleaseElement(this.pictureLocking);
		}

		readonly OpenXmlNamespace ns;

		readonly OpenXmlChildElement<PictureLockingElement> pictureLocking;

		readonly IntOpenXmlAttribute preferRelativeResize;
	}
}
