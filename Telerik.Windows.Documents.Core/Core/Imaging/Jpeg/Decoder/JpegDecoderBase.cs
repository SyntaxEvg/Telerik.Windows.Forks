using System;
using System.IO;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.ColorSpaces;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder
{
	 abstract class JpegDecoderBase : IHuffmanTablesOwner
	{
		public JpegDecoderBase(byte[] data)
			: this()
		{
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			this.reader = new JpegByteArrayReader(data);
		}

		public JpegDecoderBase(Stream stream)
			: this()
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			this.reader = new JpegStreamReader(stream);
		}

		JpegDecoderBase()
		{
			this.quantizationTables = new QuantizationTable[4];
			this.huffmanTables = new HuffmanTable[2, 4];
			this.IsRestartIntervalDefined = false;
			this.ColorTransform = JpegColorSpace.Undefined;
		}

		~JpegDecoderBase()
		{
			this.Reader.FreeResources();
		}

		public IJpegReader Reader
		{
			get
			{
				return this.reader;
			}
		}

		public FrameHeader FrameHeader { get; set; }

		public ScanHeader ScanHeader { get; set; }

		public ScanDecoder ComponentDecoder { get; set; }

		public JpegColorSpace ColorTransform { get; set; }

		public int RestartInterval { get; set; }

		public bool IsRestartIntervalDefined { get; set; }

		public void AddQuantisationTable(QuantizationTable table)
		{
			Guard.ThrowExceptionIfNull<QuantizationTable>(table, "table");
			this.quantizationTables[(int)table.TableIndex] = table;
		}

		public QuantizationTable GetQuantisationTable(int quantizationTableId)
		{
			return this.quantizationTables[quantizationTableId];
		}

		public void AddHuffmanTable(HuffmanTable table)
		{
			Guard.ThrowExceptionIfNull<HuffmanTable>(table, "table");
			this.huffmanTables[(int)table.TableClass, (int)table.TableIndex] = table;
		}

		public HuffmanTable GetHuffmanTable(TableClass tableClass, int tableIndex)
		{
			return this.huffmanTables[(int)tableClass, tableIndex];
		}

		protected bool TryReadJpegStartOfImageMarker()
		{
			if (this.Reader.Length <= 2L)
			{
				return false;
			}
			byte b = this.Reader.Read();
			if (b == 255)
			{
				byte code = this.Reader.Read();
				JpegMarker marker = JpegMarker.GetMarker(code);
				return marker.MarkerType == JpegMarkerType.SOI;
			}
			return false;
		}

		readonly IJpegReader reader;

		readonly QuantizationTable[] quantizationTables;

		readonly HuffmanTable[,] huffmanTables;
	}
}
