using System;
using Telerik.Windows.Documents.Flow.Model.Lists;

namespace Telerik.Windows.Documents.Flow.Model.Styles.Core
{
	class StyleProperty<TValue> : StylePropertyBase<TValue>
	{
		public StyleProperty(StylePropertyDefinition<TValue> propertyDefinition, DocumentElementPropertiesBase propertyContainer)
			: base(propertyDefinition, propertyContainer)
		{
		}

		public override TValue GetActualValue()
		{
			if (this.HasLocalValue)
			{
				return base.LocalValue;
			}
			return this.GetActualValueInternal();
		}

		internal TValue GetDocumentDefaultValue(RadFlowDocument rootDocument)
		{
			TValue result = default(TValue);
			if (rootDocument != null)
			{
				if (base.PropertyContainer is CharacterProperties)
				{
					IStyleProperty styleProperty = rootDocument.DefaultStyle.CharacterProperties.GetStyleProperty(base.PropertyDefinition);
					object localValueAsObject = styleProperty.GetLocalValueAsObject();
					if (localValueAsObject != null)
					{
						result = (TValue)((object)localValueAsObject);
					}
				}
				else if (base.PropertyContainer is ParagraphProperties)
				{
					IStyleProperty styleProperty2 = rootDocument.DefaultStyle.ParagraphProperties.GetStyleProperty(base.PropertyDefinition);
					object localValueAsObject2 = styleProperty2.GetLocalValueAsObject();
					if (localValueAsObject2 != null)
					{
						result = (TValue)((object)localValueAsObject2);
					}
				}
			}
			return result;
		}

		protected TValue GetActualValueInternal()
		{
			if (!base.PropertyContainer.SuppressStylePropertyEvaluation && base.PropertyContainer.Document != null)
			{
				if (base.PropertyContainer.OwnerDocumentElement != null)
				{
					return this.GetValueFromDocumentElement();
				}
				if (base.PropertyContainer.OwnerStyle != null)
				{
					TValue valueFromStyle = this.GetValueFromStyle();
					if (valueFromStyle != null)
					{
						return valueFromStyle;
					}
					return base.DefaultValue;
				}
				else if (base.PropertyContainer.OwnerListLevel != null)
				{
					return this.GetValueFromListLevel();
				}
			}
			return base.DefaultValue;
		}

		TValue GetValueFromDocumentElement()
		{
			DocumentElementBase ownerDocumentElement = base.PropertyContainer.OwnerDocumentElement;
			TValue elementValue = this.GetElementValue(ownerDocumentElement);
			if (elementValue != null)
			{
				return elementValue;
			}
			TValue valueFromDefaultStyle = this.GetValueFromDefaultStyle(ownerDocumentElement.Document, null);
			if (valueFromDefaultStyle != null)
			{
				return valueFromDefaultStyle;
			}
			return base.DefaultValue;
		}

		TValue GetValueFromStyle()
		{
			string properLinkedStyleId = this.GetProperLinkedStyleId();
			if (!string.IsNullOrEmpty(properLinkedStyleId))
			{
				Style style = base.PropertyContainer.OwnerStyle.Document.StyleRepository.GetStyle(properLinkedStyleId);
				if (style != null)
				{
					object propertyValue = style.GetPropertyValue(base.PropertyDefinition);
					if (propertyValue != null)
					{
						return (TValue)((object)propertyValue);
					}
				}
			}
			if (!string.IsNullOrEmpty(base.PropertyContainer.OwnerStyle.BasedOnStyleId))
			{
				Style style2 = base.PropertyContainer.OwnerStyle.Document.StyleRepository.GetStyle(base.PropertyContainer.OwnerStyle.BasedOnStyleId);
				if (style2 != null)
				{
					object propertyValue2 = style2.GetPropertyValue(base.PropertyDefinition);
					if (propertyValue2 != null)
					{
						return (TValue)((object)propertyValue2);
					}
				}
			}
			if (base.PropertyContainer.OwnerStyle.StyleType == StyleType.Character && !string.IsNullOrEmpty(base.PropertyContainer.OwnerStyle.LinkedStyleId))
			{
				Style style3 = base.PropertyContainer.OwnerStyle.Document.StyleRepository.GetStyle(base.PropertyContainer.OwnerStyle.LinkedStyleId);
				if (style3 != null && style3.StyleType == StyleType.Paragraph && !string.IsNullOrEmpty(style3.BasedOnStyleId))
				{
					Style style4 = base.PropertyContainer.OwnerStyle.Document.StyleRepository.GetStyle(style3.BasedOnStyleId);
					if (style4 != null)
					{
						object propertyValue3 = style4.GetPropertyValue(base.PropertyDefinition);
						if (propertyValue3 != null)
						{
							return (TValue)((object)propertyValue3);
						}
					}
				}
			}
			if (base.PropertyContainer.OwnerStyle.StyleType == StyleType.Table && !base.PropertyContainer.OwnerStyle.IsDefault)
			{
				Style defaultStyle = base.PropertyContainer.OwnerStyle.Document.StyleRepository.GetDefaultStyle(base.PropertyContainer.OwnerStyle.StyleType);
				if (defaultStyle != null)
				{
					object propertyValue4 = defaultStyle.GetPropertyValue(base.PropertyDefinition);
					if (propertyValue4 != null)
					{
						return (TValue)((object)propertyValue4);
					}
				}
			}
			else
			{
				TValue documentDefaultValue = this.GetDocumentDefaultValue(base.PropertyContainer.OwnerStyle.Document);
				if (documentDefaultValue != null)
				{
					return documentDefaultValue;
				}
			}
			return default(TValue);
		}

