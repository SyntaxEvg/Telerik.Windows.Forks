using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils
{
	class PdfPropertyDescriptor
	{
		public PdfPropertyDescriptor()
		{
		}

		public PdfPropertyDescriptor(string name)
		{
			this.Name = name;
		}

		public PdfPropertyDescriptor(string name, bool isRequired)
			: this(name)
		{
			this.IsRequired = isRequired;
		}

		public string Name { get; set; }

		public bool IsMultipleProperties { get; set; }

		public PdfPropertyState State { get; set; }

		public bool IsRequired { get; set; }

		public bool IsInheritable { get; set; }
	}
}
