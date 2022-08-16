using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	public class SignatureValidationResultBuilder
	{
		public SignatureValidationResultBuilder()
			: this(new SignatureValidationResult())
		{
		}

		internal SignatureValidationResultBuilder(SignatureValidationResult validationResult)
		{
			this.validationResult = validationResult;
		}

		public void BuildFieldName(string fieldName)
		{
			this.validationResult.FieldName = fieldName;
		}

		public void BuildIsDocumentModified(bool isDocumentModified)
		{
			this.validationResult.IsDocumentModified = isDocumentModified;
		}

		public void BuildIsCertificateValid(bool isCertificateValid)
		{
			this.validationResult.IsCertificateValid = isCertificateValid;
		}

		public void BuildCertificates(X509Certificate2Collection certificates)
		{
			this.validationResult.Certificates = certificates;
		}

		public void BuildCertificatesChainElements(X509ChainElementCollection certificatesChainElements)
		{
			this.validationResult.CertificatesChainElements = certificatesChainElements;
		}

		public void BuildSignerInformation(string signerInformation)
		{
			if (!string.IsNullOrEmpty(signerInformation))
			{
				this.validationResult.SignerInformation = signerInformation;
			}
		}

		public void BuildHashAlgorithm(Oid hashAlgorithm)
		{
			this.validationResult.HashAlgorithm = hashAlgorithm;
		}

		public SignatureValidationResult GetResult()
		{
			return this.validationResult;
		}

		readonly SignatureValidationResult validationResult;
	}
}
