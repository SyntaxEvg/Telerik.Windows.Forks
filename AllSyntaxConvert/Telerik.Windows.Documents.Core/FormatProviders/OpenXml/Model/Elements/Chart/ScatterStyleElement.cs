using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Model.Drawing.Charts;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class ScatterStyleElement : ChartElementBase
	{
		public ScatterStyleElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<ConvertedOpenXmlAttribute<ScatterStyle>>(new ConvertedOpenXmlAttribute<ScatterStyle>("val", Converters.ScatterStyleToStringConverter, ScatterStyle.Marker, true));
		}

		public override string ElementName
		{
			get
			{
				return "scatterStyle";
			}
		}

		public ScatterStyle Value
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

		readonly ConvertedOpenXmlAttribute<ScatterStyle> value;
	}
}
