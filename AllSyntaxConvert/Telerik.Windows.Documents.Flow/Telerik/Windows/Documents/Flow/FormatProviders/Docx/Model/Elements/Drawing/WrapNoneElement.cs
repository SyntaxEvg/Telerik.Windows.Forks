using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Drawing
{
	class WrapNoneElement : DrawingElementBase
	{
		public WrapNoneElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "wrapNone";
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
