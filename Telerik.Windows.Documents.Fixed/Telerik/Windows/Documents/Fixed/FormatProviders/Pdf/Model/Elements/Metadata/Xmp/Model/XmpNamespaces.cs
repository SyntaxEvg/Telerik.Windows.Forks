using System;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Namespaces;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model
{
	static class XmpNamespaces
	{
		const string DefaultNamespaceName = "xmlns";

		public static readonly XmlNamespace AdobeMeta = new XmlNamespace("xmlns", "x", "adobe:ns:meta/");

		public static readonly XmlNamespace Rdf = new XmlNamespace("xmlns", "rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");

		public static readonly XmlNamespace Xmp = new XmlNamespace("xmlns", "xmp", "http://ns.adobe.com/xap/1.0/");

		public static readonly XmlNamespace PdfAId = new XmlNamespace("xmlns", "pdfaid", "http://www.aiim.org/pdfa/ns/id/");

		public static readonly XmlNamespace Dc = new XmlNamespace("xmlns", "dc", "http://purl.org/dc/elements/1.1/");

		public static readonly XmlNamespace Xml = new XmlNamespace("", "xml", "");
	}
}
