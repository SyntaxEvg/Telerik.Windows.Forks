using System;
using System.Text;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Printing
{
	class HeaderFooterSectionTextSanitizer : IHeaderFooterSectionTextParseHandler
	{
		public HeaderFooterSectionTextSanitizer()
		{
			this.parser = new HeaderFooterSectionTextParser(this);
			this.resultText = new StringBuilder();
		}

		public string Sanitize(string sectionText)
		{
			this.parser.Parse(sectionText);
			string result = this.resultText.ToString();
			this.resultText.Clear();
			return result;
		}

		void IHeaderFooterSectionTextParseHandler.OnColorParse(string colorValue)
		{
			this.WriteField(string.Format("{0}{1}", 'K', colorValue));
		}

		void IHeaderFooterSectionTextParseHandler.OnFontPropertiesParse(string fontPropertiesValue)
		{
			this.WriteField(string.Format("{0}{1}{2}", '"', fontPropertiesValue, '"'));
		}

		void IHeaderFooterSectionTextParseHandler.OnBoldParse()
		{
			this.WriteField('B');
		}

		void IHeaderFooterSectionTextParseHandler.OnItalicParse()
		{
			this.WriteField('I');
		}

		void IHeaderFooterSectionTextParseHandler.OnUnderlineParse()
		{
			this.WriteField('U');
		}

		void IHeaderFooterSectionTextParseHandler.OnDoubleUnderlineParse()
		{
			this.WriteField('E');
		}

		void IHeaderFooterSectionTextParseHandler.OnStrikeThroughParse()
		{
			this.WriteField('S');
		}

		void IHeaderFooterSectionTextParseHandler.OnSubscriptParse()
		{
			this.WriteField('Y');
		}

		void IHeaderFooterSectionTextParseHandler.OnSuperscriptParse()
		{
			this.WriteField('X');
		}

		void IHeaderFooterSectionTextParseHandler.OnFontSizeParse(int fontSize)
		{
			string parameter = fontSize.ToString();
			this.WriteField(parameter);
		}

		void IHeaderFooterSectionTextParseHandler.OnPageNumberParse()
		{
			this.WriteField('P');
		}

		void IHeaderFooterSectionTextParseHandler.OnAddToPageNumberParse()
		{
			this.WriteField('+');
		}

		void IHeaderFooterSectionTextParseHandler.OnSubstractFromPageNumberParse()
		{
			this.WriteField('-');
		}

		void IHeaderFooterSectionTextParseHandler.OnNumberOfPagesParse()
		{
			this.WriteField('N');
		}

		void IHeaderFooterSectionTextParseHandler.OnDateParse()
		{
			this.WriteField('D');
		}

		void IHeaderFooterSectionTextParseHandler.OnTimeParse()
		{
			this.WriteField('T');
		}

		void IHeaderFooterSectionTextParseHandler.OnFilePathParse()
		{
			this.WriteField('Z');
		}

		void IHeaderFooterSectionTextParseHandler.OnFileNameParse()
		{
			this.WriteField('F');
		}

		void IHeaderFooterSectionTextParseHandler.OnSheetNameParse()
		{
			this.WriteField('A');
		}

		void IHeaderFooterSectionTextParseHandler.OnPictureParse()
		{
		}

		void IHeaderFooterSectionTextParseHandler.OnAmpersandParse()
		{
			this.WriteField('&');
		}

		void IHeaderFooterSectionTextParseHandler.OnOutlineStyleParse()
		{
			this.WriteField('O');
		}

		void IHeaderFooterSectionTextParseHandler.OnShadowStyleParse()
		{
			this.WriteField('H');
		}

		void IHeaderFooterSectionTextParseHandler.OnTextFragmentParse(string text)
		{
			this.resultText.Append(text);
		}

		void WriteField(char symbol)
		{
			this.resultText.AppendFormat("{0}{1}", '&', symbol);
		}

		void WriteField(string parameter)
		{
			this.resultText.AppendFormat("{0}{1}", '&', parameter);
		}

		readonly StringBuilder resultText;

		readonly HeaderFooterSectionTextParser parser;
	}
}
