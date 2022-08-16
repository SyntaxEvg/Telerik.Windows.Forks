using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	static class NumberFormatTypes
	{
		public static Dictionary<string, int> BuiltInFormatStrings
		{
			get
			{
				return NumberFormatTypes.builtInFormats;
			}
		}

		public static bool IsBuiltInFormat(string format)
		{
			return NumberFormatTypes.BuiltInFormatStrings.ContainsKey(format);
		}

		public static readonly int FirstCustomFormatId = 160;

		static readonly Dictionary<string, int> builtInFormats = BuiltInNumberFormatsGenerator.GetBuiltInFormatsForCurrentCulture();
	}
}
