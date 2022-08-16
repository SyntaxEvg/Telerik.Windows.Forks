using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	sealed class IndirectObject : PdfPrimitive
	{
		public IndirectObject(IndirectReference reference, PdfPrimitive content)
		{
			Guard.ThrowExceptionIfNull<IndirectReference>(reference, "reference");
			Guard.ThrowExceptionIfNull<PdfPrimitive>(content, "content");
			this.reference = reference;
			this.content = content;
		}

		public override PdfElementType Type
		{
			get
			{
				return PdfElementType.IndirectObject;
			}
		}

		public PdfPrimitive Content
		{
			get
			{
				Guard.ThrowExceptionIfTrue(this.isContentReleased, "isContentReleased");
				return this.content;
			}
		}

		public IndirectReference Reference
		{
			get
			{
				return this.reference;
			}
		}

		public T GetContent<T>() where T : PdfPrimitive
		{
			return (T)((object)this.Content);
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			context.BeginExportIndirectObject(this, writer.Position);
			writer.WriteLine("{0} {1} {2}", new object[]
			{
				this.Reference.ObjectNumber,
				this.Reference.GenerationNumber,
				"obj"
			});
			this.Content.Write(writer, context);
			writer.WriteLine();
			writer.WriteLine("endobj");
			context.EndExportIndirectObject();
		}

		internal void ReleaseContent()
		{
			this.isContentReleased = true;
			this.content = null;
		}

		readonly IndirectReference reference;

		PdfPrimitive content;

		bool isContentReleased;
	}
}
