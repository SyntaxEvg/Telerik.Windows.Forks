using System;
using System.ComponentModel;
using System.IO;

namespace Telerik.Windows.Zip
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class ZipOutputStream : CompressedStream
	{
		public ZipOutputStream(Stream baseStream)
			: this(baseStream, CompressionMethod.Deflate)
		{
		}

		public ZipOutputStream(Stream baseStream, CompressionMethod method): base(baseStream, StreamOperationMode.Write, ZipHelper.GetCompressionSettings(method, null), false, null)
		{
			this.rb = new byte[1];			
		}

		//public ZipOutputStream(Stream baseStream, ZipCompression compressionLevel): base(baseStream, StreamOperationMode.Write, ZipOutputStream.CreateDeflateSettings(compressionLevel), false, null)
		//{
		//	this.rb = new byte[1];			
		//}

		//public new Stream BaseStream
		//{
		//	get
		//	{
		//		return base.BaseStream;
		//	}
		//}

		//public int UncompressedSize
		//{
		//	get
		//	{
		//		return (int)base.TotalPlainCount;
		//	}
		//}

		public override void WriteByte(byte value)
		{
			this.rb[0] = value;
			this.Write(this.rb, 0, 1);
		}

		internal static DeflateSettings CreateDeflateSettings(ZipCompression compressionLevel)
		{
			return new DeflateSettings
			{
				CompressionLevel = (CompressionLevel)((compressionLevel == ZipCompression.Default) ? ZipCompression.Method6 : compressionLevel)
			};
		}

		byte[] rb;
	}
}
