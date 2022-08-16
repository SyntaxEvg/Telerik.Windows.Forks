using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class FontsElement : StyleSheetElementBase
	{
		public FontsElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.count = base.RegisterCountAttribute();
		}

		public override string ElementName
		{
			get
			{
				return "fonts";
			}
		}

		public int Count
		{
			get
			{
				return this.count.Value;
			}
			set
			{
				this.count.Value = value;
			}
		}

		protected override void OnBeforeWrite(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			this.Count = context.StyleSheet.FontInfoTable.Count<FontInfo>();
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			foreach (FontInfo font in context.StyleSheet.FontInfoTable)
			{
				FontElement fontElement = base.CreateElement<FontElement>("font");
				fontElement.CopyPropertiesFrom(context, font);
				yield return fontElement;
			}
			yield break;
		}

		protected override void OnAfterReadChildElement(IXlsxWorkbookImportContext context, OpenXmlElementBase childElement)
		{
			FontElement fontElement = (FontElement)childElement;
			FontInfo value = fontElement.FontInfo.Value;
			context.StyleSheet.FontInfoTable.Add(value);
			base.OnAfterReadChildElement(context, childElement);
		}

		readonly OpenXmlCountAttribute count;
	}
}
