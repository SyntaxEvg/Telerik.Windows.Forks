using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data
{
	class IndirectReferenceOld
	{
		public int ObjectNumber { get; set; }

		public int GenerationNumber { get; set; }

		public static bool operator ==(IndirectReferenceOld left, IndirectReferenceOld right)
		{
			if (object.Equals(left, null))
			{
				return object.Equals(right, null);
			}
			return !object.Equals(right, null) && left.Equals(right);
		}

		public static bool operator !=(IndirectReferenceOld left, IndirectReferenceOld right)
		{
			return !(left == right);
		}

		public override string ToString()
		{
			return string.Format("{0} {1} R", this.ObjectNumber, this.GenerationNumber);
		}

		public override bool Equals(object obj)
		{
			IndirectReferenceOld indirectReferenceOld = obj as IndirectReferenceOld;
			return !(indirectReferenceOld == null) && this.ObjectNumber == indirectReferenceOld.ObjectNumber && this.GenerationNumber == indirectReferenceOld.GenerationNumber;
		}

		public override int GetHashCode()
		{
			return this.ObjectNumber ^ this.GenerationNumber;
		}
	}
}
