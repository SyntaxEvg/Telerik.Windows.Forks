using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class SchemeElement : RequiredValueElementBase<string>
	{
		public override string ElementName
		{
			get
			{
				return "scheme";
			}
		}
	}
}
