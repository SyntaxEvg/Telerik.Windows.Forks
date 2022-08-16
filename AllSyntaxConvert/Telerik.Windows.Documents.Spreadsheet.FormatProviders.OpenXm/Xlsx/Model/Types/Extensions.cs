using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	static class Extensions
	{
		public static string ToValueString(this bool b)
		{
			if (!b)
			{
				return "0";
			}
			return "1";
		}
	}
}
