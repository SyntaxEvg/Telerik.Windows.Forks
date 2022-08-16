using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms
{
	class FormFieldsTree : PdfPrimitive, IEnumerable<FormFieldNode>, IEnumerable
	{
		public FormFieldsTree()
		{
			this.fields = new List<FormFieldNode>();
		}

		public override PdfElementType Type
		{
			get
			{
				return PdfElementType.Array;
			}
		}

		public void Add(FormFieldNode node)
		{
			this.fields.Add(node);
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
			foreach (FormFieldNode primitive in this.fields)
			{
				IndirectObject indirectObject = context.CreateIndirectObject(primitive);
				pdfArray.Add(indirectObject.Reference);
			}
			pdfArray.Write(writer, context);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.fields.GetEnumerator();
		}

		public IEnumerator<FormFieldNode> GetEnumerator()
		{
			return this.fields.GetEnumerator();
		}

		readonly List<FormFieldNode> fields;
	}
}
