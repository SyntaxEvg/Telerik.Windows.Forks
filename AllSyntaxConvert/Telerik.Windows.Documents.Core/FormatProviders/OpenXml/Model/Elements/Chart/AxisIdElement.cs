using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class AxisIdElement : ChartElementBase
	{
		public AxisIdElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("val", true));
		}

		public override string ElementName
		{
			get
			{
				return "axId";
			}
		}

		public int Value
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

		readonly IntOpenXmlAttribute value;
	}
}
