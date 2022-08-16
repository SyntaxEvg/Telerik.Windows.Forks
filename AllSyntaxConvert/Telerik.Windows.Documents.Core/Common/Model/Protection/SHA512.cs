using System;
using System.Security.Cryptography;

namespace Telerik.Windows.Documents.Common.Model.Protection
{
	class SHA512 : LongDigest, IProtectionAlgorithm
	{
		public SHA512()
		{
			this.InitializeHs();
		}

		public SHA512(SHA512 t)
			: base(t)
		{
		}

		public override string AlgorithmName
		{
			get
			{
				return "SHA-512";
			}
		}

		public override int GetDigestSize()
		{
			return 64;
		}

		public override int DoFinal(byte[] output, int outOff)
		{
			base.Finish();
			Pack.UInt64_To_BE(this.H1, output, outOff);
			Pack.UInt64_To_BE(this.H2, output, outOff + 8);
			Pack.UInt64_To_BE(this.H3, output, outOff + 16);
			Pack.UInt64_To_BE(this.H4, output, outOff + 24);
			Pack.UInt64_To_BE(this.H5, output, outOff + 32);
			Pack.UInt64_To_BE(this.H6, output, outOff + 40);
			Pack.UInt64_To_BE(this.H7, output, outOff + 48);
			Pack.UInt64_To_BE(this.H8, output, outOff + 56);
			this.Reset();
			return 64;
		}

		public byte[] ComputeHash(byte[] buffer)
		{
			return SHA512.algorithm.ComputeHash(buffer);
		}

		public override void Reset()
		{
			base.Reset();
			this.InitializeHs();
		}

		void InitializeHs()
		{
			this.H1 = 7640891576956012808UL;
			this.H2 = 13503953896175478587UL;
			this.H3 = 4354685564936845355UL;
			this.H4 = 11912009170470909681UL;
			this.H5 = 5840696475078001361UL;
			this.H6 = 11170449401992604703UL;
			this.H7 = 2270897969802886507UL;
			this.H8 = 6620516959819538809UL;
		}

		public const string Name = "SHA-512";

		const int DigestLength = 64;

		static readonly SHA512CryptoServiceProvider algorithm = new SHA512CryptoServiceProvider();
	}
}
