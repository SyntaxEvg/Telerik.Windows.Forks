using System;

namespace Telerik.Windows.Zip
{
	enum VersionNeededToExtract : ushort
	{
		Default = 10,
		Deflate = 20,
		ExplicitDirectory = 20,
		TraditionalEncryption = 20,
		Zip64 = 45
	}
}
