using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming
{
	abstract class WorkbookExporterBase : EntityBase, IWorkbookExporter, IDisposable
	{
		public abstract SpreadCellStyleCollection CellStyles { get; }

		public IWorksheetExporter CreateWorksheetExporter(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			if (this.hasAliveWorksheet)
			{
				throw new InvalidOperationException("Only one IWorksheetExporter is allowed at a time. Please dispose any previously created IWorksheetExporter instances.");
			}
			IWorksheetExporter result = this.CreateWorksheetExporterOverride(name);
			this.hasAliveWorksheet = true;
			return result;
		}

		public abstract IWorksheetExporter CreateWorksheetExporterOverride(string name);

		public abstract IEnumerable<SheetInfo> GetSheetInfos();

		public void NotifyWorksheetDisposing()
		{
			this.hasAliveWorksheet = false;
		}

		bool hasAliveWorksheet;
	}
}
