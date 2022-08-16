using System;
using System.Security.Cryptography;

namespace Telerik.Windows.Documents.Common.Model.Protection
{
	class SHA1 : IProtectionAlgorithm
	{
		public byte[] ComputeHash(byte[] buffer)
		{
			return SHA1.algorithm.ComputeHash(buffer);
		}

		public const string Name = "SHA-1";

		static readonly SHA1CryptoServiceProvider algorithm = new SHA1CryptoServiceProvider();
	}
}
