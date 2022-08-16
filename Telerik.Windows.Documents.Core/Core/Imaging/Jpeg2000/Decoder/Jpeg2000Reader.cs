using System;
using System.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg2000.Decoder
{
	class Jpeg2000Reader : ReaderBase
	{
		public Jpeg2000Reader(byte[] data)
			: base(data)
		{
		}

		public short ReadBigEndianShort()
		{
			return this.ReadBigEndianObject<short>(2, (byte[] bytes) => BitConverter.ToInt16(bytes, 0));
		}

		public int ReadBigEndianInt()
		{
			return this.ReadBigEndianObject<int>(4, (byte[] bytes) => BitConverter.ToInt32(bytes, 0));
		}

		public ushort ReadBigEndianUShort()
		{
			return this.ReadBigEndianObject<ushort>(2, (byte[] bytes) => BitConverter.ToUInt16(bytes, 0));
		}

		public uint ReadBigEndianUInt()
		{
			return this.ReadBigEndianObject<uint>(4, (byte[] bytes) => BitConverter.ToUInt32(bytes, 0));
		}

		public ulong ReadBigEndianULong()
		{
			return this.ReadBigEndianObject<ulong>(8, (byte[] bytes) => BitConverter.ToUInt64(bytes, 0));
		}

		public string ReadText(int bytesCount)
		{
			byte[] array = new byte[bytesCount];
			base.Read(array, bytesCount);
			return Encoding.UTF8.GetString(array, 0, array.Length);
		}

		public Box ReadBox()
		{
			long position = this.Position;
			Box box = new Box();
			box.Length = (ulong)this.ReadBigEndianUInt();
			box.Type = this.ReadText(4);
			if (box.Length == 1UL)
			{
				box.Length = this.ReadBigEndianULong();
			}
			if (box.Length == 0UL)
			{
				box.Length = (ulong)(this.Length - position);
			}
			long num = position + (long)box.Length;
			int num2 = (int)(num - this.Position);
			byte[] array = new byte[num2];
			base.Read(array, num2);
			box.Content = array;
			return box;
		}

		T ReadBigEndianObject<T>(int numberOfBytes, Func<byte[], T> convertBytesToObject)
		{
			byte[] array = new byte[numberOfBytes];
			base.ReadBE(array, numberOfBytes);
			return convertBytesToObject(array);
		}

		const int ShortBytesCount = 2;

		const int IntegerBytesCount = 4;

		const int LongBytesCount = 8;
	}
}
