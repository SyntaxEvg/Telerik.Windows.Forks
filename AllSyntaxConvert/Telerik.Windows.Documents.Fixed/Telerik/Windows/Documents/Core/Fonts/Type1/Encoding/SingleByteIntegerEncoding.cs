using System;
using Telerik.Windows.Documents.Core.Fonts.Type1.Data;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Encoding
{
	class SingleByteIntegerEncoding : ByteEncoding
	{
		public SingleByteIntegerEncoding()
			: base(32, 246)
		{
		}

		public override object Read(EncodedDataReader reader)
		{
			byte b = reader.Read();
			return (sbyte)(b - 139);
		}
	}
}
