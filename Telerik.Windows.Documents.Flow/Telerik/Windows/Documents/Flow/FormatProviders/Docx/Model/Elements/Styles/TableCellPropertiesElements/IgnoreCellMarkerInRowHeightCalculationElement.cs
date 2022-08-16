using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.TableCellPropertiesElements
{
	class IgnoreCellMarkerInRowHeightCalculationElement : BoolValueElementBase
	{
		public IgnoreCellMarkerInRowHeightCalculationElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "hideMark";
			}
		}
	}
}
