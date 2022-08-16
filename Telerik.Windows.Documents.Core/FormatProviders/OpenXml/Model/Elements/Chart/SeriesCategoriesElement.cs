using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class SeriesCategoriesElement : SeriesDataElementBase
	{
		public SeriesCategoriesElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "cat";
			}
		}
	}
}
