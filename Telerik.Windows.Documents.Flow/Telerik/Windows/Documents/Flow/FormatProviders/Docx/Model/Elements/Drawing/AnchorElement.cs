using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Drawing
{
	class AnchorElement : DrawingElementBase
	{
		public AnchorElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.topMargin = new ConvertedOpenXmlAttribute<double>("distT", Converters.EmuOrUniversalMeasureToDipConverter, false);
			base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(this.topMargin);
			this.bottomMargin = new ConvertedOpenXmlAttribute<double>("distB", Converters.EmuOrUniversalMeasureToDipConverter, false);
			base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(this.bottomMargin);
			this.leftMargin = new ConvertedOpenXmlAttribute<double>("distL", Converters.EmuOrUniversalMeasureToDipConverter, false);
			base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(this.leftMargin);
			this.rightMargin = new ConvertedOpenXmlAttribute<double>("distR", Converters.EmuOrUniversalMeasureToDipConverter, false);
			base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(this.rightMargin);
			this.isSimplePos = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("simplePos"));
			this.zindex = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("relativeHeight", true));
			this.isBehindDoc = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("behindDoc", false, true));
			this.isLocked = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("locked", false, true));
			this.layoutInCell = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("layoutInCell", false, true));
			this.allowOverlap = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("allowOverlap", false, true));
			this.simplePos = base.RegisterChildElement<OffsetElement>("simplePos");
			this.positionH = base.RegisterChildElement<HorizontalPositionElement>("positionH");
			this.positionV = base.RegisterChildElement<VerticalPositionElement>("positionV");
			this.extent = base.RegisterChildElement<SizeElement>("extent");
			this.effectExtent = base.RegisterChildElement<EffectExtentElement>("effectExtent");
			this.wrapNone = base.RegisterChildElement<WrapNoneElement>("wrapNone");
			this.wrapSquare = base.RegisterChildElement<WrapSquareElement>("wrapSquare");
			this.wrapTopAndBottom = base.RegisterChildElement<WrapTopAndBottomElement>("wrapTopAndBottom");
			this.drawingObjectNonVisualProperties = base.RegisterChildElement<NonVisualDrawingPropertiesElement>("docPr");
			this.graphics = base.RegisterChildElement<GraphicElement>("graphic");
		}

		public override string ElementName
		{
			get
			{
				return "anchor";
			}
		}

		public bool AllowOverlap
		{
			get
			{
				return this.allowOverlap.Value;
			}
			set
			{
				this.allowOverlap.Value = value;
			}
		}

		public bool IsBehindDoc
		{
			get
			{
				return this.isBehindDoc.Value;
			}
			set
			{
				this.isBehindDoc.Value = value;
			}
		}

		public bool IsSimplePos
		{
			get
			{
				return this.isSimplePos.Value;
			}
			set
			{
				this.isSimplePos.Value = value;
			}
		}

		public bool IsLocked
		{
			get
			{
				return this.isLocked.Value;
			}
			set
			{
				this.isLocked.Value = value;
			}
		}

		public bool LayoutInCell
		{
			get
			{
				return this.layoutInCell.Value;
			}
			set
			{
				this.layoutInCell.Value = value;
			}
		}

		public int ZIndex
		{
			get
			{
				return this.zindex.Value;
			}
			set
			{
				this.zindex.Value = value;
			}
		}

		public double LeftMargin
		{
			get
			{
				return this.leftMargin.Value;
			}
			set
			{
				this.leftMargin.Value = value;
			}
		}

		public double RightMargin
		{
			get
			{
				return this.rightMargin.Value;
			}
			set
			{
				this.rightMargin.Value = value;
			}
		}

		public double TopMargin
		{
			get
			{
				return this.topMargin.Value;
			}
			set
			{
				this.topMargin.Value = value;
			}
		}

		public double BottomMargin
		{
			get
			{
				return this.bottomMargin.Value;
			}
			set
			{
				this.bottomMargin.Value = value;
			}
		}

		public HorizontalPositionElement HorizontalPositionElement
		{
			get
			{
				return this.positionH.Element;
			}
		}

		public VerticalPositionElement VerticalPositionElement
		{
			get
			{
				return this.positionV.Element;
			}
		}

		public NonVisualDrawingPropertiesElement DrawingObjectNonVisualProperties
		{
			get
			{
				return this.drawingObjectNonVisualProperties.Element;
			}
		}

		public GraphicElement GraphicElement
		{
			get
			{
				return this.graphics.Element;
			}
		}

		public SizeElement ExtentElement
		{
			get
			{
				return this.extent.Element;
			}
		}

		public WrapNoneElement WrapNoneElement
		{
			get
			{
				return this.wrapNone.Element;
			}
		}

		public WrapSquareElement WrapSquareElement
		{
			get
			{
				return this.wrapSquare.Element;
			}
		}

		public WrapTopAndBottomElement WrapTopAndBottomElement
		{
			get
			{
				return this.wrapTopAndBottom.Element;
			}
		}

		public OffsetElement SimplePositionElement
		{
			get
			{
				return this.simplePos.Element;
			}
		}

		public EffectExtentElement EffectExtentElement
		{
			get
			{
				return this.effectExtent.Element;
			}
		}

		public void CopyPropertiesFrom(IDocxExportContext context, ShapeAnchorBase anchor)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeAnchorBase>(anchor, "anchor");
			this.AllowOverlap = anchor.AllowOverlap;
			this.IsBehindDoc = anchor.IsBehindDocument;
			this.TopMargin = anchor.Margin.Top;
			this.BottomMargin = anchor.Margin.Bottom;
			this.LeftMargin = anchor.Margin.Left;
			this.RightMargin = anchor.Margin.Right;
			this.LayoutInCell = anchor.LayoutInCell;
			this.ZIndex = anchor.ZIndex;
			this.IsLocked = anchor.IsLocked;
			this.IsSimplePos = false;
			base.CreateElement(this.simplePos);
			base.CreateElement(this.positionH);
			this.HorizontalPositionElement.CopyPropertiesFrom(context, anchor);
			base.CreateElement(this.positionV);
			this.VerticalPositionElement.CopyPropertiesFrom(context, anchor);
			base.CreateElement(this.drawingObjectNonVisualProperties);
			this.DrawingObjectNonVisualProperties.Id = anchor.Shape.Id;
			this.DrawingObjectNonVisualProperties.Name = anchor.Shape.Name;
			base.CreateElement(this.graphics);
			this.GraphicElement.CopyPropertiesFrom(context, anchor.Shape);
			switch (anchor.Wrapping.WrappingType)
			{
			case ShapeWrappingType.Square:
				base.CreateElement(this.wrapSquare);
				this.WrapSquareElement.CopyPropertiesFrom(context, anchor.Wrapping);
				break;
			case ShapeWrappingType.TopAndBottom:
				base.CreateElement(this.wrapTopAndBottom);
				break;
			default:
				base.CreateElement(this.wrapNone);
				break;
			}
			base.CreateElement(this.extent);
			this.ExtentElement.ExtentWidth = anchor.Shape.Width;
			this.ExtentElement.ExtentHeight = anchor.Shape.Height;
			base.CreateElement(this.effectExtent);
			this.EffectExtentElement.CopyPropertiesFrom(context, anchor);
		}

		public ShapeAnchorBase CreateShapeInline(IDocxImportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			ShapeBase shapeBase = null;
			if (this.GraphicElement != null)
			{
				shapeBase = this.GraphicElement.CreateShapeAndCopyPropertiesTo(context);
				base.ReleaseElement(this.graphics);
			}
			if (shapeBase != null)
			{
				if (this.ExtentElement != null)
				{
					shapeBase.Width = this.ExtentElement.ExtentWidth;
					shapeBase.Height = this.ExtentElement.ExtentHeight;
					base.ReleaseElement(this.extent);
				}
				if (this.DrawingObjectNonVisualProperties != null)
				{
					shapeBase.Name = this.DrawingObjectNonVisualProperties.Name;
					shapeBase.Id = this.DrawingObjectNonVisualProperties.Id;
					base.ReleaseElement(this.drawingObjectNonVisualProperties);
				}
			}
			ShapeAnchorBase shapeAnchorBase = AnchorElement.CreateShapeInlineFromShape(context, shapeBase);
			if (shapeAnchorBase != null)
			{
				shapeAnchorBase.AllowOverlap = this.AllowOverlap;
				shapeAnchorBase.IsBehindDocument = this.IsBehindDoc;
				shapeAnchorBase.Margin = new Padding(this.LeftMargin, this.TopMargin, this.RightMargin, this.BottomMargin);
				shapeAnchorBase.LayoutInCell = this.LayoutInCell;
				shapeAnchorBase.ZIndex = this.ZIndex;
				shapeAnchorBase.IsLocked = this.IsLocked;
				if (this.HorizontalPositionElement != null)
				{
					this.HorizontalPositionElement.CopyPropertiesТо(context, shapeAnchorBase);
					base.ReleaseElement(this.positionH);
				}
				if (this.VerticalPositionElement != null)
				{
					this.VerticalPositionElement.CopyPropertiesТо(context, shapeAnchorBase);
					base.ReleaseElement(this.positionV);
				}
				if (this.WrapNoneElement != null)
				{
					shapeAnchorBase.Wrapping.WrappingType = ShapeWrappingType.None;
					base.ReleaseElement(this.wrapNone);
				}
				if (this.WrapSquareElement != null)
				{
					shapeAnchorBase.Wrapping.WrappingType = ShapeWrappingType.Square;
					this.WrapSquareElement.CopyPropertiesTo(context, shapeAnchorBase.Wrapping);
					base.ReleaseElement(this.wrapSquare);
				}
				if (this.WrapTopAndBottomElement != null)
				{
					shapeAnchorBase.Wrapping.WrappingType = ShapeWrappingType.TopAndBottom;
					base.ReleaseElement(this.wrapTopAndBottom);
				}
			}
			if (this.EffectExtentElement != null)
			{
				base.ReleaseElement(this.effectExtent);
			}
			return shapeAnchorBase;
		}

		static ShapeAnchorBase CreateShapeInlineFromShape(IDocxImportContext context, ShapeBase shape)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Image image = shape as Image;
			if (image != null)
			{
				return new FloatingImage(context.Document, image);
			}
			return null;
		}

		readonly OpenXmlChildElement<OffsetElement> simplePos;

		readonly OpenXmlChildElement<HorizontalPositionElement> positionH;

		readonly OpenXmlChildElement<VerticalPositionElement> positionV;

		readonly OpenXmlChildElement<SizeElement> extent;

		readonly OpenXmlChildElement<WrapNoneElement> wrapNone;

		readonly OpenXmlChildElement<WrapSquareElement> wrapSquare;

		readonly OpenXmlChildElement<WrapTopAndBottomElement> wrapTopAndBottom;

		readonly OpenXmlChildElement<NonVisualDrawingPropertiesElement> drawingObjectNonVisualProperties;

		readonly OpenXmlChildElement<GraphicElement> graphics;

		readonly OpenXmlChildElement<EffectExtentElement> effectExtent;

		readonly BoolOpenXmlAttribute allowOverlap;

		readonly BoolOpenXmlAttribute isBehindDoc;

		readonly BoolOpenXmlAttribute isSimplePos;

		readonly BoolOpenXmlAttribute isLocked;

		readonly BoolOpenXmlAttribute layoutInCell;

		readonly IntOpenXmlAttribute zindex;

		readonly ConvertedOpenXmlAttribute<double> leftMargin;

		readonly ConvertedOpenXmlAttribute<double> rightMargin;

		readonly ConvertedOpenXmlAttribute<double> bottomMargin;

		readonly ConvertedOpenXmlAttribute<double> topMargin;
	}
}
