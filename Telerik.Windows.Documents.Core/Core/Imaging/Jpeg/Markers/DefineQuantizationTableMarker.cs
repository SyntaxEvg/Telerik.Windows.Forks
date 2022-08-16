using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers
{
	class DefineQuantizationTableMarker : JpegMarker
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
				return JpegMarkerType.DQT;
			}
		}

		public override void InterpretMarker(JpegDecoderBase decoder)
		{
			Guard.ThrowExceptionIfNull<JpegDecoderBase>(decoder, "decoder");
			this.length = 2;
			foreach (QuantizationTable quantizationTable in decoder.Reader.ReadJpegTables<QuantizationTable>())
			{
				decoder.AddQuantisationTable(quantizationTable);
				this.length += quantizationTable.Length;
			}
		}

		public override void WriteMarker(JpegEncoder encoder)
		{
			base.WriteMarker(encoder);
			encoder.Writer.WriteJpegTables<QuantizationTable>(encoder.QuantizationTables, 2);
		}

		ushort length;
	}
}
