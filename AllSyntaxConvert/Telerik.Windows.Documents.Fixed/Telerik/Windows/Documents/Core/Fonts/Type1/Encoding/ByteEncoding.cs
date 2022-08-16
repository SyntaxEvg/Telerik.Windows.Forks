using System;
using Telerik.Windows.Documents.Core.Fonts.Type1.Data;
using Telerik.Windows.Documents.Core.Fonts.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Encoding
{
	abstract class ByteEncoding
	{
		public static ByteEncodingCollection DictByteEncodings { get; set; }

		public static ByteEncodingCollection CharStringByteEncodings { get; set; }

		static void InitializeDictByteEncodings()
		{
			ByteEncoding.DictByteEncodings = new ByteEncodingCollection();
			ByteEncoding.DictByteEncodings.Add(new SingleByteIntegerEncoding());
			ByteEncoding.DictByteEncodings.Add(new TwoByteIntegerEncodingType1());
			ByteEncoding.DictByteEncodings.Add(new TwoByteIntegerEncodingType2());
			ByteEncoding.DictByteEncodings.Add(new ThreeByteIntegerEncoding());
			ByteEncoding.DictByteEncodings.Add(new FiveByteIntegerEncoding());
			ByteEncoding.DictByteEncodings.Add(new RealByteEncoding());
		}

		static void InitializeCharStringByteEncodings()
		{
			ByteEncoding.CharStringByteEncodings = new ByteEncodingCollection();
			ByteEncoding.CharStringByteEncodings.Add(new ThreeByteIntegerEncoding());
			ByteEncoding.CharStringByteEncodings.Add(new SingleByteIntegerEncoding());
			ByteEncoding.CharStringByteEncodings.Add(new TwoByteIntegerEncodingType1());
			ByteEncoding.CharStringByteEncodings.Add(new TwoByteIntegerEncodingType2());
			ByteEncoding.CharStringByteEncodings.Add(new FiveByteFixedEncoding());
		}

		static ByteEncoding()
		{
			ByteEncoding.InitializeDictByteEncodings();
			ByteEncoding.InitializeCharStringByteEncodings();
		}

		public ByteEncoding(byte start, byte end)
		{
			this.range = new Range((int)start, (int)end);
		}

		public abstract object Read(EncodedDataReader reader);

		public bool IsInRange(byte b0)
		{
			return this.range.IsInRange((int)b0);
		}

		readonly Range range;
	}
}
