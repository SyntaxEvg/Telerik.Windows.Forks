using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing
{
	class TransformElement : OpenXmlElementBase
	{
		public TransformElement(OpenXmlPartsManager partsManager, OpenXmlNamespace ns)
			: base(partsManager)
		{
			this.ns = ns;
			this.offset = base.RegisterChildElement<OffsetElement>("off");
			this.extents = base.RegisterChildElement<SizeElement>("ext");
			this.isHorizontallyFlipped = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("flipH"));
			this.isVerticallyFlipped = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("flipV"));
			this.rotationAngle = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("rot", false));
		}

		public override string ElementName
		{
			get
			{
				return "xfrm";
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return this.ns;
			}
		}

		public bool IsHorizontallyFlipped
		{
			get
			{
				return this.isHorizontallyFlipped.Value;
			}
			set
			{
				this.isHorizontallyFlipped.Value = value;
			}
		}

		public bool IsVerticallyFlipped
		{
			get
			{
				return this.isVerticallyFlipped.Value;
			}
			set
			{
				this.isVerticallyFlipped.Value = value;
			}
		}

		public int RotationAngle
		{
			get
			{
				return this.rotationAngle.Value;
			}
			set
			{
				this.rotationAngle.Value = value;
			}
		}

		public SizeElement ExtentsElement
		{
			get
			{
				return this.extents.Element;
			}
		}

		public OffsetElement OffsetElement
		{
			get
			{
				return this.offset.Element;
			}
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, ShapeBase shape)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeBase>(shape, "shape");
			if (!shape.SizeInternal.IsEmpty)
			{
				base.CreateElement(this.extents);
				this.ExtentsElement.ExtentWidth = (double)((int)shape.Width);
				this.ExtentsElement.ExtentHeight = (double)((int)shape.Height);
			}
			base.CreateElement(this.offset);
			this.OffsetElement.X = 0;
			this.OffsetElement.Y = 0;
			if (shape.IsHorizontallyFlipped)
			{
				this.IsHorizontallyFlipped = shape.IsHorizontallyFlipped;
			}
			if (shape.IsVerticallyFlipped)
			{
				this.IsVerticallyFlipped = shape.IsVerticallyFlipped;
			}
			if (shape.RotationAngle != 0.0)
			{
				this.RotationAngle = (int)(shape.RotationAngle * 60000.0);
			}
		}

		public void CopyPropertiesTo(IOpenXmlImportContext context, ShapeBase shape)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeBase>(shape, "shape");
			shape.RotationAngle = (double)this.RotationAngle / 60000.0;
			shape.IsHorizontallyFlipped = this.IsHorizontallyFlipped;
			shape.IsVerticallyFlipped = this.IsVerticallyFlipped;
			if (this.ExtentsElement != null && this.ExtentsElement.ExtentWidth != 0.0 && this.ExtentsElement.ExtentHeight != 0.0)
			{
				shape.Width = this.ExtentsElement.ExtentWidth;
				shape.Height = this.ExtentsElement.ExtentHeight;
			}
		}

		readonly OpenXmlNamespace ns;

		readonly OpenXmlChildElement<SizeElement> extents;

		readonly OpenXmlChildElement<OffsetElement> offset;

		readonly BoolOpenXmlAttribute isHorizontallyFlipped;

		readonly BoolOpenXmlAttribute isVerticallyFlipped;

		readonly IntOpenXmlAttribute rotationAngle;
	}
}
