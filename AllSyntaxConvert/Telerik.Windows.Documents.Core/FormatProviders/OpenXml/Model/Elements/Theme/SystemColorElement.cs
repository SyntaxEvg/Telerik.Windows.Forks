using System;
using System.Windows.Media;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class SystemColorElement : ColorChoiceElementBase
	{
		public SystemColorElement(OpenXmlPartsManager partsMaanger)
			: base(partsMaanger)
		{
			this.value = base.RegisterAttribute<string>("val", true);
			this.lastColorAttribute = base.RegisterAttribute<ConvertedOpenXmlAttribute<Color>>(new ConvertedOpenXmlAttribute<Color>("lastClr", Converters.HexBinary3Converter, false));
		}

		public override string ElementName
		{
			get
			{
				return "sysClr";
			}
		}

		public string Value
		{
			get
			{
				return this.value.Value;
			}
			set
			{
				this.value.Value = value;
			}
		}

		public Color GetColor(IOpenXmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			if (this.lastColorAttribute.HasValue)
			{
				return this.lastColorAttribute.Value;
			}
			throw new ArgumentException("Cannot extract system color.");
		}

		readonly OpenXmlAttribute<string> value;

		readonly ConvertedOpenXmlAttribute<Color> lastColorAttribute;
	}
}
