using System;
using System.Windows.Media;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class RgbHexColorModelElement : ColorChoiceElementBase
	{
		public RgbHexColorModelElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.valueAttribute = base.RegisterAttribute<ConvertedOpenXmlAttribute<Color>>(new ConvertedOpenXmlAttribute<Color>("val", Converters.HexBinary3Converter, true));
		}

		public override string ElementName
		{
			get
			{
				return "srgbClr";
			}
		}

		public void SetColor(Color color)
		{
			this.valueAttribute.Value = color;
		}

		public Color GetColor()
		{
			return this.valueAttribute.Value;
		}

		readonly ConvertedOpenXmlAttribute<Color> valueAttribute;
	}
}
