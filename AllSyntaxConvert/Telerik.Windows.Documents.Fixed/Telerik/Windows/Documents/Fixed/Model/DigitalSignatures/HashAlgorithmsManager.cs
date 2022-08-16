using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	public static class HashAlgorithmsManager
	{
		static HashAlgorithmsManager()
		{
			HashAlgorithmsManager.RegisterHashAlgorithm(SignatureOids.SHA1, new SHA1Managed());
			HashAlgorithmsManager.RegisterHashAlgorithm(SignatureOids.SHA256, new SHA256Managed());
			HashAlgorithmsManager.RegisterHashAlgorithm(SignatureOids.SHA384, new SHA384Managed());
			HashAlgorithmsManager.RegisterHashAlgorithm(SignatureOids.SHA512, new SHA512Managed());
			HashAlgorithmsManager.RegisterHashAlgorithm(SignatureOids.MD5, new MD5Cng());
			HashAlgorithmsManager.RegisterHashAlgorithm(SignatureOids.MD5RSA, new MD5CryptoServiceProvider());
		}

		internal static HashAlgorithm CreateHashAlgorithm(Oid algorithmOid)
		{
			Guard.ThrowExceptionIfNull<Oid>(algorithmOid, "algorithmOid");
			HashAlgorithm result;
			if (!HashAlgorithmsManager.store.TryGetValue(algorithmOid.Value, out result))
			{
				Oid mainAlgorithmOid = SignatureOids.GetMainAlgorithmOid(algorithmOid);
				if (mainAlgorithmOid != null)
				{
					HashAlgorithmsManager.store.TryGetValue(mainAlgorithmOid.Value, out result);
				}
			}
			return result;
		}

		public static void RegisterHashAlgorithm(Oid oid, HashAlgorithm hashAlgorithm)
		{
			Guard.ThrowExceptionIfNull<Oid>(oid, "Oid");
			Guard.ThrowExceptionIfNullOrEmpty(oid.Value, "oid.Value");
			Guard.ThrowExceptionIfNull<HashAlgorithm>(hashAlgorithm, "hashAlgorithm");
			HashAlgorithmsManager.store.Add(oid.Value, hashAlgorithm);
		}

		public static bool HasRegisteredHashAlgorithm(Oid oid)
		{
			Guard.ThrowExceptionIfNull<Oid>(oid, "oid");
			return HashAlgorithmsManager.store.ContainsKey(oid.Value);
		}

		public static void UnregisterHashAlgorithm(Oid oid)
		{
			if (HashAlgorithmsManager.HasRegisteredHashAlgorithm(oid))
			{
				HashAlgorithmsManager.store.Remove(oid.Value);
			}
		}

		static Dictionary<string, HashAlgorithm> store = new Dictionary<string, HashAlgorithm>();
	}
}
