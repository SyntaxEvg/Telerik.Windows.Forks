using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists
{
	class StartIndexOverrideElement : DecimalNumberBase
	{
		public StartIndexOverrideElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "startOverride";
			}
		}
	}
}
