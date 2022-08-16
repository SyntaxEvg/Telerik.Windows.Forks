using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class FontSizeElement : RequiredValueElementBase<double>
	{
		public override string ElementName
		{
			get
			{
				return "sz";
			}
		}
	}
}
