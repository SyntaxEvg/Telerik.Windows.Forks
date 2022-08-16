using System;
using System.Security.Cryptography;

namespace Telerik.Windows.Documents.Common.Model.Protection
{
	class SHA256 : IProtectionAlgorithm
	{
		public byte[] ComputeHash(byte[] buffer)
		{
			return SHA256.algorithm.ComputeHash(buffer);
		}

		public const string Name = "SHA-256";

		static readonly SHA256CryptoServiceProvider algorithm = new SHA256CryptoServiceProvider();
	}
}
