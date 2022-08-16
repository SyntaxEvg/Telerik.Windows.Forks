using System;
using System.IO;

namespace Telerik.Windows.Zip
{
	static class ZipHelper
	{
		internal static void CopyStream(Stream input, Stream output, long bytes)
		{
			int num = 32768;
			byte[] buffer = new byte[num];
			int num2;
			while (bytes > 0L && (num2 = input.Read(buffer, 0, (int)Math.Min((long)num, bytes))) > 0)
			{
				output.Write(buffer, 0, num2);
				bytes -= (long)num2;
			}
		}

		internal static uint DateTimeToPacked(DateTime time)
		{
			ushort num = (ushort)((time.Day & 31) | ((time.Month << 5) & 480) | ((time.Year - 1980 << 9) & 65024));
			ushort num2 = (ushort)(((time.Second / 2) & 31) | ((time.Minute << 5) & 2016) | ((time.Hour << 11) & 63488));
			return (uint)(((int)num << 16) | (int)num2);
		}

		internal static ICompressionAlgorithm GetCompressionAlgorithm(CompressionSettings settings)
		{
			CompressionMethod method = settings.Method;
			ICompressionAlgorithm compressionAlgorithm;
			if (method != CompressionMethod.Stored)
			{
				if (method != CompressionMethod.Deflate)
				{
					if (method != CompressionMethod.Lzma)
					{
						throw new NotSupportedException(string.Format("Compression method {0} is not supported.", settings));
					}
					compressionAlgorithm = new LzmaAlgorithm();
				}
				else
				{
					compressionAlgorithm = new DeflateAlgorithm();
				}
			}
			else
			{
				compressionAlgorithm = new StoreAlgorithm();
			}
			if (compressionAlgorithm != null)
			{
				compressionAlgorithm.Initialize(settings);
			}
			return compressionAlgorithm;
		}

		internal static CompressionSettings GetCompressionSettings(CompressionMethod method, CompressionSettings baseSettings)
		{
			CompressionSettings compressionSettings;
			if (method != CompressionMethod.Stored)
			{
				if (method != CompressionMethod.Deflate)
				{
					if (method != CompressionMethod.Lzma)
					{
						throw new NotSupportedException(string.Format("Compression method {0} is not supported.", method));
					}
					compressionSettings = new LzmaSettings();
				}
				else
				{
					compressionSettings = new DeflateSettings();
				}
			}
			else
			{
				compressionSettings = new StoreSettings();
			}
			if (baseSettings != null && compressionSettings.GetType() == baseSettings.GetType())
			{
				compressionSettings.CopyFrom(baseSettings);
			}
			return compressionSettings;
		}

		internal static bool EndsWithDirChar(string path)
		{
			string text = path.Trim();
			return text.EndsWith(PlatformSettings.Manager.DirectorySeparatorChar) || text.EndsWith(PlatformSettings.Manager.AltDirectorySeparatorChar);
		}

		internal static bool IsCompressionMethodSupported(CompressionMethod method)
		{
			return method == CompressionMethod.Deflate || method == CompressionMethod.Stored || method == CompressionMethod.Lzma;
		}

		internal static DateTime PackedToDateTime(uint packedDateTime)
		{
			if (packedDateTime == 65535U || packedDateTime == 0U)
			{
				return new DateTime(1995, 1, 1, 0, 0, 0, 0);
			}
			ushort num = (ushort)(packedDateTime & 65535U);
			ushort num2 = (ushort)((packedDateTime & 4294901760U) >> 16);
			int year = 1980 + ((num2 & 65024) >> 9);
			int month = (num2 & 480) >> 5;
			int num3 = (int)(num2 & 31);
			int num4 = (num & 63488) >> 11;
			int num5 = (num & 2016) >> 5;
			int num6 = (int)((num & 31) * 2);
			if (num6 >= 60)
			{
				num5 += num6 / 60;
				num6 %= 60;
			}
			if (num5 >= 60)
			{
				num4 += num5 / 60;
				num5 %= 60;
			}
			if (num4 >= 24)
			{
				num3 += num4 / 24;
				num4 %= 24;
			}
			DateTime result = DateTime.Now;
			try
			{
				result = new DateTime(year, month, num3, num4, num5, num6, 0);
			}
			catch (ArgumentOutOfRangeException)
			{
				result = ZipHelper.InvalidDateTime;
			}
			catch (ArgumentException)
			{
				result = ZipHelper.InvalidDateTime;
			}
			return result;
		}

		internal static void ReadBytes(Stream stream, byte[] buffer, int bytesToRead)
		{
			int num = 0;
			while (bytesToRead > 0)
			{
				int num2 = stream.Read(buffer, num, bytesToRead);
				if (num2 == 0)
				{
					throw new IOException("Unexpected End Of Stream");
				}
				num += num2;
				bytesToRead -= num2;
			}
		}

		internal static bool SeekBackwardsToSignature(Stream stream, uint signatureToFind)
		{
			int num = 0;
			uint num2 = 0U;
			byte[] array = new byte[32];
			bool flag = false;
			bool flag2 = false;
			while (!flag2 && !flag)
			{
				flag = ZipHelper.SeekBackwardsAndRead(stream, array, out num);
				while (num >= 0 && !flag2)
				{
					num2 = (num2 << 8) | (uint)array[num];
					if (num2 != signatureToFind)
					{
						num--;
					}
					else
					{
						flag2 = true;
					}
				}
			}
			if (!flag2)
			{
				return false;
			}
			stream.Seek((long)num, SeekOrigin.Current);
			return true;
		}

		static bool SeekBackwardsAndRead(Stream stream, byte[] buffer, out int bufferPointer)
		{
			if (stream.Position < (long)buffer.Length)
			{
				int num = (int)stream.Position;
				stream.Seek(0L, SeekOrigin.Begin);
				ZipHelper.ReadBytes(stream, buffer, num);
				stream.Seek(0L, SeekOrigin.Begin);
				bufferPointer = num - 1;
				return true;
			}
			stream.Seek((long)(-(long)buffer.Length), SeekOrigin.Current);
			ZipHelper.ReadBytes(stream, buffer, buffer.Length);
			stream.Seek((long)(-(long)buffer.Length), SeekOrigin.Current);
			bufferPointer = buffer.Length - 1;
			return false;
		}

		internal static readonly DateTime InvalidDateTime = new DateTime(1980, 1, 1, 0, 0, 0, 0);
	}
}
