using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities
{
	static class CharSetHelper
	{
		static CharSetHelper()
		{
			CharSetHelper.CharSetToCodepage[0] = CharSetHelper.AnsiCodePage;
			CharSetHelper.CharSetToCodepage[1] = 0;
			CharSetHelper.CharSetToCodepage[2] = 42;
			CharSetHelper.CharSetToCodepage[77] = 10000;
			CharSetHelper.CharSetToCodepage[78] = 10001;
			CharSetHelper.CharSetToCodepage[79] = 10003;
			CharSetHelper.CharSetToCodepage[80] = 10008;
			CharSetHelper.CharSetToCodepage[81] = 10002;
			CharSetHelper.CharSetToCodepage[82] = 0;
			CharSetHelper.CharSetToCodepage[83] = 10005;
			CharSetHelper.CharSetToCodepage[84] = 10004;
			CharSetHelper.CharSetToCodepage[85] = 10006;
			CharSetHelper.CharSetToCodepage[86] = 10081;
			CharSetHelper.CharSetToCodepage[87] = 10021;
			CharSetHelper.CharSetToCodepage[88] = 10029;
			CharSetHelper.CharSetToCodepage[89] = 10007;
			CharSetHelper.CharSetToCodepage[128] = 932;
			CharSetHelper.CharSetToCodepage[129] = 949;
			CharSetHelper.CharSetToCodepage[130] = 1361;
			CharSetHelper.CharSetToCodepage[134] = 936;
			CharSetHelper.CharSetToCodepage[136] = 950;
			CharSetHelper.CharSetToCodepage[161] = 1253;
			CharSetHelper.CharSetToCodepage[162] = 1254;
			CharSetHelper.CharSetToCodepage[163] = 1258;
			CharSetHelper.CharSetToCodepage[177] = 1255;
			CharSetHelper.CharSetToCodepage[178] = 1256;
			CharSetHelper.CharSetToCodepage[179] = 0;
			CharSetHelper.CharSetToCodepage[180] = 0;
			CharSetHelper.CharSetToCodepage[181] = 0;
			CharSetHelper.CharSetToCodepage[186] = 1257;
			CharSetHelper.CharSetToCodepage[204] = 1251;
			CharSetHelper.CharSetToCodepage[222] = 874;
			CharSetHelper.CharSetToCodepage[238] = 1250;
			CharSetHelper.CharSetToCodepage[254] = 437;
			CharSetHelper.CharSetToCodepage[255] = 850;
		}

		public static int GetCodePage(int charSet)
		{
			int result = 0;
			if (CharSetHelper.CharSetToCodepage.TryGetValue(charSet, out result))
			{
				return result;
			}
			return 0;
		}

		public static readonly int AnsiCodePage = 1252;

		static readonly Dictionary<int, int> CharSetToCodepage = new Dictionary<int, int>();
	}
}
