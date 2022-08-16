using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists
{
	class NumberingStyleElement : DocxElementBase
	{
		public NumberingStyleElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.numberingStyleAttribute = new MappedOpenXmlAttribute<NumberingStyle>("val", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.NumberingStyleMapper, true);
			base.RegisterAttribute<MappedOpenXmlAttribute<NumberingStyle>>(this.numberingStyleAttribute);
		}

		public override string ElementName
		{
			get
			{
				return "numFmt";
			}
		}

		public NumberingStyle Value
		{
			get
			{
				return this.numberingStyleAttribute.Value;
			}
			set
			{
				this.numberingStyleAttribute.Value = value;
			}
		}

		readonly MappedOpenXmlAttribute<NumberingStyle> numberingStyleAttribute;
	}
}
