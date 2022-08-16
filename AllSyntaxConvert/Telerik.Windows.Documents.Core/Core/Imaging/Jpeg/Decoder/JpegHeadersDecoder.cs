using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.ColorSpaces;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder
{
	 class JpegHeadersDecoder : JpegDecoderBase
	{
		static JpegHeadersDecoder()
		{
			JpegHeadersDecoder.endMarkerTypes.Add(JpegMarkerType.EOI);
			JpegHeadersDecoder.endMarkerTypes.Add(JpegMarkerType.SOS);
			JpegHeadersDecoder.markerTypesToSkip = new HashSet<JpegMarkerType>();
			JpegHeadersDecoder.markerTypesToSkip.Add(JpegMarkerType.COM);
			JpegHeadersDecoder.markerTypesToSkip.Add(JpegMarkerType.DHT);
			JpegHeadersDecoder.markerTypesToSkip.Add(JpegMarkerType.DQT);
		}

		public JpegHeadersDecoder(byte[] data)
			: base(data)
		{
		}

		public JpegHeadersDecoder(Stream stream)
			: base(stream)
		{
		}

		public bool TryDecodeJpegImage(out JpegImageInfo imageInfo)
		{
			if (!base.TryReadJpegStartOfImageMarker())
			{
				imageInfo = null;
				return false;
			}
			bool flag = true;
			while (flag)
			{
				JpegMarker jpegMarker = base.Reader.ReadNextJpegMarker();
				if (JpegHeadersDecoder.ShouldStopDecoding(jpegMarker))
				{
					flag = false;
				}
				else if (!this.TrySkipMarker(jpegMarker))
				{
					jpegMarker.InterpretMarker(this);
				}
			}
			imageInfo = this.CreateJpegImageInfo();
			return true;
		}

		JpegImageInfo CreateJpegImageInfo()
		{
			Guard.ThrowExceptionIfNull<FrameHeader>(base.FrameHeader, "FrameHeader");
			JpegImageInfo jpegImageInfo = new JpegImageInfo();
			jpegImageInfo.Width = base.FrameHeader.Width;
			jpegImageInfo.Height = base.FrameHeader.Height;
			jpegImageInfo.ColorSpace = this.GetColorSpaceInfo();
			jpegImageInfo.HasAdobeInvertedColors = this.HasAdobeInvertedColors(jpegImageInfo.ColorSpace);
			return jpegImageInfo;
		}

		bool HasAdobeInvertedColors(JpegColorSpace imageColorSpace)
		{
			bool flag = imageColorSpace == JpegColorSpace.Cmyk;
			bool flag2 = base.ColorTransform != JpegColorSpace.Undefined;
			return flag && flag2;
		}

		JpegColorSpace GetColorSpaceInfo()
		{
			switch (base.FrameHeader.ImageComponents)
			{
			case 1:
				return JpegColorSpace.Grayscale;
			case 3:
				return JpegColorSpace.Rgb;
			case 4:
				return JpegColorSpace.Cmyk;
			}
			throw new NotSupportedException("Not supported jpeg components count!");
		}

		bool TrySkipMarker(JpegMarker marker)
		{
			if (JpegHeadersDecoder.markerTypesToSkip.Contains(marker.MarkerType))
			{
				JpegMarker.ReadLengthAndSkipData(this);
				return true;
			}
			return false;
		}

		static bool ShouldStopDecoding(JpegMarker marker)
		{
			return JpegHeadersDecoder.endMarkerTypes.Contains(marker.MarkerType);
		}

		static readonly HashSet<JpegMarkerType> endMarkerTypes = new HashSet<JpegMarkerType>();

		static readonly HashSet<JpegMarkerType> markerTypesToSkip;
	}
}
