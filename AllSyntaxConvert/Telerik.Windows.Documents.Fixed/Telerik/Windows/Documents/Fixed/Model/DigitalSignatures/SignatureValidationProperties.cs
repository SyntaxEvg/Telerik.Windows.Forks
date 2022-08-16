using System;
using System.Security.Cryptography.X509Certificates;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	public class SignatureValidationProperties
	{
		public SignatureValidationProperties()
		{
			this.Chain = new X509Chain();
			this.Chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
			this.ChainStatusFlags = X509ChainStatusFlags.Revoked | X509ChainStatusFlags.NotSignatureValid | X509ChainStatusFlags.UntrustedRoot | X509ChainStatusFlags.Cyclic | X509ChainStatusFlags.InvalidBasicConstraints | X509ChainStatusFlags.PartialChain | X509ChainStatusFlags.CtlNotSignatureValid;
		}

		public X509Chain Chain { get; set; }

		public X509ChainStatusFlags ChainStatusFlags { get; set; }
	}
}
