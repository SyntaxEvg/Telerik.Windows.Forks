using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class SeriesValuesElement : SeriesDataElementBase
	{
		public SeriesValuesElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "val";
			}
		}
	}
}
