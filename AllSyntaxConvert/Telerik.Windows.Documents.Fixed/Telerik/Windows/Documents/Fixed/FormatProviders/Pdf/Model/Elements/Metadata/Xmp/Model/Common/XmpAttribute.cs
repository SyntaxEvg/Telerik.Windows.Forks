using System;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Attributes;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Namespaces;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Common
{
	class XmpAttribute<T> : XmlAttribute<T>
	{
		public XmpAttribute(string name, XmlNamespace ns, bool isRequired = false)
			: this(name, ns, default(T), isRequired)
		{
		}

		public XmpAttribute(string name, bool isRequired = false)
			: this(name, null, default(T), isRequired)
		{
		}

		public XmpAttribute(string name, XmlNamespace ns, T defaultValue, bool isRequired = false)
			: base(name, ns, defaultValue, isRequired)
		{
		}
	}
}
