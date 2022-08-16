using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	abstract class ThemeElementBase : OpenXmlElementBase
	{
		public ThemeElementBase(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}
	}
}
