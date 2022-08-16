using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.SharedStrings
{
	class RichTextRunElement : XlsxElementBase
	{
		public RichTextRunElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.text = base.RegisterChildElement<TextElement>("t", "x:t");
		}

		public TextElement TextElement
		{
			get
			{
				return this.text.Element;
			}
			set
			{
				this.text.Element = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "r";
			}
		}

		readonly OpenXmlChildElement<TextElement> text;
	}
}
