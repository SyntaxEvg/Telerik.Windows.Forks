using System;

namespace Telerik.Windows.Zip
{
	class StoreTransformBase : BlockTransformBase
	{
		public StoreTransformBase()
		{
			base.FixedInputBlockSize = false;
		}

		public override bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		public override bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		public override int InputBlockSize
		{
			get
			{
				return 4096;
			}
		}

		public override int OutputBlockSize
		{
			get
			{
				return 4096;
			}
		}

		public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			base.ValidateParameters(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset, true);
			for (int i = 0; i < inputCount; i++)
			{
				outputBuffer[outputOffset + i] = inputBuffer[inputOffset + i];
			}
			return inputCount;
		}

		public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			base.ValidateInputBufferParameters(inputBuffer, inputOffset, inputCount, false, true);
			byte[] array = new byte[inputCount];
			this.TransformBlock(inputBuffer, inputOffset, inputCount, array, 0);
			return array;
		}

		protected override void Dispose(bool disposing)
		{
		}
	}
}
