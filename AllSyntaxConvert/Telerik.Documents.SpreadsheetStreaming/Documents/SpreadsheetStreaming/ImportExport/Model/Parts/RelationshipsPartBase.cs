using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers;
using Telerik.Documents.SpreadsheetStreaming.Model.Relations;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Parts
{
	abstract class RelationshipsPartBase : PartBase<RelationshipsElement>
	{
		public RelationshipsPartBase(PartContext context)
			: base(context)
		{
		}

		public void WriteRelationships(Relationships relationships)
		{
			base.RootElement.Write(relationships);
		}

		public void ReadRelationships(Relationships relationships)
		{
			base.RootElement.Read(ref relationships);
		}
	}
}
