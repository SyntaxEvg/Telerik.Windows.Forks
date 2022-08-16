using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers;
using Telerik.Documents.SpreadsheetStreaming.Model.Relations;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Parts
{
	class WorkbookPart : PartBase<WorkbookElementWriter>
	{
		public WorkbookPart(PartContext context)
			: base(context)
		{
		}

		public override string ContentType
		{
			get
			{
				return XlsxContentTypeNames.WorkbookContentType;
			}
		}

		protected override string PartPath
		{
			get
			{
				return PartPaths.WorkbookPartPath;
			}
		}

		public void WriteWorksheetRelationships(List<Relationship> relationships)
		{
			base.RootElement.Write(relationships);
		}

		public void Read(List<Relationship> relationships)
		{
			base.RootElement.Read(ref relationships);
		}
	}
}
