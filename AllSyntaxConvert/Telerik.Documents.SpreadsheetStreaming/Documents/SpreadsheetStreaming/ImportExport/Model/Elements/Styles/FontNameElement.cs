using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class FontNameElement : RequiredValueElementBase<string>
	{
		public override string ElementName
		{
			get
			{
				return "name";
			}
		}
	}
}
