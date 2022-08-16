using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Namespaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Structure;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Data
{
	class XmpDataElement : XmpDataElementBase
	{
		public XmpDataElement(XmlNamespace ns, string name, object value)
			: this(ns, name, value.ToString())
		{
		}

		public XmpDataElement(XmlNamespace ns, string name, string value)
			: this(ns, name)
		{
			base.InnerText = value;
		}

		public XmpDataElement(XmlNamespace ns, string name)
		{
			Guard.ThrowExceptionIfNull<XmlNamespace>(ns, "namespace");
			Guard.ThrowExceptionIfNull<string>(name, "name");
			this.ns = ns;
			this.name = name;
		}

		public XmpDataElement(XmlNamespace ns, string name, XmpDataElementBase child)
			: this(ns, name)
		{
			Guard.ThrowExceptionIfNull<XmpDataElementBase>(child, "child");
			this.child = child;
		}

		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		public override XmlNamespace Namespace
		{
			get
			{
				return this.ns;
			}
		}

		protected override IEnumerable<XmpElementBase> EnumerateInnerElements()
		{
			if (this.child != null)
			{
				yield return this.child;
			}
			yield break;
		}

		readonly string name;

		readonly XmlNamespace ns;

		readonly XmpDataElementBase child;
	}
}
