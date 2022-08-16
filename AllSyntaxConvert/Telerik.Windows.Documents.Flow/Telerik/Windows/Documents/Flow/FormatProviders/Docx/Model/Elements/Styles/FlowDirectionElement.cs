using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles
{
	class FlowDirectionElement : BoolValueElementBase
	{
		public FlowDirectionElement(DocxPartsManager partsManager, string elementName)
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

		public override bool AlwaysExport
		{
			get
			{
				return false;
			}
		}

		readonly string elementName;
	}
}
