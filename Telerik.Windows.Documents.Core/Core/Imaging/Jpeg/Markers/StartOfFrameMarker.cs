using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers
{
	 class StartOfFrameMarker : JpegMarker
	{
		public StartOfFrameMarker(JpegEncodingType encodingType)
		{
			this.encodingType = encodingType;
			this.scanDecoder = ScanDecoder.GetDecoder(encodingType);
			this.scanEncoder = ScanEncoder.GetEncoder(encodingType);
		}

		public JpegEncodingType EncodingType
		{
			get
			{
				return this.encodingType;
			}
		}

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
				return JpegMarkerType.SOF;
			}
		}

		public ScanDecoder ScanDecoder
		{
			get
			{
				return this.scanDecoder;
			}
		}

		public ScanEncoder ScanEncoder
		{
			get
			{
				return this.scanEncoder;
			}
		}

		public override void InterpretMarker(JpegDecoderBase decoder)
		{
			Guard.ThrowExceptionIfNull<JpegDecoderBase>(decoder, "decoder");
			FrameHeader frameHeader = new FrameHeader();
			frameHeader.Read(decoder.Reader);
			decoder.FrameHeader = frameHeader;
			this.length = frameHeader.Length;
		}

		public override void WriteMarker(JpegEncoder encoder)
		{
			Guard.ThrowExceptionIfNull<JpegEncoder>(encoder, "encoder");
			base.WriteMarker(encoder);
			encoder.FrameHeader.Write(encoder.Writer);
		}

		ushort length;

		readonly JpegEncodingType encodingType;

		readonly ScanDecoder scanDecoder;

		readonly ScanEncoder scanEncoder;
	}
}
