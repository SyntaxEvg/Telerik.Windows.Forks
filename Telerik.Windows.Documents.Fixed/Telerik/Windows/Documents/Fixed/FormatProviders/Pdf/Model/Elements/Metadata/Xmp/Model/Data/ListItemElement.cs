using System;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Attributes;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Structure;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata.Xmp.Model.Data
{
	class ListItemElement : XmpDataElementBase
	{
		public ListItemElement(string data)
		{
			Guard.ThrowExceptionIfNull<string>(data, "data");
			base.InnerText = data;
			this.language = base.RegisterAttribute<string>("lang", XmpNamespaces.Xml, null, false);
		}

		public ListItemElement(string data, string language)
			: this(data)
		{
			Guard.ThrowExceptionIfNull<string>(language, "language");
			this.Language = language;
		}

		public override string Name
		{
			get
			{
				return "li";
			}
		}

		public string Language
		{
			get
			{
				return this.language.GetValue();
			}
			set
			{
				this.language.SetValue(value);
			}
		}

		readonly XmlAttribute<string> language;
	}
}
