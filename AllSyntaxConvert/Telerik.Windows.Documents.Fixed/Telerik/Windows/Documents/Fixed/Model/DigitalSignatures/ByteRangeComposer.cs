using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	class ByteRangeComposer
	{
		public ByteRangeComposer(ISourceStream source, IEnumerable<SourcePart> sourcePart)
		{
			this.source = source;
			this.sourceParts = sourcePart;
		}

		public int Length
		{
			get
			{
				if (this.length == null)
				{
					this.length = new int?(0);
					foreach (SourcePart sourcePart in this.sourceParts)
					{
						this.length += sourcePart.Length;
					}
				}
				return this.length.Value;
			}
		}

		public byte[] Compose()
		{
			byte[] array = new byte[this.Length];
			this.Read(array);
			return array;
		}

		void Read(byte[] outputBuffer)
		{
			this.Read(0, outputBuffer);
		}

		void Read(int outputBufferPosition, byte[] outputBuffer)
		{
			foreach (SourcePart sourcePart in this.sourceParts)
			{
				int num = this.source.Read(outputBuffer, outputBufferPosition, (long)sourcePart.Offset, sourcePart.Length);
				if (num == -1)
				{
					break;
				}
				outputBufferPosition += num;
			}
		}

		ISourceStream source;

		IEnumerable<SourcePart> sourceParts;

		int? length;
	}
}
