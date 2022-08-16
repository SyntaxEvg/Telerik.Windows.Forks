using System;
using System.IO;
using System.Text;

namespace Telerik.Windows.Documents.Common.Model.Protection
{
	sealed class Hex
	{
		public static string ToHexString(byte[] data)
		{
			byte[] array = null;
			using (MemoryStream memoryStream = new MemoryStream(data.Length * 2))
			{
				Hex.encoder.Encode(data, 0, data.Length, memoryStream);
				array = memoryStream.ToArray();
			}
			return Encoding.ASCII.GetString(array, 0, array.Length);
		}

		public static string ToHexString(byte[] data, int off, int length)
		{
			byte[] array = null;
			using (MemoryStream memoryStream = new MemoryStream(length * 2))
			{
				Hex.encoder.Encode(data, off, length, memoryStream);
				array = memoryStream.ToArray();
			}
			return Encoding.ASCII.GetString(array, 0, array.Length);
		}

		static readonly IEncoder encoder = new HexEncoder();
	}
}
