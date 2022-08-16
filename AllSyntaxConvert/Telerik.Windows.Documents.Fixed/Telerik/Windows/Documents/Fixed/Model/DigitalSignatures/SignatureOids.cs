using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	static class SignatureOids
	{
		static SignatureOids()
		{
			SignatureOids.algorithmNames[SignatureOids.SHA1RSA.Value] = SignatureOids.SHA1;
			SignatureOids.algorithmNames[SignatureOids.SHA256RSA.Value] = SignatureOids.SHA256;
			SignatureOids.algorithmNames[SignatureOids.SHA384RSA.Value] = SignatureOids.SHA384;
			SignatureOids.algorithmNames[SignatureOids.SHA512RSA.Value] = SignatureOids.SHA512;
			SignatureOids.algorithmNames[SignatureOids.SHA1DSA.Value] = SignatureOids.SHA1;
			SignatureOids.algorithmNames["1.2.840.113549.2.5"] = SignatureOids.MD5;
			SignatureOids.algorithmNames["1.2.840.113549.2.2"] = SignatureOids.MD2;
			SignatureOids.algorithmNames["2.16.840.1.101.3.4.3.2"] = SignatureOids.SHA256;
			SignatureOids.algorithmNames["2.16.840.1.101.3.4.3.3"] = SignatureOids.SHA384;
			SignatureOids.algorithmNames["2.16.840.1.101.3.4.3.4"] = SignatureOids.SHA512;
			SignatureOids.algorithmNames["1.3.36.3.3.1.2"] = SignatureOids.RIPEMD160;
		}

		internal static Oid GetMainAlgorithmOid(Oid oid)
		{
			Oid result;
			if (SignatureOids.algorithmNames.TryGetValue(oid.Value, out result))
			{
				return result;
			}
			return oid;
		}

		internal static Oid SHA1 = new Oid("1.3.14.3.2.26", "SHA1");

		internal static Oid SHA256 = new Oid("2.16.840.1.101.3.4.2.1", "SHA256");

		internal static Oid SHA512 = new Oid("2.16.840.1.101.3.4.2.3", "SHA512");

		internal static Oid SHA384 = new Oid("2.16.840.1.101.3.4.2.2", "SHA384");

		internal static Oid MD5 = new Oid("1.2.840.113549.2.5", "MD5");

		internal static Oid MD2 = new Oid("1.2.840.113549.2.2", "MD2");

		internal static Oid MD2RSA = new Oid("1.2.840.113549.1.1.2", "MD2RSA");

		internal static Oid MD5RSA = new Oid("1.2.840.113549.1.1.4", "MD5RSA");

		internal static Oid SHA1RSA = new Oid("1.2.840.113549.1.1.5", "SHA1RSA");

		internal static Oid SHA256RSA = new Oid("1.2.840.113549.1.1.11", "SHA256RSA");

		internal static Oid SHA384RSA = new Oid("1.2.840.113549.1.1.12", "SHA384RSA");

		internal static Oid SHA512RSA = new Oid("1.2.840.113549.1.1.13", "SHA512RSA");

		internal static Oid SHA1DSA = new Oid("1.2.840.10040.4.3", "SHA1DSA");

		internal static Oid RIPEMD160 = new Oid("1.3.36.3.2.1", "RIPEMD160");

		internal static Oid PKCS7_DATA = new Oid("1.2.840.113549.1.7.1");

		internal static Oid PKCS7_SIGNED_DATA = new Oid("1.2.840.113549.1.7.2");

		internal static Oid CONTENT_TYPE = new Oid("1.2.840.113549.1.9.3", "Content Type");

		internal static Oid MESSAGE_DIGEST = new Oid("1.2.840.113549.1.9.4", "Message Digest");

		internal static Oid SIGNING_TIME = new Oid("1.2.840.113549.1.9.5", "Signing Time");

		static Dictionary<string, Oid> algorithmNames = new Dictionary<string, Oid>();
	}
}
