using System;
using System.IO;

namespace Telerik.Windows.Zip
{
	abstract class CompressionTransformBase : BlockTransformBase
	{
		public CompressionTransformBase()
		{
			base.FixedInputBlockSize = false;
		}

		public override bool CanReuseTransform
		{
			get
			{
				return false;
			}
		}

		public override bool CanTransformMultipleBlocks
		{
			get
			{
				return false;
			}
		}

		public override int InputBlockSize
		{
			get
			{
				return 8192;
			}
		}

		public override int OutputBlockSize
		{
			get
			{
				return 32768;
			}
		}

		protected int AvailableBytesIn { get; set; }

		protected byte[] InputBuffer { get; set; }

		protected int NextIn { get; set; }

		protected int TotalBytesIn { get; set; }

		protected int AvailableBytesOut { get; set; }

		protected byte[] OutputBuffer { get; set; }

		protected int NextOut { get; set; }

		protected int TotalBytesOut { get; set; }

		public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			base.ValidateParameters(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset, true);
			this.AvailableBytesIn = inputCount;
			this.InputBuffer = inputBuffer;
			this.AvailableBytesOut = outputBuffer.Length - outputOffset;
			this.OutputBuffer = outputBuffer;
			this.NextOut = outputOffset;
			this.NextIn = inputOffset;
			this.ProcessTransform(false);
			return this.NextOut - outputOffset;
		}

		public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			base.ValidateInputBufferParameters(inputBuffer, inputOffset, inputCount, false, true);
			this.AvailableBytesIn = inputCount;
			this.InputBuffer = inputBuffer;
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				bool flag;
				do
				{
					byte[] array = new byte[this.OutputBlockSize];
					this.AvailableBytesOut = array.Length;
					this.OutputBuffer = array;
					this.NextOut = 0;
					this.NextIn = 0;
					flag = this.ProcessTransform(true);
					this.AvailableBytesIn = 0;
					memoryStream.Write(array, 0, this.NextOut);
				}
				while (flag);
				result = memoryStream.ToArray();
			}
			return result;
		}

		protected override void Dispose(bool disposing)
		{
		}

		protected abstract bool ProcessTransform(bool finalBlock);
	}
}
