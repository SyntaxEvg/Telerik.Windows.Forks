using System;
using Telerik.Windows.Documents.Fixed.Model.Common;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	class SignatureReference
	{
		public SignatureReference()
		{
			this.TransformMethod = new PdfProperty<string>();
			this.TransformParameters = new PdfProperty<TransformParameters>();
			this.DigestMethod = new PdfProperty<string>();
			this.DigestValue = new PdfProperty<byte[]>();
			this.DigestLocation = new PdfProperty<int[]>();
		}

		internal PdfProperty<string> TransformMethod { get; set; }

		internal PdfProperty<TransformParameters> TransformParameters { get; set; }

		internal PdfProperty<string> DigestMethod { get; set; }

		internal PdfProperty<byte[]> DigestValue { get; set; }

		internal PdfProperty<int[]> DigestLocation { get; set; }
	}
}
