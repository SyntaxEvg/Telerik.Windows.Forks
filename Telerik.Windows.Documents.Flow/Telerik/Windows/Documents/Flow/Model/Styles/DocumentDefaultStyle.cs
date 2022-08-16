using System;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public sealed class DocumentDefaultStyle
	{
		internal DocumentDefaultStyle(RadFlowDocument document)
		{
			this.CharacterProperties = new CharacterProperties(document, true);
			this.ParagraphProperties = new ParagraphProperties(document, true);
		}

		public CharacterProperties CharacterProperties { get; set; }

		public ParagraphProperties ParagraphProperties { get; set; }
	}
}
