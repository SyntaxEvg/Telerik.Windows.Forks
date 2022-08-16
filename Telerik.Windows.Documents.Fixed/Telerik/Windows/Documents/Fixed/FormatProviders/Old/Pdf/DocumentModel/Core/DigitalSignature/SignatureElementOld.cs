using System;
using System.Security.Cryptography.X509Certificates;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.DigitalSignatures;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.DigitalSignature
{
	[PdfClass(TypeName = "Sig")]
	class SignatureElementOld : PdfObjectOld
	{
		public SignatureElementOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.filter = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor
			{
				Name = "Filter",
				IsRequired = true
			});
			this.subFilter = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor
			{
				Name = "SubFilter"
			});
			this.contents = base.CreateInstantLoadProperty<PdfStringOld>(new PdfPropertyDescriptor("Contents"));
			this.cert = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor("Cert"), Converters.ArrayConverter);
			this.byteRange = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "ByteRange"
			});
			this.signatureReference = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "Reference"
			});
			this.changes = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "Changes"
			});
			this.name = base.CreateInstantLoadProperty<PdfStringOld>(new PdfPropertyDescriptor("Name"));
			this.timeOfSigning = base.CreateInstantLoadProperty<PdfStringOld>(new PdfPropertyDescriptor("M"));
			this.location = base.CreateInstantLoadProperty<PdfStringOld>(new PdfPropertyDescriptor("Location"));
			this.reason = base.CreateInstantLoadProperty<PdfStringOld>(new PdfPropertyDescriptor("Reason"));
			this.contactInfo = base.CreateInstantLoadProperty<PdfStringOld>(new PdfPropertyDescriptor("ContactInfo"));
			this.signatureHandlerVersion = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor("R"));
			this.signatureDictionaryVersion = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "V"
			});
		}

		public PdfNameOld Filter
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

		public PdfNameOld SubFilter
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

		public PdfStringOld Contents
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

		public PdfArrayOld Cert
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

		public PdfArrayOld ByteRange
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

		public PdfArrayOld SignatureReference
		{
			get
			{
				return this.signatureReference.GetValue();
			}
			set
			{
				this.signatureReference.SetValue(value);
			}
		}

		public PdfArrayOld Changes
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

		public PdfStringOld Name
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

		public PdfStringOld TimeOfSigning
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

		public PdfStringOld Location
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

		public PdfStringOld Reason
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

		public PdfStringOld ContactInfo
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

		public PdfIntOld SignatureHandlerVersion
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

		public PdfIntOld SignatureDictionaryVersion
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

		internal void CopyPropertiesTo(Signature signature)
		{
			Guard.ThrowExceptionIfNull<Signature>(signature, "signature");
			SignatureDataPdfProperties pdfProperties = signature.Properties.PdfProperties;
			this.Filter.CopyToProperty(pdfProperties.Filter, (PdfNameOld filter) => filter.Value);
			this.SubFilter.CopyToProperty(pdfProperties.SubFilter, (PdfNameOld subFilter) => subFilter.Value);
			this.Contents.CopyToProperty(pdfProperties.Contents, (PdfStringOld contents) => contents.Value);
			this.Cert.CopyToProperty(pdfProperties.Cert, (PdfArrayOld cert) => this.ToCertificateCollection(cert));
			this.ByteRange.CopyToProperty(pdfProperties.ByteRange, (PdfArrayOld byteRange) => byteRange.ToIntArray());
			this.Changes.CopyToProperty(pdfProperties.Changes, (PdfArrayOld changes) => changes.ToIntArray());
			this.Name.CopyToProperty(pdfProperties.Name, (PdfStringOld name) => name.ToString());
			if (this.TimeOfSigning != null)
			{
				DateTime? dateTime = SignatureDateCodec.Decode(this.TimeOfSigning.ToString());
				if (dateTime != null)
				{
					pdfProperties.TimeOfSigning.Value = dateTime.Value;
				}
			}
			this.Location.CopyToProperty(pdfProperties.Location, (PdfStringOld location) => location.ToString());
			this.Reason.CopyToProperty(pdfProperties.Reason, (PdfStringOld reason) => reason.ToString());
			this.ContactInfo.CopyToProperty(pdfProperties.ContactInfo, (PdfStringOld contactInfo) => contactInfo.ToString());
			this.SignatureHandlerVersion.CopyToProperty(pdfProperties.SignatureHandlerVersion, (PdfIntOld signatureHandlerVersion) => signatureHandlerVersion.Value);
			this.SignatureDictionaryVersion.CopyToProperty(pdfProperties.SignatureDictionaryVersion, (PdfIntOld signatureDictionaryVersion) => signatureDictionaryVersion.Value);
			signature.Properties.SourceStream = base.ContentManager.GetSourceStream();
		}

		X509Certificate2Collection ToCertificateCollection(PdfArrayOld array)
		{
			X509Certificate2Collection x509Certificate2Collection = new X509Certificate2Collection();
			for (int i = 0; i < array.Count; i++)
			{
				byte[] value = ((PdfStringOld)array[i]).Value;
				X509Certificate2 certificate = new X509Certificate2(value);
				x509Certificate2Collection.Add(certificate);
			}
			return x509Certificate2Collection;
		}

		readonly InstantLoadProperty<PdfNameOld> filter;

		readonly InstantLoadProperty<PdfNameOld> subFilter;

		readonly InstantLoadProperty<PdfStringOld> contents;

		readonly InstantLoadProperty<PdfArrayOld> cert;

		readonly InstantLoadProperty<PdfArrayOld> byteRange;

		readonly InstantLoadProperty<PdfArrayOld> signatureReference;

		readonly InstantLoadProperty<PdfArrayOld> changes;

		readonly InstantLoadProperty<PdfStringOld> name;

		readonly InstantLoadProperty<PdfStringOld> location;

		readonly InstantLoadProperty<PdfStringOld> reason;

		readonly InstantLoadProperty<PdfStringOld> contactInfo;

		readonly InstantLoadProperty<PdfStringOld> timeOfSigning;

		readonly InstantLoadProperty<PdfIntOld> signatureHandlerVersion;

		readonly InstantLoadProperty<PdfIntOld> signatureDictionaryVersion;
	}
}
