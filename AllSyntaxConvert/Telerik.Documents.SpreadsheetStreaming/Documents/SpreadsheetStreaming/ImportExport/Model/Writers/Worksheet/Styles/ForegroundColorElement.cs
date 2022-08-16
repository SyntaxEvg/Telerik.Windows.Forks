using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers.Worksheet.Styles
{
	class ForegroundColorElement : ColorElementBase
	{
		public override string ElementName
		{
			get
			{
				return "fgColor";
			}
		}
	}
}
