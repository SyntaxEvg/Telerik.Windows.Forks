using System;
using System.Text;
using Telerik.Windows.Documents.Fixed.Model;

namespace Telerik.Windows.Documents.Fixed.Text
{
	class TextDocument
	{
		internal TextDocument(int count)
		{
			this.textPages = new TextPage[count];
		}

		public TextDocument(RadFixedDocument document)
			: this()
		{
			this.fixedDocument = document;
			this.PagesCount = this.fixedDocument.Pages.Count;
		}

		protected TextDocument()
		{
			this.textRecognizer = new TextRecognizer(this);
		}

		internal int PagesCount
		{
			get
			{
				if (this.textPages != null)
				{
					return this.textPages.Length;
				}
				return this.pagesCount;
			}
			set
			{
				this.pagesCount = value;
			}
		}

		internal TextPage FirstPage
		{
			get
			{
				return this.GetTextPage(0);
			}
		}

		internal TextPage LastPage
		{
			get
			{
				return this.GetTextPage(this.PagesCount - 1);
			}
		}

		internal bool IsPdfProcessing
		{
			get
			{
				return this.fixedDocument != null;
			}
		}

		internal static string GetText(TextPosition startPosition, TextPosition endPosition, string lineSeparator)
		{
			TextPosition textPosition;
			TextPosition textPosition2;
			if (startPosition < endPosition)
			{
				textPosition = startPosition;
				textPosition2 = endPosition;
			}
			else
			{
				textPosition = endPosition;
				textPosition2 = startPosition;
			}
			Line associatedLine = textPosition.GetAssociatedLine();
			Line associatedLine2 = textPosition2.GetAssociatedLine();
			string text;
			if (associatedLine == associatedLine2)
			{
				text = associatedLine.ToString();
				return text.Substring(textPosition.Index - associatedLine.FirstIndex, textPosition2.Index - textPosition.Index);
			}
			StringBuilder stringBuilder = new StringBuilder();
			TextPosition textPosition3 = new TextPosition(textPosition);
			text = associatedLine.ToString();
			stringBuilder.Append(text.Substring(textPosition.Index - associatedLine.FirstIndex));
			stringBuilder.Append(lineSeparator);
			Line line = null;
			for (;;)
			{
				if (line != null)
				{
					stringBuilder.Append(line.ToString());
					stringBuilder.Append(lineSeparator);
				}
				bool flag = textPosition3.MoveToStartOfTheNextLine();
				if (!flag)
				{
					break;
				}
				line = textPosition3.GetAssociatedLine();
				if (line == associatedLine2 && textPosition3.Page.PageNumber == textPosition2.Page.PageNumber)
				{
					goto Block_6;
				}
			}
			return string.Empty;
			Block_6:
			text = associatedLine2.ToString();
			stringBuilder.Append(text.Substring(0, textPosition2.Index - associatedLine2.FirstIndex));
			return stringBuilder.ToString();
		}

		internal static char GetCharBeforePosition(TextPosition position)
		{
			if (position.Index == 0)
			{
				return ' ';
			}
			Line associatedLine = position.GetAssociatedLine();
			if (position.Index == associatedLine.FirstIndex)
			{
				return ' ';
			}
			return associatedLine.ToString()[position.Index - associatedLine.FirstIndex - 1];
		}

		internal static char GetCharAfterPosition(TextPosition position)
		{
			if (position.Index == position.TextPage.LastIndex)
			{
				return ' ';
			}
			Line associatedLine = position.GetAssociatedLine();
			if (position.Index == associatedLine.LastIndex)
			{
				return ' ';
			}
			return associatedLine.ToString()[position.Index - associatedLine.FirstIndex];
		}

		internal TextPage GetTextPage(int index)
		{
			if (index < 0 || index >= this.PagesCount)
			{
				return null;
			}
			TextPage textPage = null;
			if (this.textPages != null)
			{
				textPage = this.textPages[index];
			}
			if (textPage == null)
			{
				textPage = this.CreateTextPage(index);
				if (this.textPages != null)
				{
					this.textPages[index] = textPage;
				}
			}
			return textPage;
		}

		internal void AddPage(int index, TextPage page)
		{
			this.textPages[index] = page;
		}

		internal int IndexOf(TextPage page)
		{
			return page.PageNumber - 1;
		}

		internal TextPage GetNextPage(TextPage page)
		{
			int num = this.IndexOf(page);
			return this.GetTextPage(num + 1);
		}

		internal TextPage GetPrevPage(TextPage page)
		{
			int num = this.IndexOf(page);
			return this.GetTextPage(num - 1);
		}

		protected virtual TextPage CreateTextPage(int index)
		{
			return this.textRecognizer.CreateTextPage(this.fixedDocument.Pages[index]);
		}

		public string ToString(string linesSeparator, string pagesSeparator)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = pagesSeparator.Contains("{0}");
			for (int i = 0; i < this.PagesCount; i++)
			{
				if (flag)
				{
					stringBuilder.Append(string.Format(pagesSeparator, i + 1));
				}
				else
				{
					stringBuilder.Append(pagesSeparator);
				}
				stringBuilder.Append(linesSeparator);
				stringBuilder.Append(this.GetTextPage(i).ToString(linesSeparator));
			}
			return stringBuilder.ToString();
		}

		readonly RadFixedDocument fixedDocument;

		readonly TextPage[] textPages;

		readonly TextRecognizer textRecognizer;

		int pagesCount;
	}
}
