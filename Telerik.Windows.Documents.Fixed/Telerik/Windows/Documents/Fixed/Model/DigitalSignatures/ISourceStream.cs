using System;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	interface ISourceStream
	{
		bool CanRead { get; }

		bool CanSeek { get; }

		int Read(byte[] outputBuffer, int outputBufferPosition, long streamOffset, int length);
	}
}
