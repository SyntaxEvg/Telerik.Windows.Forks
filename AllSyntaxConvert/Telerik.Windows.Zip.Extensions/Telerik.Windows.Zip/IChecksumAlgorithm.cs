using System;

namespace Telerik.Windows.Zip
{
	interface IChecksumAlgorithm
	{
		uint UpdateChecksum(uint checksum, byte[] buffer, int offset, int length);
	}
}
