using System;
using System.IO;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Parser
{
	class RtfReader : IDisposable
	{
		public RtfReader(Stream stream)
		{
			this.stream = stream;
			this.CodePageDecoder = new RtfCodePageDecoder(CharSetHelper.AnsiCodePage);
		}

		public RtfCodePageDecoder CodePageDecoder { get; set; }

		public int Peek()
		{
			if (this.usingBuffer)
			{
				return this.buffer;
			}
			this.buffer = this.Read();
			this.usingBuffer = true;
			return this.buffer;
		}

		public int Read()
		{
			if (this.usingBuffer)
			{
				this.usingBuffer = false;
				return this.buffer;
			}
			int num = this.stream.ReadByte();
			if (num == -1)
			{
				return -1;
			}
			return (int)this.CodePageDecoder.Convert(num);
		}

		public byte[] ReadBinary(int count)
		{
			byte[] array = new byte[count];
			int num;
			if (this.usingBuffer)
			{
				array[0] = (byte)this.buffer;
				this.usingBuffer = false;
				count--;
				num = this.stream.Read(array, 1, count);
			}
			else
			{
				num = this.stream.Read(array, 0, count);
			}
			if (num < count)
			{
				throw new EndOfStreamException("Unexpected end of stream.");
			}
			return array;
		}

		public void Close()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			this.stream.Dispose();
		}

		readonly Stream stream;

		int buffer;

		bool usingBuffer;
	}
}
