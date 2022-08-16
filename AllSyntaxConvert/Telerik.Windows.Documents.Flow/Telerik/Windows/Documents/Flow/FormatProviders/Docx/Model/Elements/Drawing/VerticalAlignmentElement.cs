using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Drawing
{
	class VerticalAlignmentElement : DrawingElementBase
	{
		public VerticalAlignmentElement(DocxPartsManager partsManager)
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
