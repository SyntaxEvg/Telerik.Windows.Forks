using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Workbooks
{
	class WorkbookViewElement : WorkbookElementBase
	{
		public WorkbookViewElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.activeTab = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("activeTab", 0, false));
		}

		public int ActiveTab
		{
			get
			{
				return this.activeTab.Value;
			}
			set
			{
				this.activeTab.Value = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "workbookView";
			}
		}

		protected override void OnAfterRead(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			context.SetActiveTabIndex(this.ActiveTab);
		}

		protected override void OnBeforeWrite(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			this.ActiveTab = context.GetActiveTabIndex();
		}

		readonly IntOpenXmlAttribute activeTab;
	}
}
