using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers
{
	class StartOfScanMarker : JpegMarker
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
				return JpegMarkerType.SOS;
			}
		}

		public override void InterpretMarker(JpegDecoderBase decoder)
		{
			Guard.ThrowExceptionIfNull<JpegDecoderBase>(decoder, "decoder");
			ScanHeader scanHeader = new ScanHeader();
			scanHeader.Read(decoder.Reader);
			decoder.ScanHeader = scanHeader;
			this.length = scanHeader.Length;
		}

		public override void WriteMarker(JpegEncoder encoder)
		{
			Guard.ThrowExceptionIfNull<JpegEncoder>(encoder, "encoder");
			base.WriteMarker(encoder);
			encoder.ScanHeader.Write(encoder.Writer);
		}

		ushort length;
	}
}
