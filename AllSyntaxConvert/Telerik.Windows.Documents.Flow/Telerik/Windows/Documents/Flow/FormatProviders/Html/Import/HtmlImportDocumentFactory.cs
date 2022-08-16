using System;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Import
{
	static class HtmlImportDocumentFactory
	{
		public static RadFlowDocument CreateDocumentForImport()
		{
			RadFlowDocument radFlowDocument = new RadFlowDocument();
			HtmlImportDocumentFactory.ModifyBuiltInStyles(radFlowDocument);
			return radFlowDocument;
		}

		static void ModifyBuiltInStyles(RadFlowDocument document)
		{
			document.DefaultStyle.CharacterProperties.FontFamily.LocalValue = HtmlImportDocumentFactory.defaultFontFamily;
			document.DefaultStyle.CharacterProperties.FontSize.ClearValue();
			document.DefaultStyle.ParagraphProperties.SpacingAfter.ClearValue();
			document.DefaultStyle.ParagraphProperties.LineSpacing.ClearValue();
			document.DefaultStyle.ParagraphProperties.LineSpacingType.ClearValue();
			document.StyleRepository.GetStyle("Normal").CharacterProperties.FontSize.LocalValue = new double?(16.0);
		}

		const int NormalStyleFontSize = 16;

		static readonly ThemableFontFamily defaultFontFamily = new ThemableFontFamily("Times New Roman");
	}
}
