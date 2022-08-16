using System;

namespace Telerik.Windows.Zip
{
	struct LzmaState
	{
		public uint Index { get; set; }

		public static uint GetLenToPosState(uint length)
		{
			length -= 2U;
			if (length < 4U)
			{
				return length;
			}
			return 3U;
		}

		public void UpdateChar()
		{
			if (this.Index < 4U)
			{
				this.Index = 0U;
				return;
			}
			if (this.Index < 10U)
			{
				this.Index -= 3U;
				return;
			}
			this.Index -= 6U;
		}

		public void UpdateMatch()
		{
			this.Index = ((this.Index < 7U) ? 7U : 10U);
		}

		public void UpdateRep()
		{
			this.Index = ((this.Index < 7U) ? 8U : 11U);
		}

		public void UpdateShortRep()
		{
			this.Index = ((this.Index < 7U) ? 9U : 11U);
		}

		public bool IsCharState()
		{
			return this.Index < 7U;
		}
	}
}
