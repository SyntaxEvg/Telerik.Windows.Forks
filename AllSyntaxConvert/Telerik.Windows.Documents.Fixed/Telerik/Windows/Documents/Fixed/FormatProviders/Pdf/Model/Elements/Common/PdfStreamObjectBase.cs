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
	abstract class PdfStreamObjectBase : global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common.PdfObject
	{
		public PdfStreamObjectBase()
		{
			this.length = base.RegisterDirectProperty<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfInt>(new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data.PdfPropertyDescriptor("Length", true));
			this.filters = base.RegisterDirectProperty<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray>(new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data.PdfPropertyDescriptor("Filter", true));
			this.decodeParms = base.RegisterReferenceProperty<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common.PrimitiveWrapper>(new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data.PdfPropertyDescriptor("DecodeParms"));
		}

		public override global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfElementType Type
		{
			get
			{
				return global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfElementType.PdfStreamObject;
			}
		}

		public global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray Filters
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

		public global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common.PrimitiveWrapper DecodeParms
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

		public global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfInt Length
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

		public override void Write(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.PdfWriter writer, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.IPdfExportContext context)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.PdfWriter>(writer, "writer");
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.IPdfExportContext>(context, "context");
			if (this.ExportAs == global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfElementType.PdfStreamObject)
			{
				byte[] array = this.GetData(context);
				this.Filters = this.GetExportFilters(context);
				if (this.Filters != null)
				{
					array = global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters.FiltersManager.Encode(this.CreateEncodeObject(context), array, this.Filters);
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

		protected void WriteDictionaryAndEncodedData(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.PdfWriter writer, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.IPdfExportContext context, byte[] encodedData)
		{
			this.Length = new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfInt(encodedData.Length);
			base.Write(writer, context);
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common.PdfStreamObjectBase.WriteEncodedData(writer, context, encodedData);
		}

		protected override void LoadOverride(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PostScriptReader reader, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.IPdfImportContext context, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfPrimitive primitive)
		{
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfElementType type = primitive.Type;
			if (type == global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfElementType.Stream)
			{
				global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfStreamBase pdfStreamBase = (global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfStreamBase)primitive;
				base.Load(reader, context, pdfStreamBase.Dictionary);
				this.InterpretData(reader, context, pdfStreamBase);
				return;
			}
			base.LoadOverride(reader, context, primitive);
		}

		protected virtual byte[] GetData(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.IPdfExportContext context)
		{
			throw new global::System.NotSupportedException();
		}

		protected virtual void InterpretData(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PostScriptReader reader, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.IPdfImportContext context, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfStreamBase stream)
		{
			throw new global::System.NotSupportedException();
		}

		protected virtual global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray GetExportFilters(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.IPdfExportContext context)
		{
			return new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray(new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfPrimitive[]
			{
				new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfName("FlateDecode")
			});
		}

		protected virtual global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters.PdfObject CreateEncodeObject(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.IPdfExportContext context)
		{
			return new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters.PdfObject(context);
		}

		internal static void WriteEncodedData(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.PdfWriter writer, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.IPdfExportContext context, byte[] encodedData)
		{
			writer.WriteLine();
			writer.WriteLine("stream");
			writer.Write(encodedData);
			writer.WriteLine();
			writer.Write("endstream");
		}

		private readonly global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data.DirectProperty<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfInt> length;

		private readonly global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data.DirectProperty<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types.PdfArray> filters;

		private readonly global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data.ReferenceProperty<global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common.PrimitiveWrapper> decodeParms;
	}
}
