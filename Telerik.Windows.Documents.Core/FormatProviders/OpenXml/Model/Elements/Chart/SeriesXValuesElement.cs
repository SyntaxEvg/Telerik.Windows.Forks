using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Chart
{
	class SeriesXValuesElement : SeriesDataElementBase
	{
		public SeriesXValuesElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "xVal";
			}
		}
	}
}
