using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts
{
	class ThemePart : OpenXmlPartBase
	{
		public ThemePart(OpenXmlPartsManager partsManager, string partName)
			: base(partsManager, partName)
		{
			this.theme = new ThemeElement(base.PartsManager, this);
		}

		public override int Level
		{
			get
			{
				return 2;
			}
		}

		public override OpenXmlElementBase RootElement
		{
			get
			{
				return this.theme;
			}
		}

		public override string ContentType
		{
			get
			{
				return OpenXmlContentTypeNames.ThemeContentType;
			}
		}

		readonly ThemeElement theme;
	}
}
