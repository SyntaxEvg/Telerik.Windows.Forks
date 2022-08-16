using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class AxisPositionElement : ChartElementBase
	{
		public AxisPositionElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<ConvertedOpenXmlAttribute<AxisPosition>>(new ConvertedOpenXmlAttribute<AxisPosition>("val", Converters.AxisPositionToStringConverter, true));
		}

		public override string ElementName
		{
			get
			{
				return "axPos";
			}
		}

		public AxisPosition Value
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

		readonly ConvertedOpenXmlAttribute<AxisPosition> value;
	}
}
