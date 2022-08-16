using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	abstract class HeaderFooterReferenceElementBase : DocxElementBase
	{
		public HeaderFooterReferenceElementBase(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.relationshipIdAttribute = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("id", OpenXmlNamespaces.OfficeDocumentRelationshipsNamespace, true));
			this.headerFooterTypeAttribute = base.RegisterAttribute<MappedOpenXmlAttribute<HeaderFooterType>>(new MappedOpenXmlAttribute<HeaderFooterType>("type", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.HeaderFooterTypeMapper, true));
		}

		public string RelationshipId
		{
			get
			{
				return this.relationshipIdAttribute.Value;
			}
			set
			{
				this.relationshipIdAttribute.Value = value;
			}
		}

		public HeaderFooterType HeaderFooterType
		{
			get
			{
				return this.headerFooterTypeAttribute.Value;
			}
			set
			{
				this.headerFooterTypeAttribute.Value = value;
			}
		}

		readonly OpenXmlAttribute<string> relationshipIdAttribute;

		readonly MappedOpenXmlAttribute<HeaderFooterType> headerFooterTypeAttribute;
	}
}
