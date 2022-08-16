using System;
using System.Text;
using Telerik.Windows.Documents.Flow.Model.Fields.FieldCode;

namespace Telerik.Windows.Documents.Flow.Model.Fields
{
	public class Hyperlink : Field
	{
		public Hyperlink(RadFlowDocument document)
			: base(document)
		{
		}

		public string Uri { get; internal set; }

		public bool IsAnchor { get; internal set; }

		public string ToolTip { get; internal set; }

		internal override bool PreserveResultOnUpdate
		{
			get
			{
				return true;
			}
		}

		internal string CreateHyperlinkCode()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" HYPERLINK ");
			if (this.IsAnchor)
			{
				stringBuilder.Append("\\").Append(Hyperlink.BookmarkSwitch).Append(" ");
			}
			stringBuilder.Append("\"");
			stringBuilder.Append(Field.EncodeParameter(this.Uri));
			stringBuilder.Append("\"");
			if (!string.IsNullOrEmpty(this.ToolTip))
			{
				stringBuilder.Append(" \\").Append(Hyperlink.ToolTipSwitch).Append(" \"");
				stringBuilder.Append(Field.EncodeParameter(this.ToolTip));
				stringBuilder.Append("\"");
			}
			stringBuilder.Append(" ");
			return stringBuilder.ToString();
		}

		internal override void LoadParametersOverride(FieldParameters parameters)
		{
			FieldSwitch fieldSwitch = null;
			if (parameters.TryGetSwitch(Hyperlink.BookmarkSwitch, out fieldSwitch))
			{
				this.Uri = ((fieldSwitch.Argument != null) ? fieldSwitch.Argument.Value : string.Empty);
				this.IsAnchor = true;
			}
			else
			{
				this.Uri = ((parameters.FirstArgument != null) ? parameters.FirstArgument.Value : string.Empty);
				this.IsAnchor = false;
			}
			this.ToolTip = parameters.GetSwitchArgument(Hyperlink.ToolTipSwitch);
		}

		internal override FieldResult GetFieldResult(FieldUpdateContext context)
		{
			FieldResult result = null;
			if (string.IsNullOrEmpty(context.FieldInfo.GetResult()))
			{
				result = new FieldResult(this.Uri);
			}
			return result;
		}

		protected override bool IsSwitchWithArgumentOverride(string switchKey)
		{
			return switchKey == Hyperlink.BookmarkSwitch || switchKey == Hyperlink.TargetSwitch || switchKey == Hyperlink.ToolTipSwitch;
		}

		static readonly string ToolTipSwitch = "o";

		static readonly string TargetSwitch = "t";

		static readonly string BookmarkSwitch = "l";
	}
}
