using System;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Parts
{
	class WorksheetPartWriter : ConsecutivePartWriterBase<WorksheetElement>
	{
		public WorksheetPartWriter(PartContext context)
			: base(context)
		{
		}

		public override string ContentType
		{
			get
			{
				return XlsxContentTypeNames.WorksheetContentType;
			}
		}

		protected override string PartPath
		{
			get
			{
				return PartPaths.WorksheetPartPath;
			}
		}
	}
}
