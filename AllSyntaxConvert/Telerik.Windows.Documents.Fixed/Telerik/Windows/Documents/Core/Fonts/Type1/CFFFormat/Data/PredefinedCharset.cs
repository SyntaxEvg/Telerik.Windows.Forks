using System;
using System.Linq;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.Data
{
	class PredefinedCharset
	{
		public static ushort[] GetPredefinedCodes(int b)
		{
			switch (b)
			{
			case 0:
				return PredefinedCharset.ISOAdobeCharset.GetCodes();
			case 1:
				return PredefinedCharset.ExpertCharset.GetCodes();
			case 2:
				return PredefinedCharset.ExpertSubsetCharset.GetCodes();
			default:
				return null;
			}
		}

		public static bool IsPredefinedCharset(int b)
		{
			return b == 0 || b == 1 || b == 2;
		}

		static void InitializeExpertSubsetCharset()
		{
			PredefinedCharset.ExpertSubsetCharset = new PredefinedCharset();
			PredefinedCharset.ExpertSubsetCharset.codes = new ushort[86];
			PredefinedCharset.ExpertSubsetCharset.codes[0] = 1;
			PredefinedCharset.ExpertSubsetCharset.codes[1] = 231;
			PredefinedCharset.ExpertSubsetCharset.codes[2] = 232;
			PredefinedCharset.ExpertSubsetCharset.codes[3] = 235;
			PredefinedCharset.ExpertSubsetCharset.codes[4] = 236;
			PredefinedCharset.ExpertSubsetCharset.codes[5] = 237;
			PredefinedCharset.ExpertSubsetCharset.codes[6] = 238;
			PredefinedCharset.ExpertSubsetCharset.codes[7] = 13;
			PredefinedCharset.ExpertSubsetCharset.codes[8] = 14;
			PredefinedCharset.ExpertSubsetCharset.codes[9] = 15;
			PredefinedCharset.ExpertSubsetCharset.codes[10] = 99;
			PredefinedCharset.ExpertSubsetCharset.codes[11] = 239;
			PredefinedCharset.ExpertSubsetCharset.codes[12] = 240;
			PredefinedCharset.ExpertSubsetCharset.codes[13] = 241;
			PredefinedCharset.ExpertSubsetCharset.codes[14] = 242;
			PredefinedCharset.ExpertSubsetCharset.codes[15] = 243;
			PredefinedCharset.ExpertSubsetCharset.codes[16] = 244;
			PredefinedCharset.ExpertSubsetCharset.codes[17] = 245;
			PredefinedCharset.ExpertSubsetCharset.codes[18] = 246;
			PredefinedCharset.ExpertSubsetCharset.codes[19] = 247;
			PredefinedCharset.ExpertSubsetCharset.codes[20] = 248;
			PredefinedCharset.ExpertSubsetCharset.codes[21] = 27;
			PredefinedCharset.ExpertSubsetCharset.codes[22] = 28;
			PredefinedCharset.ExpertSubsetCharset.codes[23] = 249;
			PredefinedCharset.ExpertSubsetCharset.codes[24] = 250;
			PredefinedCharset.ExpertSubsetCharset.codes[25] = 251;
			PredefinedCharset.ExpertSubsetCharset.codes[26] = 253;
			PredefinedCharset.ExpertSubsetCharset.codes[27] = 254;
			PredefinedCharset.ExpertSubsetCharset.codes[28] = 255;
			PredefinedCharset.ExpertSubsetCharset.codes[29] = 256;
			PredefinedCharset.ExpertSubsetCharset.codes[30] = 257;
			PredefinedCharset.ExpertSubsetCharset.codes[31] = 258;
			PredefinedCharset.ExpertSubsetCharset.codes[32] = 259;
			PredefinedCharset.ExpertSubsetCharset.codes[33] = 260;
			PredefinedCharset.ExpertSubsetCharset.codes[34] = 261;
			PredefinedCharset.ExpertSubsetCharset.codes[35] = 262;
			PredefinedCharset.ExpertSubsetCharset.codes[36] = 263;
			PredefinedCharset.ExpertSubsetCharset.codes[37] = 264;
			PredefinedCharset.ExpertSubsetCharset.codes[38] = 265;
			PredefinedCharset.ExpertSubsetCharset.codes[39] = 266;
			PredefinedCharset.ExpertSubsetCharset.codes[40] = 109;
			PredefinedCharset.ExpertSubsetCharset.codes[41] = 110;
			PredefinedCharset.ExpertSubsetCharset.codes[42] = 267;
			PredefinedCharset.ExpertSubsetCharset.codes[43] = 268;
			PredefinedCharset.ExpertSubsetCharset.codes[44] = 269;
			PredefinedCharset.ExpertSubsetCharset.codes[45] = 270;
			PredefinedCharset.ExpertSubsetCharset.codes[46] = 272;
			PredefinedCharset.ExpertSubsetCharset.codes[47] = 300;
			PredefinedCharset.ExpertSubsetCharset.codes[48] = 301;
			PredefinedCharset.ExpertSubsetCharset.codes[49] = 302;
			PredefinedCharset.ExpertSubsetCharset.codes[50] = 305;
			PredefinedCharset.ExpertSubsetCharset.codes[51] = 314;
			PredefinedCharset.ExpertSubsetCharset.codes[52] = 315;
			PredefinedCharset.ExpertSubsetCharset.codes[53] = 158;
			PredefinedCharset.ExpertSubsetCharset.codes[54] = 155;
			PredefinedCharset.ExpertSubsetCharset.codes[55] = 163;
			PredefinedCharset.ExpertSubsetCharset.codes[56] = 320;
			PredefinedCharset.ExpertSubsetCharset.codes[57] = 321;
			PredefinedCharset.ExpertSubsetCharset.codes[58] = 322;
			PredefinedCharset.ExpertSubsetCharset.codes[59] = 323;
			PredefinedCharset.ExpertSubsetCharset.codes[60] = 324;
			PredefinedCharset.ExpertSubsetCharset.codes[61] = 325;
			PredefinedCharset.ExpertSubsetCharset.codes[62] = 326;
			PredefinedCharset.ExpertSubsetCharset.codes[63] = 150;
			PredefinedCharset.ExpertSubsetCharset.codes[64] = 164;
			PredefinedCharset.ExpertSubsetCharset.codes[65] = 169;
			PredefinedCharset.ExpertSubsetCharset.codes[66] = 327;
			PredefinedCharset.ExpertSubsetCharset.codes[67] = 328;
			PredefinedCharset.ExpertSubsetCharset.codes[68] = 329;
			PredefinedCharset.ExpertSubsetCharset.codes[69] = 330;
			PredefinedCharset.ExpertSubsetCharset.codes[70] = 331;
			PredefinedCharset.ExpertSubsetCharset.codes[71] = 332;
			PredefinedCharset.ExpertSubsetCharset.codes[72] = 333;
			PredefinedCharset.ExpertSubsetCharset.codes[73] = 334;
			PredefinedCharset.ExpertSubsetCharset.codes[74] = 335;
			PredefinedCharset.ExpertSubsetCharset.codes[75] = 336;
			PredefinedCharset.ExpertSubsetCharset.codes[76] = 337;
			PredefinedCharset.ExpertSubsetCharset.codes[77] = 338;
			PredefinedCharset.ExpertSubsetCharset.codes[78] = 339;
			PredefinedCharset.ExpertSubsetCharset.codes[79] = 340;
			PredefinedCharset.ExpertSubsetCharset.codes[80] = 341;
			PredefinedCharset.ExpertSubsetCharset.codes[81] = 342;
			PredefinedCharset.ExpertSubsetCharset.codes[82] = 343;
			PredefinedCharset.ExpertSubsetCharset.codes[83] = 344;
			PredefinedCharset.ExpertSubsetCharset.codes[84] = 345;
			PredefinedCharset.ExpertSubsetCharset.codes[85] = 346;
		}

		static void InitializeExpertCharset()
		{
			PredefinedCharset.ExpertCharset = new PredefinedCharset();
			PredefinedCharset.ExpertCharset.codes = new ushort[165];
			PredefinedCharset.ExpertCharset.codes[0] = 1;
			PredefinedCharset.ExpertCharset.codes[1] = 229;
			PredefinedCharset.ExpertCharset.codes[2] = 230;
			PredefinedCharset.ExpertCharset.codes[3] = 231;
			PredefinedCharset.ExpertCharset.codes[4] = 232;
			PredefinedCharset.ExpertCharset.codes[5] = 233;
			PredefinedCharset.ExpertCharset.codes[6] = 234;
			PredefinedCharset.ExpertCharset.codes[7] = 235;
			PredefinedCharset.ExpertCharset.codes[8] = 236;
			PredefinedCharset.ExpertCharset.codes[9] = 237;
			PredefinedCharset.ExpertCharset.codes[10] = 238;
			PredefinedCharset.ExpertCharset.codes[11] = 13;
			PredefinedCharset.ExpertCharset.codes[12] = 14;
			PredefinedCharset.ExpertCharset.codes[13] = 15;
			PredefinedCharset.ExpertCharset.codes[14] = 99;
			PredefinedCharset.ExpertCharset.codes[15] = 239;
			PredefinedCharset.ExpertCharset.codes[16] = 240;
			PredefinedCharset.ExpertCharset.codes[17] = 241;
			PredefinedCharset.ExpertCharset.codes[18] = 242;
			PredefinedCharset.ExpertCharset.codes[19] = 243;
			PredefinedCharset.ExpertCharset.codes[20] = 244;
			PredefinedCharset.ExpertCharset.codes[21] = 245;
			PredefinedCharset.ExpertCharset.codes[22] = 246;
			PredefinedCharset.ExpertCharset.codes[23] = 247;
			PredefinedCharset.ExpertCharset.codes[24] = 248;
			PredefinedCharset.ExpertCharset.codes[25] = 27;
			PredefinedCharset.ExpertCharset.codes[26] = 28;
			PredefinedCharset.ExpertCharset.codes[27] = 249;
			PredefinedCharset.ExpertCharset.codes[28] = 250;
			PredefinedCharset.ExpertCharset.codes[29] = 251;
			PredefinedCharset.ExpertCharset.codes[30] = 252;
			PredefinedCharset.ExpertCharset.codes[31] = 253;
			PredefinedCharset.ExpertCharset.codes[32] = 254;
			PredefinedCharset.ExpertCharset.codes[33] = 255;
			PredefinedCharset.ExpertCharset.codes[34] = 256;
			PredefinedCharset.ExpertCharset.codes[35] = 257;
			PredefinedCharset.ExpertCharset.codes[36] = 258;
			PredefinedCharset.ExpertCharset.codes[37] = 259;
			PredefinedCharset.ExpertCharset.codes[38] = 260;
			PredefinedCharset.ExpertCharset.codes[39] = 261;
			PredefinedCharset.ExpertCharset.codes[40] = 262;
			PredefinedCharset.ExpertCharset.codes[41] = 263;
			PredefinedCharset.ExpertCharset.codes[42] = 264;
			PredefinedCharset.ExpertCharset.codes[43] = 265;
			PredefinedCharset.ExpertCharset.codes[44] = 266;
			PredefinedCharset.ExpertCharset.codes[45] = 109;
			PredefinedCharset.ExpertCharset.codes[46] = 110;
			PredefinedCharset.ExpertCharset.codes[47] = 267;
			PredefinedCharset.ExpertCharset.codes[48] = 268;
			PredefinedCharset.ExpertCharset.codes[49] = 269;
			PredefinedCharset.ExpertCharset.codes[50] = 270;
			PredefinedCharset.ExpertCharset.codes[51] = 271;
			PredefinedCharset.ExpertCharset.codes[52] = 272;
			PredefinedCharset.ExpertCharset.codes[53] = 273;
			PredefinedCharset.ExpertCharset.codes[54] = 274;
			PredefinedCharset.ExpertCharset.codes[55] = 275;
			PredefinedCharset.ExpertCharset.codes[56] = 276;
			PredefinedCharset.ExpertCharset.codes[57] = 277;
			PredefinedCharset.ExpertCharset.codes[58] = 278;
			PredefinedCharset.ExpertCharset.codes[59] = 279;
			PredefinedCharset.ExpertCharset.codes[60] = 280;
			PredefinedCharset.ExpertCharset.codes[61] = 281;
			PredefinedCharset.ExpertCharset.codes[62] = 282;
			PredefinedCharset.ExpertCharset.codes[63] = 283;
			PredefinedCharset.ExpertCharset.codes[64] = 284;
			PredefinedCharset.ExpertCharset.codes[65] = 285;
			PredefinedCharset.ExpertCharset.codes[66] = 286;
			PredefinedCharset.ExpertCharset.codes[67] = 287;
			PredefinedCharset.ExpertCharset.codes[68] = 288;
			PredefinedCharset.ExpertCharset.codes[69] = 289;
			PredefinedCharset.ExpertCharset.codes[70] = 290;
			PredefinedCharset.ExpertCharset.codes[71] = 291;
			PredefinedCharset.ExpertCharset.codes[72] = 292;
			PredefinedCharset.ExpertCharset.codes[73] = 293;
			PredefinedCharset.ExpertCharset.codes[74] = 294;
			PredefinedCharset.ExpertCharset.codes[75] = 295;
			PredefinedCharset.ExpertCharset.codes[76] = 296;
			PredefinedCharset.ExpertCharset.codes[77] = 297;
			PredefinedCharset.ExpertCharset.codes[78] = 298;
			PredefinedCharset.ExpertCharset.codes[79] = 299;
			PredefinedCharset.ExpertCharset.codes[80] = 300;
			PredefinedCharset.ExpertCharset.codes[81] = 301;
			PredefinedCharset.ExpertCharset.codes[82] = 302;
			PredefinedCharset.ExpertCharset.codes[83] = 303;
			PredefinedCharset.ExpertCharset.codes[84] = 304;
			PredefinedCharset.ExpertCharset.codes[85] = 305;
			PredefinedCharset.ExpertCharset.codes[86] = 306;
			PredefinedCharset.ExpertCharset.codes[87] = 307;
			PredefinedCharset.ExpertCharset.codes[88] = 308;
			PredefinedCharset.ExpertCharset.codes[89] = 309;
			PredefinedCharset.ExpertCharset.codes[90] = 310;
			PredefinedCharset.ExpertCharset.codes[91] = 311;
			PredefinedCharset.ExpertCharset.codes[92] = 312;
			PredefinedCharset.ExpertCharset.codes[93] = 313;
			PredefinedCharset.ExpertCharset.codes[94] = 314;
			PredefinedCharset.ExpertCharset.codes[95] = 315;
			PredefinedCharset.ExpertCharset.codes[96] = 316;
			PredefinedCharset.ExpertCharset.codes[97] = 317;
			PredefinedCharset.ExpertCharset.codes[98] = 318;
			PredefinedCharset.ExpertCharset.codes[99] = 158;
			PredefinedCharset.ExpertCharset.codes[100] = 155;
			PredefinedCharset.ExpertCharset.codes[101] = 163;
			PredefinedCharset.ExpertCharset.codes[102] = 319;
			PredefinedCharset.ExpertCharset.codes[103] = 320;
			PredefinedCharset.ExpertCharset.codes[104] = 321;
			PredefinedCharset.ExpertCharset.codes[105] = 322;
			PredefinedCharset.ExpertCharset.codes[106] = 323;
			PredefinedCharset.ExpertCharset.codes[107] = 324;
			PredefinedCharset.ExpertCharset.codes[108] = 325;
			PredefinedCharset.ExpertCharset.codes[109] = 326;
			PredefinedCharset.ExpertCharset.codes[110] = 150;
			PredefinedCharset.ExpertCharset.codes[111] = 164;
			PredefinedCharset.ExpertCharset.codes[112] = 169;
			PredefinedCharset.ExpertCharset.codes[113] = 327;
			PredefinedCharset.ExpertCharset.codes[114] = 328;
			PredefinedCharset.ExpertCharset.codes[115] = 329;
			PredefinedCharset.ExpertCharset.codes[116] = 330;
			PredefinedCharset.ExpertCharset.codes[117] = 331;
			PredefinedCharset.ExpertCharset.codes[118] = 332;
			PredefinedCharset.ExpertCharset.codes[119] = 333;
			PredefinedCharset.ExpertCharset.codes[120] = 334;
			PredefinedCharset.ExpertCharset.codes[121] = 335;
			PredefinedCharset.ExpertCharset.codes[122] = 336;
			PredefinedCharset.ExpertCharset.codes[123] = 337;
			PredefinedCharset.ExpertCharset.codes[124] = 338;
			PredefinedCharset.ExpertCharset.codes[125] = 339;
			PredefinedCharset.ExpertCharset.codes[126] = 340;
			PredefinedCharset.ExpertCharset.codes[127] = 341;
			PredefinedCharset.ExpertCharset.codes[128] = 342;
			PredefinedCharset.ExpertCharset.codes[129] = 343;
			PredefinedCharset.ExpertCharset.codes[130] = 344;
			PredefinedCharset.ExpertCharset.codes[131] = 345;
			PredefinedCharset.ExpertCharset.codes[132] = 346;
			PredefinedCharset.ExpertCharset.codes[133] = 347;
			PredefinedCharset.ExpertCharset.codes[134] = 348;
			PredefinedCharset.ExpertCharset.codes[135] = 349;
			PredefinedCharset.ExpertCharset.codes[136] = 350;
			PredefinedCharset.ExpertCharset.codes[137] = 351;
			PredefinedCharset.ExpertCharset.codes[138] = 352;
			PredefinedCharset.ExpertCharset.codes[139] = 353;
			PredefinedCharset.ExpertCharset.codes[140] = 354;
			PredefinedCharset.ExpertCharset.codes[141] = 355;
			PredefinedCharset.ExpertCharset.codes[142] = 356;
			PredefinedCharset.ExpertCharset.codes[143] = 357;
			PredefinedCharset.ExpertCharset.codes[144] = 358;
			PredefinedCharset.ExpertCharset.codes[145] = 359;
			PredefinedCharset.ExpertCharset.codes[146] = 360;
			PredefinedCharset.ExpertCharset.codes[147] = 361;
			PredefinedCharset.ExpertCharset.codes[148] = 362;
			PredefinedCharset.ExpertCharset.codes[149] = 363;
			PredefinedCharset.ExpertCharset.codes[150] = 364;
			PredefinedCharset.ExpertCharset.codes[151] = 365;
			PredefinedCharset.ExpertCharset.codes[152] = 366;
			PredefinedCharset.ExpertCharset.codes[153] = 367;
			PredefinedCharset.ExpertCharset.codes[154] = 368;
			PredefinedCharset.ExpertCharset.codes[155] = 369;
			PredefinedCharset.ExpertCharset.codes[156] = 370;
			PredefinedCharset.ExpertCharset.codes[157] = 371;
			PredefinedCharset.ExpertCharset.codes[158] = 372;
			PredefinedCharset.ExpertCharset.codes[159] = 373;
			PredefinedCharset.ExpertCharset.codes[160] = 374;
			PredefinedCharset.ExpertCharset.codes[161] = 375;
			PredefinedCharset.ExpertCharset.codes[162] = 376;
			PredefinedCharset.ExpertCharset.codes[163] = 377;
			PredefinedCharset.ExpertCharset.codes[164] = 378;
		}

		static void InitializeISOAdobeCharset()
		{
			PredefinedCharset.ISOAdobeCharset = new PredefinedCharset();
			PredefinedCharset.ISOAdobeCharset.codes = new ushort[229];
			PredefinedCharset.ISOAdobeCharset.codes[0] = 1;
			PredefinedCharset.ISOAdobeCharset.codes[1] = 2;
			PredefinedCharset.ISOAdobeCharset.codes[2] = 3;
			PredefinedCharset.ISOAdobeCharset.codes[3] = 4;
			PredefinedCharset.ISOAdobeCharset.codes[4] = 5;
			PredefinedCharset.ISOAdobeCharset.codes[5] = 6;
			PredefinedCharset.ISOAdobeCharset.codes[6] = 7;
			PredefinedCharset.ISOAdobeCharset.codes[7] = 8;
			PredefinedCharset.ISOAdobeCharset.codes[8] = 9;
			PredefinedCharset.ISOAdobeCharset.codes[9] = 10;
			PredefinedCharset.ISOAdobeCharset.codes[10] = 11;
			PredefinedCharset.ISOAdobeCharset.codes[11] = 12;
			PredefinedCharset.ISOAdobeCharset.codes[12] = 13;
			PredefinedCharset.ISOAdobeCharset.codes[13] = 14;
			PredefinedCharset.ISOAdobeCharset.codes[14] = 15;
			PredefinedCharset.ISOAdobeCharset.codes[15] = 16;
			PredefinedCharset.ISOAdobeCharset.codes[16] = 17;
			PredefinedCharset.ISOAdobeCharset.codes[17] = 18;
			PredefinedCharset.ISOAdobeCharset.codes[18] = 19;
			PredefinedCharset.ISOAdobeCharset.codes[19] = 20;
			PredefinedCharset.ISOAdobeCharset.codes[20] = 21;
			PredefinedCharset.ISOAdobeCharset.codes[21] = 22;
			PredefinedCharset.ISOAdobeCharset.codes[22] = 23;
			PredefinedCharset.ISOAdobeCharset.codes[23] = 24;
			PredefinedCharset.ISOAdobeCharset.codes[24] = 25;
			PredefinedCharset.ISOAdobeCharset.codes[25] = 26;
			PredefinedCharset.ISOAdobeCharset.codes[26] = 27;
			PredefinedCharset.ISOAdobeCharset.codes[27] = 28;
			PredefinedCharset.ISOAdobeCharset.codes[28] = 29;
			PredefinedCharset.ISOAdobeCharset.codes[29] = 30;
			PredefinedCharset.ISOAdobeCharset.codes[30] = 31;
			PredefinedCharset.ISOAdobeCharset.codes[31] = 32;
			PredefinedCharset.ISOAdobeCharset.codes[32] = 33;
			PredefinedCharset.ISOAdobeCharset.codes[33] = 34;
			PredefinedCharset.ISOAdobeCharset.codes[34] = 35;
			PredefinedCharset.ISOAdobeCharset.codes[35] = 36;
			PredefinedCharset.ISOAdobeCharset.codes[36] = 37;
			PredefinedCharset.ISOAdobeCharset.codes[37] = 38;
			PredefinedCharset.ISOAdobeCharset.codes[38] = 39;
			PredefinedCharset.ISOAdobeCharset.codes[39] = 40;
			PredefinedCharset.ISOAdobeCharset.codes[40] = 41;
			PredefinedCharset.ISOAdobeCharset.codes[41] = 42;
			PredefinedCharset.ISOAdobeCharset.codes[42] = 43;
			PredefinedCharset.ISOAdobeCharset.codes[43] = 44;
			PredefinedCharset.ISOAdobeCharset.codes[44] = 45;
			PredefinedCharset.ISOAdobeCharset.codes[45] = 46;
			PredefinedCharset.ISOAdobeCharset.codes[46] = 47;
			PredefinedCharset.ISOAdobeCharset.codes[47] = 48;
			PredefinedCharset.ISOAdobeCharset.codes[48] = 49;
			PredefinedCharset.ISOAdobeCharset.codes[49] = 50;
			PredefinedCharset.ISOAdobeCharset.codes[50] = 51;
			PredefinedCharset.ISOAdobeCharset.codes[51] = 52;
			PredefinedCharset.ISOAdobeCharset.codes[52] = 53;
			PredefinedCharset.ISOAdobeCharset.codes[53] = 54;
			PredefinedCharset.ISOAdobeCharset.codes[54] = 55;
			PredefinedCharset.ISOAdobeCharset.codes[55] = 56;
			PredefinedCharset.ISOAdobeCharset.codes[56] = 57;
			PredefinedCharset.ISOAdobeCharset.codes[57] = 58;
			PredefinedCharset.ISOAdobeCharset.codes[58] = 59;
			PredefinedCharset.ISOAdobeCharset.codes[59] = 60;
			PredefinedCharset.ISOAdobeCharset.codes[60] = 61;
			PredefinedCharset.ISOAdobeCharset.codes[61] = 62;
			PredefinedCharset.ISOAdobeCharset.codes[62] = 63;
			PredefinedCharset.ISOAdobeCharset.codes[63] = 64;
			PredefinedCharset.ISOAdobeCharset.codes[64] = 65;
			PredefinedCharset.ISOAdobeCharset.codes[65] = 66;
			PredefinedCharset.ISOAdobeCharset.codes[66] = 67;
			PredefinedCharset.ISOAdobeCharset.codes[67] = 68;
			PredefinedCharset.ISOAdobeCharset.codes[68] = 69;
			PredefinedCharset.ISOAdobeCharset.codes[69] = 70;
			PredefinedCharset.ISOAdobeCharset.codes[70] = 71;
			PredefinedCharset.ISOAdobeCharset.codes[71] = 72;
			PredefinedCharset.ISOAdobeCharset.codes[72] = 73;
			PredefinedCharset.ISOAdobeCharset.codes[73] = 74;
			PredefinedCharset.ISOAdobeCharset.codes[74] = 75;
			PredefinedCharset.ISOAdobeCharset.codes[75] = 76;
			PredefinedCharset.ISOAdobeCharset.codes[76] = 77;
			PredefinedCharset.ISOAdobeCharset.codes[77] = 78;
			PredefinedCharset.ISOAdobeCharset.codes[78] = 79;
			PredefinedCharset.ISOAdobeCharset.codes[79] = 80;
			PredefinedCharset.ISOAdobeCharset.codes[80] = 81;
			PredefinedCharset.ISOAdobeCharset.codes[81] = 82;
			PredefinedCharset.ISOAdobeCharset.codes[82] = 83;
			PredefinedCharset.ISOAdobeCharset.codes[83] = 84;
			PredefinedCharset.ISOAdobeCharset.codes[84] = 85;
			PredefinedCharset.ISOAdobeCharset.codes[85] = 86;
			PredefinedCharset.ISOAdobeCharset.codes[86] = 87;
			PredefinedCharset.ISOAdobeCharset.codes[87] = 88;
			PredefinedCharset.ISOAdobeCharset.codes[88] = 89;
			PredefinedCharset.ISOAdobeCharset.codes[89] = 90;
			PredefinedCharset.ISOAdobeCharset.codes[90] = 91;
			PredefinedCharset.ISOAdobeCharset.codes[91] = 92;
			PredefinedCharset.ISOAdobeCharset.codes[92] = 93;
			PredefinedCharset.ISOAdobeCharset.codes[93] = 94;
			PredefinedCharset.ISOAdobeCharset.codes[94] = 95;
			PredefinedCharset.ISOAdobeCharset.codes[95] = 96;
			PredefinedCharset.ISOAdobeCharset.codes[96] = 97;
			PredefinedCharset.ISOAdobeCharset.codes[97] = 98;
			PredefinedCharset.ISOAdobeCharset.codes[98] = 99;
			PredefinedCharset.ISOAdobeCharset.codes[99] = 100;
			PredefinedCharset.ISOAdobeCharset.codes[100] = 101;
			PredefinedCharset.ISOAdobeCharset.codes[101] = 102;
			PredefinedCharset.ISOAdobeCharset.codes[102] = 103;
			PredefinedCharset.ISOAdobeCharset.codes[103] = 104;
			PredefinedCharset.ISOAdobeCharset.codes[104] = 105;
			PredefinedCharset.ISOAdobeCharset.codes[105] = 106;
			PredefinedCharset.ISOAdobeCharset.codes[106] = 107;
			PredefinedCharset.ISOAdobeCharset.codes[107] = 108;
			PredefinedCharset.ISOAdobeCharset.codes[108] = 109;
			PredefinedCharset.ISOAdobeCharset.codes[109] = 110;
			PredefinedCharset.ISOAdobeCharset.codes[110] = 111;
			PredefinedCharset.ISOAdobeCharset.codes[111] = 112;
			PredefinedCharset.ISOAdobeCharset.codes[112] = 113;
			PredefinedCharset.ISOAdobeCharset.codes[113] = 114;
			PredefinedCharset.ISOAdobeCharset.codes[114] = 115;
			PredefinedCharset.ISOAdobeCharset.codes[115] = 116;
			PredefinedCharset.ISOAdobeCharset.codes[116] = 117;
			PredefinedCharset.ISOAdobeCharset.codes[117] = 118;
			PredefinedCharset.ISOAdobeCharset.codes[118] = 119;
			PredefinedCharset.ISOAdobeCharset.codes[119] = 120;
			PredefinedCharset.ISOAdobeCharset.codes[120] = 121;
			PredefinedCharset.ISOAdobeCharset.codes[121] = 122;
			PredefinedCharset.ISOAdobeCharset.codes[122] = 123;
			PredefinedCharset.ISOAdobeCharset.codes[123] = 124;
			PredefinedCharset.ISOAdobeCharset.codes[124] = 125;
			PredefinedCharset.ISOAdobeCharset.codes[125] = 126;
			PredefinedCharset.ISOAdobeCharset.codes[126] = 127;
			PredefinedCharset.ISOAdobeCharset.codes[127] = 128;
			PredefinedCharset.ISOAdobeCharset.codes[128] = 129;
			PredefinedCharset.ISOAdobeCharset.codes[129] = 130;
			PredefinedCharset.ISOAdobeCharset.codes[130] = 131;
			PredefinedCharset.ISOAdobeCharset.codes[131] = 132;
			PredefinedCharset.ISOAdobeCharset.codes[132] = 133;
			PredefinedCharset.ISOAdobeCharset.codes[133] = 134;
			PredefinedCharset.ISOAdobeCharset.codes[134] = 135;
			PredefinedCharset.ISOAdobeCharset.codes[135] = 136;
			PredefinedCharset.ISOAdobeCharset.codes[136] = 137;
			PredefinedCharset.ISOAdobeCharset.codes[137] = 138;
			PredefinedCharset.ISOAdobeCharset.codes[138] = 139;
			PredefinedCharset.ISOAdobeCharset.codes[139] = 140;
			PredefinedCharset.ISOAdobeCharset.codes[140] = 141;
			PredefinedCharset.ISOAdobeCharset.codes[141] = 142;
			PredefinedCharset.ISOAdobeCharset.codes[142] = 143;
			PredefinedCharset.ISOAdobeCharset.codes[143] = 144;
			PredefinedCharset.ISOAdobeCharset.codes[144] = 145;
			PredefinedCharset.ISOAdobeCharset.codes[145] = 146;
			PredefinedCharset.ISOAdobeCharset.codes[146] = 147;
			PredefinedCharset.ISOAdobeCharset.codes[147] = 148;
			PredefinedCharset.ISOAdobeCharset.codes[148] = 149;
			PredefinedCharset.ISOAdobeCharset.codes[149] = 150;
			PredefinedCharset.ISOAdobeCharset.codes[150] = 151;
			PredefinedCharset.ISOAdobeCharset.codes[151] = 152;
			PredefinedCharset.ISOAdobeCharset.codes[152] = 153;
			PredefinedCharset.ISOAdobeCharset.codes[153] = 154;
			PredefinedCharset.ISOAdobeCharset.codes[154] = 155;
			PredefinedCharset.ISOAdobeCharset.codes[155] = 156;
			PredefinedCharset.ISOAdobeCharset.codes[156] = 157;
			PredefinedCharset.ISOAdobeCharset.codes[157] = 158;
			PredefinedCharset.ISOAdobeCharset.codes[158] = 159;
			PredefinedCharset.ISOAdobeCharset.codes[159] = 160;
			PredefinedCharset.ISOAdobeCharset.codes[160] = 161;
			PredefinedCharset.ISOAdobeCharset.codes[161] = 162;
			PredefinedCharset.ISOAdobeCharset.codes[162] = 163;
			PredefinedCharset.ISOAdobeCharset.codes[163] = 164;
			PredefinedCharset.ISOAdobeCharset.codes[164] = 165;
			PredefinedCharset.ISOAdobeCharset.codes[165] = 166;
			PredefinedCharset.ISOAdobeCharset.codes[166] = 167;
			PredefinedCharset.ISOAdobeCharset.codes[167] = 168;
			PredefinedCharset.ISOAdobeCharset.codes[168] = 169;
			PredefinedCharset.ISOAdobeCharset.codes[169] = 170;
			PredefinedCharset.ISOAdobeCharset.codes[170] = 171;
			PredefinedCharset.ISOAdobeCharset.codes[171] = 172;
			PredefinedCharset.ISOAdobeCharset.codes[172] = 173;
			PredefinedCharset.ISOAdobeCharset.codes[173] = 174;
			PredefinedCharset.ISOAdobeCharset.codes[174] = 175;
			PredefinedCharset.ISOAdobeCharset.codes[175] = 176;
			PredefinedCharset.ISOAdobeCharset.codes[176] = 177;
			PredefinedCharset.ISOAdobeCharset.codes[177] = 178;
			PredefinedCharset.ISOAdobeCharset.codes[178] = 179;
			PredefinedCharset.ISOAdobeCharset.codes[179] = 180;
			PredefinedCharset.ISOAdobeCharset.codes[180] = 181;
			PredefinedCharset.ISOAdobeCharset.codes[181] = 182;
			PredefinedCharset.ISOAdobeCharset.codes[182] = 183;
			PredefinedCharset.ISOAdobeCharset.codes[183] = 184;
			PredefinedCharset.ISOAdobeCharset.codes[184] = 185;
			PredefinedCharset.ISOAdobeCharset.codes[185] = 186;
			PredefinedCharset.ISOAdobeCharset.codes[186] = 187;
			PredefinedCharset.ISOAdobeCharset.codes[187] = 188;
			PredefinedCharset.ISOAdobeCharset.codes[188] = 189;
			PredefinedCharset.ISOAdobeCharset.codes[189] = 190;
			PredefinedCharset.ISOAdobeCharset.codes[190] = 191;
			PredefinedCharset.ISOAdobeCharset.codes[191] = 192;
			PredefinedCharset.ISOAdobeCharset.codes[192] = 193;
			PredefinedCharset.ISOAdobeCharset.codes[193] = 194;
			PredefinedCharset.ISOAdobeCharset.codes[194] = 195;
			PredefinedCharset.ISOAdobeCharset.codes[195] = 196;
			PredefinedCharset.ISOAdobeCharset.codes[196] = 197;
			PredefinedCharset.ISOAdobeCharset.codes[197] = 198;
			PredefinedCharset.ISOAdobeCharset.codes[198] = 199;
			PredefinedCharset.ISOAdobeCharset.codes[199] = 200;
			PredefinedCharset.ISOAdobeCharset.codes[200] = 201;
			PredefinedCharset.ISOAdobeCharset.codes[201] = 202;
			PredefinedCharset.ISOAdobeCharset.codes[202] = 203;
			PredefinedCharset.ISOAdobeCharset.codes[203] = 204;
			PredefinedCharset.ISOAdobeCharset.codes[204] = 205;
			PredefinedCharset.ISOAdobeCharset.codes[205] = 206;
			PredefinedCharset.ISOAdobeCharset.codes[206] = 207;
			PredefinedCharset.ISOAdobeCharset.codes[207] = 208;
			PredefinedCharset.ISOAdobeCharset.codes[208] = 209;
			PredefinedCharset.ISOAdobeCharset.codes[209] = 210;
			PredefinedCharset.ISOAdobeCharset.codes[210] = 211;
			PredefinedCharset.ISOAdobeCharset.codes[211] = 212;
			PredefinedCharset.ISOAdobeCharset.codes[212] = 213;
			PredefinedCharset.ISOAdobeCharset.codes[213] = 214;
			PredefinedCharset.ISOAdobeCharset.codes[214] = 215;
			PredefinedCharset.ISOAdobeCharset.codes[215] = 216;
			PredefinedCharset.ISOAdobeCharset.codes[216] = 217;
			PredefinedCharset.ISOAdobeCharset.codes[217] = 218;
			PredefinedCharset.ISOAdobeCharset.codes[218] = 219;
			PredefinedCharset.ISOAdobeCharset.codes[219] = 220;
			PredefinedCharset.ISOAdobeCharset.codes[220] = 221;
			PredefinedCharset.ISOAdobeCharset.codes[221] = 222;
			PredefinedCharset.ISOAdobeCharset.codes[222] = 223;
			PredefinedCharset.ISOAdobeCharset.codes[223] = 224;
			PredefinedCharset.ISOAdobeCharset.codes[224] = 225;
			PredefinedCharset.ISOAdobeCharset.codes[225] = 226;
			PredefinedCharset.ISOAdobeCharset.codes[226] = 227;
			PredefinedCharset.ISOAdobeCharset.codes[227] = 228;
		}

		static PredefinedCharset()
		{
			PredefinedCharset.InitializeISOAdobeCharset();
			PredefinedCharset.InitializeExpertCharset();
			PredefinedCharset.InitializeExpertSubsetCharset();
		}

		ushort[] GetCodes()
		{
			return this.codes.ToArray<ushort>();
		}

		public static PredefinedCharset ISOAdobeCharset;

		public static PredefinedCharset ExpertCharset;

		public static PredefinedCharset ExpertSubsetCharset;

		ushort[] codes;
	}
}
