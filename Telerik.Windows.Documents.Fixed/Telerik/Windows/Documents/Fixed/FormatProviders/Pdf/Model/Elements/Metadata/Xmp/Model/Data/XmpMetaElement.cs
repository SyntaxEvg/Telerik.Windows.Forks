using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Namespaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Structure;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Data
{
	class XmpMetaElement : XmpDataElementBase
	{
		public XmpMetaElement(IEnumerable<DescriptionElement> descriptions)
		{
			this.rdfElement = new RdfElement(descriptions);
		}

		public override string Name
		{
			get
			{
				return "xmpmeta";
			}
		}

		public override XmlNamespace Namespace
		{
			get
			{
				return XmpNamespaces.AdobeMeta;
			}
		}

		protected override IEnumerable<XmpElementBase> EnumerateInnerElements()
		{
			yield return this.rdfElement;
			yield break;
		}

		readonly RdfElement rdfElement;
	}
}
