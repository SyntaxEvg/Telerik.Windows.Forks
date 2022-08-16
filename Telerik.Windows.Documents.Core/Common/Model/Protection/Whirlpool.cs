using System;

namespace Telerik.Windows.Documents.Common.Model.Protection
{
	sealed class Whirlpool : IDigest, IProtectionAlgorithm
	{
		static Whirlpool()
		{
			Whirlpool.eight[31] = 8;
			for (int i = 0; i < 256; i++)
			{
				int num = Whirlpool.sbox[i];
				int num2 = Whirlpool.MaskWithReductionPolynomial(num << 1);
				int num3 = Whirlpool.MaskWithReductionPolynomial(num2 << 1);
				int num4 = num3 ^ num;
				int num5 = Whirlpool.MaskWithReductionPolynomial(num3 << 1);
				int num6 = num5 ^ num;
				Whirlpool.c0[i] = Whirlpool.PackIntoLong(num, num, num3, num, num5, num4, num2, num6);
				Whirlpool.c1[i] = Whirlpool.PackIntoLong(num6, num, num, num3, num, num5, num4, num2);
				Whirlpool.c2[i] = Whirlpool.PackIntoLong(num2, num6, num, num, num3, num, num5, num4);
				Whirlpool.c3[i] = Whirlpool.PackIntoLong(num4, num2, num6, num, num, num3, num, num5);
				Whirlpool.c4[i] = Whirlpool.PackIntoLong(num5, num4, num2, num6, num, num, num3, num);
				Whirlpool.c5[i] = Whirlpool.PackIntoLong(num, num5, num4, num2, num6, num, num, num3);
				Whirlpool.c6[i] = Whirlpool.PackIntoLong(num3, num, num5, num4, num2, num6, num, num);
				Whirlpool.c7[i] = Whirlpool.PackIntoLong(num, num3, num, num5, num4, num2, num6, num);
			}
		}

		public Whirlpool()
		{
			this.rc[0] = 0L;
			for (int i = 1; i <= 10; i++)
			{
				int num = 8 * (i - 1);
				this.rc[i] = (Whirlpool.c0[num] & -72057594037927936L) ^ (Whirlpool.c1[num + 1] & 71776119061217280L) ^ (Whirlpool.c2[num + 2] & 280375465082880L) ^ (Whirlpool.c3[num + 3] & 1095216660480L) ^ (Whirlpool.c4[num + 4] & (long)-16777216) ^ (Whirlpool.c5[num + 5] & 16711680L) ^ (Whirlpool.c6[num + 6] & 65280L) ^ (Whirlpool.c7[num + 7] & 255L);
			}
		}

		static long PackIntoLong(int b7, int b6, int b5, int b4, int b3, int b2, int b1, int b0)
		{
			return ((long)b7 << 56) ^ ((long)b6 << 48) ^ ((long)b5 << 40) ^ ((long)b4 << 32) ^ ((long)b3 << 24) ^ ((long)b2 << 16) ^ ((long)b1 << 8) ^ (long)b0;
		}

		static int MaskWithReductionPolynomial(int input)
		{
			int num = input;
			if ((long)num >= 256L)
			{
				num ^= 285;
			}
			return num;
		}

		public Whirlpool(Whirlpool originalDigest)
		{
			Array.Copy(originalDigest.rc, 0, this.rc, 0, this.rc.Length);
			Array.Copy(originalDigest.buffer, 0, this.buffer, 0, this.buffer.Length);
			this.bufferPosition = originalDigest.bufferPosition;
			Array.Copy(originalDigest.bitCount, 0, this.bitCount, 0, this.bitCount.Length);
			Array.Copy(originalDigest.hash, 0, this.hash, 0, this.hash.Length);
			Array.Copy(originalDigest.k, 0, this.k, 0, this.k.Length);
			Array.Copy(originalDigest.l, 0, this.l, 0, this.l.Length);
			Array.Copy(originalDigest.block, 0, this.block, 0, this.block.Length);
			Array.Copy(originalDigest.state, 0, this.state, 0, this.state.Length);
		}

		public string AlgorithmName
		{
			get
			{
				return "WHIRLPOOL";
			}
		}

		public int GetDigestSize()
		{
			return 64;
		}

		public int DoFinal(byte[] output, int outOff)
		{
			this.Finish();
			for (int i = 0; i < 8; i++)
			{
				Whirlpool.ConvertLongToByteArray(this.hash[i], output, outOff + i * 8);
			}
			this.Reset();
			return this.GetDigestSize();
		}

