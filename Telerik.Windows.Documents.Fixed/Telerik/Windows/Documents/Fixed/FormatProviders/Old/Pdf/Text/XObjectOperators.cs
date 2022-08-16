using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text
{
	static class XObjectOperators
	{
		public static bool IsOperator(string s)
		{
			return s != null && s == "Do";
		}

		public const string Do = "Do";
	}
}
