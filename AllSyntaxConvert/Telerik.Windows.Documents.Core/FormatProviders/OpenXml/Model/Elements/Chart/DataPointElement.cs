using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class DataPointElement : ChartElementBase
	{
		public DataPointElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterChildElement<ValueElement>("v", "c:v");
			this.index = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("idx", true));
		}

		public override string ElementName
		{
			get
			{
				return "pt";
			}
		}

		public ValueElement ValueElement
		{
			get
			{
				return this.value.Element;
			}
			set
			{
				this.value.Element = value;
			}
		}

		public int Index
		{
			get
			{
				return this.index.Value;
			}
			set
			{
				this.index.Value = value;
			}
		}

		public void CopyPropertiesFrom(string value)
		{
			base.CreateElement(this.value);
			this.ValueElement.InnerText = value;
		}

		readonly OpenXmlChildElement<ValueElement> value;

		readonly IntOpenXmlAttribute index;
	}
}
