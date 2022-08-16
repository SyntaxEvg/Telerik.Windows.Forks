using System;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Lists
{
	public class ParagraphProperties : PropertiesBase<ParagraphProperties>
	{
		public double FirstLineIndent { get; set; }

		public double LeftIndent { get; set; }

		public override void CopyFrom(ParagraphProperties other)
		{
			this.FirstLineIndent = other.FirstLineIndent;
			this.LeftIndent = other.LeftIndent;
		}
	}
}
