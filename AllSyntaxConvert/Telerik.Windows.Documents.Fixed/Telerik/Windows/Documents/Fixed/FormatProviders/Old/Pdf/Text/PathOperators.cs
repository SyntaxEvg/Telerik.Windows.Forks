using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text
{
	static class PathOperators
	{
		public static bool IsOperator(string s)
		{
			switch (s)
			{
			case "m":
			case "l":
			case "c":
			case "v":
			case "y":
			case "h":
			case "re":
			case "s":
			case "S":
			case "f":
			case "F":
			case "f*":
			case "B":
			case "B*":
			case "b":
			case "b*":
			case "n":
			case "W":
			case "W*":
			case "sh":
				return true;
			}
			return false;
		}

		public const string m = "m";

		public const string l = "l";

		public const string c = "c";

		public const string v = "v";

		public const string y = "y";

		public const string h = "h";

		public const string re = "re";

		public const string S = "S";

		public const string s = "s";

		public const string f = "f";

		public const string F = "F";

		public const string fStar = "f*";

		public const string B = "B";

		public const string BStar = "B*";

		public const string b = "b";

		public const string bStar = "b*";

		public const string n = "n";

		public const string W = "W";

		public const string WStar = "W*";

		public const string Sh = "sh";
	}
}
