using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme;
using Telerik.Windows.Documents.Model.Drawing.Charts;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Model.Drawing.Theming;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common
{
	class ShapePropertiesElement : OpenXmlElementBase
	{
		public ShapePropertiesElement(OpenXmlPartsManager partsManager, OpenXmlNamespace ns)
			: base(partsManager)
		{
			Guard.ThrowExceptionIfNull<OpenXmlNamespace>(ns, "ns");
			this.ns = ns;
			this.transform = base.RegisterChildElement<TransformElement>("xfrm", "a:xfrm");
			this.presetGeometry = base.RegisterChildElement<PresetGeometryElement>("prstGeom");
			this.solidFill = base.RegisterChildElement<SolidFillElement>("solidFill");
			this.noFill = base.RegisterChildElement<NoFillElement>("noFill");
			this.outline = base.RegisterChildElement<OutlineElement>("ln");
		}

		public override string ElementName
		{
			get
			{
				return "spPr";
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

		public TransformElement TransformElement
		{
			get
			{
				return this.transform.Element;
			}
		}

		public PresetGeometryElement PresetGeometryElement
		{
			get
			{
				return this.presetGeometry.Element;
			}
		}

		public OutlineElement OutlineElement
		{
			get
			{
				return this.outline.Element;
			}
		}

		public SolidFillElement SolidFillElement
		{
			get
			{
				return this.solidFill.Element;
			}
		}

		public NoFillElement NoFillElement
		{
			get
			{
				return this.noFill.Element;
			}
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, ShapeBase shape)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeBase>(shape, "shape");
			base.CreateElement(this.transform);
			this.TransformElement.CopyPropertiesFrom(context, shape);
			base.CreateElement(this.presetGeometry);
			this.PresetGeometryElement.CopyPropertiesFrom(context, shape);
			this.ExportOutline(context, shape.Outline);
			this.ExportFill(context, shape.Fill);
		}

		public void CopyPropertiesTo(IOpenXmlImportContext context, ShapeBase shape)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ShapeBase>(shape, "shape");
			if (this.TransformElement != null)
			{
				this.TransformElement.CopyPropertiesTo(context, shape);
				base.ReleaseElement(this.transform);
			}
			if (this.PresetGeometryElement != null)
			{
				this.PresetGeometryElement.CopyPropertiesTo(context, shape);
				base.ReleaseElement(this.presetGeometry);
			}
			this.ImportOutline(shape.Outline);
			shape.Fill = this.ImportFill();
		}

		Fill ImportFill()
		{
			Fill result = null;
			if (this.SolidFillElement != null)
			{
				result = this.SolidFillElement.CreateSolidFill();
				base.ReleaseElement(this.solidFill);
			}
			if (this.NoFillElement != null)
			{
				result = new NoFill();
				base.ReleaseElement(this.noFill);
			}
			return result;
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, Axis axis)
		{
			Guard.ThrowExceptionIfNull<Axis>(axis, "axis");
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			this.ExportOutline(context, axis.Outline);
		}

		public void CopyPropertiesTo(IOpenXmlImportContext context, Axis axis)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Axis>(axis, "axis");
			this.ImportOutline(axis.Outline);
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, ChartLine line)
		{
			Guard.ThrowExceptionIfNull<ChartLine>(line, "line");
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			this.ExportOutline(context, line.Outline);
		}

		public void CopyPropertiesTo(IOpenXmlImportContext context, ChartLine line)
		{
			Guard.ThrowExceptionIfNull<ChartLine>(line, "line");
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			this.ImportOutline(line.Outline);
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, SeriesBase series)
		{
			Guard.ThrowExceptionIfNull<SeriesBase>(series, "series");
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			this.ExportOutline(context, series.Outline);
		}

		public void CopyPropertiesTo(IOpenXmlImportContext context, SeriesInfo seriesInfo)
		{
			Guard.ThrowExceptionIfNull<SeriesInfo>(seriesInfo, "seriesInfo");
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			this.ImportOutline(seriesInfo.Outline);
		}

		public void CopyPropertiesFrom(IOpenXmlExportContext context, Marker marker)
		{
			Guard.ThrowExceptionIfNull<Marker>(marker, "marker");
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			this.ExportFill(context, marker.Fill);
			this.ExportOutline(context, marker.Outline);
		}

		public void CopyPropertiesTo(IOpenXmlImportContext context, Marker marker)
		{
			Guard.ThrowExceptionIfNull<Marker>(marker, "marker");
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			marker.Fill = this.ImportFill();
			this.ImportOutline(marker.Outline);
		}

		void ImportOutline(Outline outline)
		{
			if (this.OutlineElement != null)
			{
				this.OutlineElement.CopyPropertiesTo(outline);
				base.ReleaseElement(this.outline);
			}
		}

		void ExportOutline(IOpenXmlExportContext context, Outline outline)
		{
			if (outline != null && (outline.Fill != null || outline.Width != null))
			{
				base.CreateElement(this.outline);
				this.OutlineElement.CopyPropertiesFrom(context, outline);
			}
		}

		void ExportFill(IOpenXmlExportContext context, Fill fill)
		{
			if (fill == null)
			{
				return;
			}
			switch (fill.ShapeFillType)
			{
			case ShapeType.Solid:
				base.CreateElement(this.solidFill);
				this.SolidFillElement.CopyPropertiesFrom(context, fill as SolidFill);
				return;
			case ShapeType.NoFill:
				base.CreateElement(this.noFill);
				return;
			default:
				throw new NotSupportedException("This type of fill is not supported.");
			}
		}

		readonly OpenXmlNamespace ns;

		readonly OpenXmlChildElement<TransformElement> transform;

		readonly OpenXmlChildElement<PresetGeometryElement> presetGeometry;

		readonly OpenXmlChildElement<OutlineElement> outline;

		readonly OpenXmlChildElement<SolidFillElement> solidFill;

		readonly OpenXmlChildElement<NoFillElement> noFill;
	}
}
