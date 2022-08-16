using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class RoundedCornersElement : ChartElementBase
	{
		public RoundedCornersElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("val", true));
		}

		public override string ElementName
		{
			get
			{
				return "roundedCorners";
			}
		}

		public bool Value
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

		readonly BoolOpenXmlAttribute value;
	}
}
