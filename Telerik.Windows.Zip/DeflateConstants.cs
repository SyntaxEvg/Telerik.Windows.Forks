using System;

namespace Telerik.Windows.Zip
{
	static class DeflateConstants
	{
		public const int HeaderDeflated = 8;

		public const int MaxBits = 15;

		public const int BitLengthCodes = 19;

		public const int DynamicCodes = 30;

		public const int Literals = 256;

		public const int CodesLength = 29;

		public const int LiteralCodes = 286;

		public const int EndOfBlockCode = 256;

		public const int MaxDistanceTreeElements = 32;

		public const int MaxLiteralTreeElements = 288;

		public const int MaxBitLengthBits = 7;

		public const int Repeat3To6 = 16;

		public const int RepeatZero3To10 = 17;

		public const int RepeatZero11To138 = 18;
	}
}
