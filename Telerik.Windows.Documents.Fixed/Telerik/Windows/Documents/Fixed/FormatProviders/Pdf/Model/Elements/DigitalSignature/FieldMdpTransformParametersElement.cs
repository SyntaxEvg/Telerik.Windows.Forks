using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.DigitalSignatures;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DigitalSignature
{
	class FieldMdpTransformParametersElement : TransformParametersElement<FieldMdpTransformParameters>
	{
		public FieldMdpTransformParametersElement()
		{
			this.action = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("Action"));
			this.fields = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Fields"));
		}

		public PdfName Action
		{
			get
			{
				return this.action.GetValue();
			}
			set
			{
				this.action.SetValue(value);
			}
		}

		public PdfArray Fields
		{
			get
			{
				return this.fields.GetValue();
			}
			set
			{
				this.fields.SetValue(value);
			}
		}

		protected override void CopyPropertiesTo(FieldMdpTransformParameters transformParameters)
		{
			this.Action.CopyToProperty(transformParameters.Action, (PdfName action) => action.Value);
		}

		readonly DirectProperty<PdfName> action;

		readonly DirectProperty<PdfArray> fields;
	}
}
