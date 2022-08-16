using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	static class InlineRangeEditor
	{
		internal static IEnumerable<InlineBase> EnumerateInlinesInRange(InlineBase start, InlineBase end, bool respectParagraphs = false)
		{
			InlineRangeEditor.ValidateInlineRange(start, end);
			Paragraph currentParagraph = start.Paragraph;
			int curIndex = currentParagraph.Inlines.IndexOf(start) + 1;
			Paragraph endPar = end.Paragraph;
			int endIndex = endPar.Inlines.IndexOf(end);
			while (currentParagraph != null)
			{
				if (currentParagraph == endPar)
				{
					break;
				}
				while (curIndex < currentParagraph.Inlines.Count)
				{
					yield return currentParagraph.Inlines[curIndex];
					curIndex++;
				}
				curIndex = 0;
				int nextParIndex = currentParagraph.BlockContainer.Blocks.IndexOf(currentParagraph) + 1;
				currentParagraph = ((currentParagraph.BlockContainer.Blocks.Count > nextParIndex) ? (currentParagraph.BlockContainer.Blocks[nextParIndex] as Paragraph) : null);
				if (respectParagraphs && currentParagraph != null)
				{
					EndOfParagraphMarker br = new EndOfParagraphMarker(start.Document);
					yield return br;
				}
			}
			while (curIndex < endIndex)
			{
				yield return currentParagraph.Inlines[curIndex];
				curIndex++;
			}
			yield break;
		}

		internal static IEnumerable<InlineBase> EnumerateFieldInlinesInRange(InlineBase start, InlineBase end, bool respectParagraphs = false)
		{
			FieldCharacter expectedFieldCharacter = null;
			foreach (InlineBase inline in InlineRangeEditor.EnumerateInlinesInRange(start, end, respectParagraphs))
			{
				if (expectedFieldCharacter != null)
				{
					if (expectedFieldCharacter == inline)
					{
						expectedFieldCharacter = null;
					}
				}
				else
				{
					yield return inline;
					FieldCharacter fieldChar = inline as FieldCharacter;
					if (fieldChar != null && fieldChar.FieldCharacterType == FieldCharacterType.Start)
					{
						expectedFieldCharacter = fieldChar.FieldInfo.Separator ?? fieldChar.FieldInfo.End;
					}
				}
			}
			yield break;
		}

		internal static CharacterProperties GetCharacterFormattingFromFirstRunInRange(InlineBase start, InlineBase end)
		{
			foreach (InlineBase inlineBase in InlineRangeEditor.EnumerateInlinesInRange(start, end, false))
			{
				Run run = inlineBase as Run;
				if (run != null)
				{
					return run.Properties;
				}
			}
			return null;
		}

		internal static string GetTextInRange(InlineBase start, InlineBase end, bool getContentFormFieldResultsOnly, bool respectParagraphs)
		{
			IEnumerable<InlineBase> enumerable;
			if (getContentFormFieldResultsOnly)
			{
				enumerable = InlineRangeEditor.EnumerateFieldInlinesInRange(start, end, respectParagraphs);
			}
			else
			{
				enumerable = InlineRangeEditor.EnumerateInlinesInRange(start, end, respectParagraphs);
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (InlineBase inlineBase in enumerable)
			{
				Run run = inlineBase as Run;
				if (run != null)
				{
					stringBuilder.Append(run.Text);
				}
				else
				{
					Break @break = inlineBase as Break;
					if (@break != null && @break.BreakType == BreakType.LineBreak)
					{
						stringBuilder.Append(SpecialSymbols.VerticalTabSymbol);
					}
					else
					{
						EndOfParagraphMarker endOfParagraphMarker = inlineBase as EndOfParagraphMarker;
						if (endOfParagraphMarker != null)
						{
							stringBuilder.Append(Environment.NewLine);
						}
					}
				}
			}
			return stringBuilder.ToString();
		}

		internal static void InsertInlinesInRange(InlineBase start, IEnumerable<InlineBase> inlines)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<InlineBase>>(inlines, "inlines");
			Paragraph paragraph = start.Paragraph;
			int index = paragraph.Inlines.IndexOf(start) + 1;
			paragraph.Inlines.InsertRange(index, inlines);
		}

		internal static void DeleteContent(InlineBase start, InlineBase end)
		{
			InlineRangeEditor.ValidateInlineRange(start, end);
			if (start.Paragraph == end.Paragraph)
			{
				InlineRangeEditor.DeleteContentSameParagraph(start, end);
				return;
			}
			InlineRangeEditor.DeleteContentDifferentParagraphs(start, end);
		}

		static void DeleteContentSameParagraph(InlineBase start, InlineBase end)
		{
			Guard.ThrowExceptionIfFalse(start.Paragraph == end.Paragraph, "Start and end should belong to the same paragraph.");
			Paragraph paragraph = start.Paragraph;
			int num = paragraph.Inlines.IndexOf(start) + 1;
			int num2 = paragraph.Inlines.IndexOf(end);
			paragraph.Inlines.RemoveRange(num, num2 - num);
		}

		static void DeleteContentDifferentParagraphs(InlineBase start, InlineBase end)
		{
			Paragraph paragraph = start.Paragraph;
			Paragraph paragraph2 = end.Paragraph;
			BlockContainerBase blockContainer = paragraph.BlockContainer;
			int num = blockContainer.Blocks.IndexOf(paragraph);
			int num2 = blockContainer.Blocks.IndexOf(paragraph2);
			blockContainer.Blocks.RemoveRange(num + 1, num2 - num - 1);
			int num3 = paragraph.Inlines.IndexOf(start) + 1;
			paragraph.Inlines.RemoveRange(num3, paragraph.Inlines.Count - num3);
			int count = paragraph2.Inlines.IndexOf(end);
			paragraph2.Inlines.RemoveRange(0, count);
			List<InlineBase> list = paragraph.Inlines.ToList<InlineBase>();
			paragraph.Inlines.RemoveRange(0, list.Count);
			paragraph2.Inlines.InsertRange(0, list);
			blockContainer.Blocks.RemoveAt(num);
		}

		static void ValidateInlineRange(InlineBase start, InlineBase end)
		{
			Guard.ThrowExceptionIfNull<InlineBase>(start, "start");
			Guard.ThrowExceptionIfNull<InlineBase>(end, "end");
			Guard.ThrowExceptionIfFalse(start.Document == end.Document, "Start and end inlines should belong to the same document.");
			Paragraph paragraph = start.Paragraph;
			Paragraph paragraph2 = end.Paragraph;
			if (paragraph == paragraph2)
			{
				if (!InlineRangeEditor.IsBefore(start, end))
				{
					throw new InvalidOperationException("Start inline should be before end inline.");
				}
			}
			else
			{
				if (paragraph.Parent != paragraph2.Parent)
				{
					throw new InvalidOperationException("Start and end inlines should belong to paragraph in one block container.");
				}
				if (!InlineRangeEditor.IsBefore(paragraph, paragraph2))
				{
					throw new InvalidOperationException("Start inline should be before end inline.");
				}
			}
		}

		static bool IsBefore(DocumentElementBase start, DocumentElementBase end)
		{
			Guard.ThrowExceptionIfFalse(start.Parent == end.Parent, "Start and End should have same parent.");
			foreach (DocumentElementBase documentElementBase in start.Parent.Children)
			{
				if (documentElementBase == start)
				{
					return true;
				}
				if (documentElementBase == end)
				{
					return false;
				}
			}
			throw new InvalidOperationException("Child element not contained in parent.");
		}
	}
}
