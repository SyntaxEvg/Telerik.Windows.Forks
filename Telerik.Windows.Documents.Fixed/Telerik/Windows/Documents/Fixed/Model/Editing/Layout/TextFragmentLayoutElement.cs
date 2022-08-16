using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	class TextFragmentLayoutElement : ContentElementLayoutElementBase<TextFragment>
	{
		public TextFragmentLayoutElement(TextFragment textFragment, TextProperties properties, bool useTextFragmentProperties)
			: base(textFragment, TextFragmentLayoutElement.MeasureWidth(textFragment), TextFragmentLayoutElement.MeasureWidth(textFragment, textFragment.Text.TrimEnd(new char[0])), useTextFragmentProperties ? TextFragmentLayoutElement.MeasureHeight(textFragment) : TextFragmentLayoutElement.GetBaselineOffset(properties.Font, properties.FontSize), useTextFragmentProperties ? textFragment.Font : properties.Font, useTextFragmentProperties ? textFragment.FontSize : properties.FontSize, properties.RenderingMode, properties.UnderlineColor, properties.UnderlinePattern, properties.HighlightColor)
		{
		}

		public TextFragmentLayoutElement(TextFragment textFragment, TextFragmentLayoutElement other)
			: base(textFragment, TextFragmentLayoutElement.MeasureWidth(textFragment), TextFragmentLayoutElement.MeasureWidth(textFragment, textFragment.Text.TrimEnd(new char[0])), TextFragmentLayoutElement.MeasureHeight(textFragment), other)
		{
		}

		internal override bool CanAddToLineEnding
		{
			get
			{
				string text = base.Element.Text;
				return text.Length == 0 || char.IsWhiteSpace(text[0]);
			}
		}

		internal override double GetElementWidthFromStartToDecimal()
		{
			int length;
			if (this.TryGetDecimalIndex(out length))
			{
				string text = base.Element.Text.Substring(0, length);
				return TextFragmentLayoutElement.MeasureWidth(base.Element, text);
			}
			return base.GetElementWidthFromStartToDecimal();
		}

		bool TryGetDecimalIndex(out int lastDigitIndex)
		{
			lastDigitIndex = 0;
			bool flag = false;
			foreach (char c in base.Element.Text.ToCharArray())
			{
				if (char.IsDigit(c))
				{
					flag = true;
				}
				else if (flag)
				{
					return true;
				}
				lastDigitIndex++;
			}
			return false;
		}

		internal override bool CanSplitToMergeWithNextLayoutElement
		{
			get
			{
				string text = base.Element.Text;
				int indexOfLastWhiteSpace = TextFragmentLayoutElement.GetIndexOfLastWhiteSpace(text);
				if (indexOfLastWhiteSpace > -1)
				{
					int num = indexOfLastWhiteSpace + 1;
					if (num < text.Length)
					{
						return true;
					}
				}
				return false;
			}
		}

		internal override bool CanMergeWithNextLayoutElement
		{
			get
			{
				string text = base.Element.Text;
				return text.Length > 0 && !char.IsWhiteSpace(text[text.Length - 1]);
			}
		}

		public static IEnumerable<LayoutElementBase> CreateTextFragmentLayoutElements(Block block, string text)
		{
			Guard.ThrowExceptionIfNull<Block>(block, "block");
			Guard.ThrowExceptionIfNullOrEmpty(text, "text");
			StringBuilder builder = new StringBuilder();
			FontBase font = block.TextProperties.Font;
			FontBase fallbackFont = null;
			foreach (char c in text)
			{
				if (TextFragmentLayoutElement.IsCharacterInFont(font, c))
				{
					if (fallbackFont != null)
					{
						TextFragment textFragment;
						using (block.SaveTextProperties())
						{
							block.TextProperties.Font = fallbackFont;
							textFragment = TextFragmentLayoutElement.CreateTextFragment(block, builder.ToString());
						}
						yield return new TextFragmentLayoutElement(textFragment, block.TextProperties, false);
						fallbackFont = null;
						builder.Clear();
					}
				}
				else
				{
					if (fallbackFont == null && builder.Length > 0)
					{
						TextFragment textFragment2 = TextFragmentLayoutElement.CreateTextFragment(block, builder.ToString());
						yield return new TextFragmentLayoutElement(textFragment2, block.TextProperties, true);
						builder.Clear();
					}
					if (fallbackFont == null || !TextFragmentLayoutElement.IsCharacterInFont(fallbackFont, c))
					{
						fallbackFont = TextFragmentLayoutElement.FindFallback(font, c);
					}
				}
				builder.Append(c);
			}
			if (builder.Length > 0)
			{
				if (fallbackFont == null)
				{
					TextFragment textFragment3 = TextFragmentLayoutElement.CreateTextFragment(block, builder.ToString());
					yield return new TextFragmentLayoutElement(textFragment3, block.TextProperties, true);
				}
				else
				{
					TextFragment textFragment4;
					using (block.SaveTextProperties())
					{
						block.TextProperties.Font = fallbackFont;
						textFragment4 = TextFragmentLayoutElement.CreateTextFragment(block, builder.ToString());
					}
					yield return new TextFragmentLayoutElement(textFragment4, block.TextProperties, false);
				}
			}
			yield break;
		}

		public override string ToString()
		{
			return base.Element.ToString();
		}

		protected override Matrix Transform(DrawLayoutElementContext context, Matrix transform)
		{
			Guard.ThrowExceptionIfNull<DrawLayoutElementContext>(context, "context");
			return base.Transform(context, transform).TranslateMatrix(0.0, context.GetLineActualBaselineOffset());
		}

		internal override bool CanFit(double maxTotalWidth, double currentLineOffsetX)
		{
			return base.TrimmedWidth == 0.0 || base.CanFit(maxTotalWidth, currentLineOffsetX);
		}

		internal override void SplitToMergeWithNextLayoutElement(out LayoutElementBase firstPart, out LayoutElementBase secondPart)
		{
			if (this.CanSplitToMergeWithNextLayoutElement)
			{
				int indexOfLastWhiteSpace = TextFragmentLayoutElement.GetIndexOfLastWhiteSpace(base.Element.Text);
				int secondStartIndex = indexOfLastWhiteSpace + 1;
				string text;
				string text2;
				TextFragmentLayoutElement.SplitInTwo(base.Element.Text, secondStartIndex, out text, out text2);
				firstPart = new TextFragmentLayoutElement(new TextFragment(base.Element, text), this);
				secondPart = new TextFragmentLayoutElement(new TextFragment(base.Element, text2), this);
				return;
			}
			base.SplitToMergeWithNextLayoutElement(out firstPart, out secondPart);
		}

		internal override bool TrySplitToAddToLineEnding(out LayoutElementBase firstPart, out LayoutElementBase secondPart)
		{
			string text;
			string text2;
			if (TextFragmentLayoutElement.TrySplitAfterFirstWhitespace(base.Element.Text, 0, out text, out text2))
			{
				firstPart = new TextFragmentLayoutElement(new TextFragment(base.Element, text), this);
				secondPart = new TextFragmentLayoutElement(new TextFragment(base.Element, text2), this);
				return true;
			}
			return base.TrySplitToAddToLineEnding(out firstPart, out secondPart);
		}

		internal override LineInfo CompleteLine(double maxLineWidth, double currentLineOffsetX, List<LayoutElementBase> elementsInCurrentLine, out IEnumerable<LayoutElementBase> pendingLayoutElements)
		{
			Guard.ThrowExceptionIfLessThan<double>(maxLineWidth, currentLineOffsetX + base.Width, "lineWidth");
			LineInfo result = null;
			if (!this.TryCompleteLineByFittingFirstPart(maxLineWidth, currentLineOffsetX, elementsInCurrentLine, out result, out pendingLayoutElements) && !this.TryCompleteLineBySplittingCurrentLineElements(elementsInCurrentLine, out result, out pendingLayoutElements))
			{
				result = this.CompleteLineByForceSplittingThisElement(maxLineWidth, currentLineOffsetX, elementsInCurrentLine, out pendingLayoutElements);
			}
			return result;
		}

		internal override double GetMinWidth()
		{
			double num = 0.0;
			for (int i = 0; i < base.Element.Text.Length; i++)
			{
				num = Math.Max(TextFragmentLayoutElement.MeasureWidth(base.Element, base.Element.Text[i].ToString()), num);
			}
			return num;
		}

		internal override Size GetMinMeasureSize()
		{
			if (string.IsNullOrEmpty(base.Element.Text))
			{
				return new Size(0.0, 0.0);
			}
			string text = base.Element.Text[0].ToString();
			double width = (string.IsNullOrWhiteSpace(text) ? 0.0 : TextFragmentLayoutElement.MeasureWidth(base.Element, text));
			double height = TextFragmentLayoutElement.MeasureHeight(base.Element);
			return new Size(width, height);
		}

		LineInfo CompleteLineByForceSplittingThisElement(double maxLineWidth, double currentLineOffsetX, List<LayoutElementBase> elementsInCurrentLine, out IEnumerable<LayoutElementBase> pendingLayoutElements)
		{
			LineInfo lineInfo = new LineInfo();
			lineInfo.AddRange(elementsInCurrentLine);
			string text = base.Element.Text;
			double num = TextFragmentLayoutElement.MeasureWidth(base.Element, text.Substring(0, 1));
			int num2 = 1;
			for (int i = 1; i < text.Length; i++)
			{
				string text2 = text.Substring(0, i + 1);
				double num3 = TextFragmentLayoutElement.MeasureWidth(base.Element, text2);
				if (num3 + currentLineOffsetX > maxLineWidth)
				{
					num2 = i;
					if (!char.IsWhiteSpace(text[num2]))
					{
						break;
					}
				}
			}
			bool flag = !elementsInCurrentLine.Any<LayoutElementBase>();
			bool flag2 = num + currentLineOffsetX <= maxLineWidth;
			bool flag3 = num2 < text.Length && !char.IsWhiteSpace(text[num2]);
			flag3 = (flag3 && flag2) || (flag3 && flag);
			TextFragmentLayoutElement textFragmentLayoutElement = null;
			if (flag3)
			{
				string text3;
				string text4;
				TextFragmentLayoutElement.SplitInTwo(text, num2, out text3, out text4);
				textFragmentLayoutElement = new TextFragmentLayoutElement(new TextFragment(base.Element, text3), this);
				pendingLayoutElements = new TextFragmentLayoutElement[]
				{
					new TextFragmentLayoutElement(new TextFragment(base.Element, text4), this)
				};
			}
			else if (flag)
			{
				textFragmentLayoutElement = this;
				pendingLayoutElements = Enumerable.Empty<TextFragmentLayoutElement>();
			}
			else
			{
				pendingLayoutElements = new TextFragmentLayoutElement[] { this };
			}
			if (textFragmentLayoutElement != null)
			{
				lineInfo.Add(textFragmentLayoutElement);
			}
			return lineInfo;
		}

		bool TryCompleteLineBySplittingCurrentLineElements(List<LayoutElementBase> elementsInCurrentLine, out LineInfo line, out IEnumerable<LayoutElementBase> pendingLayoutElements)
		{
			int i;
			for (i = elementsInCurrentLine.Count - 1; i > -1; i--)
			{
				LayoutElementBase layoutElementBase = elementsInCurrentLine[i];
				if (!layoutElementBase.CanMergeWithNextLayoutElement || layoutElementBase.CanSplitToMergeWithNextLayoutElement)
				{
					break;
				}
			}
			if (i > -1)
			{
				line = this.CompleteLineBySplittingCurrentLineElements(elementsInCurrentLine, i, out pendingLayoutElements);
				return true;
			}
			line = null;
			pendingLayoutElements = null;
			return false;
		}

		LineInfo CompleteLineBySplittingCurrentLineElements(List<LayoutElementBase> elementsInCurrentLine, int indexOfLastElementToPreserve, out IEnumerable<LayoutElementBase> pendingLayoutElements)
		{
			LayoutElementBase layoutElementBase = null;
			LayoutElementBase layoutElementBase2 = null;
			LayoutElementBase layoutElementBase3 = elementsInCurrentLine[indexOfLastElementToPreserve];
			bool canSplitToMergeWithNextLayoutElement = layoutElementBase3.CanSplitToMergeWithNextLayoutElement;
			if (canSplitToMergeWithNextLayoutElement)
			{
				layoutElementBase3.SplitToMergeWithNextLayoutElement(out layoutElementBase, out layoutElementBase2);
			}
			int num = (canSplitToMergeWithNextLayoutElement ? indexOfLastElementToPreserve : (indexOfLastElementToPreserve + 1));
			int num2 = elementsInCurrentLine.Count - num;
			List<LayoutElementBase> list = new List<LayoutElementBase>();
			if (layoutElementBase2 != null)
			{
				list.Add(layoutElementBase2);
			}
			if (num2 > 0)
			{
				list.AddRange(elementsInCurrentLine.Skip(canSplitToMergeWithNextLayoutElement ? (num + 1) : num).Take(num2));
				elementsInCurrentLine.RemoveRange(num, num2);
			}
			list.Add(this);
			if (layoutElementBase != null)
			{
				elementsInCurrentLine.Add(layoutElementBase);
			}
			LineInfo lineInfo = new LineInfo();
			lineInfo.AddRange(elementsInCurrentLine);
			pendingLayoutElements = list;
			return lineInfo;
		}

		bool TryCompleteLineByFittingFirstPart(double maxLineWidth, double currentLineOffsetX, IEnumerable<LayoutElementBase> elementsInCurrentLine, out LineInfo line, out IEnumerable<LayoutElementBase> pendingElements)
		{
			string text = base.Element.Text;
			bool flag = false;
			line = null;
			pendingElements = null;
			TextFragmentLayoutElement textFragmentLayoutElement = null;
			string text2;
			string text3;
			if (TextFragmentLayoutElement.TrySplitAfterFirstWhitespace(text, 0, out text2, out text3))
			{
				textFragmentLayoutElement = new TextFragmentLayoutElement(new TextFragment(base.Element, text2), this);
				flag = textFragmentLayoutElement.CanFit(maxLineWidth, currentLineOffsetX);
				if (flag)
				{
					bool flag2 = true;
					TextFragmentLayoutElement textFragmentLayoutElement2 = textFragmentLayoutElement;
					string text4 = text3;
					while (flag2)
					{
						int length = text2.Length;
						if (TextFragmentLayoutElement.TrySplitAfterFirstWhitespace(text, length, out text2, out text3))
						{
							textFragmentLayoutElement = new TextFragmentLayoutElement(new TextFragment(base.Element, text2), this);
							flag2 = textFragmentLayoutElement.CanFit(maxLineWidth, currentLineOffsetX);
							if (flag2)
							{
								textFragmentLayoutElement2 = textFragmentLayoutElement;
								text4 = text3;
							}
						}
						else
						{
							flag2 = false;
						}
					}
					textFragmentLayoutElement = textFragmentLayoutElement2;
					pendingElements = new LayoutElementBase[]
					{
						new TextFragmentLayoutElement(new TextFragment(base.Element, text4), this)
					};
				}
				else if (char.IsWhiteSpace(text[0]))
				{
					pendingElements = new TextFragmentLayoutElement[]
					{
						new TextFragmentLayoutElement(new TextFragment(base.Element, text3), this)
					};
					flag = true;
				}
			}
			else if (string.IsNullOrWhiteSpace(text))
			{
				textFragmentLayoutElement = this;
				pendingElements = Enumerable.Empty<LayoutElementBase>();
				flag = true;
			}
			if (flag)
			{
				line = new LineInfo();
				line.AddRange(elementsInCurrentLine);
				line.Add(textFragmentLayoutElement);
			}
			return flag;
		}

		public static double GetBaselineOffset(FontBase font, double fontSize)
		{
			Guard.ThrowExceptionIfNull<FontBase>(font, "font");
			return fontSize * (Math.Abs(font.Ascent.Value) + Math.Abs(font.LineGap));
		}

		static bool TrySplitAfterFirstWhitespace(string text, int startIndex, out string firstPart, out string secondPart)
		{
			firstPart = null;
			secondPart = null;
			int indexOfFirstWhiteSpace = TextFragmentLayoutElement.GetIndexOfFirstWhiteSpace(text, startIndex);
			int indexOfFirstNonWhiteSpace = TextFragmentLayoutElement.GetIndexOfFirstNonWhiteSpace(text, indexOfFirstWhiteSpace + 1);
			bool flag = indexOfFirstWhiteSpace >= 0 && indexOfFirstNonWhiteSpace > 0 && indexOfFirstNonWhiteSpace < text.Length;
			if (flag)
			{
				TextFragmentLayoutElement.SplitInTwo(text, indexOfFirstNonWhiteSpace, out firstPart, out secondPart);
			}
			return flag;
		}

		static int GetIndexOfFirstWhiteSpace(string text, int startIndex)
		{
			for (int i = startIndex; i < text.Length; i++)
			{
				if (char.IsWhiteSpace(text[i]))
				{
					return i;
				}
			}
			return -1;
		}

		static int GetIndexOfLastWhiteSpace(string text)
		{
			for (int i = text.Length - 1; i >= 0; i--)
			{
				if (char.IsWhiteSpace(text[i]))
				{
					return i;
				}
			}
			return -1;
		}

		static int GetIndexOfFirstNonWhiteSpace(string text, int startIndex)
		{
			for (int i = startIndex; i < text.Length; i++)
			{
				if (!char.IsWhiteSpace(text[i]))
				{
					return i;
				}
			}
			return -1;
		}

		static TextFragment CreateTextFragment(Block block, string text)
		{
			Guard.ThrowExceptionIfNull<Block>(block, "block");
			Guard.ThrowExceptionIfNullOrEmpty(text, "text");
			TextFragment textFragment = new TextFragment();
			textFragment.Text = text;
			block.GraphicProperties.CopyTo(textFragment.TextProperties.GeometryProperties);
			block.TextProperties.CopyTo(textFragment.TextProperties);
			if (block.TextProperties.BaselineAlignment != Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment.Baseline)
			{
				double num = block.TextProperties.FontSize * block.TextProperties.Font.Height;
				textFragment.FontSize = block.TextProperties.FontSize * 0.65;
				switch (block.TextProperties.BaselineAlignment)
				{
				case Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment.Subscript:
					textFragment.TextRise = new double?(num * -0.141);
					break;
				case Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment.Superscript:
					textFragment.TextRise = new double?(num * 0.35);
					break;
				}
			}
			return textFragment;
		}

		internal static double MeasureWidth(TextFragment textFragment)
		{
			Guard.ThrowExceptionIfNull<TextFragment>(textFragment, "textFragment");
			return TextFragmentLayoutElement.MeasureWidth(textFragment.TextCollection, textFragment.FontSize, textFragment.WordSpacing, textFragment.CharacterSpacing, textFragment.HorizontalScaling);
		}

		internal static double MeasureHeight(TextFragment textFragment)
		{
			Guard.ThrowExceptionIfNull<TextFragment>(textFragment, "textFragment");
			return TextFragmentLayoutElement.GetBaselineOffset(textFragment.Font, textFragment.FontSize);
		}

		internal static double MeasureWidth(CharCode charCode, FontBase font, double fontSize, double? wordSpacing, double? characterSpacing, double? horizontalScaling)
		{
			double num = font.GetWidth(charCode.Code) / 1000.0;
			double num2 = num * fontSize;
			if (wordSpacing != null && charCode.Code == 32 && charCode.Size == 1)
			{
				num2 += wordSpacing.Value;
			}
			if (characterSpacing != null)
			{
				num2 += characterSpacing.Value;
			}
			if (horizontalScaling != null)
			{
				num2 *= horizontalScaling.Value;
			}
			return num2;
		}

		static double MeasureWidth(TextFragment textFragment, string text)
		{
			Guard.ThrowExceptionIfNull<TextFragment>(textFragment, "textFragment");
			Guard.ThrowExceptionIfNull<string>(text, "text");
			if (string.IsNullOrEmpty(text))
			{
				return 0.0;
			}
			return TextFragmentLayoutElement.MeasureWidth(new TextCollection(textFragment.Font, text), textFragment.FontSize, textFragment.WordSpacing, textFragment.CharacterSpacing, textFragment.HorizontalScaling);
		}

		static double MeasureWidth(TextCollection textCollection, double fontSize, double? wordSpacing, double? characterSpacing, double? horizontalScaling)
		{
			Guard.ThrowExceptionIfNull<TextCollection>(textCollection, "textCollection");
			double num = 0.0;
			foreach (CharInfo charInfo in textCollection.Characters)
			{
				double num2 = TextFragmentLayoutElement.MeasureWidth(charInfo.CharCode, textCollection.Font, fontSize, wordSpacing, characterSpacing, horizontalScaling);
				num += num2;
			}
			return num;
		}

		static bool IsCharacterInFont(FontBase font, char c)
		{
			ushort num;
			return font.TryGetGlyphId((int)c, out num);
		}

		static FontBase FindFallback(FontBase original, char c)
		{
			FallbackRange fallbackRange = FallbackRange.GetFallbackRange(c);
			if (fallbackRange == null)
			{
				return FallbackRange.DefaultRange.GetFallbackFont(original, c);
			}
			return fallbackRange.GetFallbackFont(original, c);
		}

		static void SplitInTwo(string text, int secondStartIndex, out string firstPart, out string secondPart)
		{
			firstPart = text.Substring(0, secondStartIndex);
			secondPart = text.Substring(secondStartIndex);
		}

		const double SubscriptAndSuperscriptSizeRatio = 0.65;

		const double SubscriptPositionRatio = -0.141;

		const double SuperscriptPositionRatio = 0.35;
	}
}
