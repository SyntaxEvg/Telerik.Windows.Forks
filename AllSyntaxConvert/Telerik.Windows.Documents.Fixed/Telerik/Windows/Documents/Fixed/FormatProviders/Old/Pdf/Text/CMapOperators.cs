using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text
{
	static class CMapOperators
	{
		public static bool IsOperator(string s)
		{
			switch (s)
			{
			case "beginbfchar":
			case "beginbfrange":
			case "begincodespacerange":
			case "begincidchar":
			case "begincidrange":
			case "beginnotdefchar,":
			case "beginnotdefrange,":
				return true;
			}
			return false;
		}

		public const string beginbfrange = "beginbfrange";

		public const string beginbfchar = "beginbfchar";

		public const string begincodespacerange = "begincodespacerange";

		public const string begincidchar = "begincidchar";

		public const string begincidrange = "begincidrange";

		public const string beginnotdefchar = "beginnotdefchar,";

		public const string beginnotdefrange = "beginnotdefrange,";
	}
}
