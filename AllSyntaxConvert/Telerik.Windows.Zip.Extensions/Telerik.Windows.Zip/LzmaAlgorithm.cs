using System;

namespace Telerik.Windows.Zip
{
	class LzmaAlgorithm : ICompressionAlgorithm
	{
		public IBlockTransform CreateCompressor()
		{
			return new LzmaCompressor(this.lzmaSettings);
		}

		public IBlockTransform CreateDecompressor()
		{
			return new LzmaDecompressor(this.lzmaSettings);
		}

		public void Initialize(CompressionSettings settings)
		{
			this.lzmaSettings = settings as LzmaSettings;
			if (this.lzmaSettings == null)
			{
				throw new ArgumentException("Wrong settings type.");
			}
		}

		LzmaSettings lzmaSettings;
	}
}
