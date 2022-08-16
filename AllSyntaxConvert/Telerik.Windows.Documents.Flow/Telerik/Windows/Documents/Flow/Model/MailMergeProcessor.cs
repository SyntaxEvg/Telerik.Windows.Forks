using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model.Editing;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.Flow.Model.Fields.MailMerge;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	static class MailMergeProcessor
	{
		public static RadFlowDocument Execute(RadFlowDocument document, IEnumerable collection)
		{
			Guard.ThrowExceptionIfNull<IEnumerable>(collection, "collection");
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			RadFlowDocument radFlowDocument = null;
			IEnumerator enumerator = collection.GetEnumerator();
			while (enumerator.MoveNext())
			{
				RadFlowDocument radFlowDocument2 = document.Clone();
				MailMergeProcessor.ExecuteMailMerge(radFlowDocument2, enumerator.Current);
				if (radFlowDocument == null)
				{
					radFlowDocument = radFlowDocument2;
				}
				else
				{
					radFlowDocument.Merge(radFlowDocument2);
				}
			}
			if (radFlowDocument == null)
			{
				throw new ArgumentException("Collection could not be empty.", "collection");
			}
			return radFlowDocument;
		}

		static void ExecuteMailMerge(RadFlowDocument document, object record)
		{
			IEnumerable<FieldCharacter> fields = document.EnumerateChildrenOfType<FieldCharacter>();
			List<FieldCharacter> orderedListOfFieldToUpdate = FieldUpdateScheduler.GetOrderedListOfFieldToUpdate(fields);
			foreach (FieldCharacter fieldCharacter in orderedListOfFieldToUpdate)
			{
				FieldInfo fieldInfo = fieldCharacter.FieldInfo;
				string code = fieldInfo.GetCode();
				if (!code.Contains(MergeField.Type))
				{
					fieldInfo.UpdateField();
				}
				else
				{
					ParagraphProperties properties = fieldInfo.Start.Paragraph.Properties;
					ParagraphProperties properties2 = fieldInfo.End.Paragraph.Properties;
					MergeFieldUpdateContext context = new MergeFieldUpdateContext(fieldInfo, record);
					fieldInfo.UpdateFieldInternal(context);
					string result = fieldInfo.GetResult();
					CharacterProperties characterFormattingFromFirstRunInRange = InlineRangeEditor.GetCharacterFormattingFromFirstRunInRange(fieldInfo.Start, fieldInfo.CodeEnd);
					InlineRangeEditor.DeleteContent(fieldInfo.Start, fieldInfo.End);
					Paragraph paragraph = fieldInfo.Start.Paragraph;
					paragraph.Properties.CopyPropertiesFrom(properties);
					int num = fieldInfo.Start.Paragraph.Inlines.IndexOf(fieldInfo.Start);
					fieldInfo.Start.Paragraph.Inlines.Remove(fieldInfo.Start);
					fieldInfo.End.Paragraph.Inlines.Remove(fieldInfo.End);
					RadFlowDocumentEditor radFlowDocumentEditor = new RadFlowDocumentEditor(paragraph.Document);
					if (num + 1 <= paragraph.Inlines.Count)
					{
						radFlowDocumentEditor.MoveToInlineStart(paragraph.Inlines[num]);
					}
					else if (num > 0)
					{
						radFlowDocumentEditor.MoveToInlineEnd(paragraph.Inlines[num - 1]);
					}
					else
					{
						radFlowDocumentEditor.MoveToParagraphStart(paragraph);
					}
					radFlowDocumentEditor.CharacterFormatting.CopyPropertiesFrom(characterFormattingFromFirstRunInRange);
					radFlowDocumentEditor.ParagraphFormatting.CopyPropertiesFrom(properties2);
					radFlowDocumentEditor.InsertText(result);
				}
			}
		}
	}
}
