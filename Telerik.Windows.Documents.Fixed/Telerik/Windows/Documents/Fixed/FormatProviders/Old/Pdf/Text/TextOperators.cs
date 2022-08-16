using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text
{
	static class TextOperators
	{
		public static bool IsOperator(string s)
		{
			switch (s)
			{
			case "BT":
			case "ET":
			case "Tc":
			case "Tw":
			case "Tz":
			case "TL":
			case "Tf":
			case "Tr":
			case "Ts":
			case "Tm":
			case "TJ":
			case "Tj":
			case "BDC":
			case "EMC":
			case "Td":
			case "TD":
			case "T*":
			case "'":
			case "\"":
				return true;
			}
			return false;
		}

		public const string BT = "BT";

		public const string ET = "ET";

		public const string Tf = "Tf";

		public const string Tc = "Tc";

		public const string Tm = "Tm";

		public const string Tw = "Tw";

		public const string Tz = "Tz";

		public const string TL = "TL";

		public const string Tr = "Tr";

		public const string Ts = "Ts";

		public const string TJ = "TJ";

		public const string Tj = "Tj";

		public const string BDC = "BDC";

		public const string EMC = "EMC";

		public const string Td = "Td";

		public const string TD = "TD";

		public const string TStar = "T*";

		public const string Ap = "'";

		public const string Qu = "\"";
	}
}
