using System;

namespace Telerik.Windows.Zip
{
	class Adler32 : IChecksumAlgorithm
	{
		public uint UpdateChecksum(uint checksum, byte[] buffer, int offset, int length)
		{
			if (checksum == 1U || buffer == null)
			{
				checksum = 1U;
			}
			if (buffer != null)
			{
				OperationStream.ValidateBufferParameters(buffer, offset, length, true);
				uint num = checksum & 65535U;
				uint num2 = (checksum >> 16) & 65535U;
				while (length > 0)
				{
					int num3 = ((length < 5552) ? length : 5552);
					length -= num3;
					for (int i = 0; i < num3; i++)
					{
						num += (uint)(buffer[offset++] & byte.MaxValue);
						num2 += num;
					}
					num %= 65521U;
					num2 %= 65521U;
				}
				checksum = (num2 << 16) | num;
			}
			return checksum;
		}

		const uint Base = 65521U;

		const int MaxIterations = 5552;
	}
}
