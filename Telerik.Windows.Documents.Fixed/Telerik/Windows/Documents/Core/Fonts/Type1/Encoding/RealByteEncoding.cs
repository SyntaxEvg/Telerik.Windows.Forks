using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Telerik.Windows.Documents.Core.Fonts.Type1.Data;
using Telerik.Windows.Documents.Core.Fonts.Type1.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Encoding
{
	class RealByteEncoding : ByteEncoding
	{
		static RealByteEncoding()
		{
			RealByteEncoding.nibbleMapping[new Nibble(0)] = "0";
			RealByteEncoding.nibbleMapping[new Nibble(1)] = "1";
			RealByteEncoding.nibbleMapping[new Nibble(2)] = "2";
			RealByteEncoding.nibbleMapping[new Nibble(3)] = "3";
			RealByteEncoding.nibbleMapping[new Nibble(4)] = "4";
			RealByteEncoding.nibbleMapping[new Nibble(5)] = "5";
			RealByteEncoding.nibbleMapping[new Nibble(6)] = "6";
			RealByteEncoding.nibbleMapping[new Nibble(7)] = "7";
			RealByteEncoding.nibbleMapping[new Nibble(8)] = "8";
			RealByteEncoding.nibbleMapping[new Nibble(9)] = "9";
			RealByteEncoding.nibbleMapping[new Nibble(10)] = ".";
			RealByteEncoding.nibbleMapping[new Nibble(11)] = "E";
			RealByteEncoding.nibbleMapping[new Nibble(12)] = "E-";
			RealByteEncoding.nibbleMapping[new Nibble(13)] = "";
			RealByteEncoding.nibbleMapping[new Nibble(14)] = "-";
			RealByteEncoding.endOfNumber = new Nibble(15);
		}

		public RealByteEncoding()
			: base(30, 30)
		{
		}

		public override object Read(EncodedDataReader reader)
		{
			bool flag = false;
			reader.Read();
			StringBuilder stringBuilder = new StringBuilder();
			do
			{
				byte b = reader.Read();
				Nibble[] nibbles = Nibble.GetNibbles(b);
				foreach (Nibble nibble in nibbles)
				{
					if (nibble == RealByteEncoding.endOfNumber)
					{
						flag = true;
						break;
					}
					stringBuilder.Append(RealByteEncoding.nibbleMapping[nibble]);
				}
			}
			while (!flag);
			return double.Parse(stringBuilder.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
		}

		static Dictionary<Nibble, string> nibbleMapping = new Dictionary<Nibble, string>();

		static Nibble endOfNumber;
	}
}
