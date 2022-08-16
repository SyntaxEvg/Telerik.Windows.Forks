using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class BarGroupingElement : ChartElementBase
	{
		public BarGroupingElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<ConvertedOpenXmlAttribute<SeriesGrouping>>(new ConvertedOpenXmlAttribute<SeriesGrouping>("val", Converters.BarGroupingToStringConverter, true));
		}

		public override string ElementName
		{
			get
			{
				return "grouping";
			}
		}

		public SeriesGrouping Value
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

		readonly ConvertedOpenXmlAttribute<SeriesGrouping> value;
	}
}
