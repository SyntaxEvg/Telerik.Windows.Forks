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
	abstract class CellFormatsElementBase : StyleSheetElementBase
	{
		public CellFormatsElementBase(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.count = base.RegisterCountAttribute();
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
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
			this.Count = this.GetFormattings(context).Count<FormattingRecord>();
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			foreach (FormattingRecord format in this.GetFormattings(context))
			{
				FormatElement formatElement = base.CreateElement<FormatElement>("xf");
				formatElement.CopyPropertiesFrom(context, format);
				yield return formatElement;
			}
			yield break;
		}

		protected override void OnAfterReadChildElement(IXlsxWorkbookImportContext context, OpenXmlElementBase element)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(element, "element");
			this.SetFormatting(context, (FormatElement)element);
		}

		protected abstract IEnumerable<FormattingRecord> GetFormattings(IXlsxWorkbookExportContext context);

		protected abstract void SetFormatting(IXlsxWorkbookImportContext context, FormatElement format);

		readonly OpenXmlCountAttribute count;
	}
}
