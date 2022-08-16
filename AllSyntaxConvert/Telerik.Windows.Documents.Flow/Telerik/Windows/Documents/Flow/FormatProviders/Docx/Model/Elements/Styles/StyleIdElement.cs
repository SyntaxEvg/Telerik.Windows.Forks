using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles
{
	class StyleIdElement : DocxRequiredValueElementBase<string>
	{
		public StyleIdElement(DocxPartsManager partsManager, string elementName)
			: base(partsManager)
		{
			this.elementName = elementName;
		}

		public override string ElementName
		{
			get
			{
				return this.elementName;
			}
		}

		readonly string elementName;
	}
}
