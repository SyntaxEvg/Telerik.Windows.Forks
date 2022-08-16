using System;
using System.Text;

namespace Telerik.Windows.Zip
{
	abstract class DefaultCryptoTransformBase : BlockTransformBase
	{
		public DefaultCryptoTransformBase()
		{
			base.FixedInputBlockSize = false;
			base.Header.Length = 12;
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
				return 12;
			}
		}

		public override int OutputBlockSize
		{
			get
			{
				return 12;
			}
		}

		protected byte EncodingByte
		{
			get
			{
				uint num = (uint)((ushort)((this.encryptionKeys[2] & 65535U) | 2U));
				return (byte)(num * (num ^ 1U) >> 8);
			}
		}

		public override void CreateHeader()
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			byte[] array = new byte[12];
			random.NextBytes(array);
			if (base.Header.InitData != null)
			{
				array[11] = base.Header.InitData[0];
			}
			byte[] array2 = new byte[12];
			this.TransformBlock(array, 0, 12, array2, 0);
			base.Header.Buffer = array2;
		}

		public override void InitHeaderReading()
		{
			base.Header.BytesToRead = 12;
		}

		public override void ProcessHeader()
		{
			byte[] outputBuffer = new byte[12];
			this.TransformBlock(base.Header.Buffer, 0, base.Header.Buffer.Length, outputBuffer, 0);
			base.Header.BytesToRead = 0;
		}

		internal void InitKeys(string password)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(password);
			foreach (byte byteValue in bytes)
			{
				this.UpdateKeys(byteValue);
			}
		}

		protected override void Dispose(bool disposing)
		{
		}

		protected void UpdateKeys(byte byteValue)
		{
			this.encryptionKeys[0] = Crc32.ComputeCrc32(this.encryptionKeys[0], byteValue);
			this.encryptionKeys[1] = this.encryptionKeys[1] + (uint)((byte)this.encryptionKeys[0]);
			this.encryptionKeys[1] = this.encryptionKeys[1] * 134775813U + 1U;
			this.encryptionKeys[2] = Crc32.ComputeCrc32(this.encryptionKeys[2], (byte)(this.encryptionKeys[1] >> 24));
		}

		protected const int HeaderSize = 12;

		uint[] encryptionKeys = new uint[] { 305419896U, 591751049U, 878082192U };
	}
}
