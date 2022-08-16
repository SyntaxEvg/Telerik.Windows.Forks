using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.RunPropertiesElements;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class RunPropertiesElement : DocumentElementBase
	{
		public RunPropertiesElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.styleIdElement = base.RegisterChildElement<StyleIdElement>("rStyle");
			this.fontChildElement = base.RegisterChildElement<FontElement>("rFonts");
			this.boldChildElement = base.RegisterChildElement<BoldElement>("b");
			this.italicChildElement = base.RegisterChildElement<ItalicElement>("i");
			this.strikethroughChildElement = base.RegisterChildElement<StrikethroughElement>("strike");
			this.foregroundColorChildElement = base.RegisterChildElement<ForegroundColorElement>("color");
			this.fontSizeChildElement = base.RegisterChildElement<FontSizeElement>("sz");
			this.highlightColorChildElement = base.RegisterChildElement<HighlightElement>("highlight");
			this.underlineElementChildElement = base.RegisterChildElement<UnderlineElement>("u");
			this.shadingChildElement = base.RegisterChildElement<ShadingElement>("shd");
			this.baselineAlignmentChildElement = base.RegisterChildElement<BaselineAlignmentElement>("vertAlign");
			this.flowDirectionElement = base.RegisterChildElement<FlowDirectionElement>("rtl");
		}

		public override string ElementName
		{
			get
			{
				return "rPr";
			}
		}

		public CharacterProperties CharacterProperties
		{
			get
			{
				return this.characterProperties;
			}
		}

		public void SetAssociatedFlowModelElement(CharacterProperties characterProperties)
		{
			Guard.ThrowExceptionIfNull<CharacterProperties>(characterProperties, "characterProperties");
			this.characterProperties = characterProperties;
		}

		public void CopyPropertiesTo(CharacterProperties characterProperties)
		{
			Guard.ThrowExceptionIfNull<CharacterProperties>(characterProperties, "characterProperties");
			if (this.boldChildElement.Element != null)
			{
				characterProperties.FontWeight.LocalValue = new FontWeight?(this.boldChildElement.Element.Value ? FontWeights.Bold : FontWeights.Normal);
				base.ReleaseElement(this.boldChildElement);
			}
			if (this.italicChildElement.Element != null)
			{
				characterProperties.FontStyle.LocalValue = new FontStyle?(this.italicChildElement.Element.Value ? FontStyles.Italic : FontStyles.Normal);
				base.ReleaseElement(this.italicChildElement);
			}
			if (this.strikethroughChildElement.Element != null)
			{
				characterProperties.Strikethrough.LocalValue = new bool?(this.strikethroughChildElement.Element.Value);
				base.ReleaseElement(this.strikethroughChildElement);
			}
			if (this.baselineAlignmentChildElement.Element != null)
			{
				characterProperties.BaselineAlignment.LocalValue = new Telerik.Windows.Documents.Flow.Model.Styles.BaselineAlignment?(this.baselineAlignmentChildElement.Element.Value);
				base.ReleaseElement(this.baselineAlignmentChildElement);
			}
			if (this.shadingChildElement.Element != null)
			{
				this.shadingChildElement.Element.ReadAttributes(characterProperties);
				base.ReleaseElement(this.shadingChildElement);
			}
			if (this.fontSizeChildElement.Element != null)
			{
				double value = Unit.PointToDip(this.fontSizeChildElement.Element.Value / 2.0);
				characterProperties.FontSize.LocalValue = new double?(value);
				base.ReleaseElement(this.fontSizeChildElement);
			}
			if (this.fontChildElement.Element != null)
			{
				ThemableFontFamily fontFamily = this.fontChildElement.Element.GetFontFamily();
				if (fontFamily != null)
				{
					characterProperties.FontFamily.LocalValue = fontFamily;
				}
				base.ReleaseElement(this.fontChildElement);
			}
			if (this.foregroundColorChildElement.Element != null)
			{
				ThemableColor themableColor = this.foregroundColorChildElement.Element.GetThemableColor();
				if (themableColor != null)
				{
					characterProperties.ForegroundColor.LocalValue = themableColor;
				}
				base.ReleaseElement(this.foregroundColorChildElement);
			}
			if (this.highlightColorChildElement.Element != null)
			{
				characterProperties.HighlightColor.LocalValue = new Color?(this.highlightColorChildElement.Element.Value);
				base.ReleaseElement(this.highlightColorChildElement);
			}
			if (this.underlineElementChildElement.Element != null)
			{
				this.underlineElementChildElement.Element.ReadAttributes(characterProperties);
				base.ReleaseElement(this.underlineElementChildElement);
			}
			if (this.flowDirectionElement.Element != null)
			{
				characterProperties.FlowDirection.LocalValue = new Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection?(this.flowDirectionElement.Element.Value ? Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection.RightToLeft : Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection.LeftToRight);
				base.ReleaseElement(this.flowDirectionElement);
			}
			if (this.styleIdElement.Element != null)
			{
				characterProperties.StyleId = this.styleIdElement.Element.Value;
				base.ReleaseElement(this.styleIdElement);
			}
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			DocumentTheme theme = context.Document.Theme;
			if (this.CharacterProperties.FontWeight.HasLocalValue)
			{
				base.CreateElement(this.boldChildElement);
				FontWeight value = this.CharacterProperties.FontWeight.LocalValue.Value;
				this.boldChildElement.Element.Value = value == FontWeights.Bold;
			}
			if (this.CharacterProperties.FontStyle.HasLocalValue)
			{
				base.CreateElement(this.italicChildElement);
				FontStyle value2 = this.CharacterProperties.FontStyle.LocalValue.Value;
				this.italicChildElement.Element.Value = value2 == FontStyles.Italic;
			}
			if (this.CharacterProperties.Strikethrough.HasLocalValue)
			{
				base.CreateElement(this.strikethroughChildElement);
				this.strikethroughChildElement.Element.Value = this.CharacterProperties.Strikethrough.LocalValue.Value;
			}
			if (this.CharacterProperties.BaselineAlignment.HasLocalValue)
			{
				base.CreateElement(this.baselineAlignmentChildElement);
				this.baselineAlignmentChildElement.Element.Value = this.CharacterProperties.BaselineAlignment.LocalValue.Value;
			}
			if (this.CharacterProperties.ShadingPattern.HasLocalValue || this.CharacterProperties.ShadingPatternColor.HasLocalValue || this.CharacterProperties.BackgroundColor.HasLocalValue)
			{
				base.CreateElement(this.shadingChildElement);
				this.shadingChildElement.Element.FillAttributes(this.CharacterProperties);
			}
			if (this.CharacterProperties.FontSize.HasLocalValue)
			{
				base.CreateElement(this.fontSizeChildElement);
				double value3 = this.CharacterProperties.FontSize.LocalValue.Value;
				this.fontSizeChildElement.Element.Value = (double)((int)Math.Round(2.0 * Unit.DipToPoint(value3)));
			}
			if (this.CharacterProperties.FontFamily.HasLocalValue)
			{
				base.CreateElement(this.fontChildElement);
				ThemableFontFamily localValue = this.CharacterProperties.FontFamily.LocalValue;
				this.fontChildElement.Element.FillAttributes(localValue, theme);
			}
			if (this.CharacterProperties.ForegroundColor.HasLocalValue)
			{
				base.CreateElement(this.foregroundColorChildElement);
				ThemableColor localValue2 = this.CharacterProperties.ForegroundColor.LocalValue;
				this.foregroundColorChildElement.Element.FillAttributes(localValue2, theme);
			}
			if (this.CharacterProperties.HighlightColor.HasLocalValue)
			{
				base.CreateElement(this.highlightColorChildElement);
				Color value4 = this.characterProperties.HighlightColor.LocalValue.Value;
				this.highlightColorChildElement.Element.Value = value4;
			}
			if (this.CharacterProperties.UnderlineColor.HasLocalValue || this.CharacterProperties.UnderlinePattern.HasLocalValue)
			{
				base.CreateElement(this.underlineElementChildElement);
				this.underlineElementChildElement.Element.FillAttributes(this.characterProperties, theme);
			}
			if (this.CharacterProperties.FlowDirection.HasLocalValue)
			{
				base.CreateElement(this.flowDirectionElement);
				Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection value5 = this.CharacterProperties.FlowDirection.LocalValue.Value;
				this.flowDirectionElement.Element.Value = value5 == Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection.RightToLeft;
			}
			if (!string.IsNullOrEmpty(this.CharacterProperties.StyleId))
			{
				base.CreateElement(this.styleIdElement);
				this.styleIdElement.Element.Value = this.CharacterProperties.StyleId;
			}
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			if (this.characterProperties != null)
			{
				this.CopyPropertiesTo(this.characterProperties);
			}
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return this.characterProperties != null || base.ShouldExport(context);
		}

		protected override void ClearOverride()
		{
			this.characterProperties = null;
		}

		readonly OpenXmlChildElement<BoldElement> boldChildElement;

		readonly OpenXmlChildElement<ItalicElement> italicChildElement;

		readonly OpenXmlChildElement<StrikethroughElement> strikethroughChildElement;

		readonly OpenXmlChildElement<BaselineAlignmentElement> baselineAlignmentChildElement;

		readonly OpenXmlChildElement<ShadingElement> shadingChildElement;

		readonly OpenXmlChildElement<FontSizeElement> fontSizeChildElement;

		readonly OpenXmlChildElement<FontElement> fontChildElement;

		readonly OpenXmlChildElement<ForegroundColorElement> foregroundColorChildElement;

		readonly OpenXmlChildElement<HighlightElement> highlightColorChildElement;

		readonly OpenXmlChildElement<UnderlineElement> underlineElementChildElement;

		readonly OpenXmlChildElement<FlowDirectionElement> flowDirectionElement;

		readonly OpenXmlChildElement<StyleIdElement> styleIdElement;

		CharacterProperties characterProperties;
	}
}
