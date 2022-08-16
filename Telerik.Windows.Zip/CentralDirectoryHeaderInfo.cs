using System;

namespace Telerik.Windows.Zip
{
	class CentralDirectoryHeaderInfo : FileHeaderInfoBase
	{
		public CentralDirectoryHeaderInfo(CentralDirectoryHeader header)
			: base(header)
		{
			this.LocalHeaderOffsetOverflow = header.LocalHeaderOffset == uint.MaxValue;
		}

		public bool LocalHeaderOffsetOverflow { get; set; }
	}
}
