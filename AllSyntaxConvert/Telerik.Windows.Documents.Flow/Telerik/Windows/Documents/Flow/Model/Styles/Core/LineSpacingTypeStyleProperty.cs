using System;

namespace Telerik.Windows.Documents.Flow.Model.Styles.Core
{
	class LineSpacingTypeStyleProperty : StyleProperty<HeightType?>
	{
		public LineSpacingTypeStyleProperty(StylePropertyDefinition<HeightType?> propertyDefinition, DocumentElementPropertiesBase propertyContainer)
			: base(propertyDefinition, propertyContainer)
		{
		}

		ParagraphProperties OwnerProperties
		{
			get
			{
				return (ParagraphProperties)base.PropertyContainer;
			}
		}

		StyleRepository StyleRepository
		{
			get
			{
				return base.PropertyContainer.Document.StyleRepository;
			}
		}

		public override HeightType? GetActualValue()
		{
			if (base.PropertyContainer.Document != null)
			{
				return this.EvaluateActualValue(this.OwnerProperties);
			}
			if (base.PropertyContainer.OwnerStyle != null)
			{
				return base.GetActualValue();
			}
			return base.DefaultValue;
		}

		static string GetStyleId(ParagraphProperties properties)
		{
			string result = properties.StyleId;
			if (properties.OwnerStyle != null)
			{
				result = properties.OwnerStyle.Id;
			}
			return result;
		}

		static HeightType GetValue(ParagraphProperties properties)
		{
			HeightType? heightType = properties.LineSpacingType.GetLocalValueAsObject() as HeightType?;
			if (heightType == null)
			{
				return HeightType.Auto;
			}
			return heightType.GetValueOrDefault();
		}

		HeightType? EvaluateActualValue(ParagraphProperties properties)
		{
			IStyleProperty<double?> lineSpacing = properties.LineSpacing;
			if (lineSpacing.HasLocalValue)
			{
				return new HeightType?(LineSpacingTypeStyleProperty.GetValue(properties));
			}
			string styleId = LineSpacingTypeStyleProperty.GetStyleId(properties);
			if (!string.IsNullOrEmpty(styleId))
			{
				Style style = this.StyleRepository.GetStyle(styleId);
				if (style != null)
				{
					if (style.ParagraphProperties.LineSpacing.HasLocalValue)
					{
						return new HeightType?(LineSpacingTypeStyleProperty.GetValue(style.ParagraphProperties));
					}
					string basedOnStyleId = style.BasedOnStyleId;
					if (!string.IsNullOrEmpty(basedOnStyleId))
					{
						Style style2 = this.StyleRepository.GetStyle(basedOnStyleId);
						if (style2 != null)
						{
							return this.EvaluateActualValue(style2.ParagraphProperties);
						}
					}
				}
			}
			else
			{
				Style defaultStyle = this.StyleRepository.GetDefaultStyle(StyleType.Paragraph);
				if (defaultStyle != null)
				{
					return this.EvaluateActualValue(defaultStyle.ParagraphProperties);
				}
			}
			if (((StyleProperty<double?>)this.OwnerProperties.LineSpacing).GetDocumentDefaultValue(base.PropertyContainer.Document) == null)
			{
				return base.DefaultValue;
			}
			HeightType? documentDefaultValue = base.GetDocumentDefaultValue(base.PropertyContainer.Document);
			if (documentDefaultValue == null)
			{
				return base.DefaultValue;
			}
			return new HeightType?(documentDefaultValue.GetValueOrDefault());
		}
	}
}
