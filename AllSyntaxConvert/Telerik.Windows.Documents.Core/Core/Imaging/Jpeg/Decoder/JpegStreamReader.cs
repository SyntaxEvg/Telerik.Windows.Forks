using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Utils;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder
{
	class JpegStreamReader : IJpegReader, IReader
	{
		public JpegStreamReader(Stream stream)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			this.jpegProcessor = new JpegDataProcessor(this);
			this.binaryReader = new BinaryReader(stream);
		}

		Stream Stream
		{
			get
			{
				return this.binaryReader.BaseStream;
			}
		}

		public long Position
		{
			get
			{
				return this.Stream.Position;
			}
			set
			{
				this.Stream.Position = value;
			}
		}

		public bool EndOfFile
		{
			get
			{
				return this.Stream.Position >= this.Length;
			}
		}

		public long Length
		{
			get
			{
				return this.Stream.Length;
			}
		}

		public byte Peek(int skip = 0)
		{
			long position = this.Position;
			int num = 0;
			byte result;
			while (num++ < skip)
			{
				result = this.binaryReader.ReadByte();
			}
			result = this.binaryReader.ReadByte();
			this.Position = position;
			return result;
		}

		public byte Read()
		{
			return this.binaryReader.ReadByte();
		}

		public int Read(byte[] buffer, int count)
		{
			return this.binaryReader.Read(buffer, 0, count);
		}

		public byte Read4()
		{
			return this.jpegProcessor.Read4();
		}

		public byte Read8()
		{
			return this.jpegProcessor.Read8();
		}

		public ushort Read16()
		{
			return this.jpegProcessor.Read16();
		}

		public int ReadBit()
		{
			return this.jpegProcessor.ReadBit();
		}

		public JpegMarker ReadNextJpegMarker()
		{
			return this.jpegProcessor.ReadNextJpegMarker();
		}

		public IEnumerable<T> ReadJpegTables<T>() where T : JpegTable, new()
		{
			return this.jpegProcessor.ReadJpegTables<T>();
		}

		public int ReceiveAndExtend(int length)
		{
			return this.jpegProcessor.ReceiveAndExtend(length);
		}

		public int Receive(int length)
		{
			return this.jpegProcessor.Receive(length);
		}

		public void Seek(long offset, SeekOrigin origin)
		{
			switch (origin)
			{
			case SeekOrigin.Begin:
				this.Position = offset;
				return;
			case SeekOrigin.Current:
				this.Position += offset;
				return;
			case SeekOrigin.End:
				this.Position = this.Length - offset;
				return;
			default:
				return;
			}
		}

		public void Restart()
		{
		}

		public void FreeResources()
		{
			this.binaryReader.Dispose();
		}

		readonly BinaryReader binaryReader;

		readonly JpegDataProcessor jpegProcessor;
	}
}
