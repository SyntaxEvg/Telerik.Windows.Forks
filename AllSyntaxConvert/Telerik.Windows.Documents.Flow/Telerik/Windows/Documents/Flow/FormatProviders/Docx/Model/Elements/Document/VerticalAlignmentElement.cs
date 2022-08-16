using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class VerticalAlignmentElement : DocumentElementBase
	{
		public VerticalAlignmentElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.verticalAlignment = base.RegisterAttribute<MappedOpenXmlAttribute<VerticalAlignment>>(new MappedOpenXmlAttribute<VerticalAlignment>("val", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.VerticalAlignmentMapper, false));
		}

		public override string ElementName
		{
			get
			{
				return "vAlign";
			}
		}

		public VerticalAlignment Value
		{
			get
			{
				return this.verticalAlignment.Value;
			}
			set
			{
				this.verticalAlignment.Value = value;
			}
		}

		readonly MappedOpenXmlAttribute<VerticalAlignment> verticalAlignment;
	}
}
