using System;
using System.Security.Cryptography;

namespace Telerik.Windows.Documents.Common.Model.Protection
{
	class SHA384 : LongDigest, IProtectionAlgorithm
	{
		public SHA384()
		{
			this.InitializeHs();
		}

		public SHA384(SHA384 t)
			: base(t)
		{
		}

		public override string AlgorithmName
		{
			get
			{
				return "SHA-384";
			}
		}

		public override int GetDigestSize()
		{
			return 48;
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
			this.Reset();
			return 48;
		}

		public byte[] ComputeHash(byte[] buffer)
		{
			return SHA384.algorithm.ComputeHash(buffer);
		}

		public override void Reset()
		{
			base.Reset();
			this.InitializeHs();
		}

		void InitializeHs()
		{
			this.H1 = 14680500436340154072UL;
			this.H2 = 7105036623409894663UL;
			this.H3 = 10473403895298186519UL;
			this.H4 = 1526699215303891257UL;
			this.H5 = 7436329637833083697UL;
			this.H6 = 10282925794625328401UL;
			this.H7 = 15784041429090275239UL;
			this.H8 = 5167115440072839076UL;
		}

		public const string Name = "SHA-384";

		const int DigestLength = 48;

		static readonly SHA384CryptoServiceProvider algorithm = new SHA384CryptoServiceProvider();
	}
}
