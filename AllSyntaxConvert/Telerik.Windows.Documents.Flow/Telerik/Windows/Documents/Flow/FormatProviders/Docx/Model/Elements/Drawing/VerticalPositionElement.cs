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
	class VerticalPositionElement : DrawingElementBase
	{
		public VerticalPositionElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.align = base.RegisterChildElement<VerticalAlignmentElement>("align");
			this.offset = base.RegisterChildElement<OneDimentionOffsetElement>("posOffset");
			this.relativeFrom = new MappedOpenXmlAttribute<VerticalRelativeFrom>("relativeFrom", null, TypeMappers.VerticalRelativeFromMapper, true);
			base.RegisterAttribute<MappedOpenXmlAttribute<VerticalRelativeFrom>>(this.relativeFrom);
		}

		public override string ElementName
		{
			get
			{
				return "positionV";
			}
		}

		public OneDimentionOffsetElement OffsetElement
		{
			get
			{
				return this.offset.Element;
			}
		}

		public VerticalAlignmentElement VerticalAlignmentElement
		{
			get
			{
				return this.align.Element;
			}
		}

		public VerticalRelativeFrom RelativeFrom
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
			this.RelativeFrom = shape.VerticalPosition.RelativeFrom;
			switch (shape.VerticalPosition.ValueType)
			{
			case PositionValueType.Offset:
				base.CreateElement(this.offset);
				this.OffsetElement.InnerText = Converters.EmuToDipConverter.ConvertToString(shape.VerticalPosition.Offset);
				return;
			case PositionValueType.Alignment:
				base.CreateElement(this.align);
				this.VerticalAlignmentElement.InnerText = TypeMappers.RelativeVerticalAlignmentMapper.GetFromValue(shape.VerticalPosition.Alignment);
				return;
			default:
				return;
			}
		}

		public void CopyPropertiesТо(IDocxImportContext context, ShapeAnchorBase shape)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeAnchorBase>(shape, "shape");
			shape.VerticalPosition.RelativeFrom = this.RelativeFrom;
			if (this.VerticalAlignmentElement != null)
			{
				RelativeVerticalAlignment toValue = TypeMappers.RelativeVerticalAlignmentMapper.GetToValue(this.VerticalAlignmentElement.InnerText);
				shape.VerticalPosition = new VerticalPosition(this.RelativeFrom, toValue);
				base.ReleaseElement(this.align);
			}
			if (this.OffsetElement != null)
			{
				shape.VerticalPosition = new VerticalPosition(this.RelativeFrom, Converters.EmuToDipConverter.ConvertFromString(this.OffsetElement.InnerText));
				base.ReleaseElement(this.offset);
			}
		}

		protected override OpenXmlElementBase CreateElement(string elementName)
		{
			if (elementName == "align")
			{
				return new VerticalAlignmentElement(base.PartsManager)
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

		readonly OpenXmlChildElement<VerticalAlignmentElement> align;

		readonly OpenXmlChildElement<OneDimentionOffsetElement> offset;

		readonly MappedOpenXmlAttribute<VerticalRelativeFrom> relativeFrom;
	}
}
