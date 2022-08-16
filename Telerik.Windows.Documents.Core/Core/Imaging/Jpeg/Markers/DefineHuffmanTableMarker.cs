using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers
{
	class DefineHuffmanTableMarker : JpegMarker
	{
		public override ushort Length
		{
			get
			{
				return this.length;
			}
		}

		public override JpegMarkerType MarkerType
		{
			get
			{
				return JpegMarkerType.DHT;
			}
		}

		public override void InterpretMarker(JpegDecoderBase decoder)
		{
			Guard.ThrowExceptionIfNull<JpegDecoderBase>(decoder, "decoder");
			this.length = 2;
			foreach (HuffmanTable huffmanTable in decoder.Reader.ReadJpegTables<HuffmanTable>())
			{
				decoder.AddHuffmanTable(huffmanTable);
				this.length += huffmanTable.Length;
			}
		}

		public override void WriteMarker(JpegEncoder encoder)
		{
			base.WriteMarker(encoder);
			encoder.Writer.WriteJpegTables<HuffmanTable>(encoder.HuffmanTables, 2);
		}

		ushort length;
	}
}
