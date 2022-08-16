using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.TableRowPropertiesElements
{
	class CanSplitElement : BoolValueElementBase
	{
		public CanSplitElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "cantSplit";
			}
		}
	}
}
