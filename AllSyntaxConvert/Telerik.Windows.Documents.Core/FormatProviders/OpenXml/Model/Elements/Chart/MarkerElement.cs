using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class MarkerElement : ChartElementBase
	{
		public MarkerElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.symbol = base.RegisterChildElement<SymbolElement>("symbol");
			this.size = base.RegisterChildElement<SizeElement>("size");
			this.shapeProperties = base.RegisterChildElement<ShapePropertiesElement>("spPr", "c:spPr");
		}

		public override string ElementName
		{
			get
			{
				return "marker";
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return false;
			}
		}

		public SymbolElement SymbolElement
		{
			get
			{
				return this.symbol.Element;
			}
		}

		public SizeElement SizeElement
		{
			get
			{
				return this.size.Element;
			}
		}

		public ShapePropertiesElement ShapePropertiesElement
		{
			get
			{
				return this.shapeProperties.Element;
			}
		}

		internal void CopyPropertiesFrom(IOpenXmlExportContext context, Marker marker)
		{
			base.CreateElement(this.symbol);
			this.SymbolElement.Value = marker.Symbol;
			base.CreateElement(this.size);
			this.SizeElement.Value = marker.Size;
			base.CreateElement(this.shapeProperties);
			this.ShapePropertiesElement.CopyPropertiesFrom(context, marker);
		}

		internal Marker CreateMarker(IOpenXmlImportContext context)
		{
			Marker marker = new Marker();
			marker.Symbol = this.SymbolElement.Value;
			base.ReleaseElement(this.symbol);
			if (this.SizeElement != null)
			{
				marker.Size = this.SizeElement.Value;
				base.ReleaseElement(this.size);
			}
			if (this.ShapePropertiesElement != null)
			{
				this.ShapePropertiesElement.CopyPropertiesTo(context, marker);
				base.ReleaseElement(this.shapeProperties);
			}
			return marker;
		}

		readonly OpenXmlChildElement<SymbolElement> symbol;

		readonly OpenXmlChildElement<SizeElement> size;

		readonly OpenXmlChildElement<ShapePropertiesElement> shapeProperties;
	}
}
