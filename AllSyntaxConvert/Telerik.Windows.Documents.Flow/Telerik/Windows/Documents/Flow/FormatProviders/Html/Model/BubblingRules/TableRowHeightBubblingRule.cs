using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.BubblingRules
{
	class TableRowHeightBubblingRule : HtmlBubblingPropertyRule
	{
		public TableRowHeightBubblingRule(IStylePropertyDefinition propertyDefinition)
			: base(propertyDefinition)
		{
		}

		public override bool ShouldRegisterProperty(DocumentElementPropertiesBase documentElementProperties)
		{
			TableCellProperties tableCellProperties = documentElementProperties as TableCellProperties;
			if (tableCellProperties != null)
			{
				return tableCellProperties.RowSpan.GetActualValue().Value == 1;
			}
			return documentElementProperties is TableRowProperties;
		}

		protected override bool IsRuleSatisfied(object value, object candidateValue, out object result)
		{
			Guard.ThrowExceptionIfNull<object>(value, "value");
			Guard.ThrowExceptionIfNull<object>(candidateValue, "candidateValue");
			TableRowHeight tableRowHeight = (TableRowHeight)value;
			TableRowHeight tableRowHeight2 = (TableRowHeight)candidateValue;
			if (tableRowHeight2.Value > tableRowHeight.Value)
			{
				result = candidateValue;
				return true;
			}
			result = null;
			return false;
		}
	}
}
