using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model
{
	sealed class RtfText : RtfElement
	{
		public RtfText(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			this.Text = text;
		}

		public override RtfElementType Type
		{
			get
			{
				return RtfElementType.Text;
			}
		}

		public string Text { get; set; }

		public override string ToString()
		{
			return this.Text;
		}

		protected override bool IsEqual(object obj)
		{
			RtfText rtfText = obj as RtfText;
			return rtfText != null && base.IsEqual(obj) && this.Text.Equals(rtfText.Text);
		}

		protected override int ComputeHashCode()
		{
			return HashTool.AddHashCode(base.ComputeHashCode(), this.Text);
		}
	}
}
