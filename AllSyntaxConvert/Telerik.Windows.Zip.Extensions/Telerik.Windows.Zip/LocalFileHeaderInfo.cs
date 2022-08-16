using System;

namespace Telerik.Windows.Zip
{
	class LocalFileHeaderInfo : FileHeaderInfoBase
	{
		public LocalFileHeaderInfo(LocalFileHeader header)
			: base(header)
		{
		}
	}
}
