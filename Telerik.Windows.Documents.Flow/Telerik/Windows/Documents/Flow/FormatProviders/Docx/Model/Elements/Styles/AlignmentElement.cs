using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles
{
	class AlignmentElement : DocxElementBase
	{
		public AlignmentElement(DocxPartsManager partsManager, string elementName)
			: base(partsManager)
		{
			this.elementName = elementName;
			this.alignmentAttribute = new MappedOpenXmlAttribute<Alignment>("val", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.AlignmentMapper, false);
			base.RegisterAttribute<MappedOpenXmlAttribute<Alignment>>(this.alignmentAttribute);
		}

		public override string ElementName
		{
			get
			{
				return this.elementName;
			}
		}

		public Alignment Value
		{
			get
			{
				return this.alignmentAttribute.Value;
			}
			set
			{
				this.alignmentAttribute.Value = value;
			}
		}

		readonly MappedOpenXmlAttribute<Alignment> alignmentAttribute;

		readonly string elementName;
	}
}
