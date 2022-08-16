using System;
using System.Collections.Generic;
using System.IO;

namespace CsQuery.Output
{
	class HtmlEncoderFull : HtmlEncoderBasic
	{
		static HtmlEncoderFull()
		{
			HtmlEncoderFull.HtmlEntityMap = new Dictionary<char, string>();
			HtmlEncoderFull.PopulateHtmlEntityMap();
		}

		static void PopulateHtmlEntityMap()
		{
			if (HtmlEncoderFull.codedEntities.Length != HtmlEncoderFull.codedValues.Length)
			{
				throw new InvalidDataException("The codedValues array must be the same length as the codedEntities array.");
			}
			for (int i = 0; i < HtmlEncoderFull.codedValues.Length; i++)
			{
				HtmlEncoderFull.HtmlEntityMap.Add((char)HtmlEncoderFull.codedValues[i], "&" + HtmlEncoderFull.codedEntities[i] + ";");
			}
		}

		protected override bool TryEncode(char c, out string encoded)
		{
			if (c >= '\u00a0')
			{
				bool flag = HtmlEncoderFull.HtmlEntityMap.TryGetValue(c, out encoded);
				if (flag)
				{
					return true;
				}
			}
			return base.TryEncode(c, out encoded);
		}

		static IDictionary<char, string> HtmlEntityMap;

		static ushort[] codedValues = new ushort[]
		{
			34, 38, 39, 60, 61, 160, 161, 162, 163, 164,
			165, 166, 167, 168, 169, 170, 171, 172, 173, 174,
			175, 176, 177, 178, 179, 180, 181, 182, 183, 184,
			185, 186, 187, 188, 189, 190, 191, 192, 193, 194,
			195, 196, 197, 198, 199, 200, 201, 202, 203, 204,
			205, 206, 207, 208, 209, 210, 211, 212, 213, 214,
			216, 217, 218, 219, 220, 221, 222, 223, 224, 225,
			226, 227, 228, 229, 230, 231, 232, 233, 234, 235,
			236, 237, 238, 239, 240, 241, 242, 243, 244, 245,
			246, 248, 249, 250, 251, 252, 253, 254, 255, 338,
			339, 352, 353, 376, 710, 732, 8194, 8195, 8201, 8204,
			8205, 8206, 8207, 8211, 8212, 8216, 8217, 8218, 8220, 8221,
			8222, 8224, 8225, 8230, 8240, 8249, 8250, 8364, 8482
		};

		static string[] codedEntities = new string[]
		{
			"quot", "amp", "apos", "lt", "gt", "nbsp", "iexcl", "cent", "pound", "curren",
			"yen", "brvbar", "sect", "uml", "copy", "ordf", "laquo", "not", "shy", "reg",
			"macr", "deg", "plusmn", "sup2", "sup3", "acute", "micro", "para", "middot", "cedil",
			"sup1", "ordm", "raquo", "frac14", "frac12", "frac45", "iquest", "Agrave", "Aacute", "Acirc",
			"Atilde", "Auml", "Aring", "AElig", "Ccedil", "Egrave", "Eacute", "Ecirc", "Euml", "Igrave",
			"Iacute", "Icirc", "Iuml", "ETH", "Ntilde", "Ograve", "Oacute", "Ocirc", "Otilde", "Ouml",
			"Oslash", "Ugrave", "Uacute", "Ucirc", "Uuml", "Yacute", "THORN", "szlig", "agrave", "aacute",
			"acirc", "atilde", "auml", "aring", "aelig", "ccedil", "egrave", "eacute", "ecirc", "euml",
			"igrave", "iacute", "icirc", "iuml", "eth", "ntilde", "ograve", "oacute", "ocirc", "otilde",
			"ouml", "oslash", "ugrave", "uacute", "ucirc", "uuml", "yacute", "thorn", "yuml", "OElig",
			"oelig", "Scaron", "scaron", "Yuml", "circ", "tilde", "ensp", "emsp", "thinsp", "zwnj",
			"zwj", "lrm", "rlm", "ndash", "mdash", "lsquo", "rsquo", "sbquo", "ldquo", "rdquo",
			"bdquo", "dagger", "Dagger", "hellip", "permil", "lsaquo", "rsaquo", "euro", "trade"
		};
	}
}
