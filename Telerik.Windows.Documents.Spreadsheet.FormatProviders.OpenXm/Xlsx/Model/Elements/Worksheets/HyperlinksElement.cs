using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class HyperlinksElement : WorksheetElementBase
	{
		public HyperlinksElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "hyperlinks";
			}
		}

		protected override bool ShouldExport(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			return context.Hyperlinks.Any<SpreadsheetHyperlink>() || base.ShouldExport(context);
		}

		protected override IEnumerable<XlsxElementBase> EnumerateChildElements(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			foreach (SpreadsheetHyperlink hyperlink in context.Hyperlinks)
			{
				HyperlinkElement hyperlinkElement = base.CreateElement<HyperlinkElement>("hyperlink");
				hyperlinkElement.CopyPropertiesFrom(context, hyperlink);
				yield return hyperlinkElement;
			}
			yield break;
		}
	}
}
