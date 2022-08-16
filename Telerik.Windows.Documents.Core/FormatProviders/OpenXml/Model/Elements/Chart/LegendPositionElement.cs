using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class LegendPositionElement : ChartElementBase
	{
		public LegendPositionElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<ConvertedOpenXmlAttribute<LegendPosition>>(new ConvertedOpenXmlAttribute<LegendPosition>("val", Converters.LegendPositionToStringConverter, true));
		}

		public override string ElementName
		{
			get
			{
				return "legendPos";
			}
		}

		public LegendPosition Value
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

		readonly ConvertedOpenXmlAttribute<LegendPosition> value;
	}
}
