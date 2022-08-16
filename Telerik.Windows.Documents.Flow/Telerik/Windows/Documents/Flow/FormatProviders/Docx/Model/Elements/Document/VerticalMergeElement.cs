using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class VerticalMergeElement : MergeElementBase
	{
		public VerticalMergeElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "vMerge";
			}
		}
	}
}