		string GetProperLinkedStyleId()
		{
			if (base.PropertyContainer.OwnerStyle == null)
			{
				return null;
			}
			if (string.IsNullOrEmpty(base.PropertyContainer.OwnerStyle.LinkedStyleId))
			{
				return null;
			}
			if (base.PropertyContainer is CharacterProperties)
			{
				if (base.PropertyContainer.OwnerStyle.StyleType == StyleType.Character)
				{
					return null;
				}
				return base.PropertyContainer.OwnerStyle.LinkedStyleId;
			}
			else
			{
				if (!(base.PropertyContainer is ParagraphProperties))
				{
					return null;
				}
				if (base.PropertyContainer.OwnerStyle.StyleType == StyleType.Paragraph)
				{
					return null;
				}
				return base.PropertyContainer.OwnerStyle.LinkedStyleId;
			}
		}

		TValue GetElementValue(DocumentElementBase element)
		{
			TValue tvalue = default(TValue);
			if (element == null)
			{
				return tvalue;
			}
			RadFlowDocument radFlowDocument = element as RadFlowDocument;
			if (radFlowDocument != null)
			{
				return this.GetValueFromDefaultStyle(radFlowDocument, null);
			}
			IElementWithStyle elementWithStyle = element as IElementWithStyle;
			if (elementWithStyle != null)
			{
				if (string.IsNullOrEmpty(elementWithStyle.StyleId) && element.Type == DocumentElementType.Paragraph)
				{
					Style defaultStyle = element.Document.StyleRepository.GetDefaultStyle(StyleType.Paragraph);
					if (defaultStyle != null)
					{
						IStyleProperty styleProperty = defaultStyle.GetStyleProperty(base.PropertyDefinition);
						if (styleProperty != null && styleProperty.HasLocalValue)
						{
							object localValueAsObject = styleProperty.GetLocalValueAsObject();
							if (localValueAsObject != null)
							{
								return (TValue)((object)localValueAsObject);
							}
						}
					}
				}
				else if (!string.IsNullOrEmpty(elementWithStyle.StyleId))
				{
					Style style = element.Document.StyleRepository.GetStyle(elementWithStyle.StyleId);
					if (style != null)
					{
						object propertyValue = style.GetPropertyValue(base.PropertyDefinition);
						if (propertyValue != null)
						{
							return (TValue)((object)propertyValue);
						}
					}
				}
			}
			if (element.Type == DocumentElementType.Paragraph && base.PropertyDefinition != Paragraph.ListIdPropertyDefinition && base.PropertyDefinition != Paragraph.ListLevelPropertyDefinition)
			{
				Paragraph paragraph = (Paragraph)element;
				if (paragraph.ListId > Paragraph.ListIdPropertyDefinition.DefaultValue && paragraph.ListLevel > Paragraph.ListLevelPropertyDefinition.DefaultValue)
				{
					List list = paragraph.Document.Lists.GetList(paragraph.ListId);
					if (list != null && !string.IsNullOrEmpty(list.Levels[paragraph.ListLevel].StyleId))
					{
						Style style2 = element.Document.StyleRepository.GetStyle(list.Levels[paragraph.ListLevel].StyleId);
						if (style2 != null)
						{
							object propertyValue2 = style2.GetPropertyValue(base.PropertyDefinition);
							if (propertyValue2 != null)
							{
								return (TValue)((object)propertyValue2);
							}
						}
					}
				}
			}
			if (tvalue == null)
			{
				DocumentElementBase element2;
				if (element is Table)
				{
					element2 = element.Document;
				}
				else
				{
					element2 = element.Parent;
				}
				tvalue = this.GetElementValue(element2);
			}
			return tvalue;
		}

		TValue GetValueFromDefaultStyle(RadFlowDocument document, StyleType? assignableType = null)
		{
			TValue result = default(TValue);
			if (assignableType == null)
			{
				if (base.PropertyContainer.OwnerDocumentElement is Paragraph || base.PropertyContainer.OwnerDocumentElement is Run)
				{
					assignableType = new StyleType?(StyleType.Paragraph);
				}
				else if (base.PropertyContainer.OwnerDocumentElement is Table)
				{
					assignableType = new StyleType?(StyleType.Table);
				}
			}
			if (assignableType != null)
			{
				Style defaultStyle = document.StyleRepository.GetDefaultStyle(assignableType.Value);
				if (defaultStyle != null)
				{
					object propertyValue = defaultStyle.GetPropertyValue(base.PropertyDefinition);
					if (propertyValue != null)
					{
						result = (TValue)((object)propertyValue);
					}
				}
			}
			return result;
		}

		TValue GetValueFromListLevel()
		{
			if (!string.IsNullOrEmpty(base.PropertyContainer.OwnerListLevel.StyleId))
			{
				Style style = base.PropertyContainer.OwnerListLevel.Document.StyleRepository.GetStyle(base.PropertyContainer.OwnerListLevel.StyleId);
				if (style != null && style.StyleType == StyleType.Paragraph)
				{
					object propertyValue = style.GetPropertyValue(base.PropertyDefinition);
					if (propertyValue != null)
					{
						return (TValue)((object)propertyValue);
					}
				}
			}
			TValue valueFromDefaultStyle = this.GetValueFromDefaultStyle(base.PropertyContainer.OwnerListLevel.Document, new StyleType?(StyleType.Paragraph));
			if (valueFromDefaultStyle != null)
			{
				return valueFromDefaultStyle;
			}
			return base.DefaultValue;
		}
	}
}
