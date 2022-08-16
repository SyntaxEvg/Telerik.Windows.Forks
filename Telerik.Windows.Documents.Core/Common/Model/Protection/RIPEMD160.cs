using System;
using System.Security.Cryptography;

namespace Telerik.Windows.Documents.Common.Model.Protection
{
	class RIPEMD160 : GeneralDigest, IProtectionAlgorithm
	{
		public RIPEMD160()
		{
			this.InitializeHs();
		}

		public RIPEMD160(RIPEMD160 t)
			: base(t)
		{
			this.H0 = t.H0;
			this.H1 = t.H1;
			this.H2 = t.H2;
			this.H3 = t.H3;
			this.H4 = t.H4;
			Array.Copy(t.X, 0, this.X, 0, t.X.Length);
			this.xOff = t.xOff;
		}

		public byte[] ComputeHash(byte[] buffer)
		{
			return RIPEMD160.algorithm.ComputeHash(buffer);
		}

		public override string AlgorithmName
		{
			get
			{
				return "RIPEMD-160";
			}
		}

		public override int GetDigestSize()
		{
			return 20;
		}

		internal override void ProcessWord(byte[] input, int inOff)
		{
			this.X[this.xOff++] = (int)(input[inOff] & byte.MaxValue) | ((int)(input[inOff + 1] & byte.MaxValue) << 8) | ((int)(input[inOff + 2] & byte.MaxValue) << 16) | ((int)(input[inOff + 3] & byte.MaxValue) << 24);
			if (this.xOff == 16)
			{
				this.ProcessBlock();
			}
		}

		internal override void ProcessLength(long bitLength)
		{
			if (this.xOff > 14)
			{
				this.ProcessBlock();
			}
			this.X[14] = (int)(bitLength & (long)-1);
			this.X[15] = (int)((ulong)bitLength >> 32);
		}

		static void UnpackWord(int word, byte[] outBytes, int outOff)
		{
			outBytes[outOff] = (byte)word;
			outBytes[outOff + 1] = (byte)((uint)word >> 8);
			outBytes[outOff + 2] = (byte)((uint)word >> 16);
			outBytes[outOff + 3] = (byte)((uint)word >> 24);
		}

		public override int DoFinal(byte[] output, int outOff)
		{
			base.Finish();
			RIPEMD160.UnpackWord(this.H0, output, outOff);
			RIPEMD160.UnpackWord(this.H1, output, outOff + 4);
			RIPEMD160.UnpackWord(this.H2, output, outOff + 8);
			RIPEMD160.UnpackWord(this.H3, output, outOff + 12);
			RIPEMD160.UnpackWord(this.H4, output, outOff + 16);
			this.Reset();
			return 20;
		}

		public override void Reset()
		{
			base.Reset();
			this.InitializeHs();
		}

		void InitializeHs()
		{
			this.H0 = 1732584193;
			this.H1 = -271733879;
			this.H2 = -1732584194;
			this.H3 = 271733878;
			this.H4 = -1009589776;
			this.xOff = 0;
			for (int num = 0; num != this.X.Length; num++)
			{
				this.X[num] = 0;
			}
		}

		static int RL(int x, int n)
		{
			return (x << n) | (int)((uint)x >> 32 - n);
		}

		static int F1(int x, int y, int z)
		{
			return x ^ y ^ z;
		}

		static int F2(int x, int y, int z)
		{
			return (x & y) | (~x & z);
		}

		static int F3(int x, int y, int z)
		{
			return (x | ~y) ^ z;
		}

		static int F4(int x, int y, int z)
		{
			return (x & z) | (y & ~z);
		}

		static int F5(int x, int y, int z)
		{
			return x ^ (y | ~z);
		}

