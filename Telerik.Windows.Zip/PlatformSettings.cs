using System;

namespace Telerik.Windows.Zip
{
	public static class PlatformSettings
	{
		public static IPlatformManager Manager
		{
			get
			{
				return PlatformSettings.manager;
			}
			set
			{
				PlatformSettings.manager = value;
			}
		}

		static IPlatformManager manager = new DefaultPlatformManager();
	}
}
