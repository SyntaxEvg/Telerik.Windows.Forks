using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	abstract class BreaksElementBase : WorksheetElementBase
	{
		public BreaksElementBase(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.countAttribute = base.RegisterCountAttribute();
			this.manualBreakCountAttribute = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("manualBreakCount", 0, false));
		}

		public int Count
		{
			get
			{
				return this.countAttribute.Value;
			}
			set
			{
				this.countAttribute.Value = value;
			}
		}

		public int ManualBreakCount
		{
			get
			{
				return this.manualBreakCountAttribute.Value;
			}
			set
			{
				this.manualBreakCountAttribute.Value = value;
			}
		}

		protected abstract IEnumerable<PageBreakInfo> EnumeratePageBreaks(IXlsxWorksheetExportContext context);

		protected abstract void ApplyPageBreakInfo(IXlsxWorksheetImportContext context, PageBreakInfo pageBreakInfo);

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			int num = this.EnumeratePageBreaks(context).Count<PageBreakInfo>();
			if (num > 0)
			{
				this.Count = num;
				this.ManualBreakCount = num;
			}
		}

		protected override bool ShouldExport(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			return this.EnumeratePageBreaks(context).Any<PageBreakInfo>();
		}

		protected override IEnumerable<XlsxElementBase> EnumerateChildElements(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			foreach (PageBreakInfo pageBreak in this.EnumeratePageBreaks(context))
			{
				BreakElement breakElement = base.CreateElement<BreakElement>("brk");
				breakElement.OnBeforeWrite(pageBreak);
				yield return breakElement;
			}
			yield break;
		}

		protected override void OnAfterReadChildElement(IXlsxWorksheetImportContext context, OpenXmlElementBase childElement)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			if (childElement.ElementName == "brk")
			{
				BreakElement breakElement = (BreakElement)childElement;
				this.ApplyPageBreakInfo(context, breakElement.PageBreakInfo);
			}
		}

		readonly OpenXmlCountAttribute countAttribute;

		readonly IntOpenXmlAttribute manualBreakCountAttribute;
	}
}
