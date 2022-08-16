using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	public class Signature
	{
		public Signature(X509Certificate2 certificate)
			: this()
		{
			Guard.ThrowExceptionIfNull<X509Certificate2>(certificate, "certificate");
			this.certificate = certificate;
			this.BuildInitalSignState();
		}

		internal Signature()
		{
			this.Properties = new SignatureDataProperties();
		}

		public SignatureDataProperties Properties { get; set; }

		public bool SupportsValidation
		{
			get
			{
				return this.certificate == null;
			}
		}

		internal bool CanSign
		{
			get
			{
				return this.certificate != null;
			}
		}

		public bool TryValidate(out SignatureValidationResult validationResult)
		{
			try
			{
				validationResult = this.Validate();
				return true;
			}
			catch
			{
			}
			validationResult = null;
			return false;
		}

		public bool TryValidate(SignatureValidationProperties validationProperties, out SignatureValidationResult validationResult)
		{
			Guard.ThrowExceptionIfNull<SignatureValidationProperties>(validationProperties, "validationProperties");
			try
			{
				validationResult = this.Validate(validationProperties);
				return true;
			}
			catch
			{
			}
			validationResult = null;
			return false;
		}

		public SignatureValidationResult Validate()
		{
			SignatureValidationProperties validationProperties = new SignatureValidationProperties();
			return this.Validate(validationProperties);
		}

		public SignatureValidationResult Validate(SignatureValidationProperties validationProperties)
		{
			Guard.ThrowExceptionIfNull<SignatureValidationProperties>(validationProperties, "validationProperties");
			if (!this.Properties.SourceStream.CanRead)
			{
				throw new InvalidOperationException("Document source stream does not support reading. Ensure that the stream is opened.");
			}
			if (!this.Properties.SourceStream.CanSeek)
			{
				throw new InvalidOperationException("Document source stream does not support seek operations. Ensure that the stream is opened.");
			}
			SignatureValidationHandlerBase handler = SignatureValidationHandlersManager.GetHandler(this.Properties.SubFilter);
			if (handler == null)
			{
				throw new NotSupportedException(string.Format("No signature validation handler was found for the subfilter: {0}", this.Properties.SubFilter));
			}
			SignatureValidationResult signatureValidationResult = handler.Validate(this.Properties, validationProperties);
			if (signatureValidationResult == null)
			{
				throw new NotSupportedException("No signature validation result was constucted during the validation");
			}
			return signatureValidationResult;
		}

		internal void BuildInitalSignState()
		{
			this.Properties.SubFilter = "adbe.pkcs7.detached";
			byte[] byteArrayCompositionForHash = new byte[] { 1 };
			byte[] contents = this.CalculateContentsPackage(byteArrayCompositionForHash);
			this.Properties.Contents = contents;
			this.Properties.TimeOfSigning = DateTime.Now;
			int[] byteRange = new int[] { int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue };
			this.Properties.ByteRange = byteRange;
		}

		internal byte[] CalculateContentsPackage(byte[] byteArrayCompositionForHash)
		{
			ContentInfo contentInfo = new ContentInfo(byteArrayCompositionForHash);
			SignedCms signedCms = new SignedCms(contentInfo, true);
			byte[] result = null;
			try
			{
				result = this.GetContentPackage(signedCms, X509IncludeOption.WholeChain);
			}
			catch (CryptographicException)
			{
				result = this.GetContentPackage(signedCms, X509IncludeOption.EndCertOnly);
			}
			return result;
		}

		byte[] GetContentPackage(SignedCms signedCms, X509IncludeOption includeOption)
		{
			signedCms.ComputeSignature(new CmsSigner(this.certificate)
			{
				IncludeOption = includeOption
			});
			return signedCms.Encode();
		}

		X509Certificate2 certificate;
	}
}
