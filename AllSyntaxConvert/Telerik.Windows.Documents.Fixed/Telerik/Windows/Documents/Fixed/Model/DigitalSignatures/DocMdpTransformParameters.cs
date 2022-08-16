using System;
using Telerik.Windows.Documents.Fixed.Model.Common;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	class DocMdpTransformParameters : TransformParameters
	{
		public DocMdpTransformParameters()
		{
			this.AccessPermission = new PdfProperty<int>();
		}

		internal PdfProperty<int> AccessPermission { get; set; }
	}
}
