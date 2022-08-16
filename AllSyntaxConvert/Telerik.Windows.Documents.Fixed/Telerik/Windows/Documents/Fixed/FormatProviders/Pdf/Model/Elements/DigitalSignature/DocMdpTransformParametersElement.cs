using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.DigitalSignatures;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DigitalSignature
{
	class DocMdpTransformParametersElement : TransformParametersElement<DocMdpTransformParameters>
	{
		public DocMdpTransformParametersElement()
		{
			this.accessPermission = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("P"), new PdfInt(2));
		}

		public PdfInt AccessPermission
		{
			get
			{
				return this.accessPermission.GetValue();
			}
			set
			{
				this.accessPermission.SetValue(value);
			}
		}

		protected override void CopyPropertiesTo(DocMdpTransformParameters transformParameters)
		{
			this.AccessPermission.CopyToProperty(transformParameters.AccessPermission, (PdfInt accessPermission) => accessPermission.Value);
		}

		readonly DirectProperty<PdfInt> accessPermission;
	}
}
