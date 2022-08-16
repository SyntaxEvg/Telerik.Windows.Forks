using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.SharedStrings;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class RichTextInlineElement : WorksheetElementBase
	{
		public RichTextInlineElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "is";
			}
		}

		public string ResultText { get; set; }

		protected override void OnAfterReadChildElement(IXlsxWorksheetImportContext context, OpenXmlElementBase childElement)
		{
			if (this.ResultText == null)
			{
				this.ResultText = string.Empty;
			}
			RichTextRunElement richTextRunElement = childElement as RichTextRunElement;
			if (richTextRunElement != null)
			{
				this.ResultText += richTextRunElement.TextElement.InnerText;
			}
			TextElement textElement = childElement as TextElement;
			if (textElement != null)
			{
				this.ResultText += textElement.InnerText;
			}
		}

		protected override void ClearOverride()
		{
			this.ResultText = string.Empty;
		}
	}
}
