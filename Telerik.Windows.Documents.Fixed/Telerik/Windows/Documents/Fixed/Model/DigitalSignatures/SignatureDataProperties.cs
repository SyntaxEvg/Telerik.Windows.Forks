using System;
using System.Security.Cryptography.X509Certificates;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	public class SignatureDataProperties
	{
		public SignatureDataProperties()
		{
			this.PdfProperties = new SignatureDataPdfProperties();
		}

		public string FieldName { get; internal set; }

		public string Filter
		{
			get
			{
				return this.PdfProperties.Filter.Value;
			}
			internal set
			{
				this.PdfProperties.Filter.Value = value;
			}
		}

		public string SubFilter
		{
			get
			{
				return this.PdfProperties.SubFilter.Value;
			}
			internal set
			{
				this.PdfProperties.SubFilter.Value = value;
			}
		}

		public byte[] Contents
		{
			get
			{
				return this.PdfProperties.Contents.Value;
			}
			internal set
			{
				this.PdfProperties.Contents.Value = value;
			}
		}

		public X509Certificate2Collection Certificates
		{
			get
			{
				return this.PdfProperties.Cert.Value;
			}
			internal set
			{
				this.PdfProperties.Cert.Value = value;
			}
		}

		public int[] ByteRange
		{
			get
			{
				return this.PdfProperties.ByteRange.Value;
			}
			internal set
			{
				this.PdfProperties.ByteRange.Value = value;
			}
		}

		public int[] Changes
		{
			get
			{
				return this.PdfProperties.Changes.Value;
			}
			internal set
			{
				this.PdfProperties.Changes.Value = value;
			}
		}

		public string Name
		{
			get
			{
				return this.PdfProperties.Name.Value;
			}
			set
			{
				this.PdfProperties.Name.Value = value;
			}
		}

		public string Location
		{
			get
			{
				return this.PdfProperties.Location.Value;
			}
			set
			{
				this.PdfProperties.Location.Value = value;
			}
		}

		public string Reason
		{
			get
			{
				return this.PdfProperties.Reason.Value;
			}
			set
			{
				this.PdfProperties.Reason.Value = value;
			}
		}

		public string ContactInfo
		{
			get
			{
				return this.PdfProperties.ContactInfo.Value;
			}
			set
			{
				this.PdfProperties.ContactInfo.Value = value;
			}
		}

		public int SignatureHandlerVersion
		{
			get
			{
				return this.PdfProperties.SignatureHandlerVersion.Value;
			}
			internal set
			{
				this.PdfProperties.SignatureHandlerVersion.Value = value;
			}
		}

		public int SignatureDictionaryVersion
		{
			get
			{
				return this.PdfProperties.SignatureDictionaryVersion.Value;
			}
			internal set
			{
				this.PdfProperties.SignatureDictionaryVersion.Value = value;
			}
		}

		public DateTime TimeOfSigning
		{
			get
			{
				return this.PdfProperties.TimeOfSigning.Value;
			}
			set
			{
				this.PdfProperties.TimeOfSigning.Value = value;
			}
		}

		internal SignatureDataPdfProperties PdfProperties { get; set; }

		internal ISourceStream SourceStream
		{
			get
			{
				return this.sourceStream;
			}
			set
			{
				Guard.ThrowExceptionIfNull<ISourceStream>(value, "SourceStream");
				if (this.sourceStream != null)
				{
					throw new ArgumentException("SourceStream is already defined.");
				}
				this.sourceStream = value;
			}
		}

		internal bool HasValidationRequiredProperties
		{
			get
			{
				return !string.IsNullOrEmpty(this.SubFilter) && this.Contents != null && this.ByteRange != null;
			}
		}

		ISourceStream sourceStream;
	}
}
