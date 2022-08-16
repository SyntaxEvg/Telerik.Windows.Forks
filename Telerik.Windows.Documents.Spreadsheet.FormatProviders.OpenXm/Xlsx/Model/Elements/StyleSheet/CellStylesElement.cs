using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class CellStylesElement : StyleSheetElementBase
	{
		public CellStylesElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.count = base.RegisterCountAttribute();
		}

		public override string ElementName
		{
			get
			{
				return "cellStyles";
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
			this.Count = context.StyleSheet.StyleInfoTable.Count<StyleInfo>();
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			foreach (StyleInfo style in context.StyleSheet.StyleInfoTable)
			{
				CellStyleElement cellStyleElement = base.CreateElement<CellStyleElement>("cellStyle");
				cellStyleElement.CopyPropertiesFrom(context, style);
				yield return cellStyleElement;
			}
			yield break;
		}

		readonly OpenXmlCountAttribute count;
	}
}
