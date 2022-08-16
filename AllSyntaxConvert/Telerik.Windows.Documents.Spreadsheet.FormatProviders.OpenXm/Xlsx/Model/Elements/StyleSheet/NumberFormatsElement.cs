using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class NumberFormatsElement : StyleSheetElementBase
	{
		public NumberFormatsElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.count = base.RegisterCountAttribute();
		}

		public override string ElementName
		{
			get
			{
				return "numFmts";
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
			this.Count = (from f in context.StyleSheet.CellValueFormatTable
				where !NumberFormatTypes.IsBuiltInFormat(f.FormatString)
				select f).Count<CellValueFormat>();
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			foreach (CellValueFormat cellValueFormat in from f in context.StyleSheet.CellValueFormatTable
				where !NumberFormatTypes.IsBuiltInFormat(f.FormatString)
				select f)
			{
				NumberFormatElement numberFormatElement = base.CreateElement<NumberFormatElement>("x:numFmt");
				numberFormatElement.CopyPropertiesFrom(context, cellValueFormat);
				yield return numberFormatElement;
			}
			yield break;
		}

		protected override void OnAfterReadChildElement(IXlsxWorkbookImportContext context, OpenXmlElementBase childElement)
		{
			NumberFormatElement numberFormatElement = (NumberFormatElement)childElement;
			CellValueFormat cellValueFormat = new CellValueFormat(numberFormatElement.FormatCode, false);
			if (numberFormatElement.HasFormatId)
			{
				context.StyleSheet.CellValueFormatTable[numberFormatElement.FormatId] = cellValueFormat;
				return;
			}
			context.StyleSheet.CellValueFormatTable.Add(cellValueFormat);
		}

		readonly OpenXmlCountAttribute count;
	}
}
