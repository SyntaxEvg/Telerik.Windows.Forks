using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Namespaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Structure;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Data
{
	class DescriptionElement : XmpDataElementBase
	{
		public DescriptionElement(params XmpDataElement[] elements)
		{
			this.elements = elements;
			base.RegisterAttribute<string>("about", "", true);
		}

		public override string Name
		{
			get
			{
				return "Description";
			}
		}

		public override IEnumerable<XmlNamespace> Namespaces
		{
			get
			{
				return (from e in this.elements
					select e.Namespace).Distinct<XmlNamespace>();
			}
		}

		protected override IEnumerable<XmpElementBase> EnumerateInnerElements()
		{
			return this.elements;
		}

		readonly IEnumerable<XmpDataElement> elements;
	}
}
