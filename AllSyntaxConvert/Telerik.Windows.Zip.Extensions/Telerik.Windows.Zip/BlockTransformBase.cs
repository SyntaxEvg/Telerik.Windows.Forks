using System;

namespace Telerik.Windows.Zip
{
	public abstract class BlockTransformBase : IBlockTransform, IDisposable
	{
		public abstract bool CanReuseTransform { get; }

		public abstract bool CanTransformMultipleBlocks { get; }

		public TransformationHeader Header
		{
			get
			{
				return this.header;
			}
		}

		public abstract int InputBlockSize { get; }

		public abstract int OutputBlockSize { get; }

		protected bool FixedInputBlockSize { get; set; }

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual void CreateHeader()
		{
		}

		public virtual void InitHeaderReading()
		{
		}

		public virtual void ProcessHeader()
		{
			this.Header.BytesToRead = 0;
		}

		public abstract int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset);

		public abstract byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount);

		protected abstract void Dispose(bool disposing);

		protected void ValidateInputBufferParameters(byte[] inputBuffer, int inputOffset, int inputCount, bool validateBlockSize, bool allowZeroCount)
		{
			OperationStream.ValidateBufferParameters(inputBuffer, inputOffset, inputCount, allowZeroCount);
			if (this.FixedInputBlockSize && validateBlockSize && inputCount % this.InputBlockSize != 0)
			{
				throw new ArgumentException("Invalid value.", "inputCount");
			}
		}

		protected void ValidateParameters(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset, bool allowZeroCount)
		{
			this.ValidateInputBufferParameters(inputBuffer, inputOffset, inputCount, true, allowZeroCount);
			OperationStream.ValidateBufferParameters(outputBuffer, outputOffset, 0, true);
		}

		TransformationHeader header = new TransformationHeader();
	}
}
