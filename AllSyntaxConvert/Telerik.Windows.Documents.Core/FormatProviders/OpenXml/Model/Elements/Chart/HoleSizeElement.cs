using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class HoleSizeElement : ChartElementBase
	{
		public HoleSizeElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("val", true));
		}

		public override string ElementName
		{
			get
			{
				return "holeSize";
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
