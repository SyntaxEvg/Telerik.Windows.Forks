using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet
{
	class SheetViewsElement : ConsecutiveElementBase
	{
		public override string ElementName
		{
			get
			{
				return "sheetViews";
			}
		}

		internal SheetViewElement CreateSheetViewWriter()
		{
			return base.CreateChildElement<SheetViewElement>();
		}
	}
}
