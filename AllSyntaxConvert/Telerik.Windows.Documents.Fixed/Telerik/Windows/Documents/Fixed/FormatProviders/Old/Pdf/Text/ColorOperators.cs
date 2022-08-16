using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text
{
	static class ColorOperators
	{
		public static bool IsOperator(string s)
		{
			switch (s)
			{
			case "CS":
			case "cs":
			case "SC":
			case "SCN":
			case "sc":
			case "scn":
			case "G":
			case "g":
			case "RG":
			case "rg":
			case "K":
			case "k":
				return true;
			}
			return false;
		}

		public const string CS = "CS";

		public const string cs = "cs";

		public const string SC = "SC";

		public const string SCN = "SCN";

		public const string sc = "sc";

		public const string scn = "scn";

		public const string G = "G";

		public const string g = "g";

		public const string RG = "RG";

		public const string rg = "rg";

		public const string K = "K";

		public const string k = "k";
	}
}
