using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.StylePropertiesElements
{
	class NextStyleIdElement : DocxRequiredValueElementBase<string>
	{
		public NextStyleIdElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "next";
			}
		}
	}
}
