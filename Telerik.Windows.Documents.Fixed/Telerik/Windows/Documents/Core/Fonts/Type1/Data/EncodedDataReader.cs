using System;
using Telerik.Windows.Documents.Core.Fonts.Type1.Encoding;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Data
{
	class EncodedDataReader : ReaderBase
	{
		public EncodedDataReader(byte[] data, ByteEncodingCollection encodings)
			: base(data)
		{
			this.encodings = encodings;
		}

		public object ReadOperand()
		{
			byte b = this.Peek(0);
			ByteEncoding byteEncoding = this.encodings.FindEncoding(b);
			return byteEncoding.Read(this);
		}

		readonly ByteEncodingCollection encodings;
	}
}
