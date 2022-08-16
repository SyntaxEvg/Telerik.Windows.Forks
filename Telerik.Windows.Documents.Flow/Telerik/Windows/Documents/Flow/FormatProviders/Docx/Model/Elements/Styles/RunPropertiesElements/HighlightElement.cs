using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.RunPropertiesElements
{
	class HighlightElement : DocxElementBase
	{
		public HighlightElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.colorAttribute = new MappedOpenXmlAttribute<Color>("val", OpenXmlNamespaces.WordprocessingMLNamespace, HighlightColors.Mapper, false);
			base.RegisterAttribute<MappedOpenXmlAttribute<Color>>(this.colorAttribute);
		}

		public override string ElementName
		{
			get
			{
				return "highlight";
			}
		}

		public Color Value
		{
			get
			{
				return this.colorAttribute.Value;
			}
			set
			{
				this.colorAttribute.Value = HighlightColors.GetNearestHighlightingColor(value);
			}
		}

		readonly MappedOpenXmlAttribute<Color> colorAttribute;
	}
}
