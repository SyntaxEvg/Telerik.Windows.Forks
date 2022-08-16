using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure
{
	class ObjectStream : PdfStreamObjectBase
	{
		public ObjectStream()
		{
			this.n = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("N", true, PdfPropertyRestrictions.MustBeDirectObject));
			this.first = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("First", true, PdfPropertyRestrictions.MustBeDirectObject));
			this.extends = base.RegisterReferenceProperty<ObjectStream>(new PdfPropertyDescriptor("Extends", false, PdfPropertyRestrictions.MustBeIndirectReference));
			this.objectNumberToOffset = new List<KeyValuePair<int, int>>();
		}

		public PdfInt N
		{
			get
			{
				return this.n.GetValue();
			}
			set
			{
				this.n.SetValue(value);
			}
		}

		public PdfInt First
		{
			get
			{
				return this.first.GetValue();
			}
			set
			{
				this.first.SetValue(value);
			}
		}

		public ObjectStream Extends
		{
			get
			{
				return this.extends.GetValue();
			}
			set
			{
				this.extends.SetValue(value);
			}
		}

		protected override void InterpretData(PostScriptReader reader, IPdfImportContext context, PdfStreamBase stream)
		{
			if (this.reader == null)
			{
				byte[] buffer = stream.ReadDecodedPdfData();
				this.reader = new PostScriptReader(new MemoryStream(buffer), new KeywordCollection());
				int value = this.N.Value;
				for (int i = 0; i < value; i++)
				{
					int value2 = this.reader.Read<PdfInt>(context, PdfElementType.PdfInt).Value;
					int value3 = this.reader.Read<PdfInt>(context, PdfElementType.PdfInt).Value;
					this.objectNumberToOffset.Add(new KeyValuePair<int, int>(value2, value3));
				}
			}
		}

		internal PdfPrimitive ReadObjectContent(CrossReferenceEntry entry, IPdfImportContext context)
		{
			int field = entry.Field2;
			int offset = this.GetOffset(field);
			this.reader.Reader.Seek((long)offset, SeekOrigin.Begin);
			long endPosition = ((field >= this.objectNumberToOffset.Count - 1) ? this.reader.Reader.Length : ((long)this.GetOffset(field + 1)));
			PdfPrimitive result;
			using (context.BeginImportOfStreamInnerContent())
			{
				PdfPrimitive[] array = this.reader.Read(context, endPosition);
				PdfPrimitive pdfPrimitive = array[0];
				result = pdfPrimitive;
			}
			return result;
		}

		int GetOffset(int objectIndex)
		{
			int value = this.objectNumberToOffset[objectIndex].Value;
			return value + this.First.Value;
		}

		readonly DirectProperty<PdfInt> n;

		readonly DirectProperty<PdfInt> first;

		readonly ReferenceProperty<ObjectStream> extends;

		PostScriptReader reader;

		readonly List<KeyValuePair<int, int>> objectNumberToOffset;
	}
}
