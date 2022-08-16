using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Drawing
{
	class HorizontalAlignmentElement : DrawingElementBase
	{
		public HorizontalAlignmentElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "align";
			}
		}
	}
}
