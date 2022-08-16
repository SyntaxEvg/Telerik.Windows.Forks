using System;

namespace Telerik.Windows.Zip
{
	class StoreAlgorithm : ICompressionAlgorithm
	{
		public IBlockTransform CreateCompressor()
		{
			return new StoreCompressor();
		}

		public IBlockTransform CreateDecompressor()
		{
			return new StoreDecompressor();
		}

		public void Initialize(CompressionSettings settings)
		{
		}
	}
}
