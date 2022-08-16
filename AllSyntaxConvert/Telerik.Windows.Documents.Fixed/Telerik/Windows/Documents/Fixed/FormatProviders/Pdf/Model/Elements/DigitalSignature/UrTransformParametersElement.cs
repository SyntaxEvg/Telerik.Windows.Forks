using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.DigitalSignatures;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DigitalSignature
{
	class UrTransformParametersElement : TransformParametersElement<UrTransformParameters>
	{
		public UrTransformParametersElement()
		{
			this.message = base.RegisterDirectProperty<PdfString>(new PdfPropertyDescriptor("Msg"));
			this.annots = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Annots"));
			this.formFieldUsage = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Form"));
		}

		public PdfString Message
		{
			get
			{
				return this.message.GetValue();
			}
			set
			{
				this.message.SetValue(value);
			}
		}

		public PdfArray Annotations
		{
			get
			{
				return this.annots.GetValue();
			}
			set
			{
				this.annots.SetValue(value);
			}
		}

		public PdfArray FormFieldUsage
		{
			get
			{
				return this.formFieldUsage.GetValue();
			}
			set
			{
				this.formFieldUsage.SetValue(value);
			}
		}

		protected override void CopyPropertiesTo(UrTransformParameters transformParameters)
		{
			this.Message.CopyToProperty(transformParameters.Message, (PdfString message) => message.ToString());
		}

		readonly DirectProperty<PdfString> message;

		readonly DirectProperty<PdfArray> annots;

		readonly DirectProperty<PdfArray> formFieldUsage;
	}
}
