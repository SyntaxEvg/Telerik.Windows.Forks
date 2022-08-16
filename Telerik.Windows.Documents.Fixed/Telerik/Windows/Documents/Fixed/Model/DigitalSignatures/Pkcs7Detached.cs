using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	public class Pkcs7Detached : SignatureValidationHandlerBase
	{
		protected override SignatureValidationResult ValidateOverride(SignatureDataProperties dataProperties, SignatureValidationProperties validationProperties)
		{
			SignatureValidationResultBuilder signatureValidationResultBuilder = new SignatureValidationResultBuilder();
			byte[] byteArrayCompositionForHash = base.GetByteArrayCompositionForHash();
			ContentInfo contentInfo = new ContentInfo(byteArrayCompositionForHash);
			SignedCms signedCms = new SignedCms(contentInfo, true);
			signedCms.Decode(dataProperties.Contents);
			base.ValidateCertificates(signatureValidationResultBuilder, signedCms.Certificates);
			signatureValidationResultBuilder.BuildCertificates(signedCms.Certificates);
			if (signedCms.SignerInfos.Count == 1)
			{
				SignerInfo signerInfo = signedCms.SignerInfos[0];
				CryptographicAttributeObject cryptographicAttributeObject = null;
				foreach (CryptographicAttributeObject cryptographicAttributeObject2 in signerInfo.SignedAttributes)
				{
					if (cryptographicAttributeObject2.Oid.Value == SignatureOids.MESSAGE_DIGEST.Value)
					{
						cryptographicAttributeObject = cryptographicAttributeObject2;
					}
				}
				if (cryptographicAttributeObject != null)
				{
					Pkcs9MessageDigest pkcs9MessageDigest = (Pkcs9MessageDigest)cryptographicAttributeObject.Values[0];
					byte[] messageDigest = pkcs9MessageDigest.MessageDigest;
					bool flag = base.ValidateHash(messageDigest, signerInfo.DigestAlgorithm);
					signatureValidationResultBuilder.BuildIsDocumentModified(!flag);
				}
				else
				{
					try
					{
						signerInfo.CheckSignature(true);
						signatureValidationResultBuilder.BuildIsDocumentModified(false);
					}
					catch
					{
						signatureValidationResultBuilder.BuildIsDocumentModified(true);
					}
				}
				base.SetResultHashAlgorithm(signatureValidationResultBuilder, signerInfo.DigestAlgorithm);
				signatureValidationResultBuilder.BuildSignerInformation(dataProperties.Name);
				return signatureValidationResultBuilder.GetResult();
			}
			throw new NotSupportedException("There is more then one signer specified for a single signature.");
		}
	}
}
