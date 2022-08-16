using System;
using System.Collections.Generic;
using System.Globalization;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export
{
	class FlagWriter<T> where T : struct, IConvertible
	{
		public FlagWriter()
		{
			this.flags = 0;
		}

		public FlagWriter(IEnumerable<int> reservedSetFlagNumbers)
			: this()
		{
			foreach (int bit in reservedSetFlagNumbers)
			{
				this.SetBit(bit);
			}
		}

		public int ResultFlags
		{
			get
			{
				return this.flags;
			}
		}

		public void SetFlagOnCondition(T flag, bool shouldSetFlag)
		{
			if (shouldSetFlag)
			{
				this.SetFlag(flag);
			}
		}

		public void SetFlag(T flag)
		{
			int bit = flag.ToInt32(CultureInfo.InvariantCulture);
			this.SetBit(bit);
		}

		void SetBit(int bitNumber)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(1, 32, bitNumber, "bitNumber");
			int num = bitNumber - 1;
			this.flags |= 1 << num;
		}

		int flags;
	}
}
