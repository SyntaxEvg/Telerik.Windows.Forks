using System;
using Telerik.Windows.Documents.Flow.Model.Fields.FieldCode;
using Telerik.Windows.Documents.Flow.Model.Fields.MailMerge;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	public class MergeField : Field
	{
		public MergeField(RadFlowDocument document)
			: base(document)
		{
		}

		public string Prefix { get; internal set; }

		public string Suffix { get; internal set; }

		internal bool IsMapped { get; set; }

		internal bool EnablesVerticalFormatting { get; set; }

		internal override void LoadParametersOverride(FieldParameters parameters)
		{
			this.Prefix = parameters.GetSwitchArgument(MergeField.PrefixSwitch);
			this.Suffix = parameters.GetSwitchArgument(MergeField.SuffixSwitch);
			this.IsMapped = parameters.IsSwitchDefined(MergeField.MappedSwitch);
			this.EnablesVerticalFormatting = parameters.IsSwitchDefined(MergeField.VerticalFormattingSwitch);
		}

		internal override FieldResult GetFieldResult(FieldUpdateContext context)
		{
			Guard.ThrowExceptionIfNull<FieldUpdateContext>(context, "context");
			MergeFieldUpdateContext mergeFieldUpdateContext = context as MergeFieldUpdateContext;
			string text;
			if (mergeFieldUpdateContext == null || mergeFieldUpdateContext.CurrentRecord == null)
			{
				text = this.GetDefaultResult(context);
			}
			else
			{
				text = this.GetUpdatedResult(mergeFieldUpdateContext);
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = this.Prefix + text + this.Suffix;
			}
			return new FieldResult(text ?? string.Empty);
		}

		protected override bool IsSwitchWithArgumentOverride(string switchKey)
		{
			return switchKey == MergeField.PrefixSwitch || switchKey == MergeField.SuffixSwitch || switchKey == MergeField.MappedSwitch || switchKey == MergeField.VerticalFormattingSwitch;
		}

		string GetDefaultResult(FieldUpdateContext context)
		{
			if (context.Parameters.FirstArgument == null || string.IsNullOrEmpty(context.Parameters.FirstArgument.Value))
			{
				return null;
			}
			string str = context.Parameters.FirstArgument.Value.ToString();
			return "«" + str + "»";
		}

		string GetUpdatedResult(MergeFieldUpdateContext mergeFieldUpdateContext)
		{
			string value = mergeFieldUpdateContext.Parameters.FirstArgument.Value;
			if (string.IsNullOrEmpty(value))
			{
				return this.GetDefaultResult(mergeFieldUpdateContext);
			}
			object value2 = PropertyValueExtractor.GetValue(mergeFieldUpdateContext.CurrentRecord, value);
			if (value2 == null)
			{
				return string.Empty;
			}
			return value2.ToString();
		}

		internal static readonly string Type = "MERGEFIELD";

		static readonly string PrefixSwitch = "b";

		static readonly string SuffixSwitch = "f";

		static readonly string MappedSwitch = "m";

		static readonly string VerticalFormattingSwitch = "v";
	}
}
