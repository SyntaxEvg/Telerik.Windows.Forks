using System;

namespace Telerik.Windows.Documents.Core.PostScript.Data
{
	static class PredefinedEncodings
	{
		public static string[] StandardEncoding { get; set; }

		static void InitializeStandardEncoding()
		{
			PredefinedEncodings.StandardEncoding = new string[256];
			PredefinedEncodings.StandardEncoding[179] = "daggerdbl";
			PredefinedEncodings.StandardEncoding[100] = "d";
			PredefinedEncodings.StandardEncoding[101] = "e";
			PredefinedEncodings.StandardEncoding[102] = "f";
			PredefinedEncodings.StandardEncoding[103] = "g";
			PredefinedEncodings.StandardEncoding[104] = "h";
			PredefinedEncodings.StandardEncoding[105] = "i";
			PredefinedEncodings.StandardEncoding[106] = "j";
			PredefinedEncodings.StandardEncoding[107] = "k";
			PredefinedEncodings.StandardEncoding[108] = "l";
			PredefinedEncodings.StandardEncoding[109] = "m";
			PredefinedEncodings.StandardEncoding[110] = "n";
			PredefinedEncodings.StandardEncoding[111] = "o";
			PredefinedEncodings.StandardEncoding[112] = "p";
			PredefinedEncodings.StandardEncoding[113] = "q";
			PredefinedEncodings.StandardEncoding[114] = "r";
			PredefinedEncodings.StandardEncoding[115] = "s";
			PredefinedEncodings.StandardEncoding[116] = "t";
			PredefinedEncodings.StandardEncoding[117] = "u";
			PredefinedEncodings.StandardEncoding[118] = "v";
			PredefinedEncodings.StandardEncoding[119] = "w";
			PredefinedEncodings.StandardEncoding[120] = "x";
			PredefinedEncodings.StandardEncoding[121] = "y";
			PredefinedEncodings.StandardEncoding[122] = "z";
			PredefinedEncodings.StandardEncoding[123] = "braceleft";
			PredefinedEncodings.StandardEncoding[124] = "bar";
			PredefinedEncodings.StandardEncoding[125] = "braceright";
			PredefinedEncodings.StandardEncoding[126] = "asciitilde";
			PredefinedEncodings.StandardEncoding[161] = "exclamdown";
			PredefinedEncodings.StandardEncoding[162] = "cent";
			PredefinedEncodings.StandardEncoding[163] = "sterling";
			PredefinedEncodings.StandardEncoding[164] = "fraction";
			PredefinedEncodings.StandardEncoding[165] = "yen";
			PredefinedEncodings.StandardEncoding[166] = "florin";
			PredefinedEncodings.StandardEncoding[167] = "section";
			PredefinedEncodings.StandardEncoding[168] = "currency";
			PredefinedEncodings.StandardEncoding[169] = "quotesingle";
			PredefinedEncodings.StandardEncoding[170] = "quotedblleft";
			PredefinedEncodings.StandardEncoding[171] = "guillemotleft";
			PredefinedEncodings.StandardEncoding[172] = "guilsinglleft";
			PredefinedEncodings.StandardEncoding[173] = "guilsinglright";
			PredefinedEncodings.StandardEncoding[174] = "fi";
			PredefinedEncodings.StandardEncoding[175] = "fl";
			PredefinedEncodings.StandardEncoding[177] = "endash";
			PredefinedEncodings.StandardEncoding[178] = "dagger";
			PredefinedEncodings.StandardEncoding[180] = "periodcentered";
			PredefinedEncodings.StandardEncoding[182] = "paragraph";
			PredefinedEncodings.StandardEncoding[183] = "bullet";
			PredefinedEncodings.StandardEncoding[184] = "quotesinglbase";
			PredefinedEncodings.StandardEncoding[185] = "quotedblbase";
			PredefinedEncodings.StandardEncoding[186] = "quotedblright";
			PredefinedEncodings.StandardEncoding[187] = "guillemotright";
			PredefinedEncodings.StandardEncoding[188] = "ellipsis";
			PredefinedEncodings.StandardEncoding[189] = "perthousand";
			PredefinedEncodings.StandardEncoding[191] = "questiondown";
			PredefinedEncodings.StandardEncoding[193] = "grave";
			PredefinedEncodings.StandardEncoding[194] = "acute";
			PredefinedEncodings.StandardEncoding[195] = "circumflex";
			PredefinedEncodings.StandardEncoding[196] = "tilde";
			PredefinedEncodings.StandardEncoding[197] = "macron";
			PredefinedEncodings.StandardEncoding[198] = "breve";
			PredefinedEncodings.StandardEncoding[199] = "dotaccent";
			PredefinedEncodings.StandardEncoding[200] = "dieresis";
			PredefinedEncodings.StandardEncoding[202] = "ring";
			PredefinedEncodings.StandardEncoding[203] = "cedilla";
			PredefinedEncodings.StandardEncoding[205] = "hungarumlaut";
			PredefinedEncodings.StandardEncoding[206] = "ogonek";
			PredefinedEncodings.StandardEncoding[207] = "caron";
			PredefinedEncodings.StandardEncoding[208] = "emdash";
			PredefinedEncodings.StandardEncoding[225] = "AE";
			PredefinedEncodings.StandardEncoding[227] = "ordfeminine";
			PredefinedEncodings.StandardEncoding[232] = "Lslash";
			PredefinedEncodings.StandardEncoding[233] = "Oslash";
			PredefinedEncodings.StandardEncoding[234] = "OE";
			PredefinedEncodings.StandardEncoding[235] = "ordmasculine";
			PredefinedEncodings.StandardEncoding[241] = "ae";
			PredefinedEncodings.StandardEncoding[245] = "dotlessi";
			PredefinedEncodings.StandardEncoding[248] = "lslash";
			PredefinedEncodings.StandardEncoding[249] = "oslash";
			PredefinedEncodings.StandardEncoding[250] = "oe";
			PredefinedEncodings.StandardEncoding[251] = "germandbls";
			PredefinedEncodings.StandardEncoding[32] = "space";
			PredefinedEncodings.StandardEncoding[33] = "exclam";
			PredefinedEncodings.StandardEncoding[34] = "quotedbl";
			PredefinedEncodings.StandardEncoding[35] = "numbersign";
			PredefinedEncodings.StandardEncoding[36] = "dollar";
			PredefinedEncodings.StandardEncoding[37] = "percent";
			PredefinedEncodings.StandardEncoding[38] = "ampersand";
			PredefinedEncodings.StandardEncoding[39] = "quoteright";
			PredefinedEncodings.StandardEncoding[40] = "parenleft";
			PredefinedEncodings.StandardEncoding[41] = "parenright";
			PredefinedEncodings.StandardEncoding[42] = "asterisk";
			PredefinedEncodings.StandardEncoding[43] = "plus";
			PredefinedEncodings.StandardEncoding[44] = "comma";
			PredefinedEncodings.StandardEncoding[45] = "hyphen";
			PredefinedEncodings.StandardEncoding[46] = "period";
			PredefinedEncodings.StandardEncoding[47] = "slash";
			PredefinedEncodings.StandardEncoding[48] = "zero";
			PredefinedEncodings.StandardEncoding[49] = "one";
			PredefinedEncodings.StandardEncoding[50] = "two";
			PredefinedEncodings.StandardEncoding[51] = "three";
			PredefinedEncodings.StandardEncoding[52] = "four";
			PredefinedEncodings.StandardEncoding[53] = "five";
			PredefinedEncodings.StandardEncoding[54] = "six";
			PredefinedEncodings.StandardEncoding[55] = "seven";
			PredefinedEncodings.StandardEncoding[56] = "eight";
			PredefinedEncodings.StandardEncoding[57] = "nine";
			PredefinedEncodings.StandardEncoding[58] = "colon";
			PredefinedEncodings.StandardEncoding[59] = "semicolon";
			PredefinedEncodings.StandardEncoding[60] = "less";
			PredefinedEncodings.StandardEncoding[61] = "equal";
			PredefinedEncodings.StandardEncoding[62] = "greater";
			PredefinedEncodings.StandardEncoding[63] = "question";
			PredefinedEncodings.StandardEncoding[64] = "at";
			PredefinedEncodings.StandardEncoding[65] = "A";
			PredefinedEncodings.StandardEncoding[66] = "B";
			PredefinedEncodings.StandardEncoding[67] = "C";
			PredefinedEncodings.StandardEncoding[68] = "D";
			PredefinedEncodings.StandardEncoding[69] = "E";
			PredefinedEncodings.StandardEncoding[70] = "F";
			PredefinedEncodings.StandardEncoding[71] = "G";
			PredefinedEncodings.StandardEncoding[72] = "H";
			PredefinedEncodings.StandardEncoding[73] = "I";
			PredefinedEncodings.StandardEncoding[74] = "J";
			PredefinedEncodings.StandardEncoding[75] = "K";
			PredefinedEncodings.StandardEncoding[76] = "L";
			PredefinedEncodings.StandardEncoding[77] = "M";
			PredefinedEncodings.StandardEncoding[78] = "N";
			PredefinedEncodings.StandardEncoding[79] = "O";
			PredefinedEncodings.StandardEncoding[80] = "P";
			PredefinedEncodings.StandardEncoding[81] = "Q";
			PredefinedEncodings.StandardEncoding[82] = "R";
			PredefinedEncodings.StandardEncoding[83] = "S";
			PredefinedEncodings.StandardEncoding[84] = "T";
			PredefinedEncodings.StandardEncoding[85] = "U";
			PredefinedEncodings.StandardEncoding[86] = "V";
			PredefinedEncodings.StandardEncoding[87] = "W";
			PredefinedEncodings.StandardEncoding[88] = "X";
			PredefinedEncodings.StandardEncoding[89] = "Y";
			PredefinedEncodings.StandardEncoding[90] = "Z";
			PredefinedEncodings.StandardEncoding[91] = "bracketleft";
			PredefinedEncodings.StandardEncoding[92] = "backslash";
			PredefinedEncodings.StandardEncoding[93] = "bracketright";
			PredefinedEncodings.StandardEncoding[94] = "asciicircum";
			PredefinedEncodings.StandardEncoding[95] = "underscore";
			PredefinedEncodings.StandardEncoding[96] = "quoteleft";
			PredefinedEncodings.StandardEncoding[97] = "a";
			PredefinedEncodings.StandardEncoding[98] = "b";
			PredefinedEncodings.StandardEncoding[99] = "c";
		}

		static PredefinedEncodings()
		{
			PredefinedEncodings.InitializeStandardEncoding();
		}

		public static PostScriptArray CreateEncoding(string predefinedEncoding)
		{
			string[] array = null;
			if (predefinedEncoding != null && predefinedEncoding == "StandardEncoding")
			{
				array = PredefinedEncodings.StandardEncoding;
			}
			if (array == null)
			{
				return null;
			}
			PostScriptArray postScriptArray = new PostScriptArray(array.Length);
			foreach (string item in array)
			{
				postScriptArray.Add(item);
			}
			return postScriptArray;
		}
	}
}
