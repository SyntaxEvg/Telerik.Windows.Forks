using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class FillsElement : StyleSheetElementBase
	{
		public FillsElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.count = base.RegisterCountAttribute();
		}

		public override string ElementName
		{
			get
			{
				return "fills";
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
			this.Count = context.StyleSheet.FillTable.Count<IFill>();
		}

		protected override void OnAfterReadChildElement(IXlsxWorkbookImportContext context, OpenXmlElementBase childElement)
		{
			context.StyleSheet.FillTable.Add(((FillElement)childElement).Fill);
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			foreach (IFill fill in context.StyleSheet.FillTable)
			{
				FillElement fillElement = base.CreateElement<FillElement>("fill");
				fillElement.CopyPropertiesFrom(context, fill);
				yield return fillElement;
			}
			yield break;
		}

		readonly OpenXmlCountAttribute count;
	}
}
