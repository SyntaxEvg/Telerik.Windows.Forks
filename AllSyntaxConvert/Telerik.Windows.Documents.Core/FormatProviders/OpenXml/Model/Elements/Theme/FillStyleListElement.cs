using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class FillStyleListElement : FillStyleListElementBase
	{
		public FillStyleListElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "fillStyleLst";
			}
		}
	}
}
