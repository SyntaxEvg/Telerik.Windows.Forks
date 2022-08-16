using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.TableRowPropertiesElements
{
	class RepeatOnEveryPageElement : BoolValueElementBase
	{
		public RepeatOnEveryPageElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "tblHeader";
			}
		}
	}
}
