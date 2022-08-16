using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.ParagraphPropertiesElements
{
	class OutlineLevelElement : DocxElementBase
	{
		public OutlineLevelElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.outlineLevelAttribute = new MappedOpenXmlAttribute<OutlineLevel>("val", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.OutlineLevelMapper, false);
			base.RegisterAttribute<MappedOpenXmlAttribute<OutlineLevel>>(this.outlineLevelAttribute);
		}

		public override string ElementName
		{
			get
			{
				return "outlineLvl";
			}
		}

		public OutlineLevel Value
		{
			get
			{
				return this.outlineLevelAttribute.Value;
			}
			set
			{
				this.outlineLevelAttribute.Value = value;
			}
		}

		readonly MappedOpenXmlAttribute<OutlineLevel> outlineLevelAttribute;
	}
}
