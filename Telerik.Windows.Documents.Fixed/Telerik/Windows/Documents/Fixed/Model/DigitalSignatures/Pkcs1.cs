using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Asn1;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	public class Pkcs1 : SignatureValidationHandlerBase
	{
		protected override SignatureValidationResult ValidateOverride(SignatureDataProperties dataProperties, SignatureValidationProperties validationProperties)
		{
			SignatureValidationResultBuilder signatureValidationResultBuilder = new SignatureValidationResultBuilder();
			if (dataProperties.Certificates != null && dataProperties.Certificates.Count > 0)
			{
				signatureValidationResultBuilder.BuildCertificates(dataProperties.Certificates);
				X509Certificate2 signingCertificate = base.GetSigningCertificate(dataProperties.Certificates);
				RSACryptoServiceProvider rsacryptoServiceProvider = (RSACryptoServiceProvider)signingCertificate.PublicKey.Key;
				Asn1InputStream asn1InputStream = new Asn1InputStream(new MemoryStream(dataProperties.Contents));
				Asn1OctetString asn1OctetString = (Asn1OctetString)asn1InputStream.ReadObject();
				byte[] octets = asn1OctetString.GetOctets();
				byte[] byteArrayCompositionForHash = base.GetByteArrayCompositionForHash();
				Oid mainAlgorithmOid = SignatureOids.GetMainAlgorithmOid(signingCertificate.SignatureAlgorithm);
				base.SetResultHashAlgorithm(signatureValidationResultBuilder, mainAlgorithmOid);
				bool flag = rsacryptoServiceProvider.VerifyData(byteArrayCompositionForHash, mainAlgorithmOid.Value, octets);
				signatureValidationResultBuilder.BuildIsDocumentModified(!flag);
				base.ValidateCertificates(signatureValidationResultBuilder, dataProperties.Certificates);
				signatureValidationResultBuilder.BuildSignerInformation(dataProperties.Name);
				return signatureValidationResultBuilder.GetResult();
			}
			throw new NotSupportedException("No certificates are specified in the signature \"Cert\" property.");
		}
	}
}
