using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Utilities
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
