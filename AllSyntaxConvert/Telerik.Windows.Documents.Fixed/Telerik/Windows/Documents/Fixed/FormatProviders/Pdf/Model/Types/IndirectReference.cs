using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	class IndirectReference : PdfPrimitive
	{
		public IndirectReference(int objectNumber, int generationNumber)
		{
			this.objectNumber = objectNumber;
			this.generationNumber = generationNumber;
		}

		public override PdfElementType Type
		{
			get
			{
				return PdfElementType.IndirectReference;
			}
		}

		public int GenerationNumber
		{
			get
			{
				return this.generationNumber;
			}
		}

		public int ObjectNumber
		{
			get
			{
				return this.objectNumber;
			}
		}

		public static bool operator ==(IndirectReference left, IndirectReference right)
		{
			if (object.Equals(left, null))
			{
				return object.Equals(right, null);
			}
			return !object.Equals(right, null) && left.Equals(right);
		}

		public static bool operator !=(IndirectReference left, IndirectReference right)
		{
			return !(left == right);
		}

		public override string ToString()
		{
			return string.Format("{0} {1} R", this.ObjectNumber, this.GenerationNumber);
		}

		public override bool Equals(object obj)
		{
			IndirectReference indirectReference = obj as IndirectReference;
			return !(indirectReference == null) && this.ObjectNumber == indirectReference.ObjectNumber && this.GenerationNumber == indirectReference.GenerationNumber;
		}

		public override int GetHashCode()
		{
			return this.ObjectNumber ^ this.GenerationNumber;
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			writer.WriteIndirectReference(this);
		}

		readonly int objectNumber;

		readonly int generationNumber;
	}
}
