using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils
{
	[AttributeUsage(AttributeTargets.Property)]
	class PdfPropertyAttribute : Attribute
	{
		public string Name { get; set; }

		public bool IsMultipleProperties { get; set; }

		public PdfPropertyState State { get; set; }

		public bool IsRequired { get; set; }

		public bool IsInheritable { get; set; }
	}
}
