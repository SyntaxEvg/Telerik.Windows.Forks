using System;

namespace Telerik.Windows.Zip
{
	[Flags]
	enum GeneralPurposeBitFlag : ushort
	{
		FileIsEncrypted = 1,
		ZeroLocalHeader = 8,
		ReservedForEnhancedDeflating = 16,
		CompressedPatchedData = 32,
		StrongEncryption = 64,
		EncodingUtf8 = 2048,
		HideLocalHeader = 8192
	}
}
