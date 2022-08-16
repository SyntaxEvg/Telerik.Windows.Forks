using System;
using System.IO;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Utils
{
	public interface IReader
	{
		long Length { get; }

		bool EndOfFile { get; }

		long Position { get; }

		byte Read();

		byte Read4();

		byte Read8();

		ushort Read16();

		int ReadBit();

		int ReceiveAndExtend(int length);

		int Read(byte[] buffer, int count);

		int Receive(int length);

		void Seek(long offset, SeekOrigin origin);

		byte Peek(int skip = 0);

		void Restart();

		void FreeResources();
	}
}
