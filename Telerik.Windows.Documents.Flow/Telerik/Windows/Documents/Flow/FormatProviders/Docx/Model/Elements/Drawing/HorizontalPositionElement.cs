using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Drawing
{
	class HorizontalPositionElement : DrawingElementBase
	{
		public HorizontalPositionElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.align = base.RegisterChildElement<HorizontalAlignmentElement>("align");
			this.offset = base.RegisterChildElement<OneDimentionOffsetElement>("posOffset");
			this.relativeFrom = new MappedOpenXmlAttribute<HorizontalRelativeFrom>("relativeFrom", null, TypeMappers.HorizontalRelativeFromMapper, true);
			base.RegisterAttribute<MappedOpenXmlAttribute<HorizontalRelativeFrom>>(this.relativeFrom);
		}

		public override string ElementName
		{
			get
			{
				return "positionH";
			}
		}

		public OneDimentionOffsetElement OffsetElement
		{
			get
			{
				return this.offset.Element;
			}
		}

		public HorizontalAlignmentElement HorizontalAlignmentElement
		{
			get
			{
				return this.align.Element;
			}
		}

		public HorizontalRelativeFrom RelativeFrom
		{
			get
			{
				return this.relativeFrom.Value;
			}
			set
			{
				this.relativeFrom.Value = value;
			}
		}

		public void CopyPropertiesFrom(IDocxExportContext context, ShapeAnchorBase shape)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeAnchorBase>(shape, "shape");
			this.RelativeFrom = shape.HorizontalPosition.RelativeFrom;
			switch (shape.HorizontalPosition.ValueType)
			{
			case PositionValueType.Offset:
				base.CreateElement(this.offset);
				this.OffsetElement.InnerText = Converters.EmuToDipConverter.ConvertToString(shape.HorizontalPosition.Offset);
				return;
			case PositionValueType.Alignment:
				base.CreateElement(this.align);
				this.HorizontalAlignmentElement.InnerText = TypeMappers.RelativeHorizontalAlignmentMapper.GetFromValue(shape.HorizontalPosition.Alignment);
				return;
			default:
				return;
			}
		}

		public void CopyPropertiesТо(IDocxImportContext context, ShapeAnchorBase shape)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeAnchorBase>(shape, "shape");
			shape.HorizontalPosition.RelativeFrom = this.RelativeFrom;
			if (this.HorizontalAlignmentElement != null)
			{
				RelativeHorizontalAlignment toValue = TypeMappers.RelativeHorizontalAlignmentMapper.GetToValue(this.HorizontalAlignmentElement.InnerText);
				shape.HorizontalPosition = new HorizontalPosition(this.RelativeFrom, toValue);
				base.ReleaseElement(this.align);
			}
			if (this.OffsetElement != null)
			{
				shape.HorizontalPosition = new HorizontalPosition(this.RelativeFrom, Converters.EmuToDipConverter.ConvertFromString(this.OffsetElement.InnerText));
				base.ReleaseElement(this.offset);
			}
		}

		protected override OpenXmlElementBase CreateElement(string elementName)
		{
			if (elementName == "align")
			{
				return new HorizontalAlignmentElement(base.PartsManager)
				{
					Part = base.Part
				};
			}
			return base.CreateElement(elementName);
		}

		protected override void ReleaseElementOverride(OpenXmlElementBase element)
		{
			if (element.ElementName != "align")
			{
				base.ReleaseElementOverride(element);
			}
		}

		readonly OpenXmlChildElement<HorizontalAlignmentElement> align;

		readonly OpenXmlChildElement<OneDimentionOffsetElement> offset;

		readonly MappedOpenXmlAttribute<HorizontalRelativeFrom> relativeFrom;
	}
}
