using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common
{
	abstract class PdfStreamObjectBase : PdfObject
	{
		public PdfStreamObjectBase()
		{
			this.length = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("Length", true));
			this.filters = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Filter", true));
			this.decodeParms = base.RegisterReferenceProperty<PrimitiveWrapper>(new PdfPropertyDescriptor("DecodeParms"));
		}

		public override PdfElementType Type
		{
			get
			{
				return PdfElementType.PdfStreamObject;
			}
		}

		public PdfArray Filters
		{
			get
			{
				return this.filters.GetValue();
			}
			set
			{
				this.filters.SetValue(value);
			}
		}

		public PrimitiveWrapper DecodeParms
		{
			get
			{
				return this.decodeParms.GetValue();
			}
			set
			{
				this.decodeParms.SetValue(value);
			}
		}

		public PdfInt Length
		{
			get
			{
				return this.length.GetValue();
			}
			set
			{
				this.length.SetValue(value);
			}
		}

		public virtual bool ShouldEncryptData
		{
			get
			{
				return true;
			}
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			if (this.ExportAs == PdfElementType.PdfStreamObject)
			{
				byte[] array = this.GetData(context);
				this.Filters = this.GetExportFilters(context);
				if (this.Filters != null)
				{
					array = FiltersManager.Encode(this.CreateEncodeObject(context), array, this.Filters);
				}
				if (this.ShouldEncryptData)
				{
					array = context.EncryptStream(array);
				}
				this.WriteDictionaryAndEncodedData(writer, context, array);
				return;
			}
			base.Write(writer, context);
		}

		protected void WriteDictionaryAndEncodedData(PdfWriter writer, IPdfExportContext context, byte[] encodedData)
		{
			this.Length = new PdfInt(encodedData.Length);
			base.Write(writer, context);
			PdfStreamObjectBase.WriteEncodedData(writer, context, encodedData);
		}

		protected override void LoadOverride(PostScriptReader reader, IPdfImportContext context, PdfPrimitive primitive)
		{
			PdfElementType type = primitive.Type;
			if (type == PdfElementType.Stream)
			{
				PdfStreamBase pdfStreamBase = (PdfStreamBase)primitive;
				base.Load(reader, context, pdfStreamBase.Dictionary);
				this.InterpretData(reader, context, pdfStreamBase);
				return;
			}
			base.LoadOverride(reader, context, primitive);
		}

		protected virtual byte[] GetData(IPdfExportContext context)
		{
			throw new NotSupportedException();
		}

		protected virtual void InterpretData(PostScriptReader reader, IPdfImportContext context, PdfStreamBase stream)
		{
			throw new NotSupportedException();
		}

		protected virtual PdfArray GetExportFilters(IPdfExportContext context)
		{
			return new PdfArray(new PdfPrimitive[]
			{
				new PdfName("FlateDecode")
			});
		}

		protected virtual PdfObject CreateEncodeObject(IPdfExportContext context)
		{
			return new PdfObject(context);
		}

		internal static void WriteEncodedData(PdfWriter writer, IPdfExportContext context, byte[] encodedData)
		{
			writer.WriteLine();
			writer.WriteLine("stream");
			writer.Write(encodedData);
			writer.WriteLine();
			writer.Write("endstream");
		}

		readonly DirectProperty<PdfInt> length;

		readonly DirectProperty<PdfArray> filters;

		readonly ReferenceProperty<PrimitiveWrapper> decodeParms;
	}
}