		internal override void ProcessBlock()
		{
			int num2;
			int num = (num2 = this.H0);
			int num4;
			int num3 = (num4 = this.H1);
			int num6;
			int num5 = (num6 = this.H2);
			int num8;
			int num7 = (num8 = this.H3);
			int num10;
			int num9 = (num10 = this.H4);
			num2 = RIPEMD160.RL(num2 + RIPEMD160.F1(num4, num6, num8) + this.X[0], 11) + num10;
			num6 = RIPEMD160.RL(num6, 10);
			num10 = RIPEMD160.RL(num10 + RIPEMD160.F1(num2, num4, num6) + this.X[1], 14) + num8;
			num4 = RIPEMD160.RL(num4, 10);
			num8 = RIPEMD160.RL(num8 + RIPEMD160.F1(num10, num2, num4) + this.X[2], 15) + num6;
			num2 = RIPEMD160.RL(num2, 10);
			num6 = RIPEMD160.RL(num6 + RIPEMD160.F1(num8, num10, num2) + this.X[3], 12) + num4;
			num10 = RIPEMD160.RL(num10, 10);
			num4 = RIPEMD160.RL(num4 + RIPEMD160.F1(num6, num8, num10) + this.X[4], 5) + num2;
			num8 = RIPEMD160.RL(num8, 10);
			num2 = RIPEMD160.RL(num2 + RIPEMD160.F1(num4, num6, num8) + this.X[5], 8) + num10;
			num6 = RIPEMD160.RL(num6, 10);
			num10 = RIPEMD160.RL(num10 + RIPEMD160.F1(num2, num4, num6) + this.X[6], 7) + num8;
			num4 = RIPEMD160.RL(num4, 10);
			num8 = RIPEMD160.RL(num8 + RIPEMD160.F1(num10, num2, num4) + this.X[7], 9) + num6;
			num2 = RIPEMD160.RL(num2, 10);
			num6 = RIPEMD160.RL(num6 + RIPEMD160.F1(num8, num10, num2) + this.X[8], 11) + num4;
			num10 = RIPEMD160.RL(num10, 10);
			num4 = RIPEMD160.RL(num4 + RIPEMD160.F1(num6, num8, num10) + this.X[9], 13) + num2;
			num8 = RIPEMD160.RL(num8, 10);
			num2 = RIPEMD160.RL(num2 + RIPEMD160.F1(num4, num6, num8) + this.X[10], 14) + num10;
			num6 = RIPEMD160.RL(num6, 10);
			num10 = RIPEMD160.RL(num10 + RIPEMD160.F1(num2, num4, num6) + this.X[11], 15) + num8;
			num4 = RIPEMD160.RL(num4, 10);
			num8 = RIPEMD160.RL(num8 + RIPEMD160.F1(num10, num2, num4) + this.X[12], 6) + num6;
			num2 = RIPEMD160.RL(num2, 10);
			num6 = RIPEMD160.RL(num6 + RIPEMD160.F1(num8, num10, num2) + this.X[13], 7) + num4;
			num10 = RIPEMD160.RL(num10, 10);
			num4 = RIPEMD160.RL(num4 + RIPEMD160.F1(num6, num8, num10) + this.X[14], 9) + num2;
			num8 = RIPEMD160.RL(num8, 10);
			num2 = RIPEMD160.RL(num2 + RIPEMD160.F1(num4, num6, num8) + this.X[15], 8) + num10;
			num6 = RIPEMD160.RL(num6, 10);
			num = RIPEMD160.RL(num + RIPEMD160.F5(num3, num5, num7) + this.X[5] + 1352829926, 8) + num9;
			num5 = RIPEMD160.RL(num5, 10);
			num9 = RIPEMD160.RL(num9 + RIPEMD160.F5(num, num3, num5) + this.X[14] + 1352829926, 9) + num7;
			num3 = RIPEMD160.RL(num3, 10);
			num7 = RIPEMD160.RL(num7 + RIPEMD160.F5(num9, num, num3) + this.X[7] + 1352829926, 9) + num5;
			num = RIPEMD160.RL(num, 10);
			num5 = RIPEMD160.RL(num5 + RIPEMD160.F5(num7, num9, num) + this.X[0] + 1352829926, 11) + num3;
			num9 = RIPEMD160.RL(num9, 10);
			num3 = RIPEMD160.RL(num3 + RIPEMD160.F5(num5, num7, num9) + this.X[9] + 1352829926, 13) + num;
			num7 = RIPEMD160.RL(num7, 10);
			num = RIPEMD160.RL(num + RIPEMD160.F5(num3, num5, num7) + this.X[2] + 1352829926, 15) + num9;
			num5 = RIPEMD160.RL(num5, 10);
			num9 = RIPEMD160.RL(num9 + RIPEMD160.F5(num, num3, num5) + this.X[11] + 1352829926, 15) + num7;
			num3 = RIPEMD160.RL(num3, 10);
			num7 = RIPEMD160.RL(num7 + RIPEMD160.F5(num9, num, num3) + this.X[4] + 1352829926, 5) + num5;
			num = RIPEMD160.RL(num, 10);
			num5 = RIPEMD160.RL(num5 + RIPEMD160.F5(num7, num9, num) + this.X[13] + 1352829926, 7) + num3;
			num9 = RIPEMD160.RL(num9, 10);
			num3 = RIPEMD160.RL(num3 + RIPEMD160.F5(num5, num7, num9) + this.X[6] + 1352829926, 7) + num;
			num7 = RIPEMD160.RL(num7, 10);
			num = RIPEMD160.RL(num + RIPEMD160.F5(num3, num5, num7) + this.X[15] + 1352829926, 8) + num9;
			num5 = RIPEMD160.RL(num5, 10);
			num9 = RIPEMD160.RL(num9 + RIPEMD160.F5(num, num3, num5) + this.X[8] + 1352829926, 11) + num7;
			num3 = RIPEMD160.RL(num3, 10);
			num7 = RIPEMD160.RL(num7 + RIPEMD160.F5(num9, num, num3) + this.X[1] + 1352829926, 14) + num5;
			num = RIPEMD160.RL(num, 10);
			num5 = RIPEMD160.RL(num5 + RIPEMD160.F5(num7, num9, num) + this.X[10] + 1352829926, 14) + num3;
			num9 = RIPEMD160.RL(num9, 10);
			num3 = RIPEMD160.RL(num3 + RIPEMD160.F5(num5, num7, num9) + this.X[3] + 1352829926, 12) + num;
			num7 = RIPEMD160.RL(num7, 10);
			num = RIPEMD160.RL(num + RIPEMD160.F5(num3, num5, num7) + this.X[12] + 1352829926, 6) + num9;
			num5 = RIPEMD160.RL(num5, 10);
			num10 = RIPEMD160.RL(num10 + RIPEMD160.F2(num2, num4, num6) + this.X[7] + 1518500249, 7) + num8;
			num4 = RIPEMD160.RL(num4, 10);
			num8 = RIPEMD160.RL(num8 + RIPEMD160.F2(num10, num2, num4) + this.X[4] + 1518500249, 6) + num6;
			num2 = RIPEMD160.RL(num2, 10);
			num6 = RIPEMD160.RL(num6 + RIPEMD160.F2(num8, num10, num2) + this.X[13] + 1518500249, 8) + num4;
			num10 = RIPEMD160.RL(num10, 10);
			num4 = RIPEMD160.RL(num4 + RIPEMD160.F2(num6, num8, num10) + this.X[1] + 1518500249, 13) + num2;
			num8 = RIPEMD160.RL(num8, 10);
			num2 = RIPEMD160.RL(num2 + RIPEMD160.F2(num4, num6, num8) + this.X[10] + 1518500249, 11) + num10;
			num6 = RIPEMD160.RL(num6, 10);
			num10 = RIPEMD160.RL(num10 + RIPEMD160.F2(num2, num4, num6) + this.X[6] + 1518500249, 9) + num8;
			num4 = RIPEMD160.RL(num4, 10);
			num8 = RIPEMD160.RL(num8 + RIPEMD160.F2(num10, num2, num4) + this.X[15] + 1518500249, 7) + num6;
			num2 = RIPEMD160.RL(num2, 10);
			num6 = RIPEMD160.RL(num6 + RIPEMD160.F2(num8, num10, num2) + this.X[3] + 1518500249, 15) + num4;
			num10 = RIPEMD160.RL(num10, 10);
			num4 = RIPEMD160.RL(num4 + RIPEMD160.F2(num6, num8, num10) + this.X[12] + 1518500249, 7) + num2;
			num8 = RIPEMD160.RL(num8, 10);
			num2 = RIPEMD160.RL(num2 + RIPEMD160.F2(num4, num6, num8) + this.X[0] + 1518500249, 12) + num10;
			num6 = RIPEMD160.RL(num6, 10);
			num10 = RIPEMD160.RL(num10 + RIPEMD160.F2(num2, num4, num6) + this.X[9] + 1518500249, 15) + num8;
			num4 = RIPEMD160.RL(num4, 10);
			num8 = RIPEMD160.RL(num8 + RIPEMD160.F2(num10, num2, num4) + this.X[5] + 1518500249, 9) + num6;
			num2 = RIPEMD160.RL(num2, 10);
			num6 = RIPEMD160.RL(num6 + RIPEMD160.F2(num8, num10, num2) + this.X[2] + 1518500249, 11) + num4;
			num10 = RIPEMD160.RL(num10, 10);
			num4 = RIPEMD160.RL(num4 + RIPEMD160.F2(num6, num8, num10) + this.X[14] + 1518500249, 7) + num2;
			num8 = RIPEMD160.RL(num8, 10);
			num2 = RIPEMD160.RL(num2 + RIPEMD160.F2(num4, num6, num8) + this.X[11] + 1518500249, 13) + num10;
			num6 = RIPEMD160.RL(num6, 10);
			num10 = RIPEMD160.RL(num10 + RIPEMD160.F2(num2, num4, num6) + this.X[8] + 1518500249, 12) + num8;
			num4 = RIPEMD160.RL(num4, 10);
			num9 = RIPEMD160.RL(num9 + RIPEMD160.F4(num, num3, num5) + this.X[6] + 1548603684, 9) + num7;
			num3 = RIPEMD160.RL(num3, 10);
			num7 = RIPEMD160.RL(num7 + RIPEMD160.F4(num9, num, num3) + this.X[11] + 1548603684, 13) + num5;
			num = RIPEMD160.RL(num, 10);
			num5 = RIPEMD160.RL(num5 + RIPEMD160.F4(num7, num9, num) + this.X[3] + 1548603684, 15) + num3;
			num9 = RIPEMD160.RL(num9, 10);
			num3 = RIPEMD160.RL(num3 + RIPEMD160.F4(num5, num7, num9) + this.X[7] + 1548603684, 7) + num;
			num7 = RIPEMD160.RL(num7, 10);
			num = RIPEMD160.RL(num + RIPEMD160.F4(num3, num5, num7) + this.X[0] + 1548603684, 12) + num9;
			num5 = RIPEMD160.RL(num5, 10);
			num9 = RIPEMD160.RL(num9 + RIPEMD160.F4(num, num3, num5) + this.X[13] + 1548603684, 8) + num7;
			num3 = RIPEMD160.RL(num3, 10);
			num7 = RIPEMD160.RL(num7 + RIPEMD160.F4(num9, num, num3) + this.X[5] + 1548603684, 9) + num5;
			num = RIPEMD160.RL(num, 10);
			num5 = RIPEMD160.RL(num5 + RIPEMD160.F4(num7, num9, num) + this.X[10] + 1548603684, 11) + num3;
			num9 = RIPEMD160.RL(num9, 10);
			num3 = RIPEMD160.RL(num3 + RIPEMD160.F4(num5, num7, num9) + this.X[14] + 1548603684, 7) + num;
			num7 = RIPEMD160.RL(num7, 10);
			num = RIPEMD160.RL(num + RIPEMD160.F4(num3, num5, num7) + this.X[15] + 1548603684, 7) + num9;
			num5 = RIPEMD160.RL(num5, 10);
			num9 = RIPEMD160.RL(num9 + RIPEMD160.F4(num, num3, num5) + this.X[8] + 1548603684, 12) + num7;
			num3 = RIPEMD160.RL(num3, 10);
			num7 = RIPEMD160.RL(num7 + RIPEMD160.F4(num9, num, num3) + this.X[12] + 1548603684, 7) + num5;
			num = RIPEMD160.RL(num, 10);
			num5 = RIPEMD160.RL(num5 + RIPEMD160.F4(num7, num9, num) + this.X[4] + 1548603684, 6) + num3;
			num9 = RIPEMD160.RL(num9, 10);
			num3 = RIPEMD160.RL(num3 + RIPEMD160.F4(num5, num7, num9) + this.X[9] + 1548603684, 15) + num;
			num7 = RIPEMD160.RL(num7, 10);
			num = RIPEMD160.RL(num + RIPEMD160.F4(num3, num5, num7) + this.X[1] + 1548603684, 13) + num9;
			num5 = RIPEMD160.RL(num5, 10);
			num9 = RIPEMD160.RL(num9 + RIPEMD160.F4(num, num3, num5) + this.X[2] + 1548603684, 11) + num7;
			num3 = RIPEMD160.RL(num3, 10);
			num8 = RIPEMD160.RL(num8 + RIPEMD160.F3(num10, num2, num4) + this.X[3] + 1859775393, 11) + num6;
			num2 = RIPEMD160.RL(num2, 10);
			num6 = RIPEMD160.RL(num6 + RIPEMD160.F3(num8, num10, num2) + this.X[10] + 1859775393, 13) + num4;
			num10 = RIPEMD160.RL(num10, 10);
			num4 = RIPEMD160.RL(num4 + RIPEMD160.F3(num6, num8, num10) + this.X[14] + 1859775393, 6) + num2;
			num8 = RIPEMD160.RL(num8, 10);
			num2 = RIPEMD160.RL(num2 + RIPEMD160.F3(num4, num6, num8) + this.X[4] + 1859775393, 7) + num10;
			num6 = RIPEMD160.RL(num6, 10);
			num10 = RIPEMD160.RL(num10 + RIPEMD160.F3(num2, num4, num6) + this.X[9] + 1859775393, 14) + num8;
			num4 = RIPEMD160.RL(num4, 10);
			num8 = RIPEMD160.RL(num8 + RIPEMD160.F3(num10, num2, num4) + this.X[15] + 1859775393, 9) + num6;
			num2 = RIPEMD160.RL(num2, 10);
			num6 = RIPEMD160.RL(num6 + RIPEMD160.F3(num8, num10, num2) + this.X[8] + 1859775393, 13) + num4;
			num10 = RIPEMD160.RL(num10, 10);
			num4 = RIPEMD160.RL(num4 + RIPEMD160.F3(num6, num8, num10) + this.X[1] + 1859775393, 15) + num2;
			num8 = RIPEMD160.RL(num8, 10);
			num2 = RIPEMD160.RL(num2 + RIPEMD160.F3(num4, num6, num8) + this.X[2] + 1859775393, 14) + num10;
			num6 = RIPEMD160.RL(num6, 10);
			num10 = RIPEMD160.RL(num10 + RIPEMD160.F3(num2, num4, num6) + this.X[7] + 1859775393, 8) + num8;
			num4 = RIPEMD160.RL(num4, 10);
			num8 = RIPEMD160.RL(num8 + RIPEMD160.F3(num10, num2, num4) + this.X[0] + 1859775393, 13) + num6;
			num2 = RIPEMD160.RL(num2, 10);
			num6 = RIPEMD160.RL(num6 + RIPEMD160.F3(num8, num10, num2) + this.X[6] + 1859775393, 6) + num4;
			num10 = RIPEMD160.RL(num10, 10);
			num4 = RIPEMD160.RL(num4 + RIPEMD160.F3(num6, num8, num10) + this.X[13] + 1859775393, 5) + num2;
			num8 = RIPEMD160.RL(num8, 10);
			num2 = RIPEMD160.RL(num2 + RIPEMD160.F3(num4, num6, num8) + this.X[11] + 1859775393, 12) + num10;
			num6 = RIPEMD160.RL(num6, 10);
			num10 = RIPEMD160.RL(num10 + RIPEMD160.F3(num2, num4, num6) + this.X[5] + 1859775393, 7) + num8;
			num4 = RIPEMD160.RL(num4, 10);
			num8 = RIPEMD160.RL(num8 + RIPEMD160.F3(num10, num2, num4) + this.X[12] + 1859775393, 5) + num6;
			num2 = RIPEMD160.RL(num2, 10);
			num7 = RIPEMD160.RL(num7 + RIPEMD160.F3(num9, num, num3) + this.X[15] + 1836072691, 9) + num5;
			num = RIPEMD160.RL(num, 10);
			num5 = RIPEMD160.RL(num5 + RIPEMD160.F3(num7, num9, num) + this.X[5] + 1836072691, 7) + num3;
			num9 = RIPEMD160.RL(num9, 10);
			num3 = RIPEMD160.RL(num3 + RIPEMD160.F3(num5, num7, num9) + this.X[1] + 1836072691, 15) + num;
			num7 = RIPEMD160.RL(num7, 10);
			num = RIPEMD160.RL(num + RIPEMD160.F3(num3, num5, num7) + this.X[3] + 1836072691, 11) + num9;
			num5 = RIPEMD160.RL(num5, 10);
			num9 = RIPEMD160.RL(num9 + RIPEMD160.F3(num, num3, num5) + this.X[7] + 1836072691, 8) + num7;
			num3 = RIPEMD160.RL(num3, 10);
			num7 = RIPEMD160.RL(num7 + RIPEMD160.F3(num9, num, num3) + this.X[14] + 1836072691, 6) + num5;
			num = RIPEMD160.RL(num, 10);
			num5 = RIPEMD160.RL(num5 + RIPEMD160.F3(num7, num9, num) + this.X[6] + 1836072691, 6) + num3;
			num9 = RIPEMD160.RL(num9, 10);
			num3 = RIPEMD160.RL(num3 + RIPEMD160.F3(num5, num7, num9) + this.X[9] + 1836072691, 14) + num;
			num7 = RIPEMD160.RL(num7, 10);
			num = RIPEMD160.RL(num + RIPEMD160.F3(num3, num5, num7) + this.X[11] + 1836072691, 12) + num9;
			num5 = RIPEMD160.RL(num5, 10);
			num9 = RIPEMD160.RL(num9 + RIPEMD160.F3(num, num3, num5) + this.X[8] + 1836072691, 13) + num7;
			num3 = RIPEMD160.RL(num3, 10);
			num7 = RIPEMD160.RL(num7 + RIPEMD160.F3(num9, num, num3) + this.X[12] + 1836072691, 5) + num5;
			num = RIPEMD160.RL(num, 10);
			num5 = RIPEMD160.RL(num5 + RIPEMD160.F3(num7, num9, num) + this.X[2] + 1836072691, 14) + num3;
			num9 = RIPEMD160.RL(num9, 10);
			num3 = RIPEMD160.RL(num3 + RIPEMD160.F3(num5, num7, num9) + this.X[10] + 1836072691, 13) + num;
			num7 = RIPEMD160.RL(num7, 10);
			num = RIPEMD160.RL(num + RIPEMD160.F3(num3, num5, num7) + this.X[0] + 1836072691, 13) + num9;
			num5 = RIPEMD160.RL(num5, 10);
			num9 = RIPEMD160.RL(num9 + RIPEMD160.F3(num, num3, num5) + this.X[4] + 1836072691, 7) + num7;
			num3 = RIPEMD160.RL(num3, 10);
			num7 = RIPEMD160.RL(num7 + RIPEMD160.F3(num9, num, num3) + this.X[13] + 1836072691, 5) + num5;
			num = RIPEMD160.RL(num, 10);
			num6 = RIPEMD160.RL(num6 + RIPEMD160.F4(num8, num10, num2) + this.X[1] + -1894007588, 11) + num4;
			num10 = RIPEMD160.RL(num10, 10);
			num4 = RIPEMD160.RL(num4 + RIPEMD160.F4(num6, num8, num10) + this.X[9] + -1894007588, 12) + num2;
			num8 = RIPEMD160.RL(num8, 10);
			num2 = RIPEMD160.RL(num2 + RIPEMD160.F4(num4, num6, num8) + this.X[11] + -1894007588, 14) + num10;
			num6 = RIPEMD160.RL(num6, 10);
			num10 = RIPEMD160.RL(num10 + RIPEMD160.F4(num2, num4, num6) + this.X[10] + -1894007588, 15) + num8;
			num4 = RIPEMD160.RL(num4, 10);
			num8 = RIPEMD160.RL(num8 + RIPEMD160.F4(num10, num2, num4) + this.X[0] + -1894007588, 14) + num6;
			num2 = RIPEMD160.RL(num2, 10);
			num6 = RIPEMD160.RL(num6 + RIPEMD160.F4(num8, num10, num2) + this.X[8] + -1894007588, 15) + num4;
			num10 = RIPEMD160.RL(num10, 10);
			num4 = RIPEMD160.RL(num4 + RIPEMD160.F4(num6, num8, num10) + this.X[12] + -1894007588, 9) + num2;
			num8 = RIPEMD160.RL(num8, 10);
			num2 = RIPEMD160.RL(num2 + RIPEMD160.F4(num4, num6, num8) + this.X[4] + -1894007588, 8) + num10;
			num6 = RIPEMD160.RL(num6, 10);
			num10 = RIPEMD160.RL(num10 + RIPEMD160.F4(num2, num4, num6) + this.X[13] + -1894007588, 9) + num8;
			num4 = RIPEMD160.RL(num4, 10);
			num8 = RIPEMD160.RL(num8 + RIPEMD160.F4(num10, num2, num4) + this.X[3] + -1894007588, 14) + num6;
			num2 = RIPEMD160.RL(num2, 10);
			num6 = RIPEMD160.RL(num6 + RIPEMD160.F4(num8, num10, num2) + this.X[7] + -1894007588, 5) + num4;
			num10 = RIPEMD160.RL(num10, 10);
			num4 = RIPEMD160.RL(num4 + RIPEMD160.F4(num6, num8, num10) + this.X[15] + -1894007588, 6) + num2;
			num8 = RIPEMD160.RL(num8, 10);
			num2 = RIPEMD160.RL(num2 + RIPEMD160.F4(num4, num6, num8) + this.X[14] + -1894007588, 8) + num10;
			num6 = RIPEMD160.RL(num6, 10);
			num10 = RIPEMD160.RL(num10 + RIPEMD160.F4(num2, num4, num6) + this.X[5] + -1894007588, 6) + num8;
			num4 = RIPEMD160.RL(num4, 10);
			num8 = RIPEMD160.RL(num8 + RIPEMD160.F4(num10, num2, num4) + this.X[6] + -1894007588, 5) + num6;
			num2 = RIPEMD160.RL(num2, 10);
			num6 = RIPEMD160.RL(num6 + RIPEMD160.F4(num8, num10, num2) + this.X[2] + -1894007588, 12) + num4;
			num10 = RIPEMD160.RL(num10, 10);
			num5 = RIPEMD160.RL(num5 + RIPEMD160.F2(num7, num9, num) + this.X[8] + 2053994217, 15) + num3;
			num9 = RIPEMD160.RL(num9, 10);
			num3 = RIPEMD160.RL(num3 + RIPEMD160.F2(num5, num7, num9) + this.X[6] + 2053994217, 5) + num;
			num7 = RIPEMD160.RL(num7, 10);
			num = RIPEMD160.RL(num + RIPEMD160.F2(num3, num5, num7) + this.X[4] + 2053994217, 8) + num9;
			num5 = RIPEMD160.RL(num5, 10);
			num9 = RIPEMD160.RL(num9 + RIPEMD160.F2(num, num3, num5) + this.X[1] + 2053994217, 11) + num7;
			num3 = RIPEMD160.RL(num3, 10);
			num7 = RIPEMD160.RL(num7 + RIPEMD160.F2(num9, num, num3) + this.X[3] + 2053994217, 14) + num5;
			num = RIPEMD160.RL(num, 10);
			num5 = RIPEMD160.RL(num5 + RIPEMD160.F2(num7, num9, num) + this.X[11] + 2053994217, 14) + num3;
			num9 = RIPEMD160.RL(num9, 10);
			num3 = RIPEMD160.RL(num3 + RIPEMD160.F2(num5, num7, num9) + this.X[15] + 2053994217, 6) + num;
			num7 = RIPEMD160.RL(num7, 10);
			num = RIPEMD160.RL(num + RIPEMD160.F2(num3, num5, num7) + this.X[0] + 2053994217, 14) + num9;
			num5 = RIPEMD160.RL(num5, 10);
			num9 = RIPEMD160.RL(num9 + RIPEMD160.F2(num, num3, num5) + this.X[5] + 2053994217, 6) + num7;
			num3 = RIPEMD160.RL(num3, 10);
			num7 = RIPEMD160.RL(num7 + RIPEMD160.F2(num9, num, num3) + this.X[12] + 2053994217, 9) + num5;
			num = RIPEMD160.RL(num, 10);
			num5 = RIPEMD160.RL(num5 + RIPEMD160.F2(num7, num9, num) + this.X[2] + 2053994217, 12) + num3;
			num9 = RIPEMD160.RL(num9, 10);
			num3 = RIPEMD160.RL(num3 + RIPEMD160.F2(num5, num7, num9) + this.X[13] + 2053994217, 9) + num;
			num7 = RIPEMD160.RL(num7, 10);
			num = RIPEMD160.RL(num + RIPEMD160.F2(num3, num5, num7) + this.X[9] + 2053994217, 12) + num9;
			num5 = RIPEMD160.RL(num5, 10);
			num9 = RIPEMD160.RL(num9 + RIPEMD160.F2(num, num3, num5) + this.X[7] + 2053994217, 5) + num7;
			num3 = RIPEMD160.RL(num3, 10);
			num7 = RIPEMD160.RL(num7 + RIPEMD160.F2(num9, num, num3) + this.X[10] + 2053994217, 15) + num5;
			num = RIPEMD160.RL(num, 10);
			num5 = RIPEMD160.RL(num5 + RIPEMD160.F2(num7, num9, num) + this.X[14] + 2053994217, 8) + num3;
			num9 = RIPEMD160.RL(num9, 10);
			num4 = RIPEMD160.RL(num4 + RIPEMD160.F5(num6, num8, num10) + this.X[4] + -1454113458, 9) + num2;
			num8 = RIPEMD160.RL(num8, 10);
			num2 = RIPEMD160.RL(num2 + RIPEMD160.F5(num4, num6, num8) + this.X[0] + -1454113458, 15) + num10;
			num6 = RIPEMD160.RL(num6, 10);
			num10 = RIPEMD160.RL(num10 + RIPEMD160.F5(num2, num4, num6) + this.X[5] + -1454113458, 5) + num8;
			num4 = RIPEMD160.RL(num4, 10);
			num8 = RIPEMD160.RL(num8 + RIPEMD160.F5(num10, num2, num4) + this.X[9] + -1454113458, 11) + num6;
			num2 = RIPEMD160.RL(num2, 10);
			num6 = RIPEMD160.RL(num6 + RIPEMD160.F5(num8, num10, num2) + this.X[7] + -1454113458, 6) + num4;
			num10 = RIPEMD160.RL(num10, 10);
			num4 = RIPEMD160.RL(num4 + RIPEMD160.F5(num6, num8, num10) + this.X[12] + -1454113458, 8) + num2;
			num8 = RIPEMD160.RL(num8, 10);
			num2 = RIPEMD160.RL(num2 + RIPEMD160.F5(num4, num6, num8) + this.X[2] + -1454113458, 13) + num10;
			num6 = RIPEMD160.RL(num6, 10);
			num10 = RIPEMD160.RL(num10 + RIPEMD160.F5(num2, num4, num6) + this.X[10] + -1454113458, 12) + num8;
			num4 = RIPEMD160.RL(num4, 10);
			num8 = RIPEMD160.RL(num8 + RIPEMD160.F5(num10, num2, num4) + this.X[14] + -1454113458, 5) + num6;
			num2 = RIPEMD160.RL(num2, 10);
			num6 = RIPEMD160.RL(num6 + RIPEMD160.F5(num8, num10, num2) + this.X[1] + -1454113458, 12) + num4;
			num10 = RIPEMD160.RL(num10, 10);
			num4 = RIPEMD160.RL(num4 + RIPEMD160.F5(num6, num8, num10) + this.X[3] + -1454113458, 13) + num2;
			num8 = RIPEMD160.RL(num8, 10);
			num2 = RIPEMD160.RL(num2 + RIPEMD160.F5(num4, num6, num8) + this.X[8] + -1454113458, 14) + num10;
			num6 = RIPEMD160.RL(num6, 10);
			num10 = RIPEMD160.RL(num10 + RIPEMD160.F5(num2, num4, num6) + this.X[11] + -1454113458, 11) + num8;
			num4 = RIPEMD160.RL(num4, 10);
			num8 = RIPEMD160.RL(num8 + RIPEMD160.F5(num10, num2, num4) + this.X[6] + -1454113458, 8) + num6;
			num2 = RIPEMD160.RL(num2, 10);
			num6 = RIPEMD160.RL(num6 + RIPEMD160.F5(num8, num10, num2) + this.X[15] + -1454113458, 5) + num4;
			num10 = RIPEMD160.RL(num10, 10);
			num4 = RIPEMD160.RL(num4 + RIPEMD160.F5(num6, num8, num10) + this.X[13] + -1454113458, 6) + num2;
			num8 = RIPEMD160.RL(num8, 10);
			num3 = RIPEMD160.RL(num3 + RIPEMD160.F1(num5, num7, num9) + this.X[12], 8) + num;
			num7 = RIPEMD160.RL(num7, 10);
			num = RIPEMD160.RL(num + RIPEMD160.F1(num3, num5, num7) + this.X[15], 5) + num9;
			num5 = RIPEMD160.RL(num5, 10);
			num9 = RIPEMD160.RL(num9 + RIPEMD160.F1(num, num3, num5) + this.X[10], 12) + num7;
			num3 = RIPEMD160.RL(num3, 10);
			num7 = RIPEMD160.RL(num7 + RIPEMD160.F1(num9, num, num3) + this.X[4], 9) + num5;
			num = RIPEMD160.RL(num, 10);
			num5 = RIPEMD160.RL(num5 + RIPEMD160.F1(num7, num9, num) + this.X[1], 12) + num3;
			num9 = RIPEMD160.RL(num9, 10);
			num3 = RIPEMD160.RL(num3 + RIPEMD160.F1(num5, num7, num9) + this.X[5], 5) + num;
			num7 = RIPEMD160.RL(num7, 10);
			num = RIPEMD160.RL(num + RIPEMD160.F1(num3, num5, num7) + this.X[8], 14) + num9;
			num5 = RIPEMD160.RL(num5, 10);
			num9 = RIPEMD160.RL(num9 + RIPEMD160.F1(num, num3, num5) + this.X[7], 6) + num7;
			num3 = RIPEMD160.RL(num3, 10);
			num7 = RIPEMD160.RL(num7 + RIPEMD160.F1(num9, num, num3) + this.X[6], 8) + num5;
			num = RIPEMD160.RL(num, 10);
			num5 = RIPEMD160.RL(num5 + RIPEMD160.F1(num7, num9, num) + this.X[2], 13) + num3;
			num9 = RIPEMD160.RL(num9, 10);
			num3 = RIPEMD160.RL(num3 + RIPEMD160.F1(num5, num7, num9) + this.X[13], 6) + num;
			num7 = RIPEMD160.RL(num7, 10);
			num = RIPEMD160.RL(num + RIPEMD160.F1(num3, num5, num7) + this.X[14], 5) + num9;
			num5 = RIPEMD160.RL(num5, 10);
			num9 = RIPEMD160.RL(num9 + RIPEMD160.F1(num, num3, num5) + this.X[0], 15) + num7;
			num3 = RIPEMD160.RL(num3, 10);
			num7 = RIPEMD160.RL(num7 + RIPEMD160.F1(num9, num, num3) + this.X[3], 13) + num5;
			num = RIPEMD160.RL(num, 10);
			num5 = RIPEMD160.RL(num5 + RIPEMD160.F1(num7, num9, num) + this.X[9], 11) + num3;
			num9 = RIPEMD160.RL(num9, 10);
			num3 = RIPEMD160.RL(num3 + RIPEMD160.F1(num5, num7, num9) + this.X[11], 11) + num;
			num7 = RIPEMD160.RL(num7, 10);
			num7 += num6 + this.H1;
			this.H1 = this.H2 + num8 + num9;
			this.H2 = this.H3 + num10 + num;
			this.H3 = this.H4 + num2 + num3;
			this.H4 = this.H0 + num4 + num5;
			this.H0 = num7;
			this.xOff = 0;
			for (int num11 = 0; num11 != this.X.Length; num11++)
			{
				this.X[num11] = 0;
			}
		}

		public const string Name = "RIPEMD-160";

		const int DigestLength = 20;

		static readonly RIPEMD160Managed algorithm = new RIPEMD160Managed();

		int H0;

		int H1;

		int H2;

		int H3;

		int H4;

		int[] X = new int[16];

		int xOff;
	}
}
