using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	public class SignatureValidationResult
	{
		public string FieldName { get; internal set; }

		public bool IsDocumentModified { get; internal set; }

		public bool IsCertificateValid { get; internal set; }

		public X509Certificate2Collection Certificates { get; internal set; }

		public X509ChainElementCollection CertificatesChainElements { get; internal set; }

		public string SignerInformation { get; internal set; }

		public Oid HashAlgorithm { get; internal set; }
	}
}
