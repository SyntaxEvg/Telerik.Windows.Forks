using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Elements;
using Telerik.Windows.Documents.Common.FormatProviders.XmlBased.Model.Namespaces;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Text;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements
{
	abstract class HtmlElementBase : XmlElementBase<HtmlAttribute>
	{
		public HtmlElementBase(HtmlContentManager contentManager)
		{
			Guard.ThrowExceptionIfNull<HtmlContentManager>(contentManager, "contentManager");
			this.contentManager = contentManager;
			this.styleAttribute = new StyleAttribute();
			base.RegisterAttribute(this.styleAttribute);
			this.classAttribute = new ClassAttribute();
			base.RegisterAttribute(this.classAttribute);
			this.inheritedProperties = new HtmlStyleProperties();
			this.bubblingProperties = new HtmlBubblingProperties();
		}

		public ClassNamesCollection Classes
		{
			get
			{
				return this.classAttribute.Value;
			}
		}

		public StyleAttribute Style
		{
			get
			{
				return this.styleAttribute;
			}
		}

		public HtmlStyleProperties InheritedProperties
		{
			get
			{
				return this.inheritedProperties;
			}
		}

		public HtmlStyleProperties LocalProperties
		{
			get
			{
				return this.Style.Value;
			}
		}

		public HtmlBubblingProperties BubblingProperties
		{
			get
			{
				return this.bubblingProperties;
			}
		}

		protected HtmlContentManager ContentManager
		{
			get
			{
				return this.contentManager;
			}
		}

		protected virtual bool CanHaveStyle
		{
			get
			{
				return false;
			}
		}

		protected virtual bool ForceFullEndElement
		{
			get
			{
				return false;
			}
		}

		protected virtual bool PreserveWhiteSpaces
		{
			get
			{
				return false;
			}
		}

		public void Read(IHtmlReader reader, IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			this.OnBeforeRead(reader, context);
			reader.BeginReadElement();
			while (reader.MoveToNextAttribute())
			{
				string attributeName = reader.GetAttributeName();
				base.TrySetAttributeValue(attributeName, reader.GetAttributeValue());
			}
			this.EvaluateLocalProperties(context);
			this.OnAfterReadAttributes(reader, context);
			this.EvaluatePropertiesAfterReadAttributes(context);
			string innerText = reader.GetInnerText();
			if (!string.IsNullOrEmpty(innerText))
			{
				this.OnReadInnerText(reader, context, innerText);
			}
			if (reader.HasChildElements())
			{
				do
				{
					HtmlElementBase htmlElementBase;
					if (reader.GetCurrentChildElementType() == HtmlElementType.Text)
					{
						htmlElementBase = this.CreateElement("TEXT");
					}
					else
					{
						string currentChildElementName = reader.GetCurrentChildElementName();
						htmlElementBase = this.CreateElement(currentChildElementName);
					}
					if (htmlElementBase != null)
					{
						bool onlyInheritable = HtmlElementBase.ShouldCopyOnlyInheritableProperties(reader.IsInsideSpanElement(), htmlElementBase.GetType());
						htmlElementBase.CopyInheritablePropertiesFrom(this.LocalProperties, onlyInheritable);
						this.OnBeforeReadChildElement(reader, context, htmlElementBase);
						htmlElementBase.Read(reader, context);
						this.OnAfterReadChildElement(reader, context, htmlElementBase);
						this.CopyBubblingPropertiesFrom(context, htmlElementBase.BubblingProperties);
						this.ReleaseElement(htmlElementBase);
					}
				}
				while (reader.MoveToNextChildElement());
			}
			reader.EndReadElement();
			this.OnAfterRead(reader, context);
		}

		public virtual void Write(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			this.OnBeforeWrite(writer, context);
			base.WriteElementStart(writer);
			this.WriteAttributes(writer, context);
			this.WriteContent(writer, context);
			writer.WriteElementEnd(this.ForceFullEndElement);
			this.OnAfterWrite(writer, context);
		}

		protected virtual void WriteContent(IHtmlWriter writer, IHtmlExportContext context)
		{
			if (!string.IsNullOrEmpty(base.InnerText))
			{
				writer.WriteValue(base.InnerText, this.PreserveWhiteSpaces);
			}
			foreach (HtmlElementBase htmlElementBase in this.OnEnumerateInnerElements(writer, context))
			{
				htmlElementBase.Write(writer, context);
				this.ReleaseElement(htmlElementBase);
			}
		}

		protected HtmlAttribute<T> RegisterAttribute<T>(string name, XmlNamespace ns, T defaultValue, bool isRequired = false)
		{
			HtmlAttribute<T> htmlAttribute = new HtmlAttribute<T>(name, ns, defaultValue, isRequired);
			base.RegisterAttribute(htmlAttribute);
			return htmlAttribute;
		}

		protected HtmlAttribute<T> RegisterAttribute<T>(string name, XmlNamespace ns, bool isRequired = false)
		{
			HtmlAttribute<T> htmlAttribute = new HtmlAttribute<T>(name, ns, isRequired);
			base.RegisterAttribute(htmlAttribute);
			return htmlAttribute;
		}

		protected HtmlAttribute<T> RegisterAttribute<T>(string name, T defaultValue, bool isRequired = false)
		{
			return this.RegisterAttribute<T>(name, null, defaultValue, isRequired);
		}

		protected HtmlAttribute<T> RegisterAttribute<T>(string name, bool isRequired = false)
		{
			return this.RegisterAttribute<T>(name, null, default(T), isRequired);
		}

		protected virtual void OnBeforeRead(IHtmlReader reader, IHtmlImportContext context)
		{
		}

		protected virtual void OnAfterReadAttributes(IHtmlReader reader, IHtmlImportContext context)
		{
		}

		protected virtual void EvaluatePropertiesAfterReadAttributes(IHtmlImportContext context)
		{
		}

		protected virtual void OnBeforeReadChildElement(IHtmlReader reader, IHtmlImportContext context, HtmlElementBase innerElement)
		{
		}

		protected virtual void OnAfterReadChildElement(IHtmlReader reader, IHtmlImportContext context, HtmlElementBase innerElement)
		{
		}

		protected virtual void OnAfterRead(IHtmlReader reader, IHtmlImportContext context)
		{
		}

		protected virtual void OnReadInnerText(IHtmlReader reader, IHtmlImportContext context, string text)
		{
		}

		protected virtual void OnBeforeWrite(IHtmlWriter writer, IHtmlExportContext context)
		{
		}

		protected virtual IEnumerable<HtmlElementBase> OnEnumerateInnerElements(IHtmlWriter writer, IHtmlExportContext context)
		{
			return Enumerable.Empty<HtmlElementBase>();
		}

		protected virtual void OnAfterWrite(IHtmlWriter writer, IHtmlExportContext context)
		{
		}

		protected void CopyLocalPropertiesFrom(IHtmlExportContext context, IElementWithProperties element)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<IElementWithProperties>(element, "element");
			if (element.Properties != null && element.Properties.HasLocalValues())
			{
				this.LocalProperties.CopyFrom(context, element.Properties);
			}
		}

		protected void CopyStyleFrom(IHtmlExportContext context, IElementWithStyle element)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<IElementWithStyle>(element, "element");
			if (!string.IsNullOrEmpty(element.StyleId))
			{
				this.CopyStylePropertiesFrom(context, element.StyleId);
				return;
			}
			string styleId;
			if (this.GetDefaultStyleId(context, out styleId))
			{
				this.CopyStylePropertiesFrom(context, styleId);
			}
		}

		protected virtual bool GetDefaultStyleId(IHtmlExportContext context, out string styleId)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			styleId = null;
			return false;
		}

		protected void CopyStylePropertiesFrom(IHtmlExportContext context, string styleId)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNullOrEmpty(styleId, "styleId");
			switch (context.Settings.StylesExportMode)
			{
			case StylesExportMode.External:
			case StylesExportMode.Embedded:
				this.SetClassNamesFromStyle(context, styleId);
				return;
			case StylesExportMode.Inline:
				this.CopyStylePropertiesAsLocalProperties(context, styleId);
				return;
			default:
				return;
			}
		}

		protected void RemoveLocalPropertiesAlreadyInAppliedStyles(IHtmlExportContext context, string elementType)
		{
			if (this.Classes.Count > 0)
			{
				foreach (HtmlStyleProperty htmlStyleProperty in this.LocalProperties.ToList<HtmlStyleProperty>())
				{
					foreach (string styleId in this.Classes.ToEnumerable())
					{
						Selector style = context.HtmlStyleRepository.GetStyle(styleId, elementType);
						HtmlStyleProperty htmlStyleProperty2;
						if (style != null && style.TryGetProperty(htmlStyleProperty.Name, out htmlStyleProperty2) && htmlStyleProperty.UnparsedValue.Equals(htmlStyleProperty2.UnparsedValue))
						{
							this.LocalProperties.RemoveProperty(htmlStyleProperty.Name);
						}
					}
				}
			}
		}

		protected void CopyLocalPropertiesTo(IHtmlImportContext context, IElementWithProperties element)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<IElementWithProperties>(element, "element");
			this.CopyLocalPropertiesTo(context, element.Properties);
		}

		protected void CopyLocalPropertiesTo(IHtmlImportContext context, DocumentElementPropertiesBase properties)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<DocumentElementPropertiesBase>(properties, "properties");
			if (this.LocalProperties.HasProperties)
			{
				this.LocalProperties.CopyTo(context, properties, this.BubblingProperties);
			}
		}

		protected virtual void RuleOutBubblingProperties(IHtmlImportContext context, IElementWithProperties element)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<IElementWithProperties>(element, "element");
			this.RuleOutBubblingProperties(context, element.Properties);
		}

		protected virtual void RuleOutBubblingProperties(IHtmlImportContext context, DocumentElementPropertiesBase properties)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<DocumentElementPropertiesBase>(properties, "properties");
			this.BubblingProperties.RuleOutProperties(context, properties);
		}

		protected void ApplyStyle(IHtmlImportContext context, IElementWithStyle element)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<IElementWithStyle>(element, "element");
			if (this.Classes.Count > 0)
			{
				element.StyleId = this.Classes.GetLast();
			}
			this.CopyStyleToOverride(context, element);
		}

		protected virtual void CopyStyleToOverride(IHtmlImportContext context, IElementWithStyle element)
		{
		}

		protected virtual HtmlElementBase CreateElement(string elementName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			return this.contentManager.CreateElement(elementName, false);
		}

		protected T CreateElement<T>(string elementName) where T : HtmlElementBase
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			return (T)((object)this.CreateElement(elementName));
		}

		protected void ReleaseElement(HtmlElementBase element)
		{
			Guard.ThrowExceptionIfNull<HtmlElementBase>(element, "element");
			this.contentManager.ReleaseElement(element);
		}

		protected override void ClearOverride()
		{
			base.ClearOverride();
			this.InheritedProperties.ClearProperties();
			this.BubblingProperties.Clear();
		}

		static bool ShouldCopyOnlyInheritableProperties(bool isInSpanElement, Type elementType)
		{
			return !isInSpanElement || (!(elementType == typeof(TextElement)) && !(elementType == typeof(TextBasedElement)));
		}

		void WriteAttributes(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			foreach (HtmlAttribute htmlAttribute in base.Attributes)
			{
				if (htmlAttribute.ShouldExport(writer, context))
				{
					XmlElementBase<HtmlAttribute>.WriteAttribute(writer, htmlAttribute);
				}
			}
		}

		void CopyInheritablePropertiesFrom(HtmlStyleProperties parentProperties, bool onlyInheritable = true)
		{
			Guard.ThrowExceptionIfNull<HtmlStyleProperties>(parentProperties, "parentProperties");
			this.InheritedProperties.CopyInheritablePropertiesFrom(parentProperties, onlyInheritable);
		}

		void EvaluateLocalProperties(IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			HtmlStyleProperties htmlStyleProperties = new HtmlStyleProperties();
			this.ApplyInheritedProperties(context, htmlStyleProperties);
			this.ApplyStyleProperties(context, htmlStyleProperties);
			this.ApplyLocalProperties(context, htmlStyleProperties);
			this.Style.Value = htmlStyleProperties;
		}

		void ApplyInheritedProperties(IHtmlImportContext context, HtmlStyleProperties properties)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlStyleProperties>(properties, "properties");
			properties.CopyFrom(this.InheritedProperties);
		}

		void ApplyStyleProperties(IHtmlImportContext context, HtmlStyleProperties properties)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlStyleProperties>(properties, "properties");
			if (this.Classes.Count > 0)
			{
				string text = null;
				if (!this.CanHaveStyle)
				{
					goto IL_E2;
				}
				text = this.Classes.GetFirst();
				Selector selector;
				if (!context.HtmlStyleRepository.TryGetStyle(text, out selector))
				{
					goto IL_E2;
				}
				using (IEnumerator<HtmlStyleProperty> enumerator = selector.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						HtmlStyleProperty htmlStyleProperty = enumerator.Current;
						HtmlStylePropertyDescriptor htmlStylePropertyDescriptor;
						if (htmlStyleProperty.Values.HasRelativeValues && HtmlStylePropertyDescriptors.TryGetPropertyDescriptor(htmlStyleProperty.Name, out htmlStylePropertyDescriptor) && htmlStylePropertyDescriptor.ApplyAsLocalValueIfRelative)
						{
							properties.RegisterProperty(htmlStyleProperty.Name, htmlStyleProperty.UnparsedValue);
						}
						else
						{
							properties.RemoveProperty(htmlStyleProperty.Name);
						}
					}
					goto IL_E2;
				}
				IL_C0:
				Selector style;
				if (context.HtmlStyleRepository.TryGetStyle(this.Classes.GetFirst(), out style))
				{
					properties.CopyFrom(style);
				}
				IL_E2:
				if (this.Classes.Count > 0)
				{
					goto IL_C0;
				}
				if (this.CanHaveStyle && !string.IsNullOrEmpty(text))
				{
					this.Classes.AddFirst(text);
				}
			}
		}

		void ApplyLocalProperties(IHtmlImportContext context, HtmlStyleProperties properties)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlStyleProperties>(properties, "properties");
			properties.CopyFrom(this.LocalProperties);
		}

		void SetClassNamesFromStyle(IHtmlExportContext context, string styleId)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNullOrEmpty(styleId, "styleId");
			string text = StyleNamesConverter.ConvertStyleNameOnExport(styleId);
			if (context.HtmlStyleRepository.ContainsStyle(text))
			{
				this.Classes.AddFirst(text);
				Style style = context.Document.StyleRepository.GetStyle(styleId);
				if (style != null)
				{
					while (!string.IsNullOrEmpty(style.BasedOnStyleId))
					{
						style = context.Document.StyleRepository.GetStyle(style.BasedOnStyleId);
						if (style != null)
						{
							text = StyleNamesConverter.ConvertStyleNameOnExport(style.Id);
							if (context.HtmlStyleRepository.ContainsStyle(text))
							{
								this.Classes.AddFirst(text);
							}
						}
					}
				}
			}
		}

		void CopyStylePropertiesAsLocalProperties(IHtmlExportContext context, string styleId)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNullOrEmpty(styleId, "styleId");
			Style style = context.Document.StyleRepository.GetStyle(styleId);
			if (style != null)
			{
				string styleId2 = StyleNamesConverter.ConvertStyleNameOnExport(style.Id);
				this.LocalProperties.CopyFrom(context.HtmlStyleRepository.GetStyle(styleId2));
				while (!string.IsNullOrEmpty(style.BasedOnStyleId))
				{
					style = context.Document.StyleRepository.GetStyle(style.BasedOnStyleId);
					if (style != null)
					{
						styleId2 = StyleNamesConverter.ConvertStyleNameOnExport(style.Id);
						this.LocalProperties.CopyFrom(context.HtmlStyleRepository.GetStyle(styleId2));
					}
				}
			}
		}

		void CopyBubblingPropertiesFrom(IHtmlImportContext context, HtmlBubblingProperties bubblingProperties)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlBubblingProperties>(bubblingProperties, "bubblingProperties");
			this.BubblingProperties.CopyFrom(bubblingProperties);
		}

		readonly HtmlContentManager contentManager;

		readonly StyleAttribute styleAttribute;

		readonly ClassAttribute classAttribute;

		readonly HtmlStyleProperties inheritedProperties;

		readonly HtmlBubblingProperties bubblingProperties;
	}
}
