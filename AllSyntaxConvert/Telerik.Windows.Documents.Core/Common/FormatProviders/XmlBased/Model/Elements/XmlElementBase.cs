using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Export;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Attributes;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Namespaces;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Elements
{
	abstract class XmlElementBase<TAttribute> where TAttribute : XmlAttribute
	{
		public XmlElementBase()
		{
			this.attributes = new Dictionary<string, TAttribute>();
		}

		public abstract string Name { get; }

		public string InnerText { get; set; }

		public virtual XmlNamespace Namespace
		{
			get
			{
				return null;
			}
		}

		public virtual IEnumerable<XmlNamespace> Namespaces
		{
			get
			{
				return Enumerable.Empty<XmlNamespace>();
			}
		}

		public IEnumerable<TAttribute> Attributes
		{
			get
			{
				return this.attributes.Values;
			}
		}

		public void Clear()
		{
			foreach (TAttribute tattribute in this.attributes.Values)
			{
				XmlAttribute xmlAttribute = tattribute;
				xmlAttribute.ResetValue();
			}
			this.ClearOverride();
		}

		protected static void WriteAttribute(IXmlWriter writer, TAttribute attribute)
		{
			Guard.ThrowExceptionIfNull<IXmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<TAttribute>(attribute, "attribute");
			if (attribute.Namespace == null)
			{
				writer.WriteAttribute(attribute.Name, attribute.GetValue());
				return;
			}
			writer.WriteAttribute(attribute.Namespace.LocalName, attribute.Name, attribute.GetValue());
		}

		protected virtual void ClearOverride()
		{
		}

		protected void RegisterAttribute(TAttribute attribute)
		{
			Guard.ThrowExceptionIfNull<TAttribute>(attribute, "attribute");
			if (attribute.Namespace != null)
			{
				this.attributes[string.Format("{0}:{1}", attribute.Namespace.LocalName, attribute.Name)] = attribute;
				return;
			}
			this.attributes[attribute.Name] = attribute;
		}

		protected void TrySetAttributeValue(string name, string value)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			TAttribute tattribute;
			if (this.attributes.TryGetValue(name, out tattribute))
			{
				tattribute.SetValue(value);
			}
		}

		protected void WriteElementStart(IXmlWriter writer)
		{
			if (this.Namespace != null)
			{
				writer.WriteElementStart(this.Namespace.LocalName, this.Name, this.Namespace.Value);
			}
			else
			{
				writer.WriteElementStart(this.Name);
			}
			foreach (XmlNamespace xmlNamespace in this.Namespaces)
			{
				if (string.IsNullOrEmpty(xmlNamespace.Prefix))
				{
					writer.WriteAttribute(xmlNamespace.LocalName, xmlNamespace.Value);
				}
				else
				{
					writer.WriteAttribute(xmlNamespace.Prefix, xmlNamespace.LocalName, xmlNamespace.Value);
				}
			}
		}

		readonly Dictionary<string, TAttribute> attributes;
	}
}
