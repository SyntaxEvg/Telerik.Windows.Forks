using System;
using Telerik.Windows.Documents.Core.Fonts.Type1.Data;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Encoding
{
	class TwoByteIntegerEncodingType2 : ByteEncoding
	{
		public TwoByteIntegerEncodingType2()
			: base(251, 254)
		{
		}

		public override object Read(EncodedDataReader reader)
		{
			byte b = reader.Read();
			byte b2 = reader.Read();
			return (short)((int)(-(int)(b - 251)) * 256 - (int)b2 - 108);
		}
	}
}
