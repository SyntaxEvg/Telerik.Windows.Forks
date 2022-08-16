using System;
using System.ComponentModel;

namespace Telerik.Windows.Zip
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This class has been deprecated. Use ZipArchive instead of ZipPackage.")]
	public enum CompressionType
	{
		Default,
		Lzma
	}
}
