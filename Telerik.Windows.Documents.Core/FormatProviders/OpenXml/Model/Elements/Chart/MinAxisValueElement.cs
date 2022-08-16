using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class MinAxisValueElement : ChartElementBase
	{
		public MinAxisValueElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<double>("val", true);
		}

		public override string ElementName
		{
			get
			{
				return "min";
			}
		}

		public double Value
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

		readonly OpenXmlAttribute<double> value;
	}
}
