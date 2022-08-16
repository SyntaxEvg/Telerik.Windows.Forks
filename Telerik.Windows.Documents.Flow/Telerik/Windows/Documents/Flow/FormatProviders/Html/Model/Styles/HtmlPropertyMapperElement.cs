using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles
{
	class HtmlPropertyMapperElement
	{
		public HtmlPropertyMapperElement(HtmlStylePropertyDescriptor htmlPropertyDescriptor, IStylePropertyDefinition propertyDefinition, IStringConverter converter, bool isDescriptorToDefinitionElement, bool isDefinitionToDescriptorElement)
		{
			Guard.ThrowExceptionIfNull<IStringConverter>(converter, "converter");
			Guard.ThrowExceptionIfNull<IStylePropertyDefinition>(propertyDefinition, "propertyDefinition");
			Guard.ThrowExceptionIfNull<HtmlStylePropertyDescriptor>(htmlPropertyDescriptor, "htmlPropertyDescriptor");
			this.propertyDefinition = propertyDefinition;
			this.htmlPropertyDescriptor = htmlPropertyDescriptor;
			this.converter = converter;
			this.isDescriptorToDefinitionElement = isDescriptorToDefinitionElement;
			this.isDefinitionToDescriptorElement = isDefinitionToDescriptorElement;
		}

		public HtmlStylePropertyDescriptor HtmlPropertyDescriptor
		{
			get
			{
				return this.htmlPropertyDescriptor;
			}
		}

		public IStringConverter Converter
		{
			get
			{
				return this.converter;
			}
		}

		public bool IsDescriptorToDefinitionElement
		{
			get
			{
				return this.isDescriptorToDefinitionElement;
			}
		}

		public bool IsDefinitionToDescriptorElement
		{
			get
			{
				return this.isDefinitionToDescriptorElement;
			}
		}

		public IStylePropertyDefinition PropertyDefinition
		{
			get
			{
				return this.propertyDefinition;
			}
		}

		readonly IStylePropertyDefinition propertyDefinition;

		readonly bool isDescriptorToDefinitionElement;

		readonly bool isDefinitionToDescriptorElement;

		readonly IStringConverter converter;

		readonly HtmlStylePropertyDescriptor htmlPropertyDescriptor;
	}
}
