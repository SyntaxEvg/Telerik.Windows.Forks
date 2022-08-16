using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Encoding
{
	class ByteEncodingCollection : List<ByteEncoding>
	{
		public ByteEncoding FindEncoding(byte b0)
		{
			foreach (ByteEncoding byteEncoding in this)
			{
				if (byteEncoding.IsInRange(b0))
				{
					return byteEncoding;
				}
			}
			return null;
		}
	}
}
