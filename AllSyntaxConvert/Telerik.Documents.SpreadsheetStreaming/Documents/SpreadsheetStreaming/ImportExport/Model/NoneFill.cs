using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model
{
	class NoneFill : ISpreadFill
	{
		NoneFill()
		{
		}

		public static readonly ISpreadFill Instance = new NoneFill();
	}
}
