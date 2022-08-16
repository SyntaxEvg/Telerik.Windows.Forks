using System;
using System.Security.Cryptography.Pkcs;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	public class Pkcs7 : SignatureValidationHandlerBase
	{
		protected override SignatureValidationResult ValidateOverride(SignatureDataProperties dataProperties, SignatureValidationProperties validationProperties)
		{
			SignatureValidationResultBuilder signatureValidationResultBuilder = new SignatureValidationResultBuilder();
			SignedCms signedCms = new SignedCms();
			signedCms.Decode(dataProperties.Contents);
			base.ValidateCertificates(signatureValidationResultBuilder, signedCms.Certificates);
			signatureValidationResultBuilder.BuildCertificates(signedCms.Certificates);
			if (signedCms.SignerInfos.Count == 1)
			{
				SignerInfo signerInfo = signedCms.SignerInfos[0];
				base.SetResultHashAlgorithm(signatureValidationResultBuilder, signerInfo.DigestAlgorithm);
				bool flag = base.ValidateHash(signedCms.ContentInfo.Content, signerInfo.DigestAlgorithm);
				signatureValidationResultBuilder.BuildIsDocumentModified(!flag);
				signatureValidationResultBuilder.BuildSignerInformation(dataProperties.Name);
				return signatureValidationResultBuilder.GetResult();
			}
			throw new NotSupportedException("There is more then one signer specified for a single signature.");
		}
	}
}
