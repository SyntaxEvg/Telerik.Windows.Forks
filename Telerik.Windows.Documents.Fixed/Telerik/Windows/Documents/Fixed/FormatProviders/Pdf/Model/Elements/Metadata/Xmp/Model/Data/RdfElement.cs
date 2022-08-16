using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Structure;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Data
{
	class RdfElement : XmpDataElementBase
	{
		public RdfElement(IEnumerable<DescriptionElement> descriptions)
		{
			this.descriptions = descriptions;
		}

		public override string Name
		{
			get
			{
				return "RDF";
			}
		}

		protected override IEnumerable<XmpElementBase> EnumerateInnerElements()
		{
			return this.descriptions;
		}

		readonly IEnumerable<DescriptionElement> descriptions;
	}
}
