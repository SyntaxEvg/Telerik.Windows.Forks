using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists
{
	class MultiLevelTypeElement : DocxElementBase
	{
		public MultiLevelTypeElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.multiLevelTypeAttribute = new MappedOpenXmlAttribute<MultilevelType>("val", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.MultilevelTypeMapper, true);
			base.RegisterAttribute<MappedOpenXmlAttribute<MultilevelType>>(this.multiLevelTypeAttribute);
		}

		public override string ElementName
		{
			get
			{
				return "multiLevelType";
			}
		}

		public MultilevelType Value
		{
			get
			{
				return this.multiLevelTypeAttribute.Value;
			}
			set
			{
				this.multiLevelTypeAttribute.Value = value;
			}
		}

		readonly MappedOpenXmlAttribute<MultilevelType> multiLevelTypeAttribute;
	}
}