		public void Reset()
		{
			this.bufferPosition = 0;
			Array.Clear(this.bitCount, 0, this.bitCount.Length);
			Array.Clear(this.buffer, 0, this.buffer.Length);
			Array.Clear(this.hash, 0, this.hash.Length);
			Array.Clear(this.k, 0, this.k.Length);
			Array.Clear(this.l, 0, this.l.Length);
			Array.Clear(this.block, 0, this.block.Length);
			Array.Clear(this.state, 0, this.state.Length);
		}

		void ProcessFilledBuffer()
		{
			for (int i = 0; i < this.state.Length; i++)
			{
				this.block[i] = Whirlpool.BytesToLongFromBuffer(this.buffer, i * 8);
			}
			this.ProcessBlock();
			this.bufferPosition = 0;
			Array.Clear(this.buffer, 0, this.buffer.Length);
		}

		static long BytesToLongFromBuffer(byte[] buffer, int startPos)
		{
			return (long)((((ulong)buffer[startPos] & 255UL) << 56) | (((ulong)buffer[startPos + 1] & 255UL) << 48) | (((ulong)buffer[startPos + 2] & 255UL) << 40) | (((ulong)buffer[startPos + 3] & 255UL) << 32) | (((ulong)buffer[startPos + 4] & 255UL) << 24) | (((ulong)buffer[startPos + 5] & 255UL) << 16) | (((ulong)buffer[startPos + 6] & 255UL) << 8) | ((ulong)buffer[startPos + 7] & 255UL));
		}

		static void ConvertLongToByteArray(long inputLong, byte[] outputArray, int offSet)
		{
			for (int i = 0; i < 8; i++)
			{
				outputArray[offSet + i] = (byte)((inputLong >> 56 - i * 8) & 255L);
			}
		}

		void ProcessBlock()
		{
			for (int i = 0; i < 8; i++)
			{
				this.state[i] = this.block[i] ^ (this.k[i] = this.hash[i]);
			}
			for (int j = 1; j <= 10; j++)
			{
				for (int k = 0; k < 8; k++)
				{
					this.l[k] = 0L;
					this.l[k] ^= Whirlpool.c0[(int)(this.k[k & 7] >> 56) & 255];
					this.l[k] ^= Whirlpool.c1[(int)(this.k[(k - 1) & 7] >> 48) & 255];
					this.l[k] ^= Whirlpool.c2[(int)(this.k[(k - 2) & 7] >> 40) & 255];
					this.l[k] ^= Whirlpool.c3[(int)(this.k[(k - 3) & 7] >> 32) & 255];
					this.l[k] ^= Whirlpool.c4[(int)(this.k[(k - 4) & 7] >> 24) & 255];
					this.l[k] ^= Whirlpool.c5[(int)(this.k[(k - 5) & 7] >> 16) & 255];
					this.l[k] ^= Whirlpool.c6[(int)(this.k[(k - 6) & 7] >> 8) & 255];
					this.l[k] ^= Whirlpool.c7[(int)this.k[(k - 7) & 7] & 255];
				}
				Array.Copy(this.l, 0, this.k, 0, this.k.Length);
				this.k[0] ^= this.rc[j];
				for (int l = 0; l < 8; l++)
				{
					this.l[l] = this.k[l];
					this.l[l] ^= Whirlpool.c0[(int)(this.state[l & 7] >> 56) & 255];
					this.l[l] ^= Whirlpool.c1[(int)(this.state[(l - 1) & 7] >> 48) & 255];
					this.l[l] ^= Whirlpool.c2[(int)(this.state[(l - 2) & 7] >> 40) & 255];
					this.l[l] ^= Whirlpool.c3[(int)(this.state[(l - 3) & 7] >> 32) & 255];
					this.l[l] ^= Whirlpool.c4[(int)(this.state[(l - 4) & 7] >> 24) & 255];
					this.l[l] ^= Whirlpool.c5[(int)(this.state[(l - 5) & 7] >> 16) & 255];
					this.l[l] ^= Whirlpool.c6[(int)(this.state[(l - 6) & 7] >> 8) & 255];
					this.l[l] ^= Whirlpool.c7[(int)this.state[(l - 7) & 7] & 255];
				}
				Array.Copy(this.l, 0, this.state, 0, this.state.Length);
			}
			for (int m = 0; m < 8; m++)
			{
				this.hash[m] ^= this.state[m] ^ this.block[m];
			}
		}

		public void Update(byte input)
		{
			this.buffer[this.bufferPosition] = input;
			this.bufferPosition++;
			if (this.bufferPosition == this.buffer.Length)
			{
				this.ProcessFilledBuffer();
			}
			this.Increment();
		}

