using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class NumberFormatElement : ChartElementBase
	{
		public NumberFormatElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.formatCode = base.RegisterAttribute<string>("formatCode", true);
		}

		public override string ElementName
		{
			get
			{
				return "numFmt";
			}
		}

		public string FormatCode
		{
			get
			{
				return this.formatCode.Value;
			}
			set
			{
				this.formatCode.Value = value;
			}
		}

		readonly OpenXmlAttribute<string> formatCode;
	}
}
