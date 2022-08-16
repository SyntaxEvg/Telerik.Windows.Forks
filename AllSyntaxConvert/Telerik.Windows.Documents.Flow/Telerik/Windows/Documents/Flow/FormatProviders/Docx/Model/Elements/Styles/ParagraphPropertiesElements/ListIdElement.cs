using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.ParagraphPropertiesElements
{
	class ListIdElement : DecimalNumberBase
	{
		public ListIdElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "numId";
			}
		}
	}
}
