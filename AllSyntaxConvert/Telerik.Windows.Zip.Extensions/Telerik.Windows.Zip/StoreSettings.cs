using System;

namespace Telerik.Windows.Zip
{
	public class StoreSettings : CompressionSettings
	{
		public StoreSettings()
		{
			base.Method = CompressionMethod.Stored;
		}
	}
}
