using System;
using CsQuery.StringScanner.Implementation;

namespace CsQuery.StringScanner
{
	static class Scanner
	{
		public static IStringScanner Create(string text)
		{
			return new StringScannerEngine(text);
		}
	}
}
