using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.DigitalSignatures;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DigitalSignature
{
	class SignatureObject : PdfObject
	{
		public SignatureObject()
		{
			this.filter = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("Filter"));
			this.subFilter = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("SubFilter"));
			this.byteRange = base.RegisterDirectProperty<SignatureByteRange>(new PdfPropertyDescriptor("ByteRange"));
			this.contents = base.RegisterDirectProperty<SignatureContents>(new PdfPropertyDescriptor("Contents"));
			this.cert = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Cert"));
			this.reference = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("Reference"));
			this.changes = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Changes"));
			this.name = base.RegisterDirectProperty<PdfString>(new PdfPropertyDescriptor("Name"));
			this.timeOfSigning = base.RegisterDirectProperty<PdfString>(new PdfPropertyDescriptor("M"));
			this.location = base.RegisterDirectProperty<PdfString>(new PdfPropertyDescriptor("Location"));
			this.reason = base.RegisterDirectProperty<PdfString>(new PdfPropertyDescriptor("Reason"));
			this.contactInfo = base.RegisterDirectProperty<PdfString>(new PdfPropertyDescriptor("ContactInfo"));
			this.signatureHandlerVersion = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("R"));
			this.signatureDictionaryVersion = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("V"));
		}

		public PdfName Filter
		{
			get
			{
				return this.filter.GetValue();
			}
			set
			{
				this.filter.SetValue(value);
			}
		}

		public PdfName SubFilter
		{
			get
			{
				return this.subFilter.GetValue();
			}
			set
			{
				this.subFilter.SetValue(value);
			}
		}

		public SignatureContents Contents
		{
			get
			{
				return this.contents.GetValue();
			}
			set
			{
				this.contents.SetValue(value);
			}
		}

		public PdfArray Cert
		{
			get
			{
				return this.cert.GetValue();
			}
			set
			{
				this.cert.SetValue(value);
			}
		}

		public SignatureByteRange ByteRange
		{
			get
			{
				return this.byteRange.GetValue();
			}
			set
			{
				this.byteRange.SetValue(value);
			}
		}

		public PdfArray Reference
		{
			get
			{
				return this.reference.GetValue();
			}
			set
			{
				this.reference.SetValue(value);
			}
		}

		public PdfArray Changes
		{
			get
			{
				return this.changes.GetValue();
			}
			set
			{
				this.changes.SetValue(value);
			}
		}

		public PdfString Name
		{
			get
			{
				return this.name.GetValue();
			}
			set
			{
				this.name.SetValue(value);
			}
		}

		public PdfString TimeOfSigning
		{
			get
			{
				return this.timeOfSigning.GetValue();
			}
			set
			{
				this.timeOfSigning.SetValue(value);
			}
		}

		public PdfString Location
		{
			get
			{
				return this.location.GetValue();
			}
			set
			{
				this.location.SetValue(value);
			}
		}

		public PdfString Reason
		{
			get
			{
				return this.reason.GetValue();
			}
			set
			{
				this.reason.SetValue(value);
			}
		}

		public PdfString ContactInfo
		{
			get
			{
				return this.contactInfo.GetValue();
			}
			set
			{
				this.contactInfo.SetValue(value);
			}
		}

		public PdfInt SignatureHandlerVersion
		{
			get
			{
				return this.signatureHandlerVersion.GetValue();
			}
			set
			{
				this.signatureHandlerVersion.SetValue(value);
			}
		}

		public PdfInt SignatureDictionaryVersion
		{
			get
			{
				return this.signatureDictionaryVersion.GetValue();
			}
			set
			{
				this.signatureDictionaryVersion.SetValue(value);
			}
		}

		internal void CopyPropertiesTo(Signature signature, PostScriptReader reader, IRadFixedDocumentImportContext context)
		{
			Guard.ThrowExceptionIfNull<Signature>(signature, "signature");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IRadFixedDocumentImportContext>(context, "context");
			SignatureDataPdfProperties pdfProperties = signature.Properties.PdfProperties;
			this.Filter.CopyToProperty(pdfProperties.Filter, (PdfName filter) => filter.Value);
			this.SubFilter.CopyToProperty(pdfProperties.SubFilter, (PdfName subFilter) => subFilter.Value);
			this.Contents.CopyToProperty(pdfProperties.Contents, (SignatureContents contents) => contents.Value);
			this.Cert.CopyToProperty(pdfProperties.Cert, (PdfArray cert) => this.ToCertificateCollection(cert));
			this.ByteRange.CopyToProperty(pdfProperties.ByteRange, (SignatureByteRange byteRange) => ((PdfArray)byteRange.Primitive).ToIntArray());
			this.Reference.CopyToProperty(pdfProperties.Reference, (PdfArray reference) => SignatureObject.ReferenceToModelElements(reference, reader, context));
			this.Changes.CopyToProperty(pdfProperties.Changes, (PdfArray changes) => changes.ToIntArray());
			this.Name.CopyToProperty(pdfProperties.Name, (PdfString name) => name.ToString());
			if (this.TimeOfSigning != null)
			{
				DateTime? dateTime = SignatureDateCodec.Decode(this.TimeOfSigning.ToString());
				if (dateTime != null)
				{
					pdfProperties.TimeOfSigning.Value = dateTime.Value;
				}
			}
			this.Location.CopyToProperty(pdfProperties.Location, (PdfString location) => location.ToString());
			this.Reason.CopyToProperty(pdfProperties.Reason, (PdfString reason) => reason.ToString());
			this.ContactInfo.CopyToProperty(pdfProperties.ContactInfo, (PdfString contactInfo) => contactInfo.ToString());
			this.SignatureHandlerVersion.CopyToProperty(pdfProperties.SignatureHandlerVersion, (PdfInt signatureHandlerVersion) => signatureHandlerVersion.Value);
			this.SignatureDictionaryVersion.CopyToProperty(pdfProperties.SignatureDictionaryVersion, (PdfInt signatureDictionaryVersion) => signatureDictionaryVersion.Value);
			FileSourceStream sourceStream = new FileSourceStream(reader.Reader.Stream);
			signature.Properties.SourceStream = sourceStream;
		}

		public void CopyPropertiesFrom(Signature signature)
		{
			Guard.ThrowExceptionIfNull<Signature>(signature, "signature");
			Guard.ThrowExceptionIfNull<SignatureDataProperties>(signature.Properties, "signature.Properties");
			this.signatureFieldName = signature.Properties.FieldName;
			SignatureDataPdfProperties pdfProperties = signature.Properties.PdfProperties;
			this.Filter = pdfProperties.Filter.ToPrimitive((string filter) => filter.ToPdfName(), null);
			this.SubFilter = pdfProperties.SubFilter.ToPrimitive((string filter) => filter.ToPdfName(), null);
			this.Contents = pdfProperties.Contents.ToPrimitive((byte[] contents) => new SignatureContents(contents), null);
			this.Cert = pdfProperties.Cert.ToPrimitive((X509Certificate2Collection cert) => this.FromCertificateCollection(cert), null);
			this.ByteRange = SignatureByteRange.ToSignatureByteRange(pdfProperties.ByteRange);
			this.Reference = SignatureObject.ToPdfArray(pdfProperties.Reference.Value);
			this.Changes = pdfProperties.Changes.ToPrimitive((int[] changes) => changes.ToPdfArray(), null);
			this.Name = pdfProperties.Name.ToPrimitive((string name) => name.ToPdfLiteralString(StringType.ASCII), null);
			this.TimeOfSigning = pdfProperties.TimeOfSigning.ToPrimitive((DateTime timeOfSigning) => SignatureDateCodec.Encode(timeOfSigning).ToPdfLiteralString(StringType.ASCII), null);
			this.Location = pdfProperties.Location.ToPrimitive((string location) => location.ToPdfLiteralString(StringType.ASCII), null);
			this.Reason = pdfProperties.Reason.ToPrimitive((string reason) => reason.ToPdfLiteralString(StringType.ASCII), null);
			this.ContactInfo = pdfProperties.ContactInfo.ToPrimitive((string contactInfo) => contactInfo.ToPdfLiteralString(StringType.ASCII), null);
			this.SignatureHandlerVersion = pdfProperties.SignatureHandlerVersion.ToPrimitive((int signatureHandlerVersion) => signatureHandlerVersion.ToPdfInt(), null);
			this.SignatureDictionaryVersion = pdfProperties.SignatureDictionaryVersion.ToPrimitive((int signatureDictionaryVersion) => signatureDictionaryVersion.ToPdfInt(), null);
		}

		static List<SignatureReference> ReferenceToModelElements(PdfArray array, PostScriptReader reader, IRadFixedDocumentImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "array");
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IRadFixedDocumentImportContext>(context, "context");
			List<SignatureReference> list = new List<SignatureReference>();
			foreach (PdfPrimitive primitive in array)
			{
				SignatureReference signatureReference = new SignatureReference();
				SignatureReferenceObject signatureReferenceObject = new SignatureReferenceObject();
				signatureReferenceObject.Load(reader, context, primitive);
				signatureReferenceObject.CopyPropertiesTo(signatureReference, reader, context);
				list.Add(signatureReference);
			}
			return list;
		}

		static PdfArray ToPdfArray(IEnumerable<SignatureReference> array)
		{
			if (array == null)
			{
				return null;
			}
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
			foreach (SignatureReference signatureReference in array)
			{
				SignatureReferenceObject signatureReferenceObject = new SignatureReferenceObject();
				signatureReferenceObject.CopyPropertiesFrom(signatureReference);
				pdfArray.Add(signatureReferenceObject);
			}
			return pdfArray;
		}

		X509Certificate2Collection ToCertificateCollection(PdfArray array)
		{
			X509Certificate2Collection x509Certificate2Collection = new X509Certificate2Collection();
			for (int i = 0; i < array.Count; i++)
			{
				byte[] value = ((PdfString)array[i]).Value;
				X509Certificate2 certificate = new X509Certificate2(value);
				x509Certificate2Collection.Add(certificate);
			}
			return x509Certificate2Collection;
		}

		PdfArray FromCertificateCollection(X509Certificate2Collection certificateCollection)
		{
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
			for (int i = 0; i < certificateCollection.Count; i++)
			{
				PdfLiteralString item = new PdfLiteralString(certificateCollection[i].RawData);
				pdfArray.Add(item);
			}
			return pdfArray;
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			context.SignatureExportInfo.SetSignatureFieldName(this.signatureFieldName);
			base.Write(writer, context);
			context.SignatureExportInfo.ClearSignatureFieldName();
		}

		readonly DirectProperty<PdfName> filter;

		readonly DirectProperty<PdfName> subFilter;

		readonly DirectProperty<SignatureContents> contents;

		readonly DirectProperty<PdfArray> cert;

		readonly DirectProperty<SignatureByteRange> byteRange;

		readonly ReferenceProperty<PdfArray> reference;

		readonly DirectProperty<PdfArray> changes;

		readonly DirectProperty<PdfString> name;

		readonly DirectProperty<PdfString> location;

		readonly DirectProperty<PdfString> reason;

		readonly DirectProperty<PdfString> contactInfo;

		readonly DirectProperty<PdfString> timeOfSigning;

		readonly DirectProperty<PdfInt> signatureHandlerVersion;

		readonly DirectProperty<PdfInt> signatureDictionaryVersion;

		string signatureFieldName;
	}
}
