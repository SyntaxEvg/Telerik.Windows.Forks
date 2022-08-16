using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Encryption;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core
{
	class TrailerOld : PdfObjectOld
	{
		public TrailerOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.size = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "Size",
				IsRequired = true,
				State = PdfPropertyState.MustBeDirectObject
			});
			this.prev = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "Prev"
			});
			this.root = base.CreateLoadOnDemandProperty<DocumentCatalogOld>(new PdfPropertyDescriptor
			{
				Name = "Root",
				IsRequired = true,
				State = PdfPropertyState.MustBeIndirectReference
			});
			this.xRefStm = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "XRefStm"
			});
			this.id = base.CreateLoadOnDemandProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "ID"
			});
			this.encrypt = base.CreateLoadOnDemandProperty<EncryptOld>(new PdfPropertyDescriptor
			{
				Name = "Encrypt"
			}, Converters.EncryptConverter);
		}

		public PdfIntOld Size
		{
			get
			{
				return this.size.GetValue();
			}
			set
			{
				this.size.SetValue(value);
			}
		}

		public PdfIntOld Prev
		{
			get
			{
				return this.prev.GetValue();
			}
			set
			{
				this.prev.SetValue(value);
			}
		}

		public DocumentCatalogOld Root
		{
			get
			{
				return this.root.GetValue();
			}
		}

		public PdfIntOld XRefStm
		{
			get
			{
				return this.xRefStm.GetValue();
			}
			set
			{
				this.xRefStm.SetValue(value);
			}
		}

		public PdfArrayOld ID
		{
			get
			{
				return this.id.GetValue();
			}
			set
			{
				this.id.SetValue(value);
			}
		}

		public EncryptOld Encrypt
		{
			get
			{
				return this.encrypt.GetValue();
			}
			set
			{
				this.encrypt.SetValue(value);
			}
		}

		internal void CopyPropertiesFrom(CrossReferenceStreamOld crossReferenceStream)
		{
			Guard.ThrowExceptionIfNull<CrossReferenceStreamOld>(crossReferenceStream, "crossReferenceStream");
			this.root = crossReferenceStream.Root;
			this.Encrypt = crossReferenceStream.Encrypt;
			this.ID = crossReferenceStream.ID;
			this.Prev = crossReferenceStream.Prev;
			this.Size = crossReferenceStream.Size;
		}

		readonly InstantLoadProperty<PdfIntOld> size;

		readonly InstantLoadProperty<PdfIntOld> prev;

		readonly InstantLoadProperty<PdfIntOld> xRefStm;

		readonly LoadOnDemandProperty<PdfArrayOld> id;

		readonly LoadOnDemandProperty<EncryptOld> encrypt;

		LoadOnDemandProperty<DocumentCatalogOld> root;
	}
}
