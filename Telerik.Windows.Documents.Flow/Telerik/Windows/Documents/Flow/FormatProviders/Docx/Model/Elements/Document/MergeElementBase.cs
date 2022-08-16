using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	abstract class MergeElementBase : DocxElementBase
	{
		protected MergeElementBase(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.valueAttribute = base.RegisterAttribute<MappedOpenXmlAttribute<TableCellMergeType>>(new MappedOpenXmlAttribute<TableCellMergeType>("val", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.TableCellMergeTypeMapper, false));
		}

		public TableCellMergeType TableCellMergeType
		{
			get
			{
				return this.valueAttribute.Value;
			}
			set
			{
				this.valueAttribute.Value = value;
			}
		}

		readonly MappedOpenXmlAttribute<TableCellMergeType> valueAttribute;
	}
}
