using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data
{
	class PdfObjectDescriptor
	{
		public PdfObjectDescriptor()
		{
			this.Type = null;
			this.Converter = PdfObjectDescriptor.defaultConverter;
			this.SubType = null;
			this.SubTypeProperty = null;
		}

		public PdfObjectDescriptor(string type)
			: this()
		{
			this.Type = type;
		}

		public PdfObjectDescriptor(IConverter converter)
			: this()
		{
			this.Converter = converter;
		}

		public PdfObjectDescriptor(string type, IConverter converter)
			: this(type)
		{
			this.Converter = converter;
		}

		public PdfObjectDescriptor(PdfPrimitive subType, string subTypeProperty = "Subtype")
			: this(null, subType, subTypeProperty)
		{
		}

		public PdfObjectDescriptor(string type, PdfPrimitive subType, string subTypeProperty = "Subtype")
			: this(type)
		{
			this.SubType = subType;
			this.SubTypeProperty = subTypeProperty;
		}

		public string Type { get; set; }

		public PdfPrimitive SubType { get; set; }

		public string SubTypeProperty { get; set; }

		public IConverter Converter { get; set; }

		static readonly IConverter defaultConverter = new Converter();
	}
}
