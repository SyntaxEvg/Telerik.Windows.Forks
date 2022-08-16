using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles
{
	class HtmlBubblingProperties
	{
		public HtmlBubblingProperties()
		{
			this.bubblingProperties = new Dictionary<string, List<object>>();
		}

		public void RegisterProperty(DocumentElementPropertiesBase properties, HtmlStyleProperty htmlStyleProperty, object value)
		{
			this.RegisterProperty(properties, htmlStyleProperty.Name, value);
		}

		public void RegisterProperty(DocumentElementPropertiesBase properties, string propertyName, object value)
		{
			if (!HtmlBubblingProperties.ShouldPropertyBeRegistered(properties, propertyName))
			{
				return;
			}
			this.RegisterProperty(propertyName, value);
		}

		public void RemoveProperty(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.bubblingProperties.Remove(name);
		}

		public void CopyFrom(HtmlBubblingProperties bubblingProperties)
		{
			foreach (KeyValuePair<string, List<object>> keyValuePair in bubblingProperties.bubblingProperties)
			{
				foreach (object value in keyValuePair.Value)
				{
					this.RegisterProperty(keyValuePair.Key, value);
				}
			}
		}

		public void CopyTo(IHtmlImportContext context, IElementWithProperties element)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<IElementWithProperties>(element, "element");
			this.CopyTo(context, element.Properties);
		}

		public void CopyTo(IHtmlImportContext context, DocumentElementPropertiesBase properties)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<DocumentElementPropertiesBase>(properties, "properties");
			foreach (KeyValuePair<string, List<object>> keyValuePair in this.bubblingProperties)
			{
				foreach (HtmlBubblingPropertyRule htmlBubblingPropertyRule in HtmlBubblingPropertyRules.TryGetPropertyRule(keyValuePair.Key))
				{
					IStyleProperty styleProperty = properties.GetStyleProperty(htmlBubblingPropertyRule.PropertyDefinition);
					if (styleProperty != null)
					{
						htmlBubblingPropertyRule.SetMostSuitableCandidateValue(properties, keyValuePair.Value);
					}
				}
			}
		}

		public void Clear()
		{
			this.bubblingProperties.Clear();
		}

		public void RuleOutProperties(IHtmlImportContext context, DocumentElementPropertiesBase properties)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<DocumentElementPropertiesBase>(properties, "properties");
			foreach (string text in this.bubblingProperties.Keys.ToArray<string>())
			{
				if (!HtmlBubblingProperties.ShouldPropertyBeRegistered(properties, text))
				{
					this.RemoveProperty(text);
				}
			}
		}

		static bool ShouldPropertyBeRegistered(DocumentElementPropertiesBase properties, string propertyName)
		{
			foreach (HtmlBubblingPropertyRule htmlBubblingPropertyRule in HtmlBubblingPropertyRules.TryGetPropertyRule(propertyName))
			{
				if (!htmlBubblingPropertyRule.ShouldRegisterProperty(properties))
				{
					return false;
				}
			}
			return true;
		}

		void RegisterProperty(string propertyName, object value)
		{
			if (!this.bubblingProperties.ContainsKey(propertyName))
			{
				this.bubblingProperties.Add(propertyName, new List<object>());
			}
			this.bubblingProperties[propertyName].Add(value);
		}

		readonly Dictionary<string, List<object>> bubblingProperties;
	}
}
