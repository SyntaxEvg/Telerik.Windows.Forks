using System;

namespace Telerik.Windows.Zip
{
	enum ExtraFieldType
	{
		Unknown = -1,
		Zip64 = 1,
		Ntfs = 10,
		StrongEncryption = 23,
		UnixTime = 21589,
		AesEncryption = 39169
	}
}
