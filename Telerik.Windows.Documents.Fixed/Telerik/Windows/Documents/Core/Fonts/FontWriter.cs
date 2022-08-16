using System;
using System.IO;
using System.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts
{
	class FontWriter : IDisposable
	{
		public FontWriter()
		{
			this.stream = new MemoryStream();
			this.writer = new BinaryWriter(this.stream);
		}

		void WriteBE(byte[] buffer, int count)
		{
			for (int i = count - 1; i >= 0; i--)
			{
				this.Write(buffer[i]);
			}
		}

		public void Write(byte b)
		{
			this.writer.Write(b);
		}

		public void WriteChar(sbyte ch)
		{
			this.Write((byte)ch);
		}

		public void WriteUShort(ushort us)
		{
			this.WriteBE(BitConverter.GetBytes(us), 2);
		}

		public void WriteShort(short s)
		{
			this.WriteBE(BitConverter.GetBytes(s), 2);
		}

		public void WriteULong(uint ul)
		{
			this.WriteBE(BitConverter.GetBytes(ul), 4);
		}

		public void WriteLong(int l)
		{
			this.WriteBE(BitConverter.GetBytes(l), 4);
		}

		public void WriteString(string str)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			this.Write((byte)bytes.Length);
			for (int i = 0; i < bytes.Length; i++)
			{
				this.Write(bytes[i]);
			}
		}

		public byte[] GetBytes()
		{
			return this.stream.ReadAllBytes();
		}

		public void Dispose()
		{
			this.writer.Flush();
			this.stream.Close();
			this.writer.Close();
		}

		readonly BinaryWriter writer;

		readonly Stream stream;
	}
}
