using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.TablePropertiesElements
{
	class TableLayoutTypeElement : DocumentElementBase
	{
		public TableLayoutTypeElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.type = base.RegisterAttribute<MappedOpenXmlAttribute<TableLayoutType>>(new MappedOpenXmlAttribute<TableLayoutType>("type", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.TableLayoutTypeMapper, false));
		}

		public override string ElementName
		{
			get
			{
				return "tblLayout";
			}
		}

		public void FillAttributes(TableProperties tableProperties)
		{
			this.type.Value = tableProperties.LayoutType.LocalValue.Value;
		}

		public void ReadAttributes(TableProperties tableProperties)
		{
			if (this.type.HasValue)
			{
				tableProperties.LayoutType.LocalValue = new TableLayoutType?(this.type.Value);
			}
		}

		readonly MappedOpenXmlAttribute<TableLayoutType> type;
	}
}
