using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common
{
	abstract class DecimalNumberBase : DocxRequiredValueElementBase<int>
	{
		public DecimalNumberBase(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}
	}
}
