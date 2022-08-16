using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public sealed class CharacterProperties : DocumentElementPropertiesBase, IPropertiesWithShading
	{
		internal CharacterProperties(Style style)
			: base(style)
		{
			this.InitProperties();
		}

		internal CharacterProperties(Run run)
			: base(run, false)
		{
			this.InitProperties();
		}

		internal CharacterProperties(DocumentElementBase ownerElement, bool suppressStylePropertyEvaluation)
			: base(ownerElement, suppressStylePropertyEvaluation)
		{
			this.InitProperties();
		}

		internal CharacterProperties(ListLevel listLevel, bool suppressStylePropertyEvaluation)
			: base(listLevel, suppressStylePropertyEvaluation)
		{
			this.InitProperties();
		}

		public IStyleProperty<ThemableFontFamily> FontFamily
		{
			get
			{
				return this.fontFamilyStyleProperty;
			}
		}

		public IStyleProperty<double?> FontSize
		{
			get
			{
				return this.fontSizeStyleProperty;
			}
		}

		public IStyleProperty<FontStyle?> FontStyle
		{
			get
			{
				return this.fontStyleStyleProperty;
			}
		}

		public IStyleProperty<FontWeight?> FontWeight
		{
			get
			{
				return this.fontWeightStyleProperty;
			}
		}

		public IStyleProperty<ThemableColor> ForegroundColor
		{
			get
			{
				return this.foregroundColorStyleProperty;
			}
		}

		public IStyleProperty<Color?> HighlightColor
		{
			get
			{
				return this.highlightColorStyleProperty;
			}
		}

		public IStyleProperty<BaselineAlignment?> BaselineAlignment
		{
			get
			{
				return this.baselineAlignmentStyleProperty;
			}
		}

		public IStyleProperty<bool?> Strikethrough
		{
			get
			{
				return this.strikethroughStyleProperty;
			}
		}

		public IStyleProperty<ThemableColor> BackgroundColor
		{
			get
			{
				return this.backgroundColorStyleProperty;
			}
		}

		public IStyleProperty<ThemableColor> ShadingPatternColor
		{
			get
			{
				return this.shadingPatternColorStyleProperty;
			}
		}

		public IStyleProperty<ShadingPattern?> ShadingPattern
		{
			get
			{
				return this.shadingPatternStyleProperty;
			}
		}

		public IStyleProperty<ThemableColor> UnderlineColor
		{
			get
			{
				return this.underlineColorStyleProperty;
			}
		}

		public IStyleProperty<UnderlinePattern?> UnderlinePattern
		{
			get
			{
				return this.underlinePatternStyleProperty;
			}
		}

		public IStyleProperty<FlowDirection?> FlowDirection
		{
			get
			{
				return this.flowDirectionStyleProperty;
			}
		}

		RadFlowDocument IPropertiesWithShading.Document
		{
			get
			{
				return base.Document;
			}
		}

		protected override IEnumerable<IStyleProperty> EnumerateStyleProperties()
		{
			yield return this.FontFamily;
			yield return this.FontSize;
			yield return this.FontStyle;
			yield return this.FontWeight;
			yield return this.ForegroundColor;
			yield return this.HighlightColor;
			yield return this.BaselineAlignment;
			yield return this.Strikethrough;
			yield return this.BackgroundColor;
			yield return this.ShadingPatternColor;
			yield return this.ShadingPattern;
			yield return this.UnderlineColor;
			yield return this.UnderlinePattern;
			yield return this.FlowDirection;
			yield break;
		}

		protected override IStyleProperty GetStylePropertyOverride(IStylePropertyDefinition propertyDefinition)
		{
			if (propertyDefinition != DocumentElementPropertiesBase.StyleIdPropertyDefinition && propertyDefinition.StylePropertyType != StylePropertyType.Character)
			{
				return null;
			}
			return CharacterProperties.stylePropertyGetters[propertyDefinition.GlobalPropertyIndex](this);
		}

		static void InitStylePropertyGetters()
		{
			if (CharacterProperties.stylePropertyGetters != null)
			{
				return;
			}
			CharacterProperties.stylePropertyGetters = new Func<CharacterProperties, IStyleProperty>[15];
			CharacterProperties.stylePropertyGetters[DocumentElementPropertiesBase.StyleIdPropertyDefinition.GlobalPropertyIndex] = (CharacterProperties prop) => prop.StyleIdProperty;
			CharacterProperties.stylePropertyGetters[Run.FontSizePropertyDefinition.GlobalPropertyIndex] = (CharacterProperties prop) => prop.fontSizeStyleProperty;
			CharacterProperties.stylePropertyGetters[Run.FontFamilyPropertyDefinition.GlobalPropertyIndex] = (CharacterProperties prop) => prop.fontFamilyStyleProperty;
			CharacterProperties.stylePropertyGetters[Run.FontStylePropertyDefinition.GlobalPropertyIndex] = (CharacterProperties prop) => prop.fontStyleStyleProperty;
			CharacterProperties.stylePropertyGetters[Run.FontWeightPropertyDefinition.GlobalPropertyIndex] = (CharacterProperties prop) => prop.fontWeightStyleProperty;
			CharacterProperties.stylePropertyGetters[Run.ForegroundColorPropertyDefinition.GlobalPropertyIndex] = (CharacterProperties prop) => prop.foregroundColorStyleProperty;
			CharacterProperties.stylePropertyGetters[Run.HighlightColorPropertyDefinition.GlobalPropertyIndex] = (CharacterProperties prop) => prop.highlightColorStyleProperty;
			CharacterProperties.stylePropertyGetters[Run.BaselineAlignmentPropertyDefinition.GlobalPropertyIndex] = (CharacterProperties prop) => prop.baselineAlignmentStyleProperty;
			CharacterProperties.stylePropertyGetters[Run.StrikethroughPropertyDefinition.GlobalPropertyIndex] = (CharacterProperties prop) => prop.strikethroughStyleProperty;
			CharacterProperties.stylePropertyGetters[Run.BackgroundColorPropertyDefinition.GlobalPropertyIndex] = (CharacterProperties prop) => prop.backgroundColorStyleProperty;
			CharacterProperties.stylePropertyGetters[Run.ShadingPatternColorPropertyDefinition.GlobalPropertyIndex] = (CharacterProperties prop) => prop.shadingPatternColorStyleProperty;
			CharacterProperties.stylePropertyGetters[Run.ShadingPatternPropertyDefinition.GlobalPropertyIndex] = (CharacterProperties prop) => prop.shadingPatternStyleProperty;
			CharacterProperties.stylePropertyGetters[Run.UnderlineColorPropertyDefinition.GlobalPropertyIndex] = (CharacterProperties prop) => prop.underlineColorStyleProperty;
			CharacterProperties.stylePropertyGetters[Run.UnderlinePatternPropertyDefinition.GlobalPropertyIndex] = (CharacterProperties prop) => prop.underlinePatternStyleProperty;
			CharacterProperties.stylePropertyGetters[Run.FlowDirectionPropertyDefinition.GlobalPropertyIndex] = (CharacterProperties prop) => prop.flowDirectionStyleProperty;
		}

		void InitProperties()
		{
			this.fontSizeStyleProperty = new StyleProperty<double?>(Run.FontSizePropertyDefinition, this);
			this.fontFamilyStyleProperty = new StyleProperty<ThemableFontFamily>(Run.FontFamilyPropertyDefinition, this);
			this.fontStyleStyleProperty = new StyleProperty<FontStyle?>(Run.FontStylePropertyDefinition, this);
			this.fontWeightStyleProperty = new StyleProperty<FontWeight?>(Run.FontWeightPropertyDefinition, this);
			this.foregroundColorStyleProperty = new StyleProperty<ThemableColor>(Run.ForegroundColorPropertyDefinition, this);
			this.highlightColorStyleProperty = new StyleProperty<Color?>(Run.HighlightColorPropertyDefinition, this);
			this.baselineAlignmentStyleProperty = new StyleProperty<BaselineAlignment?>(Run.BaselineAlignmentPropertyDefinition, this);
			this.strikethroughStyleProperty = new StyleProperty<bool?>(Run.StrikethroughPropertyDefinition, this);
			this.backgroundColorStyleProperty = new StyleProperty<ThemableColor>(Run.BackgroundColorPropertyDefinition, this);
			this.shadingPatternColorStyleProperty = new StyleProperty<ThemableColor>(Run.ShadingPatternColorPropertyDefinition, this);
			this.shadingPatternStyleProperty = new StyleProperty<ShadingPattern?>(Run.ShadingPatternPropertyDefinition, this);
			this.underlineColorStyleProperty = new StyleProperty<ThemableColor>(Run.UnderlineColorPropertyDefinition, this);
			this.underlinePatternStyleProperty = new StyleProperty<UnderlinePattern?>(Run.UnderlinePatternPropertyDefinition, this);
			this.flowDirectionStyleProperty = new StyleProperty<FlowDirection?>(Run.FlowDirectionPropertyDefinition, this);
			CharacterProperties.InitStylePropertyGetters();
		}

		static Func<CharacterProperties, IStyleProperty>[] stylePropertyGetters;

		StyleProperty<double?> fontSizeStyleProperty;

		StyleProperty<ThemableFontFamily> fontFamilyStyleProperty;

		StyleProperty<FontStyle?> fontStyleStyleProperty;

		StyleProperty<FontWeight?> fontWeightStyleProperty;

		StyleProperty<ThemableColor> foregroundColorStyleProperty;

		StyleProperty<Color?> highlightColorStyleProperty;

		StyleProperty<BaselineAlignment?> baselineAlignmentStyleProperty;

		StyleProperty<bool?> strikethroughStyleProperty;

		StyleProperty<ThemableColor> backgroundColorStyleProperty;

		StyleProperty<ThemableColor> shadingPatternColorStyleProperty;

		StyleProperty<ShadingPattern?> shadingPatternStyleProperty;

		StyleProperty<ThemableColor> underlineColorStyleProperty;

		StyleProperty<UnderlinePattern?> underlinePatternStyleProperty;

		StyleProperty<FlowDirection?> flowDirectionStyleProperty;
	}
}
