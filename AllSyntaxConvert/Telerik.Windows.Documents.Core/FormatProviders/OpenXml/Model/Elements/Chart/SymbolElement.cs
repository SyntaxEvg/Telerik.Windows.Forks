using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class SymbolElement : ChartElementBase
	{
		public SymbolElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<ConvertedOpenXmlAttribute<MarkerStyle>>(new ConvertedOpenXmlAttribute<MarkerStyle>("val", Converters.MarkerStyleToStringConverter, MarkerStyle.Auto, true));
		}

		public override string ElementName
		{
			get
			{
				return "symbol";
			}
		}

		public MarkerStyle Value
		{
			get
			{
				return this.value.Value;
			}
			set
			{
				this.value.Value = value;
			}
		}

		readonly ConvertedOpenXmlAttribute<MarkerStyle> value;
	}
}
