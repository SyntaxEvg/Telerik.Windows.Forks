using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	abstract class PdfPrimitive : IInstanceIdOwner, IPdfSharedObject
	{
		protected PdfPrimitive()
		{
			this.id = InstanceIdGenerator.GetNextId();
		}

		public abstract PdfElementType Type { get; }

		public virtual PdfElementType ExportAs
		{
			get
			{
				return this.Type;
			}
		}

		int IInstanceIdOwner.InstanceId
		{
			get
			{
				return this.id;
			}
		}

		public bool IsOldSchema
		{
			get
			{
				return false;
			}
		}

		public abstract void Write(PdfWriter writer, IPdfExportContext context);

		protected static void WriteIndirectReference(PdfWriter writer, IPdfExportContext context, PdfPrimitive primitive)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			IndirectObject indirectObject = context.CreateIndirectObject(primitive);
			writer.WriteIndirectReference(indirectObject.Reference);
		}

		readonly int id;
	}
}
