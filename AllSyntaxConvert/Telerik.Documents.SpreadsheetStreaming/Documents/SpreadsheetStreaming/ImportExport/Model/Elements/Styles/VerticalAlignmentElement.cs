using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class VerticalAlignmentElement : RequiredValueElementBase<string>
	{
		public override string ElementName
		{
			get
			{
				return "vertAlign";
			}
		}
	}
}
