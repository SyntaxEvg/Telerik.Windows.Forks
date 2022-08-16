using System;
using Telerik.Windows.Documents.Core.Fonts.Type1.Data;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Encoding
{
	class TwoByteIntegerEncodingType1 : ByteEncoding
	{
		public TwoByteIntegerEncodingType1()
			: base(247, 250)
		{
		}

		public override object Read(EncodedDataReader reader)
		{
			byte b = reader.Read();
			byte b2 = reader.Read();
			return (short)((int)(b - 247) * 256 + (int)b2 + 108);
		}
	}
}
