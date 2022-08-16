using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Settings
{
	class EvenAndOddHeadersFootersElement : BoolValueElementBase
	{
		public EvenAndOddHeadersFootersElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "evenAndOddHeaders";
			}
		}
	}
}
