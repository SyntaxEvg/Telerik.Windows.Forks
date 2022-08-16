using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class HorizontalMergeElement : MergeElementBase
	{
		public HorizontalMergeElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "hMerge";
			}
		}
	}
}
