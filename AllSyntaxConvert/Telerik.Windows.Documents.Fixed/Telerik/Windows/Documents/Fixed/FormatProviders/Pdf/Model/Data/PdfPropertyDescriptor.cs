using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data
{
	class PdfPropertyDescriptor
	{
		public PdfPropertyDescriptor()
		{
			this.Restrictions = PdfPropertyRestrictions.None;
			this.IsRequired = false;
		}

		public PdfPropertyDescriptor(string name)
			: this()
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.Name = name;
		}

		public PdfPropertyDescriptor(string name, bool isRequired)
			: this(name)
		{
			this.IsRequired = isRequired;
		}

		public PdfPropertyDescriptor(string name, bool isRequired, bool alwaysExport)
			: this(name, isRequired)
		{
			this.AlwaysExport = alwaysExport;
		}

		public PdfPropertyDescriptor(string name, bool isRequired, PdfPropertyRestrictions restrictions)
			: this(name, isRequired)
		{
			this.Restrictions = restrictions;
		}

		public string Name { get; set; }

		public bool IsRequired { get; set; }

		public PdfPropertyRestrictions Restrictions { get; set; }

		public bool AlwaysExport { get; set; }
	}
}
