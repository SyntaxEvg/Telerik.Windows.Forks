using System;

namespace Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Namespaces
{
	class XmlNamespace
	{
		public XmlNamespace(string prefix, string localName, string value)
		{
			this.prefix = prefix;
			this.localName = localName;
			this.value = value;
		}

		public XmlNamespace(string value)
			: this(string.Empty, string.Empty, value)
		{
		}

		public string Prefix
		{
			get
			{
				return this.prefix;
			}
		}

		public string LocalName
		{
			get
			{
				return this.localName;
			}
		}

		public string Value
		{
			get
			{
				return this.value;
			}
		}

		readonly string prefix;

		readonly string localName;

		readonly string value;
	}
}
