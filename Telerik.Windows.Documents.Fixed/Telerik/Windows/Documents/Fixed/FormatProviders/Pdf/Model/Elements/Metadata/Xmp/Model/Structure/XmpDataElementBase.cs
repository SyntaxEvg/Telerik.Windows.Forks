using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Attributes;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Elements;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Namespaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Common;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Structure
{
	abstract class XmpDataElementBase : XmpElementBase
	{
		public override XmlNamespace Namespace
		{
			get
			{
				return XmpNamespaces.Rdf;
			}
		}

		public override void Write(IXmpWriter writer)
		{
			Guard.ThrowExceptionIfNull<IXmpWriter>(writer, "writer");
			base.WriteElementStart(writer);
			this.WriteAttributes(writer);
			if (!string.IsNullOrEmpty(base.InnerText))
			{
				writer.WriteValue(base.InnerText);
			}
			foreach (XmpElementBase xmpElementBase in this.EnumerateInnerElements())
			{
				xmpElementBase.Write(writer);
			}
			writer.WriteElementEnd();
		}

		protected virtual IEnumerable<XmpElementBase> EnumerateInnerElements()
		{
			return Enumerable.Empty<XmpElementBase>();
		}

		void WriteAttributes(IXmpWriter writer)
		{
			Guard.ThrowExceptionIfNull<IXmpWriter>(writer, "writer");
			foreach (XmlAttribute xmlAttribute in base.Attributes)
			{
				if (xmlAttribute.ShouldExport())
				{
					XmlElementBase<XmlAttribute>.WriteAttribute(writer, xmlAttribute);
				}
			}
		}
	}
}
