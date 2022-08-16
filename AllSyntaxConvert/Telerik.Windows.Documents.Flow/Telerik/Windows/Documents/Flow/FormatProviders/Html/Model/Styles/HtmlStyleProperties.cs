using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles
{
	class HtmlStyleProperties : IEnumerable<HtmlStyleProperty>, IEnumerable
	{
		public HtmlStyleProperties()
		{
			this.htmlStyleProperties = new Dictionary<string, HtmlStyleProperty>();
		}

		public bool HasProperties
		{
			get
			{
				return this.htmlStyleProperties.Count > 0;
			}
		}

		public void RegisterProperty(string name, string value)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			Guard.ThrowExceptionIfNullOrEmpty(value, "value");
			HtmlStyleProperty property = new HtmlStyleProperty(name, value);
			this.RegisterPropertyInternal(property);
		}

		public bool TryGetProperty(string name, out HtmlStyleProperty property)
		{
			if (this.htmlStyleProperties.ContainsKey(name))
			{
				property = this.htmlStyleProperties[name];
				return true;
			}
			property = null;
			return false;
		}

		public void RemoveProperty(string name)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.htmlStyleProperties.Remove(name);
		}

		public void Write(CssWriter writer)
		{
			foreach (HtmlStyleProperty htmlStyleProperty in this.htmlStyleProperties.Values)
			{
				writer.WriteStyleProperty(htmlStyleProperty.Name, htmlStyleProperty.UnparsedValue);
			}
		}

		public void CopyFrom(Selector style)
		{
			Guard.ThrowExceptionIfNull<Selector>(style, "style");
			this.CopyPropertiesInternal(style, false);
		}

		public void CopyFrom(HtmlStyleProperties properties)
		{
			Guard.ThrowExceptionIfNull<HtmlStyleProperties>(properties, "properties");
			this.CopyPropertiesInternal(properties, false);
		}

		public void CopyInheritablePropertiesFrom(HtmlStyleProperties properties, bool onlyInheritable = true)
		{
			Guard.ThrowExceptionIfNull<HtmlStyleProperties>(properties, "properties");
			this.CopyPropertiesInternal(properties, onlyInheritable);
		}

		public void CopyFrom(IHtmlExportContext context, DocumentElementPropertiesBase properties)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			if (properties != null)
			{
				foreach (IStyleProperty property in properties.StyleProperties)
				{
					this.CopyFrom(context, property, properties, true);
				}
			}
		}

		public void CopyFrom(IHtmlExportContext context, IStyleProperty property, DocumentElementPropertiesBase properties, bool shouldCalculateExportOfActualValue = true)
		{
			foreach (HtmlPropertyMapperElement htmlPropertyMapperElement in HtmlPropertyMappers.TryGetPropertyMappingElements(property.PropertyDefinition))
			{
				bool flag = HtmlStyleProperties.ShouldExportActualValue(property, htmlPropertyMapperElement.HtmlPropertyDescriptor);
				if (flag || property.HasLocalValue || !shouldCalculateExportOfActualValue)
				{
					object obj = ((flag || !shouldCalculateExportOfActualValue) ? property.GetActualValueAsObject() : property.GetLocalValueAsObject());
					string value;
					if (obj != null && htmlPropertyMapperElement.Converter.ConvertBack(context, obj, htmlPropertyMapperElement.HtmlPropertyDescriptor, properties, out value) && !string.IsNullOrEmpty(value))
					{
						this.RegisterProperty(htmlPropertyMapperElement.HtmlPropertyDescriptor.Name, value);
					}
				}
			}
		}

		public void CopyTo(IHtmlImportContext context, DocumentElementPropertiesBase properties)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			this.CopyTo(context, properties, null);
		}

		public void CopyTo(IHtmlImportContext context, DocumentElementPropertiesBase properties, HtmlBubblingProperties bubblingProperties)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			if (properties != null)
			{
				foreach (KeyValuePair<string, HtmlStyleProperty> keyValuePair in this.htmlStyleProperties)
				{
					if (!string.IsNullOrEmpty(keyValuePair.Value.UnparsedValue))
					{
						foreach (HtmlPropertyMapperElement htmlPropertyMapperElement in HtmlPropertyMappers.TryGetPropertyMappingElements(keyValuePair.Value))
						{
							IStylePropertyDefinition propertyDefinition = htmlPropertyMapperElement.PropertyDefinition;
							if (propertyDefinition != null)
							{
								IStyleProperty styleProperty = properties.GetStyleProperty(propertyDefinition);
								if (styleProperty != null || htmlPropertyMapperElement.HtmlPropertyDescriptor.IsBubblingInheritable)
								{
									object obj = null;
									if (htmlPropertyMapperElement.Converter.Convert(context, keyValuePair.Value, properties, out obj) && obj != null)
									{
										if (styleProperty != null)
										{
											styleProperty.SetValueAsObject(obj);
										}
										else if (bubblingProperties != null && htmlPropertyMapperElement.HtmlPropertyDescriptor.IsBubblingInheritable && properties.OwnerDocumentElement != null)
										{
											bubblingProperties.RegisterProperty(properties, keyValuePair.Value, obj);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		public void ClearProperties()
		{
			this.htmlStyleProperties.Clear();
		}

		public IEnumerator<HtmlStyleProperty> GetEnumerator()
		{
			return this.htmlStyleProperties.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		protected void CopyFromIncludingNonHtmlDefaultActualValues(IHtmlExportContext context, DocumentElementPropertiesBase properties)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			if (properties != null)
			{
				foreach (IStyleProperty styleProperty in properties.StyleProperties)
				{
					bool flag = false;
					bool flag2 = false;
					if (styleProperty.HasLocalValue)
					{
						flag = true;
						flag2 = false;
					}
					else if (StylePropertyHelper.IsStylePropertyWithNonDefaultHtmlActualValue(styleProperty.PropertyDefinition))
					{
						flag = true;
						flag2 = true;
					}
					if (flag)
					{
						foreach (HtmlPropertyMapperElement htmlPropertyMapperElement in HtmlPropertyMappers.TryGetPropertyMappingElements(styleProperty.PropertyDefinition))
						{
							object obj = (flag2 ? styleProperty.GetActualValueAsObject() : styleProperty.GetLocalValueAsObject());
							string value;
							if (obj != null && htmlPropertyMapperElement.Converter.ConvertBack(context, obj, htmlPropertyMapperElement.HtmlPropertyDescriptor, properties, out value) && !string.IsNullOrEmpty(value))
							{
								this.RegisterProperty(htmlPropertyMapperElement.HtmlPropertyDescriptor.Name, value);
							}
						}
					}
				}
			}
		}

		static bool ShouldExportActualValue(IStyleProperty styleProperty, HtmlStylePropertyDescriptor htmlDescriptor)
		{
			return styleProperty.PropertyDefinition == TableCell.BordersPropertyDefinition || styleProperty.PropertyDefinition == TableCell.PaddingPropertyDefinition || htmlDescriptor == HtmlStylePropertyDescriptors.BorderCollapsePropertyDescriptor;
		}

		void RegisterPropertyInternal(HtmlStyleProperty property)
		{
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			if (this.htmlStyleProperties.ContainsKey(property.Name))
			{
				HtmlStyleProperty value = new HtmlStyleProperty(property, this.htmlStyleProperties[property.Name]);
				this.htmlStyleProperties[property.Name] = value;
				return;
			}
			this.htmlStyleProperties[property.Name] = property;
		}

		void CopyPropertiesInternal(HtmlStyleProperties properties, bool onlyInheritable)
		{
			foreach (HtmlStyleProperty htmlStyleProperty in properties.htmlStyleProperties.Values)
			{
				HtmlStylePropertyDescriptor htmlStylePropertyDescriptor;
				if (HtmlStylePropertyDescriptors.TryGetPropertyDescriptor(htmlStyleProperty.Name, out htmlStylePropertyDescriptor))
				{
					bool flag = true;
					if (onlyInheritable)
					{
						flag = htmlStylePropertyDescriptor.IsInheritable;
					}
					if (flag)
					{
						this.RegisterPropertyInternal(htmlStyleProperty);
					}
				}
			}
		}

		readonly Dictionary<string, HtmlStyleProperty> htmlStyleProperties;
	}
}
