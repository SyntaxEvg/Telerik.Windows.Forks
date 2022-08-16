using System;
using System.ComponentModel;

namespace Telerik.Windows.Zip
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("This class has been deprecated. Use ZipArchive instead of ZipPackage.")]
	public enum ZipCompression
	{
		Default = -1,
		Stored,
		BestSpeed,
		Method2,
		Method3,
		Method4,
		Method5,
		Method6,
		Method7,
		Deflated,
		Deflate64
	}
}
