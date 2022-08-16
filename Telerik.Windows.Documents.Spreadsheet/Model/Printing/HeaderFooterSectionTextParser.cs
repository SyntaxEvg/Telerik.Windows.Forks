using System;
using System.Collections.Generic;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Printing
{
	class HeaderFooterSectionTextParser
	{
		public HeaderFooterSectionTextParser(IHeaderFooterSectionTextParseHandler parseHandler)
		{
			Guard.ThrowExceptionIfNull<IHeaderFooterSectionTextParseHandler>(parseHandler, "parseHandler");
			this.parseHandler = parseHandler;
			this.textFragmentBuilder = new StringBuilder();
			this.isParsing = false;
			this.fieldHandlers = new Dictionary<char, Action>();
			this.fieldHandlers.Add('K', new Action(this.OnParsingColor));
			this.fieldHandlers.Add('"', new Action(this.OnParsingFontProperties));
			this.fieldHandlers.Add('B', new Action(this.parseHandler.OnBoldParse));
			this.fieldHandlers.Add('I', new Action(this.parseHandler.OnItalicParse));
			this.fieldHandlers.Add('U', new Action(this.parseHandler.OnUnderlineParse));
			this.fieldHandlers.Add('E', new Action(this.parseHandler.OnDoubleUnderlineParse));
			this.fieldHandlers.Add('S', new Action(this.parseHandler.OnStrikeThroughParse));
			this.fieldHandlers.Add('Y', new Action(this.parseHandler.OnSubscriptParse));
			this.fieldHandlers.Add('X', new Action(this.parseHandler.OnSuperscriptParse));
			this.fieldHandlers.Add('P', new Action(this.parseHandler.OnPageNumberParse));
			this.fieldHandlers.Add('+', new Action(this.parseHandler.OnAddToPageNumberParse));
			this.fieldHandlers.Add('-', new Action(this.parseHandler.OnSubstractFromPageNumberParse));
			this.fieldHandlers.Add('N', new Action(this.parseHandler.OnNumberOfPagesParse));
			this.fieldHandlers.Add('D', new Action(this.parseHandler.OnDateParse));
			this.fieldHandlers.Add('T', new Action(this.parseHandler.OnTimeParse));
			this.fieldHandlers.Add('Z', new Action(this.parseHandler.OnFilePathParse));
			this.fieldHandlers.Add('F', new Action(this.parseHandler.OnFileNameParse));
			this.fieldHandlers.Add('A', new Action(this.parseHandler.OnSheetNameParse));
			this.fieldHandlers.Add('G', new Action(this.parseHandler.OnPictureParse));
			this.fieldHandlers.Add('&', new Action(this.parseHandler.OnAmpersandParse));
			this.fieldHandlers.Add('O', new Action(this.parseHandler.OnOutlineStyleParse));
			this.fieldHandlers.Add('H', new Action(this.parseHandler.OnShadowStyleParse));
		}

		public void Parse(string headerFooterSectionText)
		{
			this.OnBeforeParse(headerFooterSectionText);
			if (!string.IsNullOrEmpty(headerFooterSectionText))
			{
				this.ParseText();
			}
			this.OnAfterParse();
		}

		void OnBeforeParse(string headerFooterSectionText)
		{
			Guard.ThrowExceptionIfTrue(this.isParsing, "isParsing");
			Guard.ThrowExceptionIfGreaterThan<int>(0, this.textFragmentBuilder.Length, "textFragmentBuilder.Length");
			this.isParsing = true;
			this.currentIndex = 0;
			this.text = headerFooterSectionText;
		}

		void OnAfterParse()
		{
			Guard.ThrowExceptionIfFalse(this.isParsing, "isParsing");
			Guard.ThrowExceptionIfGreaterThan<int>(0, this.textFragmentBuilder.Length, "textFragmentBuilder.Length");
			this.text = null;
			this.currentIndex = 0;
			this.isParsing = false;
		}

		void ParseText()
		{
			while (this.currentIndex < this.text.Length)
			{
				char c = this.text[this.currentIndex];
				if (c == '&')
				{
					this.PopTextFragment();
					this.currentIndex++;
					this.OnParsingSpecialSymbol();
				}
				else
				{
					this.textFragmentBuilder.Append(c);
					this.currentIndex++;
				}
			}
			this.PopTextFragment();
		}

		void OnParsingSpecialSymbol()
		{
			if (this.currentIndex < this.text.Length)
			{
				char c = this.text[this.currentIndex];
				Action action;
				if (this.fieldHandlers.TryGetValue(c, out action))
				{
					action();
					this.currentIndex++;
					return;
				}
				if (char.IsDigit(c))
				{
					this.OnParsingFontSize();
				}
			}
		}

		void OnParsingColor()
		{
			this.currentIndex++;
			int num = this.currentIndex + 6;
			bool flag = num < this.text.Length;
			if (flag)
			{
				string colorValue = this.text.Substring(this.currentIndex, 6);
				this.parseHandler.OnColorParse(colorValue);
				this.currentIndex = num - 1;
				return;
			}
			this.currentIndex = this.text.Length - 1;
		}

		void OnParsingFontProperties()
		{
			this.currentIndex++;
			int num = this.text.IndexOf('"', this.currentIndex);
			bool flag = -1 < num && num < this.text.Length - 1;
			if (flag)
			{
				int length = num - this.currentIndex;
				string text = this.text.Substring(this.currentIndex, length);
				this.parseHandler.OnFontPropertiesParse(text.Trim());
				this.currentIndex = num;
				return;
			}
			this.currentIndex = this.text.Length - 1;
		}

		void OnParsingFontSize()
		{
			StringBuilder stringBuilder = new StringBuilder();
			char c = this.text[this.currentIndex];
			while (this.currentIndex < this.text.Length && char.IsDigit(c))
			{
				stringBuilder.Append(c);
				this.currentIndex++;
				if (this.currentIndex < this.text.Length)
				{
					c = this.text[this.currentIndex];
				}
			}
			bool flag = this.currentIndex < this.text.Length;
			if (flag)
			{
				int fontSize = int.Parse(stringBuilder.ToString());
				this.parseHandler.OnFontSizeParse(fontSize);
			}
		}

		void PopTextFragment()
		{
			if (this.textFragmentBuilder.Length > 0)
			{
				this.parseHandler.OnTextFragmentParse(this.textFragmentBuilder.ToString());
				this.textFragmentBuilder.Clear();
			}
		}

		public const int ColorTextLength = 6;

		readonly Dictionary<char, Action> fieldHandlers;

		readonly StringBuilder textFragmentBuilder;

		readonly IHeaderFooterSectionTextParseHandler parseHandler;

		bool isParsing;

		int currentIndex;

		string text;
	}
}
