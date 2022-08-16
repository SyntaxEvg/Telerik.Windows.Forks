using System;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Attributes;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Common;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Structure
{
	class XPacketElement : XmpElementBase
	{
		public override string Name
		{
			get
			{
				return "xpacket";
			}
		}

		public override void Write(IXmpWriter writer)
		{
			writer.WriteRaw(string.Format("<?xpacket ", new object[0]));
			foreach (XmlAttribute xmlAttribute in base.Attributes)
			{
				writer.WriteRaw(string.Format("{0}='{1}' ", xmlAttribute.Name, xmlAttribute.GetValue()));
			}
			writer.WriteRaw(string.Format("?>", new object[0]));
			writer.WriteRawLine();
		}
	}
}
