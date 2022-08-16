using System;

namespace Telerik.Windows.Zip
{
	class DeflateConfiguration
	{
		DeflateConfiguration(int goodLength, int maxLazy, int niceLength, int maxChainLength)
		{
			this.GoodLength = goodLength;
			this.MaxLazy = maxLazy;
			this.NiceLength = niceLength;
			this.MaxChainLength = maxChainLength;
		}

		internal int GoodLength { get; set; }

		internal int MaxLazy { get; set; }

		internal int NiceLength { get; set; }

		internal int MaxChainLength { get; set; }

		public static DeflateConfiguration Lookup(int compressionLevel)
		{
			return DeflateConfiguration.table[compressionLevel];
		}

		static readonly DeflateConfiguration[] table = new DeflateConfiguration[]
		{
			new DeflateConfiguration(0, 0, 0, 0),
			new DeflateConfiguration(4, 4, 8, 4),
			new DeflateConfiguration(4, 5, 16, 8),
			new DeflateConfiguration(4, 6, 32, 32),
			new DeflateConfiguration(4, 4, 16, 16),
			new DeflateConfiguration(8, 16, 32, 32),
			new DeflateConfiguration(8, 16, 128, 128),
			new DeflateConfiguration(8, 32, 128, 256),
			new DeflateConfiguration(32, 128, 258, 1024),
			new DeflateConfiguration(32, 258, 258, 4096)
		};
	}
}
