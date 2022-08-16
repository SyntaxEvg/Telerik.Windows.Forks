using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public sealed class DocumentProperties : DocumentElementPropertiesBase
	{
		internal DocumentProperties(RadFlowDocument document)
			: base(document, false)
		{
			this.InitProperties();
		}

		public IStyleProperty<bool?> HasDifferentEvenOddPageHeadersFooters
		{
			get
			{
				return this.hasDifferentEvenOddPageHeadersFootersProperty;
			}
		}

		public IStyleProperty<DocumentViewType?> ViewType
		{
			get
			{
				return this.viewTypeProperty;
			}
		}

		public IStyleProperty<double?> DefaultTabStopWidth
		{
			get
			{
				return this.defaultTabStopWidthProperty;
			}
		}

		protected override IEnumerable<IStyleProperty> EnumerateStyleProperties()
		{
			yield return this.HasDifferentEvenOddPageHeadersFooters;
			yield return this.ViewType;
			yield return this.DefaultTabStopWidth;
			yield break;
		}

		protected override IStyleProperty GetStylePropertyOverride(IStylePropertyDefinition propertyDefinition)
		{
			if (propertyDefinition != DocumentElementPropertiesBase.StyleIdPropertyDefinition && propertyDefinition.StylePropertyType != StylePropertyType.Document)
			{
				return null;
			}
			return DocumentProperties.stylePropertyGetters[propertyDefinition.GlobalPropertyIndex](this);
		}

		static void InitStylePropertyGetters()
		{
			if (DocumentProperties.stylePropertyGetters != null)
			{
				return;
			}
			DocumentProperties.stylePropertyGetters = new Func<DocumentProperties, IStyleProperty>[4];
			DocumentProperties.stylePropertyGetters[DocumentElementPropertiesBase.StyleIdPropertyDefinition.GlobalPropertyIndex] = (DocumentProperties prop) => prop.StyleIdProperty;
			DocumentProperties.stylePropertyGetters[RadFlowDocument.HasDifferentEvenOddPageHeadersFootersPropertyDefinition.GlobalPropertyIndex] = (DocumentProperties prop) => prop.hasDifferentEvenOddPageHeadersFootersProperty;
			DocumentProperties.stylePropertyGetters[RadFlowDocument.ViewTypePropertyDefinition.GlobalPropertyIndex] = (DocumentProperties prop) => prop.viewTypeProperty;
			DocumentProperties.stylePropertyGetters[RadFlowDocument.DefaultTabStopWidthPropertyDefinition.GlobalPropertyIndex] = (DocumentProperties prop) => prop.defaultTabStopWidthProperty;
		}

		void InitProperties()
		{
			this.hasDifferentEvenOddPageHeadersFootersProperty = new LocalProperty<bool?>(RadFlowDocument.HasDifferentEvenOddPageHeadersFootersPropertyDefinition, this);
			this.viewTypeProperty = new LocalProperty<DocumentViewType?>(RadFlowDocument.ViewTypePropertyDefinition, this);
			this.defaultTabStopWidthProperty = new LocalProperty<double?>(RadFlowDocument.DefaultTabStopWidthPropertyDefinition, this);
			DocumentProperties.InitStylePropertyGetters();
		}

		static Func<DocumentProperties, IStyleProperty>[] stylePropertyGetters;

		LocalProperty<bool?> hasDifferentEvenOddPageHeadersFootersProperty;

		LocalProperty<DocumentViewType?> viewTypeProperty;

		LocalProperty<double?> defaultTabStopWidthProperty;
	}
}
