using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.ParagraphPropertiesElements
{
	class ApplyEastAsianLineBreakingRulesElement : BoolValueElementBase
	{
		public ApplyEastAsianLineBreakingRulesElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "kinsoku";
			}
		}
	}
}
