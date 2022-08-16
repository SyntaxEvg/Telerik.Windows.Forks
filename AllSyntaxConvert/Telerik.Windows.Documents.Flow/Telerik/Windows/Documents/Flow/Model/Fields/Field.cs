using System;
using System.Globalization;
using Telerik.Windows.Documents.Flow.Model.Fields.FieldCode;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	public abstract class Field
	{
		protected Field(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			this.Document = document;
		}

		public string DateTimeFormatting { get; set; }

		public string NumericFormatting { get; set; }

		public string GeneralFormatting { get; set; }

		public RadFlowDocument Document { get; set; }

		internal virtual bool ExpectComparisonArgument
		{
			get
			{
				return false;
			}
		}

		internal virtual bool PreserveResultOnUpdate
		{
			get
			{
				return false;
			}
		}

		internal void LoadParameters(FieldParameters parameters)
		{
			this.DateTimeFormatting = parameters.GetSwitchArgument(Field.DateFormattingSwitch);
			this.NumericFormatting = parameters.GetSwitchArgument(Field.NumericFormattingSwitch);
			this.GeneralFormatting = parameters.GetSwitchArgument(Field.GeneralFormattingSwitch);
			this.LoadParametersOverride(parameters);
		}

		internal bool IsSwitchWithArgument(string sw)
		{
			return (sw.Length == 1 && (sw[0] == FieldParserConstants.GeneralFormattingSwitch || sw[0] == FieldParserConstants.NumericFormattingSwitch || sw[0] == FieldParserConstants.DateFormattingSwitch)) || this.IsSwitchWithArgumentOverride(sw);
		}

		internal virtual void LoadParametersOverride(FieldParameters parameters)
		{
		}

		internal abstract FieldResult GetFieldResult(FieldUpdateContext context);

		internal bool UpdateResult(FieldUpdateContext context)
		{
			Guard.ThrowExceptionIfNull<FieldUpdateContext>(context, "context");
			FieldResult fieldResult = this.GetFieldResult(context);
			if (fieldResult != null)
			{
				Field.UpdateResult(context, fieldResult);
				return true;
			}
			return false;
		}

		protected static string EncodeParameter(string parameter)
		{
			return TextHelper.AddSlashes(parameter);
		}

		protected string GetFormattedDate(DateTime date)
		{
			string empty = string.Empty;
			string format;
			if (!string.IsNullOrEmpty(this.DateTimeFormatting))
			{
				format = this.DateTimeFormatting.Replace("am/pm", "tt").Replace("AM/PM", "tt");
			}
			else
			{
				format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
			}
			return date.ToString(format, CultureInfo.InvariantCulture);
		}

		protected virtual bool IsSwitchWithArgumentOverride(string switchKey)
		{
			return false;
		}

		static void UpdateResult(FieldUpdateContext context, FieldResult fieldResult)
		{
			Guard.ThrowExceptionIfNull<FieldUpdateContext>(context, "context");
			Guard.ThrowExceptionIfNull<FieldResult>(fieldResult, "fieldResult");
			FieldCharacter end = context.FieldInfo.Separator ?? context.FieldInfo.End;
			CharacterProperties characterFormattingFromFirstRunInRange = InlineRangeEditor.GetCharacterFormattingFromFirstRunInRange(context.FieldInfo.Start, end);
			context.Editor.CharacterFormatting.CopyPropertiesFrom(characterFormattingFromFirstRunInRange);
			context.Editor.InsertText(fieldResult.Result);
		}

		static readonly string DateFormattingSwitch = "@";

		static readonly string NumericFormattingSwitch = "#";

		static readonly string GeneralFormattingSwitch = "*";
	}
}
