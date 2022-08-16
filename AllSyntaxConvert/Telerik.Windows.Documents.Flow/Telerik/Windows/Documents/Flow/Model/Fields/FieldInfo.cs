using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Flow.Model.Fields.FieldCode;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	public class FieldInfo
	{
		public FieldInfo(RadFlowDocument document)
			: this(document, null)
		{
		}

		internal FieldInfo(RadFlowDocument document, Field field)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			this.document = document;
			this.Field = field;
			this.AttachFieldCharacters(new FieldCharacter(this.Document, FieldCharacterType.Start), new FieldCharacter(this.Document, FieldCharacterType.Separator), new FieldCharacter(this.Document, FieldCharacterType.End));
		}

		internal FieldInfo(FieldCharacter start, FieldCharacter separate, FieldCharacter end)
		{
			Guard.ThrowExceptionIfNull<FieldCharacter>(start, "start");
			Guard.ThrowExceptionIfNull<FieldCharacter>(end, "end");
			this.document = start.Document;
			if (separate == null)
			{
				separate = new FieldCharacter(this.Document, FieldCharacterType.Separator);
			}
			this.AttachFieldCharacters(start, separate, end);
		}

		public FieldCharacter Start { get; set; }

		public FieldCharacter Separator { get; set; }

		public FieldCharacter End { get; set; }

		public Field Field
		{
			get
			{
				return this.field;
			}
			internal set
			{
				this.field = value;
			}
		}

		public bool IsLocked { get; set; }

		public bool IsDirty { get; set; }

		public RadFlowDocument Document
		{
			get
			{
				return this.document;
			}
		}

		internal FieldCharacter CodeEnd
		{
			get
			{
				if (this.Separator.Paragraph == null)
				{
					return this.End;
				}
				return this.Separator;
			}
		}

		public void UpdateField()
		{
			FieldUpdateContext context = new FieldUpdateContext(this);
			this.UpdateFieldInternal(context);
		}

		public string GetCode()
		{
			if (this.Start.Parent == null || this.End.Parent == null)
			{
				return string.Empty;
			}
			return InlineRangeEditor.GetTextInRange(this.Start, this.CodeEnd, true, false);
		}

		public string GetResult()
		{
			if (this.Separator == null || this.Separator.Parent == null || this.End.Parent == null)
			{
				return string.Empty;
			}
			return InlineRangeEditor.GetTextInRange(this.Separator, this.End, true, true);
		}

		internal void UpdateFieldInternal(FieldUpdateContext context)
		{
			IEnumerable<FieldCharacter> fields = (from inline in InlineRangeEditor.EnumerateInlinesInRange(this.Start, this.CodeEnd, false)
				where inline.Type == DocumentElementType.FieldCharacter
				select inline).Cast<FieldCharacter>();
			List<FieldCharacter> orderedListOfFieldToUpdate = FieldUpdateScheduler.GetOrderedListOfFieldToUpdate(fields);
			orderedListOfFieldToUpdate.Add(this.Start);
			foreach (FieldCharacter fieldCharacter in orderedListOfFieldToUpdate)
			{
				fieldCharacter.FieldInfo.UpdateFieldCore(context);
			}
		}

		internal FieldCodeParseResult LoadFieldFromCode()
		{
			FieldCodeParseResult fieldCodeParseResult = FieldCodeParser.ParseField(this);
			this.Field = fieldCodeParseResult.Field;
			this.Field.LoadParameters(fieldCodeParseResult.FieldParameters);
			return fieldCodeParseResult;
		}

		internal void UpdateFieldCore(FieldUpdateContext context)
		{
			if (this.IsLocked)
			{
				return;
			}
			FieldCodeParseResult fieldCodeParseResult = this.LoadFieldFromCode();
			context.Parameters = fieldCodeParseResult.FieldParameters;
			IEnumerable<InlineBase> enumerable = null;
			if (this.Separator.Paragraph == null)
			{
				context.Editor.MoveToInlineStart(this.End);
				context.Editor.InsertInline(this.Separator);
			}
			else
			{
				if (!this.Field.PreserveResultOnUpdate)
				{
					enumerable = InlineRangeEditor.EnumerateFieldInlinesInRange(this.Separator, this.End, false).ToList<InlineBase>();
					InlineRangeEditor.DeleteContent(this.Separator, this.End);
				}
				context.Editor.MoveToInlineEnd(this.Separator);
			}
			if (!this.Field.UpdateResult(context) && enumerable != null)
			{
				InlineRangeEditor.InsertInlinesInRange(this.Separator, enumerable);
			}
		}

		void AttachFieldCharacters(FieldCharacter start, FieldCharacter separate, FieldCharacter end)
		{
			Guard.ThrowExceptionIfNull<FieldCharacter>(start, "start");
			Guard.ThrowExceptionIfNull<FieldCharacter>(separate, "separate");
			Guard.ThrowExceptionIfNull<FieldCharacter>(end, "end");
			Guard.ThrowExceptionIfFalse(start.FieldCharacterType == FieldCharacterType.Start, "Unexpected field character type");
			Guard.ThrowExceptionIfFalse(separate.FieldCharacterType == FieldCharacterType.Separator, "Unexpected field character type");
			Guard.ThrowExceptionIfFalse(end.FieldCharacterType == FieldCharacterType.End, "Unexpected field character type");
			Guard.ThrowExceptionIfFalse(start.FieldInfo == null, "Start field character already associated to another FieldInfo.");
			Guard.ThrowExceptionIfFalse(separate.FieldInfo == null, "Start field character already associated to another FieldInfo.");
			Guard.ThrowExceptionIfFalse(end.FieldInfo == null, "End field character already associated to another FieldInfo.");
			Guard.ThrowExceptionIfFalse(start.Document == separate.Document, "Start and Separator characters belong to different documents.");
			Guard.ThrowExceptionIfFalse(start.Document == end.Document, "Start and End characters belong to different documents.");
			this.Start = start;
			this.Start.FieldInfo = this;
			this.Separator = separate;
			this.Separator.FieldInfo = this;
			this.End = end;
			this.End.FieldInfo = this;
		}

		readonly RadFlowDocument document;

		Field field;
	}
}
