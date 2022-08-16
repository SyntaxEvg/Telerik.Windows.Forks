using System;
using System.Text;

namespace Telerik.Documents.SpreadsheetStreaming.Utilities
{
	class SpreadsheetCultureHelper
	{
		public static string ClearFormulaValue(string formula)
		{
			StringBuilder stringBuilder = new StringBuilder(formula.Trim());
			if (stringBuilder.Length > 0 && SpreadsheetCultureHelper.IsCharEqualTo(stringBuilder[0], new string[] { "=" }))
			{
				stringBuilder.Remove(0, 1);
			}
			return stringBuilder.ToString();
		}

		public static bool IsCharEqualTo(char ch, params string[] values)
		{
			string a = ch.ToString();
			for (int i = 0; i < values.Length; i++)
			{
				if (a == values[i])
				{
					return true;
				}
			}
			return false;
		}

		internal const string Colon = ":";

		internal const string FunctionStart = "=";
	}
}
