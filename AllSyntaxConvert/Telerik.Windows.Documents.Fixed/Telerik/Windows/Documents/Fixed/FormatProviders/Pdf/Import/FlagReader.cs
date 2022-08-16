using System;
using System.Globalization;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	class FlagReader<T> where T : struct, IConvertible
	{
		public FlagReader(int flags)
		{
			this.flags = flags;
		}

		public bool IsSet(T flag)
		{
			int num = flag.ToInt32(CultureInfo.InvariantCulture);
			Guard.ThrowExceptionIfOutOfRange<int>(1, 32, num, "bitNumber");
			int num2 = num - 1;
			return (this.flags & (1 << num2)) != 0;
		}

		readonly int flags;
	}
}
