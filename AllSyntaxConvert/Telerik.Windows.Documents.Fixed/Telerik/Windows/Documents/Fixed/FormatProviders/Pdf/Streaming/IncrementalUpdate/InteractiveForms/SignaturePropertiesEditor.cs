using System;
using System.Security.Cryptography.X509Certificates;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DigitalSignature;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.DigitalSignatures;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming.IncrementalUpdate.InteractiveForms
{
	class SignaturePropertiesEditor : SingleStateAppearanceFieldPropertiesEditor
	{
		public SignaturePropertiesEditor(PdfIncrementalStreamExportContext context, PdfFormFieldSource fieldSource)
			: base(context, fieldSource)
		{
		}

		public void SetValue(X509Certificate2 certificate)
		{
			Guard.ThrowExceptionIfNull<X509Certificate2>(certificate, "certificate");
			Signature signature = new Signature(certificate);
			signature.Properties.FieldName = base.Field.FieldName;
			this.signatureValue = new SignatureObject();
			this.signatureValue.CopyPropertiesFrom(signature);
			base.Context.SignatureToUpdate = signature;
			if (base.Field.PdfDictionary.ContainsKey("V"))
			{
				IndirectReference indirectReference = (IndirectReference)base.Field.PdfDictionary["V"];
				base.Context.RegisterIndirectReference(this.signatureValue, indirectReference.ObjectNumber, true);
				return;
			}
			IndirectReference reference = base.Context.CreateIndirectObject(this.signatureValue).Reference;
			PdfDictionary clonedFieldDictionary = base.GetClonedFieldDictionary();
			clonedFieldDictionary["V"] = reference;
		}

		SignatureObject signatureValue;
	}
}
