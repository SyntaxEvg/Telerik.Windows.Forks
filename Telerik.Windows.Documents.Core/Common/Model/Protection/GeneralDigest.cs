﻿using System;

namespace Telerik.Windows.Documents.Common.Model.Protection
{
	abstract class GeneralDigest : IDigest
	{
		internal GeneralDigest()
		{
			this.xBuf = new byte[4];
		}

		internal GeneralDigest(GeneralDigest t)
		{
			this.xBuf = new byte[t.xBuf.Length];
			Array.Copy(t.xBuf, 0, this.xBuf, 0, t.xBuf.Length);
			this.xBufOff = t.xBufOff;
			this.byteCount = t.byteCount;
		}

		public void Update(byte input)
		{
			this.xBuf[this.xBufOff++] = input;
			if (this.xBufOff == this.xBuf.Length)
			{
				this.ProcessWord(this.xBuf, 0);
				this.xBufOff = 0;
			}
			this.byteCount += 1L;
		}

		public void BlockUpdate(byte[] input, int inOff, int length)
		{
			while (this.xBufOff != 0)
			{
				if (length <= 0)
				{
					break;
				}
				this.Update(input[inOff]);
				inOff++;
				length--;
			}
			while (length > this.xBuf.Length)
			{
				this.ProcessWord(input, inOff);
				inOff += this.xBuf.Length;
				length -= this.xBuf.Length;
				this.byteCount += (long)this.xBuf.Length;
			}
			while (length > 0)
			{
				this.Update(input[inOff]);
				inOff++;
				length--;
			}
		}

		public void Finish()
		{
			long bitLength = this.byteCount << 3;
			this.Update(128);
			while (this.xBufOff != 0)
			{
				this.Update(0);
			}
			this.ProcessLength(bitLength);
			this.ProcessBlock();
		}

		public virtual void Reset()
		{
			this.byteCount = 0L;
			this.xBufOff = 0;
			Array.Clear(this.xBuf, 0, this.xBuf.Length);
		}

		public int GetByteLength()
		{
			return 64;
		}

		internal abstract void ProcessWord(byte[] input, int inOff);

		internal abstract void ProcessLength(long bitLength);

		internal abstract void ProcessBlock();

		public abstract string AlgorithmName { get; }

		public abstract int GetDigestSize();

		public abstract int DoFinal(byte[] output, int outOff);

		const int ByteLength = 64;

		readonly byte[] xBuf;

		int xBufOff;

		long byteCount;
	}
}
