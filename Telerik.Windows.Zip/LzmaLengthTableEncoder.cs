using System;

namespace Telerik.Windows.Zip
{
	class LzmaLengthTableEncoder : LzmaLengthEncoder
	{
		public void SetTableSize(uint size)
		{
			this.tableSize = size;
		}

		public uint GetPrice(uint symbol, uint posState)
		{
			return this.prices[(int)((UIntPtr)(posState * 272U + symbol))];
		}

		public void UpdateTable(uint posState)
		{
			base.SetPrices(posState, this.tableSize, this.prices, posState * 272U);
			this.counters[(int)((UIntPtr)posState)] = this.tableSize;
		}

		public void UpdateTables(uint posStates)
		{
			for (uint num = 0U; num < posStates; num += 1U)
			{
				this.UpdateTable(num);
			}
		}

		public new void Encode(LzmaRangeEncoder rangeEncoder, uint symbol, uint posState)
		{
			base.Encode(rangeEncoder, symbol, posState);
			if ((this.counters[(int)((UIntPtr)posState)] -= 1U) == 0U)
			{
				this.UpdateTable(posState);
			}
		}

		uint[] prices = new uint[4352];

		uint tableSize;

		uint[] counters = new uint[16];
	}
}
