using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers.Worksheet
{
	class MergedCellsElementWriter : ConsecutiveElementBase
	{
		public override string ElementName
		{
			get
			{
				return "mergeCells";
			}
		}

		public MergedCellElementWriter CreateMergedCellElementWriter()
		{
			return base.CreateChildElement<MergedCellElementWriter>();
		}
	}
}
