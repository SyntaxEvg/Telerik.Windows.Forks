using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Drawing
{
	class WrapTopAndBottomElement : DrawingElementBase
	{
		public WrapTopAndBottomElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "wrapTopAndBottom";
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}
	}
}
