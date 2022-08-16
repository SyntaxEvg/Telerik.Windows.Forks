using System;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Parts
{
	class WorkbookRelationshipsPart : RelationshipsPartBase
	{
		public WorkbookRelationshipsPart(PartContext context)
			: base(context)
		{
		}

		public override string ContentType
		{
			get
			{
				return XlsxContentTypeNames.RelationshipContentType;
			}
		}

		protected override string PartPath
		{
			get
			{
				return PartPaths.WorkbookRelationshipsPartPath;
			}
		}
	}
}
