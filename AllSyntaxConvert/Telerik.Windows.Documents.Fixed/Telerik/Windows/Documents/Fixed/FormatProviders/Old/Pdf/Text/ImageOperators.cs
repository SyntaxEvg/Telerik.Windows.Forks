using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text
{
	static class ImageOperators
	{
		public static bool IsOperator(string s)
		{
			return s != null && (s == "BI" || s == "EI" || s == "ID");
		}

		public const string BI = "BI";

		public const string ID = "ID";

		public const string EI = "EI";
	}
}
