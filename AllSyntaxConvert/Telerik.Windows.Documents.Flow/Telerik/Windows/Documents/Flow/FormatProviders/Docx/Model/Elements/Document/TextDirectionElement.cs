using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class TextDirectionElement : DocxElementBase
	{
		public TextDirectionElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.textDirection = base.RegisterAttribute<MappedOpenXmlAttribute<TextDirection>>(new MappedOpenXmlAttribute<TextDirection>("val", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.TextDirectionMapper, false));
		}

		public override string ElementName
		{
			get
			{
				return "textDirection";
			}
		}

		public TextDirection Value
		{
			get
			{
				return this.textDirection.Value;
			}
			set
			{
				this.textDirection.Value = value;
			}
		}

		readonly MappedOpenXmlAttribute<TextDirection> textDirection;
	}
}
