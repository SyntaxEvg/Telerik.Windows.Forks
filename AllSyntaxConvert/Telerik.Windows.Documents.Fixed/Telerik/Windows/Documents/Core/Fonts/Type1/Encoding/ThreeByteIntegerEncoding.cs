using System;
using Telerik.Windows.Documents.Core.Fonts.Type1.Data;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Encoding
{
	class ThreeByteIntegerEncoding : ByteEncoding
	{
		public ThreeByteIntegerEncoding()
			: base(28, 28)
		{
		}

		public override object Read(EncodedDataReader reader)
		{
			reader.Read();
			byte b = reader.Read();
			byte b2 = reader.Read();
			return (short)(((int)b << 8) | (int)b2);
		}
	}
}
