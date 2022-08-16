using System;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Attributes;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Elements;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Namespaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Export;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Common
{
	abstract class XmpElementBase : XmlElementBase<XmlAttribute>
	{
		public abstract void Write(IXmpWriter writer);

		protected XmpAttribute<T> RegisterAttribute<T>(string name, XmlNamespace ns, T defaultValue, bool isRequired = false)
		{
			XmpAttribute<T> xmpAttribute = new XmpAttribute<T>(name, ns, defaultValue, isRequired);
			base.RegisterAttribute(xmpAttribute);
			return xmpAttribute;
		}

		protected XmpAttribute<T> XmpAttribute<T>(string name, XmlNamespace ns, bool isRequired = false)
		{
			XmpAttribute<T> xmpAttribute = new XmpAttribute<T>(name, ns, isRequired);
			base.RegisterAttribute(xmpAttribute);
			return xmpAttribute;
		}

		protected XmpAttribute<T> RegisterAttribute<T>(string name, T defaultValue, bool isRequired = false)
		{
			return this.RegisterAttribute<T>(name, null, defaultValue, isRequired);
		}

		protected XmpAttribute<T> RegisterAttribute<T>(string name, bool isRequired = false)
		{
			return this.RegisterAttribute<T>(name, null, default(T), isRequired);
		}
	}
}
