using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text
{
	static class GeneralGraphicsStateOperators
	{
		public static bool IsOperator(string s)
		{
			switch (s)
			{
			case "w":
			case "J":
			case "j":
			case "M":
			case "d":
			case "ri":
			case "i":
			case "gs":
			case "q":
			case "Q":
			case "cm":
				return true;
			}
			return false;
		}

		public const string w = "w";

		public const string J = "J";

		public const string j = "j";

		public const string M = "M";

		public const string d = "d";

		public const string ri = "ri";

		public const string i = "i";

		public const string gs = "gs";

		public const string q = "q";

		public const string Q = "Q";

		public const string cm = "cm";
	}
}
