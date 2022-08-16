using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class SizeElement : ChartElementBase
	{
		public SizeElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<OpenXmlAttribute<byte>>(new OpenXmlAttribute<byte>("val", 5, true));
		}

		public override string ElementName
		{
			get
			{
				return "size";
			}
		}

		public byte Value
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

		readonly OpenXmlAttribute<byte> value;
	}
}
