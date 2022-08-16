using System;

namespace Telerik.Windows.Zip
{
	abstract class LzmaTransformBase : CompressionTransformBase
	{
		public LzmaTransformBase(LzmaSettings settings)
		{
			this.Settings = settings;
		}

		protected LzmaSettings Settings { get; set; }

		public const uint RepeaterDistancesNumber = 4U;

		public const uint StatesNumber = 12U;

		public const int PosSlotBitsNumber = 6;

		public const int DictionaryLogSizeMin = 0;

		public const int LengthToPosStatesBits = 2;

		public const uint LengthToPosStates = 4U;

		public const uint MatchMinLength = 2U;

		public const int AlignBitsNumber = 4;

		public const uint AlignTableSize = 16U;

		public const uint AlignMask = 15U;

		public const uint StartPosModelIndex = 4U;

		public const uint EndPosModelIndex = 14U;

		public const uint PosModelsNumber = 10U;

		public const uint FullDistancesNumber = 128U;

		public const uint LiteralPosStatesBitsEncodingMax = 4U;

		public const int LiteralContextBitsMax = 8;

		public const int PosStatesBitsMax = 4;

		public const uint PosStatesMaxNumber = 16U;

		public const int PosStatesBitsEncodingMax = 4;

		public const uint PosStatesEncodingMax = 16U;

		public const int LowLengthBitsNumber = 3;

		public const int MiddleLengthBitsNumber = 3;

		public const int HighLengthBitsNumber = 8;

		public const uint LowLengthSymbolsNumber = 8U;

		public const uint MiddleLengthSymbolsNumber = 8U;

		public const uint LengthSymbolsNumber = 272U;

		public const uint MatchMaxLen = 273U;
	}
}
