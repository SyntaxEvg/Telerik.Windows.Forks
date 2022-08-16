using System;
using System.Text;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.SharedStrings
{
	class StringItemElement : SharedStringElementBase
	{
		public StringItemElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.text = base.RegisterChildElement<TextElement>("t", "x:t");
		}

		public override string ElementName
		{
			get
			{
				return "si";
			}
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

		public void CopyPropertiesFrom(IXlsxWorkbookExportContext context, SharedString sharedString)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<SharedString>(sharedString, "sharedString");
			SharedStringType type = sharedString.Type;
			if (type != SharedStringType.Text)
			{
				return;
			}
			base.CreateElement(this.text);
			this.TextElement.CopyPropertiesFrom(context, (TextSharedString)sharedString);
		}

		protected override void OnAfterRead(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			if (this.TextElement != null)
			{
				string text = this.TextElement.InnerText ?? string.Empty;
				base.ReleaseElement(this.text);
				context.SharedStrings.Add(new TextSharedString(text));
				return;
			}
			if (this.builder != null)
			{
				string text2 = this.builder.ToString();
				context.SharedStrings.Add(new RichTextRunSharedString(text2));
				this.builder = null;
				return;
			}
			context.SharedStrings.Add(new TextSharedString(string.Empty));
		}

		protected override void OnAfterReadChildElement(IXlsxWorkbookImportContext context, OpenXmlElementBase element)
		{
			RichTextRunElement richTextRunElement = element as RichTextRunElement;
			if (richTextRunElement == null)
			{
				return;
			}
			if (this.builder == null)
			{
				this.builder = new StringBuilder();
			}
			this.builder.Append(richTextRunElement.TextElement.InnerText);
		}

		readonly OpenXmlChildElement<TextElement> text;

		StringBuilder builder;
	}
}
