using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements
{
	abstract class OpenXmlElementBase
	{
		public OpenXmlElementBase(OpenXmlPartsManager partsManager)
		{
			Guard.ThrowExceptionIfNull<OpenXmlPartsManager>(partsManager, "partsManager");
			this.partsManager = partsManager;
			this.attributes = new Dictionary<string, OpenXmlAttributeBase>();
			this.childElements = new QueueDictionary<string, IOpenXmlChildElement>();
			this.InnerText = string.Empty;
		}

		public abstract string ElementName { get; }

		public string UniquePoolId { get; set; }

		public virtual bool AlwaysExport
		{
			get
			{
				return false;
			}
		}

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

		public string InnerText { get; set; }

		public OpenXmlPartsManager PartsManager
		{
			get
			{
				return this.partsManager;
			}
		}

		public OpenXmlPartBase Part { get; set; }

		protected IEnumerable<IOpenXmlChildElement> ChildElements
		{
			get
			{
				return this.childElements.Values;
			}
		}

		public void Write(IOpenXmlWriter writer, IOpenXmlExportContext context)
		{
			this.OnBeforeWrite(context);
			if (this.ShouldExport(context))
			{
				this.WriteElementStart(writer);
				this.WriteAttributes(writer);
				this.WriteInnerText(writer);
				foreach (IOpenXmlChildElement openXmlChildElement in this.ChildElements)
				{
					if (openXmlChildElement.HasElement)
					{
						openXmlChildElement.GetElement().Write(writer, context);
						this.ReleaseElement(openXmlChildElement);
					}
				}
				foreach (OpenXmlElementBase openXmlElementBase in this.EnumerateChildElements(context))
				{
					openXmlElementBase.Write(writer, context);
					this.ReleaseElement(openXmlElementBase);
				}
				OpenXmlElementBase.WriteElementEnd(writer);
			}
		}

		public void Read(IOpenXmlReader reader, IOpenXmlImportContext context)
		{
			OpenXmlElementBase.ReadElementStart(reader);
			if (this.ShouldImport(context))
			{
				bool flag = reader.IsEmptyElement();
				this.ReadAttributes(reader, context);
				this.OnAfterReadAttributes(context);
				reader.Read();
				if (!flag)
				{
					XmlNodeType nodeType = reader.GetNodeType();
					if (nodeType == XmlNodeType.Text || nodeType == XmlNodeType.SignificantWhitespace)
					{
						this.ReadInnerText(reader);
						reader.Read();
					}
					else
					{
						this.OnBeforeReadInnerElements(context);
						this.ReadChildElements(reader, context);
						this.OnAfterReadInnerElements(context);
					}
				}
				this.OnAfterRead(context);
				return;
			}
			reader.SkipElement();
		}

		public void Clear()
		{
			foreach (OpenXmlAttributeBase openXmlAttributeBase in this.attributes.Values)
			{
				openXmlAttributeBase.ResetValue();
			}
			this.InnerText = string.Empty;
			this.Part = null;
			this.ClearOverride();
		}

		protected virtual void ClearOverride()
		{
		}

		protected virtual bool ShouldExport(IOpenXmlExportContext context)
		{
			if (this.AlwaysExport)
			{
				return true;
			}
			if (!string.IsNullOrEmpty(this.InnerText))
			{
				return true;
			}
			return this.attributes.Values.Any((OpenXmlAttributeBase a) => a.ShouldExport()) || this.ChildElements.Any((IOpenXmlChildElement ce) => ce.HasElement && ce.GetElement().ShouldExport(context));
		}

		protected virtual IEnumerable<OpenXmlElementBase> EnumerateChildElements(IOpenXmlExportContext context)
		{
			return Enumerable.Empty<OpenXmlElementBase>();
		}

		protected virtual void OnBeforeWrite(IOpenXmlExportContext context)
		{
		}

		protected virtual bool ShouldImport(IOpenXmlImportContext context)
		{
			return !context.IsImportSuspended;
		}

		protected virtual void OnAfterReadAttributes(IOpenXmlImportContext context)
		{
		}

		protected virtual void OnBeforeReadInnerElements(IOpenXmlImportContext context)
		{
		}

		protected virtual void OnBeforeReadChildElement(IOpenXmlImportContext context, OpenXmlElementBase chiledElement)
		{
		}

		protected virtual void OnAfterReadChildElement(IOpenXmlImportContext context, OpenXmlElementBase childElement)
		{
		}

		protected virtual void OnAfterReadInnerElements(IOpenXmlImportContext context)
		{
		}

		protected virtual void OnAfterRead(IOpenXmlImportContext context)
		{
		}

		protected OpenXmlAttribute<T> RegisterAttribute<T>(string name, OpenXmlNamespace ns, T defaultValue, bool isRequired = false)
		{
			OpenXmlAttribute<T> openXmlAttribute = new OpenXmlAttribute<T>(name, ns, defaultValue, isRequired);
			this.RegisterAttribute<OpenXmlAttribute<T>>(openXmlAttribute);
			return openXmlAttribute;
		}

		protected OpenXmlAttribute<T> RegisterAttribute<T>(string name, OpenXmlNamespace ns, bool isRequired = false)
		{
			OpenXmlAttribute<T> openXmlAttribute = new OpenXmlAttribute<T>(name, ns, isRequired);
			this.RegisterAttribute<OpenXmlAttribute<T>>(openXmlAttribute);
			return openXmlAttribute;
		}

		protected OpenXmlAttribute<T> RegisterAttribute<T>(string name, T defaultValue, bool isRequired = false)
		{
			return this.RegisterAttribute<T>(name, null, defaultValue, isRequired);
		}

		protected OpenXmlAttribute<T> RegisterAttribute<T>(string name, bool isRequired = false)
		{
			return this.RegisterAttribute<T>(name, null, default(T), isRequired);
		}

		protected OpenXmlCountAttribute RegisterCountAttribute()
		{
			OpenXmlCountAttribute openXmlCountAttribute = new OpenXmlCountAttribute();
			this.RegisterAttribute<OpenXmlCountAttribute>(openXmlCountAttribute);
			return openXmlCountAttribute;
		}

		protected SpaceAttribute RegisterSpaceAttribute()
		{
			SpaceAttribute spaceAttribute = new SpaceAttribute(this);
			this.RegisterAttribute<SpaceAttribute>(spaceAttribute);
			return spaceAttribute;
		}

		protected T RegisterAttribute<T>(T attribute) where T : OpenXmlAttributeBase
		{
			Guard.ThrowExceptionIfNull<T>(attribute, "attribute");
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

		protected OpenXmlChildElement<T> RegisterChildElement<T>(string elementName) where T : OpenXmlElementBase
		{
			return this.RegisterChildElement<T>(elementName, elementName);
		}

		protected OpenXmlChildElement<T> RegisterChildElement<T>(string elementName, string uniqueElementName) where T : OpenXmlElementBase
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			Guard.ThrowExceptionIfNullOrEmpty(uniqueElementName, "uniqueElementName");
			OpenXmlChildElement<T> openXmlChildElement = new OpenXmlChildElement<T>(elementName, uniqueElementName);
			this.RegisterChildElementInternal(openXmlChildElement);
			return openXmlChildElement;
		}

		protected OpenXmlChildElement<T> RegisterChildElement<T>(OpenXmlChildElement<T> childElement) where T : OpenXmlElementBase
		{
			Guard.ThrowExceptionIfNull<OpenXmlChildElement<T>>(childElement, "childElement");
			this.RegisterChildElementInternal(childElement);
			return childElement;
		}

		protected virtual OpenXmlElementBase CreateElement(string elementName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			return this.partsManager.CreateElement(elementName, this.Part);
		}

		protected T CreateElement<T>(string elementName) where T : OpenXmlElementBase
		{
			return (T)((object)this.CreateElement(elementName));
		}

		protected virtual void ReleaseElementOverride(OpenXmlElementBase element)
		{
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(element, "element");
			foreach (IOpenXmlChildElement childElement in element.ChildElements)
			{
				element.ReleaseElement(childElement);
			}
			this.partsManager.ReleaseElement(element);
		}

		protected void CreateElement(IOpenXmlChildElement childElement)
		{
			OpenXmlElementBase openXmlElementBase = this.CreateElement(childElement.UniquePoolId);
			openXmlElementBase.UniquePoolId = childElement.UniquePoolId;
			childElement.SetElement(openXmlElementBase);
		}

		protected void ReleaseElement(IOpenXmlChildElement childElement)
		{
			if (childElement.HasElement)
			{
				this.ReleaseElement(childElement.GetElement());
				childElement.ClearElement();
			}
		}

		protected bool TryGetChildElement(string elementName, out IOpenXmlChildElement childElement)
		{
			return this.childElements.TryGetValue(elementName, out childElement);
		}

		static void WriteElementEnd(IOpenXmlWriter writer)
		{
			writer.WriteElementEnd();
		}

		static void ReadElementStart(IOpenXmlReader reader)
		{
			while (reader.GetNodeType() != XmlNodeType.Element)
			{
				if (!reader.Read())
				{
					throw new InvalidOperationException("Element not found");
				}
			}
		}

		void RegisterChildElementInternal(IOpenXmlChildElement child)
		{
			this.childElements[child.ElementName] = child;
		}

		void ReleaseElement(OpenXmlElementBase element)
		{
			this.ReleaseElementOverride(element);
		}

		void ReadChildElements(IOpenXmlReader reader, IOpenXmlImportContext context)
		{
			while (!reader.IsElementOfType(XmlNodeType.EndElement))
			{
				XmlNodeType nodeType = reader.GetNodeType();
				if (nodeType == XmlNodeType.Element)
				{
					string elementName;
					IOpenXmlChildElement openXmlChildElement;
					if (reader.GetElementName(out elementName) && this.TryGetChildElement(elementName, out openXmlChildElement))
					{
						this.CreateElement(openXmlChildElement);
						OpenXmlElementBase openXmlElementBase = openXmlChildElement.GetElement();
						this.OnBeforeReadChildElement(context, openXmlElementBase);
						openXmlElementBase.Read(reader, context);
						this.OnAfterReadChildElement(context, openXmlElementBase);
					}
					else if (reader.GetElementName(out elementName))
					{
						OpenXmlElementBase openXmlElementBase = this.CreateElement(elementName);
						if (openXmlElementBase != null)
						{
							this.OnBeforeReadChildElement(context, openXmlElementBase);
							openXmlElementBase.Read(reader, context);
							this.OnAfterReadChildElement(context, openXmlElementBase);
							this.ReleaseElement(openXmlElementBase);
						}
						else
						{
							reader.SkipElement();
						}
					}
					else
					{
						reader.SkipElement();
					}
				}
				else
				{
					reader.SkipElement();
				}
			}
			reader.Read();
		}

		void ReadAttributes(IOpenXmlReader reader, IOpenXmlImportContext context)
		{
			if (reader.HasAttributes() && reader.MoveToFirstAttribute())
			{
				string key;
				string value;
				while (reader.GetAttributeNameAndValue(out key, out value))
				{
					OpenXmlAttributeBase openXmlAttributeBase;
					if (this.attributes.TryGetValue(key, out openXmlAttributeBase))
					{
						openXmlAttributeBase.SetStringValue(context, value);
					}
					if (!reader.MoveToNextAttribute())
					{
						return;
					}
				}
				throw new InvalidOperationException("Attribute not found.");
			}
		}

		void ReadInnerText(IOpenXmlReader reader)
		{
			string innerText;
			if (reader.GetElementValue(out innerText))
			{
				this.InnerText = innerText;
				reader.Read();
				return;
			}
			reader.SkipElement();
		}

		void WriteElementStart(IOpenXmlWriter writer)
		{
			writer.WriteElementStart(this);
			foreach (OpenXmlNamespace ns in this.Namespaces)
			{
				writer.WriteNamespace(ns);
			}
		}

		void WriteAttributes(IOpenXmlWriter writer)
		{
			foreach (KeyValuePair<string, OpenXmlAttributeBase> keyValuePair in this.attributes)
			{
				if (keyValuePair.Value.ShouldExport())
				{
					writer.WriteAttribute(keyValuePair.Value);
				}
			}
		}

		void WriteInnerText(IOpenXmlWriter writer)
		{
			writer.WriteValue(this.InnerText);
		}

		readonly OpenXmlPartsManager partsManager;

		readonly Dictionary<string, OpenXmlAttributeBase> attributes;

		readonly QueueDictionary<string, IOpenXmlChildElement> childElements;
	}
}
