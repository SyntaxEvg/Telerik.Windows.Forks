using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class BackgroundFillStyleListElement : FillStyleListElementBase
	{
		public BackgroundFillStyleListElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "bgFillStyleLst";
			}
		}
	}
}
