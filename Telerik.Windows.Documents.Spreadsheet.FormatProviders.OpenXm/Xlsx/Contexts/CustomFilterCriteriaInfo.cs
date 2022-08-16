using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class CustomFilterCriteriaInfo
	{
		public CustomFilterCriteriaInfo(CustomFilterCriteria criteria)
		{
			this.FilterValue = criteria.FilterValue;
			this.FilterOperator = criteria.ComparisonOperator;
		}

		public string FilterValue { get; set; }

		public ComparisonOperator FilterOperator { get; set; }

		public CustomFilterCriteria ToCriteria()
		{
			return new CustomFilterCriteria(this.FilterOperator, this.FilterValue);
		}
	}
}
