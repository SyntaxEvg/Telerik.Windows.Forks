using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class SeriesBubbleSizeElement : SeriesDataElementBase
	{
		public SeriesBubbleSizeElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "bubbleSize";
			}
		}
	}
}
