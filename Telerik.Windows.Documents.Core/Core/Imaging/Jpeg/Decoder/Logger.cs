using System;
using System.Collections.Generic;
using System.Globalization;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder
{
	static class Logger
	{
		static Logger()
		{
			Logger.markerNames["ffd8"] = "SOI - start of image";
			Logger.markerNames["ffc0"] = "SOF0 - start of frame Baseline DCT";
			Logger.markerNames["ffc1"] = "SOF1 - start of frame Extended DCT";
			Logger.markerNames["ffc2"] = "SOF2 - start of frame Progressive DCT";
			Logger.markerNames["ffc4"] = "DHT - define huffman table";
			Logger.markerNames["ffdb"] = "DQT - define quantization table";
			Logger.markerNames["ffdd"] = "DRI - define restart interval";
			Logger.markerNames["ffda"] = "SOS - start of scan";
			for (int i = 0; i < 8; i++)
			{
				Logger.markerNames["ffd" + i] = "RST" + i + " - restart of scan";
			}
			for (int j = 0; j < 16; j++)
			{
				string str = string.Format("{0:X}", j);
				Logger.markerNames["ffe" + str] = "APP" + str + " - application specific";
			}
			Logger.markerNames["fffe"] = "COM - comment";
			Logger.markerNames["ffd9"] = "EOI - end of image";
		}

		public static void Log(string text)
		{
			Logger.log(text);
		}

		public static void LogMarker(byte code)
		{
			string text = string.Format("ff{0}", Logger.ToHex((int)code));
			string arg;
			if (!Logger.markerNames.TryGetValue(text, out arg))
			{
				arg = "unknown marker";
			}
			Logger.log(string.Format("marker: {0} ({1})", text, arg));
		}

		public static string ToHex(int b)
		{
			string text = string.Format("{0:X}", b).ToLower(CultureInfo.InvariantCulture);
			if ((text.Length & 1) == 1)
			{
				text = "0" + text;
			}
			return text;
		}

		static readonly Action<string> log = delegate(string s)
		{
		};

		static readonly Dictionary<string, string> markerNames = new Dictionary<string, string>();
	}
}
