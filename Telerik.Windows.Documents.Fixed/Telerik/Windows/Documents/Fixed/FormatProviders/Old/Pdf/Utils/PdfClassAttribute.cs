using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils
{
	[AttributeUsage(AttributeTargets.Class)]
	class PdfClassAttribute : Attribute
	{
		public string TypeName { get; set; }

		public string SubtypeProperty { get; set; }

		public string SubtypeValue { get; set; }
	}
}
