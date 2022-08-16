using System;
using Telerik.Windows.Documents.Core.Fonts.Type1.Data;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Encoding
{
	class FiveByteFixedEncoding : ByteEncoding
	{
		public FiveByteFixedEncoding()
			: base(byte.MaxValue, byte.MaxValue)
		{
		}

		public override object Read(EncodedDataReader reader)
		{
			reader.Read();
			byte b = reader.Read();
			byte b2 = reader.Read();
			byte b3 = reader.Read();
			byte b4 = reader.Read();
			int num = ((int)b << 24) | ((int)b2 << 16) | ((int)b3 << 8) | (int)b4;
			if ((num & 65535) != 0 && (num & 32768) != 0)
			{
				return (num >> 16) + 1;
			}
			return num >> 16;
		}
	}
}
