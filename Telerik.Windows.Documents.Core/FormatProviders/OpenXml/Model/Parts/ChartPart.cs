using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts
{
	class ChartPart : OpenXmlPartBase
	{
		public ChartPart(OpenXmlPartsManager partsManager, string name)
			: base(partsManager, name)
		{
			this.chartSpaceElement = new ChartSpaceElement(base.PartsManager, this);
		}

		public override string ContentType
		{
			get
			{
				return OpenXmlContentTypeNames.ChartContentType;
			}
		}

		public override int Level
		{
			get
			{
				return 6;
			}
		}

		public override OpenXmlElementBase RootElement
		{
			get
			{
				return this.chartSpaceElement;
			}
		}

		readonly ChartSpaceElement chartSpaceElement;
	}
}
