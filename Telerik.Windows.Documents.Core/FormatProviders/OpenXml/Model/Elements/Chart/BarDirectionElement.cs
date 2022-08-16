using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class BarDirectionElement : ChartElementBase
	{
		public BarDirectionElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<ConvertedOpenXmlAttribute<BarDirection>>(new ConvertedOpenXmlAttribute<BarDirection>("val", Converters.BarDirectionToValueConverter, true));
		}

		public override string ElementName
		{
			get
			{
				return "barDir";
			}
		}

		public BarDirection Value
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

		readonly ConvertedOpenXmlAttribute<BarDirection> value;
	}
}
