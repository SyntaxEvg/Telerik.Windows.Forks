using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Text;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Editing.Layout;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Fixed.Utilities.Rendering;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Text
{
	class TextRecognizer
	{
		public TextRecognizer(TextDocument parent)
		{
			Guard.ThrowExceptionIfNull<TextDocument>(parent, "parent");
			this.parent = parent;
		}

		public TextPage CreateTextPage(RadFixedPage fixedPage)
		{
			Guard.ThrowExceptionIfNull<RadFixedPage>(fixedPage, "fixedPage");
			TextPage result = null;
			lock (this.lockObject)
			{
				this.StartNewTextPage(fixedPage);
				foreach (ContentElementBase contentElementBase in fixedPage.Content)
				{
					if (contentElementBase.ElementType == FixedDocumentElementType.TextFragment)
					{
						TextFragment textFragment = (TextFragment)contentElementBase;
						Matrix matrix = textFragment.TextMatrix;
						Matrix m = PageLayoutHelper.CalculateVisibibleContentTransformation(fixedPage);
						Matrix matrix2 = ShowText.FlipTextTransformation.MultiplyBy(textFragment.TextMatrix).InverseMatrix().MultiplyBy(textFragment.Position.Matrix);
						matrix2 = matrix2.MultiplyBy(m);
						foreach (CharInfo charInfo in textFragment.TextCollection.Characters)
						{
							Matrix glyphTransform = matrix.MultiplyBy(matrix2);
							double num = TextFragmentLayoutElement.MeasureWidth(charInfo.CharCode, textFragment.TextCollection.Font, textFragment.FontSize, textFragment.WordSpacing, textFragment.CharacterSpacing, textFragment.HorizontalScaling);
							Rect boundingRect = this.CalculateBoundingRect(charInfo, glyphTransform, num, textFragment);
							double charSpacing = ((textFragment.CharacterSpacing != null) ? textFragment.CharacterSpacing.Value : 0.0);
							Character character = new Character(boundingRect, charInfo.Unicode, charSpacing);
							this.ProcessCharacter(character);
							matrix = TranslateText.GetTranslatedTextMatrix(matrix, num);
						}
					}
				}
				result = this.FinishCurrentTextPage();
			}
			return result;
		}

		protected void ProcessCharacter(Character character)
		{
			this.currentCharacter = character;
			if (this.IsLineBreak(character))
			{
				this.StartNewLine();
			}
			else if (this.IsWordEnd(character) || this.IsWhiteSpace(character))
			{
				this.StartNewWord();
			}
			this.AddToCurrentWord(character);
		}

		protected TextPage FinishCurrentTextPage()
		{
			this.FinishCurrentLine();
			this.currentTextPage.Finish();
			this.currentLine = null;
			this.currentWord = null;
			TextPage result = this.currentTextPage;
			this.currentTextPage = null;
			return result;
		}

		protected void StartNewTextPage(RadFixedPage fixedPage)
		{
			Guard.ThrowExceptionIfNull<RadFixedPage>(fixedPage, "fixedPage");
			this.currentTextPage = new TextPage(this.parent, fixedPage);
			this.StartNewTextPageInternal();
		}

		Rect CalculateBoundingRect(CharInfo charInfo, Matrix glyphTransform, double glyphWidth, TextFragment textFragment)
		{
			double num = (textFragment.Font.Ascent.HasValue ? textFragment.Font.Ascent.Value : 1.0);
			double num2 = (textFragment.Font.Descent.HasValue ? textFragment.Font.Descent.Value : 1.0);
			double? textRise = textFragment.TextRise;
			double num3 = ((textRise != null) ? textRise.GetValueOrDefault() : 0.0);
			double y = num2 * textFragment.FontSize + num3;
			double num4 = Math.Max(1.0, (num - 2.0 * num2) / 1000.0);
			double height = num4 * textFragment.FontSize;
			Rect rect = new Rect(new Point(0.0, y), new Size(glyphWidth, height));
			rect = glyphTransform.Transform(rect);
			return rect;
		}

		void StartNewTextPageInternal()
		{
			this.StartNewLine();
			this.currentIndex = 0;
		}

		bool ShouldInclude(string str)
		{
			return !Extensions.IsNullEmptyOrWhiteSpace(str);
		}

		Character GetLastCharacter()
		{
			if (!this.currentWord.IsEmpty)
			{
				return this.currentWord.LastCharacter;
			}
			if (!this.currentLine.IsEmpty)
			{
				return this.currentLine.LastWord.LastCharacter;
			}
			Line lastLine = this.currentTextPage.LastLine;
			if (lastLine == null)
			{
				return null;
			}
			return lastLine.LastWord.LastCharacter;
		}

		bool IsWhiteSpace(Character character)
		{
			Guard.ThrowExceptionIfNull<Character>(character, "glyph");
			if (Extensions.IsNullEmptyOrWhiteSpace(character.ToUnicode))
			{
				return true;
			}
			Character lastCharacter = this.GetLastCharacter();
			if (lastCharacter == null)
			{
				return false;
			}
			double num = character.BoundingRect.Left - lastCharacter.BoundingRect.Right;
			num -= lastCharacter.CharSpacing;
			return num > 1.0 || num < -lastCharacter.BoundingRect.Width / 2.0;
		}

		bool IsPunctuation(string str)
		{
			return !string.IsNullOrEmpty(str) && str.Length <= 1 && char.IsPunctuation(str[0]);
		}

		bool IsWordEnd(Character character)
		{
			Guard.ThrowExceptionIfNull<Character>(character, "glyph");
			Character lastCharacter = this.GetLastCharacter();
			return lastCharacter != null && (this.IsPunctuation(character.ToUnicode) ^ this.IsPunctuation(lastCharacter.ToUnicode));
		}

		bool IsLineBreak(Character glyph)
		{
			Guard.ThrowExceptionIfNull<Character>(glyph, "glyph");
			Character lastCharacter = this.GetLastCharacter();
			if (lastCharacter == null)
			{
				return false;
			}
			double epsilon = 0.1;
			double top = lastCharacter.BoundingRect.Top;
			double bottom = lastCharacter.BoundingRect.Bottom;
			double top2 = glyph.BoundingRect.Top;
			double bottom2 = glyph.BoundingRect.Bottom;
			bool flag = top.IsGreaterThanOrEqualTo(top2, epsilon) && bottom.IsLessThanOrEqualTo(bottom2, epsilon);
			bool flag2 = top2.IsGreaterThanOrEqualTo(top, epsilon) && bottom2.IsLessThanOrEqualTo(bottom, epsilon);
			return !flag && !flag2;
		}

		void AddToCurrentWord(Character character)
		{
			Guard.ThrowExceptionIfNull<Character>(character, "glyph");
			if (!this.ShouldInclude(character.ToUnicode))
			{
				return;
			}
			this.currentWord.AddPosition(this.currentIndex++);
			this.currentWord.AddCharacter(character);
		}

		void StartNewWord()
		{
			if (this.currentWord != null)
			{
				if (this.currentWord.IsEmpty)
				{
					return;
				}
				this.FinishCurrentWord();
			}
			this.currentWord = new Word();
		}

		void FinishCurrentWord()
		{
			this.FinishCurrentWord(false);
		}

		void FinishCurrentWord(bool forcePositionAtTheEnd)
		{
			if (!this.currentWord.IsEmpty)
			{
				if (forcePositionAtTheEnd)
				{
					this.currentWord.AddPosition(this.currentIndex++);
					this.currentWord.Finish(false);
				}
				else if (this.IsWhiteSpace(this.currentCharacter))
				{
					this.currentWord.AddPosition(this.currentIndex++);
					this.currentWord.Finish(true);
				}
				else
				{
					this.currentWord.Finish(false);
				}
				this.currentLine.AddWord(this.currentWord);
				this.currentWord = null;
			}
		}

		void StartNewLine()
		{
			if (this.currentLine != null)
			{
				this.FinishCurrentLine();
			}
			this.StartNewWord();
			this.currentLine = new Line();
		}

		void FinishCurrentLine()
		{
			this.FinishCurrentWord(true);
			if (!this.currentLine.IsEmpty)
			{
				this.currentLine.Finish();
				this.currentTextPage.AddLine(this.currentLine);
				this.currentLine = null;
			}
		}

		internal const double Delta = 1.0;

		readonly object lockObject = new object();

		readonly TextDocument parent;

		TextPage currentTextPage;

		Line currentLine;

		Word currentWord;

		Character currentCharacter;

		int currentIndex;
	}
}
