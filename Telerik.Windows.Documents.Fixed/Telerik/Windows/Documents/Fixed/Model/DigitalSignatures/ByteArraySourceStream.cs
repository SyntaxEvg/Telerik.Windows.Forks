using System;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	class ByteArraySourceStream : ISourceStream
	{
		public ByteArraySourceStream(byte[] source)
		{
			this.source = source;
		}

		public bool CanRead
		{
			get
			{
				return true;
			}
		}

		public bool CanSeek
		{
			get
			{
				return true;
			}
		}

		public int Read(byte[] outputBuffer, int outputBufferPosition, long streamOffset, int length)
		{
			Array.Copy(this.source, streamOffset, outputBuffer, (long)outputBufferPosition, (long)length);
			return length;
		}

		byte[] source;
	}
}
