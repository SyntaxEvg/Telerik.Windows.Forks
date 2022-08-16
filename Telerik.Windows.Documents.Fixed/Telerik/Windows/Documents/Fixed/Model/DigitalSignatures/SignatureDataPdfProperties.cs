using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Telerik.Windows.Documents.Fixed.Model.Common;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	class SignatureDataPdfProperties
	{
		internal PdfProperty<string> Filter { get; set; }

		internal PdfProperty<string> SubFilter { get; set; }

		internal PdfProperty<byte[]> Contents { get; set; }

		internal PdfProperty<X509Certificate2Collection> Cert { get; set; }

		internal PdfProperty<int[]> ByteRange { get; set; }

		internal PdfProperty<List<SignatureReference>> Reference { get; set; }

		internal PdfProperty<int[]> Changes { get; set; }

		internal PdfProperty<string> Name { get; set; }

		internal PdfProperty<string> Location { get; set; }

		internal PdfProperty<DateTime> TimeOfSigning { get; set; }

		internal PdfProperty<string> Reason { get; set; }

		internal PdfProperty<string> ContactInfo { get; set; }

		internal PdfProperty<int> SignatureHandlerVersion { get; set; }

		internal PdfProperty<int> SignatureDictionaryVersion { get; set; }

		public SignatureDataPdfProperties()
		{
			this.Filter = new PdfProperty<string>();
			this.SubFilter = new PdfProperty<string>();
			this.Contents = new PdfProperty<byte[]>();
			this.Cert = new PdfProperty<X509Certificate2Collection>();
			this.ByteRange = new PdfProperty<int[]>();
			this.Reference = new PdfProperty<List<SignatureReference>>();
			this.Changes = new PdfProperty<int[]>();
			this.Name = new PdfProperty<string>();
			this.Location = new PdfProperty<string>();
			this.TimeOfSigning = new PdfProperty<DateTime>();
			this.Reason = new PdfProperty<string>();
			this.ContactInfo = new PdfProperty<string>();
			this.SignatureHandlerVersion = new PdfProperty<int>();
			this.SignatureDictionaryVersion = new PdfProperty<int>();
		}
	}
}
