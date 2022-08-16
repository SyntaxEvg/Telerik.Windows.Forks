using System;
using Telerik.Windows.Documents.Fixed.Model.Common;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	class FieldMdpTransformParameters : TransformParameters
	{
		public FieldMdpTransformParameters()
		{
			this.Action = new PdfProperty<string>();
		}

		internal PdfProperty<string> Action { get; set; }
	}
}
