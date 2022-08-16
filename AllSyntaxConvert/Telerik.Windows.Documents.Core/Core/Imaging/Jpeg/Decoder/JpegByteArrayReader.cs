using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Utils;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder
{
	class JpegByteArrayReader : ReaderBase, IJpegReader, IReader
	{
		public JpegByteArrayReader(byte[] data)
			: base(data)
		{
			this.jpegProcessor = new JpegDataProcessor(this);
			this.Restart();
		}

		public int ReadBit()
		{
			int result;
			if (this.bitsCount > 0)
			{
				this.bitsCount--;
				result = (this.bitsData >> this.bitsCount) & 1;
			}
			else
			{
				this.bitsData = this.Read();
				if (this.bitsData == 255)
				{
					byte b = this.Read();
					if (b != 0)
					{
						throw new InvalidOperationException(string.Format("Unexpected marker {0:X}!", ((int)this.bitsData << 8) | (int)b));
					}
				}
				this.bitsCount = 7;
				result = this.bitsData >> 7;
			}
			return result;
		}

		public void Restart()
		{
			this.bitsCount = 0;
			this.bitsData = 0;
		}

		public JpegMarker ReadNextJpegMarker()
		{
			return this.jpegProcessor.ReadNextJpegMarker();
		}

		public IEnumerable<T> ReadJpegTables<T>() where T : JpegTable, new()
		{
			return this.jpegProcessor.ReadJpegTables<T>();
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

		public int ReceiveAndExtend(int length)
		{
			return this.jpegProcessor.ReceiveAndExtend(length);
		}

		public int Receive(int length)
		{
			return this.jpegProcessor.Receive(length);
		}

		public void FreeResources()
		{
		}

		readonly JpegDataProcessor jpegProcessor;

		byte bitsData;

		int bitsCount;
	}
}
