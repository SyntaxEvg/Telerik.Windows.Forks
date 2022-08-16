using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers
{
	 abstract class JpegMarker
	{
		static JpegMarker()
		{
			JpegMarker.RegisterMarker(224, new JfifMarker());
			JpegMarker.RegisterMarker(238, new AdobeMarker());
			JpegMarker.RegisterMarker(216, new StartOfImageMarker());
			JpegMarker.RegisterMarker(192, 193, new StartOfFrameMarker(JpegEncodingType.BaselineDct));
			JpegMarker.RegisterMarker(201, new StartOfFrameMarker(JpegEncodingType.BaselineDct));
			JpegMarker.RegisterMarker(194, new StartOfFrameMarker(JpegEncodingType.ProgressiveDct));
			JpegMarker.RegisterMarker(202, new StartOfFrameMarker(JpegEncodingType.ProgressiveDct));
			JpegMarker.RegisterMarker(195, new StartOfFrameMarker(JpegEncodingType.NotSupported));
			JpegMarker.RegisterMarker(197, 199, new StartOfFrameMarker(JpegEncodingType.NotSupported));
			JpegMarker.RegisterMarker(203, new StartOfFrameMarker(JpegEncodingType.NotSupported));
			JpegMarker.RegisterMarker(203, 207, new StartOfFrameMarker(JpegEncodingType.NotSupported));
			JpegMarker.RegisterMarker(196, new DefineHuffmanTableMarker());
			JpegMarker.RegisterMarker(219, new DefineQuantizationTableMarker());
			JpegMarker.RegisterMarker(221, new DefineRestartIntervalMarker());
			JpegMarker.RegisterMarker(218, new StartOfScanMarker());
			JpegMarker.RegisterMarker(208, 215, new RestartMarker());
			JpegMarker.RegisterMarker(217, new EndOfImageMarker());
			JpegMarker.RegisterMarker(254, new CommentMarker());
		}

		public abstract ushort Length { get; }

		public abstract JpegMarkerType MarkerType { get; }

		public byte Code { get; set; }

		public static JpegMarker GetMarker(byte code)
		{
			JpegMarker result;
			if (JpegMarker.markers.TryGetValue(code, out result))
			{
				return result;
			}
			return new NotSupportedMarker
			{
				Code = code
			};
		}

		public static JpegMarker GetMarker(JpegMarkerType markerType)
		{
			return (from mrk in JpegMarker.markers.Values
				where mrk.MarkerType == markerType
				select mrk).FirstOrDefault<JpegMarker>();
		}

		public static StartOfFrameMarker GetSOFMarkerForEncoding(JpegEncodingType encoding)
		{
			return (from mrk in JpegMarker.markers.Values
				where mrk.MarkerType == JpegMarkerType.SOF && ((StartOfFrameMarker)mrk).EncodingType == encoding
				select mrk).FirstOrDefault<JpegMarker>() as StartOfFrameMarker;
		}

		public abstract void InterpretMarker(JpegDecoderBase decoder);

		public static bool IsJpegMarker(byte b1, byte b2)
		{
			return JpegMarker.IsJpegMarker(b1) && b2 != 0;
		}

		public static bool IsJpegMarker(byte b)
		{
			return b == byte.MaxValue;
		}

		public static ushort ReadLengthAndSkipData(JpegDecoderBase decoder)
		{
			Guard.ThrowExceptionIfNull<JpegDecoderBase>(decoder, "decoder");
			ushort num = decoder.Reader.Read16();
			decoder.Reader.Seek((long)(num - 2), SeekOrigin.Current);
			return num;
		}

		public virtual void WriteMarker(JpegEncoder encoder)
		{
			encoder.Writer.WriteJpegMarker(this);
		}

		static void RegisterMarker(byte fromCode, byte toCode, JpegMarker marker)
		{
			for (byte b = fromCode; b <= toCode; b += 1)
			{
				JpegMarker.RegisterMarker(b, marker);
			}
		}

		static void RegisterMarker(byte code, JpegMarker marker)
		{
			marker.Code = code;
			JpegMarker.markers[code] = marker;
		}

		public const int MarkerIdentifier = 255;

		public const int EscapeFFByte = 0;

		static readonly Dictionary<byte, JpegMarker> markers = new Dictionary<byte, JpegMarker>();
	}
}
