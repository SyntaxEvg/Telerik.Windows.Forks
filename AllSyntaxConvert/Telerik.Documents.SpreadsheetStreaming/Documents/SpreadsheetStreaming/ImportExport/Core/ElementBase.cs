using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.Model.Themes;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core
{
	abstract class ElementBase : IChildEntry
	{
		public ElementBase()
		{
			this.attributes = new Dictionary<string, OpenXmlAttributeBase>();
		}

		public bool IsEmptyElement { get; set; }

		public virtual OpenXmlNamespace Namespace
		{
			get
			{
				return null;
			}
		}

		public virtual IEnumerable<OpenXmlNamespace> Namespaces
		{
			get
			{
				return Enumerable.Empty<OpenXmlNamespace>();
			}
		}

		public abstract string ElementName { get; }

		public bool IsWritingStarted { get; set; }

		public bool IsWritingEnded { get; set; }

		public bool IsReadingStarted { get; set; }

		public bool IsReadingEnded { get; set; }

		bool IChildEntry.IsUsageBegan
		{
			get
			{
				return this.IsWritingStarted || this.IsReadingStarted;
			}
		}

		bool IChildEntry.IsUsageCompleted
		{
			get
			{
				return this.IsWritingEnded || this.IsReadingEnded;
			}
		}

		protected string InnerText { get; set; }

		protected virtual bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		protected SpreadTheme Theme { get; set; }

		protected OpenXmlWriter Writer { get; set; }

		protected OpenXmlReader Reader { get; set; }

		public void SetContext(ElementContext context)
		{
			this.Writer = context.Writer;
			this.Reader = context.Reader;
			this.Theme = context.Theme;
		}

		protected void BeginWrite()
		{
			this.WriteElementStart();
			this.WriteAttributes();
			this.WriteInnerText();
			this.IsWritingStarted = true;
		}

		protected void EndWrite()
		{
			this.WriteElementEnd();
			this.IsWritingEnded = true;
		}

		protected void BeginRead()
		{
			this.ReadElementStart();
			this.IsEmptyElement = this.Reader.IsEmptyElement();
			this.ReadAttributes();
			this.Reader.Read();
			this.IsReadingStarted = true;
		}

		protected void EndRead()
		{
			this.IsReadingEnded = true;
		}

		protected bool ShouldExport()
		{
			if (this.AlwaysExport)
			{
				return true;
			}
			if (!string.IsNullOrEmpty(this.InnerText))
			{
				return true;
			}
			return this.attributes.Values.Any((OpenXmlAttributeBase a) => a.ShouldExport());
		}

		protected OpenXmlCountAttribute RegisterCountAttribute()
		{
			OpenXmlCountAttribute openXmlCountAttribute = new OpenXmlCountAttribute();
			this.RegisterAttribute<OpenXmlCountAttribute>(openXmlCountAttribute);
			return openXmlCountAttribute;
		}

		protected OpenXmlAttribute<TValue> RegisterAttribute<TValue>(string name, TValue defaultValue, bool isRequired = false)
		{
			return this.RegisterAttribute<TValue>(name, null, defaultValue, isRequired);
		}

		protected OpenXmlAttribute<TValue> RegisterAttribute<TValue>(string name, bool isRequired = false)
		{
			return this.RegisterAttribute<TValue>(name, null, default(TValue), isRequired);
		}

		protected OpenXmlAttribute<TValue> RegisterAttribute<TValue>(string name, OpenXmlNamespace ns, TValue defaultValue, bool isRequired = false)
		{
			OpenXmlAttribute<TValue> openXmlAttribute = new OpenXmlAttribute<TValue>(name, ns, defaultValue, isRequired);
			this.RegisterAttribute<OpenXmlAttribute<TValue>>(openXmlAttribute);
			return openXmlAttribute;
		}

		protected TValue RegisterAttribute<TValue>(TValue attribute) where TValue : OpenXmlAttributeBase
		{
			if (attribute.Namespace != null)
			{
				this.attributes[string.Format("{0}:{1}", attribute.Namespace.LocalName, attribute.Name)] = attribute;
			}
			else
			{
				this.attributes[attribute.Name] = attribute;
			}
			return attribute;
		}

		protected ElementBase CreateElement(string elementName)
		{
			ElementContext context = new ElementContext(this.Writer, this.Reader, this.Theme);
			return ElementsFactory.CreateElement(elementName, context);
		}

		protected void ReadInnerText()
		{
			string innerText;
			if (this.Reader.GetElementValue(out innerText))
			{
				this.InnerText = innerText;
				this.Reader.Read();
				return;
			}
			this.Reader.SkipElement();
		}

		void WriteElementStart()
		{
			this.Writer.WriteElementStart(this);
			foreach (OpenXmlNamespace ns in this.Namespaces)
			{
				this.Writer.WriteNamespace(ns);
			}
		}

		void WriteAttributes()
		{
			foreach (KeyValuePair<string, OpenXmlAttributeBase> keyValuePair in this.attributes)
			{
				OpenXmlAttributeBase value = keyValuePair.Value;
				if (value.ShouldExport())
				{
					this.Writer.WriteAttribute(value);
				}
				value.MarkAsWritten();
			}
		}

		void WriteInnerText()
		{
			this.Writer.WriteValue(this.InnerText);
		}

		void WriteElementEnd()
		{
			this.Writer.WriteElementEnd();
		}

		void ReadElementStart()
		{
			while (this.Reader.GetNodeType() != XmlNodeType.Element)
			{
				if (!this.Reader.Read())
				{
					throw new InvalidOperationException("Element not found");
				}
			}
		}

		void ReadAttributes()
		{
			if (this.Reader.HasAttributes() && this.Reader.MoveToFirstAttribute())
			{
				string key;
				string stringValue;
				while (this.Reader.GetAttributeNameAndValue(out key, out stringValue))
				{
					OpenXmlAttributeBase openXmlAttributeBase;
					if (this.attributes.TryGetValue(key, out openXmlAttributeBase))
					{
						openXmlAttributeBase.SetStringValue(stringValue);
					}
					if (!this.Reader.MoveToNextAttribute())
					{
						return;
					}
				}
				throw new InvalidOperationException("Attribute not found.");
			}
		}

		readonly Dictionary<string, OpenXmlAttributeBase> attributes;
	}
}
