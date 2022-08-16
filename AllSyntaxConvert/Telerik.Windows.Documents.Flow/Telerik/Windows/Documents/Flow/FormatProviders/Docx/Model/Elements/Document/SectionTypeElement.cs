using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class SectionTypeElement : DocxElementBase
	{
		public SectionTypeElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.sectionTypeAttribute = base.RegisterAttribute<MappedOpenXmlAttribute<SectionType>>(new MappedOpenXmlAttribute<SectionType>("val", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.SectionTypeMapper, false));
		}

		public override string ElementName
		{
			get
			{
				return "type";
			}
		}

		public SectionType SectionType
		{
			get
			{
				return this.sectionTypeAttribute.Value;
			}
			set
			{
				this.sectionTypeAttribute.Value = value;
			}
		}

		readonly MappedOpenXmlAttribute<SectionType> sectionTypeAttribute;
	}
}
