using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class NoneFill : IFill
	{
		NoneFill()
		{
		}

		public static readonly IFill Instance = new NoneFill();
	}
}
