using System;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Namespaces;

namespace Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Attributes
{
	abstract class XmlAttribute
	{
		public XmlAttribute(string name, XmlNamespace ns, bool isRequired = false)
		{
			this.name = name;
			this.ns = ns;
			this.isRequired = isRequired;
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public XmlNamespace Namespace
		{
			get
			{
				return this.ns;
			}
		}

		public bool IsRequired
		{
			get
			{
				return this.isRequired;
			}
		}

		public abstract bool HasValue { get; }

		public abstract string GetValue();

		public abstract void SetValue(string value);

		public abstract bool ShouldExport();

		public abstract void ResetValue();

		readonly string name;

		readonly XmlNamespace ns;

		readonly bool isRequired;
	}
}
