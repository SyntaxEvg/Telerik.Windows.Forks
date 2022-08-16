using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format.Pfb
{
	class PfbHeader
	{
		public long Offset
		{
			get
			{
				return this.offset;
			}
		}

		public int HeaderLength
		{
			get
			{
				return this.headerLength;
			}
		}

		public uint NextHeaderOffset { get; set; }

		public PfbHeader(long offset, int headerLength = 6)
		{
			Guard.ThrowExceptionIfLessThan<long>(0L, offset, "offset");
			this.offset = offset;
			this.headerLength = headerLength;
		}

		public void Read(PfbFontReader reader)
		{
			Guard.ThrowExceptionIfNull<PfbFontReader>(reader, "reader");
			reader.Read();
			reader.Read();
			this.NextHeaderOffset = reader.ReadUInt();
		}

		public bool IsPositionInside(long position)
		{
			return this.Offset <= position && position < this.Offset + (long)this.HeaderLength;
		}

		readonly long offset;

		readonly int headerLength;
	}
}
