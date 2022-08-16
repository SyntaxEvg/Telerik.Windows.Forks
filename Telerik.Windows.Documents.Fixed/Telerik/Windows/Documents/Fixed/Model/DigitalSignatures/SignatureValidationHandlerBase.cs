using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	public abstract class SignatureValidationHandlerBase
	{
		public SignatureValidationHandlerBase()
		{
		}

		SignatureDataProperties DataProperties { get; set; }

		SignatureValidationProperties ValidationProperties { get; set; }

		protected abstract SignatureValidationResult ValidateOverride(SignatureDataProperties dataProperties, SignatureValidationProperties validationProperties);

		internal SignatureValidationResult Validate(SignatureDataProperties dataProperties, SignatureValidationProperties validationProperties)
		{
			this.DataProperties = dataProperties;
			this.ValidationProperties = validationProperties;
			SignatureValidationResult signatureValidationResult = this.ValidateOverride(dataProperties, validationProperties);
			if (string.IsNullOrEmpty(signatureValidationResult.FieldName))
			{
				SignatureValidationResultBuilder signatureValidationResultBuilder = new SignatureValidationResultBuilder(signatureValidationResult);
				signatureValidationResultBuilder.BuildFieldName(this.DataProperties.FieldName);
			}
			return signatureValidationResult;
		}

		internal byte[] ComputeHash(Oid hashAlgorithmOid)
		{
			HashAlgorithm hashAlgorithm = HashAlgorithmsManager.CreateHashAlgorithm(hashAlgorithmOid);
			return this.ComputeHash(hashAlgorithm);
		}

		internal byte[] ComputeHash(HashAlgorithm hashAlgorithm)
		{
			byte[] byteArrayCompositionForHash = this.GetByteArrayCompositionForHash();
			return hashAlgorithm.ComputeHash(byteArrayCompositionForHash);
		}

		protected byte[] GetByteArrayCompositionForHash()
		{
			IEnumerable<SourcePart> sourceParts = this.GetSourceParts();
			ByteRangeComposer byteRangeComposer = new ByteRangeComposer(this.DataProperties.SourceStream, sourceParts);
			return byteRangeComposer.Compose();
		}

		internal IEnumerable<SourcePart> GetSourceParts()
		{
			int[] byteRange = this.DataProperties.ByteRange;
			List<SourcePart> list = new List<SourcePart>(byteRange.Length / 2);
			for (int i = 0; i < byteRange.Length; i += 2)
			{
				int offset = byteRange[i];
				int length = byteRange[i + 1];
				SourcePart item = new SourcePart(offset, length);
				list.Add(item);
			}
			return list;
		}

		internal bool ValidateHash(byte[] originalHash, Oid hashAlgorithmOid)
		{
			byte[] computedHash = this.ComputeHash(hashAlgorithmOid);
			return this.ValidateHash(originalHash, computedHash);
		}

		internal bool ValidateHash(byte[] originalHash, byte[] computedHash)
		{
			for (int i = 0; i < originalHash.Length; i++)
			{
				if (originalHash[i] != computedHash[i])
				{
					return false;
				}
			}
			return true;
		}

		internal void ValidateCertificates(SignatureValidationResultBuilder validationResultBuilder, X509Certificate2Collection certificateCollection)
		{
			X509Certificate2 signingCertificate = this.GetSigningCertificate(certificateCollection);
			if (signingCertificate != null)
			{
				bool flag = this.ValidationProperties.Chain.Build(signingCertificate);
				if (flag)
				{
					validationResultBuilder.BuildIsCertificateValid(true);
					return;
				}
				bool isCertificateValid = !(from ch in this.ValidationProperties.Chain.ChainStatus
					where this.ValidationProperties.ChainStatusFlags.HasFlag(ch.Status)
					select ch).Any<X509ChainStatus>();
				validationResultBuilder.BuildIsCertificateValid(isCertificateValid);
				validationResultBuilder.BuildCertificatesChainElements(this.ValidationProperties.Chain.ChainElements);
			}
		}

		internal void SetResultHashAlgorithm(SignatureValidationResultBuilder validationResultBuilder, Oid digestAlgorithm)
		{
			Oid mainAlgorithmOid = SignatureOids.GetMainAlgorithmOid(digestAlgorithm);
			validationResultBuilder.BuildHashAlgorithm(mainAlgorithmOid);
		}

		internal X509Certificate2 GetSigningCertificate(X509Certificate2Collection certificateCollection)
		{
			X509Certificate2 result = null;
			if (certificateCollection.Count == 1)
			{
				result = certificateCollection[0];
			}
			else
			{
				foreach (X509Certificate2 x509Certificate in certificateCollection)
				{
					bool flag = this.IsIssuerCertificate(certificateCollection, x509Certificate);
					if (flag)
					{
						this.ValidationProperties.Chain.ChainPolicy.ExtraStore.Add(x509Certificate);
					}
					else
					{
						result = x509Certificate;
					}
				}
			}
			return result;
		}

		bool IsIssuerCertificate(X509Certificate2Collection certificateCollection, X509Certificate2 issuerCertificateCandidate)
		{
			foreach (X509Certificate2 x509Certificate in certificateCollection)
			{
				if (x509Certificate.IssuerName.Name == issuerCertificateCandidate.SubjectName.Name)
				{
					return true;
				}
			}
			return false;
		}
	}
}
