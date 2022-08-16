using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Model.Drawing.Theming;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class OutlineElement : ThemeElementBase
	{
		public OutlineElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.width = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("w", false));
			this.solidFill = base.RegisterChildElement<SolidFillElement>("solidFill");
			this.noFill = base.RegisterChildElement<NoFillElement>("noFill");
		}

		public override string ElementName
		{
			get
			{
				return "ln";
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.DrawingMLNamespace;
			}
		}

		public SolidFillElement SolidFillElement
		{
			get
			{
				return this.solidFill.Element;
			}
			set
			{
				this.solidFill.Element = value;
			}
		}

		public NoFillElement NoFillElement
		{
			get
			{
				return this.noFill.Element;
			}
			set
			{
				this.noFill.Element = value;
			}
		}

		public int Width
		{
			get
			{
				return this.width.Value;
			}
			set
			{
				this.width.Value = value;
			}
		}

		public void SetDefaultProperties()
		{
			base.CreateElement(this.solidFill);
			this.SolidFillElement.SetDefaultProperties();
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, Outline shapeOutline)
		{
			if (shapeOutline.Width != null)
			{
				this.Width = (int)Unit.DipToEmu(Unit.PointToDip(shapeOutline.Width.Value));
			}
			if (shapeOutline.Fill == null)
			{
				return;
			}
			switch (shapeOutline.Fill.ShapeFillType)
			{
			case ShapeType.Solid:
				base.CreateElement(this.solidFill);
				this.SolidFillElement.CopyPropertiesFrom(context, shapeOutline.Fill as SolidFill);
				return;
			case ShapeType.NoFill:
				base.CreateElement(this.noFill);
				return;
			default:
				throw new NotSupportedException("This type of fill is not supported.");
			}
		}

		public void CopyPropertiesTo(Outline shapeOutline)
		{
			if (this.width.HasValue)
			{
				shapeOutline.Width = new double?(Unit.DipToPoint(Unit.EmuToDip((double)this.Width)));
			}
			if (this.SolidFillElement != null)
			{
				shapeOutline.Fill = this.SolidFillElement.CreateSolidFill();
				return;
			}
			if (this.NoFillElement != null)
			{
				shapeOutline.Fill = new NoFill();
			}
		}

		readonly IntOpenXmlAttribute width;

		readonly OpenXmlChildElement<SolidFillElement> solidFill;

		readonly OpenXmlChildElement<NoFillElement> noFill;
	}
}
