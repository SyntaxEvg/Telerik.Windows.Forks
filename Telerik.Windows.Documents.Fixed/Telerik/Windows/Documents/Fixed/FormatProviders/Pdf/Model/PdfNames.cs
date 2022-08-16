using System;
using System.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model
{
	static class PdfNames
	{
		public static string EscapeName(string name)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int length = name.Length;
			for (int i = 0; i < length; i++)
			{
				char c = name[i];
				byte b = (byte)c;
				if (Characters.IsDelimiter(b) || Characters.IsWhiteSpace(b) || c == '#')
				{
					stringBuilder.Append(string.Format("#{0:X2}", b));
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		public static string StripEscape(StringBuilder name)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int length = name.Length;
			for (int i = 0; i < length; i++)
			{
				char c = name[i];
				if (c == '#' && i + 2 < length)
				{
					stringBuilder.Append((char)BytesHelper.ToByteArray(name.ToString(i + 1, 2))[0]);
					i += 2;
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		public static string GetNameFromAbbreviation(string abbreviation)
		{
			switch (abbreviation)
			{
			case "BPC":
				return "BitsPerComponent";
			case "CS":
				return "ColorSpace";
			case "D":
				return "Decode";
			case "DP":
				return "DecodeParms";
			case "F":
				return "Filter";
			case "H":
				return "Height";
			case "IM":
				return "ImageMask";
			case "I":
				return "Interpolate";
			case "W":
				return "Width";
			case "AHx":
				return "ASCIIHexDecode";
			case "A85":
				return "ASCII85Decode";
			case "LZW":
				return "LZWDecode";
			case "Fl":
				return "FlateDecode";
			case "RL":
				return "RunLengthDecode";
			case "CCF":
				return "CCITTFaxDecode";
			case "DCT":
				return "DCTDecode";
			case "G":
				return "DeviceGray";
			case "RGB":
				return "DeviceRGB";
			case "CMYK":
				return "DeviceCMYK";
			}
			return abbreviation;
		}

		public const string PdfNameIdentifier = "/";

		public const string IndirectObjectStart = "obj";

		public const string IndirectObjectEnd = "endobj";

		public const string PdfStreamStart = "stream";

		public const string PdfStreamEnd = "endstream";

		public const string IndirectReferenceMarker = "R";

		public const string PdfDocumentHeader = "%PDF-1.7";

		public const string BinaryMarker = "%úûüý";

		public const string Type = "Type";

		public const string SubType = "Subtype";

		public const string S = "S";

		public const string CrossReferenceTableMarker = "xref";

		public const string StartXRef = "startxref";

		public const string TrailerMarker = "trailer";

		public const string EndOfFileMarker = "%%EOF";

		public const string Length = "Length";

		public const string Filters = "Filter";

		public const string Filter = "Filter";

		public const string StandardEncrypt = "Standard";

		public const string Identity = "Identity";

		public const string IdentityH = "Identity-H";

		public const string PdfNull = "null";

		public const string PatternType = "PatternType";

		public const string ShadingType = "ShadingType";

		public const string FunctionType = "FunctionType";

		public const string DecodeParameters = "DecodeParms";

		public const string ColorSpace = "ColorSpace";

		public const string StandardCryptFilter = "StdCF";

		public static readonly string HexadecimalStringStart = '<'.ToString();

		public static readonly string HexadecimalStringEnd = '>'.ToString();

		public static readonly string PdfDictionaryStart = string.Format("{0}{0}", '<');

		public static readonly string PdfDictionaryEnd = string.Format("{0}{0}", '>');

		public static readonly string PdfArrayStart = '['.ToString();

		public static readonly string PdfArrayEnd = ']'.ToString();
	}
}
