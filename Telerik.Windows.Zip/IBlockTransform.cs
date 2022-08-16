using System;

namespace Telerik.Windows.Zip
{
	public interface IBlockTransform : IDisposable
	{
		bool CanReuseTransform { get; }

		bool CanTransformMultipleBlocks { get; }

		TransformationHeader Header { get; }

		int InputBlockSize { get; }

		int OutputBlockSize { get; }

		void CreateHeader();

		void InitHeaderReading();

		void ProcessHeader();

		int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset);

		byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount);
	}
}
