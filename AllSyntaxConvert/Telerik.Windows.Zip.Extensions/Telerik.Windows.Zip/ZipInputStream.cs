using System;
using System.ComponentModel;
using System.IO;

namespace Telerik.Windows.Zip
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This class has been deprecated. Use CompressedStream instead of ZipInputStream.")]
	public class ZipInputStream : CompressedStream
	{
		public ZipInputStream(Stream baseStream)
			: base(baseStream, StreamOperationMode.Read, new DeflateSettings(), false, null)
		{
		}

		public new Stream BaseStream
		{
			get
			{
				return base.BaseStream;
			}
		}

		public int UncompressedSize
		{
			get
			{
				return (int)base.TotalPlainCount;
			}
		}

		public override int ReadByte()
		{
			if (this.Read(this.rb, 0, 1) != 0)
			{
				return (int)this.rb[0];
			}
			return -1;
		}

		byte[] rb = new byte[1];
	}
}
