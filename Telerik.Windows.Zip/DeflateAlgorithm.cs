using System;

namespace Telerik.Windows.Zip
{
	class DeflateAlgorithm : ICompressionAlgorithm
	{
		public IBlockTransform CreateCompressor()
		{
			return new DeflateCompressor(this.deflateSettings);
		}

		public IBlockTransform CreateDecompressor()
		{
			return new DeflateDecompressor(this.deflateSettings);
		}

		public void Initialize(CompressionSettings settings)
		{
			this.deflateSettings = settings as DeflateSettings;
			if (this.deflateSettings == null)
			{
				throw new ArgumentException("Wrong settings type.");
			}
		}

		DeflateSettings deflateSettings;
	}
}
