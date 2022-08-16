using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Drawing
{
	class WrapSquareElement : WrapTextElementBase
	{
		public WrapSquareElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "wrapSquare";
			}
		}
	}
}
