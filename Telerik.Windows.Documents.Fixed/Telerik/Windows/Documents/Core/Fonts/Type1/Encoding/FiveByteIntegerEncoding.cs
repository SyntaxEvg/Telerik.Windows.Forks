using System;
using Telerik.Windows.Documents.Core.Fonts.Type1.Data;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Encoding
{
	class FiveByteIntegerEncoding : ByteEncoding
	{
		public FiveByteIntegerEncoding()
			: base(29, 29)
		{
		}

		public override object Read(EncodedDataReader reader)
		{
			reader.Read();
			byte b = reader.Read();
			byte b2 = reader.Read();
			byte b3 = reader.Read();
			byte b4 = reader.Read();
			return ((int)b << 24) | ((int)b2 << 16) | ((int)b3 << 8) | (int)b4;
		}
	}
}
