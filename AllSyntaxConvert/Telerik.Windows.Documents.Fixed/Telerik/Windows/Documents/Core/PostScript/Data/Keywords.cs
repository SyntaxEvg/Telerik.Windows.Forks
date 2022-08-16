using System;

namespace Telerik.Windows.Documents.Core.PostScript.Data
{
	static class Keywords
	{
		public static bool IsKeyword(string str)
		{
			return str != null && (str == "StandardEncoding" || str == "ISOLatin1Encoding");
		}

		internal static object GetValue(string keyword)
		{
			if (keyword != null && keyword == "StandardEncoding")
			{
				return new PostScriptArray(PredefinedEncodings.StandardEncoding);
			}
			return null;
		}

		internal const string True = "true";

		internal const string False = "false";

		internal const string IndirectReference = "R";

		internal const string Null = "null";

		internal const string StartXRef = "startxref";

		internal const string XRef = "xref";

		internal const string Trailer = "trailer";

		internal const string StreamStart = "stream";

		internal const string StreamEnd = "endstream";

		internal const string IndirectObjectStart = "obj";

		internal const string IndirectObjectEnd = "endobj";

		internal const string PdfHeader = "%PDF-";

		internal const string BinaryMarker = "%âãÏÓ";

		internal const string EndOfFile = "%%EOF";

		internal const string DictionaryStart = "<<";

		internal const string DictionaryEnd = ">>";

		internal const string EndOfInlineImage = "EI";

		internal const string StandardEncoding = "StandardEncoding";

		internal const string ISOLatin1Encoding = "ISOLatin1Encoding";
	}
}
