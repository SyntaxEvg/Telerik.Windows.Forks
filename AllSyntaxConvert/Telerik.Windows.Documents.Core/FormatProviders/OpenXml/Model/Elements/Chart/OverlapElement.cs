using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class OverlapElement : ChartElementBase
	{
		public OverlapElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("val", true));
		}

		public override string ElementName
		{
			get
			{
				return "overlap";
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

		public const int FullOverlap = 100;

		readonly IntOpenXmlAttribute value;
	}
}
