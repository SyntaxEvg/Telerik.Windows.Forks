using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class TextTitle : Title
	{
		public TextTitle(string text)
		{
			this.text = text;
		}

		public override TitleType TitleType
		{
			get
			{
				return TitleType.Text;
			}
		}

		public string Text
		{
			get
			{
				return this.text;
			}
		}

		public override Title Clone()
		{
			return new TextTitle(this.Text);
		}

		readonly string text;
	}
}
