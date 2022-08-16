﻿using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding
{
	class PredefinedSimpleEncoding
	{
		PredefinedSimpleEncoding(string encodingName, Action<string[]> initDefinedNames)
		{
			this.encodingName = encodingName;
			this.names = new string[256];
			for (int i = 0; i < 256; i++)
			{
				this.names[i] = ".notdef";
			}
			initDefinedNames(this.names);
			this.mapping = new Dictionary<string, byte>(this.names.Length);
			for (int j = 0; j < this.names.Length; j++)
			{
				if (this.names[j] != null)
				{
					this.mapping[this.names[j]] = (byte)j;
				}
			}
		}

		public string Name
		{
			get
			{
				return this.encodingName;
			}
		}

		public static PredefinedSimpleEncoding GetPredefinedEncoding(string encoding)
		{
			if (encoding != null)
			{
				if (encoding == "PdfDocEncoding")
				{
					return PredefinedSimpleEncoding.PdfDocEncoding;
				}
				if (encoding == "MacRomanEncoding")
				{
					return PredefinedSimpleEncoding.MacRomanEncoding;
				}
				if (encoding == "WinAnsiEncoding")
				{
					return PredefinedSimpleEncoding.WinAnsiEncoding;
				}
				if (encoding == "StandardEncoding")
				{
					return PredefinedSimpleEncoding.StandardEncoding;
				}
			}
			return null;
		}

		public string[] GetNames()
		{
			return (string[])this.names.Clone();
		}

		public byte GetCharId(string name)
		{
			return this.mapping[name];
		}

		static void InitializePdfEncoding(string[] pdfDocEncodingNames)
		{
			pdfDocEncodingNames[130] = "adblgrave";
			pdfDocEncodingNames[100] = "d";
			pdfDocEncodingNames[101] = "e";
			pdfDocEncodingNames[102] = "f";
			pdfDocEncodingNames[103] = "g";
			pdfDocEncodingNames[104] = "h";
			pdfDocEncodingNames[105] = "i";
			pdfDocEncodingNames[106] = "j";
			pdfDocEncodingNames[107] = "k";
			pdfDocEncodingNames[108] = "l";
			pdfDocEncodingNames[109] = "m";
			pdfDocEncodingNames[110] = "n";
			pdfDocEncodingNames[111] = "o";
			pdfDocEncodingNames[112] = "p";
			pdfDocEncodingNames[113] = "q";
			pdfDocEncodingNames[114] = "r";
			pdfDocEncodingNames[115] = "s";
			pdfDocEncodingNames[116] = "t";
			pdfDocEncodingNames[117] = "u";
			pdfDocEncodingNames[118] = "v";
			pdfDocEncodingNames[119] = "w";
			pdfDocEncodingNames[120] = "x";
			pdfDocEncodingNames[121] = "y";
			pdfDocEncodingNames[122] = "z";
			pdfDocEncodingNames[123] = "braceleft";
			pdfDocEncodingNames[124] = "bar";
			pdfDocEncodingNames[125] = "braceright";
			pdfDocEncodingNames[126] = "asciitilde";
			pdfDocEncodingNames[128] = "bullet";
			pdfDocEncodingNames[129] = "dagger";
			pdfDocEncodingNames[131] = "ellipsis";
			pdfDocEncodingNames[132] = "emdash";
			pdfDocEncodingNames[133] = "endash";
			pdfDocEncodingNames[134] = "florin";
			pdfDocEncodingNames[135] = "fraction";
			pdfDocEncodingNames[136] = "guilsinglleft";
			pdfDocEncodingNames[137] = "guilsinglright";
			pdfDocEncodingNames[138] = "minus";
			pdfDocEncodingNames[139] = "perthousand";
			pdfDocEncodingNames[140] = "quotedblbase";
			pdfDocEncodingNames[141] = "quotedblleft";
			pdfDocEncodingNames[142] = "quotedblright";
			pdfDocEncodingNames[143] = "quoteleft";
			pdfDocEncodingNames[144] = "quoteright";
			pdfDocEncodingNames[145] = "quotesinglbase";
			pdfDocEncodingNames[146] = "trademark";
			pdfDocEncodingNames[147] = "fi";
			pdfDocEncodingNames[148] = "fl";
			pdfDocEncodingNames[149] = "Lslash";
			pdfDocEncodingNames[150] = "OE";
			pdfDocEncodingNames[151] = "Scaron";
			pdfDocEncodingNames[152] = "Ydieresis";
			pdfDocEncodingNames[153] = "Zcaron";
			pdfDocEncodingNames[154] = "dotlessi";
			pdfDocEncodingNames[155] = "lslash";
			pdfDocEncodingNames[156] = "oe";
			pdfDocEncodingNames[157] = "scaron";
			pdfDocEncodingNames[158] = "zcaron";
			pdfDocEncodingNames[160] = "Euro";
			pdfDocEncodingNames[161] = "exclamdown";
			pdfDocEncodingNames[162] = "cent";
			pdfDocEncodingNames[163] = "sterling";
			pdfDocEncodingNames[164] = "currency";
			pdfDocEncodingNames[165] = "yen";
			pdfDocEncodingNames[166] = "brokenbar";
			pdfDocEncodingNames[167] = "section";
			pdfDocEncodingNames[168] = "dieresis";
			pdfDocEncodingNames[169] = "copyright";
			pdfDocEncodingNames[170] = "ordfeminine";
			pdfDocEncodingNames[171] = "guillemotleft";
			pdfDocEncodingNames[172] = "logicalnot";
			pdfDocEncodingNames[174] = "registered";
			pdfDocEncodingNames[175] = "macron";
			pdfDocEncodingNames[176] = "degree";
			pdfDocEncodingNames[177] = "plusminus";
			pdfDocEncodingNames[178] = "twosuperior";
			pdfDocEncodingNames[179] = "threesuperior";
			pdfDocEncodingNames[180] = "acute";
			pdfDocEncodingNames[181] = "mu";
			pdfDocEncodingNames[182] = "paragraph";
			pdfDocEncodingNames[183] = "middot";
			pdfDocEncodingNames[184] = "cedilla";
			pdfDocEncodingNames[185] = "onesuperior";
			pdfDocEncodingNames[186] = "ordmasculine";
			pdfDocEncodingNames[187] = "guillemotright";
			pdfDocEncodingNames[188] = "onequarter";
			pdfDocEncodingNames[189] = "onehalf";
			pdfDocEncodingNames[190] = "threequarters";
			pdfDocEncodingNames[191] = "questiondown";
			pdfDocEncodingNames[192] = "Agrave";
			pdfDocEncodingNames[193] = "Aacute";
			pdfDocEncodingNames[194] = "Acircumflex";
			pdfDocEncodingNames[195] = "Atilde";
			pdfDocEncodingNames[196] = "Adieresis";
			pdfDocEncodingNames[197] = "Aring";
			pdfDocEncodingNames[198] = "AE";
			pdfDocEncodingNames[199] = "Ccedilla";
			pdfDocEncodingNames[200] = "Egrave";
			pdfDocEncodingNames[201] = "Eacute";
			pdfDocEncodingNames[202] = "Ecircumflex";
			pdfDocEncodingNames[203] = "Edieresis";
			pdfDocEncodingNames[204] = "Igrave";
			pdfDocEncodingNames[205] = "Iacute";
			pdfDocEncodingNames[206] = "Icircumflex";
			pdfDocEncodingNames[207] = "Idieresis";
			pdfDocEncodingNames[208] = "Eth";
			pdfDocEncodingNames[209] = "Ntilde";
			pdfDocEncodingNames[211] = "Oacute";
			pdfDocEncodingNames[212] = "Ocircumflex";
			pdfDocEncodingNames[213] = "Otilde";
			pdfDocEncodingNames[214] = "Odieresis";
			pdfDocEncodingNames[215] = "multiply";
			pdfDocEncodingNames[216] = "Oslash";
			pdfDocEncodingNames[217] = "Ugrave";
			pdfDocEncodingNames[218] = "Uacute";
			pdfDocEncodingNames[219] = "Ucircumflex";
			pdfDocEncodingNames[220] = "Udieresis";
			pdfDocEncodingNames[221] = "Yacute";
			pdfDocEncodingNames[222] = "Thorn";
			pdfDocEncodingNames[223] = "germandbls";
			pdfDocEncodingNames[224] = "agrave";
			pdfDocEncodingNames[225] = "aacute";
			pdfDocEncodingNames[226] = "acircumflex";
			pdfDocEncodingNames[227] = "atilde";
			pdfDocEncodingNames[228] = "adieresis";
			pdfDocEncodingNames[229] = "aring";
			pdfDocEncodingNames[230] = "ae";
			pdfDocEncodingNames[231] = "ccedilla";
			pdfDocEncodingNames[232] = "egrave";
			pdfDocEncodingNames[233] = "eacute";
			pdfDocEncodingNames[234] = "ecircumflex";
			pdfDocEncodingNames[235] = "edieresis";
			pdfDocEncodingNames[236] = "igrave";
			pdfDocEncodingNames[237] = "iacute";
			pdfDocEncodingNames[238] = "icircumflex";
			pdfDocEncodingNames[239] = "idieresis";
			pdfDocEncodingNames[24] = "breve";
			pdfDocEncodingNames[240] = "eth";
			pdfDocEncodingNames[241] = "ntilde";
			pdfDocEncodingNames[242] = "ograve";
			pdfDocEncodingNames[243] = "oacute";
			pdfDocEncodingNames[244] = "ocircumflex";
			pdfDocEncodingNames[245] = "otilde";
			pdfDocEncodingNames[246] = "odieresis";
			pdfDocEncodingNames[247] = "divide";
			pdfDocEncodingNames[248] = "oslash";
			pdfDocEncodingNames[249] = "ugrave";
			pdfDocEncodingNames[25] = "caron";
			pdfDocEncodingNames[250] = "uacute";
			pdfDocEncodingNames[251] = "ucircumflex";
			pdfDocEncodingNames[252] = "udieresis";
			pdfDocEncodingNames[253] = "yacute";
			pdfDocEncodingNames[254] = "thorn";
			pdfDocEncodingNames[255] = "ydieresis";
			pdfDocEncodingNames[26] = "circumflex";
			pdfDocEncodingNames[27] = "dotaccent";
			pdfDocEncodingNames[28] = "hungarumlaut";
			pdfDocEncodingNames[29] = "ogonek";
			pdfDocEncodingNames[30] = "ring";
			pdfDocEncodingNames[31] = "ilde";
			pdfDocEncodingNames[32] = "space";
			pdfDocEncodingNames[33] = "exclam";
			pdfDocEncodingNames[34] = "quotedbl";
			pdfDocEncodingNames[35] = "numbersign";
			pdfDocEncodingNames[36] = "dollar";
			pdfDocEncodingNames[37] = "percent";
			pdfDocEncodingNames[38] = "ampersand";
			pdfDocEncodingNames[39] = "quotesingle";
			pdfDocEncodingNames[40] = "parenleft";
			pdfDocEncodingNames[41] = "parenright";
			pdfDocEncodingNames[42] = "asterisk";
			pdfDocEncodingNames[43] = "plus";
			pdfDocEncodingNames[44] = "comma";
			pdfDocEncodingNames[45] = "hyphen";
			pdfDocEncodingNames[46] = "period";
			pdfDocEncodingNames[47] = "slash";
			pdfDocEncodingNames[48] = "zero";
			pdfDocEncodingNames[49] = "one";
			pdfDocEncodingNames[50] = "two";
			pdfDocEncodingNames[51] = "three";
			pdfDocEncodingNames[52] = "four";
			pdfDocEncodingNames[53] = "five";
			pdfDocEncodingNames[54] = "six";
			pdfDocEncodingNames[55] = "seven";
			pdfDocEncodingNames[56] = "eight";
			pdfDocEncodingNames[57] = "nine";
			pdfDocEncodingNames[58] = "colon";
			pdfDocEncodingNames[59] = "semicolon";
			pdfDocEncodingNames[60] = "less";
			pdfDocEncodingNames[61] = "equal";
			pdfDocEncodingNames[62] = "greater";
			pdfDocEncodingNames[63] = "question";
			pdfDocEncodingNames[64] = "at";
			pdfDocEncodingNames[65] = "A";
			pdfDocEncodingNames[66] = "B";
			pdfDocEncodingNames[67] = "C";
			pdfDocEncodingNames[68] = "D";
			pdfDocEncodingNames[69] = "E";
			pdfDocEncodingNames[70] = "F";
			pdfDocEncodingNames[71] = "G";
			pdfDocEncodingNames[72] = "H";
			pdfDocEncodingNames[73] = "I";
			pdfDocEncodingNames[74] = "J";
			pdfDocEncodingNames[75] = "K";
			pdfDocEncodingNames[76] = "L";
			pdfDocEncodingNames[77] = "M";
			pdfDocEncodingNames[78] = "N";
			pdfDocEncodingNames[79] = "O";
			pdfDocEncodingNames[80] = "P";
			pdfDocEncodingNames[81] = "Q";
			pdfDocEncodingNames[82] = "R";
			pdfDocEncodingNames[83] = "S";
			pdfDocEncodingNames[84] = "T";
			pdfDocEncodingNames[85] = "U";
			pdfDocEncodingNames[86] = "V";
			pdfDocEncodingNames[87] = "W";
			pdfDocEncodingNames[88] = "X";
			pdfDocEncodingNames[89] = "Y";
			pdfDocEncodingNames[90] = "Z";
			pdfDocEncodingNames[91] = "bracketleft";
			pdfDocEncodingNames[92] = "backslash";
			pdfDocEncodingNames[93] = "bracketright";
			pdfDocEncodingNames[94] = "asciicircum";
			pdfDocEncodingNames[95] = "underscore";
			pdfDocEncodingNames[96] = "grave";
			pdfDocEncodingNames[97] = "a";
			pdfDocEncodingNames[98] = "b";
			pdfDocEncodingNames[99] = "c";
		}

		static void InitializeWinAnsiEncoding(string[] winAnsiEncodingNames)
		{
			winAnsiEncodingNames[173] = "hyphen";
			winAnsiEncodingNames[160] = "space";
			winAnsiEncodingNames[135] = "adblgrave";
			winAnsiEncodingNames[100] = "d";
			winAnsiEncodingNames[101] = "e";
			winAnsiEncodingNames[102] = "f";
			winAnsiEncodingNames[103] = "g";
			winAnsiEncodingNames[104] = "h";
			winAnsiEncodingNames[105] = "i";
			winAnsiEncodingNames[106] = "j";
			winAnsiEncodingNames[107] = "k";
			winAnsiEncodingNames[108] = "l";
			winAnsiEncodingNames[109] = "m";
			winAnsiEncodingNames[110] = "n";
			winAnsiEncodingNames[111] = "o";
			winAnsiEncodingNames[112] = "p";
			winAnsiEncodingNames[113] = "q";
			winAnsiEncodingNames[114] = "r";
			winAnsiEncodingNames[115] = "s";
			winAnsiEncodingNames[116] = "t";
			winAnsiEncodingNames[117] = "u";
			winAnsiEncodingNames[118] = "v";
			winAnsiEncodingNames[119] = "w";
			winAnsiEncodingNames[120] = "x";
			winAnsiEncodingNames[121] = "y";
			winAnsiEncodingNames[122] = "z";
			winAnsiEncodingNames[123] = "braceleft";
			winAnsiEncodingNames[124] = "bar";
			winAnsiEncodingNames[125] = "braceright";
			winAnsiEncodingNames[126] = "asciitilde";
			winAnsiEncodingNames[128] = "Euro";
			winAnsiEncodingNames[130] = "quotesinglbase";
			winAnsiEncodingNames[131] = "florin";
			winAnsiEncodingNames[132] = "quotedblbase";
			winAnsiEncodingNames[133] = "ellipsis";
			winAnsiEncodingNames[134] = "dagger";
			winAnsiEncodingNames[136] = "circumflex";
			winAnsiEncodingNames[137] = "perthousand";
			winAnsiEncodingNames[138] = "Scaron";
			winAnsiEncodingNames[139] = "guilsinglleft";
			winAnsiEncodingNames[140] = "OE";
			winAnsiEncodingNames[142] = "Zcaron";
			winAnsiEncodingNames[145] = "quoteleft";
			winAnsiEncodingNames[146] = "quoteright";
			winAnsiEncodingNames[147] = "quotedblleft";
			winAnsiEncodingNames[148] = "quotedblright";
			winAnsiEncodingNames[149] = "bullet";
			winAnsiEncodingNames[150] = "endash";
			winAnsiEncodingNames[151] = "emdash";
			winAnsiEncodingNames[152] = "ilde";
			winAnsiEncodingNames[153] = "trademark";
			winAnsiEncodingNames[154] = "scaron";
			winAnsiEncodingNames[155] = "guilsinglright";
			winAnsiEncodingNames[156] = "oe";
			winAnsiEncodingNames[158] = "zcaron";
			winAnsiEncodingNames[159] = "Ydieresis";
			winAnsiEncodingNames[161] = "exclamdown";
			winAnsiEncodingNames[162] = "cent";
			winAnsiEncodingNames[163] = "sterling";
			winAnsiEncodingNames[164] = "currency";
			winAnsiEncodingNames[165] = "yen";
			winAnsiEncodingNames[166] = "brokenbar";
			winAnsiEncodingNames[167] = "section";
			winAnsiEncodingNames[168] = "dieresis";
			winAnsiEncodingNames[169] = "copyright";
			winAnsiEncodingNames[170] = "ordfeminine";
			winAnsiEncodingNames[171] = "guillemotleft";
			winAnsiEncodingNames[172] = "logicalnot";
			winAnsiEncodingNames[174] = "registered";
			winAnsiEncodingNames[175] = "macron";
			winAnsiEncodingNames[176] = "degree";
			winAnsiEncodingNames[177] = "plusminus";
			winAnsiEncodingNames[178] = "twosuperior";
			winAnsiEncodingNames[179] = "threesuperior";
			winAnsiEncodingNames[180] = "acute";
			winAnsiEncodingNames[181] = "mu";
			winAnsiEncodingNames[182] = "paragraph";
			winAnsiEncodingNames[183] = "middot";
			winAnsiEncodingNames[184] = "cedilla";
			winAnsiEncodingNames[185] = "onesuperior";
			winAnsiEncodingNames[186] = "ordmasculine";
			winAnsiEncodingNames[187] = "guillemotright";
			winAnsiEncodingNames[188] = "onequarter";
			winAnsiEncodingNames[189] = "onehalf";
			winAnsiEncodingNames[190] = "threequarters";
			winAnsiEncodingNames[191] = "questiondown";
			winAnsiEncodingNames[192] = "Agrave";
			winAnsiEncodingNames[193] = "Aacute";
			winAnsiEncodingNames[194] = "Acircumflex";
			winAnsiEncodingNames[195] = "Atilde";
			winAnsiEncodingNames[196] = "Adieresis";
			winAnsiEncodingNames[197] = "Aring";
			winAnsiEncodingNames[198] = "AE";
			winAnsiEncodingNames[199] = "Ccedilla";
			winAnsiEncodingNames[200] = "Egrave";
			winAnsiEncodingNames[201] = "Eacute";
			winAnsiEncodingNames[202] = "Ecircumflex";
			winAnsiEncodingNames[203] = "Edieresis";
			winAnsiEncodingNames[204] = "Igrave";
			winAnsiEncodingNames[205] = "Iacute";
			winAnsiEncodingNames[206] = "Icircumflex";
			winAnsiEncodingNames[207] = "Idieresis";
			winAnsiEncodingNames[208] = "Eth";
			winAnsiEncodingNames[209] = "Ntilde";
			winAnsiEncodingNames[210] = "Ograve";
			winAnsiEncodingNames[211] = "Oacute";
			winAnsiEncodingNames[212] = "Ocircumflex";
			winAnsiEncodingNames[213] = "Otilde";
			winAnsiEncodingNames[214] = "Odieresis";
			winAnsiEncodingNames[215] = "multiply";
			winAnsiEncodingNames[216] = "Oslash";
			winAnsiEncodingNames[217] = "Ugrave";
			winAnsiEncodingNames[218] = "Uacute";
			winAnsiEncodingNames[219] = "Ucircumflex";
			winAnsiEncodingNames[220] = "Udieresis";
			winAnsiEncodingNames[221] = "Yacute";
			winAnsiEncodingNames[222] = "Thorn";
			winAnsiEncodingNames[223] = "germandbls";
			winAnsiEncodingNames[224] = "agrave";
			winAnsiEncodingNames[225] = "aacute";
			winAnsiEncodingNames[226] = "acircumflex";
			winAnsiEncodingNames[227] = "atilde";
			winAnsiEncodingNames[228] = "adieresis";
			winAnsiEncodingNames[229] = "aring";
			winAnsiEncodingNames[230] = "ae";
			winAnsiEncodingNames[231] = "ccedilla";
			winAnsiEncodingNames[232] = "egrave";
			winAnsiEncodingNames[233] = "eacute";
			winAnsiEncodingNames[234] = "ecircumflex";
			winAnsiEncodingNames[235] = "edieresis";
			winAnsiEncodingNames[236] = "igrave";
			winAnsiEncodingNames[237] = "iacute";
			winAnsiEncodingNames[238] = "icircumflex";
			winAnsiEncodingNames[239] = "idieresis";
			winAnsiEncodingNames[240] = "eth";
			winAnsiEncodingNames[241] = "ntilde";
			winAnsiEncodingNames[242] = "ograve";
			winAnsiEncodingNames[243] = "oacute";
			winAnsiEncodingNames[244] = "ocircumflex";
			winAnsiEncodingNames[245] = "otilde";
			winAnsiEncodingNames[246] = "odieresis";
			winAnsiEncodingNames[247] = "divide";
			winAnsiEncodingNames[248] = "oslash";
			winAnsiEncodingNames[249] = "ugrave";
			winAnsiEncodingNames[250] = "uacute";
			winAnsiEncodingNames[251] = "ucircumflex";
			winAnsiEncodingNames[252] = "udieresis";
			winAnsiEncodingNames[253] = "yacute";
			winAnsiEncodingNames[254] = "thorn";
			winAnsiEncodingNames[255] = "ydieresis";
			winAnsiEncodingNames[32] = "space";
			winAnsiEncodingNames[33] = "exclam";
			winAnsiEncodingNames[34] = "quotedbl";
			winAnsiEncodingNames[35] = "numbersign";
			winAnsiEncodingNames[36] = "dollar";
			winAnsiEncodingNames[37] = "percent";
			winAnsiEncodingNames[38] = "ampersand";
			winAnsiEncodingNames[39] = "quotesingle";
			winAnsiEncodingNames[40] = "parenleft";
			winAnsiEncodingNames[41] = "parenright";
			winAnsiEncodingNames[42] = "asterisk";
			winAnsiEncodingNames[43] = "plus";
			winAnsiEncodingNames[44] = "comma";
			winAnsiEncodingNames[45] = "hyphen";
			winAnsiEncodingNames[46] = "period";
			winAnsiEncodingNames[47] = "slash";
			winAnsiEncodingNames[48] = "zero";
			winAnsiEncodingNames[49] = "one";
			winAnsiEncodingNames[50] = "two";
			winAnsiEncodingNames[51] = "three";
			winAnsiEncodingNames[52] = "four";
			winAnsiEncodingNames[53] = "five";
			winAnsiEncodingNames[54] = "six";
			winAnsiEncodingNames[55] = "seven";
			winAnsiEncodingNames[56] = "eight";
			winAnsiEncodingNames[57] = "nine";
			winAnsiEncodingNames[58] = "colon";
			winAnsiEncodingNames[59] = "semicolon";
			winAnsiEncodingNames[60] = "less";
			winAnsiEncodingNames[61] = "equal";
			winAnsiEncodingNames[62] = "greater";
			winAnsiEncodingNames[63] = "question";
			winAnsiEncodingNames[64] = "at";
			winAnsiEncodingNames[65] = "A";
			winAnsiEncodingNames[66] = "B";
			winAnsiEncodingNames[67] = "C";
			winAnsiEncodingNames[68] = "D";
			winAnsiEncodingNames[69] = "E";
			winAnsiEncodingNames[70] = "F";
			winAnsiEncodingNames[71] = "G";
			winAnsiEncodingNames[72] = "H";
			winAnsiEncodingNames[73] = "I";
			winAnsiEncodingNames[74] = "J";
			winAnsiEncodingNames[75] = "K";
			winAnsiEncodingNames[76] = "L";
			winAnsiEncodingNames[77] = "M";
			winAnsiEncodingNames[78] = "N";
			winAnsiEncodingNames[79] = "O";
			winAnsiEncodingNames[80] = "P";
			winAnsiEncodingNames[81] = "Q";
			winAnsiEncodingNames[82] = "R";
			winAnsiEncodingNames[83] = "S";
			winAnsiEncodingNames[84] = "T";
			winAnsiEncodingNames[85] = "U";
			winAnsiEncodingNames[86] = "V";
			winAnsiEncodingNames[87] = "W";
			winAnsiEncodingNames[88] = "X";
			winAnsiEncodingNames[89] = "Y";
			winAnsiEncodingNames[90] = "Z";
			winAnsiEncodingNames[91] = "bracketleft";
			winAnsiEncodingNames[92] = "backslash";
			winAnsiEncodingNames[93] = "bracketright";
			winAnsiEncodingNames[94] = "asciicircum";
			winAnsiEncodingNames[95] = "underscore";
			winAnsiEncodingNames[96] = "grave";
			winAnsiEncodingNames[97] = "a";
			winAnsiEncodingNames[98] = "b";
			winAnsiEncodingNames[99] = "c";
		}

		static void InitializeMacEncoding(string[] macEncodingNames)
		{
			macEncodingNames[202] = "space";
			macEncodingNames[224] = "adblgrave";
			macEncodingNames[100] = "d";
			macEncodingNames[101] = "e";
			macEncodingNames[102] = "f";
			macEncodingNames[103] = "g";
			macEncodingNames[104] = "h";
			macEncodingNames[105] = "i";
			macEncodingNames[106] = "j";
			macEncodingNames[107] = "k";
			macEncodingNames[108] = "l";
			macEncodingNames[109] = "m";
			macEncodingNames[110] = "n";
			macEncodingNames[111] = "o";
			macEncodingNames[112] = "p";
			macEncodingNames[113] = "q";
			macEncodingNames[114] = "r";
			macEncodingNames[115] = "s";
			macEncodingNames[116] = "t";
			macEncodingNames[117] = "u";
			macEncodingNames[118] = "v";
			macEncodingNames[119] = "w";
			macEncodingNames[120] = "x";
			macEncodingNames[121] = "y";
			macEncodingNames[122] = "z";
			macEncodingNames[123] = "braceleft";
			macEncodingNames[124] = "bar";
			macEncodingNames[125] = "braceright";
			macEncodingNames[126] = "asciitilde";
			macEncodingNames[128] = "Adieresis";
			macEncodingNames[129] = "Aring";
			macEncodingNames[130] = "Ccedilla";
			macEncodingNames[131] = "Eacute";
			macEncodingNames[132] = "Ograve";
			macEncodingNames[133] = "Odieresis";
			macEncodingNames[134] = "Udieresis";
			macEncodingNames[135] = "aacute";
			macEncodingNames[136] = "agrave";
			macEncodingNames[137] = "acircumflex";
			macEncodingNames[138] = "adieresis";
			macEncodingNames[139] = "atilde";
			macEncodingNames[140] = "aring";
			macEncodingNames[141] = "ccedilla";
			macEncodingNames[142] = "eacute";
			macEncodingNames[143] = "egrave";
			macEncodingNames[144] = "ecircumflex";
			macEncodingNames[145] = "edieresis";
			macEncodingNames[146] = "iacute";
			macEncodingNames[147] = "igrave";
			macEncodingNames[148] = "icircumflex";
			macEncodingNames[149] = "idieresis";
			macEncodingNames[150] = "ntilde";
			macEncodingNames[151] = "oacute";
			macEncodingNames[152] = "ograve";
			macEncodingNames[153] = "ocircumflex";
			macEncodingNames[154] = "odieresis";
			macEncodingNames[155] = "otilde";
			macEncodingNames[156] = "uacute";
			macEncodingNames[157] = "ugrave";
			macEncodingNames[158] = "ucircumflex";
			macEncodingNames[159] = "udieresis";
			macEncodingNames[160] = "dagger";
			macEncodingNames[161] = "degree";
			macEncodingNames[162] = "cent";
			macEncodingNames[163] = "sterling";
			macEncodingNames[164] = "section";
			macEncodingNames[165] = "bullet";
			macEncodingNames[166] = "paragraph";
			macEncodingNames[167] = "germandbls";
			macEncodingNames[168] = "registered";
			macEncodingNames[169] = "copyright";
			macEncodingNames[170] = "trademark";
			macEncodingNames[171] = "acute";
			macEncodingNames[172] = "dieresis";
			macEncodingNames[174] = "AE";
			macEncodingNames[175] = "Oslash";
			macEncodingNames[177] = "plusminus";
			macEncodingNames[180] = "yen";
			macEncodingNames[181] = "mu";
			macEncodingNames[187] = "ordfeminine";
			macEncodingNames[188] = "ordmasculine";
			macEncodingNames[190] = "ae";
			macEncodingNames[191] = "oslash";
			macEncodingNames[192] = "questiondown";
			macEncodingNames[193] = "exclamdown";
			macEncodingNames[194] = "logicalnot";
			macEncodingNames[196] = "florin";
			macEncodingNames[199] = "guillemotleft";
			macEncodingNames[200] = "guillemotright";
			macEncodingNames[201] = "ellipsis";
			macEncodingNames[203] = "Agrave";
			macEncodingNames[204] = "Atilde";
			macEncodingNames[205] = "Otilde";
			macEncodingNames[206] = "OE";
			macEncodingNames[207] = "oe";
			macEncodingNames[208] = "endash";
			macEncodingNames[209] = "emdash";
			macEncodingNames[210] = "quotedblleft";
			macEncodingNames[211] = "quotedblright";
			macEncodingNames[212] = "quoteleft";
			macEncodingNames[213] = "quoteright";
			macEncodingNames[214] = "divide";
			macEncodingNames[216] = "ydieresis";
			macEncodingNames[217] = "Ydieresis";
			macEncodingNames[218] = "fraction";
			macEncodingNames[219] = "currency";
			macEncodingNames[220] = "guilsinglleft";
			macEncodingNames[221] = "guilsinglright";
			macEncodingNames[222] = "fi";
			macEncodingNames[223] = "fl";
			macEncodingNames[225] = "middot";
			macEncodingNames[226] = "quotesinglbase";
			macEncodingNames[227] = "quotedblbase";
			macEncodingNames[228] = "perthousand";
			macEncodingNames[229] = "Acircumflex";
			macEncodingNames[230] = "Ecircumflex";
			macEncodingNames[231] = "Aacute";
			macEncodingNames[232] = "Edieresis";
			macEncodingNames[233] = "Egrave";
			macEncodingNames[234] = "Iacute";
			macEncodingNames[235] = "Icircumflex";
			macEncodingNames[236] = "Idieresis";
			macEncodingNames[237] = "Igrave";
			macEncodingNames[238] = "Oacute";
			macEncodingNames[239] = "Ocircumflex";
			macEncodingNames[241] = "Ntilde";
			macEncodingNames[242] = "Uacute";
			macEncodingNames[243] = "Ucircumflex";
			macEncodingNames[244] = "Ugrave";
			macEncodingNames[245] = "dotlessi";
			macEncodingNames[246] = "circumflex";
			macEncodingNames[247] = "ilde";
			macEncodingNames[248] = "macron";
			macEncodingNames[249] = "breve";
			macEncodingNames[250] = "dotaccent";
			macEncodingNames[251] = "ring";
			macEncodingNames[252] = "cedilla";
			macEncodingNames[253] = "hungarumlaut";
			macEncodingNames[254] = "ogonek";
			macEncodingNames[255] = "caron";
			macEncodingNames[32] = "space";
			macEncodingNames[33] = "exclam";
			macEncodingNames[34] = "quotedbl";
			macEncodingNames[35] = "numbersign";
			macEncodingNames[36] = "dollar";
			macEncodingNames[37] = "percent";
			macEncodingNames[38] = "ampersand";
			macEncodingNames[39] = "quotesingle";
			macEncodingNames[40] = "parenleft";
			macEncodingNames[41] = "parenright";
			macEncodingNames[42] = "asterisk";
			macEncodingNames[43] = "plus";
			macEncodingNames[44] = "comma";
			macEncodingNames[45] = "hyphen";
			macEncodingNames[46] = "period";
			macEncodingNames[47] = "slash";
			macEncodingNames[48] = "zero";
			macEncodingNames[49] = "one";
			macEncodingNames[50] = "two";
			macEncodingNames[51] = "three";
			macEncodingNames[52] = "four";
			macEncodingNames[53] = "five";
			macEncodingNames[54] = "six";
			macEncodingNames[55] = "seven";
			macEncodingNames[56] = "eight";
			macEncodingNames[57] = "nine";
			macEncodingNames[58] = "colon";
			macEncodingNames[59] = "semicolon";
			macEncodingNames[60] = "less";
			macEncodingNames[61] = "equal";
			macEncodingNames[62] = "greater";
			macEncodingNames[63] = "question";
			macEncodingNames[64] = "at";
			macEncodingNames[65] = "A";
			macEncodingNames[66] = "B";
			macEncodingNames[67] = "C";
			macEncodingNames[68] = "D";
			macEncodingNames[69] = "E";
			macEncodingNames[70] = "F";
			macEncodingNames[71] = "G";
			macEncodingNames[72] = "H";
			macEncodingNames[73] = "I";
			macEncodingNames[74] = "J";
			macEncodingNames[75] = "K";
			macEncodingNames[76] = "L";
			macEncodingNames[77] = "M";
			macEncodingNames[78] = "N";
			macEncodingNames[79] = "O";
			macEncodingNames[80] = "P";
			macEncodingNames[81] = "Q";
			macEncodingNames[82] = "R";
			macEncodingNames[83] = "S";
			macEncodingNames[84] = "T";
			macEncodingNames[85] = "U";
			macEncodingNames[86] = "V";
			macEncodingNames[87] = "W";
			macEncodingNames[88] = "X";
			macEncodingNames[89] = "Y";
			macEncodingNames[90] = "Z";
			macEncodingNames[91] = "bracketleft";
			macEncodingNames[92] = "backslash";
			macEncodingNames[93] = "bracketright";
			macEncodingNames[94] = "asciicircum";
			macEncodingNames[95] = "underscore";
			macEncodingNames[96] = "grave";
			macEncodingNames[97] = "a";
			macEncodingNames[98] = "b";
			macEncodingNames[99] = "c";
		}

		static void InitializeStandardMacEncoding(string[] standardMacEncodingNames)
		{
			standardMacEncodingNames[224] = "adblgrave";
			standardMacEncodingNames[100] = "d";
			standardMacEncodingNames[101] = "e";
			standardMacEncodingNames[102] = "f";
			standardMacEncodingNames[103] = "g";
			standardMacEncodingNames[104] = "h";
			standardMacEncodingNames[105] = "i";
			standardMacEncodingNames[106] = "j";
			standardMacEncodingNames[107] = "k";
			standardMacEncodingNames[108] = "l";
			standardMacEncodingNames[109] = "m";
			standardMacEncodingNames[110] = "n";
			standardMacEncodingNames[111] = "o";
			standardMacEncodingNames[112] = "p";
			standardMacEncodingNames[113] = "q";
			standardMacEncodingNames[114] = "r";
			standardMacEncodingNames[115] = "s";
			standardMacEncodingNames[116] = "t";
			standardMacEncodingNames[117] = "u";
			standardMacEncodingNames[118] = "v";
			standardMacEncodingNames[119] = "w";
			standardMacEncodingNames[120] = "x";
			standardMacEncodingNames[121] = "y";
			standardMacEncodingNames[122] = "z";
			standardMacEncodingNames[123] = "braceleft";
			standardMacEncodingNames[124] = "bar";
			standardMacEncodingNames[125] = "braceright";
			standardMacEncodingNames[126] = "asciitilde";
			standardMacEncodingNames[128] = "Adieresis";
			standardMacEncodingNames[129] = "Aring";
			standardMacEncodingNames[130] = "Ccedilla";
			standardMacEncodingNames[131] = "Eacute";
			standardMacEncodingNames[132] = "Ograve";
			standardMacEncodingNames[133] = "Odieresis";
			standardMacEncodingNames[134] = "Udieresis";
			standardMacEncodingNames[135] = "aacute";
			standardMacEncodingNames[137] = "acircumflex";
			standardMacEncodingNames[137] = "agrave";
			standardMacEncodingNames[138] = "adieresis";
			standardMacEncodingNames[139] = "atilde";
			standardMacEncodingNames[140] = "aring";
			standardMacEncodingNames[141] = "ccedilla";
			standardMacEncodingNames[142] = "eacute";
			standardMacEncodingNames[143] = "egrave";
			standardMacEncodingNames[144] = "ecircumflex";
			standardMacEncodingNames[145] = "edieresis";
			standardMacEncodingNames[146] = "iacute";
			standardMacEncodingNames[147] = "igrave";
			standardMacEncodingNames[148] = "icircumflex";
			standardMacEncodingNames[149] = "idieresis";
			standardMacEncodingNames[150] = "ntilde";
			standardMacEncodingNames[151] = "oacute";
			standardMacEncodingNames[152] = "ograve";
			standardMacEncodingNames[153] = "ocircumflex";
			standardMacEncodingNames[154] = "odieresis";
			standardMacEncodingNames[155] = "otilde";
			standardMacEncodingNames[156] = "uacute";
			standardMacEncodingNames[157] = "ugrave";
			standardMacEncodingNames[158] = "ucircumflex";
			standardMacEncodingNames[159] = "udieresis";
			standardMacEncodingNames[160] = "dagger";
			standardMacEncodingNames[161] = "degree";
			standardMacEncodingNames[162] = "cent";
			standardMacEncodingNames[163] = "sterling";
			standardMacEncodingNames[164] = "section";
			standardMacEncodingNames[165] = "bullet";
			standardMacEncodingNames[166] = "paragraph";
			standardMacEncodingNames[167] = "germandbls";
			standardMacEncodingNames[168] = "registered";
			standardMacEncodingNames[169] = "copyright";
			standardMacEncodingNames[170] = "trademark";
			standardMacEncodingNames[171] = "acute";
			standardMacEncodingNames[172] = "dieresis";
			standardMacEncodingNames[174] = "AE";
			standardMacEncodingNames[175] = "Oslash";
			standardMacEncodingNames[177] = "plusminus";
			standardMacEncodingNames[180] = "yen";
			standardMacEncodingNames[181] = "mu";
			standardMacEncodingNames[187] = "ordfeminine";
			standardMacEncodingNames[188] = "ordmasculine";
			standardMacEncodingNames[190] = "ae";
			standardMacEncodingNames[191] = "oslash";
			standardMacEncodingNames[192] = "questiondown";
			standardMacEncodingNames[193] = "exclamdown";
			standardMacEncodingNames[194] = "logicalnot";
			standardMacEncodingNames[196] = "florin";
			standardMacEncodingNames[199] = "guillemotleft";
			standardMacEncodingNames[200] = "guillemotright";
			standardMacEncodingNames[201] = "ellipsis";
			standardMacEncodingNames[203] = "Agrave";
			standardMacEncodingNames[204] = "Atilde";
			standardMacEncodingNames[205] = "Otilde";
			standardMacEncodingNames[206] = "OE";
			standardMacEncodingNames[207] = "oe";
			standardMacEncodingNames[208] = "endash";
			standardMacEncodingNames[209] = "emdash";
			standardMacEncodingNames[210] = "quotedblleft";
			standardMacEncodingNames[211] = "quotedblright";
			standardMacEncodingNames[212] = "quoteleft";
			standardMacEncodingNames[213] = "quoteright";
			standardMacEncodingNames[214] = "divide";
			standardMacEncodingNames[216] = "ydieresis";
			standardMacEncodingNames[217] = "Ydieresis";
			standardMacEncodingNames[218] = "fraction";
			standardMacEncodingNames[220] = "guilsinglleft";
			standardMacEncodingNames[221] = "guilsinglright";
			standardMacEncodingNames[222] = "fi";
			standardMacEncodingNames[223] = "fl";
			standardMacEncodingNames[225] = "middot";
			standardMacEncodingNames[226] = "quotesinglbase";
			standardMacEncodingNames[227] = "quotedblbase";
			standardMacEncodingNames[228] = "perthousand";
			standardMacEncodingNames[229] = "Acircumflex";
			standardMacEncodingNames[230] = "Ecircumflex";
			standardMacEncodingNames[231] = "Aacute";
			standardMacEncodingNames[232] = "Edieresis";
			standardMacEncodingNames[233] = "Egrave";
			standardMacEncodingNames[234] = "Iacute";
			standardMacEncodingNames[235] = "Icircumflex";
			standardMacEncodingNames[236] = "Idieresis";
			standardMacEncodingNames[237] = "Igrave";
			standardMacEncodingNames[238] = "Oacute";
			standardMacEncodingNames[239] = "Ocircumflex";
			standardMacEncodingNames[241] = "Ntilde";
			standardMacEncodingNames[242] = "Uacute";
			standardMacEncodingNames[243] = "Ucircumflex";
			standardMacEncodingNames[244] = "Ugrave";
			standardMacEncodingNames[245] = "dotlessi";
			standardMacEncodingNames[246] = "circumflex";
			standardMacEncodingNames[247] = "ilde";
			standardMacEncodingNames[248] = "macron";
			standardMacEncodingNames[249] = "breve";
			standardMacEncodingNames[250] = "dotaccent";
			standardMacEncodingNames[251] = "ring";
			standardMacEncodingNames[252] = "cedilla";
			standardMacEncodingNames[253] = "hungarumlaut";
			standardMacEncodingNames[254] = "ogonek";
			standardMacEncodingNames[255] = "caron";
			standardMacEncodingNames[32] = "space";
			standardMacEncodingNames[33] = "exclam";
			standardMacEncodingNames[34] = "quotedbl";
			standardMacEncodingNames[35] = "numbersign";
			standardMacEncodingNames[36] = "dollar";
			standardMacEncodingNames[37] = "percent";
			standardMacEncodingNames[38] = "ampersand";
			standardMacEncodingNames[39] = "quotesingle";
			standardMacEncodingNames[40] = "parenleft";
			standardMacEncodingNames[41] = "parenright";
			standardMacEncodingNames[42] = "asterisk";
			standardMacEncodingNames[43] = "plus";
			standardMacEncodingNames[44] = "comma";
			standardMacEncodingNames[45] = "hyphen";
			standardMacEncodingNames[46] = "period";
			standardMacEncodingNames[47] = "slash";
			standardMacEncodingNames[48] = "zero";
			standardMacEncodingNames[49] = "one";
			standardMacEncodingNames[50] = "two";
			standardMacEncodingNames[51] = "three";
			standardMacEncodingNames[52] = "four";
			standardMacEncodingNames[53] = "five";
			standardMacEncodingNames[54] = "six";
			standardMacEncodingNames[55] = "seven";
			standardMacEncodingNames[56] = "eight";
			standardMacEncodingNames[57] = "nine";
			standardMacEncodingNames[58] = "colon";
			standardMacEncodingNames[59] = "semicolon";
			standardMacEncodingNames[60] = "less";
			standardMacEncodingNames[61] = "equal";
			standardMacEncodingNames[62] = "greater";
			standardMacEncodingNames[63] = "question";
			standardMacEncodingNames[64] = "at";
			standardMacEncodingNames[65] = "A";
			standardMacEncodingNames[66] = "B";
			standardMacEncodingNames[67] = "C";
			standardMacEncodingNames[68] = "D";
			standardMacEncodingNames[69] = "E";
			standardMacEncodingNames[70] = "F";
			standardMacEncodingNames[71] = "G";
			standardMacEncodingNames[72] = "H";
			standardMacEncodingNames[73] = "I";
			standardMacEncodingNames[74] = "J";
			standardMacEncodingNames[75] = "K";
			standardMacEncodingNames[76] = "L";
			standardMacEncodingNames[77] = "M";
			standardMacEncodingNames[78] = "N";
			standardMacEncodingNames[79] = "O";
			standardMacEncodingNames[80] = "P";
			standardMacEncodingNames[81] = "Q";
			standardMacEncodingNames[82] = "R";
			standardMacEncodingNames[83] = "S";
			standardMacEncodingNames[84] = "T";
			standardMacEncodingNames[85] = "U";
			standardMacEncodingNames[86] = "V";
			standardMacEncodingNames[87] = "W";
			standardMacEncodingNames[88] = "X";
			standardMacEncodingNames[89] = "Y";
			standardMacEncodingNames[90] = "Z";
			standardMacEncodingNames[91] = "bracketleft";
			standardMacEncodingNames[92] = "backslash";
			standardMacEncodingNames[93] = "bracketright";
			standardMacEncodingNames[94] = "asciicircum";
			standardMacEncodingNames[95] = "underscore";
			standardMacEncodingNames[96] = "grave";
			standardMacEncodingNames[97] = "a";
			standardMacEncodingNames[98] = "b";
			standardMacEncodingNames[99] = "c";
			standardMacEncodingNames[173] = "notequal";
			standardMacEncodingNames[176] = "infinity";
			standardMacEncodingNames[178] = "lessequal";
			standardMacEncodingNames[179] = "greaterequal";
			standardMacEncodingNames[182] = "partialdiff";
			standardMacEncodingNames[183] = "summation";
			standardMacEncodingNames[184] = "product";
			standardMacEncodingNames[185] = "pi";
			standardMacEncodingNames[186] = "integral";
			standardMacEncodingNames[189] = "Omega";
			standardMacEncodingNames[195] = "radical";
			standardMacEncodingNames[197] = "approxequal";
			standardMacEncodingNames[198] = "Delta";
			standardMacEncodingNames[215] = "lozenge";
			standardMacEncodingNames[219] = "Euro";
			standardMacEncodingNames[240] = "apple";
		}

		static void InitializeStandardEncoding(string[] standardEncodingNames)
		{
			for (int i = 0; i < standardEncodingNames.Length; i++)
			{
				string text = PredefinedEncodings.StandardEncoding[i];
				if (text != null)
				{
					standardEncodingNames[i] = text;
				}
			}
		}

		public const string PdfDocEncodingName = "PdfDocEncoding";

		public const string WinAnsiEncodingName = "WinAnsiEncoding";

		public const string MacRomanEncodingName = "MacRomanEncoding";

		public const string StandardEncodingName = "StandardEncoding";

		public static readonly PredefinedSimpleEncoding PdfDocEncoding = new PredefinedSimpleEncoding("PdfDocEncoding", new Action<string[]>(PredefinedSimpleEncoding.InitializePdfEncoding));

		public static readonly PredefinedSimpleEncoding WinAnsiEncoding = new PredefinedSimpleEncoding("WinAnsiEncoding", new Action<string[]>(PredefinedSimpleEncoding.InitializeWinAnsiEncoding));

		public static readonly PredefinedSimpleEncoding MacRomanEncoding = new PredefinedSimpleEncoding("MacRomanEncoding", new Action<string[]>(PredefinedSimpleEncoding.InitializeMacEncoding));

		public static readonly PredefinedSimpleEncoding StandardMacRomanEncoding = new PredefinedSimpleEncoding("MacRomanEncoding", new Action<string[]>(PredefinedSimpleEncoding.InitializeStandardMacEncoding));

		public static readonly PredefinedSimpleEncoding StandardEncoding = new PredefinedSimpleEncoding("StandardEncoding", new Action<string[]>(PredefinedSimpleEncoding.InitializeStandardEncoding));

		readonly string encodingName;

		readonly string[] names;

		readonly Dictionary<string, byte> mapping;
	}
}
