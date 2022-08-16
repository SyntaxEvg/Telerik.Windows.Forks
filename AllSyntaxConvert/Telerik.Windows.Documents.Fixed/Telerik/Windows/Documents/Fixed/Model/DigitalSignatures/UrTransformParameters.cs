using System;
using Telerik.Windows.Documents.Fixed.Model.Common;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	class UrTransformParameters : TransformParameters
	{
		public UrTransformParameters()
		{
			this.Message = new PdfProperty<string>();
		}

		internal PdfProperty<string> Message { get; set; }
	}
}