		void Increment()
		{
			int num = 0;
			for (int i = this.bitCount.Length - 1; i >= 0; i--)
			{
				int num2 = (int)((this.bitCount[i] & 255) + Whirlpool.eight[i]) + num;
				num = num2 >> 8;
				this.bitCount[i] = (short)(num2 & 255);
			}
		}

		public void BlockUpdate(byte[] input, int inOff, int length)
		{
			while (length > 0)
			{
				this.Update(input[inOff]);
				inOff++;
				length--;
			}
		}

		void Finish()
		{
			byte[] array = this.CopyBitLength();
			byte[] array2 = this.buffer;
			int num = this.bufferPosition++;
			array2[num] |= 128;
			if (this.bufferPosition == this.buffer.Length)
			{
				this.ProcessFilledBuffer();
			}
			if (this.bufferPosition > 32)
			{
				while (this.bufferPosition != 0)
				{
					this.Update(0);
				}
			}
			while (this.bufferPosition <= 32)
			{
				this.Update(0);
			}
			Array.Copy(array, 0, this.buffer, 32, array.Length);
			this.ProcessFilledBuffer();
		}

		byte[] CopyBitLength()
		{
			byte[] array = new byte[32];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (byte)(this.bitCount[i] & 255);
			}
			return array;
		}

		public int GetByteLength()
		{
			return 64;
		}

		public byte[] ComputeHash(byte[] buffer)
		{
			byte[] array = new byte[this.GetDigestSize()];
			this.BlockUpdate(buffer, 0, buffer.Length);
			this.DoFinal(array, 0);
			return array;
		}

		public const string Name = "WHIRLPOOL";

		const int ByteLength = 64;

		const int DigestLengthBytes = 64;

		const int Rounds = 10;

		const int ReductionPolynomial = 285;

		const int BitcountArraySize = 32;

		static readonly int[] sbox = new int[]
		{
			24, 35, 198, 232, 135, 184, 1, 79, 54, 166,
			210, 245, 121, 111, 145, 82, 96, 188, 155, 142,
			163, 12, 123, 53, 29, 224, 215, 194, 46, 75,
			254, 87, 21, 119, 55, 229, 159, 240, 74, 218,
			88, 201, 41, 10, 177, 160, 107, 133, 189, 93,
			16, 244, 203, 62, 5, 103, 228, 39, 65, 139,
			167, 125, 149, 216, 251, 238, 124, 102, 221, 23,
			71, 158, 202, 45, 191, 7, 173, 90, 131, 51,
			99, 2, 170, 113, 200, 25, 73, 217, 242, 227,
			91, 136, 154, 38, 50, 176, 233, 15, 213, 128,
			190, 205, 52, 72, 255, 122, 144, 95, 32, 104,
			26, 174, 180, 84, 147, 34, 100, 241, 115, 18,
			64, 8, 195, 236, 219, 161, 141, 61, 151, 0,
			207, 43, 118, 130, 214, 27, 181, 175, 106, 80,
			69, 243, 48, 239, 63, 85, 162, 234, 101, 186,
			47, 192, 222, 28, 253, 77, 146, 117, 6, 138,
			178, 230, 14, 31, 98, 212, 168, 150, 249, 197,
			37, 89, 132, 114, 57, 76, 94, 120, 56, 140,
			209, 165, 226, 97, 179, 33, 156, 30, 67, 199,
			252, 4, 81, 153, 109, 13, 250, 223, 126, 36,
			59, 171, 206, 17, 143, 78, 183, 235, 60, 129,
			148, 247, 185, 19, 44, 211, 231, 110, 196, 3,
			86, 68, 127, 169, 42, 187, 193, 83, 220, 11,
			157, 108, 49, 116, 246, 70, 172, 137, 20, 225,
			22, 58, 105, 9, 112, 182, 208, 237, 204, 66,
			152, 164, 40, 92, 248, 134
		};

		static readonly long[] c0 = new long[256];

		static readonly long[] c1 = new long[256];

		static readonly long[] c2 = new long[256];

		static readonly long[] c3 = new long[256];

		static readonly long[] c4 = new long[256];

		static readonly long[] c5 = new long[256];

		static readonly long[] c6 = new long[256];

		static readonly long[] c7 = new long[256];

		readonly long[] rc = new long[11];

		static readonly short[] eight = new short[32];

		readonly byte[] buffer = new byte[64];

		int bufferPosition;

		readonly short[] bitCount = new short[32];

		readonly long[] hash = new long[8];

		readonly long[] k = new long[8];

		readonly long[] l = new long[8];

		readonly long[] block = new long[8];

		readonly long[] state = new long[8];
	}
}
