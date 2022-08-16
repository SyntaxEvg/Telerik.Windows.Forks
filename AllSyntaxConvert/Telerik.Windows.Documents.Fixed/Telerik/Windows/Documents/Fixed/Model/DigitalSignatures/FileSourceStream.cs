using System;
using System.IO;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	class FileSourceStream : ISourceStream
	{
		public FileSourceStream(Stream stream)
		{
			this.stream = stream;
		}

		public bool CanRead
		{
			get
			{
				return this.stream.CanRead;
			}
		}

		public bool CanSeek
		{
			get
			{
				return this.stream.CanSeek;
			}
		}

		public int Read(byte[] outputBuffer, int outputBufferPosition, long streamOffset, int length)
		{
			long position = this.stream.Position;
			if (this.stream.Position != streamOffset)
			{
				this.stream.Seek(streamOffset, SeekOrigin.Begin);
			}
			int num = this.stream.Read(outputBuffer, outputBufferPosition, length);
			if (this.stream.Position != position)
			{
				this.stream.Seek(position, SeekOrigin.Begin);
			}
			if (num != 0)
			{
				return num;
			}
			return -1;
		}

		Stream stream;
	}
}
