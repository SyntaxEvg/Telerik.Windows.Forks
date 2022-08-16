using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class FontFamilyElement : RequiredValueElementBase<string>
	{
		public override string ElementName
		{
			get
			{
				return "family";
			}
		}
	}
}
