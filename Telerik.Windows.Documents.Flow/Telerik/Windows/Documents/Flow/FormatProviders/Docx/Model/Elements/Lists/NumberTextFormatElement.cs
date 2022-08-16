using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists
{
	class NumberTextFormatElement : DocxRequiredValueElementBase<string>
	{
		public NumberTextFormatElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "lvlText";
			}
		}
	}
}
