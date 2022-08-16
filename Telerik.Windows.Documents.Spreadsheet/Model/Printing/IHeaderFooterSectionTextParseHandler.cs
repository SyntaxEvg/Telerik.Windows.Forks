using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Printing
{
	interface IHeaderFooterSectionTextParseHandler
	{
		void OnColorParse(string colorValue);

		void OnFontPropertiesParse(string fontPropertiesValue);

		void OnBoldParse();

		void OnItalicParse();

		void OnUnderlineParse();

		void OnDoubleUnderlineParse();

		void OnStrikeThroughParse();

		void OnSubscriptParse();

		void OnSuperscriptParse();

		void OnFontSizeParse(int fontSize);

		void OnPageNumberParse();

		void OnAddToPageNumberParse();

		void OnSubstractFromPageNumberParse();

		void OnNumberOfPagesParse();

		void OnDateParse();

		void OnTimeParse();

		void OnFilePathParse();

		void OnFileNameParse();

		void OnSheetNameParse();

		void OnPictureParse();

		void OnAmpersandParse();

		void OnTextFragmentParse(string text);

		void OnOutlineStyleParse();

		void OnShadowStyleParse();
	}
}
