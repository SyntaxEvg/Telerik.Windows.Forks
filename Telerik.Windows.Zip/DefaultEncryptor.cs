using System;

namespace Telerik.Windows.Zip
{
	class DefaultEncryptor : DefaultCryptoTransformBase
	{
		public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			base.ValidateParameters(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset, true);
			for (int i = 0; i < inputCount; i++)
			{
				byte b = inputBuffer[inputOffset + i];
				outputBuffer[outputOffset + i] = (byte)(b ^ base.EncodingByte);
				base.UpdateKeys(b);
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
	}
}
