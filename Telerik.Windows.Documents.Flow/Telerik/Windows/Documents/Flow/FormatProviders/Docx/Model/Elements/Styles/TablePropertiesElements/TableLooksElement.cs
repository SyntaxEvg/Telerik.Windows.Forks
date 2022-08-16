using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.TablePropertiesElements
{
	class TableLooksElement : DocumentElementBase
	{
		public TableLooksElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.value = base.RegisterAttribute<ConvertedOpenXmlAttribute<TableLooks>>(new ConvertedOpenXmlAttribute<TableLooks>("val", OpenXmlNamespaces.WordprocessingMLNamespace, DocxConverters.TableLookConverter, true));
		}

		public override string ElementName
		{
			get
			{
				return "tblLook";
			}
		}

		public void FillAttributes(TableProperties tableProperties)
		{
			this.value.Value = tableProperties.Looks.LocalValue.Value;
		}

		public void ReadAttributes(TableProperties tableProperties)
		{
			if (this.value.HasValue)
			{
				tableProperties.Looks.LocalValue = new TableLooks?(this.value.Value);
			}
		}

		readonly ConvertedOpenXmlAttribute<TableLooks> value;
	}
}
