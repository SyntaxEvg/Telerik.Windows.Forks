using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure
{
	class Trailer : PdfObject
	{
		public Trailer()
		{
			this.prev = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("Prev", true, PdfPropertyRestrictions.MustBeDirectObject));
			this.size = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("Size", true, PdfPropertyRestrictions.MustBeDirectObject));
			this.root = base.RegisterReferenceProperty<DocumentCatalog>(new PdfPropertyDescriptor("Root", true, PdfPropertyRestrictions.MustBeIndirectReference));
			this.id = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("ID"));
			this.encryption = base.RegisterReferenceProperty<Encrypt>(new PdfPropertyDescriptor("Encrypt", false, PdfPropertyRestrictions.MustBeIndirectReference));
			this.xRefStm = base.RegisterReferenceProperty<PdfInt>(new PdfPropertyDescriptor("XRefStm", false, PdfPropertyRestrictions.MustBeDirectObject));
		}

		public PdfInt Prev
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

		public PdfInt Size
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

		public DocumentCatalog Root
		{
			get
			{
				return this.root.GetValue();
			}
			set
			{
				this.root.SetValue(value);
			}
		}

		public Encrypt Encryption
		{
			get
			{
				return this.encryption.GetValue();
			}
			set
			{
				this.encryption.SetValue(value);
			}
		}

		public PdfInt XRefStm
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

		public PdfArray Id
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

		public void CopyPropertiesFrom(IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			this.Id = new PdfArray(new PdfPrimitive[0]);
			this.Id.Add(new PdfHexString(context.DocumentId));
			this.Id.Add(new PdfHexString(context.DocumentId));
			this.Size = new PdfInt(context.CrossReferenceCollection.MaxObjectNumber + 1);
			this.Encryption = context.Encryption;
		}

		public void CopyPropertiesTo(PostScriptReader reader, IPdfImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			if (context.Encryption == null)
			{
				context.Encryption = this.Encryption;
			}
			if (context.DocumentId == null)
			{
				PdfString pdfString;
				if (this.Id != null && this.Id.TryGetElement<PdfString>(reader, context, 0, out pdfString))
				{
					context.DocumentId = pdfString.Value;
				}
				else
				{
					context.DocumentId = new byte[0];
				}
			}
			if (context.Root == null)
			{
				context.Root = this.root;
			}
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			writer.WriteLine("trailer");
			base.Write(writer, context);
		}

		readonly DirectProperty<PdfInt> size;

		readonly DirectProperty<PdfInt> prev;

		readonly ReferenceProperty<DocumentCatalog> root;

		readonly DirectProperty<PdfArray> id;

		readonly ReferenceProperty<Encrypt> encryption;

		readonly ReferenceProperty<PdfInt> xRefStm;
	}
}
