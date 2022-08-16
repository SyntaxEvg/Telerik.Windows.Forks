using System;
using System.Linq;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Import
{
	class StylesImporter
	{
		StylesImporter(IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			this.context = context;
			this.document = context.Document;
		}

		public static void Import(IHtmlImportContext context)
		{
			StylesImporter stylesImporter = new StylesImporter(context);
			stylesImporter.CreateTemplateStyles();
			stylesImporter.Import();
		}

		void Import()
		{
			foreach (Selector selector in this.context.HtmlStyleRepository.GetStyles())
			{
				if (this.ShouldImportParagraphStyle(selector))
				{
					this.ImportParagraphStyle(selector);
				}
				if (this.ShouldImportCharacterStyle(selector))
				{
					this.ImportCharacterStyle(selector);
				}
			}
			if (this.context.ShouldImportNormalWeb)
			{
				this.document.StyleRepository.Add(this.normalWebStyle);
			}
		}

		void CreateTemplateStyles()
		{
			this.normalWebStyle = this.CreateNormalWebStyle();
			this.defaultCharacterStyle = this.CreateDefaultCharacterStyle();
		}

		void ImportCharacterStyle(Selector selector)
		{
			Style characterStyle = this.CreateCharacterStyle(selector, this.defaultCharacterStyle);
			this.document.StyleRepository.Add(characterStyle);
			if (selector.Type == SelectorType.Style && characterStyle.Id != selector.Name)
			{
				(from run in this.document.EnumerateChildrenOfType<Run>()
					where run.StyleId == selector.Name
					select run).ForEach(delegate(Run run)
				{
					run.StyleId = characterStyle.Id;
				});
			}
		}

		void ImportParagraphStyle(Selector selector)
		{
			Style paragraphStyle = this.CreateParagraphStyle(selector, this.normalWebStyle);
			this.document.StyleRepository.Add(paragraphStyle);
			if (selector.Type == SelectorType.Style && paragraphStyle.Id != selector.Name)
			{
				(from p in this.document.EnumerateChildrenOfType<Paragraph>()
					where p.StyleId == selector.Name
					select p).ForEach(delegate(Paragraph p)
				{
					p.StyleId = paragraphStyle.Id;
				});
			}
		}

		bool ShouldImportCharacterStyle(Selector selector)
		{
			return this.context.StyleNamesInfo.CharacterStyles.Contains(selector.Name);
		}

		bool ShouldImportParagraphStyle(Selector selector)
		{
			if (StyleNamesConverter.IsTelerikExportedDefaultBuiltInStyle(selector.Name))
			{
				return selector.HasProperties;
			}
			return this.context.StyleNamesInfo.ParagraphStyles.Contains(selector.Name) || selector.HasProperties;
		}

		Style CreateDefaultCharacterStyle()
		{
			Style style = new Style("temporaryStyle", StyleType.Character);
			this.context.HtmlStyleRepository.DefaultCharacterStyle.CopyTo(this.context, style.CharacterProperties);
			return style;
		}

		Style CreateNormalWebStyle()
		{
			Style style = BuiltInStyles.GetStyle("NormalWeb");
			style.CharacterProperties.FontFamily.ClearValue();
			style.CharacterProperties.FontSize.ClearValue();
			style.ParagraphProperties.SpacingBefore.LocalValue = new double?(6.66);
			style.ParagraphProperties.AutomaticSpacingBefore.LocalValue = new bool?(true);
			style.ParagraphProperties.SpacingAfter.LocalValue = new double?(6.66);
			style.ParagraphProperties.AutomaticSpacingAfter.LocalValue = new bool?(true);
			double? localValue = style.ParagraphProperties.SpacingAfter.LocalValue;
			double? localValue2 = style.ParagraphProperties.SpacingBefore.LocalValue;
			style.ParagraphProperties.SpacingAfter.ClearValue();
			style.ParagraphProperties.SpacingBefore.ClearValue();
			this.context.HtmlStyleRepository.DefaultParagraphStyle.CopyTo(this.context, style.ParagraphProperties);
			this.context.HtmlStyleRepository.DefaultParagraphStyle.CopyTo(this.context, style.CharacterProperties);
			if (style.ParagraphProperties.SpacingAfter.HasLocalValue)
			{
				style.ParagraphProperties.AutomaticSpacingAfter.LocalValue = new bool?(false);
			}
			else
			{
				style.ParagraphProperties.SpacingAfter.LocalValue = localValue;
			}
			if (style.ParagraphProperties.SpacingBefore.HasLocalValue)
			{
				style.ParagraphProperties.AutomaticSpacingBefore.LocalValue = new bool?(false);
			}
			else
			{
				style.ParagraphProperties.SpacingBefore.LocalValue = localValue2;
			}
			return style;
		}

		Style CreateParagraphStyle(Selector selector, Style templateStyle)
		{
			Style style = this.CreateStyle(selector.Name, StyleType.Paragraph, templateStyle);
			double? localValue = style.ParagraphProperties.SpacingAfter.LocalValue;
			double? localValue2 = style.ParagraphProperties.SpacingAfter.LocalValue;
			style.ParagraphProperties.SpacingAfter.ClearValue();
			style.ParagraphProperties.SpacingBefore.ClearValue();
			selector.CopyTo(this.context, style);
			if (style.ParagraphProperties.SpacingAfter.HasLocalValue)
			{
				style.ParagraphProperties.AutomaticSpacingAfter.LocalValue = new bool?(false);
			}
			else
			{
				style.ParagraphProperties.SpacingAfter.LocalValue = localValue;
			}
			if (style.ParagraphProperties.SpacingBefore.HasLocalValue)
			{
				style.ParagraphProperties.AutomaticSpacingBefore.LocalValue = new bool?(false);
			}
			else
			{
				style.ParagraphProperties.SpacingBefore.LocalValue = localValue2;
			}
			return style;
		}

		Style CreateCharacterStyle(Selector selector, Style templateStyle)
		{
			Style style = this.CreateStyle(selector.Name, StyleType.Character, templateStyle);
			selector.CopyTo(this.context, style);
			return style;
		}

		Style CreateStyle(string suggestedStyleId, StyleType styleType, Style templateStyle)
		{
			string text = this.ComputeStyleId(suggestedStyleId);
			Style style;
			if (BuiltInStyles.IsBuiltInStyle(text))
			{
				style = BuiltInStyles.GetStyle(text);
			}
			else
			{
				style = new Style(text, styleType);
				style.CopyStylePropertiesFrom(templateStyle);
				style.BasedOnStyleId = templateStyle.BasedOnStyleId;
			}
			return style;
		}

		string ComputeStyleId(string styleId)
		{
			string text;
			if (StyleNamesConverter.TryGetTelerikExportedBuiltInStyle(styleId, out text))
			{
				return text;
			}
			text = styleId;
			while (this.document.StyleRepository.Contains(text))
			{
				text += "1";
			}
			return text;
		}

		const string ConflictingStyleIdSuffix = "1";

		readonly RadFlowDocument document;

		readonly IHtmlImportContext context;

		Style normalWebStyle;

		Style defaultCharacterStyle;
	}
}
