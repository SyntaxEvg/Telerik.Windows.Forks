using System;

namespace Telerik.Windows.Zip
{
	interface ICompressionAlgorithm
	{
		IBlockTransform CreateCompressor();

		IBlockTransform CreateDecompressor();

		void Initialize(CompressionSettings settings);
	}
}
