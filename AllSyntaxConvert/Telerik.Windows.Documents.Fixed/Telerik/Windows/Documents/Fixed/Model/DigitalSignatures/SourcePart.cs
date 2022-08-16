using System;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	class SourcePart
	{
		public SourcePart(int offset, int length)
		{
			this.Offset = offset;
			this.Length = length;
		}

		public int Offset { get; set; }

		public int Length { get; set; }
	}
}
