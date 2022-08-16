using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.StylePropertiesElements
{
	class StyleNameElement : DocxRequiredValueElementBase<string>
	{
		public StyleNameElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "name";
			}
		}
	}
}
